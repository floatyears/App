package user

import (
	_ "fmt"
	//"io/ioutil"
	"log"
	"net/http"
	//"strconv"
	"time"
)
import (
	bbproto "../bbproto"
	//"../common"
	"../const"
	//"../data"
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

	//log.Printf("isUserExists=%v value len=%v value: ['%v']  ", isUserExists, len(value), value)
	if isUserExists {
		tNow := uint32(time.Now().Unix())

		//TODO: assign Userdetail.* into rspMsg
		*rspMsg.User = *userdetail.User
		*rspMsg.User.StaminaRecover = uint32(tNow + 600) //10 minutes
		*rspMsg.User.LoginTime = uint32(tNow)
		log.Printf("read Userdetail ret err:%v, Userdetail: %+v", err, userdetail)
	} else { //generate new user
		log.Printf("Cannot find data for user uuid:%v, create new user...", uuid)

		newUserId, err := GetNewUserId()
		if err != nil {
			return err
		}
		defaultName := cs.DEFAULT_USER_NAME
		tNow := uint32(time.Now().Unix())
		rank := int32(0)
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
		log.Printf("rspMsg.User=%v...", rspMsg.User)
		log.Printf("rspMsg=%+v...", rspMsg)

		//TODO:save userinfo to db through goroutine
		AddNewUser(uuid, rspMsg.User)
		//zUserData, err := proto.Marshal(&userdetail)
		//err = db.Set(uuid, zUserData)
		//log.Printf("db.Set(%v) save new userinfo, return %v", uuid, err)
	}

	return err
}
