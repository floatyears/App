package user

import (
	//"fmt"
	"errors"
	"log"
	//"time"
)
import (
	bbproto "../bbproto"
	"../common"
	"../const"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

func UpdateLoginInfo(db *data.Data, userdetail *bbproto.UserInfoDetail) (err error) {
	tNow := common.Now() //- 86400*1

	if userdetail == nil || userdetail.Login == nil {
		return errors.New("ERROR: invalid input param: userdetail.Login")
	}

	if userdetail.Login.LastLoginTime == nil {
		userdetail.Login.LastLoginTime = &tNow
	}

	if !common.IsToday(*userdetail.Login.LastLoginTime) {
		*userdetail.Login.LoginTotal += 1

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
		return err
	}

	err = db.Set(common.Utoa(*userdetail.User.UserId), zUserData)
	log.Printf("[TRACE] UpdateLoginInfo for (%v) , return err(%v)", *userdetail.User.UserId, err)

	return err
}
