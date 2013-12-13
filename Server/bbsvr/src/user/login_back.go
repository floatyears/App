package user

import (
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
)
import (
	bbproto "../bbproto"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
)

//import "../comm"

func checkInput(reqBuffer []byte) (msg bbproto.ReqLoginBack, err error) {
	//msg := &bbproto.ReqLoginBack{}
	err = proto.Unmarshal(reqBuffer, &msg) //unSerialize into msg
	if err != nil {
		log.Printf("parse proto err: %v", err)
		return msg, err
	}
	log.Printf("recv msg: %+v", msg)

	return msg, nil
}

func sendResponse(rsp http.ResponseWriter, rsperr error) (err error) {
	fmt.Fprintf(rsp, "parse proto err: %v", rsperr)

	return nil
}

func LoginBackHandler(rsp http.ResponseWriter, req *http.Request) {
	req_data, err := ioutil.ReadAll(req.Body)
	if err != nil {
		sendResponse(rsp, err)
		return
	}
	log.Printf("GetQuestMap req.body: %v ", req_data)

	msg, err := checkInput(req_data)
	if err != nil {
		sendResponse(rsp, err)
		return
	}

	id := strconv.Itoa(int(*msg.UserId))
	db := &data.Data{}
	db.Open("0")
	defer db.Close()
	value, err := db.Get(id)
	log.Printf("get for '%v' ret err:%v, value: %v", *msg.UserId, err, value)

	isUserExists := value != ""
	if isUserExists {
		//size, err := rsp.Write(value)
		size, err := fmt.Fprintf(rsp, value) //rsp string

		//usrinfo := &bbproto.UserInfo{}
		rspMsg := &bbproto.RspLoginBack{}

		err = proto.Unmarshal([]byte(value), rspMsg.User) //unSerialize into usrinfo
		log.Printf("rsp msg err:%v, size[%d]: %+v", err, size, rspMsg.User)
	} else {
		log.Printf("ERR: Cannot find data for user:%v", *msg.UserId)
	}

}
