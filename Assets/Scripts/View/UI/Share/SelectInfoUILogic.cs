using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectInfoUILogic : ConcreteComponent {

	public SelectInfoUILogic(string uiName):base(uiName) {}
	string currentState;

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();

	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ShowSelectUnitInfo, ReceiveSelectedUnitInfo);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowSelectUnitInfo, ReceiveSelectedUnitInfo);

        }

        void ReceiveSelectedUnitInfo(object data){
		Debug.Log("SelectInfoUILogic.UpdateUI(), receive command, to show unitInfo...");
		BriefUnitInfo bui = data as BriefUnitInfo;
		switch (bui.tag){
			case "page" :
				currentState = "page";
				break;
			case "unitList" :
				currentState = "unitList";
				break;
			default:
				break;
		}
		ExcuteCallback(bui.data);
	}

	public override void Callback(object data){
		base.Callback(data);
		string call = data as string;
		switch (call){
			case "Choose" : 
				NoticeFuncParty();
				break;
			case "ViewInfo" : 
				NoticeShowUnitDetail();
				break;
			case "Exit" :
				//HideUI();
				break;
			default:
				break;
		}
	}
	
	void NoticeShowUnitDetail(){
		MsgCenter.Instance.Invoke(CommandEnum.ShowSelectUnitDetail, null);
	}

	//notice to activate party function
	void NoticeFuncParty(){
		if(currentState == "page")
			MsgCenter.Instance.Invoke(CommandEnum.NoticeFuncParty, "page");
		if(currentState == "unitList")
			MsgCenter.Instance.Invoke(CommandEnum.OnSubmitChangePartyItem, "unitList");
	}



}
