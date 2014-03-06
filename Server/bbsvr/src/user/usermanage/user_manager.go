// user_manger.go
package usermanage

import (
	"../../bbproto"
	"../../common"
	"../../common/Error"
	"../../common/log"
	"../../const"
	"../../data"
	"../../unit"
	proto "code.google.com/p/goprotobuf/proto"
	_ "fmt"
)

func AddNewUser(db *data.Data, uuid string) (userdetail *bbproto.UserInfoDetail, e Error.Error) {
	if db == nil {
		return nil, Error.New(cs.INVALID_PARAMS, "invalid db pointer")
	}
	if err := db.Select(cs.TABLE_USER); err != nil {
		return nil, Error.New(cs.READ_DB_ERROR, err)
	}
	userdetail = &bbproto.UserInfoDetail{}

	newUserId, err := GetNewUserId()
	if err != nil {
		return nil, Error.New(cs.EU_GET_NEWUSERID_FAIL, err)
	}
	defaultName := cs.DEFAULT_USER_NAME
	tNow := common.Now()
	rank := int32(30 + common.Randn(10)) //int32(1)
	exp := int32(0)
	staminaNow := int32(100)
	staminaMax := int32(100)
	staminaRecover := uint32(tNow + 600) //10 minutes

	userdetail.User = &bbproto.UserInfo{
		UserId:         &newUserId,
		NickName:       &defaultName,
		Rank:           &rank,
		Exp:            &exp,
		StaminaNow:     &staminaNow,
		StaminaMax:     &staminaMax,
		StaminaRecover: &staminaRecover,
	}

	userdetail.Account = &bbproto.AccountInfo{
		PayTotal:       proto.Int32(0),
		PayMonth:       proto.Int32(0),
		Money:          proto.Int32(10000),
		StonePay:       proto.Int32(0),
		StoneFree:      proto.Int32(3),
		Stone:          proto.Int32(3),
		FriendPoint:    proto.Int32(50),
		FirstSelectNum: proto.Int32(1),
	}

	//TODO: fill other default values
	//userdetail.Quest = nil //

	userdetail.Login = &bbproto.LoginInfo{}
	userdetail.Login.LoginTotal = proto.Int32(1)
	userdetail.Login.LoginChain = proto.Int32(0)
	userdetail.Login.LastLoginTime = proto.Uint32(tNow)
	userdetail.Login.LastPlayTime = userdetail.Login.LastLoginTime

	if db == nil {
		db = &data.Data{}
		err := db.Open(cs.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return nil, Error.New(cs.CONNECT_DB_ERROR, err)
		}
	} else if err := db.Select(cs.TABLE_UNIT); err != nil {
		return nil, Error.New(cs.SET_DB_ERROR, err.Error())
	}

	//malloc 3 default unit, TODO: let user to select a actor.
	unitId1, e := unit.GetUnitUniqueId(db, *userdetail.User.UserId, 0)
	if e.IsError() {
		return nil, e
	}
	userUnit1 := &bbproto.UserUnit{
		UniqueId:  proto.Uint32(unitId1),
		UnitId:    proto.Uint32(uint32(common.Rand(1, 20))),
		Exp:       proto.Int32(1),
		Level:     proto.Int32(1),
		GetTime:   &tNow,
		AddAttack: proto.Int32(3),
		AddHp:     proto.Int32(2),
	}
	userdetail.UnitList = append(userdetail.UnitList, userUnit1)

	for i := 1; i <= 28; i++ {
		unitId2, e := unit.GetUnitUniqueId(db, *userdetail.User.UserId, i)
		if e.IsError() {
			return nil, e
		}
		userUnit2 := &bbproto.UserUnit{
			UniqueId:  proto.Uint32(unitId2),
			UnitId:    proto.Uint32(uint32(i)),
			Exp:       proto.Int32(1),
			Level:     proto.Int32(common.Rand(1, int32(i))),
			GetTime:   &tNow,
			AddAttack: proto.Int32(common.Randn(int32(i) % 10)),
			AddHp:     proto.Int32(common.Randn(int32(i) % 10)),
		}
		userdetail.UnitList = append(userdetail.UnitList, userUnit2)
	}

	userdetail.User.Unit = userUnit1

	//make default party
	userdetail.Party = &bbproto.PartyInfo{}
	userdetail.Party.CurrentParty = proto.Int(0)
	for i := int32(0); i < 5; i++ { //5 group
		party := &bbproto.UnitParty{}
		party.Id = proto.Int32(i)
		for pos := int32(0); pos < 4; pos++ {
			item := &bbproto.PartyItem{}
			item.UnitPos = proto.Int32(pos)
			item.UnitUniqueId = userdetail.UnitList[pos].UniqueId
			party.Items = append(party.Items, item)
		}
		userdetail.Party.PartyList = append(userdetail.Party.PartyList, party)
	}

	if err := db.Select(cs.TABLE_USER); err != nil {
		return nil, Error.New(cs.READ_DB_ERROR, err.Error())
	}

	zUserData, err := proto.Marshal(userdetail)
	err = db.Set(common.Utoa(*userdetail.User.UserId), zUserData)
	log.T("db.Set(%v) save new userinfo, return err(%v)", *userdetail.User.UserId, err)
	if err != nil {
		log.Error("set db(userinfo) ret error:%v", err)
		return nil, Error.New(cs.SET_DB_ERROR, err)
	}

	//save uuid -> uid
	err = db.Set(cs.X_UUID+uuid, []byte(common.Utoa(*userdetail.User.UserId)))
	if err != nil {
		log.Error("set db(uuid -> userId) ret error:%v", err)
		return nil, Error.New(cs.SET_DB_ERROR, err)
	}

	return userdetail, Error.OK()
}

