package unit

import (
	"fmt"
	"net/http"
	//"time"
)

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common/EC"
	"common/Error"
	//"common/consts"
	"common"
	"common/log"
	"data"
	"model/friend"
	"model/quest"
	"model/unit"
	"model/user"
)

/////////////////////////////////////////////////////////////////////////////

func EvolveDoneHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqEvolveDone
	rspMsg := &bbproto.RspEvolveDone{}

	handler := &EvolveDone{}
	e := handler.ParseInput(req, &reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
		return
	}

	e = handler.verifyParams(&reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
		return
	}

	// game logic

	e = handler.ProcessLogic(&reqMsg, rspMsg)

	e = handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, e))
	log.Printf("sendrsp err:%v, rspMsg.Header: %+v", e, rspMsg.Header)
}

/////////////////////////////////////////////////////////////////////////////

type EvolveDone struct {
	bbproto.BaseProtoHandler
}

func (t EvolveDone) verifyParams(reqMsg *bbproto.ReqEvolveDone) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.QuestId == nil || reqMsg.GetUnit == nil ||
		reqMsg.HitGrid == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if reqMsg.GetMoney == nil {
		reqMsg.GetMoney = proto.Int32(0)
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

func (t EvolveDone) FillResponseMsg(reqMsg *bbproto.ReqEvolveDone, rspMsg *bbproto.RspEvolveDone, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header.Error = proto.String(rspErr.Error())
	}

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func (t EvolveDone) ProcessLogic(reqMsg *bbproto.ReqEvolveDone, rspMsg *bbproto.RspEvolveDone) (e Error.Error) {
	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
		return Error.New(EC.CONNECT_DB_ERROR, err)
	}
	uid := *reqMsg.Header.UserId

	//1. get userinfo
	userDetail, exists, err := user.GetUserInfo(db, uid)
	if err != nil || !exists {
		log.Error("getUserInfo(%v) failed.", uid)
		return Error.New(EC.EU_GET_USERINFO_FAIL, err)
	}

	if userDetail.Quest == nil {
		log.Error("user(%v) not playing quest now, cannot clear quest.", uid)
		return Error.New(EC.EQ_USER_QUEST_NOT_PLAYING, "user not playing quest now, cannot finish evolve quest.")
	}

	log.T("userDetail.Quest:%+v", userDetail.Quest)

	//2. get evolve_start info
	reqEvolveStart, e := unit.CheckEvolveSession(db, uid)
	if e.IsError() {
		return e
	}

	stageId := *userDetail.Quest.StageId
	questId := *reqEvolveStart.EvolveQuestId
	gotMoney := *reqMsg.GetMoney
	gotExp := int32(0)
	gotStone := int32(0)
	gotFriendPt := int32(0)

	//3. getUnitInfo of baseUniqueId
	baseUserUnit, e := unit.GetUserUnitInfo(userDetail, *reqEvolveStart.BaseUniqueId)
	if e.IsError() {
		log.Error("GetUserUnitInfo(%v) failed: %v", *reqEvolveStart.BaseUniqueId, e.Error())
		return e
	}
	baseUnit, e := unit.GetUnitInfo(*baseUserUnit.UnitId)
	if e.IsError() {
		log.Error("GetUnitInfo(%v) failed: %v", *baseUserUnit.UnitId, e.Error())
		return e
	}
	log.T("baseUserUnit:(%+v).", baseUserUnit)
	log.T("baseUnit:(%+v).", baseUnit)

	if baseUnit.EvolveInfo == nil {
		e = Error.New(EC.E_UNIT_HAS_NO_EVOLVEINFO, fmt.Sprintf("unit(%v) has no evolve info.", *baseUnit.Id))
		log.Error(e.Error())
		return e
	}

//	// verify unit type
//	if *unit.GetTodayEvolveType() != *baseUnit.Type {
//		e = Error.New(EC.E_UNIT_CANNOT_EVOLVE_TODAY, fmt.Sprintf("unit type %v cannot evolve today.", *baseUnit.Type))
//		return e
//	}

	//4. update questPlayRecord (also add dropUnits to user.UnitList)

	gotMoney, gotExp, gotFriendPt, rspMsg.GotUnit, e =
		quest.UpdateQuestLog(db, userDetail, questId, reqMsg.GetUnit, gotMoney)
	if e.IsError() {
		return e
	}

	//5. malloc new evolved unit
	newUniqueId, e := unit.GetUnitUniqueId(db, uid, len(userDetail.UnitList))
	rspMsg.EvolvedUnit = &bbproto.UserUnit{}
	rspMsg.EvolvedUnit.UniqueId = proto.Uint32(newUniqueId)
	rspMsg.EvolvedUnit.UnitId = baseUnit.EvolveInfo.EvolveUnitId
	rspMsg.EvolvedUnit.AddAttack = baseUserUnit.AddAttack
	rspMsg.EvolvedUnit.AddHp = baseUserUnit.AddHp
	rspMsg.EvolvedUnit.AddDefence = baseUserUnit.AddDefence
	rspMsg.EvolvedUnit.IsFavorite = baseUserUnit.IsFavorite
	rspMsg.EvolvedUnit.Exp = proto.Int32(0)
	rspMsg.EvolvedUnit.Level = proto.Int32(1)
	rspMsg.EvolvedUnit.GetTime = proto.Uint32(common.Now())

	//6. remove BaseUnit & partUnits
	reqEvolveStart.PartUniqueId = append(reqEvolveStart.PartUniqueId, *reqEvolveStart.BaseUniqueId)
	if e = unit.RemoveMyUnit(&userDetail.UnitList, reqEvolveStart.PartUniqueId); e.IsError() {
		return e
	}

	//7. update exp, rank
	*userDetail.User.Exp += gotExp

	addRank,addCostMax, addFriendMax, addUnitMax,addStaminaMax, e := user.RefreshRank(userDetail.User)
	if e.IsError() { return e }
	if addRank > 0 { rspMsg.RankAdd = proto.Int32( addRank ) }
	if addCostMax > 0 { rspMsg.CostMaxAdd = proto.Int32( addCostMax ) }
	if addFriendMax > 0 { rspMsg.FriendMaxAdd = proto.Int32( addFriendMax ) }
	if addStaminaMax > 0 { rspMsg.StaminaMaxAdd = proto.Int32( addStaminaMax ) }
	if addUnitMax > 0 { rspMsg.UnitMaxAdd = proto.Int32( addUnitMax ) }


	//check stage isClear or not, give gotStone gift
	stageInfo, e := quest.GetStageInfo(db, stageId)
	if e.IsError() {
		log.Error("GetStageInfo(%v) error: %v", stageId, e.Error())
		return e
	}

	//TODO: 9.try getFriendState(helperUid) -> getFriendPoint
	friendPoint, e := friend.GetFriendPoint(db, uid, *reqEvolveStart.HelperUserId)
	if e.IsError() {
		return e
	}
	rspMsg.FriendPoint = proto.Int32(friendPoint)

	if e = friend.UpdateHelperUsedRecord(db, uid, *reqEvolveStart.HelperUserId); e.IsError() {
		return e
	}

	_, lastNotClear, e := quest.IsStageCleared(db, uid, stageId, stageInfo)
	if e.IsError() {
		return e
	} else if lastNotClear {
		gotStone = 1
	}

	if e = quest.SetQuestCleared(db, userDetail, stageId, questId, stageInfo, lastNotClear); e.IsError() {
		return e
	}

	//8. update userinfo
	if e = user.UpdateUserInfo(db, userDetail); e.IsError() {
		return e
	}

	//9. fill response
	rspMsg.Exp = userDetail.User.Exp
	rspMsg.Rank = userDetail.User.Rank
	rspMsg.Money = userDetail.Account.Money
	rspMsg.GotExp = proto.Int32(gotExp)
	rspMsg.GotStone = proto.Int32(gotStone)
	rspMsg.GotMoney = proto.Int32(gotMoney)
	rspMsg.GotFriendPoint = proto.Int32(gotFriendPt)
	rspMsg.StaminaNow = userDetail.User.StaminaNow
	rspMsg.StaminaMax = userDetail.User.StaminaMax
	rspMsg.StaminaRecover = userDetail.User.StaminaRecover

	log.T("=================EvolveDone rspMsg begin==================")
	log.T("\t rspMsg:%+v", rspMsg)
	log.T(">>>>>>>>>>>>>>>>>>>EvolveDone Rsp End<<<<<<<<<<<<<<<<<<<<")

	return e
}
