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
				ModuleManager.SendMessage (ModuleEnum.LoadingModule, "FirstLogin", supportSelectUnits[ curSelectPos ].ID);
				ModuleManager.Instance.DestroyModule(ModuleEnum.SelectRoleModule);
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

	void RecordSelectState(object args){
		curSelectPos = (int)args;
		Debug.Log("receive the click from view, to select unit with the position of " + curSelectPos);
	}

}
