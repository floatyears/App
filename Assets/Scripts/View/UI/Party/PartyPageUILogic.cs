using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageUILogic : ConcreteComponent {

	private enum FocusState{
		empty = 0,
		first = 1,
		second = 2,
		third = 3,
		fouth = 4
	}

	private FocusState curFocusState;
		
	void SetFocusState( FocusState state){
		curFocusState = state;
	}

	TUserUnit currentFocusUserUnit;

	TUserUnit currentChangeUnit;
	bool canSubmit = false;

	public PartyPageUILogic(string uiName):base(uiName) {
		curFocusState = FocusState.empty;
	}

	public override void CreatUI(){
		base.CreatUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		currentFocusUserUnit = null;
				
                currentChangeUnit = null;
                canSubmit = false;
                
                RefreshCurrentPartyInfo("current");
	}
	
	public override void HideUI(){
		base.HideUI();
		
		NoticeServerUpdatePartyInfo();
                RemoveCommandListener();
	}
	
	void RspFocusRequestFromView( int position ){
		if(UIManager.Instance.baseScene.CurrentScene != SceneEnum.Party){
			Debug.LogError("PartyPageUILogic.RspFocusRequestFromView(), current scene do not recive click...");
			return;
		}

		switch (position){
			case 1 :
				break;
				SetFocusState( FocusState.first );
			case 2 :
				SetFocusState( FocusState.second );
				break;
			case 3 :
				SetFocusState( FocusState.third );
				break;
			case 4 : 
				SetFocusState( FocusState.fouth );
				break;
			default:
				break;
		}
				
		int focusIndex = (int)curFocusState;
		TUserUnit curUnitInfo = GlobalData.partyInfo.CurrentParty.GetUserUnit()[ focusIndex - 1 ];
		BriefUnitInfo briefInfo = new BriefUnitInfo("PartyItem", curUnitInfo);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitBriefInfo, briefInfo);
	}
	
	void EnsureFocusOnCurrentPick(object msg){

		int curPos = (int)curFocusState;
                CallBackDeliver cbd = new CallBackDeliver( "LightCurSprite", curPos );
		ExcuteCallback( cbd );

	}
	
	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ShowFocusUnitDetail, ShowFocusUnitDetail);
		MsgCenter.Instance.AddListener(CommandEnum.OnSubmitChangePartyItem, ReplaceFocusPartyItem);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureFocusOnPartyItem, EnsureFocusOnCurrentPick);
		MsgCenter.Instance.AddListener(CommandEnum.ReplacePartyFocusItem, ReplaceFocusPartyItem);
	}

	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFocusUnitDetail, ShowFocusUnitDetail);
		MsgCenter.Instance.RemoveListener(CommandEnum.OnSubmitChangePartyItem, ReplaceFocusPartyItem);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureFocusOnPartyItem, EnsureFocusOnCurrentPick);
                MsgCenter.Instance.RemoveListener(CommandEnum.ReplacePartyFocusItem, ReplaceFocusPartyItem);
                
        }
        
        void RefreshCurrentPartyInfo(string partyType){
		if(GlobalData.partyInfo.CurrentParty == null){
			return;
		}
		TUnitParty curParty = null;

		if(partyType == "prev"){
			Debug.Log("PartyPagePanel.RefreshCurrentPartyInfo(), to prev party");
			curParty = GlobalData.partyInfo.PrevParty;
		} else if(partyType == "next"){
			Debug.Log("PartyPagePanel.RefreshCurrentPartyInfo(), to next party");
			curParty = GlobalData.partyInfo.NextParty;
		} else{
			Debug.Log("PartyPagePanel.RefreshCurrentPartyInfo(), refresh current party");
			curParty = GlobalData.partyInfo.CurrentParty;
		}

		if( curParty.GetUserUnit() == null ){
			return;
		}
		
		List<TUserUnit> curUserUnitList = curParty.GetUserUnit();
		
		List<Texture2D> curPartyTexList = GetPartyTexture( curUserUnitList );
		int curPartyIndex = GetPartyIndex();

                CallBackDeliver cbd_index = new CallBackDeliver("RefreshPartyIndex", curPartyIndex );
                ExcuteCallback( cbd_index );
                
                CallBackDeliver cbd_texture = new CallBackDeliver("RefreshPartyTexture", curPartyTexList );
                
                ExcuteCallback( cbd_texture );

		MsgCenter.Instance.Invoke(CommandEnum.UpdatePartyInfoPanel, curParty);
	}


	List<Texture2D> GetPartyTexture(List<TUserUnit> tuuList){
		List<Texture2D> textureList = new List<Texture2D>();

		for( int i = 0; i < 4; i++){
			if(tuuList[ i ] == null){
				Debug.Log( string.Format("PartyPageUILogic.GetPartyTexture(), Pos[{0}] data is NULL", i ) );
				textureList.Add( null );
			} else {
				Texture2D t2d = tuuList[ i ].UnitInfo.GetAsset(UnitAssetType.Avatar);
				Debug.Log( string.Format("PartyPageUILogic.GetPartyTexture(), Pos[{0}] texture name is {1}", i, t2d.name ) );
				textureList.Add( t2d );
			}
		}

		return textureList;
	}

	int GetPartyIndex(){
		return GlobalData.partyInfo.CurrentPartyId + 1;
	}


	void NoticeServerUpdatePartyInfo(){
		Debug.Log("PartyPageUILogic.NoticeServerUpdatePartyInfo(), Start...");
		GlobalData.partyInfo.ExitParty();
		Debug.Log("PartyPageUILogic.NoticeServerUpdatePartyInfo(), End...");
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
//		currentFocusPos = pos + 1;

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
		//MsgCenter.Instance.Invoke(CommandEnum.ShowUnitBriefInfo, briefinfo);
	}

	void ShowFocusUnitDetail(object data){
		//Turn to UnitDetai Scene to show
		if(curFocusState == FocusState.empty){
			Debug.LogError("PartyPageUILogic.ShowFocusUnitDetail(), focus is empty, do nothing!");
			return;
		}

		int pos = (int)curFocusState - 1;
		TUserUnit targetUnit = GlobalData.partyInfo.CurrentParty.GetUserUnit()[ pos ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, targetUnit);
		Debug.LogError("To unitdetail");
	}

	//refresh and notice server
	void ReplaceFocusPartyItem(object data){

		Debug.Log("PartyPageUILogic.ReplaceFocusPartyItem(), Start...");
		TUserUnit newPartyUnit = data as TUserUnit;

		int pos = (int)curFocusState;
		uint uniqueId = newPartyUnit.UnitID;

		GlobalData.partyInfo.ChangeParty( pos, uniqueId );

		string callName = "Replace" + pos.ToString();
		Debug.LogError("Repace, Pos : " + pos );

		CallBackDeliver cbd = new CallBackDeliver(callName, newPartyUnit);
		ExcuteCallback( cbd );

		Debug.Log("PartyPageUILogic.ReplaceFocusPartyItem(), End...");

	}

	public override void Callback(object data){
		base.Callback(data);
				
		CallBackDeliver cbd = data as CallBackDeliver;
		
		switch (cbd.callBackName){
			case "PageBack" : 
				RefreshCurrentPartyInfo("prev");
				break;
			case "PageForward" : 
                        	RefreshCurrentPartyInfo("next");
                        	break;
			case "ClickItem" : 
				int pos = (int)cbd.callBackContent;
				RspFocusRequestFromView(pos);
                        	break;
                	default:
                        	break;
                }
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

