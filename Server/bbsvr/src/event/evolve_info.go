package event

import (
	"bbproto"
	"time"
	//"common/Error"
	"common"
	"common/log"
	//"common/consts"
	//"data"
	//proto "code.google.com/p/goprotobuf/proto"
)

func GetEvolveType() (etype *bbproto.EUnitType) {
	evolveType := bbproto.EUnitType(bbproto.EUnitType_UALL)
	switch common.WeekDay() {
	case time.Sunday:
		evolveType = bbproto.EUnitType(bbproto.EUnitType_UALL)

	case time.Monday:
		evolveType = bbproto.EUnitType(bbproto.EUnitType_UDARK)

	case time.Tuesday:
		evolveType = bbproto.EUnitType(bbproto.EUnitType_UFIRE)

	case time.Wednesday:
		evolveType = bbproto.EUnitType(bbproto.EUnitType_UWATER)

	case time.Thursday:
		evolveType = bbproto.EUnitType(bbproto.EUnitType_UWIND)

	case time.Friday:
		evolveType = bbproto.EUnitType(bbproto.EUnitType_ULIGHT)

	case time.Saturday:
		evolveType = bbproto.EUnitType(bbproto.EUnitType_UNONE)
	}

	log.T("GetEvolveType return %v", evolveType)
	return &evolveType
}
