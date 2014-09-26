using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuBtns : ViewBase 
{
	void Start()
	{
//		Init("MenuBtns");
	}
	
	
	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);
		
		UIEventListenerCustom.Get (transform.Find ("ImgBtn_Friends").gameObject).onClick = TurnToFriends;
		
		UIEventListenerCustom.Get (transform.Find ("ImgBtn_Quest").gameObject).onClick = TurnToQuest;
		
		UIEventListenerCustom.Get (transform.Find ("ImgBtn_Scratch").gameObject).onClick = TurnToScratch;
		
		UIEventListenerCustom.Get (transform.Find ("ImgBtn_Shop").gameObject).onClick = TurnToShop;
		
		UIEventListenerCustom.Get (transform.Find ("ImgBtn_Others").gameObject).onClick = TurnToOthers;
		
		UIEventListenerCustom.Get (transform.Find ("ImgBtn_Units").gameObject).onClick = TurnToUnits;
	}
	
	public void TurnToQuest(GameObject go)
	{
		//ControllerManager.Instance.ChangeScene(ModuleEnum.Quest);
	}
	
	public void TurnToFriends(GameObject go)
	{
		//ControllerManager.Instance.ChangeScene(ModuleEnum.Friends);
	}
	
	public void TurnToScratch(GameObject go)
	{
		//ControllerManager.Instance.ChangeScene(ModuleEnum.Scratch);
	}
	
	public void TurnToOthers(GameObject go)
	{
		//ControllerManager.Instance.ChangeScene(ModuleEnum.Others);
	}
	
	public void TurnToUnits(GameObject go)
	{
		//ControllerManager.Instance.ChangeScene(ModuleEnum.Units);
	}
	
	public void TurnToShop(GameObject go)
	{
		//ControllerManager.Instance.ChangeScene(ModuleEnum.Shop);
	}
	
}
