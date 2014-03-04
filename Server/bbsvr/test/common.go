package main

import (
	"io"
	"io/ioutil"
	"log"
	"math/rand"
	"net/http"
	"time"
)

const (
	_PROTO_LOGIN_PACK     = "/login_pack"
	_PROTO_AUTH_USER      = "/auth_user"
	_PROTO_GET_QUEST_MAP  = "/get_new_quest_map"
	_PROTO_START_QUEST    = "/start_quest"
	_PROTO_CLEAR_QUEST    = "/clear_quest"
	_PROTO_GET_QUEST_INFO = "/get_quest_info"
	_PROTO_RENAME_NICK    = "/rename_nick"

	//friend
	_PROTO_GET_FRIEND    = "/get_friend"
	_PROTO_ADD_FRIEND    = "/add_friend"
	_PROTO_DEL_FRIEND    = "/del_friend"
	_PROTO_ACCEPT_FRIEND = "/accept_friend"
	_PROTO_FIND_FRIEND   = "/find_friend"
)

//const WEB_SERVER_ADDR = "http://127.0.0.1:6666"

const WEB_SERVER_ADDR = "http://107.170.243.127:6666"

func Init() {
	log.SetFlags(log.Ltime | log.Lmicroseconds | log.Lshortfile)
	rand.Seed(time.Now().UTC().UnixNano())

}

func SendHttpPost(dataBuf io.Reader, protoAddr string) (outbuffer []byte, err error) {
	resp, err := http.Post(WEB_SERVER_ADDR+protoAddr, "application/binary", dataBuf)
	if resp != nil && resp.Body != nil {
		log.Printf("SendHttpPost resp.Body.Close()...")
		defer resp.Body.Close()
	}

	if err != nil {
		log.Printf("post err:%+v", err)
		return nil, err
	}
	if resp.StatusCode != http.StatusOK {
		log.Printf("post ret code:%+v", resp.StatusCode)
		return nil, err
	}

	log.Printf("Header:%+v", resp.Header)
	outbuffer, err = ioutil.ReadAll(resp.Body)
	//log.Printf("recv resp:%+v", outbuffer)

	return outbuffer, err
}
