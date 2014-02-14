package quest

import (
	"fmt"
	"log"
	"net/http"
	//"time"
)

import (
	"../bbproto"
	"../common"
	"../common/Error"
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
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", e, rspMsg)
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
	userdetail, isUserExists, err := usermanage.GetUserInfo(uid)
	if err != nil {
		return Error.New(cs.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", uid, err.Error()))
	}
	if !isUserExists {
		return Error.New(cs.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
	}
	log.Printf("[TRACE] getUser(%v) ret userinfo: %v", uid, userdetail.User)

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

	questInfo, e := GetQuestInfo(db, &stageInfo, questId)
	if e.IsError() {
		return e
	}
	if questInfo == nil {
		return Error.New(cs.EQ_GET_QUESTINFO_ERROR, "GetQuestInfo ret ok, but result is nil.")
	}
	log.Printf("questInfo:%+v", questInfo)

	//update stamina
	log.Printf("[TRACE]--Old Stamina:%v staminaRecover:%v", *userdetail.User.StaminaNow, *userdetail.User.StaminaRecover)
	e = UpdateStamina(userdetail.User.StaminaRecover, userdetail.User.StaminaNow, *userdetail.User.StaminaMax, *questInfo.Stamina)
	if e.IsError() {
		return e
	}
	log.Printf("[TRACE]--New StaminaNow:%v staminaRecover:%v", *userdetail.User.StaminaNow, *userdetail.User.StaminaRecover)

	//get quest config
	questConf, e := GetQuestConfig(db, questId)
	log.Printf("[TRACE] questConf:%+v", questConf)
	if e.IsError() {
		return e
	}

	//make quest data
	questData, e := MakeQuestData(&questConf)
	if e.IsError() {
		return e
	}
	//fill response
	rspMsg.StaminaNow = proto.Int32(*userdetail.User.StaminaNow - *questInfo.Stamina)
	rspMsg.StaminaRecover = proto.Uint32(*userdetail.User.StaminaRecover)
	rspMsg.DungeonData = &questData

	log.Printf("=========== StartQuest total cost %v ms. ============\n\n", cost.Cost())

	return Error.OK()
}
