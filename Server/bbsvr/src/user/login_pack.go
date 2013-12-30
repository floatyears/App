package user

import (
	//	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
	"time"
)
import (
	bbproto "../bbproto"
	"../common"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
)

type LoginPack struct {
}

//import "../comm"
func (t LoginPack) verifyParams(msg bbproto.ReqLoginPack) (err error) {
	return err
}

func (t LoginPack) checkInput(req *http.Request) (msg bbproto.ReqLoginPack, err error) {
	reqBuffer, err := ioutil.ReadAll(req.Body)
	if err != nil {
		log.Printf("ERR: ioutil.ReadAll failed: %v ", err)
		return msg, err
	}
	log.Printf("GetQuestMap req.body: %v ", reqBuffer)

	err = proto.Unmarshal(reqBuffer, &msg) //unSerialize into msg
	if err != nil {
		log.Printf("parse proto err: %v", err)
		return msg, err
	}
	log.Printf("recv msg: %+v", msg)

	err = t.verifyParams(msg)

	return msg, nil
}

func (t LoginPack) FillRspHeader(reqMsg *bbproto.ReqLoginPack, rspMsg *bbproto.RspLoginPack) (outbuffer []byte, err error) {
	*rspMsg.Header = *reqMsg.Header
	outbuffer, err = proto.Marshal(rspMsg)
	return outbuffer, err
}

func LoginPackHandler(rsp http.ResponseWriter, req *http.Request) {
	p := &LoginPack{}
	rspMsg := &bbproto.RspLoginPack{}

	msg, err := p.checkInput(req)
	if err != nil {
		data, err := p.FillRspHeader(&msg, rspMsg)
		common.SendResponse(rsp, data, err)
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
		size, err := common.SendResponse(rsp, data, err)
		log.Printf("rsp msg err:%v, size[%d]: %+v", err, size, rspMsg.User)
	} else {
		//rspMsg.User = &bbproto.UserInfo{}
		*rspMsg.User.UserId, err = GetNewUserId()
		*rspMsg.User.UserName = common.NEW_USER_NAME
		*rspMsg.User.LoginTime = uint32(time.Now().Unix())

		log.Printf("ERR: Cannot find data for user:%v", *msg.UserId)
	}
}
