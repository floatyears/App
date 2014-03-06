package friend

import (
	"fmt"
	"math/rand"
	"strconv"
	//"net/http"
	//"time"
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

func AddFriend(db *data.Data, uid uint32, fid uint32, friendState bbproto.EFriendState, updateTime uint32) (e Error.Error) {
	//first check friend exists or not
	friendData := &bbproto.FriendData{}
	e = findFriendData(db, common.Utoa(uid), fid, friendData)
	if !e.IsError() { //already exists FriendData
		if friendState == bbproto.EFriendState_FRIENDOUT { //request is addFriend
			if friendData.FriendState != nil {
				if *friendData.FriendState == bbproto.EFriendState_FRIENDOUT {
					return Error.New(EC.EF_IS_ALREADY_FRIENDOUT,
						fmt.Sprintf("already request add %v before, cannot add again.", fid))
				} else if *friendData.FriendState == bbproto.EFriendState_ISFRIEND {
					return Error.New(EC.EF_IS_ALREADY_FRIEND,
						fmt.Sprintf("user(%v) is already your friend, cannot add again.", fid))
				} else if *friendData.FriendState == bbproto.EFriendState_FRIENDIN {
					//friend request add me before I add him, directly accept it as friend now.
					friendState = bbproto.EFriendState_ISFRIEND
				}
			}
		} else if friendState == bbproto.EFriendState_ISFRIEND { //request is accept friendin
			if *friendData.FriendState == bbproto.EFriendState_ISFRIEND {
				return Error.New(EC.EF_IS_ALREADY_FRIEND,
					fmt.Sprintf("user(%v) is already your friend, cannot accept again.", fid))

			} else if *friendData.FriendState != bbproto.EFriendState_FRIENDIN {
				//invalid friend state
				return Error.New(EC.EF_INVALID_FRIEND_STATE,
					fmt.Sprintf("Unexcepted: user:%v friendState(%v) is invalid, cannot be accepted.", fid, *friendData.FriendState))
			}
		}
	} else { //findFriendData ret err!=nil, probably data not exists, or db error.
		log.T("[TRACE] findFriendData ret err: %v", e.Error())

		if e.Code() == EC.DATA_NOT_EXISTS {
			if friendState == bbproto.EFriendState_ISFRIEND { //request is accept friendin
				return Error.New(EC.EF_INVALID_FRIEND_STATE,
					fmt.Sprintf("Unexcepted: %v state is not FRIENDIN, cannot be accepted.", fid))
			}
		} else { //other db Error
			return e
		}
	}

	// add friend to me
	err := innerAddFriend(db, common.Utoa(uid), fid, friendState, updateTime)
	if err != nil {
		return Error.New(EC.EF_ADD_FRIEND_FAIL, err.Error())
	}

	// add me to friend
	if friendState == bbproto.EFriendState_FRIENDOUT {
		friendState = bbproto.EFriendState_FRIENDIN
	}
	err = innerAddFriend(db, common.Utoa(fid), uid, friendState, updateTime)
	if err != nil {
		return Error.New(EC.EF_ADD_FRIEND_FAIL, err.Error())
	}

	return Error.OK()
}

func AddHelper(db *data.Data, uid uint32, fid uint32, friendState bbproto.EFriendState, updateTime uint32) error {
	return innerAddFriend(db, consts.X_HELPER_MY+common.Utoa(uid), fid, friendState, updateTime)
}

func innerAddFriend(db *data.Data, sUid string, fid uint32, friendState bbproto.EFriendState, updateTime uint32) error {

	friendData := &bbproto.FriendData{}
	friendData.UserId = &fid
	friendData.FriendState = &friendState
	friendData.FriendStateUpdate = &updateTime
	friend, err := proto.Marshal(friendData)
	if err != nil {
		return err
	}
	err = db.HSet(sUid, common.Utoa(fid), friend)
	log.T("AddFriend uid:%v, fid:%v, state:%v  ret err:%v", sUid, fid, friendState, err)

	return err
}

func DelFriend(db *data.Data, uid uint32, fid uint32) (num int, err error) {
	//delete friend from me
	num, err = db.HDel(common.Utoa(uid), common.Utoa(fid))
	if err != nil {
		return num, err
	}

	//delete me from friend
	num, err = db.HDel(common.Utoa(fid), common.Utoa(uid))
	return num, err
}

//------------------------------------------------------
func findFriendData(db *data.Data, sUid string, fUid uint32, friendData *bbproto.FriendData) (e Error.Error) {
	if db == nil || sUid == "" || fUid == 0 || friendData == nil {
		log.Error("db == nil || sUid == '' || fUid == 0 || friendData == nil.")
		return Error.New(EC.INVALID_PARAMS)
	}

	zFriendData, err := db.HGet(sUid, common.Utoa(fUid))
	if err != nil {
		log.Fatal(" HGet(%v, %v) ret err:%v", sUid, fUid, err)
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	if len(zFriendData) == 0 {
		log.T("user: %v's friend(%v) data not exists.", sUid, fUid)
		return Error.New(EC.DATA_NOT_EXISTS)
	}

	log.T("TABLE_FRIEND :: HGet(%v, %v) ret err:%v, friendData: %v",
		sUid, fUid, err, friendData)

	err = proto.Unmarshal(zFriendData, friendData) //unSerialize to friend
	if err != nil {
		log.Error(" unSerialize FriendData '%v' ret err:%v. sFridata:%v", fUid, err, zFriendData)
		return Error.New(EC.UNMARSHAL_ERROR, err.Error())
	}

	return Error.OK()
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

		if isGetOnlyFriends && *friendData.FriendState != bbproto.EFriendState_ISFRIEND {
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
	count := 3 + rand.Intn(3)

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

func GetOnlyFriends(db *data.Data, uid uint32, rank uint32) (friendsInfo map[string]bbproto.FriendInfo, e Error.Error) {
	//get all friends & helper, but NOT include friendIn & friendOut
	return GetFriendInfo(db, uid, rank, true, true, true)
}

func GetFriendInfo(db *data.Data, uid uint32, rank uint32, isGetOnlyFriends bool, isGetFriend bool, isGetHelper bool) (friendsInfo map[string]bbproto.FriendInfo, e Error.Error) {
	if db == nil {
		return friendsInfo, Error.New(EC.INVALID_PARAMS, "[ERROR] db pointer is nil.")
	}
	if err := db.Select(consts.TABLE_FRIEND); err != nil {
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
