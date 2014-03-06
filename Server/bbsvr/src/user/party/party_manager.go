package party

import (
	"../../bbproto"
	"../../common/Error"
	"../../common/log"
	"../../const"
	"../../data"
	"../../user/usermanage"
	//proto "code.google.com/p/goprotobuf/proto"
)

func ChangeParty(db *data.Data, uid uint32, party *bbproto.PartyInfo) (e Error.Error) {
	if db == nil {
		db = &data.Data{}
		err := db.Open(cs.TABLE_USER)
		defer db.Close()
		if err != nil {
			log.Error("uid:%v, open db ret err:%v", uid, err)
			return Error.New(cs.CONNECT_DB_ERROR, err)
		}
	} else {
		if err := db.Select(cs.TABLE_USER); err != nil {
			return Error.New(cs.READ_DB_ERROR, err)
		}
	}

	userDetail, isExists, err := usermanage.GetUserInfo(db, uid)
	if err != nil {
		log.Error("[UNMARSHAL_ERROR] GetUserInfo for '%v' ret err:%v", uid, e.Error())
		return Error.New(err)
	}
	if !isExists {
		return Error.New(cs.EU_USER_NOT_EXISTS)
	}

	userDetail.Party = party

	//save data
	e = usermanage.UpdateUserInfo(db, &userDetail)

	log.T("user:%v changeParty success.", uid)
	return e
}
