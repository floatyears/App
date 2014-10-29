using UnityEngine;
using System.Collections;
using bbproto;

public class AchieveView : ViewBase {

	DragPanel dragPnael;

	private string currTab;

	private UILabel tabCountLabel;
	private GameObject tabCount;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		MsgCenter.Instance.AddListener (CommandEnum.AchieveDataChange, OnAchieveDataChange);

		FindChild<UILabel> ("Title").text = TextCenter.GetText ("Achieve_Title");

		dragPnael = new DragPanel ("AchieveDragPanel","Prefabs/UI/Achieve/AchieveAndTaskItem",typeof(AchieveAndTaskItemView),transform.FindChild ("Content"));

		FindChild<UILabel> ("1/Label").text = TextCenter.GetText ("Achieve_Tab1");
		FindChild<UILabel> ("2/Label").text = TextCenter.GetText ("Achieve_Tab2");

		UIEventListenerCustom.Get (FindChild ("OkBtn")).onClick = OnClickOK;

		tabCount = FindChild ("2/Num");
		tabCountLabel = FindChild<UILabel> ("2/Num/Label");


	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.AchieveBonusChange, OnBonusChange);

		if (UIToggle.GetActiveToggle (5).name == "1") {
			dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveNotCompList());
		}else{
			dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveCompList());
		}

		int count = DataCenter.Instance.TaskAndAchieveData.AchieveBonusCount;
		if (count > 0) {
			tabCount.SetActive(true);
			tabCountLabel.text = count.ToString();
		}else{
			tabCount.SetActive(false);
		}

	}

	public override void CallbackView (params object[] args)
	{
		base.CallbackView (args);
	}

	public override void HideUI ()
	{
		MsgCenter.Instance.RemoveListener (CommandEnum.AchieveDataChange, OnAchieveDataChange);
		DataCenter.Instance.TaskAndAchieveData.SendTempAward ();
		base.HideUI ();
	}

	public override void DestoryUI ()
	{

		dragPnael.DestoryUI ();
		base.DestoryUI ();
	}

	private void OnAchieveDataChange(object data){
		if (UIToggle.GetActiveToggle (5).name == "1") {
			dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveNotCompList());
		}else{
			dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveCompList());
		}
	}

	/// <summary>
	/// Changes the tab. used in ui prefab
	/// </summary>
	public void ChangeTab(){
		UIToggle toggle = UIToggle.GetActiveToggle (5);
		
		if (toggle != null) {
			if (toggle.name == "1") {
				dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveNotCompList());
			}else{
				dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveCompList());
			}
		}
	}

	void OnClickOK(GameObject obj){
		ModuleManager.Instance.HideModule (ModuleEnum.AchieveModule);
	}

	void OnBonusChange(object data){
		int count = DataCenter.Instance.TaskAndAchieveData.AchieveBonusCount;
		if (count > 0) {
			tabCount.SetActive(true);
			tabCountLabel.text = count.ToString();
		}else{
			tabCount.SetActive(false);
		}
	}
}
