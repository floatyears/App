package friend

import (
	"fmt"
	"log"
	"net/http"
	//"time"
)

import (
	"../bbproto"
	Error "../common/error"
	"../const"
	"../data"
	"../user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func FindFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqFindFriend
	rspMsg := &bbproto.RspFindFriend{}

	handler := &FindFriend{}
	err := handler.ParseInput(req, &reqMsg)
	if err != nil {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, Error.New(cs.INVALID_PARAMS, err.Error())))
		return
	}

	Err := handler.verifyParams(&reqMsg)
	if Err.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, Err))
		return
	}

	// game logic

	Err = handler.ProcessLogic(&reqMsg, rspMsg)

	err = handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, Err))
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", err, rspMsg)
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
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header = reqMsg.Header //including the sessionId
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

	db := &data.Data{}
	err := db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil {
		return Error.NewError(err)
	}

	//get user's rank from user table
	userdetail, isUserExists, err := usermanage.GetUserInfo(uid)
	if err != nil {
		return Error.New(cs.EF_GET_USERINFO_FAIL, fmt.Sprintf("ERROR: GetUserInfo failed. userId %v", uid))
	}
	log.Printf("[TRACE] getUser(%v) ret userinfo: %v", uid, userdetail.User)

	// get FriendInfo
	if isUserExists {
		rspMsg.Friend = userdetail.User
	} else {

	}

	return Error.OK()
}
