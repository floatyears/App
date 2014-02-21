// user_manger.go
package usermanage

import (
	bbproto "../../bbproto"
	"../../common"
	"../../common/Error"
	"../../common/log"
	"../../const"
	"../../data"
	proto "code.google.com/p/goprotobuf/proto"
	_ "fmt"
)

func AddNewUser(uuid string, userInfo *bbproto.UserInfo) (err error) {
	db := &data.Data{}
	err = db.Open(cs.TABLE_USER)
	defer db.Close()
	if err != nil || *userInfo.UserId <= 0 {
		return
	}

	userdetail := &bbproto.UserInfoDetail{}
	userdetail.User = userInfo

	//TODO: fill other default values
	//userdetail.CurrentUnitParty = 0
	//userdetail.Account = xx //
	//userdetail.UnitPartyList = xx //
	//userdetail.Quest = xx //

	userdetail.Login = &bbproto.LoginInfo{}
	userdetail.Login.LoginTotal = proto.Int32(1)
	userdetail.Login.LoginChain = proto.Int32(0)
	userdetail.Login.LastLoginTime = proto.Uint32(common.Now())
	userdetail.Login.LastPlayTime = userdetail.Login.LastLoginTime

	zUserData, err := proto.Marshal(userdetail)
	err = db.Set(common.Utoa(*userInfo.UserId), zUserData)
	log.T("db.Set(%v) save new userinfo, return err(%v)", *userInfo.UserId, err)
	if err != nil {
		return err
	}

	//save uuid -> uid
	err = db.Set(cs.X_UUID+uuid, []byte(common.Utoa(*userInfo.UserId)))

	return err
}

func UpdateUserInfo(db *data.Data, userdetail *bbproto.UserInfoDetail) (e Error.Error) {
	if db == nil {
		return Error.New(cs.INVALID_PARAMS, "invalid db pointer")
	}

	zUserData, err := proto.Marshal(userdetail)
	if err != nil {
		return Error.New(cs.MARSHAL_ERROR, err.Error())
	}

	if err = db.Select(cs.TABLE_USER); err != nil {
		return Error.New(cs.SET_DB_ERROR, err.Error())
	}

	if err = db.Set(common.Utoa(*userdetail.User.UserId), zUserData); err != nil {
		return Error.New(cs.SET_DB_ERROR, err.Error())
	}
	log.T("UpdateUserInfo for (%v) , return OK", *userdetail.User.UserId)

	return Error.OK()
}

func GetUserInfo(uid uint32) (userInfo bbproto.UserInfoDetail, isUserExists bool, err error) {
	isUserExists = false

	db := &data.Data{}
	err = db.Open(cs.TABLE_USER)
	defer db.Close()
	if err != nil || uid <= 0 {
		return
	}

	var value []byte
	value, err = db.Gets(common.Utoa(uid))
	if err != nil {
		log.Printf("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return userInfo, isUserExists, err
	}
	isUserExists = len(value) != 0
	//log.Printf("isUserExists=%v value len=%v value: ['%v']  ", isUserExists, len(value), value)

	if isUserExists {
		err = proto.Unmarshal(value, &userInfo)
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

	return GetUserInfo(uid)
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
