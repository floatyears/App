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

type AuthUser struct {
}

func (t AuthUser) verifyParams(msg bbproto.ReqAuthUser) (err error) {
	//TODO: do some params validation
	return nil
}

func (t AuthUser) checkInput(req *http.Request) (msg bbproto.ReqAuthUser, err error) {
	reqBuffer, err := ioutil.ReadAll(req.Body)
	if err != nil {
		log.Printf("ERR: ioutil.ReadAll failed: %v ", err)
		return msg, err
	}
	log.Printf("GetQuestMap req.body: %v ", reqBuffer)

	err = proto.Unmarshal(reqBuffer, &msg) //unSerialize into msg
	if err != nil {
		log.Printf("ERR: parse proto err: %v", err)
		return msg, err
	}
	log.Printf("recv msg: %+v", msg)

	err = t.verifyParams(msg)

	return msg, err
}

func (t AuthUser) FillRspHeader(reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser) (outbuffer []byte, err error) {
	*rspMsg.Header = *reqMsg.Header
	outbuffer, err = proto.Marshal(rspMsg)
	return outbuffer, err
}

func GetNewUserId() (userid int32, err error) {
	userid = 100001
	return userid, err
}

func AuthUserHandler(rsp http.ResponseWriter, req *http.Request) {
	p := &AuthUser{}
	rspMsg := &bbproto.RspAuthUser{}

	msg, err := p.checkInput(req)
	if err != nil {
		data, err := p.FillRspHeader(&msg, rspMsg)
		common.SendResponse(rsp, data, err)
		return
	}

	value := ""
	uuid := *msg.Terminal.Uuid
	if uuid != "" {
		db := &data.Data{}
		db.Open(common.TABLE_NAME_USER)
		defer db.Close()
		value, err := db.Get(uuid)
		log.Printf("get for '%v' ret err:%v, value: %v", uuid, err, value)
	}

	isUserExists := value != ""
	if isUserExists {
		err = proto.Unmarshal([]byte(value), rspMsg.User) //unSerialize into usrinfo

		log.Printf("rsp msg err:%v, userinfo: %+v", err, rspMsg.User)
	} else {
		log.Printf("Cannot find data for user uuid:%v, create new user...", uuid)
		//rspMsg.User = &bbproto.UserInfo{}
		*rspMsg.User.UserId, err = GetNewUserId()
		*rspMsg.User.UserName = common.NEW_USER_NAME
		*rspMsg.User.LoginTime = int32(time.Now().Unix())

		//TODO:save userinfo to db through goroutine
	}

	data, err := p.FillRspHeader(&msg, rspMsg)
	size, err := common.SendResponse(rsp, data, err)
	log.Printf("sendrsp err:%v, rsp size: %v", err, size)
}
