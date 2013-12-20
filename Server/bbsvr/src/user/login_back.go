package user

import (
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
	"time"
)
import (
	bbproto "../bbproto"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
)

const (
	NEW_USER_NAME = "no name"
)

//import "../comm"
func verifyParams(msg bbproto.ReqLoginBack) (err error) {
	return err
}

func checkInput(req *http.Request) (msg bbproto.ReqLoginBack, err error) {
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

	err = verifyParams(msg)

	return msg, nil
}

func sendResponse(rsp http.ResponseWriter, rsperr error) (size int, err error) {
	size, err = fmt.Fprintf(rsp, "parse proto err: %v", rsperr)
	//size, err := rsp.Write(value)

	return size, err
}

func GetNewUserId() (userid int32, err error) {
	userid = 1001
	return userid, err
}

func LoginBackHandler(rsp http.ResponseWriter, req *http.Request) {

	msg, err := checkInput(req)
	if err != nil {
		sendResponse(rsp, err)
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

	rspMsg := &bbproto.RspLoginBack{}

	isUserExists := value != ""
	if isUserExists {
		err = proto.Unmarshal([]byte(value), rspMsg.User) //unSerialize into usrinfo

		size, err := sendResponse(rsp, err)
		log.Printf("rsp msg err:%v, size[%d]: %+v", err, size, rspMsg.User)
	} else {
		//rspMsg.User = &bbproto.UserInfo{}
		*rspMsg.User.UserId, err = GetNewUserId()
		*rspMsg.User.UserName = NEW_USER_NAME
		*rspMsg.User.LoginTime = int32(time.Now().Unix())

		log.Printf("ERR: Cannot find data for user:%v", *msg.UserId)
	}
}
