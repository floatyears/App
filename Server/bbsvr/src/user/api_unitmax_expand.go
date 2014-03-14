package user

import (
	"net/http"
	//"time"
)

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/log"
	"data"
	//"model/unit"
	"common/consts"
	"model/user"
)

/////////////////////////////////////////////////////////////////////////////

func UnitMaxExpandHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqUnitMaxExpand
	rspMsg := &bbproto.RspUnitMaxExpand{}

	handler := &UnitMaxExpand{}
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

type UnitMaxExpand struct {
	bbproto.BaseProtoHandler
}

func (t UnitMaxExpand) verifyParams(reqMsg *bbproto.ReqUnitMaxExpand) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t UnitMaxExpand) FillResponseMsg(reqMsg *bbproto.ReqUnitMaxExpand, rspMsg *bbproto.RspUnitMaxExpand, rspErr Error.Error) (outbuffer []byte) {
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

func (t UnitMaxExpand) ProcessLogic(reqMsg *bbproto.ReqUnitMaxExpand, rspMsg *bbproto.RspUnitMaxExpand) (e Error.Error) {
	timeCost := &common.Cost{}
	timeCost.Begin()

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

	//2. check account balance
	if *userDetail.Account.Stone < consts.N_UNITMAX_EXPAND_COST {
		rspMsg.Stone = userDetail.Account.Stone
		rspMsg.UnitMax = userDetail.User.UnitMax
		return Error.New(EC.E_NO_ENOUGH_MONEY, "stone is not enough.")
	}

	//3. update account
	log.T("before update: stone=%v FriendMax=%v", *userDetail.Account.Stone, *userDetail.User.UnitMax)
	*userDetail.Account.Stone -= consts.N_UNITMAX_EXPAND_COST
	*userDetail.User.UnitMax += consts.N_UNITMAX_EXPAND_COUNT
	log.T("before update: stone=%v UnitMax=%v", *userDetail.Account.Stone, *userDetail.User.UnitMax)

	//4. update userinfo
	if e = user.UpdateUserInfo(db, &userDetail); e.IsError() {
		return e
	}

	//5. fill response
	rspMsg.Stone = userDetail.Account.Stone
	rspMsg.UnitMax = userDetail.User.UnitMax

	log.T("=========== response total cost %v ms. ============\n\n", timeCost.Cost())
	log.T("\t rspMsg: %+v", rspMsg)
	log.T(">>>>>>>>>>>>>>>>>>>Rsp End<<<<<<<<<<<<<<<<<<<<")

	return e
}
