package config

import (
	"common/EC"
	"common/consts"
	"common/log"
	"data"
	"common/Error"
	"code.google.com/p/goprotobuf/proto"
	"bbproto"
	"github.com/garyburd/redigo/redis"
)

var TableUnitExpType = make([]int32, 99)
var TableDevourCostCoin = make([]int32, 99)
var TableUserRankExp = make([]int32, consts.N_MAX_USER_RANK)
var GUnitInfoList = make(map[uint32]*bbproto.UnitInfo)


func InitConfig()  (e Error.Error) {
	for i := int32(0); i < 99; i++ {
		TableUnitExpType[i] = 100 * ((i + 1) * (i + 1) / 3)
		if TableUnitExpType[i] == 0 {
			TableUnitExpType[i] = 10
		}
		log.T("TableUnitExpType[%v]=%v",i, TableUnitExpType[i])
	}

	for i := int32(0); i < 99; i++ {
		TableDevourCostCoin[i] = 100 * (i + 1)
	}

	for i := int32(0); i < 500; i++ {
		TableUserRankExp[i] = 100 * (i + 1)
	}

	if e = LoadUnitInfoConfig(); e.IsError() {
		log.Error("LoadUnitInfoConfig failed.")
		return e
	}

	return Error.OK()
}

func LoadUnitInfoConfig() (e Error.Error) {

	db := &data.Data{}
	err := db.Open(consts.TABLE_UNIT)
	defer db.Close()
	if err != nil {
		return Error.New(EC.READ_DB_ERROR, err)
	}

	keys, err := db.GetKeys(consts.X_UNIT_INFO + "*")
	if err != nil {
		log.Error("LoadUnitInfoConfig  ret err:%v", err)
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	if len(keys) == 0 {
		log.Error("LoadUnitInfoConfig  ret no data.")
		return Error.New(EC.DATA_NOT_EXISTS)
	}

	unitIds := redis.Args{}
	for _, unitId := range keys {
		unitIds = unitIds.Add( unitId )
//		log.T("loop add unitId: %v", unitId)
	}

	unitInfos, err := db.MGet( unitIds )
	if err != nil {
		log.Error("LoadUnitInfoConfig db.MGet ret err:%v", err)
		return Error.New(EC.READ_DB_ERROR, err)
	}
//	log.T("db.MGet( unitIds ) ret data count:%v", len(unitInfos))

	for _, uinfo := range unitInfos {
		if uinfo == nil {
			continue
		}

		zData, err := redis.Bytes(uinfo, err)
		if err == nil && len(zData) > 0 {
			unit := &bbproto.UnitInfo{}
			if err = proto.Unmarshal(zData, unit); err != nil {
				log.Error(" Cannot Unmarshal userinfo(err:%v) userinfo: %v", err, uinfo)
				return Error.New(EC.UNMARSHAL_ERROR,err)
			}

			GUnitInfoList[*unit.Id] = unit
		} else {
			return Error.New("redis.Bytes(uinfo, err) fail.")
		}
	}

	log.T("Load ret all UnitInfo:")
	for k, unit:= range GUnitInfoList {
		log.T("[%v], %+v", k, unit)
	}
	log.T("==========Load UnitInfo ret total count: %v ==========", len(GUnitInfoList))

	return Error.OK()
}
