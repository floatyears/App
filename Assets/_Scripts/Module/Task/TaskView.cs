using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class TaskView : ViewBase {

	DragPanel dragPanel;
	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		FindChild<UILabel> ("Title").text = TextCenter.GetText ("");
		MsgCenter.Instance.AddListener (CommandEnum.TaskDataChange,OnDataChange);

		dragPanel = new DragPanel ("TaskDragPanel","Prefabs/UI/Achieve/AchieveAndTaskItemView",typeof(AchieveAndTaskItemView),transform.FindChild ("Content"));
	}
	
	public override void ShowUI ()
	{
		base.ShowUI ();

		dragPanel.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetTaskList());
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
		MsgCenter.Instance.AddListener (CommandEnum.TaskDataChange,OnDataChange);
		base.DestoryUI ();
	}

	private void OnDataChange(object data){
		dragPanel.SetData<TaskConf> (DataCenter.Instance.TaskAndAchieveData.GetTaskList());
	}
}
