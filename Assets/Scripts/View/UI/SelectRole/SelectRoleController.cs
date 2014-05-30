using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectRoleController : ConcreteComponent {

	int curSelectPos = 0;

	List<TUnitInfo> supportSelectUnits = new List<TUnitInfo>();

	public SelectRoleController(string name) : base(name){}

	public override void CreatUI(){
		base.CreatUI();
		InitSupportSelectData();      
    }
	public override void ShowUI(){
		base.ShowUI();
		ShowInitialView();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
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
		Debug.Log("when create ui, get the support select unit data....");
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(1));
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(5));
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(9));
		Debug.Log("support select unit's count is : " + supportSelectUnits.Count);
	}

	void ShowInitialView(){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowInitialView", supportSelectUnits);
		ExcuteCallback(call);
	}

	void RecordSelectState(object args){
		curSelectPos = (int)args;
		Debug.Log("receive the click from view, to select unit with the position of " + curSelectPos);
	}

	void SubmitSelectState(object args){
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetSelectRoleMsgParams());
	}

	MsgWindowParams GetSelectRoleMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("SelectRoleTitle");
		msgParams.contentText = TextCenter.GetText("SelectRoleContent");
		msgParams.btnParams = new BtnParam[2]{ new BtnParam(), new BtnParam()};
		msgParams.btnParams[ 0 ].callback = CallbackMsgBtnOk;
        return msgParams;
	}

	void CallbackMsgBtnOk(object args){
//		if (NoviceGuideStepEntityManager.isInNoviceGuide()) {
//			NoviceGuideStepEntityManager.Instance().StartStep();
//		} else {
			uint unitID = supportSelectUnits[ curSelectPos ].ID;
			MsgCenter.Instance.Invoke(CommandEnum.StartFirstLogin, unitID);
//		}

	}

}
