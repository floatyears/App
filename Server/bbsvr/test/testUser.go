package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	_ "html"
	//"io"
	//"io/ioutil"
	"log"
	//"math/rand"
	//"net/http"
	//"time"
)
import (
	"../src/bbproto"
	"../src/common"
	//"../src/const"
	//"../src/data"
	_ "../src/quest"
	//"../src/user/usermanage"
	//redis "github.com/garyburd/redigo/redis"
)

func LoginPack(uid uint32) error {
	msg := &bbproto.ReqLoginPack{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.GetFriend = proto.Bool(true)
	msg.GetHelper = proto.Bool(true)
	msg.GetLogin = proto.Bool(true)
	msg.GetPresent = proto.Bool(true)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_LOGIN_PACK)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspLoginPack{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse : [%v] error: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)
	if rspmsg.Friends != nil {
		for k, friend := range rspmsg.Friends {
			log.Printf("friend[%v]: %+v", k, friend)
		}

		//for k, friend := range rspmsg.Friends.Friend {
		//	log.Printf("friend[%v]: %+v", k, friend)
		//}

		//for k, friend := range rspmsg.Friends.Helper {
		//	log.Printf("Helper[%v]: %v", k, friend)
		//}

		//for k, friend := range rspmsg.Friends.FriendIn {
		//	log.Printf("FriendIn[%v]: %v", k, friend)
		//}

		//for k, friend := range rspmsg.Friends.FriendOut {
		//	log.Printf("FriendOut[%v]: %v", k, friend)
		//}
	} else {
		log.Printf("rspmsg.Friends is nil")
	}

	log.Printf("LoginInfo: %+v", rspmsg.Login)
	log.Printf("-----------------------Response end.----------------------\n")

	return err
}

func AuthUser(uuid string, uid uint32) {
	msg := &bbproto.ReqAuthUser{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("0.0.1")
	//log.Printf("msg.Header.UserId:%v", msg.Header.UserId)
	//uid := uint32(0)
	msg.Header.UserId = &uid
	//msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.PacketId = proto.Int32(18)
	msg.Terminal = &bbproto.TerminalInfo{}
	if uuid == "" {
		msg.Terminal.Uuid = proto.String("b2c4adfd-e6a9-4782-814d-67ce34220101")
	} else {
		msg.Terminal.Uuid = proto.String(uuid)
	}

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
		for k, friend := range rspmsg.Friends {
			log.Printf("Friend[%v]: %+v", k, friend)
		}
	} else {
		log.Printf("SendHttpPost ret err:%v", err)
	}

}

//func AddUsers(num uint32) error {
//	db := &data.Data{}
//	err := db.Open(string(cs.TABLE_USER))
//	if err != nil {
//		return err
//	}
//	defer db.Close()

//	//add to user table
//	tNow := uint32(time.Now().Unix())
//	for uid := uint32(101); uid-100 < num; uid++ {
//		rank := int32(uid-35) % 100
//		tNow += 3
//		name := "name" + common.Utoa(uid)

//		user := &bbproto.UserInfo{}
//		user.UserId = &uid
//		user.Rank = &rank
//		user.UserName = &name
//		user.LoginTime = &tNow

//		zUserinfo, err := proto.Marshal(user)
//		if err != nil {
//			return err
//		}
//		err = db.Set(common.Utoa(uid), zUserinfo)
//	}
//	return err
//}

func AddUsers() {
	for i := 0; i < 10; i++ {
		AuthUser("b2c4adfd-e6a9-4782-814d-67ce3422011"+common.Itoa(i),
			uint32(i+100))
	}
	for i := 10; i < 30; i++ {
		AuthUser("b2c4adfd-e6a9-4782-814d-67ce342201"+common.Itoa(i),
			uint32(i+100))
	}

}

type MyTp struct {
	x int32
}

type My struct {
	MyTp MyTp
}

func main() {
	Init()
	//AddUsers()

	AuthUser("b2c4adfd-e6a9-4782-814d-67ce34220101", 102)
	//LoginPack(101)

	log.Fatal("bbsvr test client finish.")
}
