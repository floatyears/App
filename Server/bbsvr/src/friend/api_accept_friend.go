package friend

import (
	"fmt"
	"net/http"
	//"time"
)

import (
	"bbproto"
	proto "code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"model/friend"
	"model/user"
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

	//1. get userinfo from user table
	userDetail, isUserExists, err := user.GetUserInfo(db, uid)
	if err != nil {  return Error.New(EC.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", uid, err.Error()))  }
	if !isUserExists { return Error.New(EC.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid)) }

	//check my friendNum overflow
	friendNum, e := friend.GetFriendNum(db, uid)
	if e.IsError() { return e }
	if friendNum >= *userDetail.User.FriendMax {
		return Error.New(EC.EF_FRIENDNUM_OVERFLOW, fmt.Sprintf("userId: %v friendNum reach max(%v).", uid, friendNum))
	}

	//check friend's friendNum overflow
	friUserDetail, isUserExists, err := user.GetUserInfo(db, fUid)
	if err != nil { return Error.New(EC.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", fUid, err.Error())) }
	if !isUserExists { return Error.New(EC.EU_USER_NOT_EXISTS, fmt.Sprintf("fUid: %v not exists", fUid)) }

	ffriendNum, e := friend.GetFriendNum(db, fUid)
	if e.IsError() { return e }
	if ffriendNum >= *friUserDetail.User.FriendMax {
		return Error.New(EC.EF_FRIENDNUM_OVERFLOW, fmt.Sprintf("fUid: %v friendNum reach max(%v).", fUid, ffriendNum))
	}

	//2. addFriend
	e = friend.AddFriend(db, uid, fUid, bbproto.EFriendState_ISFRIEND, common.Now())
	if e.IsError() {
		log.Error("user:%v AcceptFriend(%v) failed: %v", uid, fUid, e.Error())
		return e
	}

	log.T("user:%v AcceptFriend(%v) ok.", uid, fUid)

	isFriendNum := int32(0)
	rspMsg.Friends, isFriendNum, e = friend.GetFriendList(db, uid)
	if e.IsError() && e.Code() != EC.EF_FRIEND_NOT_EXISTS {
		return Error.New(EC.EF_GET_FRIENDINFO_FAIL, fmt.Sprintf("GetFriends failed for uid %v", uid))
	}
	log.T("GetFriendList ret isFriendNum:%v", isFriendNum)

	return Error.OK()
}
