using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSelectController : ConcreteComponent {

	int curSelectPos = 0;

	List<TUnitInfo> supportSelectUnits = new List<TUnitInfo>();

	public UnitSelectController(string name) : base(name){}

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

	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (call.funcName){
			case "ClickItem" :
				CallBackDispatcherHelper.DispatchCallBack(RecordSelectState,call);
				break;
			default:
				break;
		}
	}

	void InitSupportSelectData(){
		Debug.Log("when create ui, get the support select unit data....");
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(1));
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(2));
		supportSelectUnits.Add(DataCenter.Instance.GetUnitInfo(3));
		Debug.Log("support select unit's count is : " + supportSelectUnits.Count);
	}

	void ShowInitialView(){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowInitialView", supportSelectUnits);
		ExcuteCallback(call);
	}

	void RecordSelectState(object args){
		int position = (int)args;
		Debug.Log("receive the click from view, to select unit with the position of " + position);
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("UpdateSelectView", supportSelectUnits[ position ]);
//		ExcuteCallback(call);
	}

}
