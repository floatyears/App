using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectInfoUILogic : ConcreteComponent {

	public SelectInfoUILogic(string uiName):base(uiName) {}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ShowSelectUnitInfo, UpdateUI);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowSelectUnitInfo, UpdateUI);

        }
        
        void UpdateUI(object data){
		Debug.Log("SelectInfoUILogic.UpdateUI(), receive command, to show unitInfo...");
		TUserUnit tuu = data as TUserUnit;
		ExcuteCallback(tuu);
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
		MsgCenter.Instance.Invoke(CommandEnum.NoticeFuncParty, null);
	}

}
