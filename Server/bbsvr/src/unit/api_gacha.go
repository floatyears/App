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
	"common"
	"common/log"
	"data"
	"model/unit"
	"model/user"
)

/////////////////////////////////////////////////////////////////////////////

func GachaHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqGacha
	rspMsg := &bbproto.RspGacha{}

	handler := &Gacha{}
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

type Gacha struct {
	bbproto.BaseProtoHandler
}

func (t Gacha) verifyParams(reqMsg *bbproto.ReqGacha) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.GachaId == nil || reqMsg.GachaCount == nil || reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.GachaId <= 0 || *reqMsg.GachaCount <= 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t Gacha) FillResponseMsg(reqMsg *bbproto.ReqGacha, rspMsg *bbproto.RspGacha, rspErr Error.Error) (outbuffer []byte) {
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

func (t Gacha) ProcessLogic(reqMsg *bbproto.ReqGacha, rspMsg *bbproto.RspGacha) (e Error.Error) {
	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
		return Error.New(EC.CONNECT_DB_ERROR, err)
	}

	uid := *reqMsg.Header.UserId
	gachaId := *reqMsg.GachaId
	gachaCount := *reqMsg.GachaCount

	//1. get userinfo
	userDetail, exists, err := user.GetUserInfo(db, uid)
	if err != nil || !exists {
		log.Error("getUserInfo(%v) failed.", uid)
		return Error.New(EC.EU_GET_USERINFO_FAIL, err)
	}

	//2. get Gacha Config
	gachaConf, e := unit.GetGachaConfig(db, gachaId)
	if e.IsError() {
		return e
	}

	//3. check & update userDetail.Acount
	if e = unit.UpdateAcountForGacha(&userDetail, *gachaConf.GachaType, gachaCount); e.IsError() {
		return e
	}

	//4. check the gacha is availble now
	if e := unit.CheckGachaAvailble(gachaConf, gachaId); e.IsError() {
		log.Error("CheckGachaAvailble(%v) failed: %v", gachaId, e.Error())
		return e
	}

	//5. get random unit from target pool
	gotUnitIds, e := unit.GetGachaUnit(db, gachaId, gachaCount)
	if e.IsError() {
		return e
	}

	//6. add gacha unit
	for _, unitId := range gotUnitIds {
		uniqueId, e := unit.GetUnitUniqueId(db, uid, len(userDetail.UnitList))
		if e.IsError() {
			return e
		}
		userUnit := &bbproto.UserUnit{}
		userUnit.UniqueId = proto.Uint32(uniqueId)
		userUnit.UnitId = proto.Uint32(unitId)
		userUnit.AddAttack = proto.Int32(0)
		userUnit.AddHp = proto.Int32(0)
		userUnit.AddDefence = proto.Int32(0)
		userUnit.Exp = proto.Int32(0)
		userUnit.Level = proto.Int32(1)
		userUnit.GetTime = proto.Uint32(common.Now())

		rspMsg.UnitUniqueId = append(rspMsg.UnitUniqueId, uniqueId)
		userDetail.UnitList = append(userDetail.UnitList, userUnit)
	}

	//8. update userinfo
	if e = user.UpdateUserInfo(db, &userDetail); e.IsError() {
		return e
	}

	//9. fill response
	rspMsg.UnitList = userDetail.UnitList

	log.T("=================rspMsg begin==================")
	log.T("\t unitUniqueId: %+v", rspMsg.UnitUniqueId)

	for k, unit := range rspMsg.UnitList {
		log.T("\t unit[%v]: %+v", k, unit)
	}
	log.T(">>>>>>>>>>>>>>>>>>>Rsp End<<<<<<<<<<<<<<<<<<<<")

	return e
}
