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

func AcceptFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqAcceptFriend
	rspMsg := &bbproto.RspAcceptFriend{}

	handler := &AcceptFriend{}
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

type AcceptFriend struct {
	bbproto.BaseProtoHandler
}

func (t AcceptFriend) verifyParams(reqMsg *bbproto.ReqAcceptFriend) (e Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.FriendUid == nil {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.FriendUid == 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t AcceptFriend) FillResponseMsg(reqMsg *bbproto.ReqAcceptFriend, rspMsg *bbproto.RspAcceptFriend, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header.Error = proto.String(rspErr.Error())
	}

	// fill custom protocol body

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func (t AcceptFriend) ProcessLogic(reqMsg *bbproto.ReqAcceptFriend, rspMsg *bbproto.RspAcceptFriend) (e Error.Error) {

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
