package quest

import (
	_ "fmt"
	"io/ioutil"
	"log"
	"net/http"
	//"strconv"
	"time"
)

import (
	bbproto "../bbproto"
	"../common"
	"../const"
	"../data"
	"../user/usermanage"
	proto "code.google.com/p/goprotobuf/proto"
)

type StartQuest struct {
}

func (t StartQuest) verifyParams(reqMsg bbproto.ReqStartQuest) (err error) {
	//TODO: do some params validation
	return nil
}

func (t StartQuest) checkInput(req *http.Request) (reqMsg bbproto.ReqStartQuest, err error) {
	reqBuffer, err := ioutil.ReadAll(req.Body)
	if err != nil {
		log.Printf("ERR: ioutil.ReadAll failed: %v ", err)
		return reqMsg, err
	}

	err = proto.Unmarshal(reqBuffer, &reqMsg) //unSerialize into reqMsg
	if err != nil {
		log.Printf("ERR: checkInput parse proto err: %v", err)
		return reqMsg, err
	}
	log.Printf("recv reqMsg: %+v", reqMsg)

	err = t.verifyParams(reqMsg)

	return reqMsg, err
}

func (t StartQuest) FillResponseMsg(reqMsg *bbproto.ReqStartQuest, rspMsg *bbproto.RspStartQuest, rspErr error) (outbuffer []byte, err error) {
	{
		rspMsg.Header = reqMsg.Header
		//sessionId, _ := t.GenerateSessionId(reqMsg.Terminal.Uuid)
		//reqMsg.Header.SessionId = &sessionId
		//log.Printf("req header:%v reqMsg.Header:%v", *reqMsg.Header.SessionId, reqMsg.Header)
	}

	outbuffer, err = proto.Marshal(rspMsg)
	return outbuffer, err
}

func (t StartQuest) SendResponse(rsp http.ResponseWriter, reqMsg *bbproto.ReqStartQuest, rspMsg *bbproto.RspStartQuest, rspErr error) (err error) {
	data, err := t.FillResponseMsg(reqMsg, rspMsg, rspErr)
	if err != nil {
		return err
	}
	_, err = common.SendResponse(rsp, data)
	return err
}

func StartQuestHandler(rsp http.ResponseWriter, req *http.Request) {
	p := &StartQuest{}
	rspMsg := &bbproto.RspStartQuest{}

	reqMsg, err := p.checkInput(req)
	if err != nil {
		p.SendResponse(rsp, &reqMsg, rspMsg, err)
		return
	}

	questId := string(cs.KEY_QUEST_PREFIX + string(*reqMsg.QuestId))
	db := &data.Data{}
	err = db.Open(cs.TABLE_QUEST)
	defer db.Close()
	if err != nil { //connect to db failed
		p.SendResponse(rsp, &reqMsg, rspMsg, err)
		return
	}

	var value []byte
	if questId != "" {
		value, err = db.Gets(questId)
		log.Printf("db.get('%v') ret err:%v, value: %v", questId, err, value)
	}

	isExists := len(value) != 0
	log.Printf("isUserExists=%v value-len=%v value: ['%v']  ", isExists, len(value), value)
	if isExists {
		//err = proto.Unmarshal(value, rspMsg.Userdetail) //unSerialize into Userdetail
		userInfo, _, err := usermanage.GetUserInfo(*reqMsg.Header.UserId)
		if err == nil {
			tNow := uint32(time.Now().Unix())
			rspMsg.QuestId = reqMsg.QuestId
			*rspMsg.StaminaRecover = uint32(tNow + 600)
			*rspMsg.StaminaNow = uint32(1)
		}
		log.Printf("GetUserInfo ret: %v", userInfo)
	} else { //invalid questId
		log.Printf("FATAL ERR:Cannot find data for user uuid:%v .", questId)
	}

	err = p.SendResponse(rsp, &reqMsg, rspMsg, err)
	log.Printf("sendrsp err:%v, rspMsg:\n%+v", err, rspMsg)
}
