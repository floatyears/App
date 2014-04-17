using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitListLogic : ConcreteComponent {
	public MyUnitListLogic(string uiName):base(uiName){}
	public override void ShowUI(){
		base.ShowUI();
		RefreshItemCounter();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.Instace.GetCurrentText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.MyUnitList.Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}
}
