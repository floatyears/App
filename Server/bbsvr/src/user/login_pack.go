package user

import (
	//	"fmt"
	"log"
	"net/http"
	//"time"
)
import (
	bbproto "../bbproto"
	"../common"
	"../const"
	"../data"
	proto "code.google.com/p/goprotobuf/proto"
	redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func LoginPackHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqLoginPack
	rspMsg := &bbproto.RspLoginPack{}

	handler := &LoginPack{}
	err := handler.ParseInput(req, &reqMsg)
	if err != nil {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
		return
	}

	err = handler.verifyParams(&reqMsg)
	if err != nil {
		handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
		return
	}

	// game logic

	err = handler.ProcessLogic(&reqMsg, rspMsg)

	err = handler.SendResponse(rsp, handler.FillResponseMsg(&reqMsg, rspMsg, err))
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", err, rspMsg)
}

/////////////////////////////////////////////////////////////////////////////

type LoginPack struct {
	bbproto.BaseProtoHandler
}

func (t LoginPack) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t LoginPack) verifyParams(reqMsg proto.Message) (err error) {
	//TODO: do some params validation
	return nil
}

func (t LoginPack) FillResponseMsg(reqMsg *bbproto.ReqLoginPack, rspMsg *bbproto.RspLoginPack, rspErr error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId

		log.Printf("req header:%v reqMsg.Header:%v", *reqMsg.Header.SessionId, reqMsg.Header)
	}

	// fill custom protocol body
	{

	}

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func GetFriendInfo(uid uint32) (friendsInfo map[string]bbproto.FriendInfo, err error) {
	db := &data.Data{}
	err = db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil || uid == 0 {
		return
	}

	strUid := common.Utoa(uid)
	zFriendDatas, err := db.HGetAll(strUid)
	friendNum := len(zFriendDatas) / 2

	log.Printf("get from uid '%v' ret err:%v, friendsInfo[%v]: %v",
		uid, err, friendNum, friendsInfo)
	if err != nil {
		return
	}

	friendsInfo = make(map[string]bbproto.FriendInfo, friendNum)

	log.Printf("friendsInfo's len=%v friendsInfo:%v", len(friendsInfo), friendsInfo)

	i := 0
	for ; len(zFriendDatas) > 0; i++ {
		var sFid, sFridata []byte
		zFriendDatas, err = redis.Scan(zFriendDatas, &sFid, &sFridata)
		if err != nil {
			continue
		}
		log.Printf("loop fid:%v fdata:%v", sFid, sFridata)

		friendData := &bbproto.FriendData{}
		//zFriend, err := redis.Bytes(sFridata, err)
		if err == nil {
			err = proto.Unmarshal(sFridata, friendData) //unSerialize to friend
		}
		log.Printf("get from uid '%v' ret err:%v, friendData: %v", uid, err, friendData)

		//assign friend data fields
		friInfo := bbproto.FriendInfo{}
		friInfo.UserId = friendData.UserId
		friInfo.FriendState = friendData.FriendState
		friInfo.FriendStateUpdate = friendData.FriendStateUpdate
		friendsInfo[string(sFid)] = friInfo

		log.Printf("friendsInfo's len=%v data:%v", len(friendsInfo), friendsInfo)
	}

	// get userinfo by friends's uid
	fids := redis.Args{}
	for _, friInfo := range friendsInfo {
		fids = fids.Add(common.Utoa(*friInfo.UserId))
	}

	if err = db.Select(cs.TABLE_USER); err != nil {
		return
	}

	userinfos, err := db.MGet(fids)
	if err != nil {
		return
	}

	for _, uinfo := range userinfos {
		user := bbproto.UserInfo{}
		zUser, err := redis.Bytes(uinfo, err)
		if err == nil {
			err = proto.Unmarshal(zUser, &user) //unSerialize
		}
		log.Printf("userId: %v -> name:%v rank:%v LoginTime:%v", *user.UserId, *user.UserName, *user.Rank, *user.LoginTime)

		uid = *user.UserId
		friInfo, ok := friendsInfo[common.Utoa(uid)]
		if ok {
			friInfo.Rank = user.Rank
			friInfo.UserName = user.UserName
			friInfo.LastPlayTime = user.LoginTime
			//friInfo.UnitId = uint(10) // TODO: add leader's unitId to userinfo
			log.Printf("friInfo filled done.")
		} else {
			log.Printf("cannot find friInfo for: %v.", uid)
		}

	}
	log.Printf("friends's fids:%v userinfos:%v", fids, userinfos)
	log.Println("===========GetFriends finished.==========\n")

	return friendsInfo, err
}

