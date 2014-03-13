package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	//"errors"
	_ "html"
	"log"
	//"math/rand"
	//"time"
)
import (
	"bbproto"
	//"common"
	//"src/common/Error"
	//"common/consts"
	//"data"
	//redis "github.com/garyburd/redigo/redis"
)

func Gacha(uid uint32, gachaId, gachaCount int32) error {
	msg := &bbproto.ReqGacha{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.GachaId = proto.Int32(gachaId)
	msg.GachaCount = proto.Int32(gachaCount)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_GACHA)
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

func main() {
	log.Printf("==============================================")
	log.Printf("bbsvr test client begin...")

	Init()

	uid := uint32(141)
	gachaId := int32(1)
	gachaCount := int32(5)
	Gacha(uid, gachaId, gachaCount)

	log.Fatal("bbsvr test client finish.")
}
