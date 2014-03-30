package main

import (
	"bytes"
	"code.google.com/p/goprotobuf/proto"
	//"fmt"
	_ "html"
	//"math/rand"
	//"strings"
)
import (
	"bbproto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"model/unit"
	//redis "github.com/garyburd/redigo/redis"
)

func Gacha(uid uint32, gachaId, gachaCount int32) error {
	msg := &bbproto.ReqGacha{}
	msg.Header = &bbproto.ProtoHeader{}
	msg.Header.ApiVer = proto.String("1.0.0")
	msg.Header.SessionId = proto.String("S10298090290")
	msg.Header.UserId = proto.Uint32(uid)

	msg.GachaId = proto.Int32(gachaId)
	msg.GachaCount = proto.Int32(gachaCount)

	buffer, err := proto.Marshal(msg)
	if err != nil {
		log.Printf("Marshal ret err:%v buffer:%v", err, buffer)
	}

	rspbuff, err := SendHttpPost(bytes.NewReader(buffer), _PROTO_GACHA)
	if err != nil {
		log.Printf("SendHttpPost ret err:%v", err)
		return err
	}

	//decode rsp msg
	log.Printf("-----------------------Response (DataLen:%v)----------------------", len(rspbuff))
	rspmsg := &bbproto.RspGacha{}
	if err = proto.Unmarshal(rspbuff, rspmsg); err != nil {
		log.Printf("ERROR: rsp Unmarshal ret err:%v", err)
	}

	//print rsp msg
	log.Printf("Reponse : [%v] error: %v", *rspmsg.Header.Code, *rspmsg.Header.Error)
	log.T("=================rspMsg begin==================")
	log.T("\t unitUniqueId: %+v", rspmsg.UnitUniqueId)
	log.T("\t BlankUnitId: %+v", rspmsg.BlankUnitId)
	log.T("\t Stone: %v", *rspmsg.Stone)
	log.T("\t FriendPoint: %v", *rspmsg.FriendPoint)

	for k, unit := range rspmsg.UnitList {
		log.T("\t unit[%v]: %+v", k, unit)
	}

	return err
}

func AddGachaPool(gachaId int32) {
	//unitList := []uint32{}
		unitList := []uint32 {1,5,9,49,50,51,52,53,54,61,63,64,65,66,73,75,76,77,78,89,90,152,153}

	if gachaId == 3 {
		unitList := []uint32{}
		for i := int32(1); i <= 20; i++ {
			unitList = append(unitList, uint32(1))
		}
		for i := int32(1); i <= 20; i++ {
			unitList = append(unitList, uint32(51))
			unitList = append(unitList, uint32(52))
		}
		unit.SetGachaPool(nil, gachaId, unitList)

	} else {
		
		for i := int32(1); i <= 10; i++ {
			unitList = append(unitList, uint32(gachaId*i))
		}

		unit.SetGachaPool(nil, gachaId, unitList)

	}
	
}

func AddGachaConfig(db *data.Data, gachaId int32, gachaConf *bbproto.GachaConfig) (e Error.Error) {

	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_UNIT)
		defer db.Close()
		if err != nil {
			return Error.New(EC.READ_DB_ERROR, err)
		}
	} else if err := db.Select(consts.TABLE_UNIT); err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	zData, err := proto.Marshal(gachaConf)
	if err != nil {
		return Error.New(EC.MARSHAL_ERROR, err.Error())
	}

	if err = db.Set(consts.X_GACHA_CONF+common.Ntoa(gachaId), zData); err != nil {
		return Error.New(EC.SET_DB_ERROR, err.Error())
	}

	return Error.OK()
}

func AddSomeGachaConf() {
	gachaConf := &bbproto.GachaConfig{}
	gachaConf.BeginTime = proto.Uint32(common.Now() - 1000)
	gachaConf.EndTime = proto.Uint32(0)
	gachaConf.GachaId = proto.Int32(1)
	gachaType := bbproto.EGachaType_E_FRIEND_GACHA
	gachaConf.GachaType = &gachaType

	AddGachaConfig(nil, 1, gachaConf)

	gachaConf.GachaId = proto.Int32(2)
	gachaType = bbproto.EGachaType_E_BUY_GACHA
	gachaConf.GachaType = &gachaType
	AddGachaConfig(nil, 2, gachaConf)

	gachaConf.GachaId = proto.Int32(3)
	gachaType = bbproto.EGachaType_E_BUY_GACHA_EVENT
	gachaConf.GachaType = &gachaType
	AddGachaConfig(nil, 3, gachaConf)
}

func main() {
	log.Printf("==============================================")
	log.Printf("bbsvr test client begin...")

	Init()

	AddGachaPool(1)
	AddGachaPool(2)
	AddGachaPool(3) // for unitId 1
	AddSomeGachaConf()

	//uid := uint32(174)
	//gachaId := int32(3)
	//gachaCount := int32(2)
	//Gacha(uid, gachaId, gachaCount)

	log.Fatal("bbsvr test client finish.")
}
