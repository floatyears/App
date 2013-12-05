using UnityEngine;
using System.Collections;

public class QuestSelect : UIBase
{
	#region IUIInterface implementation

	public QuestSelect(string uiName):base(uiName)
	{

	}

	public override void CreatUI ()
	{
		GameObject sourceObject = Resources.Load("Prefabs/QuestSelect") as GameObject;

		insUIObject = NGUITools.AddChild(ViewManager.Instance.ParentPanel,sourceObject);
	}

	public override void ShowUI ()
	{

	}

	public override void HideUI ()
	{

	}

	public override void DestoryUI ()
	{

	}

	#endregion

}
