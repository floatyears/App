package quest

import (
	"fmt"
	//"time"
	//"container/list"
)

import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"model/unit"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

func GetQuestInfo(db *data.Data, stageInfo *bbproto.StageInfo, questId uint32) (questInfo *bbproto.QuestInfo, e Error.Error) {
	if db == nil || stageInfo == nil {
		return nil, Error.New(EC.INVALID_PARAMS, "[ERROR] db pointer or stageInfo is nil.")
	}

	for k, quest := range stageInfo.Quests {
		log.T("[LOOP] Trace [%v] quest.Id:%v", k, *quest.Id)
		if *quest.Id == questId {
			return quest, Error.OK()
		}
	}

	return nil, Error.New(EC.EQ_QUEST_ID_INVALID, fmt.Sprintf("invalid questId: %v", questId))
}

func GetStageInfo(db *data.Data, stageId uint32) (stageInfo *bbproto.StageInfo, e Error.Error) {
	if db == nil {
		return stageInfo, Error.New(EC.INVALID_PARAMS, "[ERROR] db pointer is nil.")
	}

	if err := db.Select(consts.TABLE_QUEST); err != nil {
		return stageInfo, Error.New(EC.READ_DB_ERROR, err)
	}

	log.T("begin get stageInfo: %v", stageId)

	zStageInfo, err := db.Gets(consts.X_QUEST_STAGE + common.Utoa(stageId))
	if err != nil {
		return stageInfo, Error.New(EC.READ_DB_ERROR, "read stageinfo fail")
	}

	stageInfo = &bbproto.StageInfo{}
	if len(zStageInfo) == 0 {
		return stageInfo, Error.New(EC.DATA_NOT_EXISTS, fmt.Sprintf("stageInfo [%v] not exists", stageId))
	}

	if err = proto.Unmarshal(zStageInfo, stageInfo); err != nil {
		log.T("[ERROR] unmarshal error from stage[%v] info.", stageId)
		return stageInfo, Error.New(EC.UNMARSHAL_ERROR, "unmarshal error.")
	}

	log.T("stageInfo[%v]: %+v", stageId, stageInfo)

	return stageInfo, Error.OK()
}

func GetQuestConfig(db *data.Data, questId uint32) (config bbproto.QuestConfig, e Error.Error) {
	if db == nil {
		return config, Error.New(EC.INVALID_PARAMS, "[ERROR] db pointer is nil.")
	}
	if err := db.Select(consts.TABLE_QUEST); err != nil {
		return config, Error.New(EC.READ_DB_ERROR, err)
	}

	zQuestConf, err := db.Gets(consts.X_QUEST_CONFIG + common.Utoa(questId))
	if err != nil {
		return config, Error.New(EC.EQ_GET_QUEST_CONFIG_ERROR, "get quest config fail")
	}

	if len(zQuestConf) == 0 {
		return config, Error.New(EC.DATA_NOT_EXISTS, fmt.Sprintf("QuestConfig for:%v not exists", questId))
	}

	if err = proto.Unmarshal(zQuestConf, &config); err != nil {
		log.T("[ERROR] unmarshal error from questConfig[%v].", questId)
		return config, Error.New(EC.UNMARSHAL_ERROR, "unmarshal error.")
	}

	return config, Error.OK()
}

//update tRecover, userStamina

type TUsedValue struct {
	Value int32
	Used  bool
}

//starQuest: check userDetail.Quest if exists; else: get quest state from QuestLogs(for chip gift)
func CheckQuestRecord(db *data.Data, stageId, questId uint32, userDetail *bbproto.UserInfoDetail) (state bbproto.EQuestState, e Error.Error) {
	if db == nil || userDetail == nil {
		return 0, Error.New(EC.INVALID_PARAMS, "invalid db pointer or userDetail pointer")
	}

	if err := db.Select(consts.TABLE_QUEST); err != nil {
		return 0, Error.New(EC.READ_DB_ERROR, err)
	}

	//get quest state: CLEAR or NEW
	var value []byte
	uid := *userDetail.User.UserId
	value, err := db.HGet(consts.X_QUEST_RECORD+common.Utoa(uid), common.Utoa(stageId)+"_"+common.Utoa(questId))
	if err != nil {
		log.Printf("[ERROR] GetQuestRecord for '%v' ret err:%v", uid, err)
		return 0, Error.New(EC.READ_DB_ERROR, "read quest log fail")
	}

	if len(value) == 0 {
		return 0, Error.OK() //no records
	}

	questStatus := &bbproto.QuestStatus{}
	err = proto.Unmarshal(value, questStatus)
	if err != nil {
		return 0, Error.New(EC.UNMARSHAL_ERROR)
	}

	if questStatus.State != nil {
		state = *questStatus.State
	}

	return state, Error.OK()
}

