package friend

import (
	"fmt"
	"log"
	"net/http"
	//"time"
	"errors"
)

import (
	"../bbproto"
	//"../common"
	"../const"
	"../data"
	//"../user/usermanage"

	proto "code.google.com/p/goprotobuf/proto"
	//redis "github.com/garyburd/redigo/redis"
)

/////////////////////////////////////////////////////////////////////////////

func AcceptFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqAcceptFriend
	rspMsg := &bbproto.RspAcceptFriend{}

	handler := &AcceptFriend{}
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

type AcceptFriend struct {
	bbproto.BaseProtoHandler
}

func (t AcceptFriend) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t AcceptFriend) verifyParams(reqMsg *bbproto.ReqAcceptFriend) (err error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil || reqMsg.FriendUid == nil {
		return errors.New("ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 || *reqMsg.FriendUid == 0 {
		return errors.New("ERROR: userId is invalid.")
	}

	return nil
}

func (t AcceptFriend) FillResponseMsg(reqMsg *bbproto.ReqAcceptFriend, rspMsg *bbproto.RspAcceptFriend, rspErr error) (outbuffer []byte) {
	// fill protocol header
	{
		rspMsg.Header = reqMsg.Header //including the sessionId
		log.Printf("req header:%v reqMsg.Header:%v", *reqMsg.Header.SessionId, reqMsg.Header)
	}

	// fill custom protocol body

	// serialize to bytes
	outbuffer, err := proto.Marshal(rspMsg)
	if err != nil {
		return nil
	}
	return outbuffer
}

func (t AcceptFriend) ProcessLogic(reqMsg *bbproto.ReqAcceptFriend, rspMsg *bbproto.RspAcceptFriend) (err error) {

	uid := *reqMsg.Header.UserId
	fUid := *reqMsg.FriendUid

	db := &data.Data{}
	err = db.Open(cs.TABLE_FRIEND)
	defer db.Close()
	if err != nil || uid == 0 {
		return
	}

	log.Printf("%v %v %v", db, fUid, fmt.Sprintf(""))

	return err
}
