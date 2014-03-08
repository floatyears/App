package friend

import (
	"fmt"
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
	"model/friend"
	"model/user"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func GetFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqGetFriend
	rspMsg := &bbproto.RspGetFriend{}

	handler := &GetFriend{}
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

type GetFriend struct {
	bbproto.BaseProtoHandler
}

func (t GetFriend) verifyParams(reqMsg *bbproto.ReqGetFriend) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.GetFriend == nil || reqMsg.GetHelper == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	return Error.OK()
}

func (t GetFriend) FillResponseMsg(reqMsg *bbproto.ReqGetFriend, rspMsg *bbproto.RspGetFriend, rspErr Error.Error) (outbuffer []byte) {
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

func (t GetFriend) ProcessLogic(reqMsg *bbproto.ReqGetFriend, rspMsg *bbproto.RspGetFriend) (e Error.Error) {

	uid := *reqMsg.Header.UserId
	isGetFriend := *reqMsg.GetFriend
	isGetHelper := *reqMsg.GetHelper

	db := &data.Data{}
	err := db.Open(consts.TABLE_FRIEND)
	defer db.Close()
	if err != nil {
		return Error.New(EC.CONNECT_DB_ERROR, err.Error())
	}

	rank := uint32(0)

	if isGetHelper {
		//get user's rank from user table
		userdetail, isUserExists, err := user.GetUserInfo(db, uid)
		if err != nil {
			return Error.New(EC.EU_GET_USERINFO_FAIL, fmt.Sprintf("ERROR: Get userinfo failed for %v, err:%v", uid, err))
		}
		if !isUserExists {
			return Error.New(EC.EU_INVALID_USERID, fmt.Sprintf("ERROR: Invalid userId %v", uid))
		}
		log.Printf("[TRACE] getUser(%v) ret userdetail: %v", uid, userdetail)
		rank = uint32(*userdetail.User.Rank)
	}

	// get FriendInfo
	if isGetFriend || isGetHelper {

		friendsInfo, e := friend.GetFriendInfo(db, uid, rank, false, isGetFriend, isGetHelper)
		log.Printf("[TRACE] GetFriendInfo ret err:%v. friends num=%v  ", err, len(friendsInfo))
		if e.IsError() && e.Code() != EC.EF_FRIEND_NOT_EXISTS {
			return Error.New(EC.EF_GET_FRIENDINFO_FAIL, fmt.Sprintf("GetFriends failed for uid %v, rank:%v", uid, rank))
		}

		//fill rspMsg
		rspMsg.Friends = &bbproto.FriendList{}
		if friendsInfo != nil && len(friendsInfo) > 0 {
			for _, friend := range friendsInfo {
				if friend.NickName == nil || friend.Rank == nil /*|| friend.Unit == nil*/ {
					log.Printf("[ERROR] unexcepted error: skip invalid friend(%v): %+v", *friend.UserId, friend)
					continue
				}

				//log.Printf("[TRACE] fid:%v friend:%v", fid, *friend.UserId)
				pFriend := friend
				if *friend.FriendState == bbproto.EFriendState_FRIENDHELPER {
					rspMsg.Friends.Helper = append(rspMsg.Friends.Helper, &pFriend)
				} else if *friend.FriendState == bbproto.EFriendState_ISFRIEND {
					rspMsg.Friends.Friend = append(rspMsg.Friends.Friend, &pFriend)
				} else if *friend.FriendState == bbproto.EFriendState_FRIENDIN {
					rspMsg.Friends.FriendIn = append(rspMsg.Friends.FriendIn, &pFriend)
				} else if *friend.FriendState == bbproto.EFriendState_FRIENDOUT {
					rspMsg.Friends.FriendOut = append(rspMsg.Friends.FriendOut, &pFriend)
				}
			}
		}
	}

	return Error.OK()
}