//called in clear_quest
func UpdateQuestLog(db *data.Data, userDetail *bbproto.UserInfoDetail, questId uint32,
	getUnit []uint32, getMoney int32) (gotMoney, gotExp, gotFriendPt int32, gotUnit []*bbproto.UserUnit, e Error.Error) {
	if db == nil {
		return 0, 0, 0, gotUnit, Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}

	uid := *userDetail.User.UserId
	if userDetail.Quest == nil {
		return 0, 0, 0, gotUnit, Error.New(EC.EQ_UPDATE_QUEST_RECORD_ERROR, "user.Quest is nil")
	}

	userDetail.Quest.EndTime = proto.Uint32(common.Now())

	//TODO: verity getMoney
	userDetail.Quest.GetMoney = proto.Int32(getMoney)

	//verify getUnit
	isAllValidUnit := true
	for _, dropIdGot := range getUnit {
		isValidOne := false
		for _, unitDrop := range userDetail.Quest.DropUnits {
			if *unitDrop.DropId == dropIdGot {
				isValidOne = true
				break
			}
		}
		if !isValidOne {
			log.Error("ClearQuest :: unitGot is invalid: %+v", dropIdGot)
			isAllValidUnit = false
			break
		}
	}

	if !isAllValidUnit {
		log.Error("clear request: invalid drop unit.")
		return 0, 0, 0, gotUnit, Error.New(EC.EQ_INVALID_DROP_UNIT, "clear request: invalid drop unit")
	}

	//append unit to userinfo
	for _, unitDrop := range userDetail.Quest.DropUnits {
		uniqueId, e := unit.GetUnitUniqueId(db, *userDetail.User.UserId, len(userDetail.UnitList))
		if e.IsError() {
			return 0, 0, 0, gotUnit, e
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

		gotUnit = append(gotUnit, userUnit) //return value
	}

	//already fill in getUnit, so empty dropUnit before save to QuestLog
	userDetail.Quest.DropUnits = []*bbproto.DropUnit{}

	//save userDetail.Quest to QuestLog
	zQuest, err := proto.Marshal(userDetail.Quest)
	if err != nil {
		return 0, 0, 0, gotUnit, Error.New(EC.MARSHAL_ERROR)
	}

	if err := db.Select(consts.TABLE_QUEST_LOG); err != nil {
		return 0, 0, 0, gotUnit, Error.New(EC.READ_DB_ERROR, err.Error())
	}
	if err = db.HSet(consts.X_QUEST_LOG+common.Utoa(uid), common.Utoa(questId), zQuest); err != nil {
		log.Error("HSet(X_QUEST_LOG_%v, %v) failed:%v.", uid, questId, err)
		return 0, 0, 0, gotUnit, Error.New(EC.READ_DB_ERROR)
	}

	gotMoney = *userDetail.Quest.GetMoney
	gotExp = *userDetail.Quest.GetExp
	if userDetail.Quest.GetFriendPoint != nil {
		gotFriendPt = *userDetail.Quest.GetFriendPoint
	}

	userDetail.Quest = nil

	return gotMoney, gotExp, gotFriendPt, gotUnit, Error.OK()
}

//called in start_quest
func FillQuestLog(userDetail *bbproto.UserInfoDetail, currParty int32, helperUid uint32, helperUnit *bbproto.UserUnit,
	drops []*bbproto.DropUnit, stage *bbproto.StageInfo, quest *bbproto.QuestInfo, questState bbproto.EQuestState) (e Error.Error) {
	if userDetail.Quest == nil {
		userDetail.Quest = &bbproto.QuestLog{}
		userDetail.Quest.QuestId = quest.Id
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
	userDetail.Quest.CurrentParty = proto.Int32(currParty)
	userDetail.Quest.HelperUserId = proto.Uint32(helperUid)
	userDetail.Quest.HelperUnit = helperUnit

	userDetail.Quest.State = &questState

	//fill drop unit
	for _, dropUnit := range drops {
		if dropUnit != nil {
			userDetail.Quest.DropUnits = append(userDetail.Quest.DropUnits, dropUnit)
		}
	}

	return Error.OK()
}
