package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	//"errors"
	_ "html"

	"math/rand"
	//"time"
)
import (
	"bbproto"
	"common"
	"common/consts"
	"common/log"
	"data"
	//"model/unit"
	//"src/quest"
	//"src/model/user"
	//"src/friend"
	//redis "github.com/garyburd/redigo/redis"
)

func DataAddQuestConfig(questId uint32) error {

	conf := &bbproto.QuestConfig{}
	conf.QuestId = proto.Uint32(questId)

	boss := &bbproto.EnemyInfoConf{}
	enemy := &bbproto.EnemyInfo{}

	enemy.EnemyId = proto.Uint32(1001)
	enemy.UnitId = proto.Uint32(2)
	unitType := bbproto.EUnitType_UFIRE
	enemy.Type = &unitType
	enemy.Hp = proto.Int32(1200)
	enemy.Attack = proto.Int32(300)
	enemy.Defense = proto.Int32(100)
	enemy.NextAttack = proto.Int32(2)
	boss.Enemy = enemy
	boss.DropUnitId = proto.Uint32(11)
	boss.DropUnitLevel = proto.Int32(1)
	boss.DropRate = proto.Float32(0.5)
	boss.AddHpRate = proto.Float32(0.1)
	boss.AddAttackRate = proto.Float32(0.1)

	conf.Boss = append(conf.Boss, boss)

	for i := 1; i <= 5; i++ {
		enemyConf := &bbproto.EnemyInfoConf{}
		enemy := &bbproto.EnemyInfo{}

		enemy.EnemyId = proto.Uint32(uint32(900 + i))
		enemy.UnitId = proto.Uint32(2 + uint32(i))
		enemy.Type = &unitType
		enemy.Hp = proto.Int32(1000 + int32(rand.Intn(100)*10))
		enemy.Attack = proto.Int32(300 + int32(rand.Intn(10)*10))
		enemy.Defense = proto.Int32(100 + int32(rand.Intn(10)*10))
		enemy.NextAttack = proto.Int32(int32(1 + rand.Intn(2)))

		enemyConf.Enemy = enemy
		enemyConf.DropUnitId = proto.Uint32(*enemy.UnitId)
		enemyConf.DropUnitLevel = proto.Int32(1)
		enemyConf.DropRate = proto.Float32(0.1 * float32(1+rand.Intn(9)))
		enemyConf.AddHpRate = proto.Float32(0.1)
		enemyConf.AddAttackRate = proto.Float32(0.1)
		//enemy.AddDefence = proto.Float32(0)

		conf.Enemys = append(conf.Enemys, enemyConf)
	}

	//fill block color
	for n := 1; n <= 7; n++ {
		color := &bbproto.ColorPercent{}
		unitType := bbproto.EUnitType(n)
		color.Color = &unitType
		if unitType == bbproto.EUnitType_UHeart { //
			color.Percent = proto.Float32(0.16)
		} else if unitType == bbproto.EUnitType_UWIND { //
			color.Percent = proto.Float32(0.1)
		} else if unitType == bbproto.EUnitType_UFIRE { //
			color.Percent = proto.Float32(0.1)
		} else if unitType == bbproto.EUnitType_UWATER { //
			color.Percent = proto.Float32(0.3)
		} else if unitType == bbproto.EUnitType_ULIGHT { //
			color.Percent = proto.Float32(0.04)
		} else {
			color.Percent = proto.Float32(0.14)
		}
		conf.Colors = append(conf.Colors, color)
	}

	//fill QuestFloorConfig
	floor1 := &bbproto.QuestFloorConfig{}
	floor1.TreasureNum = proto.Int32(4)
	floor1.TrapNum = proto.Int32(8)
	floor1.EnemyNum = proto.Int32(10)

	star1 := bbproto.EGridStar_GS_STAR_1
	//star2 := bbproto.EGridStar_GS_STAR_2
	star3 := bbproto.EGridStar_GS_STAR_3
	//star4 := bbproto.EGridStar_GS_STAR_4
	star5 := bbproto.EGridStar_GS_STAR_5
	//star6 := bbproto.EGridStar_GS_STAR_6
	starKey := bbproto.EGridStar_GS_KEY
	starQ := bbproto.EGridStar_GS_QUESTION
	starX := bbproto.EGridStar_GS_EXCLAMATION

	star1Conf := &bbproto.StarConfig{}
	star1Conf.Star = &star1
	star1Conf.Repeat = proto.Int32(5)
	star1Conf.Coin = &bbproto.NumRange{proto.Int32(300), proto.Int32(600), nil}
	star1Conf.EnemyNum = &bbproto.NumRange{proto.Int32(1), proto.Int32(2), nil}
	star1Conf.EnemyPool = []uint32{901, 902, 903}
	star1Conf.Trap = []uint32{2, 3, 4} //trapId
	//star1Conf.Trap = []uint32{5}

	star2Conf := &bbproto.StarConfig{}
	star2Conf.Star = &star3
	star2Conf.Repeat = proto.Int32(6)
	star2Conf.Coin = &bbproto.NumRange{proto.Int32(700), proto.Int32(900), nil}
	star2Conf.EnemyNum = &bbproto.NumRange{proto.Int32(2), proto.Int32(4), nil}
	star2Conf.EnemyPool = []uint32{901, 902, 903, 904}
	star2Conf.Trap = []uint32{3, 5} //trapId
	//star2Conf.Trap = []uint32{5}

	star3Conf := &bbproto.StarConfig{}
	star3Conf.Star = &star5
	star3Conf.Repeat = proto.Int32(6)
	star3Conf.Coin = &bbproto.NumRange{proto.Int32(2000), proto.Int32(3000), nil}
	star3Conf.EnemyNum = &bbproto.NumRange{proto.Int32(3), proto.Int32(4), nil}
	star3Conf.EnemyPool = []uint32{902, 903, 904, 905}
	star3Conf.Trap = []uint32{1, 2, 5} //trapId
	//star3Conf.Trap = []uint32{5}

	star0Conf := &bbproto.StarConfig{}
	star0Conf.Star = &starKey
	star0Conf.Repeat = proto.Int32(1)
	star0Conf.Coin = &bbproto.NumRange{proto.Int32(2000), proto.Int32(3000), nil}
	star0Conf.EnemyNum = &bbproto.NumRange{proto.Int32(3), proto.Int32(4), nil}
	star0Conf.EnemyPool = []uint32{902, 903, 904, 905}
	star0Conf.Trap = []uint32{1, 2, 4, 5, 6} //trapId
	//star0Conf.Trap = []uint32{5}

	starQConf := &bbproto.StarConfig{}
	starQConf.Star = &starQ
	starQConf.Repeat = proto.Int32(3)
	starQConf.Coin = &bbproto.NumRange{proto.Int32(3000), proto.Int32(4000), nil}
	starQConf.EnemyNum = &bbproto.NumRange{proto.Int32(5), proto.Int32(5), nil}
	starQConf.EnemyPool = []uint32{901, 902, 903, 904, 905}
	starQConf.Trap = []uint32{2, 4, 5, 6} //trapId
	//starQConf.Trap = []uint32{5}

	star_Conf := &bbproto.StarConfig{}
	star_Conf.Star = &starX
	star_Conf.Repeat = proto.Int32(1)
	star_Conf.Coin = &bbproto.NumRange{proto.Int32(3000), proto.Int32(4000), nil}
	star_Conf.EnemyNum = &bbproto.NumRange{proto.Int32(5), proto.Int32(5), nil}
	star_Conf.EnemyPool = []uint32{901, 902, 903, 904, 905}
	star_Conf.Trap = []uint32{2, 4, 5} //trapId
	//star_Conf.Trap = []uint32{5}

	floor1.Stars = append(floor1.Stars, star1Conf)
	floor1.Stars = append(floor1.Stars, star2Conf)
	floor1.Stars = append(floor1.Stars, star3Conf)
	floor1.Stars = append(floor1.Stars, star0Conf) // key
	floor1.Stars = append(floor1.Stars, starQConf) // ?
	floor1.Stars = append(floor1.Stars, star_Conf) // !

	conf.Floors = append(conf.Floors, floor1)

	log.Printf("QuestConfig: %+v", conf)

	db := &data.Data{}
	err := db.Open(string(consts.TABLE_QUEST))
	if err != nil {
		return err
	}
	defer db.Close()

	zData, err := proto.Marshal(conf)
	if err != nil {
		log.Printf("unmarshal error.")
		return err
	}
	if err = db.Set(consts.X_QUEST_CONFIG+common.Utoa(questId), zData); err != nil {
		return err
	}

	return err
}

