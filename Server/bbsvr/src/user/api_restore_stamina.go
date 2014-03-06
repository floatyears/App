package user

import (
	"net/http"
	//"time"
)

import (
	"bbproto"
	"common/EC"
	"common/Error"
	"common/log"
	"user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func RestoreStaminaHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqRestoreStamina
	rspMsg := &bbproto.RspRestoreStamina{}

	handler := &RestoreStamina{}
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

type RestoreStamina struct {
	bbproto.BaseProtoHandler
}

func (t RestoreStamina) verifyParams(reqMsg *bbproto.ReqRestoreStamina) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t RestoreStamina) FillResponseMsg(reqMsg *bbproto.ReqRestoreStamina, rspMsg *bbproto.RspRestoreStamina, rspErr Error.Error) (outbuffer []byte) {
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

func (t RestoreStamina) ProcessLogic(reqMsg *bbproto.ReqRestoreStamina, rspMsg *bbproto.RspRestoreStamina) (e Error.Error) {
	log.T("RestoreStamina ...")

	userDetail, e := usermanage.RetoreStamina(nil, *reqMsg.Header.UserId)
	if e.IsError() {
		return e
	}

	rspMsg.StaminaNow = userDetail.User.StaminaNow
	rspMsg.Account = userDetail.Account

	return e
}
