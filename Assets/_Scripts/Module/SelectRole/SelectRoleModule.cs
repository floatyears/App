using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class SelectRoleModule : ModuleBase {

	int curSelectPos = 0;

	List<UnitInfo> supportSelectUnits = new List<UnitInfo>();

	public SelectRoleModule(UIConfigItem config) : base(  config){
		CreateUI<SelectRoleView> ();
		InitSupportSelectData();  
	}

	public override void InitUI(){
		base.InitUI();
    }
	public override void ShowUI(){
		base.ShowUI();
		view.CallbackView("ShowInitialView", supportSelectUnits);
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (data[0].ToString()){
			case "ClickTab" :
				RecordSelectState(data[1]);
				break;
			case "ClickButton" :
				ModuleManager.SendMessage (ModuleEnum.LoadingModule, "FirstLogin", supportSelectUnits[ curSelectPos ].id);
				ModuleManager.Instance.DestroyModule(ModuleEnum.SelectRoleModule);
                break;
        	default:
				break;
		}
	}

	void InitSupportSelectData(){
		//Debug.Log("when create ui, get the support select unit data....");
		supportSelectUnits.Add(DataCenter.Instance.UnitData.GetUnitInfo(1));
		supportSelectUnits.Add(DataCenter.Instance.UnitData.GetUnitInfo(5));
		supportSelectUnits.Add(DataCenter.Instance.UnitData.GetUnitInfo(9));
		//Debug.LogError("support select unit's count is : " + supportSelectUnits.Count);
	}

	void RecordSelectState(object args){
		curSelectPos = (int)args;
		Debug.Log("receive the click from view, to select unit with the position of " + curSelectPos);
	}

}
