package main

import (
	"code.google.com/p/goprotobuf/proto"
	"flag"
	_ "html"
)
import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"common/config"
	"data"
	"model/unit"
	_ "quest"
	//redis "github.com/garyburd/redigo/redis"
)

func renameUnit(db *data.Data, uid uint32) (userDetail *bbproto.UserInfoDetail, e Error.Error) {
	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_USER)
		defer db.Close()
		if err != nil {
			log.Error("[ERROR] CONNECT_DB_ERROR err:%v", err)
			return nil, Error.New(EC.CONNECT_DB_ERROR, err)
		}
	} else {
		if err := db.Select(consts.TABLE_USER); err != nil {
			return nil, Error.New(EC.READ_DB_ERROR, err.Error())
		}
	}

	var value []byte
	value, err := db.Gets(common.Utoa(uid))
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return nil, Error.New(EC.READ_DB_ERROR, err)
	}

	if len(value) == 0 {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret value is empty.", uid)
		return nil, Error.New(EC.EU_USER_NOT_EXISTS, err)
	}

	userDetail = &bbproto.UserInfoDetail{}
	err = proto.Unmarshal(value, userDetail)
	if err != nil {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return nil, Error.New(EC.UNMARSHAL_ERROR)
	}
	userDetail.UnitList=[]*bbproto.UserUnit{}

	unitIdPool := []uint32 {1,5,9,49,50,51,52,53,54,61,63,64,65,66,73,75,76,77,78,89,90,152,153}
	for i := 1; i <= len(unitIdPool); i++ {
		uuId2, e := unit.GetUnitUniqueId(db, *userDetail.User.UserId, i-1)
		if e.IsError() {
			return nil, e
		}

		unitId :=uint32(unitIdPool[i])
		level:=common.Rand(1, int32(i))
		exp:= unit.GetUnitExpByUnitId(unitId, level)
		if exp == 0 {
			log.Error("GetUnitExpByUnitId fail for unitId: %v", unitId)
			continue
		}
		userUnit2 := &bbproto.UserUnit{
			UniqueId:  proto.Uint32(uuId2),
			UnitId:    proto.Uint32(unitId),
			Exp:       proto.Int32(exp),
			Level:     proto.Int32(level),
			GetTime:   proto.Uint32(common.Now()),
			AddAttack: proto.Int32(common.Randn(int32(i) % 10)),
			AddHp:     proto.Int32(common.Randn(int32(i) % 10)),
		}
		userDetail.UnitList = append(userDetail.UnitList, userUnit2)
	}

	for _, party :=range userDetail.Party.PartyList {
		for i, item:=range party.Items {
			item.UnitUniqueId = userDetail.UnitList[i].UniqueId
		}
	}

	//save data
	zUserData, err := proto.Marshal(userDetail)
	if err != nil {
		return nil, Error.New(EC.MARSHAL_ERROR, err)
	}

	if err := db.Select(consts.TABLE_USER); err != nil {
		return nil, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	if err = db.Set(common.Utoa(*userDetail.User.UserId), zUserData); err != nil {
		log.Error("SET_DB_ERROR for userDetail: %v", *userDetail.User.UserId)
		return nil, Error.New(EC.READ_DB_ERROR, err)
	}

	log.T("user:%v restore stamina success, now stamina is: %v/%v", uid, *userDetail.User.StaminaNow, *userDetail.User.StaminaMax)
	return userDetail, Error.OK()
}

func main() {
	flag.Parse()
	args := flag.Args()

	if args == nil || len(args) < 1 {
		//log.T("usage: input param: {uid}")
		//return
	}

	Init()

	config.InitConfig()

	//uid := common.Atou(args[0])
	for uid:=uint32(100);uid<2200;uid++{
		log.T("resetAccount for: {uid}", uid)
		renameUnit(nil, uid)		
	}

	log.Fatal("done.")
}
