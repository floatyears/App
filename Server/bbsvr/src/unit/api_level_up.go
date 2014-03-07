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
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", e, rspMsg)
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
	baseUserUnit, e := unit.GetUserUnitInfo(&userDetail, *reqMsg.BaseUniqueId)
	baseUnit, e := unit.GetUnitInfo(db, *baseUserUnit.UnitId)

	//3. check acount.money is enough or not
	needMoney := unit.GetLevelUpMoney(*baseUserUnit.Level, int32(len(reqMsg.PartUniqueId)))
	if *userDetail.Account.Money < needMoney {
		log.Error("no enough money: %v < %v", *userDetail.Account.Money, needMoney)
		return Error.New(EC.E_LEVELUP_NO_ENOUGH_MONEY)
	}

	//4. getUnitInfo of all material part, caculate exp
	for _, partUniqueId := range reqMsg.PartUniqueId {
		partUU, e := unit.GetUserUnitInfo(&userDetail, partUniqueId)
		if e.IsError() {
			return e
		}

		partUnit, e := unit.GetUnitInfo(db, *partUU.UnitId)

		multiple := float32(1.0)
		if *baseUnit.Race == *partUnit.Race && *baseUnit.Type == *partUnit.Type {
			multiple = float32(1.5)
		} else if *baseUnit.Race == *partUnit.Race || *baseUnit.Type == *partUnit.Type {
			multiple = float32(1.25)
		}

		*baseUserUnit.Exp += int32(float32(*partUnit.DevourValue) * float32(*partUU.Level) * multiple)
		log.T("Add partUnit:[%v | %v] DevourExp = (%v * %v) => %v", partUU.UniqueId, partUU.UnitId,
			(*partUnit.DevourValue)*(*partUU.Level), multiple, int32(float32(*partUnit.DevourValue)*float32(*partUU.Level)*multiple))
	}

	//4. remove partUnits
	e = unit.RemoveMyUnit( userDetail.UnitList, reqMsg.PartUniqueId )
	if e.IsError() {
		return e
	}

	//5. deduct user money
	*userDetail.Account.Money -= needMoney;
	log.T("after deduct money is: %v", *userDetail.Account.Money)

	//6. update userinfo
	if e = user.UpdateUserInfo(db, &userDetail); e.IsError() {
		return e
	}

	return e
}
