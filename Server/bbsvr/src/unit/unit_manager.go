package unit

import (
	//"../bbproto"
	"../common"
	"../common/Error"
	"../common/log"
	"../const"
	"../data"
)

func GetUnitUniqueId(db *data.Data, uid uint32, unitCount int) (unitId uint32, e Error.Error) {
	if db == nil {
		db = &data.Data{}
		err := db.Open(cs.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return 0, Error.New(cs.READ_DB_ERROR, err)
		}
	} else if err := db.Select(cs.TABLE_UNIT); err != nil {
		return 0, Error.New(cs.SET_DB_ERROR, err.Error())
	}

	maxId, err := db.GetInt(cs.KEY_MAX_UNIT_ID + common.Utoa(uid))
	if err != nil {
		return 0, Error.New(cs.READ_DB_ERROR, err)
	}

	if maxId == 0 {
		maxId = 1 //first unitId
		if unitCount > 0 {
			log.Fatal("data not valid: read from KEY_MAX_UNIT_ID return 0, but unit count is:%v", unitCount)
			return 0, Error.New(cs.EC_UNIT_ID_ERROR)
		}
	}

	unitId = uint32(maxId + 1)
	log.Printf("get getNewUnitId ret: %v ", unitId)
	if err = db.SetUInt(cs.KEY_MAX_UNIT_ID+common.Utoa(uid), unitId); err != nil {
		return 0, Error.New(cs.SET_DB_ERROR)
	}

	return unitId, Error.OK()
}
