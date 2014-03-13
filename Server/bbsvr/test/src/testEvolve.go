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
	//"src/quest"
	//"src/model/user"
	//"src/friend"
	//redis "github.com/garyburd/redigo/redis"
)

func EvolveStart(uid uint32, questId uint32, helperUid uint32) error {
	msg := &bbproto.ReqEvolveStart{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.EvolveQuestId = proto.Uint32(questId)
	msg.HelperUserId = proto.Uint32(helperUid)
	msg.HelperUnit = &bbproto.UserUnit{
		UniqueId:  proto.Uint32(3),
		UnitId:    proto.Uint32(10),
		Level:     proto.Int32(12),
		Exp:       proto.Int32(2000),
		AddAttack: proto.Int32(2),
		AddHp:     proto.Int32(3),
	}
	msg.BaseUniqueId = proto.Uint32(15)
	msg.PartUniqueId = append(msg.PartUniqueId, uint32(13))
	msg.PartUniqueId = append(msg.PartUniqueId, uint32(14))

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_EVOLVE_START)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response (len:%v)----------------------", len(rspbuff))
	rspmsg := &bbproto.RspEvolveStart{}
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

func EvolveDone(uid uint32, questId uint32, getMoney int32) error {
	msg := &bbproto.ReqEvolveDone{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.QuestId = proto.Uint32(questId)
	msg.GetMoney = proto.Int32(int32(getMoney))
	msg.GetUnit = append(msg.GetUnit, uint32(5))
	msg.GetUnit = append(msg.GetUnit, uint32(4))
	msg.GetUnit = append(msg.GetUnit, uint32(6))
	msg.GetUnit = append(msg.GetUnit, uint32(8))

	msg.HitGrid = append(msg.HitGrid, uint32(6))

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_EVOLVE_DONE)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response (len:%v)----------------------", len(rspbuff))
	rspmsg := &bbproto.RspEvolveDone{}
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

	uid := uint32(141)
	questId := uint32(2202)
	//helperUid := uint32(120)

	//EvolveStart(uid, questId, helperUid)
	EvolveDone(uid, questId, 12930)

	log.Fatal("bbsvr test client finish.")
}
