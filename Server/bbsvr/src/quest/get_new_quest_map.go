package quest

import (
	"fmt"
	//"errors"
	"io/ioutil"
	"log"
	"net/http"
	_ "strconv"
	//"runtime/debug"
)
import (
	"../bbproto"
	_ "../data"
	proto "code.google.com/p/goprotobuf/proto"
)

//import "../comm"

//import glog "github.com/golang/glog"

func genReqGetQuestMap() (buffer []byte, err error) {
	msg := &bbproto.ReqGetNewQuestMap{
		StartId: proto.Int32(2),
		Header: &bbproto.Header{
			ApiVer:    proto.String("0.1"),
			Code:      proto.Int32(0),
			Error:     proto.String(""),
			PacketId:  proto.Int32(1),
			SessionId: proto.String("10000001"),
		},
	}
	buffer, err = proto.Marshal(msg) //SerializeToOstream
	return buffer, err
}

//req_data, err := genReqGetQuestMap()
//msg := &bbproto.Person{
//	Name: proto.String("Rose Mary"),
//	Id:   proto.Int32(20),
//}
//buffer, err := proto.Marshal(msg) //SerializeToOstream

func GetQuestMapHandler(rsp http.ResponseWriter, req *http.Request) {
	req_data, err := ioutil.ReadAll(req.Body)
	if err != nil {
		fmt.Fprintf(rsp, "%s", err)
	}
	log.Printf("GetQuestMap req.body: %v ", req_data)

	msg := &bbproto.ReqGetNewQuestMap{}
	err = proto.Unmarshal(req_data, msg) //unSerialize into msg

	if err != nil {
		log.Printf("parse proto err: %v", err)
		fmt.Fprintf(rsp, "parse proto err: %v", err)
		return
	}
	log.Printf("recv msg: %+v", msg)

	//db := &data.Data{}
	//db.Open("0")
	//id := strconv.Itoa(int(*msg.Id))
	//value, err := db.Get(id)
	//log.Printf("get for '%v' ret: %v", msg.Id, value)
	//db.Close()

	//buffer, err := proto.Marshal(msg) //SerializeToOstream
	buffer := "{mapinfo}"
	size, err := rsp.Write([]byte(buffer))

	log.Printf("rsp msg[%d]: %+v", size, msg)
}
