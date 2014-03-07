package unit

import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"code.google.com/p/goprotobuf/proto"
)

func GetUnitUniqueId(db *data.Data, uid uint32, unitCount int) (unitId uint32, e Error.Error) {
	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return 0, Error.New(EC.READ_DB_ERROR, err)
		}
	} else if err := db.Select(consts.TABLE_UNIT); err != nil {
		return 0, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	maxId, err := db.GetInt(consts.KEY_MAX_UNIT_ID + common.Utoa(uid))
	if err != nil {
		return 0, Error.New(EC.READ_DB_ERROR, err)
	}

	if maxId == 0 {
		maxId = 1 //first unitId
		if unitCount > 0 {
			log.Fatal("data not valid: read from KEY_MAX_UNIT_ID return 0, but unit count is:%v", unitCount)
			return 0, Error.New(EC.EC_UNIT_ID_ERROR)
		}
	}

	unitId = uint32(maxId + 1)
	log.Printf("get getNewUnitId ret: %v ", unitId)
	if err = db.SetUInt(consts.KEY_MAX_UNIT_ID+common.Utoa(uid), unitId); err != nil {
		return 0, Error.New(EC.READ_DB_ERROR)
	}

	return unitId, Error.OK()
}

func getUnitInfo(db *data.Data, unitId uint32) (unit bbproto.UnitInfo, e Error.Error) {
	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return unit, Error.New(EC.READ_DB_ERROR, err)
		}
	} else if err := db.Select(consts.TABLE_UNIT); err != nil {
		return unit, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	value, err := db.Gets(consts.X_UNIT_INFO + common.Utoa(unitId))
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", unitId, err)
		return unit, Error.New(EC.READ_DB_ERROR, err.Error())
	}
	isExists := len(value) != 0
	//log.T("isUserExists=%v value len=%v value: ['%v']  ", isUserExists, len(value), value)

	if !isExists {
		log.Error("getUnitInfo: unitId(%v) not exists.", unitId)
		return unit, Error.New(EC.EC_UNIT_ID_ERROR)
	}

	err = proto.Unmarshal(value, &unit)
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", unit, err)
	}

	return unit, Error.OK()
}

func DoLevelUp(db *data.Data, userDetail *bbproto.UserInfoDetail, baseUniqueId uint32, partUniqueId []uint32, helperUid uint32, helperUnit bbproto.UserUnit) (e Error.Error) {
	//1. getUnitInfo(baseUniqueId)

	//2. getUnitInfo(partUniqueId..)

	//3. getUnitInfo(helperUnit.UnitId) *1.25, *1.5

	//4. remove partUnits,

	//5. levelup baseUnit

	//6.

	return Error.OK()
}
