using UnityEngine;
using System.Collections;

public class BattleFailView : ViewBase {


	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		UIEventListenerCustom.Get (FindChild ("Button")).onClick = ClickOK;
		UIEventListenerCustom.Get (FindChild ("Button_Attr")).onClick = ClickAttr;
		UIEventListenerCustom.Get (FindChild ("Button_Summon")).onClick = ClickSummon;
		FindChild<UILabel> ("Button/Label").text = TextCenter.GetText ("OK");
		FindChild<UILabel> ("Button_Attr/Label").text = TextCenter.GetText ("Btn_Attr");
		FindChild<UILabel> ("Button_Summon/Label").text = TextCenter.GetText ("Btn_Summon");
		FindChild<UILabel> ("Label_Desc").text = TextCenter.GetText ("BattleFail_Desc");
	}

	void ClickOK(GameObject obj){
		ModuleManager.Instance.HideModule (ModuleEnum.BattleFailModule);
		ModuleManager.Instance.EnterMainScene();
	}

	void ClickAttr(GameObject obj){
		ModuleManager.Instance.HideModule (ModuleEnum.BattleFailModule);
		ModuleManager.Instance.EnterMainScene();
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitsListModule);
	}

	void ClickSummon(GameObject obj){
		ModuleManager.Instance.HideModule (ModuleEnum.BattleFailModule);
		ModuleManager.Instance.EnterMainScene();
		ModuleManager.Instance.ShowModule (ModuleEnum.ScratchModule);
	}
}
