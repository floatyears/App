using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleShow : ModuleBase 
{
	#region IUIInterface implementation

	private ScrollView topUI;

	private ScrollView downUI;

//	private	Dictionary<string, ViewBase> currentUIDic = new Dictionary<string, ViewBase> ();

	public BattleShow(UIConfigItem config):base(  config)
	{

	}

	public override void ShowUI ()
	{
		SetActive(true);
	}

	public override void HideUI ()
	{
		SetActive(false);
	}

	public override void DestoryUI ()
	{

	}

	void SetActive(bool b)
	{
//		insUIObject.SetActive(b);
//		foreach(var item in currentUIDic.Values)
//			item.HideUI();
		topUI.insUIObject.SetActive(b);
		downUI.insUIObject.SetActive(b);
	}

	#endregion

	void ClickQuest(GameObject go)
	{
		ModuleManger.Instance.ShowModule(ModuleEnum.StageSelectModule);
	}

}
