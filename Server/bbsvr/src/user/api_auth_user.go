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
	"event"
	"model/friend"
	"model/user"
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
	log.Printf("sendrsp err:%v, AuthUser rspMsg.Header:\n%+v nickName:%v\n\n", e, rspMsg.Header, rspMsg.User.NickName)
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

	var userDetail bbproto.UserInfoDetail
	var isUserExists bool
	if uid > 0 {
		userDetail, isUserExists, err = user.GetUserInfo(db, uid)
	} else {
		userDetail, isUserExists, err = user.GetUserInfoByUuid(uuid)
	}

	log.T("GetUserInfo(%v) ret err(%v). isExists=%v userDetail: ['%v']  ",
		uuid, err, isUserExists, userDetail)
	if isUserExists && err == nil {
		uid = *userDetail.User.UserId
		log.T("+++exists user: %v", *userDetail.User.UserId)

		rank := uint32(*userDetail.User.Rank)

		// get FriendInfo
		friendsInfo, e := friend.GetOnlyFriends(db, uid, rank)
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

		//TODO: remove the testing code
		//if *userDetail.User.UserId == 103 {
		//	user.TestAddMyUnits(db, &userDetail)
		//}

		//TODO: call update in goroutine
		user.UpdateLoginInfo(db, &userDetail)

		rspMsg.Account = userDetail.Account
		rspMsg.UnitList = userDetail.UnitList
		rspMsg.Party = userDetail.Party
		rspMsg.Login = userDetail.Login
		rspMsg.Quest = userDetail.Quest
		rspMsg.User = userDetail.User

		//TODO: get present
		//rspMsg.Present = userDetail.Present

	} else { //create new user
		log.Printf("Cannot find data for user uuid:%v, create new user...", uuid)

		rspMsg.ServerTime = proto.Uint32(common.Now())

		//TODO:save userinfo to db through goroutine
		userDetail, e := user.AddNewUser(db, uuid)
		if !e.IsError() && userDetail != nil {
			rspMsg.Account = userDetail.Account
			rspMsg.UnitList = userDetail.UnitList
			rspMsg.Party = userDetail.Party
			rspMsg.Login = userDetail.Login
			rspMsg.Quest = userDetail.Quest
			rspMsg.User = userDetail.User

			reqMsg.Header.UserId = userDetail.User.UserId
		}

		log.T("create NewUser rspMsg.UserId: %v", *reqMsg.Header.UserId)

	}

	rspMsg.EvolveType = event.GetEvolveType()

	log.T(">>>>>>>>>>>>AuthUser RspMsg<<<<<<<<<<<<<<")
	log.T("\tUserinfo: %+v", rspMsg.User)
	log.T("\tAccount: %+v", rspMsg.Account)
	log.T("\tFriends: ")
	for k, friend:=range rspMsg.Friends{
		log.T("\t [%v]: %+v", k, friend)
	}
	log.T("\tParty: ")
	for _, party := range rspMsg.Party.PartyList {
		log.T("\t%v: %+v", *party.Id, party.Items)
	}
	log.T("\tLogin: %+v", rspMsg.Login)
	log.T("\tUnitList: count=%v", len(rspMsg.UnitList))
	for k, unit := range rspMsg.UnitList {
		log.T("\t\tunit[%v]: %+v", k, unit)
	}
	log.T("\tQuest: %+v", rspMsg.Quest)
	log.T("\tEvolveType: %+v", rspMsg.EvolveType)

	log.T(">>>>>>>>>>>>AuthUser RspMsg<<<<<<<<<<<<<<")

	return Error.OK()
}
