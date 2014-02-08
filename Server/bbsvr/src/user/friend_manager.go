package user

import (
	"fmt"
	"log"
	//"net/http"
	//"time"
)
import (
	bbproto "../bbproto"
	"../common"
	"../const"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
	redis "github.com/garyburd/redigo/redis"
)

func AddFriend(db *data.Data, sUid string, fid uint32, friendState bbproto.EFriendState, updateTime uint32) error {

	friendData := &bbproto.FriendData{}
	friendData.UserId = &fid
	friendData.FriendState = &friendState
	friendData.FriendStateUpdate = &updateTime
	friend, err := proto.Marshal(friendData)
	log.Printf("AddFriend uid:%v, fid:%v, state:%v", sUid, fid, friendState)
	if err != nil {
		return err
	}
	err = db.HSet(sUid, common.Utoa(fid), friend)

	return err
}

func DelFriend(db *data.Data, sUid string, fid uint32) (err error) {
	err = db.HDel(sUid, common.Utoa(fid))

	return err
}

func GetFriendsData(db *data.Data, sUid string, friendsInfo map[string]bbproto.FriendInfo) (err error) {
	if db == nil {
		return fmt.Errorf("[ERROR] db pointer is nil.")
	}

	log.Printf("[TRACE] begin friendsInfo[%v] is: %v", sUid, len(friendsInfo))

	zFriendDatas, err := db.HGetAll(sUid)
	if err != nil {
		log.Printf("[FATAL] HGetAll('%v') ret err:%v", sUid, err)
		return err
	}

	log.Printf("[TRACE] TABLE_FRIEND :: HGetAll('%v') ret err:%v, friendsInfo: %v",
		sUid, err, friendsInfo)

	friendNum := len(zFriendDatas) / 2

	log.Printf("[TRACE] GetFiendsData:: friendNum=%v friendsInfo len:%v", friendNum, len(friendsInfo))

	for i := 0; len(zFriendDatas) > 0; i++ {
		var sFid, sFridata []byte
		zFriendDatas, err = redis.Scan(zFriendDatas, &sFid, &sFridata)
		if err != nil {
			continue
		}

		friendData := &bbproto.FriendData{}
		err = proto.Unmarshal(sFridata, friendData) //unSerialize to friend
		if err != nil {
			log.Printf("[ERROR] unSerialize FriendData '%v' ret err:%v. sFridata:%v", sFid, err, sFridata)
			return err
		}

		//assign friend data fields
		friInfo := bbproto.FriendInfo{}
		friInfo.UserId = friendData.UserId
		friInfo.FriendState = friendData.FriendState
		friInfo.FriendStateUpdate = friendData.FriendStateUpdate
		friendsInfo[string(sFid)] = friInfo

		log.Printf("[TRACE] got friendData[%v]: %v", sFid, friInfo)
	}

	log.Printf("[TRACE] now friendsInfo[%v] is: %v", sUid, len(friendsInfo))

	return err
}

func GetFriendInfo(db *data.Data, uid uint32) (friendsInfo map[string]bbproto.FriendInfo, err error) {
	if db == nil {
		return friendsInfo, fmt.Errorf("[ERROR] db pointer is nil.")
	}

	friendsInfo = make(map[string]bbproto.FriendInfo)

	sUid := common.Utoa(uid)
	err = GetFriendsData(db, sUid, friendsInfo)
	if err != nil {
		log.Printf("[FATAL] GetFriendsData('%v') ret err:%v", sUid, err)
		return friendsInfo, err
	}

	log.Printf("[TRACE] +++ friendsInfo[%v] is: %v", sUid, len(friendsInfo))

	err = GetFriendsData(db, cs.X_FRIEND_HELPER+sUid, friendsInfo)
	if err != nil {
		log.Printf("[FATAL] GetFriendsData('%v') ret err:%v", cs.X_FRIEND_HELPER+sUid, err)
		return friendsInfo, err
	}

	// get userinfo by friends's uid
	fids := redis.Args{}
	for _, friInfo := range friendsInfo {
		fids = fids.Add(common.Utoa(*friInfo.UserId))
	}

	if err = db.Select(cs.TABLE_USER); err != nil {
		return
	}

	userinfos, err := db.MGet(fids)
	if err != nil {
		return
	}

	for _, uinfo := range userinfos {
		user := bbproto.UserInfo{}
		zUser, err := redis.Bytes(uinfo, err)
		if err == nil {
			err = proto.Unmarshal(zUser, &user) //unSerialize
		} else {

		}
		log.Printf("userId: %v -> name:%v rank:%v LoginTime:%v",
			*user.UserId, *user.UserName, *user.Rank, *user.LoginTime)

		uid = *user.UserId
		friInfo, ok := friendsInfo[common.Utoa(uid)]
		if ok {
			friInfo.Rank = user.Rank
			friInfo.UserName = user.UserName
			friInfo.LastPlayTime = user.LoginTime
			//friInfo.UnitId = uint(10) // TODO: add leader's unitId to userinfo
		} else {
			log.Printf("[ERROR] cannot find friInfo for: %v.", uid)
		}
	}
	log.Printf("friends's fids:%v userinfos:%v", fids, userinfos)
	log.Println("===========GetFriends finished.==========\n")

	return friendsInfo, err
}
