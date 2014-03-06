using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageUILogic : ConcreteComponent {

	public PartyPageUILogic(string uiName):base(uiName) {}

	public override void Callback(object data){
		base.Callback(data);
		string call = data as string;
		ExcuteCallback(GetPartyPageData(call));
	}

	public override void ShowUI(){
		base.ShowUI();
		Dictionary<string,object> dic =GetPartyPageData("PageCurrent"); 
//		Debug.LogError(dic.Count);
		ExcuteCallback(dic);
	}

	public override void HideUI(){
		base.HideUI();
//		GlobalData.partyInfo.ChangeParty();
	}

	Dictionary<string,object> GetPartyPageData(string pageType){
		if(GlobalData.partyInfo == null ){
			Debug.LogError("PartyPageUILogic.GetPartyPageData(), GlobalData.partyInfo is NULL");
			return null;
		}
		TUnitParty partyInfo = null;
		switch (pageType){
			case "PageForward" : 
				partyInfo = GlobalData.partyInfo.PrevParty;
				break;
			case "PageBack" : 
				partyInfo = GlobalData.partyInfo.NextParty;
				break;
			case "PageCurrent" :
				partyInfo = GlobalData.partyInfo.CurrentParty;
				break;
			default:
				partyInfo = null;
				break;
		}

		if(partyInfo == null){
			Debug.LogError("PartyPageUILogic.GetPartyPageData(), partyInfo.currentParty is NULL. " + GlobalData.partyInfo.CurrentPartyId);
			return null;
		}

		int curPartyIndex = partyInfo.ID + 1;
		
		if(partyInfo.UserUnit == null){
			Debug.LogError("PartyPageUILogic.GetPartyPageData(), partyInfo.UserUnit is NULL");
			return null;
		}

		List<Texture2D> avatarDic = new List<Texture2D>();
		for (int i = 0; i < partyInfo.UserUnit.Count; i++){
			if(partyInfo.UserUnit[ i ] == null){
				avatarDic.Add(null); 
			} else {
				Texture2D t2d = partyInfo.UserUnit[ i ].UnitInfo.GetAsset(UnitAssetType.Avatar);
				avatarDic.Add(t2d);
			}
		}
		
		Dictionary<string,object> viewInfo = new Dictionary<string, object>();
		viewInfo.Add("texture", avatarDic);
		viewInfo.Add("index",curPartyIndex);

		return viewInfo;
	}

}
