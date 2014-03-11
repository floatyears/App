package bbproto

import (
	_ "fmt"
	"io/ioutil"
	"net/http"
	//"strconv"
	"reflect"
)
import (
	proto "code.google.com/p/goprotobuf/proto"
	"common/EC"
	"common/Error"
	"common/log"
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
		log.Error("ERR: ioutil.ReadAll failed: %v ", err)
		return Error.New(EC.IOREAD_ERROR, err.Error())
	}

	//log.T("recv reqBuffer: %+v", reqBuffer)

	err = proto.Unmarshal(reqBuffer, reqMsg) //unSerialize into reqMsg
	if err != nil {
		log.Error("Unmarshal proto err: %v", err)
		return Error.New(EC.UNMARSHAL_ERROR, err.Error())
	}
	log.T("==================================================")
	log.T("recv reqMsg(%v): %+v", reflect.TypeOf(reqMsg), reqMsg)

	return Error.OK()
}

func (t BaseProtoHandler) SendResponse(rsp http.ResponseWriter, data []byte) (e Error.Error) {

	if data == nil {
		log.Fatal("Cannot SendResponse empty data bytes")
		return Error.New(EC.INVALID_PARAMS, "Cannot SendResponse empty data bytes")
	}

	size, err := rsp.Write(data)
	if err != nil {
		return Error.New(EC.IOWRITE_ERROR, err.Error())
	}

	log.T("reponse data size:%v", size)

	return Error.OK()
}
