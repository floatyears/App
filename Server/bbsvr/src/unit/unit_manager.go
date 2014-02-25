package unit

import (
	"../bbproto"
	"../common"
	"../common/Error"
	"../common/log"
	"../const"
	"../data"
)

func GetUnitUniqueId(db *data.Data, userDetail *bbproto.UserInfoDetail) (unitId uint32, e Error.Error) {
	if db == nil {
		return 0, Error.New(cs.INVALID_PARAMS, "invalid db pointer")
	}

	if err := db.Select(cs.TABLE_UNIT); err != nil {
		return 0, Error.New(cs.SET_DB_ERROR, err.Error())
	}

	uid := *userDetail.User.UserId
	if userDetail.Quest == nil {
		return 0, Error.New(cs.EQ_UPDATE_QUEST_RECORD_ERROR, "user.Quest is nil")
	}

	maxId, err := db.GetInt(cs.KEY_MAX_UNIT_ID + common.Utoa(uid))
	if err != nil {
		return 0, Error.New(cs.READ_DB_ERROR, err)
	}

	if maxId == 0 {
		maxId = 1 //first unitId
		if len(userDetail.UnitList) > 0 {
			return 0, Error.New(cs.READ_DB_ERROR)
		}
	}

	unitId += uint32(maxId + 1)
	log.Printf("get getNewUnitId ret: %v ", unitId)
	if err = db.SetUInt(cs.KEY_MAX_UNIT_ID+common.Utoa(uid), unitId); err != nil {
		return 0, Error.New(cs.SET_DB_ERROR)
	}

	return unitId, Error.OK()
}
