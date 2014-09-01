using UnityEngine;
using System.Collections;

public class MenuBtns : ViewBase 
{
	void Start()
	{
//		Init("MenuBtns");
	}
	
	
	public override void Init (UIInsConfig config)
	{
		base.Init (config);
		
		UIEventListener.Get (transform.Find ("ImgBtn_Friends").gameObject).onClick = TurnToFriends;
		
		UIEventListener.Get (transform.Find ("ImgBtn_Quest").gameObject).onClick = TurnToQuest;
		
		UIEventListener.Get (transform.Find ("ImgBtn_Scratch").gameObject).onClick = TurnToScratch;
		
		UIEventListener.Get (transform.Find ("ImgBtn_Shop").gameObject).onClick = TurnToShop;
		
		UIEventListener.Get (transform.Find ("ImgBtn_Others").gameObject).onClick = TurnToOthers;
		
		UIEventListener.Get (transform.Find ("ImgBtn_Units").gameObject).onClick = TurnToUnits;
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