func UpdateUserInfo(db *data.Data, userdetail *bbproto.UserInfoDetail) (e Error.Error) {
	if db == nil {
		return Error.New(cs.INVALID_PARAMS, "invalid db pointer")
	}

	if err := db.Select(cs.TABLE_USER); err != nil {
		return Error.New(cs.READ_DB_ERROR, err.Error())
	}

	zUserData, err := proto.Marshal(userdetail)
	if err != nil {
		return Error.New(cs.MARSHAL_ERROR, err.Error())
	}

	if err = db.Set(common.Utoa(*userdetail.User.UserId), zUserData); err != nil {
		return Error.New(cs.SET_DB_ERROR, err.Error())
	}
	log.T("UpdateUserInfo for (%v) , return OK", *userdetail.User.UserId)

	return Error.OK()
}

//TODO: may return *bbproto.UserInfoDetail
func GetUserInfo(db *data.Data, uid uint32) (userInfo bbproto.UserInfoDetail, isUserExists bool, err error) {
	isUserExists = false

	if db == nil {
		db := &data.Data{}
		err = db.Open(cs.TABLE_USER)
		defer db.Close()
		if err != nil {
			return
		}
	} else {
		if err := db.Select(cs.TABLE_USER); err != nil {
			return userInfo, isUserExists, err
		}
	}

	var value []byte
	value, err = db.Gets(common.Utoa(uid))
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return userInfo, isUserExists, err
	}
	isUserExists = len(value) != 0
	//log.T("isUserExists=%v value len=%v value: ['%v']  ", isUserExists, len(value), value)

	if isUserExists {
		err = proto.Unmarshal(value, &userInfo)
		if err != nil {
			log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		}
	}

	return userInfo, isUserExists, err
}

func GetUserInfoByUuid(uuid string) (userInfo bbproto.UserInfoDetail, isUserExists bool, err error) {
	db := &data.Data{}
	err = db.Open(cs.TABLE_USER)
	defer db.Close()
	if err != nil || len(uuid) <= 0 {
		return
	}

	var sUid string
	sUid, err = db.Get(cs.X_UUID + uuid)
	if err != nil {
		return
	}
	if len(sUid) == 0 {
		isUserExists = false
		return
	}

	uid := common.Atou(sUid)
	log.Printf("get uid by uuid('%v') ret err:%v, uid: %v", uuid, err, uid)

	return GetUserInfo(db, uid)
}

//get a new userid from db
func GetNewUserId() (userid uint32, err error) {
	db := &data.Data{}
	err = db.Open(cs.TABLE_USER)
	defer db.Close()
	if err != nil {
		return 0, err
	}

	uid, err := db.GetInt(cs.KEY_MAX_USER_ID)
	if err != nil {
		return 0, err
	}

	if uid == 0 {
		userid = 100 //first userId
	}

	userid += uint32(uid + 1)
	log.Printf("get MAX_USER_ID ret: %v ", userid)
	err = db.SetUInt(cs.KEY_MAX_USER_ID, userid)

	return userid, err
}

