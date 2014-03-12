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
	"common"
)

/////////////////////////////////////////////////////////////////////////////

func EvolveDoneHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqEvolveDone
	rspMsg := &bbproto.RspEvolveDone{}

	handler := &EvolveDone{}
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

type EvolveDone struct {
	bbproto.BaseProtoHandler
}

func (t EvolveDone) verifyParams(reqMsg *bbproto.ReqEvolveDone) (err Error.Error) {
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

func (t EvolveDone) FillResponseMsg(reqMsg *bbproto.ReqEvolveDone, rspMsg *bbproto.RspEvolveDone, rspErr Error.Error) (outbuffer []byte) {
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

func (t EvolveDone) ProcessLogic(reqMsg *bbproto.ReqEvolveDone, rspMsg *bbproto.RspEvolveDone) (e Error.Error) {
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

	//2.
	reqEvolveStart, e:= CheckEvolveSession(db, uid)

	//3. getUnitInfo of baseUniqueId
	baseUserUnit, e := unit.GetUserUnitInfo(&userDetail, *reqEvolveStart.BaseUniqueId)
	if e.IsError() {
		log.Error("GetUserUnitInfo(%v) failed: %v", *reqEvolveStart.BaseUniqueId, e.Error())
		return e
	}
	baseUnit, e := unit.GetUnitInfo(db, *baseUserUnit.UnitId)
	if e.IsError() {
		log.Error("GetUnitInfo(%v) failed: %v", *baseUserUnit.UnitId, e.Error())
		return e
	}
	log.T("baseUserUnit:(%+v).", baseUserUnit)
	log.T("baseUnit:(%+v).", baseUnit)

	//4. malloc new evolved unit
	newUniqueId, e := unit.GetUnitUniqueId(db, uid, len(userDetail.UnitList) )
	rspMsg.EvolvedUnit = &bbproto.UserUnit{}
	rspMsg.EvolvedUnit.UniqueId = proto.Uint32(newUniqueId)
	rspMsg.EvolvedUnit.UnitId = baseUnit.EvolveInfo.EvolveUnitId
	rspMsg.EvolvedUnit.AddAttack = baseUserUnit.AddAttack
	rspMsg.EvolvedUnit.AddHp = baseUserUnit.AddHp
	rspMsg.EvolvedUnit.AddDefence = baseUserUnit.AddDefence
	rspMsg.EvolvedUnit.IsFavorite = baseUserUnit.IsFavorite
	rspMsg.EvolvedUnit.Exp	= proto.Int32(0)
	rspMsg.EvolvedUnit.Level = proto.Int32(1)
	rspMsg.EvolvedUnit.GetTime = proto.Uint32(common.Now())


	//5. remove BaseUnit & partUnits
	reqEvolveStart.PartUniqueId = append(reqEvolveStart.PartUniqueId, *reqEvolveStart.BaseUniqueId)
	if e = unit.RemoveMyUnit(userDetail.UnitList, reqEvolveStart.PartUniqueId);e.IsError() {
		return e
	}


	//8. update userinfo
	if e = user.UpdateUserInfo(db, &userDetail); e.IsError() {
		return e
	}

	//9. fill response
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
