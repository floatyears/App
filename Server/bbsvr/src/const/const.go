package cs

const (
	DEFAULT_USER_NAME = "no name"
)

//redis table name list
const (
	TABLE_USER      = "1"
	TABLE_UNIT      = "2"
	TABLE_QUEST     = "3"
	TABLE_SESSION   = "4"
	TABLE_FRIEND    = "5"
	TABLE_QUEST_LOG = "6"
)

//key prefix
const (
	X_UUID      = "X_UUID_"
	X_HELPER_MY = "X_HELPER_MY_" // A user's helpers
	X_USER_RANK = "X_USER_RANK_" // ZSET: uid - rank

	//quest
	X_QUEST_STAGE  = "X_STAGE_"
	X_QUEST_CONFIG = "X_CONFIG_"
	X_QUEST_LOG    = "X_Q_LOG_"
)

const (
	KEY_MAX_USER_ID  = "K_MAX_USER_ID"
	KEY_MAX_UNIT_ID  = "K_MAX_UNIT_ID"
	KEY_QUEST_PREFIX = "K_QUEST_INFO_"
)

const (
	N_DUNGEON_GRID_COUNT    = 25
	N_USER_SPACE_PARTS      = 10
	N_HELPER_RANK_RANGE     = 5
	N_STAMINA_TIME          = 600 // seconds
	N_QUEST_COLOR_BLOCK_NUM = 2400
)
