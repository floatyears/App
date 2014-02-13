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

func MakeQuestData(config *bbproto.QuestConfig) (questData bbproto.QuestDungeonData, e Error.Error) {
	dungeonData := &bbproto.QuestDungeonData{}
	//dungeonData.Floors := make([]QuestFloor, cs.N_DUNGEON_GRID_COUNT-1)
	log.Printf("original dungeonData.Floors is:%+v", dungeonData.Floors)

	dungeonData.Floors = make([]*bbproto.QuestFloor, len(config.Floors))
	log.Printf("make %v floors is:%+v", len(config.Floors), dungeonData.Floors)

	dungeonData.Boss = config.Boss
	dungeonData.Enemys = config.Enemys

	//for k, floorConf := range config.Floors {

	//	//floorConf.TreasureNum
	//}

	return
}
