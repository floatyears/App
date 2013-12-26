package main

import (
	"log"
)
import (
	//bbproto "./bbproto"
	"./data"
	//proto "code.google.com/p/goprotobuf/proto"
	//"io/ioutil"
)

type Base struct {
	Name string
}

func (b *Base) Na() {
	log.Printf("base.Na(): %v", b.Name)
	if b.Name == "" {
		b.Name = "newMe"
	}
}

type Foo struct {
	Base
}

//func (f *Foo) Na() {
//	log.Printf("Foo.Na(): f.Name=%v", f.Name)
//}

func (f *Foo) NewNa() {
	log.Printf("Foo.NewNa()")
}

func Test() error {
	f := &Foo{}
	log.Printf("Test f: %v", f)
	f.Na()
	log.Printf("Test f.Name: %v", f.Name)

	return nil
}

func testRedis() error {
	db := &data.Data{}
	log.Printf("[1] db: %v", db)

	key := string("0")
	err := db.Open(key)
	if err != nil {
		return err
	}
	defer db.Close()
	log.Printf("[2] db: %v", db)
	id := "kory"

	/////////////////////////
	//msg, err := ioutil.ReadFile("./msg.pak")
	//err = db.Set(id, string(msg)) //
	//log.Printf("set'%v' value:%v ret err:%v", id, msg, err)

	err = db.SetInt(id, (1100002)) //
	log.Printf("redis.set('%v'，110002) ret err:%v", id, err)

	//value, err := db.Gets(id)
	value, err := db.GetInt(id)
	log.Printf("after dg.Get('%v') ret: %v", id, value)
	//msg := &bbproto.UserInfo{}
	//err = proto.Unmarshal(value, msg) //unSerialize into msg

	//log.Printf("after parse ret:%v, msg: %+v", err, msg)

	return err
}
