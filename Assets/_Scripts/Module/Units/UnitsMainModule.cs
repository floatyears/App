using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsMainModule : ModuleBase {
	TUnitParty unitPartyInfo;
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();

	public UnitsMainModule(UIConfigItem config):base(  config) {
		CreateUI<UnitsMainView> ();
	}

	public override void ShowUI () {
		base.ShowUI ();
	
    }

	public void OnReceiveMessage (object data){
		try {
			ModuleEnum se = (ModuleEnum)data;
			ModuleManager.Instance.ShowModule(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}

}
