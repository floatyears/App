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
	//redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func StartQuestHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqStartQuest
	rspMsg := &bbproto.RspStartQuest{}

	handler := &StartQuest{}
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

type StartQuest struct {
	bbproto.BaseProtoHandler
}

func (t StartQuest) FillResponseMsg(reqMsg *bbproto.ReqStartQuest, rspMsg *bbproto.RspStartQuest, rspErr Error.Error) (outbuffer []byte) {
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

func (t StartQuest) verifyParams(reqMsg *bbproto.ReqStartQuest) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.StageId == nil || reqMsg.QuestId == nil {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.StageId == 0 || *reqMsg.QuestId == 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	return Error.OK()
}

func (t StartQuest) ProcessLogic(reqMsg *bbproto.ReqStartQuest, rspMsg *bbproto.RspStartQuest) (e Error.Error) {
	cost := &common.Cost{}
	cost.Begin()

	stageId := *reqMsg.StageId
	questId := *reqMsg.QuestId
	uid := *reqMsg.Header.UserId

	//get userinfo from user table
	userDetail, isUserExists, err := usermanage.GetUserInfo(uid)
	if err != nil {
		return Error.New(cs.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", uid, err.Error()))
	}
	if !isUserExists {
		return Error.New(cs.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
	}
	log.T(" getUser(%v) ret userinfo: %v", uid, userDetail.User)

	db := &data.Data{}
	err = db.Open(cs.TABLE_QUEST)
	defer db.Close()
	if err != nil {
		return Error.New(cs.CONNECT_DB_ERROR, err.Error())
	}

	//get questInfo
	stageInfo, e := GetStageInfo(db, stageId)
	if e.IsError() {
		return e
	}

	questInfo, e := GetQuestInfo(db, stageInfo, questId)
	if e.IsError() {
		return e
	}
	if questInfo == nil {
		return Error.New(cs.EQ_GET_QUESTINFO_ERROR, "GetQuestInfo ret ok, but result is nil.")
	}
	log.T("questInfo:%+v", questInfo)

	//update stamina
	log.T("--Old Stamina:%v staminaRecover:%v", *userDetail.User.StaminaNow, *userDetail.User.StaminaRecover)
	e = UpdateStamina(userDetail.User.StaminaRecover, userDetail.User.StaminaNow, *userDetail.User.StaminaMax, *questInfo.Stamina)
	if e.IsError() {
		return e
	}
	log.T("--New StaminaNow:%v staminaRecover:%v", *userDetail.User.StaminaNow, *userDetail.User.StaminaRecover)

	//get quest config
	questConf, e := GetQuestConfig(db, questId)
	log.T(" questConf:%+v", questConf)
	if e.IsError() {
		return e
	}

	//make quest data
	questData, e := MakeQuestData(&questConf)
	if e.IsError() {
		return e
	}

	//get quest record from QuestLog, fill to userDetail.Quest
	if e = GetQuestRecord(db, questId, &userDetail); e.IsError() {
		return e
	}

	//update latest quest record of userDetail
	if e = FillQuestRecord(&userDetail, questId, questData.Drop, stageInfo, questInfo); e.IsError() {
		return e
	}

	//save updated userinfo
	if e = usermanage.UpdateUserInfo(db, &userDetail); e.IsError() {
		return e
	}

	//fill response
	rspMsg.StaminaNow = proto.Int32(*userDetail.User.StaminaNow - *questInfo.Stamina)
	rspMsg.StaminaRecover = proto.Uint32(*userDetail.User.StaminaRecover)
	rspMsg.DungeonData = &questData

	log.T("=========== StartQuest total cost %v ms. ============\n\n", cost.Cost())

	return Error.OK()
}
