package party

import (
	"bbproto"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
	"model/user"
	//proto "code.google.com/p/goprotobuf/proto"
	"code.google.com/p/goprotobuf/proto"
)

func ChangeParty(db *data.Data, uid uint32, party *bbproto.PartyInfo) (e Error.Error) {
	if party == nil || len(party.PartyList) < 1 {
		log.T("invalid party==nil")
		return Error.New(EC.INVALID_PARAMS)
	}

	if db == nil {
		db = &data.Data{}
		err := db.Open(consts.TABLE_USER)
		defer db.Close()
		if err != nil {
			log.Error("uid:%v, open db ret err:%v", uid, err)
			return Error.New(EC.CONNECT_DB_ERROR, err)
		}
	} else {
		if err := db.Select(consts.TABLE_USER); err != nil {
			return Error.New(EC.READ_DB_ERROR, err)
		}
	}

	userDetail, isExists, err := user.GetUserInfo(db, uid)
	if err != nil {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret err:%v", uid, e.Error())
		return Error.New(err)
	}
	if !isExists {
		return Error.New(EC.EU_USER_NOT_EXISTS)
	}

	for k, partyInfo := range party.PartyList {
		if partyInfo.Id == nil {
			partyInfo.Id = proto.Int32(0)
			log.T("k[%v] partyInfo.Id==nil force to 0.", k)
		}
		for i, item := range partyInfo.Items {
			if item.UnitPos == nil {
				item.UnitPos = proto.Int32(0)
				log.T("i:%v %v item.pos==nil convert to 0", i, item)
			}
			if item.UnitUniqueId == nil {
				item.UnitUniqueId = proto.Uint32(0)
				log.T("i:%v %v item.UniqueId==nil convert to 0", i, item)
			}
		}
	}

	userDetail.Party = party
	userDetail.GetUser()
	//save data
	e = user.UpdateUserInfo(db, userDetail)

	log.T("user:%v changeParty success.", uid)
	return e
}
