package quest

import (
	"fmt"
	//"time"
)

import (
	"../bbproto"
	"../common"
	"../common/Error"
	"../common/log"
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
		log.T("[LOOP] Trace [%v] quest.Id:%v", k, *quest.Id)
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

	log.T("begin get stageInfo: %v", stageId)

	zStageInfo, err := db.Gets(cs.X_QUEST_STAGE + common.Utoa(stageId))
	if err != nil {
		return stageInfo, Error.New(cs.READ_DB_ERROR, "read stageinfo fail")
	}

	if err = proto.Unmarshal(zStageInfo, &stageInfo); err != nil {
		log.T("[ERROR] unmarshal error from stage[%v] info.", stageId)
		return stageInfo, Error.New(cs.UNMARSHAL_ERROR, "unmarshal error.")
	}

	log.T("stageInfo[%v]: %+v", stageId, stageInfo)

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
		log.T("[ERROR] unmarshal error from questConfig[%v].", questId)
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
	log.T("Old Stamina:%v tRecover:%v tElapse:%v questStamina:%v", userStamina, *tRecover, tElapse, questStamina)
	*userStamina += (tElapse/cs.N_STAMINA_TIME + 1)
	log.T("Now Stamina:%v userStaminaMax:%v", *userStamina, userStaminaMax)

	if *userStamina > userStaminaMax {
		*userStamina = userStaminaMax
	}

	*tRecover = tNow + uint32(cs.N_STAMINA_TIME-tElapse%cs.N_STAMINA_TIME)

	if *userStamina < questStamina {
		return Error.New(cs.EQ_STAMINA_NOT_ENOUGH, "stamina is not enough")
	}
	//log.T("staminaNow:%+v", *userdetail.User.StaminaNow)
	return Error.OK()
}

type TUsedValue struct {
	Value int32
	Used  bool
}

//retrieve target item from list
func getItemFromList(pos int32, list *map[int32]TUsedValue) (result int32) {
	result = -1
	if pos >= int32(len(*list)) {
		return result
	}

	index := int32(0)
	for k, ss := range *list {
		//log.T("LOOP list[%v]: %+v ...  Now index=%v ", k, ss, index)
		if ss.Used {
			//log.T("==> list[%v] is used.", k)
			continue
		}

		if index == pos {
			(*list)[k] = TUsedValue{ss.Value, true}
			result = ss.Value
			log.T("LOOP list[%v]: found ss:%+v list[k]:%+v", k, ss, (*list)[k])
			log.T("\t==> all list: %+v", k, ss, (*list))
			break
		}
		index++
	}
	return result
}

func getStarConf(iStar int32, starsConf []*bbproto.StarConfig) (starConf *bbproto.StarConfig, e Error.Error) {
	for i, conf := range starsConf {
		log.T("== getStarConf : loop[%v]:%+v", i, conf)
		if conf.Star != nil && int32(*conf.Star) == iStar {
			starConf = conf
			log.T("found matched starConf[%+v]", iStar)
			return starConf, Error.OK()
		}
	}

	return nil, Error.New(cs.DATA_NOT_EXISTS, "Not found starconf.")
}

func fillGridsList(typeList map[int32]TUsedValue, starList map[int32]TUsedValue, floorConf *bbproto.QuestFloorConfig) Error.Error {
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

	for n := currNum; n < cs.N_DUNGEON_GRID_COUNT-1; n++ {
		typeList[n] = TUsedValue{int32(bbproto.EQuestGridType_Q_NONE), true}
	}
	log.T("typeList: len(%v | %v): %v", currNum, len(typeList), typeList)

	starNum := int32(0)
	for i, star := range floorConf.Stars {
		log.T(" floorConf.Stars[%v]: %+v", i, star)
		for n := starNum; n < starNum+(*star.Repeat); n++ {
			starList[n] = TUsedValue{int32(*star.Star), false}
		}
		starNum += *star.Repeat
	}
	for n := starNum; n < cs.N_DUNGEON_GRID_COUNT-1; n++ {
		starList[n] = TUsedValue{int32(bbproto.EGridStar_GS_EMPTY), true}
	}

	log.T("starList (len:%v | %v): %v", starNum, len(starList), starList)

	return Error.OK()
}

