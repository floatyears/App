using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageUILogic : ConcreteComponent {
	
	private PartyFocusState partyFocusState;
	
	void InitFocusState(){
		partyFocusState = new PartyFocusState();
	}

	void SetFocusState(){
		partyFocusState.ResetState();
	}

	TUserUnit currentFocusUserUnit;
	int currentFocusPos = -1;
	TUserUnit currentChangeUnit;
	bool canSubmit = false;

	public PartyPageUILogic(string uiName):base(uiName) {
		InitFocusState();
	}

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
		AddCmdListener();
		currentFocusUserUnit = null;
		currentFocusPos = -1;
		currentChangeUnit = null;
		canSubmit = false;

		//Reset 
		SetFocusState();

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

		NoticeServerUpdatePartyInfo();
		RemoveCommandListener();
		  
	}

	void NoticeServerUpdatePartyInfo(){
		Debug.Log("PartyPageUILogic.NoticeServerUpdatePartyInfo(), Start...");
//		if(partyFocusState.IsChanged){
//			Debug.Log("PartyPageUILogic.NoticeServerUpdatePartyInfo(), Party Info is changed...");
//			int pos = partyFocusState.Position;
//			uint id = partyFocusState.Data.ID;
//
//			Debug.Log(string.Format("PartyPageUILogic.NoticeServerUpdatePartyInfo(), changed pos is {0}, uniqueId is {1}...", pos, id));
//		}
		GlobalData.partyInfo.ExitParty();
		Debug.Log("PartyPageUILogic.NoticeServerUpdatePartyInfo(), End...");
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
				NoticeInfoPanel(GlobalData.partyInfo.NextParty);
				break;
			case "PageBack" : 
				partyInfo = GlobalData.partyInfo.PrevParty;
				NoticeInfoPanel(GlobalData.partyInfo.PrevParty);
				break;
			case "PageCurrent" :
				partyInfo = GlobalData.partyInfo.CurrentParty;
				NoticeInfoPanel(GlobalData.partyInfo.CurrentParty);
				break;
			case "ClickItem1" :
				ReceiveClickEvent(0);
				break;
			case "ClickItem2" :
				ReceiveClickEvent(1);
				break;
			case "ClickItem3" :
				ReceiveClickEvent(2);
				break;
			case "ClickItem4" :
				ReceiveClickEvent(3);
				break;
			case "CurrentPos" : 
				ExcuteCallback(currentFocusPos);
				break;
			default:
				partyInfo = null;
				break;
		}

		//NoticeInfoPanel(GlobalData.partyInfo.CurrentParty);
		
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

	void ReceiveClickEvent(int pos){
		List<TUserUnit> tuuList = GlobalData.partyInfo.CurrentParty.GetUserUnit();
		currentFocusUserUnit = tuuList[ pos ];
		currentFocusPos = pos + 1;

		switch (UIManager.Instance.baseScene.CurrentScene){
			case SceneEnum.Units : 
				//do nothing
				break;
			case SceneEnum.FriendSelect :
				//do nothing
				break;
			case SceneEnum.Party : 
				BriefUnitInfo briefInfo = new BriefUnitInfo("page", currentFocusUserUnit );
				NoticeShowUnitInfo(briefInfo);
				break;
			default:
				break;
		}
	}

	void NoticeShowUnitInfo(BriefUnitInfo briefinfo){
//		List<TUserUnit> tuuList = GlobalData.partyInfo.CurrentParty.GetUserUnit();
		if(briefinfo.data == null){
			Debug.LogError("PartyPageUILogic.NoticeShowUnitInfo(), UnitInfo is Null, do nothing!");
			return;
		}
		MsgCenter.Instance.Invoke(CommandEnum.ShowSelectUnitInfo, briefinfo);
	}

	void ShowSelectUnitDetail(object data){
		if(currentFocusUserUnit == null)	return;
	
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, currentFocusUserUnit);
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ShowSelectUnitDetail, ShowSelectUnitDetail);
		MsgCenter.Instance.AddListener(CommandEnum.NoticeFuncParty, HighLightShow);
		MsgCenter.Instance.AddListener(CommandEnum.OnPartySelectUnit, OnParty);
		MsgCenter.Instance.AddListener(CommandEnum.OnSubmitChangePartyItem, SubmitChangePartyItem);
//		MsgCenter.Instance.RemoveListener(CommandEnum.GetSubmitChangeState, GetCanSubmit);
	}

	void HighLightShow(object data){
		Dictionary<string,object> viewInfoDic = new Dictionary<string, object>();
		viewInfoDic.Add("LightSprite", currentFocusPos);
		ExcuteCallback(viewInfoDic);
		canSubmit = true;
//		CallBackDeliver cbd = new CallBackDeliver("LightSprite", currentFocusPos);
//		ExcuteCallback(cbd);
	}
	
	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowSelectUnitDetail, ShowSelectUnitDetail);
		MsgCenter.Instance.RemoveListener(CommandEnum.NoticeFuncParty, HighLightShow);
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPartySelectUnit, OnParty);
		MsgCenter.Instance.RemoveListener(CommandEnum.OnSubmitChangePartyItem, SubmitChangePartyItem);
//		MsgCenter.Instance.RemoveListener(CommandEnum.GetSubmitChangeState, GetCanSubmit);
	}

	void SubmitChangePartyItem(object data){
		
	}
	
	void OnParty(object data){
		if(!canSubmit)	return;
		TUserUnit tuu = data as TUserUnit;
		Texture2D t2d = tuu.UnitInfo.GetAsset(UnitAssetType.Avatar);
//		CallBackDeliver cbd = new CallBackDeliver("changeTexture", t2d);
//		ExcuteCallback(cbd);
		Dictionary<string,object> viewInfoDic = new Dictionary<string, object>();
		viewInfoDic.Add("changeTexture", t2d);
		ExcuteCallback(viewInfoDic);

		GlobalData.partyInfo.ChangeParty(currentFocusPos, tuu.ID);

		canSubmit = false;

		//MsgCenter.Instance.Invoke(CommandEnum.ShowSelectUnitDetail,)

	}

}


public class BriefUnitInfo{
	public BriefUnitInfo(string tag, TUserUnit data){
		this.tag = tag;
		this.data = data;
	}
	public string tag;
	public TUserUnit data;
}

