package unit

import (
	"bbproto"
//	"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/config"
	"common/log"
	"data"
	"fmt"
)

func GetUnitUniqueId(db *data.Data, uid uint32, unitCount int) (uniqueId uint32, e Error.Error) {
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
			return 0, Error.New(EC.E_UNIT_ID_ERROR)
		}
	}

	uniqueId = uint32(maxId + 1)
	//	log.T("uid(%v) get getNewUniqueId ret: %v ",uid, uniqueId)
	if err = db.SetUInt(consts.KEY_MAX_UNIT_ID+common.Utoa(uid), uniqueId); err != nil {
		return 0, Error.New(EC.READ_DB_ERROR)
	}

	return uniqueId, Error.OK()
}

//TODO: use global object to store unitinfo, avoiding read db.
func GetUnitInfo(unitId uint32) (unit *bbproto.UnitInfo, e Error.Error) {

	unit = config.GUnitInfoList[unitId]

	return unit, Error.OK()
}

// find UserUnit from userDetail.UnitList
func GetUserUnitInfo(userDetail *bbproto.UserInfoDetail, uniqueId uint32) (userunit *bbproto.UserUnit, e Error.Error) {
	for _, unit := range userDetail.UnitList {
		if *unit.UniqueId == uniqueId {
			return unit, Error.OK()
		}
	}

	return userunit, Error.New(EC.DATA_NOT_EXISTS, fmt.Sprintf("uniqueId:%v not exists in myUnitList.", uniqueId))
}

func CalculateDevourExp(db *data.Data, userDetail *bbproto.UserInfoDetail, baseUnit *bbproto.UnitInfo,
	partUniqueIds []uint32) (blendExp, addAtk, addHp, addDef int32, e Error.Error) {
	blendExp = int32(0)
	addAtk = int32(0)
	addHp = int32(0)
	addDef = int32(0)

	if userDetail == nil || baseUnit == nil {
		log.Error("Invalid userDetail or baseUnit pointer.")
		return -1, -1, -1, -1, Error.New(EC.INVALID_PARAMS)
	}
	for _, partUniqueId := range partUniqueIds {
		partUU, e := GetUserUnitInfo(userDetail, partUniqueId)
		if e.IsError() {
			return -1, -1, -1, -1, e
		}
		partUnit, e := GetUnitInfo( *partUU.UnitId)
		if e.IsError() {
			return -1, -1, -1, -1, e
		}
		log.T("partUserUnit:%+v partUnit:%+v", partUU, partUnit)

		if partUU.AddAttack != nil {
			addAtk += *partUU.AddAttack
		}
		if partUU.AddHp != nil {
			addHp += *partUU.AddHp
		}
		if partUU.AddDefence != nil {
			addDef += *partUU.AddDefence
		}

		multiple := float32(1.0)
		if *baseUnit.Race == *partUnit.Race && *baseUnit.Type == *partUnit.Type {
			multiple = float32(1.5)
		} else if *baseUnit.Race == *partUnit.Race || *baseUnit.Type == *partUnit.Type {
			multiple = float32(1.25)
		}

		blendExp += int32(float32(*partUnit.DevourValue) * float32(*partUU.Level) * multiple)

		log.T("Add partUnit:[%v | %v] DevourExp = (%v * %v) => %v", *partUU.UniqueId, *partUU.UnitId,
			(*partUnit.DevourValue)*(*partUU.Level), multiple, int32(float32(*partUnit.DevourValue)*float32(*partUU.Level)*multiple))
	}

	return blendExp, addAtk, addHp, addDef, Error.OK()
}

func CalcLevelUpAddLevel(userUnit *bbproto.UserUnit, unit *bbproto.UnitInfo, currExp int32, addExp int32) (addLevel int32, e Error.Error) {
	addLevel = int32(0)
	currLevel := int32(1)
	nextLevelExp := int32(0)
	for level := int32(1); level <= *unit.MaxLevel; level++ {
		nextLevelExp += GetUnitExpByExpType(*unit.PowerType.ExpType, level)
		if nextLevelExp <= 0 { // nextLevelExp<=0 means it reach MAX_LEVEL
			log.T("nextLevelExp=%v => reach MaxLevel: %v.", nextLevelExp, level)
			break
		}

		if currExp+addExp >= nextLevelExp {
			currLevel += 1
			log.T("currLevel:%v currExp+addExp: %v + %v  >= level[%v].nextLevelExp: %v, add Level: %v ",currLevel, currExp, addExp, level, nextLevelExp, addLevel)
		} else {
			log.T("currLevel:%v currExp+addExp: %v + %v  < level[%v].nextLevelExp: %v. return addLevel: %v ",currLevel, currExp, addExp, level, nextLevelExp, addLevel)
			break
		}
	}

	addLevel = currLevel - *userUnit.Level;
	log.T("nextLevelExp:%v - currExp+addExp:%v = nextExp:%v", nextLevelExp, currExp+addExp, nextLevelExp-(currExp+addExp))

	return addLevel, Error.OK()
}

func RemoveMyUnit(unitList *[]*bbproto.UserUnit, partUniqueId []uint32) (e Error.Error) {
	//	for k, unit := range *unitList {
	//		log.T("before remove, unitList[%v]: %+v", k, unit)
	//	}

	//	log.T("============before remove (Len:%v):===============", len(*unitList))
	for i := 0; i < len(partUniqueId); i++ {
		for pos, userunit := range *unitList {
			if *userunit.UniqueId == partUniqueId[i] {
				if pos >= len(*unitList)-1 { // is LastOne
					//log.T("[%v]loop unitList pos:%v  len(*unitList)=%v isLastOne!!", i, pos, len(*unitList))
					*unitList = (*unitList)[:pos]
				} else {
					//log.T("[%v]loop unitList pos:%v  len(*unitList)=%v", i, pos, len(*unitList))
					*unitList = append((*unitList)[:pos], (*unitList)[pos+1:]...)
				}
				break
			}
		}
	}

	//	log.T("============after remove (Len:%v):===============", len(*unitList))
	//	for k, unit := range *unitList {
	//		log.T("\tunitList[%v]: %+v", k, unit)
	//	}

	return Error.OK()
}
