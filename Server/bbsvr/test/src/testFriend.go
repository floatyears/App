package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	"fmt"
	_ "html"
	"log"
	//"math/rand"
	"time"
)
import (
	"bbproto"
	//"../src/common"
	"common/consts"
	"data"
	"model/friend"
	//redis "github.com/garyburd/redigo/redis"
)

func GetFriend(uid uint32) error {
	msg := &bbproto.ReqGetFriend{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.GetFriend = proto.Bool(true)
	msg.GetHelper = proto.Bool(true)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_GET_FRIEND)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspGetFriend{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("RspGetFriend uid:%v code[%v]: %v", uid, *rspmsg.Header.Code, *rspmsg.Header.Error)
	if err == nil && rspmsg.Friends != nil {
		for k, friend := range rspmsg.Friends.Friend {
			log.Printf("friend[%v]: %+v", k, friend)
		}

		for k, friend := range rspmsg.Friends.Helper {
			log.Printf("Helper[%v]: %v", k, friend)
		}

		for k, friend := range rspmsg.Friends.FriendIn {
			log.Printf("FriendIn[%v]: %v", k, friend)
		}

		for k, friend := range rspmsg.Friends.FriendOut {
			log.Printf("FriendOut[%v]: %v", k, friend)
		}
	}

	log.Printf("-----------------------Response end.----------------------\n")

	return err
}

func FindFriend(myUid uint32, fUid uint32) error {
	msg := &bbproto.ReqFindFriend{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(myUid)

	msg.FriendUid = proto.Uint32(fUid)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_FIND_FRIEND)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspFindFriend{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("RspFindFriend uid:%v code[%v]: %v", myUid, *rspmsg.Header.Code, *rspmsg.Header.Error)
	if err == nil && rspmsg.Friend != nil {
		log.Printf("ret friendinfo: %+v", rspmsg.Friend)
	} else {
		log.Printf("ret friendinfo is nil, msg: %+v", rspmsg)
	}

	log.Printf("-----------------------Response end.----------------------\n")

	return err
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

func DataAddFriends(uid uint32, num uint32) error {
	db := &data.Data{}
	err := db.Open(string(consts.TABLE_FRIEND))
	if err != nil {
		return err
	}
	defer db.Close()

	for fid := uint32(101); fid-100 < num; fid++ {
		if fid == uid {
			continue
		}
		updatetime := uint32(time.Now().Unix())

		if fid%4 == 1 {
			fState := bbproto.EFriendState_FRIENDHELPER
			friend.AddHelper(db, uid, fid, fState, updatetime)
		} else {
			fState := bbproto.EFriendState_ISFRIEND
			if fid%5 == 2 {
				fState = bbproto.EFriendState_FRIENDIN
			} else if fid%5 == 3 {
				fState = bbproto.EFriendState_FRIENDOUT
			}
			friend.AddFriend(db, uid, fid, fState, updatetime)
		}
	}

	return err
}

func DelFriend(myUid uint32, fUid uint32) error {
	msg := &bbproto.ReqDelFriend{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(myUid)

	msg.FriendUid = proto.Uint32(fUid)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}
	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_DEL_FRIEND)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspDelFriend{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse: code[%v]: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)
	log.Printf("-----------------------Response end.----------------------\n")

	return err
}

func AddFriend(myUid uint32, fUid uint32) error {
	msg := &bbproto.ReqAddFriend{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(myUid)

	msg.FriendUid = proto.Uint32(fUid)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}
	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_ADD_FRIEND)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspAddFriend{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse: code[%v]: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)
	log.Printf("-----------------------Response end.----------------------\n")

	return err
}

func AcceptFriend(myUid uint32, fUid uint32) error {
	msg := &bbproto.ReqAcceptFriend{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(myUid)

	msg.FriendUid = proto.Uint32(fUid)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}
	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_ACCEPT_FRIEND)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspAcceptFriend{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse: code[%v]: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)
	log.Printf("-----------------------Response end.----------------------\n")

	return err
}

func fmain() {
	log.Printf("==============================================")
	log.Printf("bbsvr test client begin...")

	Init()
	//DataAddFriends(101, 39)

	//protocol test
	//GetFriend(130)
	//FindFriend(101, 130)

	//AddFriend(130, 103)
	//AddFriend(130, 104)
	//AddFriend(130, 106)
	//AddFriend(130, 107)
	//AddFriend(130, 131)
	//AddFriend(130, 129)
	//AddFriend(130, 128)

	//AcceptFriend(101, 120)
	//AcceptFriend(130, 101)
	GetFriend(130)

	//DelFriend(120, 101)
	//GetFriend(120)

	log.Fatal("bbsvr test client finish.")
}