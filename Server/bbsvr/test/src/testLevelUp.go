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
	"bbproto"
	//"src/common"
	//"src/common/consts"
	//"src/data"
	//"src/model/user"
	//redis "github.com/garyburd/redigo/redis"
)

func LevelUp(uid uint32) error {
	msg := &bbproto.ReqLevelUp{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.BaseUniqueId = proto.Uint32(3)
	msg.PartUniqueId = append(msg.PartUniqueId, 5)
	msg.PartUniqueId = append(msg.PartUniqueId, 6)
	msg.PartUniqueId = append(msg.PartUniqueId, 7)
	msg.HelperUserId = proto.Uint32(150)
	msg.HelperUnit = &bbproto.UserUnit{}
	msg.HelperUnit.UniqueId = proto.Uint32(5)
	msg.HelperUnit.UnitId = proto.Uint32(3)
	msg.HelperUnit.Exp = proto.Int32(300)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_LEVEL_UP)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspLevelUp{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
		return err
	}

	//print rsp msg
	log.Printf("Reponse : [%v] error: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)

	return err
}

func main() {
	Init()
	LevelUp(153)

	log.Fatal("bbsvr test client finish.")
}