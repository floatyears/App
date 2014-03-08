package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	_ "html"
	"log"
	//"math/rand"
	//"time"
)
import (
	"../src/bbproto"
	//"src/common"
	//"src/common/consts"
	//"src/data"
	_ "../src/quest"
	//"src/user/usermanage"
	//"src/friend"
	//redis "github.com/garyburd/redigo/redis"
)

func ChangeParty(myUid uint32, partyinfo *bbproto.PartyInfo) error {
	msg := &bbproto.ReqChangeParty{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(myUid)

	msg.Party = partyinfo

	log.Printf("ReqMsg: changeParty for userId(%v)", *msg.Header.UserId)
	for _, party := range msg.Party.PartyList {
		log.Printf("\t%v: %+v", *party.Id, party.Items)
	}

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}
	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_CHANGE_PARTY)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspChangeParty{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse: code[%v]: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)
	log.Printf("-----------------------Response end.----------------------\n")

	return err
}

//	removePos := make([]int, len(partUniqueId) )
//	log.T("removePos len=%v cap=%v",len(removePos), cap(removePos))
//	for k, pos:=range removePos {
//		unitList = append(unitList[:pos], unitList[pos+1:])
//	}

func ppmain() {
	log.Printf("==============================================")
	log.Printf("bbsvr test client begin...")

	Init()

	pi := &bbproto.PartyInfo{}
	pi.CurrentParty = proto.Int32(0)
	for i := int32(0); i < 5; i++ {
		party := &bbproto.UnitParty{}
		party.Id = proto.Int32(i)
		for pos := int32(0); pos < 4; pos++ {
			item := &bbproto.PartyItem{}
			item.UnitPos = proto.Int32(pos)
			item.UnitUniqueId = proto.Uint32(uint32(2 + pos))
			party.Items = append(party.Items, item)
		}

		pi.PartyList = append(pi.PartyList, party)
	}

	ChangeParty(150, pi)
	log.Fatal("bbsvr test client finish.")
}
