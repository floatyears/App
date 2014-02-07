package user

import (
	"fmt"
	"log"
	"math/rand"
	"strconv"
	//"net/http"
	"time"
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

func GetHelperData(db *data.Data, uid uint32, rank uint32, friendsInfo map[string]bbproto.FriendInfo) (err error) {
	sUserSpace := cs.X_USER_RANK + strconv.Itoa(int(uid%cs.N_USER_SPACE_PARTS))

	offset := rand.Intn(2)
	count := 3 + rand.Intn(3)

	minRank := 1
	if rank > cs.N_HELPER_RANK_RANGE {
		minRank = int(rank - cs.N_HELPER_RANK_RANGE)
	}

	zHelperIds, err := db.ZRangeByScore(sUserSpace,
		int(minRank), int(rank+cs.N_HELPER_RANK_RANGE),
		offset, count)

	log.Printf("[TRACE] ZRangeByScore(%v,[%v,%v],[%v,%v]) ret err:%v, got Id count:%v",
		sUserSpace, minRank, rank+cs.N_HELPER_RANK_RANGE, offset, count, err, len(zHelperIds))

	//helperCount := len(zHelperIds)
	//helperUids = make([]string, helperCount)

	for i := 0; len(zHelperIds) > 0; i++ {
		var sFid string
		zHelperIds, err = redis.Scan(zHelperIds, &sFid)
		if err != nil {
			continue
		}

		if err != nil {
			log.Printf("[ERROR] unSerialize FriendData '%v' ret err:%v", sFid, err)
			return
		}
		log.Printf("[TRACE] helper %v: fid=%v", i, sFid)

		//assign friend data fields
		uid := common.Atou(sFid)
		state := bbproto.EFriendState_FRIENDHELPER
		tNow := uint32(time.Now().Unix())
		friInfo := bbproto.FriendInfo{}
		friInfo.UserId = &uid
		friInfo.FriendState = &state
		friInfo.FriendStateUpdate = &tNow
		friendsInfo[string(sFid)] = friInfo
	}

	return
}

func GetFriendInfo(db *data.Data, uid uint32) (friendsInfo map[string]bbproto.FriendInfo, err error) {
	if db == nil {
		return friendsInfo, fmt.Errorf("[ERROR] db pointer is nil.")
	}

	friendsInfo = make(map[string]bbproto.FriendInfo)

	//get user's rank from user table
	userdetail, isUserExists, err := GetUserInfo(uid)
	if err != nil || !isUserExists {
		return
	}
	log.Printf("[TRACE] getUser(%v) ret userdetail: %v", uid, userdetail)
	rank := uint32(*userdetail.User.Rank)

	//get friends data
	sUid := common.Utoa(uid)
	err = GetFriendsData(db, sUid, friendsInfo)
	if err != nil {
		log.Printf("[FATAL] GetFriendsData('%v') ret err:%v", sUid, err)
		return friendsInfo, err
	}

	log.Printf("[TRACE] +++ friendsInfo[%v] is: %v", sUid, len(friendsInfo))

	//get helper data
	err = GetHelperData(db, uid, rank, friendsInfo)
	if err != nil {
		log.Printf("[FATAL] GetHelperData(%v,%v) ret err:%v", uid, rank, err)
		return friendsInfo, err
	}

	if len(friendsInfo) <= 0 {
		log.Printf("[ERROR] Cannot find any friends for uid:%v rank:%v", uid, rank)
		return
	}

	// get userinfo by uid
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
		log.Printf("user:%v", user)
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
