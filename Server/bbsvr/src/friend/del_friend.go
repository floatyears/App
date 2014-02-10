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

func DelFriendHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqDelFriend
	rspMsg := &bbproto.RspDelFriend{}

	handler := &DelFriendProtocol{}
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

type DelFriendProtocol struct {
	bbproto.BaseProtoHandler
}

func (t DelFriendProtocol) GenerateSessionId(uuid *string) (sessionId string, err error) {
	//TODO: makeSidFrom(*uuid, timeNow)
	sessionId = "rcs7kga8pmvvlbtgbf90jnchmqbl9khn"
	return sessionId, nil
}

func (t DelFriendProtocol) verifyParams(reqMsg *bbproto.ReqDelFriend) (err error) {
	//TODO: input params validation
	if reqMsg.Header.UserId == nil {
		return errors.New("ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return errors.New("ERROR: userId is invalid.")
	}

	return nil
}

func (t DelFriendProtocol) FillResponseMsg(reqMsg *bbproto.ReqDelFriend, rspMsg *bbproto.RspDelFriend, rspErr error) (outbuffer []byte) {
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

func (t DelFriendProtocol) ProcessLogic(reqMsg *bbproto.ReqDelFriend, rspMsg *bbproto.RspDelFriend) (err error) {

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
