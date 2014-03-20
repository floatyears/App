package quest

import (
	"fmt"
	"net/http"
	//"time"
)

import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/log"
	"data"
	"model/quest"
	"model/user"

	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func ClearQuestHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqClearQuest
	rspMsg := &bbproto.RspClearQuest{}

	handler := &ClearQuest{}
	e := handler.ParseInput(req, &reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, Error.New(EC.INVALID_PARAMS, e.Error())))
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
	log.T("sendrsp err:%v, rspMsg:\n%+v", e, rspMsg)
}

/////////////////////////////////////////////////////////////////////////////

type ClearQuest struct {
	bbproto.BaseProtoHandler
}

func (t ClearQuest) FillResponseMsg(reqMsg *bbproto.ReqClearQuest, rspMsg *bbproto.RspClearQuest, rspErr Error.Error) (outbuffer []byte) {
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

func (t ClearQuest) verifyParams(reqMsg *bbproto.ReqClearQuest) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.GetMoney == nil || //reqMsg.HitGrid == nil ||
		reqMsg.GetUnit == nil || reqMsg.QuestId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.QuestId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	return Error.OK()
}

func (t ClearQuest) ProcessLogic(reqMsg *bbproto.ReqClearQuest, rspMsg *bbproto.RspClearQuest) (e Error.Error) {
	cost := &common.Cost{}
	cost.Begin()

	questId := *reqMsg.QuestId
	uid := *reqMsg.Header.UserId

	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
		log.Error("open TABLE_QUEST failed.")
		return Error.New(EC.CONNECT_DB_ERROR, err.Error())
	}

	//1. get userinfo from user table
	userDetail, isUserExists, err := user.GetUserInfo(db, uid)
	if err != nil {
		return Error.New(EC.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", uid, err.Error()))
	}
	if !isUserExists {
		return Error.New(EC.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
	}
	log.T("getUser(%v) ret userinfo: %v", uid, userDetail.User)

	if userDetail.Quest == nil {
		log.Error("user(%v) not playing quest now, cannot clear quest.", uid)
		return Error.New(EC.EQ_USER_QUEST_NOT_PLAYING, "user not playing quest now, cannot clear quest.")
	}

	gotMoney := *reqMsg.GetMoney
	gotExp := int32(0)
	gotStone := int32(0)
	gotFriendPt := int32(0)

	//2. check stage isClear or not, give gotStone gift
	stageId := *userDetail.Quest.StageId
	stageInfo, e := quest.GetStageInfo(db, stageId)
	if e.IsError() {
		log.Error("GetStageInfo(%v) error: %v", stageId, e.Error())
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

	//3. update questPlayRecord (also add dropUnits to user.UnitList)
	gotMoney, gotExp, gotFriendPt, rspMsg.GotUnit, e =
		quest.UpdateQuestLog(db, userDetail, questId, reqMsg.GetUnit, gotMoney)
	if e.IsError() {
		return e
	}

	//4. update exp, rank, account
	*userDetail.User.Exp += gotExp
	*userDetail.Account.Money += gotMoney
	user.RefreshRank(userDetail.User)

	log.T("==Account :: addMoney:+%v -> %v addExp:+%v -> user.Exp:%v", gotMoney, *userDetail.Account.Money, gotExp, *userDetail.User.Exp)

	//5. calculate stamina
	if e = user.RefreshStamina(userDetail.User.StaminaRecover, userDetail.User.StaminaNow, *userDetail.User.StaminaMax); e.IsError() {
		return e
	}

	//6. update userinfo (include: unitList, exp, money, stamina)
	if e = user.UpdateUserInfo(db, userDetail); e.IsError() {
		return Error.New(EC.EU_UPDATE_USERINFO_ERROR, "update userinfo failed.")
	}
	log.T("UpdateUserInfo(%v) ret OK.", uid)

	//6. fill response
	rspMsg.Rank = userDetail.User.Rank
	rspMsg.Exp = userDetail.User.Exp
	rspMsg.StaminaNow = userDetail.User.StaminaNow
	rspMsg.StaminaMax = userDetail.User.StaminaMax
	rspMsg.StaminaRecover = userDetail.User.StaminaRecover
	rspMsg.Money = userDetail.Account.Money
	rspMsg.FriendPoint = userDetail.Account.FriendPoint
	rspMsg.GotExp = proto.Int32(gotExp)
	rspMsg.GotStone = proto.Int32(gotStone)
	rspMsg.GotMoney = proto.Int32(gotMoney)
	//rspMsg.GotUnit = gotUnit
	rspMsg.GotFriendPoint = proto.Int32(gotFriendPt)

	log.T("=========== ClearQuest total cost %v ms. ============\n\n", cost.Cost())

	return Error.OK()
}
