using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectRoleModule : ModuleBase {

	int curSelectPos = 0;

	List<TUnitInfo> supportSelectUnits = new List<TUnitInfo>();

	public SelectRoleModule(UIConfigItem config) : base(  config){
		CreateUI<SelectRoleView> ();
		InitSupportSelectData();  
	}

	public override void InitUI(){
		base.InitUI();
    }
	public override void ShowUI(){
		base.ShowUI();
		ShowInitialView();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void OnReceiveMessages(object data){
		base.OnReceiveMessages(data);
		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (call.funcName){
			case "ClickTab" :
				CallBackDispatcherHelper.DispatchCallBack(RecordSelectState, call);
				break;
			case "ClickButton" :
				CallBackDispatcherHelper.DispatchCallBack(SubmitSelectState, call);
                break;
                default:
				break;
		}
	}

	void InitSupportSelectData(){
		//Debug.Log("when create ui, get the support select unit data....");
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(1));
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(5));
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(9));
		//Debug.LogError("support select unit's count is : " + supportSelectUnits.Count);
	}

	void ShowInitialView(){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowInitialView", supportSelectUnits);
		view.CallbackView(call);
	}

	void RecordSelectState(object args){
		curSelectPos = (int)args;
		Debug.Log("receive the click from view, to select unit with the position of " + curSelectPos);
	}

	void SubmitSelectState(object args){
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetSelectRoleMsgParams());
		CallbackMsgBtnOk ();
	}

//	MsgWindowParams GetSelectRoleMsgParams(){
//		MsgWindowParams msgParams = new MsgWindowParams();
//		msgParams.titleText = TextCenter.GetText("SelectRoleTitle");
//		msgParams.contentText = TextCenter.GetText("SelectRoleContent");
//		msgParams.btnParams = new BtnParam[2]{ new BtnParam(), new BtnParam()};
//		msgParams.btnParams[ 0 ].callback = CallbackMsgBtnOk;
//        return msgParams;
//	}

	void CallbackMsgBtnOk(){
//		if (NoviceGuideStepEntityManager.isInNoviceGuide()) {
//			NoviceGuideStepEntityManager.Instance().StartStep();
//		} else {
			uint unitID = supportSelectUnits[ curSelectPos ].ID;
			MsgCenter.Instance.Invoke(CommandEnum.StartFirstLogin, unitID);
//		}

	}

}
