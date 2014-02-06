using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendSelectComponent : ConcreteComponent, IUICallback {
	UnitPartyInfo upi;
	Dictionary<int,UserUnitInfo> userUnit = new Dictionary<int, UserUnitInfo> ();

	public FriendSelectComponent(string uiName):base(uiName) {

	}
	
	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();

	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	public void Callback (object data)
	{
		int partyID = 0;
		try {
			partyID = (int)data;
		} 
		catch (System.Exception ex) {
			Debug.LogError(ex.Message);
			return;
		}
		IUICallback call = viewComponent as IUICallback;
		//Debug.LogError( "Comp : " +viewComponent.ToString());
		if (call == null) {
			return;		
		}
		if (partyID == 1) {
			upi = ModelManager.Instance.GetData (ModelEnum.UnitPartyInfo, errMsg) as UnitPartyInfo;
			Dictionary<int,int> temp = upi.GetPartyItem();
			Dictionary<int,UnitBaseInfo> viewInfo = new Dictionary<int, UnitBaseInfo>();
			foreach(var item in temp) {
				UserUnitInfo uui =  GlobalData.tempUserUnitInfo[item.Value];
				if(!userUnit.ContainsKey(item.Key)) {
					userUnit.Add(item.Key,uui);
				}
				UnitBaseInfo ubi = GlobalData.tempUnitBaseInfo[uui.unitBaseInfo];
				viewInfo.Add(item.Key,ubi);
			}
			call.Callback (viewInfo);
		}
		else {
			call.Callback (null);
		}

	}

}
