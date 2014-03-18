package quest

import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"model/unit"
	"model/friend"

	"code.google.com/p/goprotobuf/proto"
)


type TUsedValue struct {
	Value int32
	Used  bool
}

//called in clear_quest: update userDetail.Quest=nil, add dropUnits to myUnitList.
func UpdateQuestLog(db *data.Data, userDetail *bbproto.UserInfoDetail, questId uint32,
	getUnit []uint32, getMoney int32) (gotMoney, gotExp, gotFriendPt int32, gotUnit []*bbproto.UserUnit, e Error.Error) {
	if db == nil {
		return 0, 0, 0, gotUnit, Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}

	if userDetail.Quest == nil {
		return 0, 0, 0, gotUnit, Error.New(EC.EQ_UPDATE_QUEST_RECORD_ERROR, "user.Quest is nil")
	}

	userDetail.Quest.EndTime = proto.Uint32(common.Now())

	//TODO: verity getMoney
	userDetail.Quest.GetMoney = proto.Int32(getMoney)

	//verify getUnit
	isAllValidUnit := true
	for _, dropIdGot := range getUnit {
		isValidOne := false
		for _, unitDrop := range userDetail.Quest.DropUnits {
			if *unitDrop.DropId == dropIdGot {
				isValidOne = true
				break
			}
		}
		if !isValidOne {
			log.Error("ClearQuest :: unitGot is invalid: %+v", dropIdGot)
			isAllValidUnit = false
			break
		}
	}

	if !isAllValidUnit {
		log.Error("clear request: invalid drop unit.")
		return 0, 0, 0, gotUnit, Error.New(EC.EQ_INVALID_DROP_UNIT, "clear request: invalid drop unit")
	}

	//append unit to userinfo.UnitList
	for _, clientDropId := range getUnit {

		for _, unitDrop := range userDetail.Quest.DropUnits {
			if *unitDrop.DropId != clientDropId {
				continue
			}

			uniqueId, e := unit.GetUnitUniqueId(db, *userDetail.User.UserId, len(userDetail.UnitList))
			if e.IsError() {
				return 0, 0, 0, gotUnit, e
			}

			userUnit := &bbproto.UserUnit{}
			userUnit.UniqueId = proto.Uint32(uniqueId)
			userUnit.UnitId = unitDrop.UnitId
			userUnit.Exp = proto.Int32(0)
			userUnit.Level = unitDrop.Level
			userUnit.AddHp = unitDrop.AddHp
			userUnit.AddAttack = unitDrop.AddAttack
			userUnit.AddDefence = unitDrop.AddDefence
			userUnit.GetTime = proto.Uint32(common.Now())

			userDetail.UnitList = append(userDetail.UnitList, userUnit)
			userDetail.Quest.GetUnit = append(userDetail.Quest.GetUnit, userUnit)

			gotUnit = append(gotUnit, userUnit) //return value
		}
	}

	//already fill in getUnit, so empty dropUnit before save to QuestLog
	userDetail.Quest.DropUnits = []*bbproto.DropUnit{}

	//save userDetail.Quest to QuestLog
	if e = SaveQuestLog(db, userDetail); e.IsError() {
		return
	}

	//update helper used time
	if e = friend.UpdateHelperUsedRecord(db, *userDetail.User.UserId, *userDetail.Quest.HelperUserId); e.IsError() {
		return
	}

	gotMoney = *userDetail.Quest.GetMoney
	gotExp = *userDetail.Quest.GetExp
	if userDetail.Quest.GetFriendPoint != nil {
		gotFriendPt = *userDetail.Quest.GetFriendPoint
	}

	userDetail.Quest = nil

	return gotMoney, gotExp, gotFriendPt, gotUnit, Error.OK()
}

//TODO: save questlog to a independent stat-server
func SaveQuestLog(db *data.Data, userDetail *bbproto.UserInfoDetail) (e Error.Error) {
	if db == nil || userDetail == nil || userDetail.Quest == nil {
		return Error.New(EC.INVALID_PARAMS, "SaveQuestLog invalid params")
	}

	zQuest, err := proto.Marshal(userDetail.Quest)
	if err != nil {
		return Error.New(EC.MARSHAL_ERROR)
	}

	if err := db.Select(consts.TABLE_QUEST_LOG); err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	uid := *userDetail.User.UserId
	questId := *userDetail.Quest.QuestId

	if err = db.HSet(consts.X_QUEST_LOG+common.Utoa(uid), common.Utoa(questId), zQuest); err != nil {
		log.Error("HSet(X_QUEST_LOG_%v, %v) failed:%v.", uid, questId, err)
		return Error.New(EC.READ_DB_ERROR)
	}

	return Error.OK()
}

//called in start_quest
func FillUserQuest(userDetail *bbproto.UserInfoDetail, currParty int32, helperUid uint32, helperUnit *bbproto.UserUnit,
	drops []*bbproto.DropUnit, stage *bbproto.StageInfo, quest *bbproto.QuestInfo, questState bbproto.EQuestState) (e Error.Error) {
	if userDetail.Quest == nil {
		userDetail.Quest = &bbproto.QuestLog{}
		userDetail.Quest.QuestId = quest.Id
		userDetail.Quest.StageId = stage.Id
		userDetail.Quest.StartTime = proto.Uint32(common.Now())
		//userDetail.Quest.EndTime = proto.Uint32(common.Now())
	}

	//fill getExp, getMoney
	getExp := *quest.RewardExp
	getMoney := *quest.RewardMoney
	if *stage.Boost.Type == bbproto.QuestBoostType_QB_BOOST_MONEY {
		log.T("boost money: %v x%v", getMoney, *stage.Boost.Value)
		getMoney *= *stage.Boost.Value
	}
	if *stage.Boost.Type == bbproto.QuestBoostType_QB_BOOST_EXP {
		log.T("boost Exp: %v x%v", getExp, *stage.Boost.Value)
		getExp *= *stage.Boost.Value
	}
	userDetail.Quest.GetExp = proto.Int32(getExp)
	userDetail.Quest.GetMoney = proto.Int32(getMoney)
	userDetail.Quest.CurrentParty = proto.Int32(currParty)
	userDetail.Quest.HelperUserId = proto.Uint32(helperUid)
	userDetail.Quest.HelperUnit = helperUnit

	userDetail.Quest.State = &questState

	//fill drop unit
	for _, dropUnit := range drops {
		if dropUnit != nil {
			userDetail.Quest.DropUnits = append(userDetail.Quest.DropUnits, dropUnit)
		}
	}

	return Error.OK()
}
