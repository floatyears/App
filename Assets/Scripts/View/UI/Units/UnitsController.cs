using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsController : ConcreteComponent, IUICallback {
	TUnitParty unitPartyInfo;
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();

	public UnitsController(string uiName):base(uiName) {
	
	}

	public override void ShowUI () {
		base.ShowUI ();
	
    }
	
	public override void HideUI () {
		base.HideUI ();

	}

	public void CallbackView (object data){
		try {
			SceneEnum se = (SceneEnum)data;
			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}

}
