package quest

import (
	"fmt"
	//"errors"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
	//"runtime/debug"
)
import (
	"../data"
	bbproto "./proto"
	proto "code.google.com/p/goprotobuf/proto"
)

//import "../comm"

//import glog "github.com/golang/glog"

func genReqGetQuestMap() (buffer []byte, err error) {
	msg := &bbproto.ReqGetQuestMap{
		StartId: proto.Int32(2),
		Req: &bbproto.Request{
			Version:  proto.Int32(1),
			CliVer:   proto.String("0.1"),
			Userid:   proto.Int32(1001),
			Username: proto.String("kory"),
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

	////msg := &bbproto.ReqGetQuestMap{}
	msg := &bbproto.Person{}
	err = proto.Unmarshal(req_data, msg) //unSerialize into msg

	if err != nil {
		log.Printf("parse proto err: %v", err)
		fmt.Fprintf(rsp, "parse proto err: %v", err)
		return
	}
	log.Printf("recv msg: %+v", msg)

	//utils.WriteData(req_data, "./msg.pak")

	db := &data.Data{}
	db.Open("0")
	id := strconv.Itoa(int(*msg.Id))
	value, err := db.Get(id)
	log.Printf("get for '%v' ret: %v", msg.Id, value)
	db.Close()

	msg.Id = proto.Int(110)
	msg.Name = proto.String("kory - bigbang")
	msg.Email = proto.String("kory@big-bang-games.com")
	buffer, err := proto.Marshal(msg) //SerializeToOstream
	size, err := rsp.Write(buffer)

	//fmt.Fprintf(rsp, "Person msg: UserId: %v - time: %v. price:%v", *msg.Id, *msg.Name, *msg.Email)
	log.Printf("rsp msg[%d]: %+v", size, msg)
}
