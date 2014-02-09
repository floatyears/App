package user

import (
	"errors"
	"fmt"
	//"io/ioutil"
	"log"
	"net/http"
	//"strconv"
	"math/rand"
	//"time"
)
import (
	bbproto "../bbproto"
	"../common"
	"../const"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func AuthUserHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqAuthUser
	rspMsg := &bbproto.RspAuthUser{}

	handler := &AuthUser{}
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

type AuthUser struct {
	bbproto.BaseProtoHandler
}

func (t AuthUser) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t AuthUser) verifyParams(reqMsg proto.Message) (err error) {
	//TODO: do some params validation
	return nil
}

func (t AuthUser) FillResponseMsg(reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser, rspErr error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header
		sessionId, _ := t.GenerateSessionId(reqMsg.Terminal.Uuid)
		reqMsg.Header.SessionId = &sessionId
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

func (t AuthUser) ProcessLogic(reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser) (err error) {
	// read user data (by uuid) from db
	uuid := *reqMsg.Terminal.Uuid
	uid := *reqMsg.Header.UserId
	var userdetail bbproto.UserInfoDetail
	var isUserExists bool
	if uid > 0 {
		userdetail, isUserExists, err = GetUserInfo(uid)
	} else {
		userdetail, isUserExists, err = GetUserInfoByUuid(uuid)
	}

	log.Printf("[TRACE] GetUserInfo(%v) ret isExists=%v userdetail: ['%v']  ",
		uuid, isUserExists, userdetail)
	if isUserExists {
		tNow := common.Now()

		//TODO: assign Userdetail.* into rspMsg
		rspMsg.User = userdetail.User
		rspMsg.User.StaminaRecover = proto.Uint32(tNow + 600) //10 minutes
		rspMsg.User.LoginTime = proto.Uint32(tNow)
		log.Printf("read Userdetail ret err:%v, Userdetail: %+v", err, userdetail)

		// get FriendInfo
		{
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

			friendsInfo, err := GetFriendInfo(db, uid, rank, true, true)
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

			//TODO: call update in goroutine
			UpdateLoginInfo(db, &userdetail)
		}
	} else { //generate new user
		log.Printf("Cannot find data for user uuid:%v, create new user...", uuid)

		newUserId, err := GetNewUserId()
		if err != nil {
			return err
		}
		defaultName := cs.DEFAULT_USER_NAME
		tNow := common.Now()
		rank := int32(30 + rand.Intn(10)) //int32(1)
		exp := int32(0)
		staminaNow := int32(10)
		staminaMax := int32(10)
		staminaRecover := uint32(tNow + 600) //10 minutes
		rspMsg.User = &bbproto.UserInfo{
			UserId:         &newUserId,
			UserName:       &defaultName,
			LoginTime:      &tNow,
			Rank:           &rank,
			Exp:            &exp,
			StaminaNow:     &staminaNow,
			StaminaMax:     &staminaMax,
			StaminaRecover: &staminaRecover,
		}
		rspMsg.ServerTime = &tNow
		log.Printf("[TRACE] rspMsg.User=%v...", rspMsg.User)
		//log.Printf("[TRACE] rspMsg=%+v...", rspMsg)

		//TODO:save userinfo to db through goroutine
		AddNewUser(uuid, rspMsg.User)
		//zUserData, err := proto.Marshal(&userdetail)
		//err = db.Set(uuid, zUserData)
		//log.Printf("db.Set(%v) save new userinfo, return %v", uuid, err)
	}

	return err
}
