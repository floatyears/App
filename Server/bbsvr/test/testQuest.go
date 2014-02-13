package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	_ "html"
	"log"
	//"math/rand"
	//"time"
)
import (
	"../src/bbproto"
	"../src/common"
	"../src/const"
	"../src/data"
	//"../src/quest"
	//"../src/user/usermanage"
	//"../src/friend"
	//redis "github.com/garyburd/redigo/redis"
)

func DataAddQuest(stageId uint32, stageName string) error {
	db := &data.Data{}
	err := db.Open(string(cs.TABLE_QUEST))
	if err != nil {
		return err
	}
	defer db.Close()

	stageInfo := &bbproto.StageInfo{}
	stageInfo.Version = proto.Int(1)
	stageInfo.Id = proto.Uint32(stageId)
	stageInfo.State = proto.Int(0)
	stageInfo.Type = proto.Int(1) // story or event
	stageInfo.StageName = proto.String(stageName)
	stageInfo.Description = proto.String("it is :" + stageName)
	stageInfo.StartTime = proto.Uint32(0)
	stageInfo.EndTime = proto.Uint32(0)
	stageInfo.BoostType = proto.Int(0) //coins , exp , dropRate
	//stageInfo.BoostValue		= nil
	//optional Position	pos				= &pos; // stage position of the city

	for i := 1; i <= 5; i++ {
		qusetInfo := &bbproto.QuestInfo{}
		qusetInfo.Id = proto.Uint32(100*stageId + uint32(i))
		qusetInfo.State = proto.Int32(0)
		qusetInfo.No = proto.Int32(1)
		qusetInfo.Name = proto.String("quest name" + common.Itoa(i))   // quest name
		qusetInfo.Story = proto.String("it is quest" + common.Itoa(i)) // story description
		qusetInfo.Stamina = proto.Int32(5)                             // cost stamina
		qusetInfo.Floor = proto.Int32(2)
		qusetInfo.RewardExp = proto.Int32(100)
		qusetInfo.RewardCoin = proto.Int32(2000)
		for b := 1; b <= 3; b++ {
			qusetInfo.BossId = append(qusetInfo.BossId, uint32(1000+b))
		}
		for b := 1; b <= 5; b++ {
			qusetInfo.EnemyId = append(qusetInfo.EnemyId, uint32(900+b))
		}

		stageInfo.Quests = append(stageInfo.Quests, qusetInfo)
	}
	log.Printf("====stageInfo: %+v", stageInfo)
	for k, q := range stageInfo.Quests {
		log.Printf("   --- quest[%v]: %+v", k, q)
	}

	zStageInfo, err := proto.Marshal(stageInfo)
	if err != nil {
		log.Printf("unmarshal error.")
		return err
	}

	err = db.Set(cs.X_QUEST_STAGE+common.Utoa(stageId), zStageInfo)
	if err != nil {
		return err
	}

	return err
}

func StartQuest(uid uint32, stageId uint32, questId uint32, helperUid uint32) error {
	msg := &bbproto.ReqStartQuest{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.QuestId = proto.Uint32(questId)
	msg.StageId = proto.Uint32(stageId)
	msg.HelperUserId = proto.Uint32(helperUid)
	//msg.HelperUnit =

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
	log.Printf("-----------------------Response----------------------")
	rspmsg := &bbproto.RspStartQuest{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	return err
}

func main() {
	log.Printf("==============================================")
	log.Printf("bbsvr test client begin...")

	Init()
	DataAddQuest(11, "Fire City")
	DataAddQuest(12, "Water City")
	DataAddQuest(13, "Win City")
	StartQuest(101, 11, 1101, 102)

	log.Fatal("bbsvr test client finish.")
}
