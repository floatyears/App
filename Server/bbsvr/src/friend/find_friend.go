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
	//"../data"
	"../user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func FindFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqFindFriend
	rspMsg := &bbproto.RspFindFriend{}

	handler := &FindFriend{}
	e := handler.ParseInput(req, &reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, Error.New(cs.INVALID_PARAMS, e.Error())))
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

type FindFriend struct {
	bbproto.BaseProtoHandler
}

func (t FindFriend) verifyParams(reqMsg *bbproto.ReqFindFriend) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t FindFriend) FillResponseMsg(reqMsg *bbproto.ReqFindFriend, rspMsg *bbproto.RspFindFriend, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		//fill error code
		rspMsg.Header = reqMsg.Header //including the sessionId
		rspMsg.Header.Code = proto.Int(rspErr.Code())

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

func (t FindFriend) ProcessLogic(reqMsg *bbproto.ReqFindFriend, rspMsg *bbproto.RspFindFriend) (Err Error.Error) {

	uid := *reqMsg.Header.UserId

	//get user's rank from user table
	userdetail, isUserExists, err := usermanage.GetUserInfo(uid)
	if err != nil {
		return Error.New(cs.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed. userId %v", uid))
	}
	log.Printf("[TRACE] getUser(%v) ret userinfo: %v", uid, userdetail.User)

	// get FriendInfo
	if isUserExists {
		rspMsg.Friend = userdetail.User
	} else {
		return Error.New(cs.EF_FRIEND_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
	}

	return Error.OK()
}
