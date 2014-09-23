using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitsMainModule : ModuleBase {
	UnitParty unitPartyInfo;
	Dictionary<int,UserUnit> userUnit = new Dictionary<int, UserUnit> ();

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
