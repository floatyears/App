package user

import (
	"fmt"
	//"io/ioutil"
	"net/http"
	//"strconv"
	//"time"
	"code.google.com/p/goprotobuf/proto"
	"reflect"
)

import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/log"
	"data"
	"model/friend"
	"model/unit"
	"model/user"
	"model/quest"
)

/////////////////////////////////////////////////////////////////////////////

func AuthUserHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqAuthUser
	rspMsg := &bbproto.RspAuthUser{}

	handler := &AuthUser{}
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
	log.Printf("sendrsp err:%v, AuthUser rspMsg.Header:%+v nickName:%v\n\n", e, rspMsg.Header, *rspMsg.User.NickName)
}

/////////////////////////////////////////////////////////////////////////////

type AuthUser struct {
	bbproto.BaseProtoHandler
}

func (t AuthUser) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t AuthUser) verifyParams(reqMsg *bbproto.ReqAuthUser) (err Error.Error) {
	//TODO: do some params validation
	if reqMsg.Terminal == nil || reqMsg.Terminal.Uuid == nil && reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Terminal.Uuid == "" && *reqMsg.Header.UserId <= 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	return Error.OK()
}

func (t AuthUser) FillResponseMsg(req interface{}, rspMsg *bbproto.RspAuthUser, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	log.T("++++++ req type:%+v", reflect.TypeOf(req))
	//tp := reflect.TypeOf(req)
	//if tp.Type() != *bbproto.ReqAuthUser {
	//	return
	//}
	reqMsg := req.(*bbproto.ReqAuthUser)
	if reqMsg.Header == nil {
		return nil
	}

	{
		rspMsg.Header = reqMsg.Header
		rspMsg.Header.Code = proto.Int(rspErr.Code())
		rspMsg.Header.Error = proto.String(rspErr.Error())

		sessionId, _ := t.GenerateSessionId(reqMsg.Terminal.Uuid)
		reqMsg.Header.SessionId = &sessionId
		log.T("req sessionId:%v reqMsg.Header:%+v\n",
			*reqMsg.Header.SessionId, reqMsg.Header)
	}

	// fill custom protocol body

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func (t AuthUser) ProcessLogic(reqMsg *bbproto.ReqAuthUser, rspMsg *bbproto.RspAuthUser) (e Error.Error) {
	// read user data (by uuid) from db
	var uuid string
	var uid uint32 = 0
	if reqMsg.Terminal.Uuid != nil {
		uuid = *reqMsg.Terminal.Uuid
	}
	if reqMsg.Header.UserId != nil {
		uid = *reqMsg.Header.UserId
	}

	db := &data.Data{}
	err := db.Open("")
	defer db.Close()
	if err != nil {
		log.Error("uid:%v, open db ret err:%v", uid, err)
		return Error.New(EC.CONNECT_DB_ERROR, err)
	}

	var userDetail *bbproto.UserInfoDetail = nil
	var isUserExists bool
	if uid > 0 {
		userDetail, isUserExists, err = user.GetUserInfo(db, uid)
		log.T("GetUserInfo by uid(%v) ret isExists=%v ", uid, isUserExists)
	} else {
		userDetail, isUserExists, err = user.GetUserInfoByUuid(uuid)
		log.T("GetUserInfo by uuid(%v) ret isExists=%v ", uuid, isUserExists)
	}

	log.T("GetUserInfo(%v) ret err(%v). isExists=%v userDetail: ['%v']  ",
		uuid, err, isUserExists, userDetail)
	if err != nil {
		log.Error("GetUserInfo ret err:%v", err)
		return Error.New(EC.EU_GET_USERINFO_FAIL, err)
	}

	if !isUserExists { //create new user
		log.Printf("Cannot find data for user ( uid:%v uuid:%v ), create new user...", uid, uuid)

		rspMsg.IsNewUser = proto.Int32(1) // is new User

		selectRole:= uint32(1)
		if reqMsg.SelectRole != nil {
			selectRole = *reqMsg.SelectRole
		}
		userDetail, e = user.AddNewUser(db, uuid, selectRole)
		if e.IsError() {
			return e
		}
		if userDetail == nil {
			log.Fatal("Unexcepted Error: create new user ok, but userDetail is nil.")
			return Error.New(EC.EU_GET_NEWUSERID_FAIL)
		}
		log.T("create NewUser ret UserId: %v", *userDetail.User.UserId)
	}

	if isUserExists {
		log.T("+++exists user: %v", *userDetail.User.UserId)
	}

	uid = *userDetail.User.UserId
	rank := uint32(*userDetail.User.Rank)

	{
		// get FriendInfo
		friendsInfo, e := friend.GetSupportFriends(db, uid, rank)
		log.T("GetFriendInfo ret err:%v. friends num=%v  ", e.Error(), len(friendsInfo))
		if e.IsError() && e.Code() != EC.EF_FRIEND_NOT_EXISTS {
			return Error.New(EC.EF_GET_FRIENDINFO_FAIL, fmt.Sprintf("GetFriends failed for uid %v, rank:%v", uid, rank))
		}

		//fill rspMsg
		if friendsInfo != nil {
			for _, friend := range friendsInfo {
				//log.T("rspMsg.append => &friend:%p fid:%v friend.Unit: %+v", &friend, *friend.UserId, *friend.Unit)
				oneFriend := &bbproto.FriendInfo{}
				*oneFriend = friend
				rspMsg.Friends = append(rspMsg.Friends, oneFriend)
			}
		}

		if e = user.RefreshStamina(userDetail.User.StaminaRecover, userDetail.User.StaminaNow, *userDetail.User.StaminaMax); e.IsError() {
			log.Error("RefreshStamina fail")
			return e
		}

		//TODO: call update in goroutine
		user.UpdateLoginInfo(db, userDetail)

		//TODO: get present
		//rspMsg.Present = userDetail.Present

	}

	// get quest clear flag
	rspMsg.QuestClear, e = quest.GetQuestClearFlag(db, userDetail)

	//fill response
	rspMsg.Account = userDetail.Account
	rspMsg.UnitList = userDetail.UnitList
	rspMsg.Party = userDetail.Party
	rspMsg.Login = userDetail.Login
	rspMsg.Quest = userDetail.Quest
	rspMsg.User = userDetail.User

	reqMsg.Header.UserId = userDetail.User.UserId

	rspMsg.EvolveType = unit.GetTodayEvolveType()
	rspMsg.ServerTime = proto.Uint32(common.Now())

	log.T(" ")
	log.T(">>>>>>>>>>>>AuthUser RspMsg (uid:%v)<<<<<<<<<<<<<<", *rspMsg.User.UserId)
	log.T("\tUserinfo: %+v", rspMsg.User)
	log.T("\tAccount: %+v", rspMsg.Account)
	log.T("\tQuestClear: %+v", rspMsg.QuestClear)

	log.T("\tFriends: ")
	for k, friend := range rspMsg.Friends {
		log.T("\t friend[%v]: %+v", k, friend)
	}
	log.T("\tParty: CurrParty: %v", *rspMsg.Party.CurrentParty)
	for k, party := range rspMsg.Party.PartyList {
		log.T("\tparty[%v]: %+v", k, *party)
	}
	log.T("\tLogin: %+v", rspMsg.Login)
	log.T("\tUnitList: count=%v", len(rspMsg.UnitList))
	for k, unit := range rspMsg.UnitList {
		log.T("\t\tunit[%v]: %+v", k, unit)
	}
	log.T("\tQuest: %+v", rspMsg.Quest)
	log.T("\tEvolveType: %+v", rspMsg.EvolveType)

	log.T(">>>>>>>>>>>>AuthUser RspMsg End (uid:%v) <<<<<<<<<<<<<<", *rspMsg.User.UserId)

	return Error.OK()
}
