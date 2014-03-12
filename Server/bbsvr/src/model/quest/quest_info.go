package quest

import (
	"fmt"
)

import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	//"model/unit"

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
