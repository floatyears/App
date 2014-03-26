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
	"common/config"
	"common/log"
	"data"
	_ "quest"
	//"src/model/user"
	//redis "github.com/garyburd/redigo/redis"
)

func resetQuest(db *data.Data, uid uint32) (userDetail *bbproto.UserInfoDetail, e Error.Error) {
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

	//restore stamina
	userDetail.Quest = nil

	//save data
	zUserData, err := proto.Marshal(userDetail)
	if err != nil {
		return nil, Error.New(EC.MARSHAL_ERROR, err)
	}

	if err = db.Set(common.Utoa(*userDetail.User.UserId), zUserData); err != nil {
		log.Error("SET_DB_ERROR for userDetail: %v", *userDetail.User.UserId)
		return nil, Error.New(EC.READ_DB_ERROR, err)
	}

	log.T("user:%v restore stamina success, now stamina is: %v/%v", uid, *userDetail.User.StaminaNow, *userDetail.User.StaminaMax)
	return userDetail, Error.OK()
}

func resetAccount(db *data.Data, uid uint32) (userDetail *bbproto.UserInfoDetail, e Error.Error) {
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

	rank := *userDetail.User.Rank
	
	userDetail.Account.Money = proto.Int32(5000)
	userDetail.Account.Stone = proto.Int32(500)
	userDetail.Account.FriendPoint = proto.Int32(5000)

	*userDetail.User.StaminaMax = config.GetStaminaMax(rank)
	*userDetail.User.CostMax = config.GetCostMax(rank) 
	*userDetail.User.UnitMax = config.GetUnitMax(rank)
	*userDetail.User.FriendMax = config.GetFriendMax(rank)


	*userDetail.User.StaminaNow = *userDetail.User.StaminaMax

	randExp:= int32(1)

	//log.T("User.Exp:%v rank:%v oldExp:%v randExp(%v) ", *userDetail.User.Exp, rank,*userDetail.User.Exp, randExp)
	*userDetail.User.Exp = config.GetTotalRankExp(rank) + randExp

//save data
	zUserData, err := proto.Marshal(userDetail)
	if err != nil {
		return nil, Error.New(EC.MARSHAL_ERROR, err)
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
		log.T("usage: input param: {uid}")
		return
	}

	Init()

	uid := common.Atou(args[0])
	log.T("resetAccount for: {uid}", uid)
	for uid=100; uid<2016; uid++{
	resetAccount(nil, uid)
	}

	log.Fatal("done.")
}
