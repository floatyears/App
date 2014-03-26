package unit

import (
	"fmt"
	"net/http"
)

import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/log"
	"data"
	"model/quest"
	"model/unit"
	"model/user"

	"code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func EvolveStartHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqEvolveStart
	rspMsg := &bbproto.RspEvolveStart{}

	handler := &EvolveStart{}
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
	log.T("sendrsp err:%v, rspMsg:%+v.\n===========================================", e, rspMsg.Header)
}

/////////////////////////////////////////////////////////////////////////////

type EvolveStart struct {
	bbproto.BaseProtoHandler
}

func (t EvolveStart) FillResponseMsg(reqMsg *bbproto.ReqEvolveStart, rspMsg *bbproto.RspEvolveStart, rspErr Error.Error) (outbuffer []byte) {
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

func (t EvolveStart) verifyParams(reqMsg *bbproto.ReqEvolveStart) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.BaseUniqueId == nil || reqMsg.PartUniqueId == nil ||
		reqMsg.HelperUserId == nil || reqMsg.HelperUnit == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || len(reqMsg.PartUniqueId) > 3 ||
		*reqMsg.HelperUserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params value is invalid.")
	}

	return Error.OK()
}

func (t EvolveStart) ProcessLogic(reqMsg *bbproto.ReqEvolveStart, rspMsg *bbproto.RspEvolveStart) (e Error.Error) {
	cost := &common.Cost{}
	cost.Begin()

	uid := *reqMsg.Header.UserId

	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
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
	log.T("\t===== 1. getUser(%v) ret userinfo: %v", uid, userDetail.User)

	//2. check user is already playing
	isRestartNewQuest := (reqMsg.RestartNew != nil && *reqMsg.RestartNew != 0)
	if isRestartNewQuest == false {
		if userDetail.Quest != nil && userDetail.Quest.State != nil {
			e = Error.New(EC.EQ_QUEST_IS_PLAYING, fmt.Sprintf("user(%v) is playing quest:%+v", *userDetail.User.UserId, *userDetail.Quest.QuestId))
			log.T(e.Error())
			return e
		}
	}

	//3. getUnitInfo of baseUniqueId
	baseUserUnit, e := unit.GetUserUnitInfo(userDetail, *reqMsg.BaseUniqueId)
	if e.IsError() {
		log.Error("GetUserUnitInfo(%v) failed: %v", *reqMsg.BaseUniqueId, e.Error())
		return e
	}
	baseUnit, e := unit.GetUnitInfo( *baseUserUnit.UnitId)
	if e.IsError() {
		log.Error("GetUnitInfo(%v) failed: %v", *baseUserUnit.UnitId, e.Error())
		return e
	}
	log.Error("\t===== 2. getUnitInfo ret baseUserUnit:(%+v).", baseUserUnit)
	log.Error("\t=====    baseUnit: type:%v rare:%v (%+v).", *baseUnit.Type, *baseUnit.Rare, baseUnit)

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

	//TODO: check base & material & helperUnit validation
	{

	}

	//4. getQuestId
	stageId, questId := unit.GetEvolveQuestId(*baseUnit.Type, *baseUnit.Rare)
	log.T("\t===== 3. GetEvolveQuestId(%v,%v) return stageId:%v questId:%v",
		*baseUnit.Type, *baseUnit.Rare, stageId, questId)
	//check Quest record for QuestState
	questState, e := quest.CheckQuestRecord(db, stageId, questId, uid)
	if e.IsError() {
		return e
	}

	//5. get questInfo
	stageInfo, e := quest.GetStageInfo(db, stageId)
	if e.IsError() {
		return e
	}

	questInfo, e := quest.GetQuestInfo(db, stageInfo, questId)
	if e.IsError() {
		return e
	}
	if questInfo == nil {
		return Error.New(EC.EQ_GET_QUESTINFO_ERROR, "GetQuestInfo ret ok, but result is nil.")
	}
	log.T("\t===== 4. GetStageInfo ret:%+v", stageInfo)
	log.T("\t===== 		 questInfo ret:%+v", questInfo)

	//6. update stamina
	log.T("--Old Stamina:%v staminaRecover:%v", *userDetail.User.StaminaNow, *userDetail.User.StaminaRecover)
	e = user.RefreshStamina(userDetail.User.StaminaRecover, userDetail.User.StaminaNow, *userDetail.User.StaminaMax)
	if e.IsError() {
		return e
	}
	log.T("--New StaminaNow: %v -> %v staminaRecover:%v",
		*userDetail.User.StaminaNow, *userDetail.User.StaminaNow-*questInfo.Stamina, *userDetail.User.StaminaRecover)

	if *userDetail.User.StaminaNow < *questInfo.Stamina {
		return Error.New(EC.EQ_STAMINA_NOT_ENOUGH, "stamina is not enough")
	}
	*userDetail.User.StaminaNow -= *questInfo.Stamina

	//7. get quest config
	questConf, e := quest.GetQuestConfig(db, questId)
	log.T(" questConf:%+v", questConf)
	if e.IsError() {
		return e
	}

	questDataMaker := &quest.QuestDataMaker{}
	//8. make quest data
	questData, e := questDataMaker.MakeData(&questConf)
	if e.IsError() {
		return e
	}

	reqMsg.EvolveQuestId = &questId
	if e = unit.SaveEvolveSession(db, reqMsg); e.IsError() {
		return e
	}

	//10. update latest quest record of userDetail
	if e = quest.FillUserQuest(userDetail, *userDetail.Party.CurrentParty, *reqMsg.HelperUserId, reqMsg.HelperUnit,
		questData.Drop, stageInfo, questInfo, questState); e.IsError() {
		return e
	}

	//11. save updated userinfo
	if e = user.UpdateUserInfo(db, userDetail); e.IsError() {
		return e
	}

	//12. fill response
	rspMsg.StaminaNow = userDetail.User.StaminaNow
	rspMsg.StaminaRecover = userDetail.User.StaminaRecover
	rspMsg.DungeonData = &questData

	log.T("=========== EvolveStart total cost %v ms. ============\n\n", cost.Cost())

	log.T(">>>>>>>>>>>>rspMsg begin<<<<<<<<<<<<<")
	log.T("--StaminaNow:%v", *rspMsg.StaminaNow)
	log.T("--StaminaRecover:%v", *rspMsg.StaminaRecover)

	log.T("--Boss:%+v", rspMsg.DungeonData.Boss)

	log.T("--Enemys: count=%v", len(rspMsg.DungeonData.Enemys))
	for k, enemy := range rspMsg.DungeonData.Enemys {
		log.T("\t enemy[%v]: %v", k, enemy)
	}

	log.T("--Drop: count=%v", len(rspMsg.DungeonData.Drop))
	for k, drop := range rspMsg.DungeonData.Drop {
		log.T("\t drop[%v]: %v", k, drop)
	}

	for k, floor := range rspMsg.DungeonData.Floors {
		log.T("--floor[%v]:", k)
		for _, grid := range floor.GridInfo {
			log.T("\t--[%+v] \n", grid)
		}
	}
	log.T(">>>>>>>>>>>>EvolveStart rspMsg end.<<<<<<<<<<<<<")

	return Error.OK()
}
