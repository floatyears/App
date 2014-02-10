using UnityEngine;
using System.Collections;

public class QuestSelectComponent : ConcreteComponent, IUICallback
{
	public QuestSelectComponent(string uiName):base(uiName)
	{
                
	}
	
	public override void CreatUI()
	{
		base.CreatUI();
	}
	
	public override void ShowUI()
	{
		base.ShowUI();

	}
	
	public override void HideUI()
	{
		base.HideUI();
	}
	
	public override void DestoryUI()
	{
		base.DestoryUI();
	}

	public void Callback(object data)
	{
		SceneEnum se = (SceneEnum)data;
		UIManager.Instance.ChangeScene(se);
	}
}
