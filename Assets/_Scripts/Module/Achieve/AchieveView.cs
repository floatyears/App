using UnityEngine;
using System.Collections;
using bbproto;

public class AchieveView : ViewBase {

	DragPanel dragPnael;

	private string currTab;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		MsgCenter.Instance.AddListener (CommandEnum.AchieveDataChange, OnAchieveDataChange);

		FindChild<UILabel> ("Title").text = TextCenter.GetText ("");

		dragPnael = new DragPanel ("AchieveDragPanel","Prefabs/UI/Achieve/AchieveAndTaskItemView",typeof(AchieveAndTaskItemView),transform.FindChild ("Content"));
	}

	public override void ShowUI ()
	{
		base.ShowUI ();

		if (UIToggle.GetActiveToggle (5).name == "2") {
			dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveCompList());
		}else{
			dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveNotCompList());
		}

	}

	public override void CallbackView (params object[] args)
	{
		base.CallbackView (args);
	}

	public override void HideUI ()
	{
		base.HideUI ();
	}

	public override void DestoryUI ()
	{
		MsgCenter.Instance.RemoveListener (CommandEnum.AchieveDataChange, OnAchieveDataChange);
		dragPnael.DestoryUI ();
		base.DestoryUI ();
	}

	private void OnAchieveDataChange(object data){
		if (UIToggle.GetActiveToggle (5).name == "1") {
			dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveCompList());
		}else{
			dragPnael.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetAchieveNotCompList());
		}
	}

	/// <summary>
	/// Changes the tab. used in ui prefab
	/// </summary>
	public void ChangeTab(){
		UIToggle toggle = UIToggle.GetActiveToggle (5);
		
		if (toggle != null) {
			if(UIToggle.GetActiveToggle (4).name == "1"){
				TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FunctionNotOpenTitle"),TextCenter.GetText("FunctionNotOpenContent"),TextCenter.GetText("OK"));
				transform.FindChild("1").SendMessage("OnClick");
				return;
			}
			if (currTab != UIToggle.GetActiveToggle (4).name) {
				currTab = UIToggle.GetActiveToggle (4).name;
			}
		}
	}
}
