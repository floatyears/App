package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	"fmt"
	_ "html"
	"io"
	"io/ioutil"
	"log"
	"net/http"
	"time"
)
import (
	"../src/bbproto"
	"../src/common"
	"../src/const"
	"../src/data"
	_ "../src/quest"
	"../src/user"
	//redis "github.com/garyburd/redigo/redis"
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
	//log.Printf("recv resp:%+v", outbuffer)

	return outbuffer, err
}

func LoginPack() {
	msg := &bbproto.ReqLoginPack{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(100002)

	buffer, err := proto.Marshal(msg)
	log.Printf("Marshal ret err:%v buffer:%v", err, buffer)

	SendHttpPost(bytes.NewReader(buffer), _PROTO_LOGIN_PACK)
}

func AuthUser() {
	msg := &bbproto.ReqAuthUser{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("0.0.1")
	//log.Printf("msg.Header.UserId:%v", msg.Header.UserId)
	uid := uint32(0)
	msg.Header.UserId = &uid
	//msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.PacketId = proto.Int32(18)
	msg.Terminal = &bbproto.TerminalInfo{}
	msg.Terminal.Uuid = proto.String("b2c4adfd-e6a9-4782-814d-67ce34220101")
	msg.Terminal.DeviceName = proto.String("kory's ipod")
	msg.Terminal.Os = proto.String("iOS 6.1")
	//msg.Terminal.Platform = proto.String("official")

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

func testType() {

	reply := []interface{}{[]byte{0x57, 0x6f, 0x72, 0x6c, 0x64}, []byte{0x48, 0x65, 0x6c, 0x6c, 0x6f}}

	for _, x := range reply {
		var v, ok = x.([]byte)
		if ok {
			fmt.Println(string(v))
		}
	}
}

func AddUsers(num uint32) error {
	db := &data.Data{}
	err := db.Open(string(cs.TABLE_USER))
	if err != nil {
		return err
	}
	defer db.Close()

	//add to user table
	tNow := uint32(time.Now().Unix())
	for uid := uint32(101); uid-100 < num; uid++ {
		rank := int32(uid-35) % 100
		tNow += 3
		name := "name" + common.Utoa(uid)

		user := &bbproto.UserInfo{}
		user.UserId = &uid
		user.Rank = &rank
		user.UserName = &name
		user.LoginTime = &tNow

		zUserinfo, err := proto.Marshal(user)
		if err != nil {
			return err
		}
		err = db.Set(common.Utoa(uid), zUserinfo)
	}
	return err
}

func AddFriends() error {
	db := &data.Data{}
	err := db.Open(string(cs.TABLE_FRIEND))
	if err != nil {
		return err
	}
	defer db.Close()

	//AddUsers(15)
	uid := uint32(101)
	sUid := common.Utoa(uid)
	num := uint32(10)
	for fid := uint32(202); fid-200 < num; fid++ {
		updatetime := uint32(time.Now().Unix())

		fState := bbproto.EFriendState_FRIENDHELPER
		user.AddFriend(db, cs.X_HELPER_MY+sUid, fid, fState, updatetime)
	}

	return err
}

func main() {
	Init()

	//AddFriends()
	LoginPack()
	//AuthUser()
	//testRedis()
	//return

	log.Fatal("bbsvr test client finish.")
}
