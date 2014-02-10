package user

import (
	"fmt"
	"log"
	"net/http"
	//"time"
	"errors"
)

import (
	"../bbproto"
	//"../common"
	"../const"
	"../data"
	"../friend"
	"./usermanage"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func LoginPackHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqLoginPack
	rspMsg := &bbproto.RspLoginPack{}

	handler := &LoginPack{}
	err := handler.ParseInput(req, &reqMsg)
	if err != nil {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
		return
	}

	err = handler.verifyParams(&reqMsg)
	if err != nil {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
		return
	}

	// game logic

	err = handler.ProcessLogic(&reqMsg, rspMsg)

	err = handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", err, rspMsg)
}

/////////////////////////////////////////////////////////////////////////////

type LoginPack struct {
	bbproto.BaseProtoHandler
}

func (t LoginPack) verifyParams(reqMsg *bbproto.ReqLoginPack) (err error) {
	//TODO: input params validation
	if reqMsg.GetFriend == nil || reqMsg.GetHelper == nil || reqMsg.GetLogin == nil || reqMsg.GetPresent == nil {
		return errors.New("ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return errors.New("ERROR: userId is invalid.")
	}

	return nil
}

func (t LoginPack) FillResponseMsg(reqMsg *bbproto.ReqLoginPack, rspMsg *bbproto.RspLoginPack, rspErr error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId

		log.Printf("req header:%v reqMsg.Header:%v", *reqMsg.Header.SessionId, reqMsg.Header)
	}

	// fill custom protocol body
	{

	}

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func (t LoginPack) ProcessLogic(reqMsg *bbproto.ReqLoginPack, rspMsg *bbproto.RspLoginPack) (err error) {
	uid := *reqMsg.Header.UserId
	isGetFriend := *reqMsg.GetFriend
	isGetHelper := *reqMsg.GetHelper
	isGetLogin := *reqMsg.GetLogin
	//isGetPresent := *reqMsg.GetPresent

	db := &data.Data{}
	err = db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil || uid == 0 {
		return
	}

	rank := uint32(0)
	if isGetHelper || isGetLogin {
		//get user's rank from user table
		userdetail, isUserExists, err := usermanage.GetUserInfo(uid)
		if err != nil || !isUserExists {
			err := errors.New(fmt.Sprintf("ERROR: Invalid userId %v", uid))
			return err
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

		friendsInfo, err := friend.GetFriendInfo(db, uid, rank, isGetFriend, isGetHelper)
		log.Printf("[TRACE] GetFriendInfo ret err:%v. friends num=%v  ", err, len(friendsInfo))

		//fill rspMsg
		for _, friend := range friendsInfo {
			if friend.UserName == nil || friend.Rank == nil /*|| friend.Unit == nil*/ {
				log.Printf("[ERROR] unexcepted error: skip invalid friend: %+v", friend)
				continue
			}

			//log.Printf("[TRACE] fid:%v friend:%v", fid, *friend.UserId)
			pFriend := friend
			if *friend.FriendState == bbproto.EFriendState_FRIENDHELPER {
				rspMsg.Helper = append(rspMsg.Helper, &pFriend)
			} else {
				rspMsg.Friend = append(rspMsg.Friend, &pFriend)
			}
		}
	}

	//UpdateLastPlayTime(db, &userdetail)

	return err
}
