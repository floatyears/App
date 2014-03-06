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
	//"src/common"
	//"src/common/consts"
	//"src/data"
	_ "../src/quest"
	//"src/user/usermanage"
	//redis "github.com/garyburd/redigo/redis"
)

func Rename(uid uint32, newName string) error {
	msg := &bbproto.ReqRenameNick{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.NewNickName = proto.String(newName)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_RENAME_NICK)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspRenameNick{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse : [%v] error: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)

	return err
}

func rmain() {
	Init()
	//Rename(104, "Jack04")
	//Rename(105, "Maggie05")
	//Rename(106, "Jessie06")
	//Rename(107, "Mary07")
	Rename(108, "Bill08")

	log.Fatal("bbsvr test client finish.")
}
