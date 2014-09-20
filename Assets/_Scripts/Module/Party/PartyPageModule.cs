using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageModule : ModuleBase{
	int currentFoucsPosition;

	public PartyPageModule(UIConfigItem config):base(  config){
		CreateUI<PartyPageView> ();
		SetFocusPostion(0);
	}

    public override void InitUI() {
        base.InitUI();
    }

    public override void ShowUI() {
        base.ShowUI();
        RefreshCurrentParty();
    }
	
    public override void HideUI() {
        base.HideUI();
        NoticeServerUpdatePartyInfo();
    }

    public virtual void RefreshCurrentParty(){
        RefreshCurrentPartyInfo("current");
    }

	public override void OnReceiveMessages(params object[] data) {
//		base.OnReceiveMessages(data);	
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (data[0].ToString()) {
			case "TurnPage": 
				RefreshCurrentPartyInfo(data[1]);
				break;
			case "PressItem": 
				ViewPartyMemberUnitDetail(data[1]);
				break;
			default:
				break;
		}
	}
	
	void SetFocusPostion(int position) {
		currentFoucsPosition = position;
	}

    TUnitParty GetPartyBySignal(string signal) {
        TUnitParty currentParty = null;
		switch (signal) {
			case "current": 
				LogHelper.Log("PartyPagePanel.RefreshCurrentPartyInfo(), to current party");
		        currentParty = DataCenter.Instance.PartyInfo.CurrentParty;
		        break;
	        case "prev":
		            LogHelper.Log("PartyPagePanel.RefreshCurrentPartyInfo(), to prev party");
		            currentParty = DataCenter.Instance.PartyInfo.PrevParty;
		            break;
	        case "next":
		            LogHelper.Log("PartyPagePanel.RefreshCurrentPartyInfo(), to next party");
		            currentParty = DataCenter.Instance.PartyInfo.NextParty;
		            break;
	        default:
	            	break;
        }
        return currentParty;
    }

    protected void RefreshCurrentPartyInfo(object args) {
        string partyType = args as string;
		if (DataCenter.Instance.PartyInfo.CurrentParty == null) return; 
        SetFocusPostion(0);
        TUnitParty curParty = GetPartyBySignal(partyType);

//		if (curParty == null)	return;
//        if (curParty.GetUserUnit() == null)	return;
//
//        List<TUserUnit> curUserUnitList = curParty.GetUserUnit();
//        List<Texture2D> curPartyTexList = GetPartyTexture(curUserUnitList);
//
//        int curPartyIndex = GetPartyIndex();
//
//        CallBackDispatcherArgs cbdIndex = new CallBackDispatcherArgs("RefreshPartyIndexView", curPartyIndex);
//        ExcuteCallback(cbdIndex);
//
//        CallBackDispatcherArgs cbdTexture = new CallBackDispatcherArgs("RefreshPartyItemView", curPartyTexList);
//        ExcuteCallback(cbdTexture);
//
//        MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);

		RefreshEvolvePartyInfo (curParty);

		// use in party scene.
        MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyUnitList, null);
    }


	//==add by lei liang start==================================================

	protected void RefreshEvolvePartyInfo (TUnitParty unitParty) {
		if (unitParty == null)	{
			return;
		}
		if (unitParty.GetUserUnit() == null){	
			return;
		}
		
		List<TUserUnit> curUserUnitList = unitParty.GetUserUnit();
//		List<Texture2D> curPartyTexList = GetPartyTexture(curUserUnitList);
		
//		int curPartyIndex = 1;
		int curPartyIndex = GetPartyIndex();
		
//		CallBackDispatcherArgs cbdIndex = new CallBackDispatcherArgs("RefreshPartyIndexView", curPartyIndex);
		view.CallbackView("RefreshPartyIndexView", curPartyIndex);
		
//		CallBackDispatcherArgs cbdTexture = new CallBackDispatcherArgs("RefreshPartyItemView", curPartyTexList);
//		ExcuteCallback(cbdTexture);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, unitParty);
	}

	//==add by lei liang end==================================================
	
    List<Texture2D> GetPartyTexture(List<TUserUnit> tuuList) {
        List<Texture2D> textureList = new List<Texture2D>();
		for (int i = 0; i < tuuList.Count; i++) {
            if (tuuList[i] == null) {
                LogHelper.Log(string.Format("PartyPageUILogic.GetPartyTexture(), Pos[{0}] data is NULL", i));
                textureList.Add(null);
            }
            else {
				ResourceManager.Instance.GetAvatar(UnitAssetType.Avatar,tuuList[i].UnitID, o=>{
//					tuuList[i].UnitInfo
					textureList.Add(o as Texture2D);
				});
                
            }
        }

		for (int i = textureList.Count; i < 4; i++) {
			textureList.Add(null);
		}
//		Debug.LogError (textureList.Count);
        return textureList;
    }

    int GetPartyIndex() {
        return DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
    }
	
    void NoticeServerUpdatePartyInfo() {
//		if(UIManager.Instance.baseScene.CurrentScene != ModuleEnum.UnitDetail)
//        	DataCenter.Instance.PartyInfo.ExitParty();
    }

    void NoticeInfoPanel(TUnitParty tup) {
        MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, tup);
    }

    void ViewPartyMemberUnitDetail(object args) {
        LogHelper.Log("PartyPageLogic.ViewPartyMemberUnitDetail(), Start...");
        int position = (int)args;
        TUserUnit tuu = null;
		
        if (DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit()[position - 1] == null) {
            LogHelper.LogError(string.Format("The position[{0}] of the current don't exist, do nothing!", position - 1));
            return;
        }
        else {
            tuu = DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit()[position - 1];
        }

		ModuleManager.Instance.ShowModule(ModuleEnum.UnitDetailModule,"unit",tuu);
    }
	
}

