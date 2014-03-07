package unit

import (
	"net/http"
	//"time"
)

import (
	"bbproto"
	"common/EC"
	"common/Error"
	"common/log"
	"data"
	"user/usermanage"

	"code.google.com/p/goprotobuf/proto"
	"common/consts"
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
	if reqMsg.NewNickName == nil || reqMsg.Header.UserId == nil {
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

	//	e = usermanage.RenameUser(*reqMsg.Header.UserId, *reqMsg.NewNickName)
	//	if e.IsError() {
	//		return e
	//	}
	uid := *reqMsg.Header.UserId

	userDetail, exists, err := usermanage.GetUserInfo(db, uid)
	if err != nil || !exists {
		log.Error("getUserInfo(%v) failed.", uid)
		return Error.New(EC.EU_GET_USERINFO_FAIL, err)
	}

	e = unit.DoLevelUp(db, &userDetail, *reqMsg.BaseUniqueId, reqMsg.PartUniqueId, *reqMsg.HelperUserId, *reqMsg.HelperUnit)

	return e
}
