using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsController : ModuleBase {
	TUnitParty unitPartyInfo;
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();

	public UnitsController(string uiName):base(uiName) {
	
	}

	public override void ShowUI () {
		base.ShowUI ();
	
    }
	
	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}

	public void CallbackView (object data){
		try {
			ModuleEnum se = (ModuleEnum)data;
			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}

}
