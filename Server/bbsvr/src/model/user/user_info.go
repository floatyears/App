package user

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	//"model/unit"
)

func UpdateUserInfo(db *data.Data, userdetail *bbproto.UserInfoDetail) (e Error.Error) {
	if db == nil {
		return Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}

	if err := db.Select(consts.TABLE_USER); err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	zUserData, err := proto.Marshal(userdetail)
	if err != nil {
		return Error.New(EC.MARSHAL_ERROR, err.Error())
	}

	if err = db.Set(common.Utoa(*userdetail.User.UserId), zUserData); err != nil {
		return Error.New(EC.SET_DB_ERROR, err.Error())
	}
	log.T("UpdateUserInfo for (%v) , return OK", *userdetail.User.UserId)

	return Error.OK()
}

//TODO: may return *bbproto.UserInfoDetail
func GetUserInfo(db *data.Data, uid uint32) (userInfo bbproto.UserInfoDetail, isUserExists bool, err error) {
	isUserExists = false

	if db == nil {
		db = &data.Data{}
		err = db.Open(consts.TABLE_USER)
		defer db.Close()
		if err != nil {
			return
		}
	} else {
		if err := db.Select(consts.TABLE_USER); err != nil {
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
	err = db.Open(consts.TABLE_USER)
	defer db.Close()
	if err != nil || len(uuid) <= 0 {
		return
	}

	var sUid string
	sUid, err = db.Get(consts.X_UUID + uuid)
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