func (t LoginPack) ProcessLogic(reqMsg *bbproto.ReqLoginPack, rspMsg *bbproto.RspLoginPack) (err error) {
	// read user data (by uuid) from db
	uid := *reqMsg.Header.UserId
	isUserExists := uid != 0

	if isUserExists {
		friendsInfo, err := GetFriendInfo(uid)
		log.Printf("GetFriendInfo ret %v. friends num=%v friendsInfo: ['%v']  ", err, len(friendsInfo), friendsInfo)
	}

	//rspMsg.loginParam = &bbproto.LoginInfo{}
	//if isSessionExists {
	//	err = proto.Unmarshal(value, rspMsg.Userdetail) //unSerialize into Userdetail
	//	tNow := uint32(time.Now().Unix())

	//	*rspMsg.Userdetail.User.LoginTime = uint32(tNow)
	//	log.Printf("read Userdetail ret err:%v, Userdetail: %+v", err, rspMsg.Userdetail)
	//} else { //generate new user
	//	log.Printf("Cannot find data for user uuid:%v, create new user...", uuid)

	//	newUserId, err := GetNewUserId()
	//	defaultName := cs.DEFAULT_USER_NAME
	//	tNow := uint32(time.Now().Unix())
	//	rank := int32(0)
	//	exp := int32(0)
	//	staminaNow := int32(10)
	//	staminaMax := int32(10)
	//	staminaRecover := uint32(tNow + 600) //10 minutes
	//	rspMsg.Userdetail.User = &bbproto.UserInfo{
	//		UserId:         &newUserId,
	//		UserName:       &defaultName,
	//		LoginTime:      &tNow,
	//		Rank:           &rank,
	//		Exp:            &exp,
	//		StaminaNow:     &staminaNow,
	//		StaminaMax:     &staminaMax,
	//		StaminaRecover: &staminaRecover,
	//	}
	//	rspMsg.ServerTime = &tNow
	//	log.Printf("rspMsg.Userdetail.User=%v...", rspMsg.Userdetail.User)
	//	log.Printf("rspMsg=%+v...", rspMsg)

	//	//TODO:save userinfo to db through goroutine
	//	outbuffer, err := proto.Marshal(rspMsg.Userdetail)
	//	err = db.Set(uuid, outbuffer)
	//	log.Printf("db.Set(%v) save new userinfo, return %v", uuid, err)
	//}

	return err
}

//func LoginPackHandler(rsp http.ResponseWriter, req *http.Request) {
//	p := &LoginPack{}
//	rspMsg := &bbproto.RspLoginPack{}

//	msg, err := p.checkInput(req)
//	if err != nil {
//		data, _ := p.FillRspHeader(&msg, rspMsg)

//		common.SendResponse(rsp, data)
//		return
//	}

//	value := ""
//	if *msg.UserId != 0 {
//		id := strconv.Itoa(int(*msg.UserId))
//		db := &data.Data{}
//		db.Open("0")
//		defer db.Close()
//		value, err := db.Get(id)
//		log.Printf("get for '%v' ret err:%v, value: %v", *msg.UserId, err, value)
//	}

//	isUserExists := value != ""
//	if isUserExists {
//		err = proto.Unmarshal([]byte(value), rspMsg.User) //unSerialize into usrinfo

//		data, err := p.FillRspHeader(&msg, rspMsg)
//		size, err := common.SendResponse(rsp, data)
//		log.Printf("rsp msg err:%v, size[%d]: %+v", err, size, rspMsg.User)
//	} else {
//		//rspMsg.User = &bbproto.UserInfo{}
//		*rspMsg.User.UserId, err = GetNewUserId()
//		*rspMsg.User.UserName = common.DEFAULT_USER_NAME
//		*rspMsg.User.LoginTime = uint32(time.Now().Unix())

//		log.Printf("ERR: Cannot find data for user:%v", *msg.UserId)
//	}
//}
