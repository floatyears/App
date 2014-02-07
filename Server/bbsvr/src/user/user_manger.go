// user_manger.go
package user

import (
	bbproto "../bbproto"
	"../common"
	proto "code.google.com/p/goprotobuf/proto"

	"../const"
	"../data"
	_ "fmt"
	"log"
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
	//userdetail.CurrentUnitParty = 0
	//userdetail.Account = xx //
	//userdetail.UnitPartyList = xx //
	//userdetail.Quest = xx //

	zUserData, err := proto.Marshal(userdetail)
	err = db.Set(common.Utoa(*userInfo.UserId), zUserData)
	log.Printf("[TRACE] db.Set(%v) save new userinfo, return err(%v)", *userInfo.UserId, err)
	if err != nil {
		return err
	}

	//save uuid -> uid
	err = db.Set(cs.X_UUID+uuid, []byte(common.Utoa(*userInfo.UserId)))

	return err
}

func GetUserInfo(uid uint32) (userInfo bbproto.UserInfoDetail, isUserExists bool, err error) {
	db := &data.Data{}
	err = db.Open(cs.TABLE_USER)
	defer db.Close()
	if err != nil || uid <= 0 {
		return
	}

	var value []byte
	value, err = db.Gets(common.Utoa(uid))
	log.Printf("GetUserInfo for '%v' ret err:%v, value: %v", uid, err, value)

	isUserExists = len(value) != 0
	log.Printf("isUserExists=%v value len=%v value: ['%v']  ", isUserExists, len(value), value)
	//userInfo = bbproto.UserInfoDetail{}
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
		userid = 100000 //first userId
	}
	userid += uint32(uid + 1)
	log.Printf("get MAX_USER_ID ret: %v ", userid)
	err = db.SetUInt(cs.KEY_MAX_USER_ID, userid)

	return userid, err
}
