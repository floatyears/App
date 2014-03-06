package user

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
	"data"
	"friend"
	"user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func LoginPackHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqLoginPack
	rspMsg := &bbproto.RspLoginPack{}

	handler := &LoginPack{}
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

type LoginPack struct {
	bbproto.BaseProtoHandler
}

func (t LoginPack) verifyParams(reqMsg *bbproto.ReqLoginPack) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.GetFriend == nil || reqMsg.GetHelper == nil || reqMsg.GetLogin == nil || reqMsg.GetPresent == nil || reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t LoginPack) FillResponseMsg(reqMsg *bbproto.ReqLoginPack, rspMsg *bbproto.RspLoginPack, rspErr Error.Error) (outbuffer []byte) {
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

func (t LoginPack) ProcessLogic(reqMsg *bbproto.ReqLoginPack, rspMsg *bbproto.RspLoginPack) (e Error.Error) {
	uid := *reqMsg.Header.UserId
	isGetFriend := *reqMsg.GetFriend
	isGetHelper := *reqMsg.GetHelper
	isGetLogin := *reqMsg.GetLogin
	//isGetPresent := *reqMsg.GetPresent

	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
		return
	}

	rank := uint32(0)
	if isGetHelper || isGetLogin {
		//get user's rank from user table
		userdetail, isUserExists, err := usermanage.GetUserInfo(db, uid)
		if err != nil {
			return Error.New(EC.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v", uid))
		}

		if !isUserExists {
			return Error.New(EC.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
		}

		log.Printf("[TRACE] getUser(%v) ret userdetail: %v", uid, userdetail)
		rank = uint32(*userdetail.User.Rank)

		//get LoginInfo
		if isGetLogin {
			rspMsg.Login = userdetail.Login
		}
	}

	// get FriendInfo
	if isGetFriend || isGetHelper {

		friendsInfo, e := friend.GetOnlyFriends(db, uid, rank)
		log.Printf("[TRACE] GetFriendInfo ret err:%v. friends num=%v  ", e.Error(), len(friendsInfo))
		if e.IsError() && e.Code() != EC.EF_FRIEND_NOT_EXISTS {
			return Error.New(EC.EF_GET_FRIENDINFO_FAIL, fmt.Sprintf("GetFriends failed for uid %v, rank:%v", uid, rank))
		}

		//fill rspMsg
		if friendsInfo != nil {
			for _, friend := range friendsInfo {
				if friend.NickName == nil || friend.Rank == nil /*|| friend.Unit == nil*/ {
					log.Printf("[ERROR] unexcepted error: skip invalid friend: %+v", friend)
					continue
				}

				//log.Printf("[TRACE] fid:%v friend:%v", fid, *friend.UserId)
				rspMsg.Friends = append(rspMsg.Friends, &friend)
			}
		}
	}

	//UpdateLastPlayTime(db, &userdetail)

	return Error.OK()
}
