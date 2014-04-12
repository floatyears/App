package quest

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
	//"model/quest"
	"model/user"

	"code.google.com/p/goprotobuf/proto"
	"common/consts"
)

/////////////////////////////////////////////////////////////////////////////

func RedoQuestHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqRedoQuest
	rspMsg := &bbproto.RspRedoQuest{}

	handler := &RedoQuest{}
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

type RedoQuest struct {
	bbproto.BaseProtoHandler
}

func (t RedoQuest) FillResponseMsg(reqMsg *bbproto.ReqRedoQuest, rspMsg *bbproto.RspRedoQuest, rspErr Error.Error) (outbuffer []byte) {
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

func (t RedoQuest) verifyParams(reqMsg *bbproto.ReqRedoQuest) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.StageId == nil || reqMsg.QuestId == nil ||
		reqMsg.HelperUserId == nil || reqMsg.HelperUnit == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	//!!IMPORTANT!!: client protobuf.net cannot serialize when value='0', so convert nil to 0.
	if reqMsg.CurrentParty == nil {
		reqMsg.CurrentParty = proto.Int32(0)
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.StageId == 0 || *reqMsg.QuestId == 0 ||
		*reqMsg.HelperUserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	return Error.OK()
}

func (t RedoQuest) ProcessLogic(reqMsg *bbproto.ReqRedoQuest, rspMsg *bbproto.RspRedoQuest) (e Error.Error) {
	cost := &common.Cost{}
	cost.Begin()

	stageId := *reqMsg.StageId
	questId := *reqMsg.QuestId
	uid := *reqMsg.Header.UserId

	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
		return Error.New(EC.CONNECT_DB_ERROR, err.Error())
	}

	//get userinfo from user table
	userDetail, isUserExists, err := user.GetUserInfo(db, uid)
	if err != nil {
		return Error.New(EC.EU_GET_USERINFO_FAIL, fmt.Sprintf("GetUserInfo failed for userId %v. err:%v", uid, err.Error()))
	}
	if !isUserExists {
		return Error.New(EC.EU_USER_NOT_EXISTS, fmt.Sprintf("userId: %v not exists", uid))
	}
	log.T(" getUser(%v) ret userinfo: %v", uid, userDetail.User)

	// check user is already playing
	if userDetail.Quest == nil || userDetail.Quest.QuestId == nil {
		e = Error.New(EC.EQ_USER_QUEST_NOT_PLAYING, fmt.Sprintf("user(%v) is NOT playing quest.", uid))
		log.Error(e.Error())
		return e
	}

	if *userDetail.Quest.QuestId != questId {
		e = Error.New(EC.EQ_USER_QUEST_NOT_PLAYING, fmt.Sprintf("user(%v) is playing quest:%v, but NOT questId:%v", uid, *userDetail.Quest.QuestId, questId))
		log.Error(e.Error())
		return e
	}

	if int(*reqMsg.Floor) >= len(*userDetail.Quest.DungeonData.Floors) {
		e = Error.New(EC.INVALID_PARAMS, fmt.Sprintf("redoQuest: request invalid floor no(%v).", *reqMsg.Floor))
		log.Error(e.Error())
		return e
	}

	//cost stone
	if *userDetail.Account.Stone < consts.N_REDO_QUEST_COST {
		e = Error.New(EC.EU_NO_ENOUGH_MONEY)
		log.Error("userid:%v stone(%v) is not enough.", uid, *userDetail.Account.Stone)
		return e
	}
	*userDetail.Account.Stone -= consts.N_REDO_QUEST_COST

	//save updated userinfo
	if e = user.UpdateUserInfo(db, userDetail); e.IsError() {
		return e
	}

	//TODO: assign new color blocks
	//userDetail.Quest.DungeonData.Colors = qm.makeColors(config.Colors, consts.N_QUEST_COLOR_BLOCK_NUM)

	//fill response
	rspMsg.StaminaNow = userDetail.User.StaminaNow
	rspMsg.StaminaRecover = userDetail.User.StaminaRecover
	rspMsg.DungeonData = userDetail.Quest.DungeonData

	log.T("=========== RedoQuest total cost %v ms. ============\n\n", cost.Cost())

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
	log.T(">>>>>>>>>>>>rspMsg end.<<<<<<<<<<<<<")

	return Error.OK()
}
