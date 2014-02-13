package quest

import (
	"fmt"
	"log"
	//"time"
)

import (
	"../bbproto"
	"../common"
	"../common/Error"
	"../const"
	"../data"
	//"../user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

func GetQuestInfo(db *data.Data, stageInfo *bbproto.StageInfo, questId uint32) (questInfo *bbproto.QuestInfo, e Error.Error) {
	if db == nil || stageInfo == nil {
		return nil, Error.New(cs.INVALID_PARAMS, "[ERROR] db pointer or stageInfo is nil.")
	}

	for k, quest := range stageInfo.Quests {
		log.Printf("[LOOP] Trace [%v] quest.Id:%v", k, *quest.Id)
		if *quest.Id == questId {
			return quest, Error.OK()
		}
	}

	return nil, Error.New(cs.EQ_QUEST_ID_INVALID, fmt.Sprintf("invalid questId: %v", questId))
}

func GetStageInfo(db *data.Data, stageId uint32) (stageInfo bbproto.StageInfo, e Error.Error) {
	if db == nil {
		return stageInfo, Error.New(cs.INVALID_PARAMS, "[ERROR] db pointer is nil.")
	}

	log.Printf("begin get stageInfo: %v", stageId)

	zStageInfo, err := db.Gets(cs.X_QUEST_STAGE + common.Utoa(stageId))
	if err != nil {
		return stageInfo, Error.New(cs.READ_DB_ERROR, "read stageinfo fail")
	}

	if err = proto.Unmarshal(zStageInfo, &stageInfo); err != nil {
		log.Printf("[ERROR] unmarshal error from stage[%v] info.", stageId)
		return stageInfo, Error.New(cs.UNMARSHAL_ERROR, "unmarshal error.")
	}

	log.Printf("stageInfo[%v]: %+v", stageId, stageInfo)

	return stageInfo, Error.OK()
}

func GetQuestConfig(db *data.Data, questId uint32) (config bbproto.QuestConfig, e Error.Error) {
	if db == nil {
		return config, Error.New(cs.INVALID_PARAMS, "[ERROR] db pointer is nil.")
	}

	zQuestConf, err := db.Gets(cs.X_QUEST_CONFIG + common.Utoa(questId))
	if err != nil {
		return config, Error.New(cs.EQ_GET_QUEST_CONFIG_ERROR, "get quest config fail")
	}

	if err = proto.Unmarshal(zQuestConf, &config); err != nil {
		log.Printf("[ERROR] unmarshal error from questConfig[%v].", questId)
		return config, Error.New(cs.UNMARSHAL_ERROR, "unmarshal error.")
	}

	return config, Error.OK()
}

//update tRecover, userStamina
func UpdateStamina(tRecover *uint32, userStamina *int32, userStaminaMax int32, questStamina int32) (e Error.Error) {
	if tRecover == nil || userStamina == nil {
		return Error.New(cs.INVALID_PARAMS, "invalid params")
	}

	tNow := common.Now()
	tElapse := int32(tNow - *tRecover)
	log.Printf("Old Stamina:%v tRecover:%v tElapse:%v questStamina:%v", userStamina, *tRecover, tElapse, questStamina)
	*userStamina += (tElapse/cs.N_STAMINA_TIME + 1)
	log.Printf("Now Stamina:%v userStaminaMax:%v", *userStamina, userStaminaMax)

	if *userStamina > userStaminaMax {
		*userStamina = userStaminaMax
	}

	*tRecover = tNow + uint32(cs.N_STAMINA_TIME-tElapse%cs.N_STAMINA_TIME)

	if *userStamina < questStamina {
		return Error.New(cs.EQ_STAMINA_NOT_ENOUGH, "stamina is not enough")
	}
	//log.Printf("staminaNow:%+v", *userdetail.User.StaminaNow)
	return Error.OK()
}

type TUsedValue struct {
	Value int32
	Used  bool
}

func MakeQuestData(config *bbproto.QuestConfig) (questData bbproto.QuestDungeonData, e Error.Error) {
	dungeonData := &bbproto.QuestDungeonData{}
	//dungeonData.Floors := make([]QuestFloor, cs.N_DUNGEON_GRID_COUNT-1)
	log.Printf("[TRACE] original dungeonData.Floors is:%+v", dungeonData.Floors)

	//dungeonData.Floors = make([]*bbproto.QuestFloor, len(config.Floors))
	//log.Printf("[TRACE] make %v floors is:%+v", floorNum, dungeonData.Floors)
	//floorNum := len(config.Floors)

	dungeonData.QuestId = config.QuestId
	dungeonData.Boss = config.Boss
	dungeonData.Enemys = config.Enemys
	dungeonData.QuestId = config.QuestId
	dungeonData.Colors = config.Colors

	//generate floor's grid data
	starList := make(map[int32]TUsedValue, cs.N_DUNGEON_GRID_COUNT-1)
	typeList := make(map[int32]TUsedValue, cs.N_DUNGEON_GRID_COUNT-1)

	for k, floorConf := range config.Floors {
		questFloor := &bbproto.QuestFloor{}

		currNum := int32(0)
		for n := currNum; n < *floorConf.TreasureNum; n++ {
			typeList[n] = TUsedValue{int32(bbproto.EQuestGridType_Q_TREATURE), false}
		}
		currNum += *floorConf.TreasureNum

		for n := currNum; n < currNum+(*floorConf.TrapNum); n++ {
			typeList[n] = TUsedValue{int32(bbproto.EQuestGridType_Q_TRAP), false}
		}
		currNum += *floorConf.TrapNum

		for n := currNum; n < currNum+(*floorConf.EnemyNum); n++ {
			typeList[n] = TUsedValue{int32(bbproto.EQuestGridType_Q_ENEMY), false}
		}
		currNum += *floorConf.EnemyNum
		log.Printf("[TRACE]  typeList len= %v | %v", currNum, len(typeList))

		starNum := int32(0)
		for i, star := range floorConf.Stars {
			log.Printf("[TRACE] floorConf.Stars[%v]: %+v", i, star)
			for n := starNum; n < starNum+(*star.Repeat); n++ {
				starList[n] = TUsedValue{int32(*star.Star), false}
			}
			starNum += *star.Repeat
		}
		log.Printf("[TRACE]  starList len= %v | %v", starNum, len(starList))

		gridCount := int32(cs.N_DUNGEON_GRID_COUNT - 1)
		for i := 0; i < cs.N_DUNGEON_GRID_COUNT-1; i++ {
			grid := &bbproto.QuestGrid{}

			randNum := common.Rand(0, gridCount-1)
			nStar := starList[randNum].Value
			var starConf *bbproto.StarConfig
			for _, conf := range floorConf.Stars {
				if *starConf.Star == nStar {
					starConf = conf
					break
				}
			}

			if typeList[randNum].Value == int32(bbproto.EQuestGridType_Q_TREATURE) {
				tmpType := bbproto.EQuestGridType_Q_TREATURE
				grid.Type = &tmpType
				grid.Coins = proto.Int32(common.Rand(*starConf.Coin.Min, *starConf.Coin.Max))
			} else if typeList[randNum].Value == int32(bbproto.EQuestGridType_Q_TRAP) {
				tmpType := bbproto.EQuestGridType_Q_TRAP
				grid.Type = &tmpType

				randn := common.Randn(int32(len(starConf.Trap)))
				grid.TrapId = proto.Uint32(starConf.Trap[randn])
				log.Printf("[TRACE] randn:%v -> trapId: %v", randn, starConf.Trap[randn])

			} else if typeList[randNum].Value == int32(bbproto.EQuestGridType_Q_ENEMY) {
				tmpType := bbproto.EQuestGridType_Q_ENEMY
				grid.Type = &tmpType

				randn := common.Rand(*starConf.EnemyNum.Min, *starConf.EnemyNum.Max)
				log.Printf("[TRACE] randnum:%v enemys: ", randn)
				for x := int32(0); x < randn; x++ {
					nn := int32(len(starConf.EnemyPool))
					nn = common.Randn(nn) % (nn - 1)

					grid.EnemyId = append(grid.EnemyId, starConf.EnemyPool[nn])
					log.Printf("[TRACE] randnn:%v enemyId:%v ", nn, starConf.EnemyPool[nn])
				}
			}

			questFloor.GridInfo = append(questFloor.GridInfo, grid)

			delete(typeList, randNum)
			delete(starList, randNum)
			gridCount -= 1
		}

		log.Printf("floor[%v]: questFloor.GridInfo:%+v floorConf:%+v", k, questFloor.GridInfo, floorConf)

		dungeonData.Floors = append(dungeonData.Floors, questFloor)
	}

	return
}
