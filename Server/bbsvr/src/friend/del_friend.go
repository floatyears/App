package friend

import (
	"fmt"
	"log"
	"net/http"
	//"time"
)

import (
	"../bbproto"
	"../common/Error"
	"../const"
	"../data"
	//"../user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
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

func (t DelFriendProtocol) verifyParams(reqMsg *bbproto.ReqDelFriend) (e Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.FriendUid == nil {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.FriendUid == 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t DelFriendProtocol) FillResponseMsg(reqMsg *bbproto.ReqDelFriend, rspMsg *bbproto.RspDelFriend, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header.Error = proto.String(rspErr.Error())

		//log.Printf("req sessionId:%v reqMsg.Header:%v", *reqMsg.Header.SessionId, reqMsg.Header)
	}

	// fill custom protocol body

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}

	return outbuffer
}

func (t DelFriendProtocol) ProcessLogic(reqMsg *bbproto.ReqDelFriend, rspMsg *bbproto.RspDelFriend) (e Error.Error) {

	//uid := *reqMsg.Header.UserId
	fUid := *reqMsg.FriendUid

	db := &data.Data{}
	err := db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil {
		return Error.New(cs.CONNECT_DB_ERROR, err.Error())
	}

	log.Printf("%v %v %v", db, fUid, fmt.Sprintf(""))

	return Error.OK()
}
