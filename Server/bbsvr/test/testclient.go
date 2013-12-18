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
	_PROTO_LOGIN_PACK     = "/login_pack"
	_PROTO_AUTH_USER      = "/auth_user"
	_PROTO_GET_QUEST_MAP  = "/get_new_quest_map"
	_PROTO_START_QUEST    = "/start_quest"
	_PROTO_CLEAR_QUEST    = "/clear_quest"
	_PROTO_GET_QUEST_INFO = "/get_quest_info"
)

const WEB_SERVER_ADDR = "http://127.0.0.1:8000"

func Init() {
	log.SetFlags(log.Ltime | log.Lmicroseconds | log.Lshortfile)
}

func SendHttpPost(dataBuf io.Reader, protoAddr string) (outbuffer []byte, err error) {
	resp, err := http.Post(WEB_SERVER_ADDR+protoAddr, "application/binary", dataBuf)
	if resp != nil && resp.Body != nil {
		log.Printf("SendHttpPost resp.Body.Close()...")
		defer resp.Body.Close()
	}

	if err != nil {
		log.Printf("post err:%+v", err)
		return nil, err
	}
	if resp.StatusCode != http.StatusOK {
		log.Printf("post ret code:%+v", resp.StatusCode)
		return nil, err
	}

	outbuffer, err = ioutil.ReadAll(resp.Body)
	log.Printf("recv resp:%+v", outbuffer)

	return outbuffer, err
}

func LoginPack() {
	msg := &bbproto.ReqLoginPack{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.UserId = proto.Int32(10011)

	buffer, err := proto.Marshal(msg)
	log.Printf("Marshal ret err:%v buffer:%v", err, buffer)

	SendHttpPost(bytes.NewReader(buffer), _PROTO_LOGIN_PACK)
}

func AuthUser() {
	msg := &bbproto.ReqAuthUser{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("0.0.1")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.PacketId = proto.Int32(18)
	msg.Terminal = &bbproto.TerminalInfo{}
	//msg.Terminal.Uuid = proto.String("b2c4adfd-e6a9-4782-814d-67ce34220110")
	msg.Terminal.Uuid = proto.String("koryyang")
	msg.Terminal.DeviceName = proto.String("kory's ipod")
	msg.Terminal.Os = proto.String("android 4.01")
	msg.Terminal.Platform = proto.String("official")

	buffer, err := proto.Marshal(msg)
	log.Printf("Marshal ret err:%v buffer:%v", err, buffer)

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_AUTH_USER)
	if err == nil {
		rspmsg := &bbproto.RspAuthUser{}
		err = proto.Unmarshal(rspbuff, rspmsg)
		log.Printf("rsp Unarshal ret err:%v rspmsg:%v", err, rspmsg)
	} else {
		log.Printf("SendHttpPost ret err:%v", err)
	}
}

func genReqGetQuestMap() (buffer []byte, err error) {
	msg := &bbproto.ReqGetQuestInfo{
		Header: &bbproto.ProtoHeader{
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

	//LoginPack()
	AuthUser()
	//testRedis()
	//return

	log.Fatal("bbsvr test client finish.")
}
