package friend

import (
	"fmt"
	//"math/rand"
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
	//redis "github.com/garyburd/redigo/redis"
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

func UpdateHelperUsedRecord(db *data.Data, uid uint32, fid uint32) (e Error.Error) {
	if db == nil {
		return Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}
	if err := db.Select(consts.TABLE_FRIEND); err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	sUsedTime := common.Utoa(common.Now())
	if err := db.HSet(consts.X_HELPER_RECORD+common.Utoa(uid), common.Utoa(fid), []byte(sUsedTime)); err != nil {
		log.Error("db.HSet(%v,%v,%v) failed.", uid, fid, sUsedTime)
		return Error.New(EC.SET_DB_ERROR, err)
	}

	return Error.OK()
}

func GetHelperUsedRecord(db *data.Data, uid uint32, fid uint32) (usedTime uint32, e Error.Error) {
	if db == nil {
		return 0, Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}
	if err := db.Select(consts.TABLE_FRIEND); err != nil {
		return 0, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	value, err := db.HGet(consts.X_HELPER_RECORD+common.Utoa(uid), common.Utoa(fid))
	if err != nil {
		log.Error("db.HGet(%v,%v) failed.", uid, fid)
		return 0, Error.New(EC.READ_DB_ERROR, err)
	}

	usedTime = common.Atou(string(value))

	return usedTime, Error.OK()
}

func GetFriendPoint(db *data.Data, uid uint32, fid uint32)  (friendPoint int32, e Error.Error) {
	friendPoint = 0
	usedTime, e := GetHelperUsedRecord(db, uid, fid)
	if e.IsError() {
		return friendPoint, e
	} else if usedTime==0 || !common.IsToday(usedTime) {
		friendData, e := GetOneFriendData(db, uid, fid)
		if e.IsError() {
			return friendPoint, e
		}

		if friendData != nil && friendData.FriendState!=nil && *friendData.FriendState == bbproto.EFriendState_ISFRIEND {
			friendPoint = consts.N_FRIEND_HELPER_POINT //10 points
		}else {
			friendPoint = consts.N_SUPPORT_HELPER_POINT //5 points
		}
	}
	log.T("GetFriendPoint :: uid:%v fid:%v, usedTime:%v => friendPoint:%v",uid, fid, usedTime, friendPoint)

	return friendPoint, e
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

func DelFriend(db *data.Data, uid uint32, fids []uint32) (num int, err error) {
	//delete friend from me
	sFids := make([]string, len(fids))
	for k, uFid := range fids {
		sFid := common.Utoa(uFid)
		sFids[k] = sFid
		num, err = db.HDel(common.Utoa(uid), sFid)
		if err != nil {
			return num, err
		}
		log.T("uid(%v) DelFriend: fids:%v ret delNum:%v", uid, sFids, num)
	}

	//delete me from friend
	for k, fid := range sFids {
		num, err = db.HDel(fid, common.Utoa(uid))
		log.T("Del me(%v) from Friend[%v]: fid=%v ret delNum:%v", uid, k, fid, num)
	}

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

func SetHelperUidRank(db *data.Data, uid uint32, rank uint32) (e Error.Error) {
	sUserSpace := consts.X_USER_RANK + strconv.Itoa(int(uid%consts.N_USER_SPACE_PARTS))

	if db == nil {
		return Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}
	if err := db.Select(consts.TABLE_FRIEND); err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	err := db.ZAdd(sUserSpace, common.Utoa(uid), int32(rank))
	if err != nil {
		log.Error("db.ZAdd(%v, %v, %v) return err:%v", sUserSpace, uid, rank, err)
		return Error.New(EC.SET_DB_ERROR, err.Error())
	}

	log.T("SetHelperUidRank :: db.ZAdd(%v, %v, %v) success.", sUserSpace, uid, rank)

	return Error.OK()
}
