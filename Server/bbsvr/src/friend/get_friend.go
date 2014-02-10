package friend

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
	"../user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func GetFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqGetFriend
	rspMsg := &bbproto.RspGetFriend{}

	handler := &GetFriend{}
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

type GetFriend struct {
	bbproto.BaseProtoHandler
}

func (t GetFriend) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t GetFriend) verifyParams(reqMsg *bbproto.ReqGetFriend) (err error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.GetFriend == nil || reqMsg.GetHelper == nil {
		return errors.New("ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return errors.New("ERROR: userId is invalid.")
	}

	return nil
}

func (t GetFriend) FillResponseMsg(reqMsg *bbproto.ReqGetFriend, rspMsg *bbproto.RspGetFriend, rspErr error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId
		log.Printf("req header:%v reqMsg.Header:%v", *reqMsg.Header.SessionId, reqMsg.Header)
	}

	// fill custom protocol body

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func (t GetFriend) ProcessLogic(reqMsg *bbproto.ReqGetFriend, rspMsg *bbproto.RspGetFriend) (err error) {

	uid := *reqMsg.Header.UserId
	isGetFriend := *reqMsg.GetFriend
	isGetHelper := *reqMsg.GetHelper

	db := &data.Data{}
	err = db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil || uid == 0 {
		return
	}

	rank := uint32(0)

	if isGetHelper {
		//get user's rank from user table
		userdetail, isUserExists, err := usermanage.GetUserInfo(uid)
		if err != nil || !isUserExists {
			err := errors.New(fmt.Sprintf("ERROR: Invalid userId %v", uid))
			return err
		}
		log.Printf("[TRACE] getUser(%v) ret userdetail: %v", uid, userdetail)
		rank = uint32(*userdetail.User.Rank)
	}

	// get FriendInfo
	if isGetFriend || isGetHelper {

		friendsInfo, err := GetFriendInfo(db, uid, rank, isGetFriend, isGetHelper)
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

	return err
}
