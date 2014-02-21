package quest

import (
	"fmt"
	//"time"
	//"container/list"
)

import (
	"../bbproto"
	"../common"
	"../common/Error"
	"../common/log"
	"../const"
	"../data"
	"../unit"
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

func GetStageInfo(db *data.Data, stageId uint32) (stageInfo *bbproto.StageInfo, e Error.Error) {
	if db == nil {
		return stageInfo, Error.New(cs.INVALID_PARAMS, "[ERROR] db pointer is nil.")
	}

	log.T("begin get stageInfo: %v", stageId)

	zStageInfo, err := db.Gets(cs.X_QUEST_STAGE + common.Utoa(stageId))
	if err != nil {
		return stageInfo, Error.New(cs.READ_DB_ERROR, "read stageinfo fail")
	}

	stageInfo = &bbproto.StageInfo{}
	if err = proto.Unmarshal(zStageInfo, stageInfo); err != nil {
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
func RefreshStamina(tRecover *uint32, userStamina *int32, userStaminaMax int32) (e Error.Error) {
	if tRecover == nil || userStamina == nil {
		return Error.New(cs.INVALID_PARAMS, "invalid params")
	}

	tNow := common.Now()
	tElapse := int32(tNow - *tRecover)
	log.T("Old Stamina:%v tRecover:%v tElapse:%v ", userStamina, *tRecover, tElapse)
	*userStamina += (tElapse/cs.N_STAMINA_TIME + 1)
	log.T("Now Stamina:%v userStaminaMax:%v", *userStamina, userStaminaMax)

	if *userStamina > userStaminaMax {
		*userStamina = userStaminaMax
	}

	*tRecover = tNow + uint32(cs.N_STAMINA_TIME-tElapse%cs.N_STAMINA_TIME)

	return Error.OK()
}

type TUsedValue struct {
	Value int32
	Used  bool
}

//get quest record from QuestLog, fill to userDetail.Quest
func GetQuestRecord(db *data.Data, questId uint32, userDetail *bbproto.UserInfoDetail) (e Error.Error) {
	if db == nil {
		return Error.New(cs.INVALID_PARAMS, "invalid db pointer")
	}

	if err := db.Select(cs.TABLE_QUEST_LOG); err != nil {
		return Error.New(cs.SET_DB_ERROR, err.Error())
	}

	var value []byte
	uid := *userDetail.User.UserId
	value, err := db.HGet(cs.X_QUEST_LOG+common.Utoa(uid), common.Utoa(questId))
	if err != nil {
		log.Printf("[ERROR] GetQuestRecord for '%v' ret err:%v", uid, err)
		return Error.New(cs.READ_DB_ERROR, "read quest log fail")
	}

	if len(value) == 0 {
		return Error.OK() //no records
	}

	userDetail.Quest = &bbproto.QuestRecord{}
	err = proto.Unmarshal(value, userDetail.Quest)
	if err != nil {
		return Error.New(cs.UNMARSHAL_ERROR)
	}

	return Error.OK()
}

func UpdateQuestRecord(db *data.Data, userDetail *bbproto.UserInfoDetail, questId uint32, getUnit []*bbproto.DropUnit, getMoney int32) (e Error.Error) {
	if db == nil {
		return Error.New(cs.INVALID_PARAMS, "invalid db pointer")
	}

	uid := *userDetail.User.UserId
	if userDetail.Quest == nil {
		return Error.New(cs.EQ_UPDATE_QUEST_RECORD_ERROR, "user.Quest is nil")
	}

	userDetail.Quest.EndTime = proto.Uint32(common.Now())

	//TODO: verity getMoney
	*userDetail.Quest.GetMoney += getMoney

	//verify getUnit
	isAllValidUnit := true
	for _, unitGot := range getUnit {
		isValidOne := false
		for _, unitDrop := range userDetail.Quest.DropUnits {
			if *unitDrop.DropId == *unitGot.DropId && *unitDrop.UnitId == *unitGot.UnitId {
				isValidOne = true
				break
			}
		}
		if !isValidOne {
			log.Error("ClearQuest :: unitGot is invalid: %+v", unitGot)
			isAllValidUnit = false
			break
		}
	}

	if !isAllValidUnit {
		log.Error("clear request: invalid drop unit.")
		return Error.New(cs.EQ_INVALID_DROP_UNIT, "clear request: invalid drop unit")
	}

	//add unit to userinfo
	for _, unitDrop := range userDetail.Quest.DropUnits {
		uniqueId, e := unit.GetUnitUniqueId(db, userDetail)
		if e.IsError() {
			return e
		}

		userUnit := &bbproto.UserUnit{}
		userUnit.UniqueId = proto.Uint32(uniqueId)
		userUnit.UnitId = unitDrop.UnitId
		userUnit.Level = unitDrop.Level
		userUnit.AddHp = unitDrop.AddHp
		userUnit.AddAttack = unitDrop.AddAttack
		userUnit.AddDefence = unitDrop.AddDefence
		userUnit.GetTime = proto.Uint32(common.Now())

		userDetail.UnitList = append(userDetail.UnitList, userUnit)
		userDetail.Quest.GetUnit = append(userDetail.Quest.GetUnit, userUnit)
	}

	//already fill in getUnit, so empty dropUnit before save to QuestLog
	userDetail.Quest.DropUnits = []*bbproto.DropUnit{}

	//save userDetail.Quest to QuestLog
	zQuest, err := proto.Marshal(userDetail.Quest)
	if err != nil {
		return Error.New(cs.MARSHAL_ERROR)
	}

	if err := db.Select(cs.TABLE_QUEST_LOG); err != nil {
		return Error.New(cs.SET_DB_ERROR, err.Error())
	}
	if err = db.HSet(cs.X_QUEST_LOG+common.Utoa(uid), common.Utoa(questId), zQuest); err != nil {
		log.Error("HSet(X_QUEST_LOG_%v, %v) failed:%v.", uid, questId, err)
		return Error.New(cs.SET_DB_ERROR)
	}

	//clear userDetail.Quest, then save userDetail
	*userDetail.User.Exp += *userDetail.Quest.GetExp
	*userDetail.Account.Money += (*userDetail.Quest.GetMoney)
	log.T("==Account :: addMoney:%v -> %v addExp:%v -> %v", *userDetail.Quest.GetMoney, *userDetail.Account.Money, *userDetail.Quest.GetExp, *userDetail.User.Exp)
	userDetail.Quest = nil

	return Error.OK()
}

func FillQuestRecord(userDetail *bbproto.UserInfoDetail, questId uint32, drops []*bbproto.DropUnit,
	stage *bbproto.StageInfo, quest *bbproto.QuestInfo) (e Error.Error) {
	if userDetail.Quest == nil {
		userDetail.Quest = &bbproto.QuestRecord{}
		userDetail.Quest.QuestId = proto.Uint32(questId)
		userDetail.Quest.StartTime = proto.Uint32(common.Now())
		//userDetail.Quest.EndTime = proto.Uint32(common.Now())
	}

	//fill getExp, getMoney
	getExp := *quest.RewardExp
	getMoney := *quest.RewardMoney
	if *stage.Boost.Type == bbproto.QuestBoostType_QB_BOOST_MONEY {
		log.T("boost money: %v x%v", getMoney, *stage.Boost.Value)
		getMoney *= *stage.Boost.Value
	}
	if *stage.Boost.Type == bbproto.QuestBoostType_QB_BOOST_EXP {
		log.T("boost Exp: %v x%v", getExp, *stage.Boost.Value)
		getExp *= *stage.Boost.Value
	}
	userDetail.Quest.GetExp = proto.Int32(getExp)
	userDetail.Quest.GetMoney = proto.Int32(getMoney)

	state := bbproto.EQuestState_QS_QUESTING
	userDetail.Quest.State = &state

	//fill drop unit
	for _, dropUnit := range drops {
		if dropUnit != nil {
			//userunit := &bbproto.UserUnit{}
			//userunit.UniqueId = proto.Uint32(0)
			//userunit.UnitId = dropUnit.UnitId
			//userunit.Level = dropUnit.Level
			//userunit.AddHp = dropUnit.AddHp
			//userunit.AddAttack = dropUnit.AddAttack
			//userunit.AddDefence = dropUnit.AddDefence

			userDetail.Quest.DropUnits = append(userDetail.Quest.DropUnits, dropUnit)
		}
	}

	return Error.OK()
}
