package user

import (
	_ "fmt"
	"io/ioutil"
	"log"
	"net/http"
	//"strconv"
	"time"
)
import (
	bbproto "../bbproto"
	"../common"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
)

type AuthUser struct {
}

func (t AuthUser) verifyParams(reqMsg bbproto.ReqAuthUser) (err error) {
	//TODO: do some params validation
	return nil
}

func (t AuthUser) checkInput(req *http.Request) (reqMsg bbproto.ReqAuthUser, err error) {
	reqBuffer, err := ioutil.ReadAll(req.Body)
	if err != nil {
		log.Printf("ERR: ioutil.ReadAll failed: %v ", err)
		return reqMsg, err
	}

	err = proto.Unmarshal(reqBuffer, &reqMsg) //unSerialize into reqMsg
	if err != nil {
		log.Printf("ERR: checkInput parse proto err: %v", err)
		return reqMsg, err
	}
	log.Printf("recv reqMsg: %+v", reqMsg)

	err = t.verifyParams(reqMsg)

	return reqMsg, err
}

func (t AuthUser) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t AuthUser) FillResponseMsg(reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser, rspErr error) (outbuffer []byte, err error) {
	{
		rspMsg.Header = reqMsg.Header
		sessionId, _ := t.GenerateSessionId(reqMsg.Terminal.Uuid)
		reqMsg.Header.SessionId = &sessionId
		log.Printf("req header:%v reqMsg.Header:%v", *reqMsg.Header.SessionId, reqMsg.Header)
	}

	outbuffer, err = proto.Marshal(rspMsg)
	return outbuffer, err
}

func (t AuthUser) SendResponse(rsp http.ResponseWriter, reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser, rspErr error) (err error) {
	data, err := t.FillResponseMsg(reqMsg, rspMsg, rspErr)
	if err != nil {
		return err
	}
	_, err = common.SendResponse(rsp, data, err)
	return err
}

func AuthUserHandler(rsp http.ResponseWriter, req *http.Request) {
	p := &AuthUser{}
	rspMsg := &bbproto.RspAuthUser{}

	reqMsg, err := p.checkInput(req)
	if err != nil {
		p.SendResponse(rsp, &reqMsg, rspMsg, err)
		return
	}

	uuid := *reqMsg.Terminal.Uuid
	db := &data.Data{}
	err = db.Open(common.TABLE_NAME_USER)
	defer db.Close()
	if err != nil {
		p.SendResponse(rsp, &reqMsg, rspMsg, err)
		return
	}

	var value []byte
	if uuid != "" {
		value, err = db.Gets(uuid)
		log.Printf("get for '%v' ret err:%v, value: %v", uuid, err, value)
	}

	isUserExists := len(value) != 0
	log.Printf("isUserExists=%v value len=%v value: ['%v']  ", isUserExists, len(value), value)
	rspMsg.Userdetail = &bbproto.UserInfoDetail{}
	if isUserExists {
		err = proto.Unmarshal(value, rspMsg.Userdetail) //unSerialize into Userdetail
		tNow := uint32(time.Now().Unix())

		*rspMsg.Userdetail.User.StaminaRecover = uint32(tNow + 600) //10 minutes
		*rspMsg.Userdetail.User.LoginTime = uint32(tNow)
		log.Printf("read Userdetail ret err:%v, Userdetail: %+v", err, rspMsg.Userdetail)
	} else { //new user
		log.Printf("Cannot find data for user uuid:%v, create new user...", uuid)

		newUserId, err := GetNewUserId()
		defaultName := common.DEFAULT_USER_NAME
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

	err = p.SendResponse(rsp, &reqMsg, rspMsg, err)
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", err, rspMsg)
}
