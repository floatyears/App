package main

import (
	//"code.google.com/p/goprotobuf/proto"
	"fmt"
	"github.com/golang/glog"
	"html"
	"math/rand"
	"net/http"
	"runtime/debug"
	"time"
)
import (
	//"./data"
	"./common/log"
	"./friend"
	"./quest"
	"./user"
)

const (
	_PROTO_LOGIN_PACK     = "/login_pack"
	_PROTO_AUTH_USER      = "/auth_user"
	_PROTO_GET_QUEST_MAP  = "/get_new_quest_map"
	_PROTO_START_QUEST    = "/start_quest"
	_PROTO_CLEAR_QUEST    = "/clear_quest"
	_PROTO_GET_QUEST_INFO = "/get_quest_info"

	//friend
	_PROTO_GET_FRIEND    = "/get_friend"
	_PROTO_ADD_FRIEND    = "/add_friend"
	_PROTO_DEL_FRIEND    = "/del_friend"
	_PROTO_ACCEPT_FRIEND = "/accept_friend"
	_PROTO_FIND_FRIEND   = "/find_friend"
)

func safeHandler(fn http.HandlerFunc) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		defer func() {
			if err, ok := recover().(error); ok {
				http.Error(w, err.Error(), http.StatusInternalServerError) // or custom 50x error pages
				// w.WriteHeader(http.StatusInternalServerError)
				// renderHtml(w, "error", e)
				// logging
				log.Error("WARN: panic in %v. - %v", fn, err)
				log.Error(string(debug.Stack()))
			}
		}()
		fn(w, r)
	}
}

func ProtoHandler(rsp http.ResponseWriter, req *http.Request) {
	fmt.Fprintf(rsp, "protoHandler:: %q", html.EscapeString(req.URL.Path))
	glog.Info("handleFunc :", req.URL.Path)
}

func Init() {
	log.SetFlags(log.Ltime | log.Lmicroseconds | log.Lshortfile)

	rand.Seed(time.Now().UTC().UnixNano())

}

func main() {
	Init()

	//testRedis()
	//Test()
	//return

	/** user protocol **/
	http.HandleFunc(_PROTO_LOGIN_PACK, safeHandler(user.LoginPackHandler))
	http.HandleFunc(_PROTO_AUTH_USER, safeHandler(user.AuthUserHandler))

	/** friend protocol **/
	http.HandleFunc(_PROTO_GET_FRIEND, safeHandler(friend.GetFriendHandler))
	http.HandleFunc(_PROTO_ADD_FRIEND, safeHandler(friend.AddFriendHandler))
	http.HandleFunc(_PROTO_DEL_FRIEND, safeHandler(friend.DelFriendHandler))
	http.HandleFunc(_PROTO_FIND_FRIEND, safeHandler(friend.FindFriendHandler))
	http.HandleFunc(_PROTO_ACCEPT_FRIEND, safeHandler(friend.AcceptFriendHandler))

	/** quest protocol **/
	//http.HandleFunc(_PROTO_GET_QUEST_MAP, safeHandler(quest.GetQuestMapHandler))
	http.HandleFunc(_PROTO_START_QUEST, safeHandler(quest.StartQuestHandler))
	http.HandleFunc(_PROTO_CLEAR_QUEST, safeHandler(quest.ClearQuestHandler))
	//http.HandleFunc(_PROTO_GET_QUEST_INFO, safeHandler(quest.GetQuestInfoHandler))

	ret := http.ListenAndServe(":8000", nil)
	log.Fatal("http.ListenAndServe ret:%d", ret)
}
