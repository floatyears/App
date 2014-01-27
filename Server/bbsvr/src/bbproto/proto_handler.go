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
	//"../data"
	proto "code.google.com/p/goprotobuf/proto"
)

type ProtoHandler interface {
	//parse input into reqMsg
	ParseInput(req *http.Request, reqMsg proto.Message) (err error)
	SendResponse(rsp http.ResponseWriter, data []byte) (err error)
}

type BaseProtoHandler struct {
}

func (t BaseProtoHandler) ParseInput(req *http.Request, reqMsg proto.Message) (err error) {
	reqBuffer, err := ioutil.ReadAll(req.Body)
	if err != nil {
		log.Printf("ERR: ioutil.ReadAll failed: %v ", err)
		return err
	}

	err = proto.Unmarshal(reqBuffer, reqMsg) //unSerialize into reqMsg
	if err != nil {
		log.Printf("ERR: checkInput parse proto err: %v", err)
		return err
	}
	log.Printf("recv reqMsg: %+v", reqMsg)

	return err
}

func (t BaseProtoHandler) SendResponse(rsp http.ResponseWriter, data []byte) (err error) {
	_, err = common.SendResponse(rsp, data)
	return err
}
