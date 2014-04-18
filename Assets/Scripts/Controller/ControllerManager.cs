using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerManager
{
	#region singleton
	private static ControllerManager instance;
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
	private IUIInterface prevScene;
	
	public void BackToPrevScene()
	{
		ChangeScene(prevScene.GetScene);
	}

	#region UI object manager
	private Dictionary<string,IUIInterface> uiDic = new Dictionary<string, IUIInterface>();

	/// <summary>
	/// add ui to uimanager
	/// </summary>
	/// <param name="uiName">ui name.</param>
	/// <param name="ui">ui object.</param>
//	public void AddUIObject(string uiName,IUIInterface ui)
//	{
//		if(!uiDic.ContainsKey(uiName))
//			uiDic.Add(uiName,ui);
//		else
//			uiDic[uiName] = ui;
//	}

	/// <summary>
	/// have this ui object?
	/// </summary>
	/// <returns><c>true</c> if this instance has user interface object the specified uiName; otherwise, <c>false</c>.</returns>
	/// <param name="uiName">ui name.</param>
//	public bool HasUIObject(string uiName)
//	{
//		if(uiDic.ContainsKey(uiName))
//			return true;
//
//		return false;
//	}

	/// <summary>
	/// get this ui object
	/// </summary>
	/// <returns>ui object.</returns>
	/// <param name="uiName">ui name.</param>
//	public IUIInterface GetUI(string uiName)
//	{
//		if(uiDic.ContainsKey(uiName))
//			return uiDic[uiName];
//		else
//			return null;
//	}

	/// <summary>
	/// Remove UI
	/// </summary>
	/// <param name="uiName">User interface name.</param>
//	public void RemoveUI(string uiName)
//	{
//		if(uiDic.ContainsKey(uiName))
//		{
//			if(currentScene == uiDic[uiName])
//				currentScene = null;
//
//			uiDic[uiName].DestoryUI();
//			uiDic.Remove(uiName);
//		}
//	}
	
	#endregion

	public void ExitBattle () {
		currentEnum = SceneEnum.None;
		currentScene.HideUI ();
		currentScene.DestoryUI();
		Main.Instance.GInput.IsCheckInput = false;
		ViewManager.Instance.TrapLabel.text = "";
		Resources.UnloadUnusedAssets ();
		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_home);
		MsgCenter.Instance.Invoke (CommandEnum.LeftBattle, null);
		UIManager.Instance.InitUIManager ();
	}

	private SceneEnum currentEnum = SceneEnum.None;
	public void ChangeScene(SceneEnum sEnum) {
		if (currentEnum == sEnum) {
			return;	
		}
		currentEnum = sEnum;
		string uiName = sEnum.ToString();
		currentScene = CreatScene(sEnum,uiName);
		currentScene.ShowUI();
		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_dungeon);
	}

	IUIInterface CreatScene(SceneEnum sEnum,string uiName) {
		IUIInterface temp;
		switch (sEnum) {	
		case SceneEnum.Fight:
			temp = new BattleQuest(uiName);
			break;
		default:
			temp = new UIBase("Null");
			break;
		}
		temp.GetScene=sEnum;
		temp.CreatUI();
		return temp;
	}

	void EnterBattle () {
		Application.LoadLevel("Battle");
		GameObject ui = GameObject.Find("UI");
		ViewManager.Instance.BattleBottom = ui.transform.Find ("Anchor/Panel/Bottom").gameObject;
		ViewManager.Instance.BattleCamera = ui.transform.Find ("Camera").GetComponent<UICamera> ();
	}
	
	#region global ui
	private string actorName = "ActorShow";
	private ActorShow actor;

	public void ShowActor(int id) {
		currentScene.HideUI ();
		actor = ViewManager.Instance.GetViewObject (actorName) as ActorShow;
		actor.Init (actorName);
		actor.ShowUI ();
		actor.ShowTextureID (id);
	}

	public void HideActor() {
		if (actor != null) {
			actor.HideUI ();
		}
		currentScene.ShowUI ();
	}
	#endregion

}
