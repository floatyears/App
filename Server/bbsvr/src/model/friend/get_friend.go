package friend

import (
	"fmt"
	"math/rand"
	"strconv"
)

import (
	bbproto "bbproto"
	proto "code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	redis "github.com/garyburd/redigo/redis"
)

func GetOneFriendData(db *data.Data, uid uint32, fid uint32) (friendData *bbproto.FriendData, e Error.Error) {
	if db == nil {
		return friendData, Error.New(EC.INVALID_PARAMS, "[ERROR] db pointer is nil.")
	}
	if err := db.Select(consts.TABLE_FRIEND); err != nil {
		return friendData, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	sUid := common.Utoa(uid)
	zFriendData, err := db.HGet(sUid, common.Utoa(fid))
	if err != nil {
		log.Fatal(" HGetAll('%v') ret err:%v", sUid, err)
		return friendData, Error.New(EC.READ_DB_ERROR, err)
	}

	friendData = &bbproto.FriendData{}
	err = proto.Unmarshal(zFriendData, friendData)
	if err != nil {
		return friendData, Error.New(EC.READ_DB_ERROR, err)
	}

	return friendData, Error.OK()
}

func GetFriendsData(db *data.Data, sUid string, isGetOnlyFriends bool, friendsInfo map[string]bbproto.FriendInfo) (err error) {
	if db == nil {
		return fmt.Errorf("[ERROR] db pointer is nil.")
	}


	log.T("begin friendsInfo[%v] is: %v", sUid, len(friendsInfo))

	zFriendDatas, err := db.HGetAll(sUid)
	if err != nil {
		log.Fatal(" HGetAll('%v') ret err:%v", sUid, err)
		return err
	}

	log.T("TABLE_FRIEND :: HGetAll('%v') ret err:%v, friendsInfo: %v",
		sUid, err, friendsInfo)

	friendNum := len(zFriendDatas) / 2

	log.T("GetFiendsData:: friendNum=%v friendsInfo len:%v", friendNum, len(friendsInfo))
	for i := 0; len(zFriendDatas) > 0; i++ {
		var sFid, sFridata []byte
		zFriendDatas, err = redis.Scan(zFriendDatas, &sFid, &sFridata)
		if err != nil {
			continue
		}

		friendData := &bbproto.FriendData{}
		err = proto.Unmarshal(sFridata, friendData) //unSerialize to friend
		if err != nil {
			log.Error(" unSerialize FriendData '%v' ret err:%v. sFridata:%v", sFid, err, sFridata)
			return err
		}

		if isGetOnlyFriends && *friendData.FriendState != bbproto.EFriendState_ISFRIEND &&
			*friendData.FriendState != bbproto.EFriendState_FRIENDHELPER {
			log.T("isGetOnlyFriends:  skip -> (fid:%v, friendState:%v)", sFid, *friendData.FriendState)
			continue
		}

		//assign friend data fields
		friInfo := bbproto.FriendInfo{}
		friInfo.UserId = friendData.UserId
		friInfo.FriendState = friendData.FriendState
		friInfo.FriendStateUpdate = friendData.FriendStateUpdate
		friendsInfo[string(sFid)] = friInfo

		log.T("got friendData[%v]: %v", sFid, friInfo)
	}

	log.T("now friendsInfo[%v] is: %v", sUid, len(friendsInfo))

	return err
}

func GetHelperData(db *data.Data, uid uint32, rank uint32, friendsInfo map[string]bbproto.FriendInfo) (err error) {
	sUserSpace := consts.X_USER_RANK + strconv.Itoa(int(uid%consts.N_USER_SPACE_PARTS))

	offset := 0 //rand.Intn(2)
	count := 10 + rand.Intn(3)

	minRank := 1
	if rank > consts.N_HELPER_RANK_RANGE {
		minRank = int(rank - consts.N_HELPER_RANK_RANGE)
	}

	zHelperIds, err := db.ZRangeByScore(sUserSpace,
		int(minRank), int(rank+consts.N_HELPER_RANK_RANGE),
		offset, count)

	if err != nil {
		return err
	}

	log.T("ZRangeByScore(%v,[%v,%v],[%v,%v]) ret err:%v, got helper count:%v",
		sUserSpace, minRank, rank+consts.N_HELPER_RANK_RANGE, offset, count, err, len(zHelperIds))

	//helperCount := len(zHelperIds)
	//helperUids = make([]string, helperCount)

	for i := 0; len(zHelperIds) > 0; i++ {
		var sFid string
		zHelperIds, err = redis.Scan(zHelperIds, &sFid)
		if err != nil {
			log.Error(" redis.Scan(zHelperIds, &sFid) ret err:%v", err)
			continue
		}

		log.T("helper %v: fid=%v", i, sFid)

		//assign friend data fields
		uid := common.Atou(sFid)
		state := bbproto.EFriendState_FRIENDHELPER
		tNow := common.Now()
		friInfo := bbproto.FriendInfo{}
		friInfo.UserId = &uid
		friInfo.FriendState = &state
		friInfo.FriendStateUpdate = &tNow

		friendsInfo[string(sFid)] = friInfo
	}

	return
}

func GetSupportFriends(db *data.Data, uid uint32, rank uint32) (friendsInfo map[string]bbproto.FriendInfo, e Error.Error) {
	//get all friends & helper, but NOT include friendIn & friendOut
	friendsInfo, e = GetFriendInfo(db, uid, rank, true, true, true)
	if e.IsError() {
		return friendsInfo, e
	}

	// fill friend point
	for k, friInfo := range friendsInfo {

		if *friInfo.FriendState != bbproto.EFriendState_ISFRIEND {

			friInfo.FriendPoint = proto.Int32(consts.N_SUPPORT_HELPER_POINT) //5 points
			friendsInfo[k]=friInfo
			log.T("GetSupportFriends :: FriendPoint:%+v", *friInfo.FriendPoint)
			continue
		}

		//TODO: read multi-fid once from db (use HMGET)
		if usedTime, e := GetHelperUsedRecord(db, uid, *friInfo.UserId); !e.IsError() {
			if common.IsToday(usedTime) {
				log.T("GetHelperUsedRecord ret IsToday(%v)=true, set(uid:%v fid:%v) FriendPoint=0",usedTime, uid, *friInfo.UserId)
				friInfo.FriendPoint = proto.Int32(0)
				friendsInfo[k]=friInfo
			} else {
				log.T("GetHelperUsedRecord ret IsToday(%v)=false, set(uid:%v fid:%v) FriendPoint=0",usedTime, uid, *friInfo.UserId)
				friInfo.FriendPoint = proto.Int32(consts.N_FRIEND_HELPER_POINT) // 10 points
				friendsInfo[k]=friInfo
			}
		}else {
			log.T("GetHelperUsedRecord ret usedTime:%v, err:%v", usedTime, e.Error())
		}
	}
	log.T("GetSupportFriends :: friendsInfo:%+v", friendsInfo)

	return friendsInfo, e
}

func GetFriendNum(db *data.Data, uid uint32) (isFriendNum int32, e Error.Error) {
	isGetOnlyFriends := true // Get IsFriend
	isFriendNum = int32(0)

	//get friends data
	friendsInfo := make(map[string]bbproto.FriendInfo)
	err := GetFriendsData(db, common.Utoa(uid), isGetOnlyFriends, friendsInfo)
	if err != nil {
		log.Error(" GetFriendsData('%v') ret err:%v", uid, err)
		return isFriendNum, Error.New(err)
	}

	for _, friend := range friendsInfo {
		if friend.NickName == nil || friend.Rank == nil /*|| friend.Unit == nil*/ {
			log.Printf("[ERROR] unexcepted error: skip invalid friend(%v): %+v", *friend.UserId, friend)
			continue
		}

		if *friend.FriendState == bbproto.EFriendState_ISFRIEND {
			isFriendNum += 1
		}
	}

	log.T("user:%v GetFriendNum ret FriendNum: %v", uid, isFriendNum)
	return isFriendNum, Error.OK()
}

func GetFriendList(db *data.Data, uid uint32) (friendList *bbproto.FriendList, isFriendNum int32, e Error.Error) {
	isGetFriend := true
	isGetHelper := false
	isGetOnlyFriends := false
	rank := uint32(0)
	isFriendNum = int32(0)

	friendsInfo, e := GetFriendInfo(db, uid, rank, isGetOnlyFriends, isGetFriend, isGetHelper)

	log.T("GetFriendInfo ret err:%v. friends num=%v  ", e.Error(), len(friendsInfo))
	if e.IsError() && e.Code() != EC.EF_FRIEND_NOT_EXISTS {
		return friendList, isFriendNum, Error.New(EC.EF_GET_FRIENDINFO_FAIL, fmt.Sprintf("GetFriends failed for uid %v, rank:%v", uid, rank))
	}

	//fill response
	friendList = &bbproto.FriendList{}
	if friendsInfo != nil && len(friendsInfo) > 0 {
		for _, friend := range friendsInfo {
			if friend.NickName == nil || friend.Rank == nil /*|| friend.Unit == nil*/ {
				log.Printf("[ERROR] unexcepted error: skip invalid friend(%v): %+v", *friend.UserId, friend)
				continue
			}

			//log.T("fid:%v friend:%v", fid, *friend.UserId)
			pFriend := friend
			if *friend.FriendState == bbproto.EFriendState_FRIENDHELPER {
				friendList.Helper = append(friendList.Helper, &pFriend)
			} else if *friend.FriendState == bbproto.EFriendState_ISFRIEND {
				friendList.Friend = append(friendList.Friend, &pFriend)
				isFriendNum += 1
			} else if *friend.FriendState == bbproto.EFriendState_FRIENDIN {
				friendList.FriendIn = append(friendList.FriendIn, &pFriend)
			} else if *friend.FriendState == bbproto.EFriendState_FRIENDOUT {
				friendList.FriendOut = append(friendList.FriendOut, &pFriend)
			}
		}
	}

	return friendList, isFriendNum, Error.OK()
}

func GetFriendInfo(db *data.Data, uid uint32, rank uint32, isGetOnlyFriends bool, isGetFriend bool, isGetHelper bool) (friendsInfo map[string]bbproto.FriendInfo, e Error.Error) {
	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_FRIEND)
		defer db.Close()
		if err != nil {
			return friendsInfo, Error.New(EC.READ_DB_ERROR, err.Error())
		}
	} else if err := db.Select(consts.TABLE_FRIEND); err != nil {
		return friendsInfo, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	friendsInfo = make(map[string]bbproto.FriendInfo)

	//get friends data
	if isGetFriend {
		sUid := common.Utoa(uid)
		err := GetFriendsData(db, sUid, isGetOnlyFriends, friendsInfo)
		if err != nil {
			log.Fatal(" GetFriendsData('%v') ret err:%v", sUid, err)
			return friendsInfo, Error.New(err)
		}

		log.T("GetFriendsData ret total %v friends", len(friendsInfo))
	}

	//get helper data
	if isGetHelper {
		err := GetHelperData(db, uid, rank, friendsInfo)
		if err != nil {
			log.Fatal(" GetHelperData(%v,%v) ret err:%v", uid, rank, err)
			return friendsInfo, Error.New(err)
		}
	}

	log.T("GetHelperData ret total %v helpers", len(friendsInfo))
	if len(friendsInfo) <= 0 {
		//log.T(err.Error())
		return friendsInfo, Error.New(EC.EF_FRIEND_NOT_EXISTS, fmt.Sprintf("[ERROR] Cannot find any friends/helpers for uid:%v rank:%v", uid, rank))
	}

	// retrieve userinfo by uids from TABLE_USER
	fids := redis.Args{}
	for _, friInfo := range friendsInfo {
		fids = fids.Add(common.Utoa(*friInfo.UserId))
	}

	if err := db.Select(consts.TABLE_USER); err != nil {
		return friendsInfo, Error.New(EC.READ_DB_ERROR)
	}

	userinfos, err := db.MGet(fids)
	if err != nil {
		return friendsInfo, Error.New(EC.READ_DB_ERROR)
	}
	//log.T("TABLE_USER.MGet(fids:%v) ret %v", fids, userinfos)

	for k, uinfo := range userinfos {
		if uinfo == nil {
			continue
		}
		userDetail := bbproto.UserInfoDetail{}
		zUser, err := redis.Bytes(uinfo, err)
		if err == nil && len(zUser) > 0 {
			if err = proto.Unmarshal(zUser, &userDetail); err != nil {
				log.Error(" Cannot Unmarshal userinfo(err:%v) userinfo: %v", err, uinfo)
				return friendsInfo, Error.New(err)
			}
		} else {
			return friendsInfo, Error.New("redis.Bytes(uinfo, err) fail.")
		}

		user := userDetail.User

		if user == nil || user.UserId == nil {
			log.Fatal("unexcepted error: user.UserId is nil. user:%v", user)
			continue
		}
		log.T("friend[%v] userId: %v -> name:%v rank:%v ",
			k, *user.UserId, *user.NickName, *user.Rank)

		uid = *user.UserId
		friInfo, ok := friendsInfo[common.Utoa(uid)]
		if ok {
			friInfo.Rank = user.Rank
			friInfo.NickName = user.NickName
			friInfo.LastPlayTime = userDetail.Login.LastPlayTime
			friInfo.Unit = userDetail.User.Unit

			friendsInfo[common.Utoa(uid)] = friInfo

			//log.T("new friend uid:%v rank:%v username:%v lastPlay:%v",
			//	uid, *newfriInfo.Rank, *newfriInfo.UserName, *newfriInfo.LastPlayTime)

		} else {
			log.Error(" cannot find friInfo for: %v.", uid)
		}
	}
	log.T("\nfriends's fids:%v friendsInfo:%v", fids, friendsInfo)
	log.T("===========GetFriends finished.==========\n")

	return friendsInfo, Error.OK()
}
