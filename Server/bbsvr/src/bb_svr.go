package main

import (
	//"code.google.com/p/goprotobuf/proto"
	"fmt"
	"github.com/golang/glog"
	"html"
	"log"
	"net/http"
	"runtime/debug"
)
import (
	//"./data"
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
)

func safeHandler(fn http.HandlerFunc) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		defer func() {
			if err, ok := recover().(error); ok {
				http.Error(w, err.Error(), http.StatusInternalServerError) // or custom 50x error pages
				// w.WriteHeader(http.StatusInternalServerError)
				// renderHtml(w, "error", e)
				// logging
				log.Println("WARN: panic in %v. - %v", fn, err)
				log.Println(string(debug.Stack()))
			}
		}()
		fn(w, r)
	}
}

func LoginHandler(rsp http.ResponseWriter, req *http.Request) {
	log.Fatal("login : %s", req.URL.Path)
}

func ProtoHandler(rsp http.ResponseWriter, req *http.Request) {
	fmt.Fprintf(rsp, "protoHandler:: %q", html.EscapeString(req.URL.Path))
	glog.Info("handleFunc :", req.URL.Path)
}

func Init() {
	log.SetFlags(log.Ltime | log.Lmicroseconds | log.Lshortfile)
}

func main() {
	Init()

	//return

	http.HandleFunc(_PROTO_LOGIN_PACK, safeHandler(user.LoginPackHandler))
	http.HandleFunc(_PROTO_AUTH_USER, safeHandler(user.AuthUserHandler))
	//http.HandleFunc(_PROTO_GET_QUEST_MAP, safeHandler(quest.GetQuestMapHandler))
	http.HandleFunc(_PROTO_START_QUEST, safeHandler(quest.StartQuestHandler))
	http.HandleFunc(_PROTO_CLEAR_QUEST, safeHandler(quest.ClearQuestHandler))
	http.HandleFunc(_PROTO_GET_QUEST_INFO, safeHandler(quest.GetQuestInfoHandler))

	ret := http.ListenAndServe(":8000", nil)
	log.Fatal("http.ListenAndServe ret:%d", ret)
}
