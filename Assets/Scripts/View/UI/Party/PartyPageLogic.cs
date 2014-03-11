using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageLogic : ConcreteComponent{

	private int currentFoucsPosition;

	public PartyPageLogic(string uiName):base(uiName){
		SetFocusPostion(0);
	}

	public override void CreatUI(){
		base.CreatUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCommandListener();
                RefreshCurrentPartyInfo("current");
	}
	
	public override void HideUI(){
		base.HideUI();
		NoticeServerUpdatePartyInfo();
                RemoveCommandListener();
	}

	void SetFocusPostion(int position){
		currentFoucsPosition = position;
	}

	void FocusOnPositionFromView(object args){
		if(UIManager.Instance.baseScene.CurrentScene != SceneEnum.Party){
			//Debug.LogError("PartyPageUILogic.FocusOnPositionFromView(), only party scene recive focus, thus do nothing...");
			return;
		}

		int position = (int)args;
		TUserUnit tuu = null;

		if(GlobalData.partyInfo.CurrentParty.GetUserUnit()[ position - 1 ] == null){
			Debug.LogError(string.Format("The position[{0}] of the current don't exist, do nothing!", position -1));
			return;
		}
		else{
			tuu = GlobalData.partyInfo.CurrentParty.GetUserUnit()[ position - 1 ];
		}
		
		LogHelper.LogError("currentFoucsPosition is : " + currentFoucsPosition);
		SetFocusPostion(position);
		BriefUnitInfo briefInfo = new BriefUnitInfo("PartyItem", tuu);

		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitBriefInfo, briefInfo);
	}
	
	void EnsureFocusOnCurrentPick(object msg){
                CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs( "LightCurSprite", currentFoucsPosition );
		ExcuteCallback( cbdArgs );
	}

	void RejectCurrentFocusPartyMember(object msg){
		LogHelper.Log("PartyPageUILogic.RejectCurrentFocusPartyMember(), Receive message from PartyDragPanel...");
		Debug.LogError ("msg : " + msg);
		//Notice server to update data
		Debug.Log("RejectCurrentFocusPartyMember(), Current id : " + (currentFoucsPosition -1));
		uint focusUnitUniqueId = GlobalData.partyInfo.CurrentParty.GetUserUnit()[ currentFoucsPosition - 1 ].ID;

		//Debug.LogError("PartyPageUILogic.RejectCurrentFocusPartyMember(), ChangeParty Before....");
		GlobalData.partyInfo.ChangeParty(currentFoucsPosition - 1, 0);
		//Debug.LogError("PartyPageUILogic.RejectCurrentFocusPartyMember(), ChangeParty Before....");

		//Notice view to clear
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ClearItem", currentFoucsPosition);
		ExcuteCallback( cbd );
	}
	
	void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ShowFocusUnitDetail, ShowFocusUnitDetail);
		MsgCenter.Instance.AddListener(CommandEnum.ReplacePartyFocusItem, ReplaceFocusPartyItem);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureFocusOnPartyItem, EnsureFocusOnCurrentPick);
		MsgCenter.Instance.AddListener(CommandEnum.RejectPartyPageFocusItem, RejectCurrentFocusPartyMember);
	}

	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFocusUnitDetail, ShowFocusUnitDetail);
		MsgCenter.Instance.RemoveListener(CommandEnum.ReplacePartyFocusItem, ReplaceFocusPartyItem);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureFocusOnPartyItem, EnsureFocusOnCurrentPick);
		MsgCenter.Instance.RemoveListener(CommandEnum.RejectPartyPageFocusItem, RejectCurrentFocusPartyMember); 
        }
        
	TUnitParty GetPartyBySignal(string signal){
		TUnitParty currentParty = null;
		switch (signal){
			case "current" : 
				LogHelper.Log("PartyPagePanel.RefreshCurrentPartyInfo(), to current party");
				currentParty = GlobalData.partyInfo.CurrentParty;
				break;
			case "prev":
				LogHelper.Log("PartyPagePanel.RefreshCurrentPartyInfo(), to prev party");
				currentParty = GlobalData.partyInfo.PrevParty;
				break;
			case "next":
				LogHelper.Log("PartyPagePanel.RefreshCurrentPartyInfo(), to next party");
				currentParty = GlobalData.partyInfo.NextParty;
				break;
			default:
				break;
		}
		return currentParty;
	}

        void RefreshCurrentPartyInfo(object args){
		//LogHelper.Log("PartyPageLogic.RefreshCurrentPartyInfo(),");
		string partyType = args as string;
		if(GlobalData.partyInfo.CurrentParty == null){
			//LogHelper.LogError("GlobalData.partyInfo.CurrentParty == null");
			return;
		}

		SetFocusPostion(0);

		TUnitParty curParty = GetPartyBySignal(partyType);
		if(curParty == null){
			//LogHelper.Log("RefreshCurrentPartyInfo(), curParty is null, return!!!");
		}

		if( curParty.GetUserUnit() == null )	return;
		List<TUserUnit> curUserUnitList = curParty.GetUserUnit();
		List<Texture2D> curPartyTexList = GetPartyTexture( curUserUnitList );
		int curPartyIndex = GetPartyIndex();

                CallBackDispatcherArgs cbdIndex = new CallBackDispatcherArgs("RefreshPartyIndexView", curPartyIndex );
                ExcuteCallback( cbdIndex );
                
                CallBackDispatcherArgs cbdTexture = new CallBackDispatcherArgs("RefreshPartyItemView", curPartyTexList );
                ExcuteCallback( cbdTexture );
	       	         
                MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyUnitList, null);
	}
	
	List<Texture2D> GetPartyTexture(List<TUserUnit> tuuList){
		List<Texture2D> textureList = new List<Texture2D>();

		for( int i = 0; i < 4; i++){
			if(tuuList[ i ] == null){
				LogHelper.Log( string.Format("PartyPageUILogic.GetPartyTexture(), Pos[{0}] data is NULL", i ) );
				textureList.Add( null );
			} 
			else {
				Texture2D t2d = tuuList[ i ].UnitInfo.GetAsset(UnitAssetType.Avatar);
				LogHelper.Log( string.Format("PartyPageUILogic.GetPartyTexture(), Pos[{0}] texture name is {1}", i, t2d.name ) );
				textureList.Add( t2d );
			}
		}
		return textureList;
	}

	int GetPartyIndex(){
		return GlobalData.partyInfo.CurrentPartyId + 1;
	}


	void NoticeServerUpdatePartyInfo(){
		//LogHelper.LogError("PartyPageUILogic.NoticeServerUpdatePartyInfo(), Start...");
		GlobalData.partyInfo.ExitParty();
		//LogHelper.LogError("PartyPageUILogic.NoticeServerUpdatePartyInfo(), End...");
	}

	
	//notice PartyInfoPanel to update data
	void NoticeInfoPanel(TUnitParty tup){
		//LogHelper.Log("PartyPageUILogic.NoticeInfoPanel(), Start...");
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, tup);
		//LogHelper.Log("PartyPageUILogic.NoticeInfoPanel(), End...");
	}
	
	void ShowFocusUnitDetail(object data){
		//Turn to UnitDetai Scene to show
		if(currentFoucsPosition == 0){
			//Debug.LogError("PartyPageUILogic.ShowFocusUnitDetail(), focus is empty, do nothing!");
			return;
		}

		TUserUnit targetUnit = GlobalData.partyInfo.CurrentParty.GetUserUnit()[ currentFoucsPosition - 1 ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, targetUnit);
		//LogHelper.LogError("To unitdetail");
	}

	//refresh and notice server
	void ReplaceFocusPartyItem(object data){
		LogHelper.Log("PartyPageUILogic.ReplaceFocusPartyItem(), Start...");

		TUserUnit newPartyUnit = data as TUserUnit;
		uint uniqueId = newPartyUnit.ID;

		Debug.LogError("PartyPageUILogic.ReplaceFocusPartyItem(), ChangeParty Before....");
//		Debug.lo
		GlobalData.partyInfo.ChangeParty( currentFoucsPosition - 1, uniqueId );
		Debug.LogError("PartyPageUILogic.ReplaceFocusPartyItem(), ChangeParty After....");

		LogHelper.LogError("PartyPageLogic.ReplaceFocusPartyItem(), The position to  repace : " + currentFoucsPosition );

		Dictionary<string,object> replaceArgsDic = new Dictionary<string, object>();
		replaceArgsDic.Add("position", currentFoucsPosition);
		replaceArgsDic.Add("unit", newPartyUnit);

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ReplaceItemView", replaceArgsDic);
		ExcuteCallback( cbdArgs );

		LogHelper.Log("PartyPageUILogic.ReplaceFocusPartyItem(), End...");

	}

	void ViewPartyMemberUnitDetail(object args){
		LogHelper.Log("PartyPageLogic.ViewPartyMemberUnitDetail(), Start...");
		int position = (int)args;
		TUserUnit tuu = null;
		
		if(GlobalData.partyInfo.CurrentParty.GetUserUnit()[ position - 1 ] == null){
			LogHelper.LogError(string.Format("The position[{0}] of the current don't exist, do nothing!", position -1));
			return;
		}
		else{
			tuu = GlobalData.partyInfo.CurrentParty.GetUserUnit()[ position - 1 ];
		}

		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, tuu);
	}

	public override void Callback(object data){
		base.Callback(data);
				
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (cbdArgs.funcName){
			case "TurnPage" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshCurrentPartyInfo, cbdArgs);
				break;
			case "ClickItem" : 
				CallBackDispatcherHelper.DispatchCallBack(FocusOnPositionFromView, cbdArgs);
                        	break;
			case "PressItem" : 
				CallBackDispatcherHelper.DispatchCallBack(ViewPartyMemberUnitDetail, cbdArgs);
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

