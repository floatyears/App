package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	_ "fmt"
	_ "html"
	"io"
	"io/ioutil"
	"log"
	"net/http"
)
import (
	"../src/bbproto"
	_ "../src/quest"
	_ "../src/user"
)

const (
	_PROTO_LOGIN_BACK     = "/login_back"
	_PROTO_GET_QUEST_MAP  = "/get_new_quest_map"
	_PROTO_START_QUEST    = "/start_quest"
	_PROTO_CLEAR_QUEST    = "/clear_quest"
	_PROTO_GET_QUEST_INFO = "/get_quest_info"
)

const WEB_SERVER_ADDR = "http://127.0.0.1:8000"

func Init() {
	log.SetFlags(log.Ltime | log.Lmicroseconds | log.Lshortfile)
}

func SendHttpPost(dataBuf io.Reader) (size int, err error) {
	resp, err := http.Post(WEB_SERVER_ADDR+_PROTO_LOGIN_BACK, "application/binary", dataBuf)
	defer resp.Body.Close()

	if err != nil {
		log.Printf("post err:%+v", err)
		return 0, err
	}
	if resp.StatusCode != http.StatusOK {
		log.Printf("post ret code:%+v", resp.StatusCode)
		return 0, err
	}

	body, err := ioutil.ReadAll(resp.Body)
	log.Printf("post resp:%+v", body)

	return 0, err
}

func LoginBack() {
	msg := &bbproto.ReqLoginBack{}
	msg.Header = &bbproto.Header{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.UserId = proto.Int32(10011)

	buffer, err := proto.Marshal(msg)
	log.Printf("Marshal ret err:%v buffer:%v", err, buffer)

	SendHttpPost(bytes.NewReader(buffer))
}

func genReqGetQuestMap() (buffer []byte, err error) {
	msg := &bbproto.ReqGetQuestInfo{
		Header: &bbproto.Header{
			ApiVer:    proto.String("0.1"),
			Code:      proto.Int32(0),
			Error:     proto.String(""),
			PacketId:  proto.Int32(1),
			SessionId: proto.String("10000001"),
		},
	}
	buffer, err = proto.Marshal(msg) //SerializeToOstream
	return buffer, err
}

func main() {
	Init()

	LoginBack()
	//testRedis()
	//return

	log.Fatal("bbsvr test client")
}
