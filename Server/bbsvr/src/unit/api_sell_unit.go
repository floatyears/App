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
	"common/log"
	"data"
	"model/unit"
	"model/user"
)

/////////////////////////////////////////////////////////////////////////////

func SellUnitHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqSellUnit
	rspMsg := &bbproto.RspSellUnit{}

	handler := &SellUnit{}
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

type SellUnit struct {
	bbproto.BaseProtoHandler
}

func (t SellUnit) verifyParams(reqMsg *bbproto.ReqSellUnit) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.UnitUniqueId == nil || len(reqMsg.UnitUniqueId) == 0 || reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t SellUnit) FillResponseMsg(reqMsg *bbproto.ReqSellUnit, rspMsg *bbproto.RspSellUnit, rspErr Error.Error) (outbuffer []byte) {
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

func (t SellUnit) ProcessLogic(reqMsg *bbproto.ReqSellUnit, rspMsg *bbproto.RspSellUnit) (e Error.Error) {
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
	gotMoney := int32(0)
	for k, uniqueId := range reqMsg.UnitUniqueId {
		baseUserUnit, e := unit.GetUserUnitInfo(userDetail, uniqueId)
		if e.IsError() {
			log.Error("user:%v GetUserUnitInfo(%v) failed: %v", uid, uniqueId, e.Error())
			return e
		}
		baseUnit, e := unit.GetUnitInfo(db, *baseUserUnit.UnitId)
		if e.IsError() {
			log.Error("user:%v GetUnitInfo(%v) failed: %v", uid, *baseUserUnit.UnitId, e.Error())
			return e
		}

		log.T("[%v] baseUserUnit:(%+v).", k, baseUserUnit)
		log.T("[%v] baseUnit[%v]: (%+v).", k, *baseUnit.Id, baseUnit)
		log.T("    baseUnit:(%v) sellValue:%v.", *baseUnit.Id, *baseUnit.SaleValue)
		gotMoney += *baseUnit.SaleValue

	}

	log.T("total sell Money: %v.", gotMoney)

	//3. remove selled uniqueIds from user's UnitList
	e = unit.RemoveMyUnit(&userDetail.UnitList, reqMsg.UnitUniqueId)
	if e.IsError() {
		return e
	}

	//4. update account
	*userDetail.Account.Money += gotMoney

	//5. update userinfo
	if e = user.UpdateUserInfo(db, userDetail); e.IsError() {
		return e
	}

	//6. fill response
	rspMsg.GotMoney = proto.Int32(gotMoney)
	rspMsg.Money = userDetail.Account.Money
	rspMsg.UnitList = userDetail.UnitList

	log.T("=================rspMsg begin==================")
	log.T("\t GotMoney: %v", *rspMsg.GotMoney)
	log.T("\t Money: %v", *rspMsg.Money)
	for k, unit := range rspMsg.UnitList {
		log.T("\t unit[%v]: %+v", k, unit)
	}
	log.T(">>>>>>>>>>>>>>>>>>>Rsp End<<<<<<<<<<<<<<<<<<<<")

	return e
}
