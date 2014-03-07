using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsComponent : ConcreteComponent, IUICallback {

	TUnitParty unitPartyInfo;
	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();

	public UnitsComponent(string uiName):base(uiName) {}

	public override void CreatUI () {
		base.CreatUI ();
//		Debug.LogError( "Units Scene: CreateUI" + this);
	}
	
	public override void ShowUI () {
		base.ShowUI ();
//		Debug.LogError( "Units Scene: ShowUI" + this);

	}
	
	public override void HideUI () {
		base.HideUI ();
//		Debug.LogError( "Units Scene: HideUI" + this);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void Callback (object data)
	{
		try {
			SceneEnum se = (SceneEnum)data;
			AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
			UIManager.Instance.ChangeScene(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}

}
