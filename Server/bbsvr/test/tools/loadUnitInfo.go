package main

import (
	//bbproto "./bbproto"
	proto "code.google.com/p/goprotobuf/proto"
	"common"
	"common/consts"
	"common/log"
	"data"
	//"io/ioutil"
	//"strconv"
	//"time"
	"bbproto"
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

func LoadUnitInfoToDB(path string) {

	db := &data.Data{}
	err := db.Open(consts.TABLE_UNIT)
	if err != nil {
		log.Error("open db failed.")
		return
	}
	defer db.Close()

	for i := 1; i <= 29; i++ {
		filename := path + common.Itoa(i) +".bytes"
		zData, err := common.ReadFile(filename)
		if err != nil {
			log.Error("readfile error:%v", err)
			continue
		}
		log.T("[%v] readfile(%v) zData: %+v", i, filename, len(zData))

		unitinfo := &bbproto.UnitInfo{}
		if err = proto.Unmarshal(zData, unitinfo); err != nil {
			log.T("[ERROR] unmarshal error from unit[%v].", i)
			continue
		}

		unitinfo.SaleValue = proto.Int32(int32(i) * 100)

		log.T("[%v] unitinfo: %+v", i, unitinfo)
		zData, err = proto.Marshal(unitinfo)
		if err != nil {
			log.T("unit[%v] Marshal err: %+v", i, err)
			continue
		}

//		if err = db.Set(consts.X_UNIT_INFO+common.Itoa(i), zData); err != nil {
//			log.Error("[%v] unitinfo save failed.", i)
//		}
	}
}

func GenerateUnit(path string) {
	db := &data.Data{}
	err := db.Open(consts.TABLE_UNIT)
	if err != nil {
		log.Error("open db failed.")
		return
	}
	defer db.Close()

	for i := int32(1); i <= 29; i++ {
		uiitem := &bbproto.UnitInfo{}
		uiitem.Id = proto.Uint32(uint32(i))
		uiitem.Name = proto.String("unit_" + common.Ntoa(i))
		unitType := (bbproto.EUnitType)(1 + i%6)
		uiitem.Type = &unitType
		uiitem.Skill1 = proto.Int32((i - 1) * 2 % 10)
		uiitem.Skill2 = proto.Int32(((i-1)*2 + 1) % 10)
		uiitem.PowerType = &bbproto.PowerType{}
		uiitem.PowerType.ExpType = proto.Int32(1)
		uiitem.PowerType.AttackType = proto.Int32(2)
		uiitem.PowerType.HpType = proto.Int32(3)
		uiitem.Cost = proto.Int32((i % 5) + 1)
		race := (bbproto.EUnitRace)(i%7) + 1
		uiitem.Race = &race
		uiitem.Rare = proto.Int32(i % 6)
		uiitem.MaxLevel = proto.Int32(10)
		uiitem.SaleValue = proto.Int32(common.Rand(1, 15) * 100)
		uiitem.DevourValue = proto.Int32(common.Rand(1, 5) * 100)
		if i == 1 {
			uiitem.LeaderSkill = proto.Int32(21)
			uiitem.ActiveSkill = proto.Int32(32)
		}
		if i == 2 {
			uiitem.ActiveSkill = proto.Int32(38)
		}
		if i == 5 {
			uiitem.LeaderSkill = proto.Int32(22)
		}

		uiitem.PassiveSkill = proto.Int32(49)

		zData, err := proto.Marshal(uiitem)
		if err != nil {
			log.T("unit[%v] Marshal err: %+v", i, err)
			continue
		}

		if err = db.Set(consts.X_UNIT_INFO+common.Ntoa(i), zData); err != nil {
			log.Error("[%v] unitinfo save failed.", i)
		}

		if err = common.WriteFile(zData, path+common.Ntoa(i)+".bytes"); err != nil {
			log.Error("writefile fail for unit(%v)", i)
		}
		log.T("unit:%+v", uiitem)
	}

}

func main() {
	path := "/Users/kory/Documents/Dev/BB002/bb002/Server/bbsvr/test/units/"

	LoadUnitInfoToDB(path)
	//	GenerateUnit(path)
}
