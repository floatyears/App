package user

import (
	//	"fmt"
	"log"
	"net/http"
	"strconv"
	"time"
)
import (
	bbproto "../bbproto"
	"../common"
	"../const"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
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

func GetFriendInfo(uid uint32) (friends []FriendData, err error) {
	err = db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil || uid == 0 {
		return err
	}

	var value []byte
	if uid != "" {
		value, err = db.Gets(uid)
		log.Printf("get from uid '%v' ret err:%v, value: %v", uid, err, value)
	}

	return friends, nil
}

func (t LoginPack) ProcessLogic(reqMsg *bbproto.ReqLoginPack, rspMsg *bbproto.RspLoginPack) (err error) {
	// read user data (by uuid) from db
	uid := *reqMsg.Header.UserId
	db := &data.Data{}

	isSessionExists := len(value) != 0
	log.Printf("isSessionExists=%v value len=%v value: ['%v']  ", isSessionExists, len(value), value)
	rspMsg.loginParam = &bbproto.LoginInfo{}
	if isSessionExists {
		err = proto.Unmarshal(value, rspMsg.Userdetail) //unSerialize into Userdetail
		tNow := uint32(time.Now().Unix())

		*rspMsg.Userdetail.User.LoginTime = uint32(tNow)
		log.Printf("read Userdetail ret err:%v, Userdetail: %+v", err, rspMsg.Userdetail)
	} else { //generate new user
		log.Printf("Cannot find data for user uuid:%v, create new user...", uuid)

		newUserId, err := GetNewUserId()
		defaultName := cs.DEFAULT_USER_NAME
		tNow := uint32(time.Now().Unix())
		rank := int32(0)
		exp := int32(0)
		staminaNow := int32(10)
		staminaMax := int32(10)
		staminaRecover := uint32(tNow + 600) //10 minutes
		rspMsg.Userdetail.User = &bbproto.UserInfo{
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
		log.Printf("rspMsg.Userdetail.User=%v...", rspMsg.Userdetail.User)
		log.Printf("rspMsg=%+v...", rspMsg)

		//TODO:save userinfo to db through goroutine
		outbuffer, err := proto.Marshal(rspMsg.Userdetail)
		err = db.Set(uuid, outbuffer)
		log.Printf("db.Set(%v) save new userinfo, return %v", uuid, err)
	}

	return err
}

func LoginPackHandler(rsp http.ResponseWriter, req *http.Request) {
	p := &LoginPack{}
	rspMsg := &bbproto.RspLoginPack{}

	msg, err := p.checkInput(req)
	if err != nil {
		data, _ := p.FillRspHeader(&msg, rspMsg)

		common.SendResponse(rsp, data)
		return
	}

	value := ""
	if *msg.UserId != 0 {
		id := strconv.Itoa(int(*msg.UserId))
		db := &data.Data{}
		db.Open("0")
		defer db.Close()
		value, err := db.Get(id)
		log.Printf("get for '%v' ret err:%v, value: %v", *msg.UserId, err, value)
	}

	isUserExists := value != ""
	if isUserExists {
		err = proto.Unmarshal([]byte(value), rspMsg.User) //unSerialize into usrinfo

		data, err := p.FillRspHeader(&msg, rspMsg)
		size, err := common.SendResponse(rsp, data)
		log.Printf("rsp msg err:%v, size[%d]: %+v", err, size, rspMsg.User)
	} else {
		//rspMsg.User = &bbproto.UserInfo{}
		*rspMsg.User.UserId, err = GetNewUserId()
		*rspMsg.User.UserName = common.DEFAULT_USER_NAME
		*rspMsg.User.LoginTime = uint32(time.Now().Unix())

		log.Printf("ERR: Cannot find data for user:%v", *msg.UserId)
	}
}
