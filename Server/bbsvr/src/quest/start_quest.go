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

func StartQuestHandler(rsp http.ResponseWriter, req *http.Request) {
	req_data, err := ioutil.ReadAll(req.Body)
	if err != nil {
		fmt.Fprintf(rsp, "%s", err)
	}
	log.Printf("GetQuestMap req.body: %v ", req_data)

	msg := &bbproto.ReqStartQuest{}
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
