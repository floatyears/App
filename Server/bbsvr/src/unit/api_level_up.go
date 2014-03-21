package unit

import (
	"net/http"
	//"time"
)

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common/EC"
	"common/Error"
	//"common/consts"
	"common/log"
	"data"
	"model/unit"
	"model/user"
	"model/friend"
)

/////////////////////////////////////////////////////////////////////////////

func LevelUpHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqLevelUp
	rspMsg := &bbproto.RspLevelUp{}

	handler := &LevelUp{}
	e := handler.ParseInput(req, &reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
		return
	}

	e = handler.verifyParams(&reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
		return
	}

	// game logic

	e = handler.ProcessLogic(&reqMsg, rspMsg)

	e = handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
	log.Printf("sendrsp err:%v, rspMsg.Header: %+v", e, rspMsg.Header)
}

/////////////////////////////////////////////////////////////////////////////

type LevelUp struct {
	bbproto.BaseProtoHandler
}

func (t LevelUp) verifyParams(reqMsg *bbproto.ReqLevelUp) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.BaseUniqueId == nil || reqMsg.PartUniqueId == nil || reqMsg.HelperUserId == nil ||
		reqMsg.HelperUnit == nil || reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	materialCount := len(reqMsg.PartUniqueId)
	if materialCount == 0 || materialCount > 4 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params PartUniqueId's length invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t LevelUp) FillResponseMsg(reqMsg *bbproto.ReqLevelUp, rspMsg *bbproto.RspLevelUp, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header.Error = proto.String(rspErr.Error())
	}

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func (t LevelUp) ProcessLogic(reqMsg *bbproto.ReqLevelUp, rspMsg *bbproto.RspLevelUp) (e Error.Error) {
	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
		return Error.New(EC.CONNECT_DB_ERROR, err)
	}
	uid := *reqMsg.Header.UserId

	//1. get userinfo
	userDetail, exists, err := user.GetUserInfo(db, uid)
	if err != nil || !exists {
		log.Error("getUserInfo(%v) failed.", uid)
		return Error.New(EC.EU_GET_USERINFO_FAIL, err)
	}

	//2. getUnitInfo of baseUniqueId
	baseUserUnit, e := unit.GetUserUnitInfo(userDetail, *reqMsg.BaseUniqueId)
	if e.IsError() {
		log.Error("GetUserUnitInfo(%v) failed: %v", *reqMsg.BaseUniqueId, e.Error())
		return e
	}
	baseUnit, e := unit.GetUnitInfo(db, *baseUserUnit.UnitId)
	if e.IsError() {
		log.Error("GetUnitInfo(%v) failed: %v", *baseUserUnit.UnitId, e.Error())
		return e
	}
	log.Error("baseUserUnit:(%+v).", baseUserUnit)
	log.Error("baseUnit:(%+v).", baseUnit)

	//3. check acount.money is enough or not
	needMoney := unit.GetLevelUpMoney(*baseUserUnit.Level, int32(len(reqMsg.PartUniqueId)))
	if *userDetail.Account.Money < needMoney {
		log.Error("no enough money: %v < %v", *userDetail.Account.Money, needMoney)
		return Error.New(EC.E_NO_ENOUGH_MONEY)
	}

	//4. getUnitInfo of all material part to caculate exp
	addExp, addAtk, addHp, addDef, e := unit.CalculateDevourExp(db, userDetail, &baseUnit, reqMsg.PartUniqueId)
	log.T("OrigExp:%v addExp:%v (addAtk:%v addHp:%v addDef:%v)", *baseUserUnit.Exp, addExp, addAtk, addHp, addDef)
	if e.IsError() {
		return e
	}

	//5. calculate Level growup
	addLevel, e := unit.CalcLevelUpAddLevel(baseUserUnit, &baseUnit, *baseUserUnit.Exp, addExp)
	if e.IsError() {
		return e
	}
	log.T("Calc ret baseUserUnit.Level(%v) + addLevel(%v)", *baseUserUnit.Level, addLevel)

	*baseUserUnit.Exp += addExp
	*baseUserUnit.Level += addLevel
	if baseUserUnit.AddAttack != nil {
		*baseUserUnit.AddAttack += addAtk
	}
	if baseUserUnit.AddHp != nil {
		*baseUserUnit.AddHp += addHp
	}
	if baseUserUnit.AddDefence != nil {
		*baseUserUnit.AddDefence += addDef
	}
	log.T("baseUserUnit ref to userDetail.UnitList[x] => after Assign value: %+v", baseUserUnit)
//	log.T("userDetail.UnitList[x] => NOW value: %+v", userDetail.UnitList)

	//6. remove partUnits
	log.T("------ before RemoveMyUnit userDetail.UnitList len:%v", len(userDetail.UnitList))
	e = unit.RemoveMyUnit(&userDetail.UnitList, reqMsg.PartUniqueId)
	if e.IsError() {
		return e
	}
	log.T("------ after RemoveMyUnit userDetail.UnitList len:%v", len(userDetail.UnitList))

	//7. deduct user money
	log.T("deduct money: %v - %v ", *userDetail.Account.Money, needMoney)
	*userDetail.Account.Money -= needMoney

	//8. update helper used time
	if e = friend.UpdateHelperUsedRecord(db, uid, *reqMsg.HelperUserId); e.IsError() {
		return e
	}

	//9. update userinfo
	if e = user.UpdateUserInfo(db, userDetail); e.IsError() {
		return e
	}

	//10. fill response
	rspMsg.BlendExp = proto.Int32(addExp)
	rspMsg.BlendUniqueId = reqMsg.BaseUniqueId
	rspMsg.UnitList = userDetail.UnitList
	rspMsg.PartUniqueId = reqMsg.PartUniqueId

	log.T("=================rspMsg begin==================")
	log.T("\t BlendExp:%v", *rspMsg.BlendExp)
	log.T("\t BlendUniqueId:%v", *rspMsg.BlendUniqueId)
	log.T("\t PartUniqueId:%v", rspMsg.PartUniqueId)

	for k, unit := range rspMsg.UnitList {
		log.T("\t [%v] unit: %+v", k, unit)
	}
	log.T(">>>>>>>>>>>>>>>>>>>Rsp End<<<<<<<<<<<<<<<<<<<<")

	return e
}
