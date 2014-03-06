package friend

import (
	//"fmt"
	"log"
	"net/http"
	//"time"
)

import (
	"bbproto"
	"common/EC"
	"common/Error"
	"common/consts"
	"data"
	//"user/usermanage"
	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func DelFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqDelFriend
	rspMsg := &bbproto.RspDelFriend{}

	handler := &DelFriendProtocol{}
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

type DelFriendProtocol struct {
	bbproto.BaseProtoHandler
}

func (t DelFriendProtocol) FillResponseMsg(reqMsg *bbproto.ReqDelFriend, rspMsg *bbproto.RspDelFriend, rspErr Error.Error) (outbuffer []byte) {
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

func (t DelFriendProtocol) verifyParams(reqMsg *bbproto.ReqDelFriend) (e Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.FriendUid == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.FriendUid == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t DelFriendProtocol) ProcessLogic(reqMsg *bbproto.ReqDelFriend, rspMsg *bbproto.RspDelFriend) (e Error.Error) {
	db := &data.Data{}
	err := db.Open(consts.TABLE_FRIEND)
	defer db.Close()
	if err != nil {
		return Error.New(EC.CONNECT_DB_ERROR, err.Error())
	}

	uid := *reqMsg.Header.UserId
	fUid := *reqMsg.FriendUid

	num, err := DelFriend(db, uid, fUid)
	if err != nil {
		log.Printf("[ERROR] user:%v DelFriend(%v) failed: %v", uid, fUid, err)
		return Error.New(EC.EF_DEL_FRIEND_FAIL, err.Error())
	}

	log.Printf("[TRACE] user:%v DelFriend(%v) ok (del %v item).", uid, fUid, num)

	return Error.OK()
}
