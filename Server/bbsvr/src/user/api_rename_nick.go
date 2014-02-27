package user

import (
	"net/http"
	//"time"
)

import (
	"../bbproto"
	"../common/Error"
	"../common/log"
	"../const"
	"./usermanage"

	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func RenameNickHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqRenameNick
	rspMsg := &bbproto.RspRenameNick{}

	handler := &RenameNick{}
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

type RenameNick struct {
	bbproto.BaseProtoHandler
}

func (t RenameNick) verifyParams(reqMsg *bbproto.ReqRenameNick) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.NewNickName == nil || reqMsg.Header.UserId == nil {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t RenameNick) FillResponseMsg(reqMsg *bbproto.ReqRenameNick, rspMsg *bbproto.RspRenameNick, rspErr Error.Error) (outbuffer []byte) {
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

func (t RenameNick) ProcessLogic(reqMsg *bbproto.ReqRenameNick, rspMsg *bbproto.RspRenameNick) (e Error.Error) {
	log.T("Rename ...")
	return usermanage.RenameUser(*reqMsg.Header.UserId, *reqMsg.NewNickName)
}
