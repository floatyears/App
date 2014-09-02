using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsModule : ModuleBase {
	TUnitParty unitPartyInfo;
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();

	public UnitsModule(UIConfigItem config):base(  config) {
//		CreateUI<unitsvi
	}

	public override void ShowUI () {
		base.ShowUI ();
	
    }
	
	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}

	public void OnReceiveMessage (object data){
		try {
			ModuleEnum se = (ModuleEnum)data;
			ModuleManger.Instance.ShowModule(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}

}
