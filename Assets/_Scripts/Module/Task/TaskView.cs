using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class TaskView : ViewBase {

	DragPanel dragPanel;
	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		FindChild<UILabel> ("Title").text = TextCenter.GetText ("Task_Title");
		FindChild<UILabel> ("OkBtn/Label").text = TextCenter.GetText("OK");

		dragPanel = new DragPanel ("TaskDragPanel","Prefabs/UI/Achieve/AchieveAndTaskItem",typeof(AchieveAndTaskItemView),transform.FindChild ("Content"));

		UIEventListenerCustom.Get (FindChild ("OkBtn")).onClick = OnClickOK;
	}
	
	public override void ShowUI ()
	{
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.TaskDataChange,OnDataChange);

		dragPanel.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetTaskList());
	}
	
	public override void CallbackView (params object[] args)
	{
		base.CallbackView (args);
	}
	
	public override void HideUI ()
	{
		MsgCenter.Instance.AddListener (CommandEnum.TaskDataChange,OnDataChange);
		DataCenter.Instance.TaskAndAchieveData.SendTempAward ();
		base.HideUI ();
	}
	
	public override void DestoryUI ()
	{

		base.DestoryUI ();
	}

	private void OnDataChange(object data){
		dragPanel.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetTaskList());
	}

	private void OnClickOK(GameObject obj){
		ModuleManager.Instance.HideModule (ModuleEnum.TaskModule);
	}
}
