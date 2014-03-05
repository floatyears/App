using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnitInfoUILogic : ConcreteComponent {

	public SelectUnitInfoUILogic(string uiName):base(uiName) {}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ShowSelectUnitInfo, ShowSelectUnitInfo);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowSelectUnitInfo, ShowSelectUnitInfo);

        }
        
        void ShowSelectUnitInfo(object data){
		ExcuteCallback(true);
	}

}
