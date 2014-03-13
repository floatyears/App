package unit

import (
	"bbproto"
	//"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"github.com/garyburd/redigo/redis"
	//"fmt"
)

func UpdateAcountForGacha(userDetail *bbproto.UserInfoDetail, gachaType bbproto.EGachaType, gachaCount int32) (e Error.Error) {
	if gachaType == bbproto.EGachaType_E_FRIEND_GACHA {
		if *userDetail.Account.FriendPoint < consts.N_GACHA_FRIEND_COST*gachaCount {
			return Error.New(EC.E_NO_ENOUGH_MONEY)
		} else {
			*userDetail.Account.FriendPoint -= consts.N_GACHA_FRIEND_COST * gachaCount
		}
	} else { // if gachaType == bbproto.EGachaType_E_BUY_GACHA
		if *userDetail.Account.Stone < consts.N_GACHA_BUY_COST*gachaCount {
			return Error.New(EC.E_NO_ENOUGH_MONEY)
		} else {
			*userDetail.Account.Stone -= consts.N_GACHA_BUY_COST * gachaCount
		}
	}

	return Error.OK()
}

func CheckGachaAvailble(gachaConf *bbproto.GachaConfig, gachaId int32) (e Error.Error) {
	if gachaConf == nil {
		return Error.New(EC.INVALID_PARAMS)
	}

	tNow := common.Now()
	if tNow < *gachaConf.BeginTime || tNow > *gachaConf.EndTime {
		log.Error("gacha tNow:%v is not invalid experiod[%v - %v]", tNow, *gachaConf.BeginTime, *gachaConf.EndTime)
		return Error.New(EC.E_GACHA_TIME_INVALID)
	}

	return Error.OK()
}

//TODO: load gacha pool on server start, save to Global array (only once load).
func LoadGachaPool(db *data.Data, gachaId int32) (unitIdList []uint32, e Error.Error) {
	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return unitIdList, Error.New(EC.READ_DB_ERROR, err)
		}
	} else if err := db.Select(consts.TABLE_UNIT); err != nil {
		return unitIdList, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	values, err := db.ListGetAll(consts.X_GACHA_UNIT + common.Ntoa(gachaId))
	if err != nil {
		return unitIdList, Error.New(EC.READ_DB_ERROR, err)
	}

	for k, unitId := range values {
		sUnitId, err := redis.String(unitId, nil)
		if err != nil {
			return unitIdList, Error.New(EC.UNMARSHAL_ERROR, err)
		}
		log.T("unitId[%v]: unitId:%v sUnitId:%v", k, unitId, sUnitId)
		unitIdList = append(unitIdList, common.Atou(sUnitId))
	}

	return unitIdList, Error.OK()
}

func GetGachaUnit(db *data.Data, gachaId, gachaCount int32) (gotUnitId []uint32, e Error.Error) {

	unitIdPool, e := LoadGachaPool(db, gachaId)
	if e.IsError() {
		log.Error("LoadGachaPool( gachaId:%v ) ret error:%v", gachaId, e.Error())
		return gotUnitId, e
	}

	for i := int32(0); i < gachaCount; i++ {
		randNum := common.Randn(int32(len(unitIdPool)))
		log.T("GetGachaUnit:: randNum:%v => unitId:%v", randNum, unitIdPool[randNum])
		gotUnitId = append(gotUnitId, unitIdPool[randNum])
	}

	return gotUnitId, Error.OK()
}
