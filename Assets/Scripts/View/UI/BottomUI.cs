using UnityEngine;
using System.Collections;

public class BottomUI : UIBaseUnity 
{
	void Start()
	{
		Init("Bottom");
	}
	public override void Init (string name)
	{
		base.Init (name);
	}

	public void TurnToQuest()
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Quest);
	}

	public void TurnToFriends()
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Friends);
	}

	public void TurnToScratch()
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Scratch);
	}

	public void TurnToOthers()
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Others);
	}

	public void TurnToUnits()
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Units);
	}

	public void TurnToShop()
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Shop);
	}

}