func MakeQuestData(config *bbproto.QuestConfig) (questData bbproto.QuestDungeonData, e Error.Error) {
	log.SetFlags(log.Ltime | log.Lmicroseconds | log.Lshortfile)

	questData.QuestId = config.QuestId
	questData.QuestId = config.QuestId
	questData.Colors = config.Colors

	for _, bossConf := range config.Boss {
		questData.Boss = append(questData.Boss, bossConf.Enemy)
	}
	for _, enemyConf := range config.Enemys {
		questData.Enemys = append(questData.Enemys, enemyConf.Enemy)
	}

	//generate floor's grid data
	starList := make(map[int32]TUsedValue, cs.N_DUNGEON_GRID_COUNT-1)
	typeList := make(map[int32]TUsedValue, cs.N_DUNGEON_GRID_COUNT-1)

	for k, floorConf := range config.Floors {
		questFloor := &bbproto.QuestFloor{}

		log.T("--pre typeList:%+v", typeList)

		fillGridsList(typeList, starList, floorConf)
		log.T("--after typeList:%+v", typeList)

		gridCount := int32(cs.N_DUNGEON_GRID_COUNT - 1)
		for i := 0; i < cs.N_DUNGEON_GRID_COUNT-1 && gridCount > 0; i++ {
			grid := &bbproto.QuestGrid{}
			grid.Position = proto.Int32(int32(i))

			randPos := common.Randn(gridCount)
			gridCount -= 1

			//retrieve target item from list
			iType := getItemFromList(randPos, &typeList)
			iStar := getItemFromList(randPos, &starList)

			log.T("--[GRID-%v] randPos:%v iType:%v, iStar:%v,\n floorConf.Stars:%+v",
				i, randPos, iType, iStar, floorConf.Stars)
			if iType < 0 || iStar < 0 {
				//it should be empty grid
				questFloor.GridInfo = append(questFloor.GridInfo, grid)
				continue
			}

			starConf, e := getStarConf(iStar, floorConf.Stars)
			if e.IsError() {
				log.Error("getStarConf[%v] ret err:%v ", iStar, e.Error())
				return questData, e //unreachable here
			}
			log.T("After found starConf[%v] ret err:%v,  starConf:%+v ", iStar, e.Error(), starConf)

			grid.Star = starConf.Star
			grid.Color = proto.Int32(common.Randn(3)) //0:red,1:yellow,2:blue

			if iType == int32(bbproto.EQuestGridType_Q_TREATURE) {
				tmpType := bbproto.EQuestGridType_Q_TREATURE
				grid.Type = &tmpType
				grid.Coins = proto.Int32(common.Rand(*starConf.Coin.Min, *starConf.Coin.Max))
				log.T("random ret treasure, grid: %+v", grid)
			} else if iType == int32(bbproto.EQuestGridType_Q_TRAP) {
				tmpType := bbproto.EQuestGridType_Q_TRAP
				grid.Type = &tmpType

				randn := common.Randn(int32(len(starConf.Trap)))
				grid.TrapId = proto.Uint32(starConf.Trap[randn])
				log.T(" randn:%v -> trapId: %v", randn, starConf.Trap[randn])

			} else if iType == int32(bbproto.EQuestGridType_Q_ENEMY) {
				tmpType := bbproto.EQuestGridType_Q_ENEMY
				grid.Type = &tmpType
				log.T("EnemyNum: %v %v", *starConf.EnemyNum.Min, *starConf.EnemyNum.Max)

				randn := common.Rand(*starConf.EnemyNum.Min, *starConf.EnemyNum.Max)
				log.T("randn:%v enemys: ", randn)
				for x := int32(0); x < randn; x++ {
					nn := int32(len(starConf.EnemyPool))
					nn = common.Randn(nn) % (nn - 1)

					grid.EnemyId = append(grid.EnemyId, starConf.EnemyPool[nn])
					log.T("\t--randnn:%v enemyId:%v ", nn, starConf.EnemyPool[nn])
				}
			} else {
				log.Warn("Random ret grid type is Empty. (randPos: %v iType:%v)", randPos, iType)
			}

			questFloor.GridInfo = append(questFloor.GridInfo, grid)
			log.T("===[GRID-%v]: %+v", i, grid)
		}

		log.T("starList: %+v", starList)
		log.T("typeList: %+v", typeList)

		log.T("floor[%v]: questFloor.GridInfo:%+v floorConf:%+v", k, questFloor.GridInfo, floorConf)

		questData.Floors = append(questData.Floors, questFloor)
	}

	return questData, Error.OK()
}
