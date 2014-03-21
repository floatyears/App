package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	_ "html"
	//"io"
	//"io/ioutil"
	//"math/rand"
	//"net/http"
	//"time"
)
import (
	"bbproto"
	"common/log"
	//"src/model/user"
	//redis "github.com/garyburd/redigo/redis"
)

func SellUnit(uid uint32, sellUniqueId []uint32) error {
	msg := &bbproto.ReqSellUnit{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.UnitUniqueId = sellUniqueId

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_SELL_UNIT)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response (DataLen: %v)----------------------", len(rspbuff))
	rspMsg := &bbproto.RspSellUnit{}
	if err = proto.Unmarshal(rspbuff, rspMsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse Header: [%v] error: %v", *rspMsg.Header.Code, *rspMsg.Header.Error)
	if *rspMsg.Header.Code != 0 {
		log.T("\t rsp: %+v", rspMsg)
		return nil
	}

	log.T("=================rspMsg begin==================")
	log.T("\t GotMoney: %v", *rspMsg.GotMoney)
	log.T("\t Money: %v", *rspMsg.Money)
	for k, unit := range rspMsg.UnitList {
		log.T("\t unit[%v]: %+v", k, unit)
	}
	log.T(">>>>>>>>>>>>>>>>>>>Rsp End<<<<<<<<<<<<<<<<<<<<")

	return err
}

func main() {
	Init()

	uid := uint32(141)
	sellUnits := []uint32{11, 12, 18}

	SellUnit(uid, sellUnits)

	log.Fatal("bbsvr test client finish.")
}
