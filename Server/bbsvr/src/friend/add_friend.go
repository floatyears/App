package friend

import (
	//"fmt"
	"log"
	"net/http"
	//"time"
)

import (
	"../bbproto"
	"../common"
	"../common/Error"
	"../const"
	"../data"
	//"../user/usermanage"
	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func AddFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqAddFriend
	rspMsg := &bbproto.RspAddFriend{}

	handler := &AddFriendProtocol{}
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

type AddFriendProtocol struct {
	bbproto.BaseProtoHandler
}

func (t AddFriendProtocol) FillResponseMsg(reqMsg *bbproto.ReqAddFriend, rspMsg *bbproto.RspAddFriend, rspErr Error.Error) (outbuffer []byte) {
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

func (t AddFriendProtocol) verifyParams(reqMsg *bbproto.ReqAddFriend) (e Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.FriendUid == nil {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.FriendUid == 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t AddFriendProtocol) ProcessLogic(reqMsg *bbproto.ReqAddFriend, rspMsg *bbproto.RspAddFriend) (e Error.Error) {
	db := &data.Data{}
	err := db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil {
		return Error.New(cs.CONNECT_DB_ERROR, err.Error())
	}

	uid := *reqMsg.Header.UserId
	fUid := *reqMsg.FriendUid

	e = AddFriend(db, uid, fUid, bbproto.EFriendState_FRIENDOUT, common.Now())
	if e.IsError() {
		log.Printf("[ERROR] user:%v AddFriend(%v) failed: %v", uid, fUid, e.Error())
		return e
	}

	log.Printf("[TRACE] user:%v AddFriend(%v) ok.", uid, fUid)

	return Error.OK()
}
