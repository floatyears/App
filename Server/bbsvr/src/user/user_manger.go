// user_manger.go
package user

import (
	//"../common"
	"../const"
	"../data"
	_ "fmt"
	"log"
)

func GetUserId(sessionId string) (userId string, err error) {
	//read from session table by sessionId
	return "", nil
}

func GetUserInfo(sessionId string) (userInfo string, err error) {

	return "", nil
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
