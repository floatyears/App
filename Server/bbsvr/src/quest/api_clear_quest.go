package quest

import (
	"fmt"
	"net/http"
	//"time"
)

import (
	"../bbproto"
	"../common"
	"../common/Error"
	"../common/log"
	"../const"
	"../data"
	"../user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func ClearQuestHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqClearQuest
	rspMsg := &bbproto.RspClearQuest{}

	handler := &ClearQuest{}
	e := handler.ParseInput(req, &reqMsg)
	if e.IsError() {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, Error.New(cs.INVALID_PARAMS, e.Error())))
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
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.QuestId == 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
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
		return Error.New(cs.CONNECT_DB_ERROR, err.Error())
	}

	//1. get userinfo from user table
	userDetail, isUserExists, err := usermanage.GetUserInfo(db, uid)
	if err != nil {
		return Error.New(cs.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", uid, err.Error()))
	}
	if !isUserExists {
		return Error.New(cs.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
	}
	log.T("getUser(%v) ret userinfo: %v", uid, userDetail.User)

	//2. update questPlayRecord (include )
	gotMoney := *reqMsg.GetMoney
	gotExp := int32(0)
	gotFriendPt := int32(0)

	gotMoney, gotExp, gotFriendPt, rspMsg.GotUnit, e =
		UpdateQuestLog(db, &userDetail, questId, reqMsg.GetUnit, gotMoney)
	if e.IsError() {
		return e
	}

	//3. calculate stamina
	if e = usermanage.RefreshStamina(userDetail.User.StaminaRecover, userDetail.User.StaminaNow, *userDetail.User.StaminaMax); e.IsError() {
		return e
	}

	//4. update userinfo (include: unitList, exp, money, stamina)
	if e = usermanage.UpdateUserInfo(db, &userDetail); e.IsError() {
		return Error.New(cs.EU_UPDATE_USERINFO_ERROR, "update userinfo failed.")
	}
	log.T("UpdateUserInfo(%v) ret OK.", uid)

	//5. fill response
	rspMsg.Rank = userDetail.User.Rank
	rspMsg.Exp = userDetail.User.Exp
	rspMsg.StaminaNow = userDetail.User.StaminaNow
	rspMsg.StaminaMax = userDetail.User.StaminaMax
	rspMsg.StaminaRecover = userDetail.User.StaminaRecover
	rspMsg.Money = userDetail.Account.Money
	rspMsg.FriendPoint = userDetail.Account.FriendPoint
	rspMsg.GotExp = proto.Int32(gotExp)
	rspMsg.GotMoney = proto.Int32(gotMoney)
	//rspMsg.GotUnit = gotUnit
	rspMsg.GotFriendPoint = proto.Int32(gotFriendPt)

	log.T("=========== ClearQuest total cost %v ms. ============\n\n", cost.Cost())

	return Error.OK()
}
