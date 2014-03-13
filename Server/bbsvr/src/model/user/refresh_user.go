package user

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"common/config"
	"data"
)

//calculate rank by current user.Exp
func RefreshRank(user *bbproto.UserInfo ) (e Error.Error ){
	if user == nil {
		return Error.New(EC.INVALID_PARAMS, "user is null")
	}
	user.Rank = proto.Int32( GetRankByExp(*user.Exp) )
	return Error.OK()
}

//return user rank by exp (in total)
func GetRankByExp(exp int32) (rank int32) {
	totalExp := int32(0)
	for rank:=int32(1); rank < consts.N_MAX_USER_RANK; rank++{
		totalExp += config.TableUserRankExp[rank]
		if totalExp >= exp {
			return rank
		}
	}
	return 1
}

//update tRecover, userStamina
func RefreshStamina(tRecover *uint32, userStamina *int32, userStaminaMax int32) (e Error.Error) {
	if tRecover == nil || userStamina == nil {
		return Error.New(EC.INVALID_PARAMS, "invalid params")
	}

	tNow := common.Now()
	tElapse := int32(tNow - *tRecover)
	log.T("Old Stamina:%v tRecover:%v tElapse:%v ", userStamina, *tRecover, tElapse)
	*userStamina += (tElapse/consts.N_STAMINA_TIME + 1)
	log.T("Now Stamina:%v userStaminaMax:%v", *userStamina, userStaminaMax)

	if *userStamina > userStaminaMax {
		*userStamina = userStaminaMax
	}

	*tRecover = tNow + uint32(consts.N_STAMINA_TIME-tElapse%consts.N_STAMINA_TIME)

	return Error.OK()
}

//restore StaminaNow to StaminaMax
func RetoreStamina(db *data.Data, uid uint32) (userDetail *bbproto.UserInfoDetail, e Error.Error) {
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
	userDetail.User.StaminaNow = userDetail.User.StaminaMax

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
