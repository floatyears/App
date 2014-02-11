package bbproto

import (
	_ "fmt"
	"io/ioutil"
	"log"
	"net/http"
	//"strconv"
)
import (
	"../common"
	"../common/Error"
	"../const"
	proto "code.google.com/p/goprotobuf/proto"
)

type ProtoHandler interface {
	//parse input into reqMsg
	ParseInput(req *http.Request, reqMsg proto.Message) (e Error.Error)
	SendResponse(rsp http.ResponseWriter, data []byte) (e Error.Error)
}

type BaseProtoHandler struct {
}

func (t BaseProtoHandler) ParseInput(req *http.Request, reqMsg proto.Message) (e Error.Error) {
	reqBuffer, err := ioutil.ReadAll(req.Body)
	if err != nil {
		log.Printf("ERR: ioutil.ReadAll failed: %v ", err)
		return Error.New(cs.IOREAD_ERROR, err.Error())
	}

	err = proto.Unmarshal(reqBuffer, reqMsg) //unSerialize into reqMsg
	if err != nil {
		log.Printf("ERR: checkInput parse proto err: %v", err)
		return Error.New(cs.UNMARSHAL_ERROR, err.Error())
	}
	log.Printf("recv reqMsg: %+v", reqMsg)

	return Error.OK()
}

func (t BaseProtoHandler) SendResponse(rsp http.ResponseWriter, data []byte) (e Error.Error) {
	_, e = common.SendResponse(rsp, data)
	return e
}
