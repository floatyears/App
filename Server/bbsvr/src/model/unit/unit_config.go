package unit

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/config"
	"common/consts"
	"common/log"
	"data"
	"event"
	"fmt"
)

func GetLevelUpMoney(level int32, count int32) int32 {

	if level <= 0 || level > int32(len(config.TableDevourCostCoin)-1) {
		return -1
	}

	return config.TableDevourCostCoin[level-1]
}

func GetUnitExpValue(expType int32, level int32) (levelExp int32) {
	//TODO: read from global exp type table
	if level > int32(len(config.TableUnitExpType)) {
		log.Error("GetUnitExpValue(%v, %v):: level excceed max Exp Level.", expType, level)
		return -1
	}

	return config.TableUnitExpType[level-1]
}

func GetUnitExpByLevel(unitId uint32, level int32) (exp int32, e Error.Error) {
	exp = 0;
	unitInfo, e := GetUnitInfo(nil, unitId)
	if e.IsError() {
		return exp, e
	}
	exp = GetUnitExpValue(*unitInfo.PowerType.ExpType, level)

	return exp, Error.OK()
}

func GetEvolveQuestId(unitType bbproto.EUnitType, unitRare int32) (stageId, questId uint32) {
	if unitRare > consts.N_MAX_RARE {
		return 0, 0
	}

	switch unitType {
	case bbproto.EUnitType_UWIND:
		stageId = 1
	case bbproto.EUnitType_UFIRE:
		stageId = 2
	case bbproto.EUnitType_UWATER:
		stageId = 3
	case bbproto.EUnitType_ULIGHT:
		stageId = 4
	case bbproto.EUnitType_UDARK:
		stageId = 5
	case bbproto.EUnitType_UNONE:
		stageId = 6
	default:
		return 0, 0
	}

	stageId += 20

	baseQuestId := uint32(1)
	switch unitRare {
	case 1:
		questId = baseQuestId + 0
	case 2:
		questId = baseQuestId + 1
	case 3:
		questId = baseQuestId + 2
	case 4:
		questId = baseQuestId + 3
	case 5:
		questId = baseQuestId + 4
	case 6:
		questId = baseQuestId + 5
	default:
		return 0, 0
	}

	questId = stageId*100 + questId

	return stageId, questId
}

func GetTodayEvolveType() (etype *bbproto.EUnitType) {
	return event.GetEvolveType()
}

func GetGachaConfig(db *data.Data, gachaId int32) (gachaConf *bbproto.GachaConfig, e Error.Error) {
	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return gachaConf, Error.New(EC.READ_DB_ERROR, err)
		}
	} else if err := db.Select(consts.TABLE_UNIT); err != nil {
		return gachaConf, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	value, err := db.Gets(consts.X_GACHA_CONF + common.Ntoa(gachaId))
	if err != nil {
		log.Error("GetGachaConf for '%v' ret err:%v", gachaId, err)
		return gachaConf, Error.New(EC.READ_DB_ERROR, err.Error())
	}
	isExists := len(value) != 0

	if !isExists {
		log.Error("GetGachaConf: gachaId(%v) not exists.", gachaId)
		return gachaConf, Error.New(EC.DATA_NOT_EXISTS, fmt.Sprintf("gachaId:%v not exists in db.", gachaId))
	}

	gachaConf = &bbproto.GachaConfig{}

	err = proto.Unmarshal(value, gachaConf)
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", gachaId, err)
		return gachaConf, Error.New(EC.UNMARSHAL_ERROR, err)
	}

	return gachaConf, Error.OK()
}
