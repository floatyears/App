package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	//"errors"
	_ "html"

	//"math/rand"
	//"time"
)
import (
	"bbproto"
	//"common"
	//"common/consts"
	"common/log"
	//"data"
)

func StartQuest(uid uint32, stageId uint32, questId uint32, helperUid uint32) error {
	msg := &bbproto.ReqStartQuest{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.QuestId = proto.Uint32(questId)
	msg.StageId = proto.Uint32(stageId)
	msg.HelperUserId = proto.Uint32(helperUid)
	msg.HelperUnit = &bbproto.UserUnit{
		UniqueId:  proto.Uint32(2),
		UnitId:    proto.Uint32(9),
		Level:     proto.Int32(12),
		Exp:       proto.Int32(2000),
		AddAttack: proto.Int32(2),
		AddHp:     proto.Int32(2),
	}

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_START_QUEST)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response (len:%v)----------------------", len(rspbuff))
	rspmsg := &bbproto.RspStartQuest{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	if *rspmsg.Header.Code != 0 {
		log.Printf("rspmsg ret err: %+v", rspmsg)
		return nil
	}

	log.Printf("staminaNow:%v", *rspmsg.StaminaNow)
	log.Printf("StaminaRecover:%v", *rspmsg.StaminaRecover)

	log.Printf("QuestId:%v", *rspmsg.DungeonData.QuestId)
	for k, boss := range rspmsg.DungeonData.Boss {
		log.Printf("boss[%v]: %v", k, boss)
	}

	for k, enemy := range rspmsg.DungeonData.Enemys {
		log.Printf("enemy[%v]: %v", k, enemy)
	}

	for k, color := range rspmsg.DungeonData.Colors {
		log.Printf("color[%v]: %v", k, color)
	}

	log.Printf("------------------------------")
	for k, floor := range rspmsg.DungeonData.Floors {
		log.Printf("floor[%v]: ", k)
		for i, grid := range floor.GridInfo {
			log.Printf("\t grid[%v]: %v", i, grid)
		}

	}

	return err
}

func ClearQuest(uid uint32, questId uint32, getMoney int32) error {
	msg := &bbproto.ReqClearQuest{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.QuestId = proto.Uint32(questId)
	msg.GetMoney = proto.Int32(int32(getMoney))
	//msg.GetUnit = append(msg.GetUnit, uint32(5))
	msg.GetUnit = append(msg.GetUnit, uint32(4))
	msg.GetUnit = append(msg.GetUnit, uint32(5))
	msg.GetUnit = append(msg.GetUnit, uint32(6))

	msg.HitGrid = append(msg.HitGrid, uint32(6))

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_CLEAR_QUEST)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response (len:%v)----------------------", len(rspbuff))
	rspmsg := &bbproto.RspClearQuest{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	if *rspmsg.Header.Code != 0 {
		log.Printf("rspmsg ret err: %+v", rspmsg)
		return nil
	}

	log.Printf("rspMsg:%+v", rspmsg)

	return err
}

func main() {
	log.Printf("==============================================")
	log.Printf("bbsvr test client begin...")

	Init()

	StartQuest(1101, 17, 174, 105)
	ClearQuest(1101, 174, 5000)

	log.Fatal("bbsvr test client finish.")
}
