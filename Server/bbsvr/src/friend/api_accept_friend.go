package friend

import (
	//"fmt"
	"log"
	"net/http"
	//"time"
)

import (
	"bbproto"
	"common"
	"common/Error"
	"common/consts"
	"common/EC"
	"data"
	//"model/user"
	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func AcceptFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqAcceptFriend
	rspMsg := &bbproto.RspAcceptFriend{}

	handler := &AcceptFriendProtocol{}
	err := handler.ParseInput(req, &reqMsg)
	if err.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
		return
	}

	err = handler.verifyParams(&reqMsg)
	if err.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
		return
	}

	// game logic

	err = handler.ProcessLogic(&reqMsg, rspMsg)

	err = handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", err, rspMsg)
}

/////////////////////////////////////////////////////////////////////////////

type AcceptFriendProtocol struct {
	bbproto.BaseProtoHandler
}

func (t AcceptFriendProtocol) FillResponseMsg(reqMsg *bbproto.ReqAcceptFriend, rspMsg *bbproto.RspAcceptFriend, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header.Error = proto.String(rspErr.Error())
	}

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		log.Printf("[ERROR] proto.Marshal error: %v", err)
		return nil
	}

	return outbuffer
}

func (t AcceptFriendProtocol) verifyParams(reqMsg *bbproto.ReqAcceptFriend) (e Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.FriendUid == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.FriendUid == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t AcceptFriendProtocol) ProcessLogic(reqMsg *bbproto.ReqAcceptFriend, rspMsg *bbproto.RspAcceptFriend) (e Error.Error) {
	db := &data.Data{}
	err := db.Open(consts.TABLE_FRIEND)
	defer db.Close()
	if err != nil {
		return Error.New(EC.CONNECT_DB_ERROR, err.Error())
	}

	uid := *reqMsg.Header.UserId
	fUid := *reqMsg.FriendUid

	e = AddFriend(db, uid, fUid, bbproto.EFriendState_ISFRIEND, common.Now())
	if e.IsError() {
		log.Printf("[ERROR] user:%v AcceptFriend(%v) failed: %v", uid, fUid, e.Error())
		return e
	}

	log.Printf("[TRACE] user:%v AcceptFriend(%v) ok.", uid, fUid)

	return Error.OK()
}
