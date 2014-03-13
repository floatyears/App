package EC

//common error
const (
	SUCCESS          = 0
	ERROR_BASE       = -100
	FAILED           = ERROR_BASE - 1
	INVALID_PARAMS   = ERROR_BASE - 2
	MARSHAL_ERROR    = ERROR_BASE - 3
	UNMARSHAL_ERROR  = ERROR_BASE - 4
	IOREAD_ERROR     = ERROR_BASE - 5
	IOWRITE_ERROR    = ERROR_BASE - 6
	CONNECT_DB_ERROR = ERROR_BASE - 7
	READ_DB_ERROR    = ERROR_BASE - 8
	SET_DB_ERROR     = ERROR_BASE - 9
	DATA_NOT_EXISTS  = ERROR_BASE - 10
)

//user error
const (
	EU_USER_BASE             = -200
	EU_INVALID_USERID        = EU_USER_BASE - 1
	EU_GET_USERINFO_FAIL     = EU_USER_BASE - 2
	EU_USER_NOT_EXISTS       = EU_USER_BASE - 3
	EU_GET_NEWUSERID_FAIL    = EU_USER_BASE - 4
	EU_UPDATE_USERINFO_ERROR = EU_USER_BASE - 5
	E_NO_ENOUGH_MONEY	= EU_USER_BASE - 6
)

//friend error
const (
	EF_FRIEND_BASE          = -300
	EF_FRIEND_NOT_EXISTS    = EF_FRIEND_BASE - 1
	EF_GET_FRIENDINFO_FAIL  = EF_FRIEND_BASE - 2
	EF_ADD_FRIEND_FAIL      = EF_FRIEND_BASE - 3
	EF_DEL_FRIEND_FAIL      = EF_FRIEND_BASE - 4
	EF_IS_ALREADY_FRIEND    = EF_FRIEND_BASE - 5
	EF_IS_ALREADY_FRIENDOUT = EF_FRIEND_BASE - 6
	EF_INVALID_FRIEND_STATE = EF_FRIEND_BASE - 7
)

//quest error
const (
	EQ_QUEST_BASE                = -400
	EQ_QUEST_ID_INVALID          = EQ_QUEST_BASE - 1
	EQ_GET_QUESTINFO_ERROR       = EQ_QUEST_BASE - 2
	EQ_STAMINA_NOT_ENOUGH        = EQ_QUEST_BASE - 3
	EQ_GET_QUEST_CONFIG_ERROR    = EQ_QUEST_BASE - 4
	EQ_GET_QUEST_LOG_ERROR       = EQ_QUEST_BASE - 5
	EQ_UPDATE_QUEST_RECORD_ERROR = EQ_QUEST_BASE - 6
	EQ_INVALID_DROP_UNIT         = EQ_QUEST_BASE - 7
	EQ_QUEST_IS_PLAYING          = EQ_QUEST_BASE - 8
	EQ_USER_QUEST_NOT_PLAYING    = EQ_QUEST_BASE - 9
)

//unit error
const (
	E_UNIT_BASE					= -500
	E_UNIT_ID_ERROR 			= E_UNIT_BASE - 1
	E_GACHA_TIME_INVALID		= E_UNIT_BASE - 2
	E_GET_UNIT_INFO_ERROR 		= E_UNIT_BASE - 3
	E_UNIT_HAS_NO_EVOLVEINFO 	= E_UNIT_BASE - 4
	E_UNIT_CANNOT_EVOLVE_TODAY 	= E_UNIT_BASE - 5
)
