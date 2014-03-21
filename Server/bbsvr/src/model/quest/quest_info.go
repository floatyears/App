package quest

import (
	"fmt"
	"strings"
)

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"github.com/garyburd/redigo/redis"
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

//starQuest:  get quest state from QuestLogs(for chip gift)
func CheckQuestRecord(db *data.Data, stageId, questId uint32, uid uint32) (state bbproto.EQuestState, e Error.Error) {
	state = bbproto.EQuestState_QS_NEW
	if db == nil {
		return state, Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}

	if err := db.Select(consts.TABLE_QUEST); err != nil {
		return state, Error.New(EC.READ_DB_ERROR, err)
	}

	//get quest state: CLEAR or NEW
	var value []byte
	value, err := db.HGet(consts.X_QUEST_RECORD+common.Utoa(uid), common.Utoa(stageId)+"_"+common.Utoa(questId))
	if err != nil {
		log.Printf("[ERROR] GetQuestRecord for '%v' ret err:%v", uid, err)
		return state, Error.New(EC.READ_DB_ERROR, "read quest log fail")
	}

	if len(value) == 0 {
		return state, Error.OK() //no records
	}

	questStatus := &bbproto.QuestStatus{}
	err = proto.Unmarshal(value, questStatus)
	if err != nil {
		return state, Error.New(EC.UNMARSHAL_ERROR)
	}

	if len(questStatus.PlayTime) != 0 {
		state = bbproto.EQuestState_QS_CLEARED
	}

	return state, Error.OK()
}

func SetQuestCleared(db *data.Data, userDetail *bbproto.UserInfoDetail, stageId, questId uint32, stageInfo *bbproto.StageInfo, isClearStage bool) (e Error.Error) {
	if db == nil {
		return Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}
	if err := db.Select(consts.TABLE_QUEST); err != nil {
		return Error.New(EC.READ_DB_ERROR, err)
	}

	if userDetail == nil || stageInfo == nil {
		log.Error("SetQuestCleared :: userDetail=%v stageInfo=%v", userDetail, stageInfo)
		return Error.New(EC.INVALID_PARAMS, "SetQuestCleared :: userDetail=nil or stageInfo=nil")
	}

	uid := *userDetail.User.UserId

	if *stageInfo.Type == bbproto.QuestType_E_QUEST_STORY {
		//update clear flag
		if userDetail.QuestClear == nil {
			userDetail.QuestClear = &bbproto.QuestClearInfo{}
		}
		if userDetail.QuestClear.StoryClear == nil {
			userDetail.QuestClear.StoryClear = &bbproto.StageClearItem{}
		}

		if userDetail.QuestClear.StoryClear.StageId != nil && userDetail.QuestClear.StoryClear.QuestId != nil {
			log.T("update Old QuestClear(STORY) [stageId:%v questId:%v]", *userDetail.QuestClear.StoryClear.StageId,
				*userDetail.QuestClear.StoryClear.QuestId)
		}

		log.T("update Lastest QuestClear(STORY) [stageId:%v questId:%v]", stageId, questId)

		userDetail.QuestClear.StoryClear.StageId = proto.Uint32(stageId)
		userDetail.QuestClear.StoryClear.QuestId = proto.Uint32(questId)

	} else if *stageInfo.Type == bbproto.QuestType_E_QUEST_EVENT || *stageInfo.Type == bbproto.QuestType_E_QUEST_EVOLVE {

		//update event quest clear flag
		questStatus := &bbproto.QuestStatus{}
		questStatus.PlayTime = append(questStatus.PlayTime, common.Now())

		zData, err := proto.Marshal(questStatus)
		if err != nil {
			return Error.New(EC.MARSHAL_ERROR)
		}

		if isClearStage {
			//TODO: delete stageId_questId*
			//		err = db.HSet(consts.X_QUEST_RECORD+common.Utoa(uid), common.Utoa(stageId), zData)
			err = db.HSet(consts.X_QUEST_RECORD+common.Utoa(uid), common.Utoa(stageId)+"_"+common.Utoa(questId), zData)
		} else {
			err = db.HSet(consts.X_QUEST_RECORD+common.Utoa(uid), common.Utoa(stageId)+"_"+common.Utoa(questId), zData)
		}
		if err != nil {
			log.Printf("[ERROR] SetQuestRecord for '%v' stage:%v quest:%v, ret err:%v", uid, stageId, questId, err)
			return Error.New(EC.SET_DB_ERROR, "read quest log fail")
		}
	}

	return Error.OK()
}

func IsStageCleared(db *data.Data, uid, stageId uint32, stageInfo *bbproto.StageInfo) (isAllClear, lastNotClear bool, e Error.Error) {
	isAllClear = false
	if db == nil || stageInfo == nil {
		return false, false, Error.New(EC.INVALID_PARAMS)
	}
	clearCount := 0
	for _, quest := range stageInfo.Quests {
		questState, e := CheckQuestRecord(db, stageId, *quest.Id, uid)
		if e.IsError() {
			log.Error("get quest state fail:%v", e.Error())
			return false, false, e
		}
		isAllClear = (questState == bbproto.EQuestState_QS_CLEARED)
		if isAllClear {
			clearCount += 1
		}
	}

	lastNotClear = (clearCount == (len(stageInfo.Quests) - 1))

	return isAllClear, lastNotClear, Error.OK()
}

func GetQuestClearFlag(db *data.Data, userDetail *bbproto.UserInfoDetail) (clearInfo *bbproto.QuestClearInfo, e Error.Error) {
	if db == nil {
		return nil, Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}

	if err := db.Select(consts.TABLE_QUEST); err != nil {
		return nil, Error.New(EC.READ_DB_ERROR, err)
	}

	if userDetail == nil {
		return nil, Error.New(EC.INVALID_PARAMS, "invalid userDetail pointer")
	}

	//get story flag
	uid := *userDetail.User.UserId

	clearInfo = &bbproto.QuestClearInfo{}
	if userDetail.QuestClear != nil {
		clearInfo.StoryClear = userDetail.QuestClear.StoryClear
	}

	//get event clear flag

	var value []byte
	zDatas, err := db.HGetAll(consts.X_QUEST_RECORD + common.Utoa(uid))
	if err != nil {
		log.Printf("[ERROR] GetQuestRecord for '%v' ret err:%v", uid, err)
		return clearInfo, Error.New(EC.READ_DB_ERROR, "read quest log fail")
	}

	if len(value) == 0 {
		return clearInfo, Error.OK() //no records
	}

	for i := 0; len(zDatas) > 0; i++ {
		var sStageIdQuestId, sQuestStatus []byte
		zDatas, err = redis.Scan(zDatas, &sStageIdQuestId, &sQuestStatus)
		if err != nil {
			continue
		}

		eventClear := &bbproto.StageClearItem{}
		ss := strings.Split(string(sStageIdQuestId), "_")
		if len(ss) == 2 {
			eventClear.StageId = proto.Uint32(common.Atou(ss[0]))
			eventClear.QuestId = proto.Uint32(common.Atou(ss[1]))
		} else if len(ss) == 1 {
			eventClear.StageId = proto.Uint32(common.Atou(ss[0]))
		}
		clearInfo.EventClear = append(clearInfo.EventClear, eventClear)
		log.T("append EventClear(%+v)", ss)
	}

	return clearInfo, Error.OK()
}
