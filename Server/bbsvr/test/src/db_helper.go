package data

import (
	//"bbproto"
	"code.google.com/p/goprotobuf/proto"
	//"common"
	"common/EC"
	"common/Error"
	//"common/config"
	//"common/consts"
	"common/log"
	"fmt"
	"reflect"
)

func GetKeyValue(db *Data, table, key string, object interface{}) (e Error.Error) {
	if db == nil {
		db = &Data{}
		err := db.Open(table)
		defer db.Close()
		if err != nil {
			return Error.New(EC.READ_DB_ERROR, err)
		}
	} else if err := db.Select(table); err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	value, err := db.Gets(key)
	if err != nil {
		log.Error("GetGachaConf for '%v' ret err:%v", key, err)
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}
	isExists := len(value) != 0

	if !isExists {
		log.Error("GetGachaConf: unitId(%v) not exists.", key)
		return Error.New(EC.DATA_NOT_EXISTS, fmt.Sprintf("gachaId:%v not exists in db.", key))
	}

	object = new(reflect.TypeOf(object))

	err = proto.Unmarshal(value, object)
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", key, err)
		return Error.New(EC.UNMARSHAL_ERROR, err)
	}

	return Error.OK()
}
