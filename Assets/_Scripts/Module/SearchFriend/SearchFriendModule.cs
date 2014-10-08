using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class SearchFriendModule : ModuleBase{
    uint searchFriendUid;

	public SearchFriendModule(UIConfigItem config) : base(   config ){
		CreateUI<SearchFriendView> ();
	}
	
}
