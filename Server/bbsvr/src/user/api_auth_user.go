package user

import (
	"fmt"
	//"io/ioutil"
	"net/http"
	//"strconv"
	"math/rand"
	//"time"

	proto "code.google.com/p/goprotobuf/proto"
)

import (
	"../bbproto"
	"../common"
	"../common/Error"
	"../common/log"
	"../const"
	"../data"
	"../friend"
	"./usermanage"
)

/////////////////////////////////////////////////////////////////////////////

func AuthUserHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqAuthUser
	rspMsg := &bbproto.RspAuthUser{}

	handler := &AuthUser{}
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

type AuthUser struct {
	bbproto.BaseProtoHandler
}

func (t AuthUser) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t AuthUser) verifyParams(reqMsg *bbproto.ReqAuthUser) (err Error.Error) {
	//TODO: do some params validation
	if reqMsg.Terminal.Uuid == nil && reqMsg.Header.UserId == nil {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Terminal.Uuid == "" && *reqMsg.Header.UserId <= 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	return Error.OK()
}

func (t AuthUser) FillResponseMsg(reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header.Error = proto.String(rspErr.Error())

		sessionId, _ := t.GenerateSessionId(reqMsg.Terminal.Uuid)
		reqMsg.Header.SessionId = &sessionId
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

func (t AuthUser) ProcessLogic(reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser) (e Error.Error) {
	// read user data (by uuid) from db
	uuid := *reqMsg.Terminal.Uuid
	uid := *reqMsg.Header.UserId
	var userdetail bbproto.UserInfoDetail
	var isUserExists bool
	var err error
	if uid > 0 {
		userdetail, isUserExists, err = usermanage.GetUserInfo(uid)
	} else {
		userdetail, isUserExists, err = usermanage.GetUserInfoByUuid(uuid)
	}

	log.T("GetUserInfo(%v) ret isExists=%v userdetail: ['%v']  ",
		uuid, isUserExists, userdetail)
	if isUserExists {
		tNow := common.Now()

		//TODO: assign Userdetail.* into rspMsg
		rspMsg.User = userdetail.User
		rspMsg.User.StaminaRecover = proto.Uint32(tNow + 600) //10 minutes
		//rspMsg.User.LoginTime = proto.Uint32(tNow)
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
			userdetail, isUserExists, err := usermanage.GetUserInfo(uid)
			if err != nil {
				return Error.New(cs.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v", uid))
			}

			if !isUserExists {
				return Error.New(cs.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
			}

			log.T("getUser(%v) ret userdetail: %v", uid, userdetail)
			rank := uint32(*userdetail.User.Rank)

			friendsInfo, err := friend.GetFriendInfo(db, uid, rank, true, true)
			log.T("GetFriendInfo ret err:%v. friends num=%v  ", err, len(friendsInfo))
			if err != nil {
				return Error.New(cs.EF_GET_FRIENDINFO_FAIL, fmt.Sprintf("GetFriends failed for uid %v, rank:%v", uid, rank))
			}

			//fill rspMsg
			rspMsg.Friends = &bbproto.FriendList{}
			for _, friend := range friendsInfo {
				//log.T("fid:%v friend:%v", fid, *friend.UserId)
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

			//TODO: call update in goroutine
			UpdateLoginInfo(db, &userdetail)

			rspMsg.Login = userdetail.Login

			//TODO: get present
			//rspMsg.Present = userdetail.Present
		}
	} else { //create new user
		log.Printf("Cannot find data for user uuid:%v, create new user...", uuid)

		newUserId, err := usermanage.GetNewUserId()
		if err != nil {
			return Error.New(cs.EU_GET_NEWUSERID_FAIL, err.Error())
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
			Rank:           &rank,
			Exp:            &exp,
			StaminaNow:     &staminaNow,
			StaminaMax:     &staminaMax,
			StaminaRecover: &staminaRecover,
		}
		rspMsg.ServerTime = &tNow
		log.T("rspMsg.User=%v...", rspMsg.User)
		//log.T("rspMsg=%+v...", rspMsg)

		//TODO:save userinfo to db through goroutine
		err = usermanage.AddNewUser(uuid, rspMsg.User)
		//zUserData, err := proto.Marshal(&userdetail)
		//err = db.Set(uuid, zUserData)
		//log.Printf("db.Set(%v) save new userinfo, return %v", uuid, err)
	}

	return Error.OK()
}
