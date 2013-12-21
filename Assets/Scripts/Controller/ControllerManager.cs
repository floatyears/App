using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerManager
{
	#region singleton
	private static ControllerManager instance;

	/// <summary>
	/// singleton 
	/// </summary>
	/// <value>The instance.</value>
	public static ControllerManager Instance
	{
		get
		{
			if(instance  == null)
				instance = new ControllerManager();

			return instance;
		}
	}
	#endregion

	private IUIInterface currentScene;

	#region UI object manager
	private Dictionary<string,IUIInterface> uiDic = new Dictionary<string, IUIInterface>();

	/// <summary>
	/// add ui to uimanager
	/// </summary>
	/// <param name="uiName">ui name.</param>
	/// <param name="ui">ui object.</param>
	public void AddUIObject(string uiName,IUIInterface ui)
	{
		if(!uiDic.ContainsKey(uiName))
			uiDic.Add(uiName,ui);
		else
			uiDic[uiName] = ui;
	}

	/// <summary>
	/// have this ui object?
	/// </summary>
	/// <returns><c>true</c> if this instance has user interface object the specified uiName; otherwise, <c>false</c>.</returns>
	/// <param name="uiName">ui name.</param>
	public bool HasUIObject(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
			return true;

		return false;
	}

	/// <summary>
	/// get this ui object
	/// </summary>
	/// <returns>ui object.</returns>
	/// <param name="uiName">ui name.</param>
	public IUIInterface GetUI(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
			return uiDic[uiName];
		else
			return null;
	}

	/// <summary>
	/// Remove UI
	/// </summary>
	/// <param name="uiName">User interface name.</param>
	public void RemoveUI(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
		{
			if(currentScene == uiDic[uiName])
				currentScene = null;

			uiDic[uiName].DestoryUI();
			uiDic.Remove(uiName);
		}
	}
	
	#endregion

	public void ChangeScene(SceneEnum sEnum)
	{
		string uiName = sEnum.ToString();

		if(currentScene != null)
		{
			if(currentScene.UIName == uiName)
				return;
			else
				currentScene.HideUI();
		}

		if(HasUIObject(uiName))
			currentScene = GetUI(uiName);
		else
			currentScene = CreatScene(sEnum,uiName);

		currentScene.ShowUI();
		AnimController.UpdateSceneInfo(uiName);
	}

	IUIInterface CreatScene(SceneEnum sEnum,string uiName)
	{
		IUIInterface temp;
		switch (sEnum)
		{	
		case SceneEnum.Quest:
			temp = new QuestView(uiName);
			break;
		case SceneEnum.Friends:
			temp = new FriendsView(uiName);
			break;
		case SceneEnum.Scratch:
			temp = new ScratchView(uiName);
			break;
		case SceneEnum.Shop:
			temp = new ShopView(uiName);
			break;
		case SceneEnum.Others:
			temp = new OthersView(uiName);
			break;
		case SceneEnum.Units:
			temp = new UnitView(uiName);
			break;
		case SceneEnum.QuestSelect:
			temp = new QuestSelectView(uiName);
			break;
		case SceneEnum.FriendSelect:
			temp = new FriendSelectView(uiName);
			break;

		case SceneEnum.Party:
			temp = new PartyView(uiName);
			break;
		case SceneEnum.Fight:
			//temp = new Battle(uiName);
			temp = new BattleQuest(uiName);
			break;
		default:
			temp = new UIBase("Null");
			break;
		}

		temp.CreatUI();

		AddUIObject(uiName,temp);

		return temp;
	}

	#region globl ui
	private string actorName = "ActorShow";

	private ActorShow actor;

	public static void ShowActor(int id)
	{
		instance.currentScene.HideUI ();
		instance.actor = ViewManager.Instance.GetViewObject (instance.actorName) as ActorShow;
		instance.actor.Init (instance.actorName);
		instance.actor.ShowUI ();
		instance.actor.ShowTextureID (id);
	}

	public static void HideActor()
	{
		if (instance.actor != null) {
			instance.actor.HideUI ();
		}

		instance.currentScene.ShowUI ();
	}
	#endregion

}
