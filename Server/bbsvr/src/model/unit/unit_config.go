package unit

import (
	"bbproto"
	//"common"
	//"common/EC"
	//"common/Error"
	"common/config"
	"common/consts"
	//"common/log"
	"event"
)

func GetLevelUpMoney(level int32, count int32) int32 {

	if level <= 0 || level > int32(len(config.TableDevourCostCoin)-1) {
		return -1
	}

	return config.TableDevourCostCoin[level-1]
}

func getUnitExpValue(expType int32, level int32) (levelExp int32) {
	//TODO: read from global exp type table
	if level > int32(len(config.TableUnitExpType)) {
		return -1
	}

	return config.TableUnitExpType[level-1]
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

	stageId+= 20

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

	questId += stageId*100 + questId

	return stageId, questId
}

func GetTodayEvolveType() (etype *bbproto.EUnitType) {
	return event.GetEvolveType()
}
