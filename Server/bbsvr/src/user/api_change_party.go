package user

import (
	"net/http"
	//"time"
	"reflect"
)

import (
	"bbproto"
	"common/EC"
	"common/Error"
	"common/log"
	"model/party"

	"code.google.com/p/goprotobuf/proto"
)

/////////////////////////////////////////////////////////////////////////////

func ChangePartyHandler(rsp http.ResponseWriter, req *http.Request) {
	var reqMsg bbproto.ReqChangeParty
	rspMsg := &bbproto.RspChangeParty{}

	handler := &ChangeParty{}
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

type ChangeParty struct {
	bbproto.BaseProtoHandler
}

func (t ChangeParty) verifyParams(reqMsg *bbproto.ReqChangeParty) (err Error.Error) {
	//TODO: input params validation
	if reqMsg.Party == nil || reqMsg.Header.UserId == nil {
		return Error.New(EC.INVALID_PARAMS, "ERROR: params is invalid.")
	}

	if *reqMsg.Header.UserId == 0 {
		return Error.New(EC.INVALID_PARAMS, "ERROR: userId is invalid.")
	}

	return Error.OK()
}

//func (t ChangeParty) FillResponseMsg(reqMsg *bbproto.ReqChangeParty, rspMsg interface{}, rspErr Error.Error) (outbuffer []byte) {
func (t ChangeParty) FillResponseMsg(reqMsg *bbproto.ReqChangeParty, rspMsg *bbproto.RspChangeParty, rspErr Error.Error) (outbuffer []byte) {
	// fill protocol header
	{
		//ty := reflect.TypeOf(rspMsg)
		//ty := reflect.ValueOf(rspMsg)
		log.T("rsp :=%v ", reflect.TypeOf(rspMsg))
		//rsp:=rspMsg.(ty.Type() )
		//log.T("rsp :=%v ty=%v", rsp, ty)
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

func (t ChangeParty) ProcessLogic(reqMsg *bbproto.ReqChangeParty, rspMsg *bbproto.RspChangeParty) (e Error.Error) {
	log.T("Req.ChangeParty: currParty:%v", *reqMsg.Party.CurrentParty)

	if reqMsg.Party.CurrentParty == nil {
		reqMsg.Party.CurrentParty = proto.Int32(0)
	}
	for _, p := range reqMsg.Party.PartyList {
		if p.Id != nil {
			log.T("id[%v]: %+v", *p.Id, p.Items)
		} else {
			log.T("party.Id=nil, force to 0. %+v", p)
			p.Id = proto.Int32(0)
		}
	}

	e = party.ChangeParty(nil, *reqMsg.Header.UserId, reqMsg.Party)

	return e
}