func DataAddStageInfo(stageId uint32, stageName string) (stageInfo *bbproto.StageInfo, err error) {
	db := &data.Data{}
	err = db.Open(string(consts.TABLE_QUEST))
	if err != nil {
		return nil, err
	}
	defer db.Close()

	state := bbproto.EQuestState_QS_NEW

	stageInfo = &bbproto.StageInfo{}
	stageInfo.Version = proto.Int(1)
	stageInfo.Id = proto.Uint32(stageId)
	stageInfo.State = &state
	stageInfo.Type = proto.Int(1) // story or event
	stageInfo.StageName = proto.String(stageName)
	stageInfo.Description = proto.String("it is :" + stageName)
	stageInfo.StartTime = proto.Uint32(0)
	stageInfo.EndTime = proto.Uint32(0)
	boostType := bbproto.QuestBoostType_QB_BOOST_MONEY
	stageInfo.Boost = new(bbproto.QuestBoost)
	stageInfo.Boost.Type = &boostType //coins , exp , dropRate
	stageInfo.Boost.Value = proto.Int(2)
	//optional Position	pos				= &pos; // stage position of the city

	for i := 1; i <= 5; i++ {
		qusetInfo := &bbproto.QuestInfo{}
		qusetInfo.Id = proto.Uint32(100*stageId + uint32(i))
		qusetInfo.State = &state
		qusetInfo.No = proto.Int32(1)
		qusetInfo.Name = proto.String("quest name" + common.Itoa(i))   // quest name
		qusetInfo.Story = proto.String("it is quest" + common.Itoa(i)) // story description
		qusetInfo.Stamina = proto.Int32(5)                             // cost stamina
		qusetInfo.Floor = proto.Int32(2)
		qusetInfo.RewardExp = proto.Int32(100)
		qusetInfo.RewardMoney = proto.Int32(2000)
		for b := 1; b <= 3; b++ {
			qusetInfo.BossId = append(qusetInfo.BossId, uint32(1000+b))
		}
		for b := 1; b <= 5; b++ {
			qusetInfo.EnemyId = append(qusetInfo.EnemyId, uint32(900+b))
		}

		stageInfo.Quests = append(stageInfo.Quests, qusetInfo)
	}
	log.Printf("====stageInfo: %+vin", stageInfo)
	for k, q := range stageInfo.Quests {
		log.Printf("   --- quest[%v]: %+v", k, q)
	}

	zStageInfo, err := proto.Marshal(stageInfo)
	if err != nil {
		log.Printf("unmarshal error.")
		return nil, err
	}

	err = db.Set(consts.X_QUEST_STAGE+common.Utoa(stageId), zStageInfo)
	if err != nil {
		return nil, err
	}

	return stageInfo, err
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

func DataAddEvolveCity() (err error) {
	CITY_ID := uint32(100)
	evolveCity := &bbproto.CityInfo{}
	//evolveCity.Pos = proto.Int32(100)
	evolveCity.Id = proto.Uint32(CITY_ID)
	evolveCity.CityName = proto.String("EvolveCity")

	stageNum := uint32(6)
	questNum := uint32(5)
	stageNames := []string{"Wind", "Fire", "Water", "Light", "Dark", "NONE"}

	for k := uint32(0); k < stageNum; k++ {
		stageId := uint32(CITY_ID + k + 1)
		stage, err := DataAddStageInfo(stageId, "Evolve "+stageNames[k]+" Stage")
		if err != nil {
			return err
		}
		evolveCity.Stages = append(evolveCity.Stages, stage)

		//Add Quest Config
		for questId := uint32(CITY_ID*stageId + 1); questId <= (CITY_ID*stageId + questNum); questId++ {
			log.T("Add Quest Config for stageId:%v questId:%v", stageId, questId)
			DataAddQuestConfig(questId)
		}

	}

	zCityData, err := proto.Marshal(evolveCity)
	if err != nil {
		log.Error("marshal error.")
		return err
	}

	err = common.WriteFile(zCityData, "./EvolveCity.pd")
	if err != nil {
		log.Error("WriteFile error.")
		return err
	}

	return err
}

func DataAddPrisonCity() (err error) {
	CITY_ID := uint32(1)
	city := &bbproto.CityInfo{}
	//city.Pos = proto.Int32(1)
	city.Id = proto.Uint32(CITY_ID)
	city.CityName = proto.String("PrisonCity")

	stageNum := uint32(7)
	questNum := uint32(5)
	stageNames := []string{"One", "Two", "Three", "Four", "Five", "Six", "Seven"}

	for k := uint32(0); k < stageNum; k++ {
		stageId := CITY_ID + k + 1
		stage, err := DataAddStageInfo(stageId, "Prison "+stageNames[k])
		if err != nil {
			return err
		}
		city.Stages = append(city.Stages, stage)

		//Add Quest Config
		for questId := uint32(CITY_ID*stageId + 1); questId <= CITY_ID*stageId+questNum; questId++ {
			log.T("Add Quest Config for stageId:%v questId:%v", stageId, questId)
			DataAddQuestConfig(questId)
		}

	}

	zCityData, err := proto.Marshal(city)
	if err != nil {
		log.Error("marshal error.")
		return err
	}

	err = common.WriteFile(zCityData, "./PrisonCity.pd")
	if err != nil {
		log.Error("WriteFile error.")
		return err
	}

	return err
}

func main() {
	log.Printf("==============================================")
	log.Printf("bbsvr test client begin...")

	Init()
	//DataAddStageInfo(11, "Fire City")
	//DataAddStageInfo(12, "Water City")
	//DataAddStageInfo(13, "Win City")
	//for stage := uint32(11); stage <= 13; stage++ {
	//	for questId := uint32(100*stage + 1); questId <= 100*stage+5; questId++ {
	//		DataAddQuestConfig(questId)
	//	}
	//}

	DataAddEvolveCity()
	DataAddPrisonCity()

	//StartQuest(101, 11, 1101, 102)

	log.Fatal("bbsvr test client finish.")
}
