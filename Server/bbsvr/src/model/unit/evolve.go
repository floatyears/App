package unit

import (
	"bbproto"
	"code.google.com/p/goprotobuf/proto"
	"common"
	"common/EC"
	"common/Error"
	"common/consts"
	"common/log"
	"data"
)

func SaveEvolveSession(db *data.Data, reqMsg *bbproto.ReqEvolveStart) (e Error.Error) {

	if db == nil {
		return Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}
	if err := db.Select(consts.TABLE_UNIT); err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	zData, err := proto.Marshal(reqMsg)
	if err != nil {
		return Error.New(EC.MARSHAL_ERROR, err.Error())
	}

	if err = db.Set(consts.X_EVOLVE_SESSION+common.Utoa(*reqMsg.Header.UserId), zData); err != nil {
		return Error.New(EC.SET_DB_ERROR, err.Error())
	}
	log.T("SaveEvolveSession for (%v) , return OK", *reqMsg.Header.UserId)

	return Error.OK()
}

func ReadEvolveSession(db *data.Data, uid uint32, reqMsg *bbproto.ReqEvolveStart) (e Error.Error) {

	if db == nil {
		return Error.New(EC.INVALID_PARAMS, "invalid db pointer")
	}
	if err := db.Select(consts.TABLE_UNIT); err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	value, err := db.Gets(consts.X_EVOLVE_SESSION + common.Utoa(uid))
	if err != nil {
		return Error.New(EC.READ_DB_ERROR, err.Error())
	}

	err = proto.Unmarshal(value, reqMsg)
	if err != nil {
		log.Error("[ERROR] ReadEvolveSession for '%v' ret err:%v", uid, err)
		return Error.New(EC.UNMARSHAL_ERROR, err)
	}

	log.T("ReadEvolveSession ret: %+v", reqMsg)

	return Error.OK()
}


func CheckEvolveSession(db *data.Data, uid uint32) (reqMsg *bbproto.ReqEvolveStart, e Error.Error) {

	reqMsg = &bbproto.ReqEvolveStart{};
	if e = ReadEvolveSession(db, uid, reqMsg); e.IsError() {
		return nil, e
	}
	if reqMsg == nil {
		log.Error("ReadEvolveSession for %v return nil", uid)
		return nil, Error.New(EC.READ_DB_ERROR, "ReadEvolveSession return nil")
	}


	return reqMsg, Error.OK()
}
