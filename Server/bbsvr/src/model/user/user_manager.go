// user_manger.go
package user

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/config"
	"common/consts"
	"common/log"
	"data"
	"model/unit"
)

func AddNewUser(db *data.Data, uuid string, selectRole uint32) (userdetail *bbproto.UserInfoDetail, e Error.Error) {
	if db == nil {
		return nil, Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}
	if err := db.Select(consts.TABLE_USER); err != nil {
		return nil, Error.New(EC.READ_DB_ERROR, err)
	}
	userdetail = &bbproto.UserInfoDetail{}

	newUserId, err := GetNewUserId()
	if err != nil {
		return nil, Error.New(EC.EU_GET_NEWUSERID_FAIL, err)
	}
	defaultName := consts.DEFAULT_USER_NAME
	tNow := common.Now()
	rank := int32(1) //int32(30 + common.Randn(10)) //
	exp := config.GetUserRankExp(rank) //((1+rank)*100*rank/2) + (100*rank/8*common.Rand(1, 7))
	staminaMax := config.GetStaminaMax( rank )
	staminaNow := staminaMax
	staminaRecover := uint32(tNow + 600) //10 minutes

	userdetail.User = &bbproto.UserInfo{
		UserId:         &newUserId,
		NickName:       &defaultName,
		Rank:           &rank,
		Exp:            &exp,
		StaminaNow:     &staminaNow,
		StaminaMax:     &staminaMax,
		StaminaRecover: &staminaRecover,
		FriendMax:    proto.Int32(config.GetFriendMax(rank)),
		UnitMax:     proto.Int32(config.GetUnitMax(rank)),
		CostMax:     proto.Int32(config.GetCostMax(rank)),
	}

	userdetail.Account = &bbproto.AccountInfo{
		PayTotal:       proto.Int32(0),
		PayMonth:       proto.Int32(0),
		Money:          proto.Int32(10000),
		StonePay:       proto.Int32(0),
		StoneFree:      proto.Int32(200),
		Stone:          proto.Int32(200),
		FriendPoint:    proto.Int32(10000),
		FirstSelectNum: proto.Int32(int32(selectRole)),
	}

	//TODO: fill other default values
	//userdetail.Quest = nil //

	userdetail.Login = &bbproto.LoginInfo{}
	userdetail.Login.LoginTotal = proto.Int32(1)
	userdetail.Login.LoginChain = proto.Int32(0)
	userdetail.Login.LastLoginTime = proto.Uint32(tNow)
	userdetail.Login.LastPlayTime = userdetail.Login.LastLoginTime

	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return nil, Error.New(EC.CONNECT_DB_ERROR, err)
		}
	} else if err := db.Select(consts.TABLE_UNIT); err != nil {
		return nil, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	//TODO: let user to select a actor.
//	unitIdPool := []uint32 {1,5,9,107,111,113,117,123,127,133,135,137,145,151,157,163,164,165,166,167,168,175,179,187,195,221,224,226,61,65,67,69,71,73,85,87,89,91}
	unitIdPool := []uint32 {1,5,9,49,50,51,52,53,54,61,63,64,65,66,73,75,76,77,78,89,90,152,153}
	for i := 0; i < len(unitIdPool); i++ {
		uniqueId, e := unit.GetUnitUniqueId(db, *userdetail.User.UserId, i-1)
		if e.IsError() {
			return nil, e
		}

		unitId :=uint32(unitIdPool[i])
		unitInfo, e :=unit.GetUnitInfo(unitId)
		if e.IsError() {
			continue
		}
		log.T("unitInfo[%v]:%+v", unitId,unitInfo)
		level:=common.Rand(1, int32(*unitInfo.MaxLevel))
		exp := unit.GetUnitExpByUnitId(unitId, level)
		if exp == 0 {
			log.Error("GetUnitExpByUnitId fail:%v",unitId)
			continue
		}
		log.T("make userunit for unitId[%v] level:%v exp:%v", unitId, level, exp-1)

		userUnit := &bbproto.UserUnit{
			UniqueId:  proto.Uint32(uniqueId),
			UnitId:    proto.Uint32(unitId),
			Exp:       proto.Int32(exp-1),
			Level:     proto.Int32(level),
			GetTime:   proto.Uint32(tNow),
			AddAttack: proto.Int32(common.Randn(int32(i) % 10)),
			AddHp:     proto.Int32(common.Randn(int32(i) % 10)),
			//ActiveSkillLevel: proto.Int32(1), //TODO: only assign it when levelUp match uprate
		}
		userdetail.UnitList = append(userdetail.UnitList, userUnit)

		if( unitId == selectRole ) {
			log.T("unitId==selectRole=%v  assign user.unit", selectRole)
			userdetail.User.Unit = userdetail.UnitList[len(userdetail.UnitList)-1] //currParty's leader
		}
	}

	log.T("selectRole=%v", selectRole)

	//make default party
	userdetail.Party = &bbproto.PartyInfo{}
	userdetail.Party.CurrentParty = proto.Int(0)
	for i := int32(0); i < 5; i++ { //5 group
		party := &bbproto.UnitParty{}
		party.Id = proto.Int32(i)
		for pos := int32(0); pos < 4; pos++ {
			item := &bbproto.PartyItem{}
			item.UnitPos = proto.Int32(pos)
			if i==*userdetail.Party.CurrentParty && pos == 0 { //currParty's leader
				item.UnitUniqueId = userdetail.User.Unit.UniqueId
			}else {
				for _, unit:= range userdetail.UnitList {
					if *unit.UniqueId ==  *userdetail.User.Unit.UniqueId { //leader is used
						continue
					}
					used:=false
					for _, it := range party.Items{
						if *unit.UniqueId == *it.UnitUniqueId { //other partyitem used
							used=true
							break
						}
					}
					if used {
						continue
					}
					item.UnitUniqueId = unit.UniqueId
				}
			}
			party.Items = append(party.Items, item)
		}
		userdetail.Party.PartyList = append(userdetail.Party.PartyList, party)
	}

	if err := db.Select(consts.TABLE_USER); err != nil {
		return nil, Error.New(EC.READ_DB_ERROR, err.Error())
	}

	zUserData, err := proto.Marshal(userdetail)
	err = db.Set(common.Utoa(*userdetail.User.UserId), zUserData)
	log.T("db.Set(%v) save new userinfo, return err(%v)", *userdetail.User.UserId, err)
	if err != nil {
		log.Error("set db(userinfo) ret error:%v", err)
		return nil, Error.New(EC.READ_DB_ERROR, err)
	}

	//save uuid -> uid
	err = db.Set(consts.X_UUID+uuid, []byte(common.Utoa(*userdetail.User.UserId)))
	if err != nil {
		log.Error("set db(uuid -> userId) ret error:%v", err)
		return nil, Error.New(EC.READ_DB_ERROR, err)
	}

	return userdetail, Error.OK()
}

