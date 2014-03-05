package user

import (
//"fmt"
)
import (
	bbproto "../bbproto"
	"../common"
	"../common/Error"
	"../common/log"
	"../const"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

func UpdateLoginInfo(db *data.Data, userdetail *bbproto.UserInfoDetail) (e Error.Error) {
	if db == nil {
		return Error.New(cs.INVALID_PARAMS, "invalid db pointer")
	}
	if userdetail == nil || userdetail.Login == nil {
		return Error.New("ERROR: invalid input param: userdetail.Login")
	}

	tNow := common.Now() //- 86400*1
	if userdetail.Login.LastLoginTime == nil {
		userdetail.Login.LastLoginTime = &tNow
	}

	*userdetail.Login.LoginTotal += 1

	if !common.IsToday(*userdetail.Login.LastLoginTime) {
		if common.IsYestoday(*userdetail.Login.LastLoginTime) {
			*userdetail.Login.LoginChain += 1
		} else {
			*userdetail.Login.LoginChain = 0
		}
	} else {
		log.Printf("[TRACE] lastLoginTime(%v) is today.", *userdetail.Login.LastLoginTime)
	}

	//update last Login time
	userdetail.Login.LastLoginTime = &tNow
	userdetail.Login.LastPlayTime = &tNow

	zUserData, err := proto.Marshal(userdetail)

	if err = db.Select(cs.TABLE_USER); err != nil {
		return Error.New(cs.READ_DB_ERROR)
	}

	if err = db.Set(common.Utoa(*userdetail.User.UserId), zUserData); err != nil {
		return Error.New(cs.SET_DB_ERROR, err)
	}
	log.Printf("[TRACE] UpdateLoginInfo for (%v) , return err(%v)", *userdetail.User.UserId, err)

	return Error.OK()
}
