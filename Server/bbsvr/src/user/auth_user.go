package user

import (
	//"fmt"
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

func GetNewUserId() (userid int32, err error) {
	userid = 100001
	return userid, err
}

type AuthUser struct {
}

func (t AuthUser) verifyParams(reqmsg bbproto.ReqAuthUser) (err error) {
	//TODO: do some params validation
	return nil
}

func (t AuthUser) checkInput(req *http.Request) (reqmsg bbproto.ReqAuthUser, err error) {
	reqBuffer, err := ioutil.ReadAll(req.Body)
	if err != nil {
		log.Printf("ERR: ioutil.ReadAll failed: %v ", err)
		return reqmsg, err
	}

	err = proto.Unmarshal(reqBuffer, &reqmsg) //unSerialize into reqmsg
	if err != nil {
		log.Printf("ERR: checkInput parse proto err: %v", err)
		return reqmsg, err
	}
	log.Printf("recv reqmsg: %+v", reqmsg)

	err = t.verifyParams(reqmsg)

	return reqmsg, err
}

func (t AuthUser) FillResponseMsg(reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser, rspErr error) (outbuffer []byte, err error) {
	rspMsg.Header = reqMsg.Header
	//log.Printf("rspMsg.Header=%v", rspMsg.Header)

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

	reqmsg, err := p.checkInput(req)
	if err != nil {
		p.SendResponse(rsp, &reqmsg, rspMsg, err)
		return
	}

	uuid := *reqmsg.Terminal.Uuid
	db := &data.Data{}
	err = db.Open(common.TABLE_NAME_USER)
	defer db.Close()
	if err != nil {
		p.SendResponse(rsp, &reqmsg, rspMsg, err)
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
		defaultName := common.NEW_USER_NAME
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

	err = p.SendResponse(rsp, &reqmsg, rspMsg, err)
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", err, rspMsg)
}
