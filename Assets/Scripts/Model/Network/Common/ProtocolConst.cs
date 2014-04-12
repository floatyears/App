using System;

class Protocol {
    public const string API_VERSION = "1.0";

    public const string AUTH_USER = "auth_user";
    public const string RENAME_NICK = "rename_nick";
	public const string CHANGE_PARTY = "change_party";

    public const string START_QUEST = "start_quest";
    public const string CLEAR_QUEST = "clear_quest";
	public const string RETIRE_QUEST = "retire_quest";
	public const string RESUME_QUEST = "resume_quest";
	public const string REDO_QUEST = "redo_quest";
	public const string GET_QUEST_COLORS = "get_quest_colors";
	
    public const string GET_FRIEND = "get_friend";
    public const string ADD_FRIEND = "add_friend";
    public const string DEL_FRIEND = "del_friend";
    public const string FIND_FRIEND = "find_friend";
    public const string ACCEPT_FRIEND = "accept_friend";
	
	public const string LEVEL_UP = "level_up";
    public const string EVOLVE_START = "evolve_start";
    public const string EVOLVE_DONE = "evolve_done";

    public const string SELL_UNIT = "sell_unit";
    public const string GACHA = "gacha";
    public const string FRIEND_MAX_EXPAND = "friend_max_expand";
    public const string RESTORE_STAMINA = "restore_stamina";
    public const string UNIT_MAX_EXPAND = "unit_max_expand";
	public const string UNIT_ADD_FAVORITE = "unit_add_favorite";
	public const string UNIT_GET_LIST = "unit_get_list";
}