//get a new userid from db
func GetNewUserId() (userid uint32, err error) {
	db := &data.Data{}
	err = db.Open(consts.TABLE_USER)
	defer db.Close()
	if err != nil {
		return 0, err
	}

	uid, err := db.GetInt(consts.KEY_MAX_USER_ID)
	if err != nil {
		return 0, err
	}

	if uid == 0 {
		userid = 100 //first userId
	}

	userid += uint32(uid + 1)
	log.Printf("get MAX_USER_ID ret: %v ", userid)
	err = db.SetUInt(consts.KEY_MAX_USER_ID, userid)

	return userid, err
}

func RenameUser(uid uint32, newNickName string) (e Error.Error) {
	db := &data.Data{}
	err := db.Open(consts.TABLE_USER)
	defer db.Close()
	if err != nil {
		log.Error("[ERROR] CONNECT_DB_ERROR err:%v", err)
		return Error.New(EC.CONNECT_DB_ERROR, err)
	}

	var value []byte
	value, err = db.Gets(common.Utoa(uid))
	if err != nil {
		log.Error("[ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return Error.New(EC.READ_DB_ERROR, err)
	}

	if len(value) == 0 {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret value is empty.", uid)
		return Error.New(EC.EU_USER_NOT_EXISTS, err)
	}

	userDetail := &bbproto.UserInfoDetail{}
	err = proto.Unmarshal(value, userDetail)
	if err != nil {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret err:%v", uid, err)
		return Error.New(EC.UNMARSHAL_ERROR)
	}

	//modify username
	userDetail.User.NickName = &newNickName

	//save data
	zUserData, err := proto.Marshal(userDetail)
	if err != nil {
		return Error.New(EC.MARSHAL_ERROR, err)
	}

	if err = db.Set(common.Utoa(*userDetail.User.UserId), zUserData); err != nil {
		log.Error("SET_DB_ERROR for userDetail: %v", *userDetail.User.UserId)
		return Error.New(EC.READ_DB_ERROR, err)
	}

	log.T("rename success: now nickName is:%v", *userDetail.User.NickName)
	return Error.OK()
}
