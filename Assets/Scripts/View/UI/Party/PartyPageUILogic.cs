using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageUILogic : ConcreteComponent {

	public PartyPageUILogic(string uiName):base(uiName) {}

	public override void CreatUI(){
		base.CreatUI();
	}

	public override void Callback(object data){
		base.Callback(data);
		string call = data as string;
		ExcuteCallback(GetPartyPageData(call));
	}

	public override void ShowUI(){
		base.ShowUI();
		if(GlobalData.partyInfo == null){
			Debug.LogError("GlobalData.partyInfo is NULL");
			return;
		}
		else{
			if(GlobalData.partyInfo.CurrentParty == null ){
				Debug.LogError("GlobalData.partyInfo.CurrentPartyId is NULL");
				return;
			}

		}
		Dictionary<string,object> dic =GetPartyPageData("PageCurrent"); 
		ExcuteCallback(dic);
	}

	public override void HideUI(){
		base.HideUI();
	}

	Dictionary<string,object> GetPartyPageData(string pageType){
		if(GlobalData.partyInfo == null ){
			Debug.LogError("PartyPageUILogic.GetPartyPageData(), GlobalData.partyInfo is NULL");
			return null;
		}
		TUnitParty partyInfo = null;
		switch (pageType){
			case "PageForward" : 
				partyInfo = GlobalData.partyInfo.NextParty;
				//NoticeInfoPanel(GlobalData.partyInfo.NextParty);
				break;
			case "PageBack" : 
				partyInfo = GlobalData.partyInfo.PrevParty;
				//NoticeInfoPanel(GlobalData.partyInfo.PrevParty);
				break;
			case "PageCurrent" :
				partyInfo = GlobalData.partyInfo.CurrentParty;
				//NoticeInfoPanel(GlobalData.partyInfo.CurrentParty);
				break;
			default:
				partyInfo = null;
				break;
		}

		NoticeInfoPanel(GlobalData.partyInfo.CurrentParty);
		
		if(partyInfo == null){
			Debug.LogError("PartyPageUILogic.GetPartyPageData(), partyInfo.currentParty is NULL. " + GlobalData.partyInfo.CurrentPartyId);
			return null;
		}

		int curPartyIndex = partyInfo.ID + 1;

		if(partyInfo.UserUnit == null){
			Debug.LogError("PartyPageUILogic.GetPartyPageData(), partyInfo.UserUnit is NULL");
			return null;
		}
		List<Texture2D> avatarTexList = new List<Texture2D>();
		if(partyInfo.UserUnit == null ){
			Debug.LogError("partyInfo.UserUnit is Null");
			return null;
		}

		List<TUserUnit> tuuList = partyInfo.GetUserUnit();

		//Get PartyItem Avatar Texture
		for( int i = 0; i < 4; i++){
			if(tuuList[ i ] == null){
				Debug.Log( string.Format("PartyPageUILogic.GetPartyPageData(), Pos[{0}] data is NULL", i ) );
				avatarTexList.Add(null);
			} else {
				Texture2D t2d = tuuList[ i ].UnitInfo.GetAsset(UnitAssetType.Avatar);
				Debug.Log( string.Format("PartyPageUILogic.GetPartyPageData(), Pos[{0}] texture name is {1}", i, t2d.name ) );
				avatarTexList.Add(t2d);
			}
		}

		Dictionary<string,object> viewInfo = new Dictionary<string, object>();
		viewInfo.Add("texture", avatarTexList);
		viewInfo.Add("index",curPartyIndex);

		return viewInfo;
	}

	//notice PartyInfoPanel to update data
	void NoticeInfoPanel(TUnitParty tup){
		Debug.Log("PartyPageUILogic.NoticeInfoPanel(), Start...");
		MsgCenter.Instance.Invoke(CommandEnum.UpdatePartyInfoPanel, tup);
		Debug.Log("PartyPageUILogic.NoticeInfoPanel(), End...");
	}

}