func RenameUser(uid uint32, newNickName string) (e Error.Error) {
	db := &data.Data{}
	err := db.Open(cs.TABLE_USER)
	defer db.Close()
	if err != nil {
		log.Error("[ERROR] CONNECT_DB_ERROR err:%v", err)
		return Error.New(cs.CONNECT_DB_ERROR, err)
	}

	var value []byte
	value, err = db.Gets(common.Utoa(uid))
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return Error.New(cs.READ_DB_ERROR, err)
	}

	if len(value) == 0 {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret value is empty.", uid)
		return Error.New(cs.EU_USER_NOT_EXISTS, err)
	}

	userDetail := &bbproto.UserInfoDetail{}
	err = proto.Unmarshal(value, userDetail)
	if err != nil {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return Error.New(cs.UNMARSHAL_ERROR)
	}

	//modify username
	userDetail.User.NickName = &newNickName

	//save data
	zUserData, err := proto.Marshal(userDetail)
	if err != nil {
		return Error.New(cs.MARSHAL_ERROR, err)
	}

	if err = db.Set(common.Utoa(*userDetail.User.UserId), zUserData); err != nil {
		log.Error("SET_DB_ERROR for userDetail: %v", *userDetail.User.UserId)
		return Error.New(cs.SET_DB_ERROR, err)
	}

	log.T("rename success: now nickName is:%v", *userDetail.User.NickName)
	return Error.OK()
}

func RetoreStamina(db *data.Data, uid uint32) (userDetail *bbproto.UserInfoDetail, e Error.Error) {
	if db == nil {
		db := &data.Data{}
		err := db.Open(cs.TABLE_USER)
		defer db.Close()
		if err != nil {
			log.Error("[ERROR] CONNECT_DB_ERROR err:%v", err)
			return nil, Error.New(cs.CONNECT_DB_ERROR, err)
		}
	} else {
		if err := db.Select(cs.TABLE_USER); err != nil {
			return nil, Error.New(cs.READ_DB_ERROR, err.Error())
		}
	}

	var value []byte
	value, err := db.Gets(common.Utoa(uid))
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return nil, Error.New(cs.READ_DB_ERROR, err)
	}

	if len(value) == 0 {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret value is empty.", uid)
		return nil, Error.New(cs.EU_USER_NOT_EXISTS, err)
	}

	userDetail = &bbproto.UserInfoDetail{}
	err = proto.Unmarshal(value, userDetail)
	if err != nil {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return nil, Error.New(cs.UNMARSHAL_ERROR)
	}

	//restore stamina
	userDetail.User.StaminaNow = userDetail.User.StaminaMax

	//save data
	zUserData, err := proto.Marshal(userDetail)
	if err != nil {
		return nil, Error.New(cs.MARSHAL_ERROR, err)
	}

	if err = db.Set(common.Utoa(*userDetail.User.UserId), zUserData); err != nil {
		log.Error("SET_DB_ERROR for userDetail: %v", *userDetail.User.UserId)
		return nil, Error.New(cs.SET_DB_ERROR, err)
	}

	log.T("user:%v restore stamina success, now stamina is: %v/%v", uid, *userDetail.User.StaminaNow, *userDetail.User.StaminaMax)
	return userDetail, Error.OK()
}

func RefreshStamina(tRecover *uint32, userStamina *int32, userStaminaMax int32) (e Error.Error) {
	if tRecover == nil || userStamina == nil {
		return Error.New(cs.INVALID_PARAMS, "invalid params")
	}

	tNow := common.Now()
	tElapse := int32(tNow - *tRecover)
	log.T("Old Stamina:%v tRecover:%v tElapse:%v ", userStamina, *tRecover, tElapse)
	*userStamina += (tElapse/cs.N_STAMINA_TIME + 1)
	log.T("Now Stamina:%v userStaminaMax:%v", *userStamina, userStaminaMax)

	if *userStamina > userStaminaMax {
		*userStamina = userStaminaMax
	}

	*tRecover = tNow + uint32(cs.N_STAMINA_TIME-tElapse%cs.N_STAMINA_TIME)

	return Error.OK()
}
