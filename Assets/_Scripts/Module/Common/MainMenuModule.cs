using UnityEngine;
using System.Collections;

public class MainMenuModule : ModuleBase {
	public MainMenuModule (UIConfigItem config) : base(  config) {
		CreateUI<MainMenuView> ();
	}

	public void OnReceiveMessage (object data){
		try {
			ModuleEnum se = (ModuleEnum)data;
			if(se == ModuleEnum.QuestSelectModule){
				if(CheckUnitCountLimit()){
					//msg box show 
//					MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetUnitExpansionMsgParams());
					TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("UnitOverflow"),
					                                   TextCenter.GetText("UnitOverflowText",DataCenter.Instance.UserUnitList.GetAllMyUnit().Count,DataCenter.Instance.UserInfo.UnitMax),
					                                   TextCenter.GetText("DoUnitExpansion"),CallBackScratchScene);
					return;
				}
			}
			ModuleManager.Instance.ShowModule(se);
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}

	bool CheckUnitCountLimit(){
//		Debug.LogError("Current MyUnitList count is " + DataCenter.Instance.MyUnitList.Count);
//		Debug.LogError("Current MyUnit Max is " + DataCenter.Instance.UserInfo.UnitMax);
		if(DataCenter.Instance.UserUnitList.GetAllMyUnit().Count > DataCenter.Instance.UserInfo.UnitMax){
			Debug.LogError("MyUnitList's count > MyMax!!! Refuse to scene of Quest...");
			return true;
		}
		else
			return false;
	}

//	MsgWindowParams GetUnitExpansionMsgParams(){
//		MsgWindowParams msgParams = new MsgWindowParams();
//		msgParams.titleText = ;
//		msgParams.contentText = ;
//		msgParams.btnParams = new BtnParam[2]{ new BtnParam(), new BtnParam()};
//		msgParams.btnParams[ 0 ].text = ;
//		msgParams.btnParams[ 0 ].callback = ;
//		return msgParams;
//	}

	void CallBackScratchScene(object args){
		ModuleManager.Instance.ShowModule(ModuleEnum.ShopModule);
	}
}
