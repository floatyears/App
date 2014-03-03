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
	"../bbproto"
	"../common"
	"../common/Error"
	"../common/log"
	"../const"
	"../data"
	"../event"
	"../friend"
	"./usermanage"
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
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", e, rspMsg)
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
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Terminal.Uuid == "" && *reqMsg.Header.UserId <= 0 {
		return Error.New(cs.INVALID_PARAMS, "ERROR: params is invalid.")
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

	var userDetail bbproto.UserInfoDetail
	var isUserExists bool
	var err error
	if uid > 0 {
		userDetail, isUserExists, err = usermanage.GetUserInfo(uid)
	} else {
		userDetail, isUserExists, err = usermanage.GetUserInfoByUuid(uuid)
	}

	log.T("GetUserInfo(%v) ret err(%v). isExists=%v userDetail: ['%v']  ",
		uuid, err, isUserExists, userDetail)
	if isUserExists && err == nil {
		uid = *userDetail.User.UserId
		log.T("+++exists user: %v", *userDetail.User.UserId)

		rank := uint32(*userDetail.User.Rank)

		// get FriendInfo
		db := &data.Data{}
		err = db.Open(cs.TABLE_FRIEND)
		defer db.Close()
		if err != nil {
			log.Error("uid:%v, open db ret err:%v", uid, err)
			return Error.New(cs.CONNECT_DB_ERROR, err)
		}

		friendsInfo, e := friend.GetOnlyFriends(db, uid, rank)
		log.T("GetFriendInfo ret err:%v. friends num=%v  ", e.Error(), len(friendsInfo))
		if e.IsError() && e.Code() != cs.EF_FRIEND_NOT_EXISTS {
			return Error.New(cs.EF_GET_FRIENDINFO_FAIL, fmt.Sprintf("GetFriends failed for uid %v, rank:%v", uid, rank))
		}

		//fill rspMsg
		if friendsInfo != nil {
			for _, friend := range friendsInfo {
				//log.T("fid:%v friend:%v", fid, *friend.UserId)
				rspMsg.Friends = append(rspMsg.Friends, &friend)
			}
		}

		if e = usermanage.RefreshStamina(userDetail.User.StaminaRecover, userDetail.User.StaminaNow, *userDetail.User.StaminaMax); e.IsError() {
			log.Error("RefreshStamina fail")
			return e
		}

		//TODO: call update in goroutine
		UpdateLoginInfo(db, &userDetail)

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

		db := &data.Data{}
		err = db.Open(cs.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return Error.New(cs.READ_DB_ERROR, err)
		}

		rspMsg.ServerTime = proto.Uint32(common.Now())

		//TODO:save userinfo to db through goroutine
		userDetail, e := usermanage.AddNewUser(db, uuid)
		if !e.IsError() && userDetail != nil {
			rspMsg.UnitList = userDetail.UnitList
			rspMsg.Party = userDetail.Party
			rspMsg.User = userDetail.User

			reqMsg.Header.UserId = userDetail.User.UserId
		}

		log.T("create NewUser rspMsg: %+v", rspMsg)

	}

	rspMsg.EvolveType = event.GetEvolveType()

	return Error.OK()
}
