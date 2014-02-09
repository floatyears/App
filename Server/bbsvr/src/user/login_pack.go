package user

import (
	"fmt"
	"log"
	"net/http"
	//"time"
	"errors"
)
import (
	bbproto "../bbproto"
	//"../common"
	"../const"
	"../data"
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

func (t LoginPack) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t LoginPack) verifyParams(reqMsg proto.Message) (err error) {
	//TODO: do some params validation
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
	if uid == 0 {
		err = errors.New("ERROR: userId is invalid.")
		return err
	}
	if reqMsg.GetFriend == nil || reqMsg.GetHelper == nil || reqMsg.GetLogin == nil || reqMsg.GetPresent == nil {
		err = errors.New("ERROR: params is invalid.")
		return err
	}

	isGetFriend := *reqMsg.GetFriend
	isGetHelper := *reqMsg.GetHelper
	isGetLogin := *reqMsg.GetLogin
	isGetPresent := *reqMsg.GetPresent

	db := &data.Data{}
	err = db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil || uid == 0 {
		return
	}

	//get user's rank from user table
	userdetail, isUserExists, err := GetUserInfo(uid)
	if err != nil || !isUserExists {
		err := errors.New(fmt.Sprintf("ERROR: Invalid userId %v", uid))
		return err
	}
	log.Printf("[TRACE] getUser(%v) ret userdetail: %v", uid, userdetail)
	rank := uint32(*userdetail.User.Rank)

	// get FriendInfo
	if isGetFriend || isGetHelper {

		friendsInfo, err := GetFriendInfo(db, uid, rank, isGetFriend, isGetHelper)
		log.Printf("[TRACE] GetFriendInfo ret err:%v. friends num=%v  ", err, len(friendsInfo))

		//fill rspMsg
		for _, friend := range friendsInfo {
			//log.Printf("[TRACE] fid:%v friend:%v", fid, *friend.UserId)
			pFriend := friend
			if *friend.FriendState == bbproto.EFriendState_FRIENDHELPER {
				rspMsg.Helper = append(rspMsg.Helper, &pFriend)
			} else {
				rspMsg.Friend = append(rspMsg.Friend, &pFriend)
			}
		}
	}

	UpdateLoginInfo(db, &userdetail)

	//get LoginInfo
	if isGetLogin {
		rspMsg.Login = userdetail.Login
	}

	//TODO: get present
	if isGetPresent {

	}

	return err
}
