package consts

const (
	DEFAULT_USER_NAME    = ""
	N_DEFAULT_COST_MAX   = 50
	N_DEFAULT_UNIT_MAX   = 50
	N_DEFAULT_FRIEND_MAX = 50
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
	X_QUEST_RECORD = "X_Q_REC_"

	//unit
	X_UNIT_INFO      = "X_UNIT_"
	X_SKILL_INFO     = "X_SKILL_"
	X_EVOLVE_SESSION = "X_EVOLVE_"
	X_GACHA_UNIT     = "X_GACHA_"
	X_GACHA_CONF     = "X_GACHA_CONF"
)

const (
	KEY_MAX_USER_ID  = "K_MAX_USER_ID"
	KEY_MAX_UNIT_ID  = "K_MAX_UNIT_ID"
	KEY_QUEST_PREFIX = "K_QUEST_INFO_"
)

const (
	N_MAX_RARE       = 6
	N_MAX_USER_RANK  = 500
	N_MAX_UNIT_NUM   = 400
	N_MAX_FRIEND_NUM = 200

	N_DUNGEON_GRID_COUNT    = 25
	N_USER_SPACE_PARTS      = 10
	N_HELPER_RANK_RANGE     = 5
	N_STAMINA_TIME          = 600 // seconds
	N_QUEST_COLOR_BLOCK_NUM = 120 //2400
	N_GACHA_MAX_COUNT       = 9

	N_UNITMAX_EXPAND_COUNT   = 5
	N_FRIENDMAX_EXPAND_COUNT = 5
)

// consume cost
const (
	N_GACHA_FRIEND_COST = 200 // cost 200 friend points
	N_GACHA_BUY_COST    = 5   // cost 5 stone for a gacha

	N_UNITMAX_EXPAND_COST   = 1 // cost 1 stone
	N_FRIENDMAX_EXPAND_COST = 1 // cost 1 stone
	N_REDO_QUEST_COST       = 1 // cost 1 stone
	N_RESUME_BATTLE_COST    = 1 // cost 1 stone
)
