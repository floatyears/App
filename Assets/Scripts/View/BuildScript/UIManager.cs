using UnityEngine;
using System.Collections.Generic;

public class UIManager {
	private static UIManager instance;

	public static UIManager Instance {
		get {
			if(instance == null)
				instance = new UIManager();

			return instance;
		}
	}

	private UIManager () {

		baseScene = new StartScene ("Game Start");
	}

	/// <summary>
	/// The base scene. when the game start, instance it.
	/// </summary>
	private StartScene baseScene; 

	/// <summary>
	/// current scene class
	/// </summary>
	private DecoratorBase current;

	private Dictionary<SceneEnum,DecoratorBase> sceneDecorator = new Dictionary<SceneEnum, DecoratorBase>();
	
	/// <summary>
	/// add ui to uimanager
	/// </summary>
	/// <param name="uiName">ui name.</param>
	/// <param name="ui">ui object.</param>
	public void AddUIObject(SceneEnum sEnum, DecoratorBase decorator)
	{
		if(!sceneDecorator.ContainsKey(sEnum))
			sceneDecorator.Add(sEnum,decorator);
		else
			sceneDecorator[sEnum] = decorator;
	}
	
	/// <summary>
	/// have this ui object?
	/// </summary>
	/// <returns><c>true</c> if this instance has user interface object the specified uiName; otherwise, <c>false</c>.</returns>
	/// <param name="uiName">ui name.</param>
	public bool HasUIObject(SceneEnum sEnum)
	{
		if(sceneDecorator.ContainsKey(sEnum))
			return true;
		
		return false;
	}
	
	/// <summary>
	/// get this ui object
	/// </summary>
	/// <returns>ui object.</returns>
	/// <param name="uiName">ui name.</param>
	public DecoratorBase GetUI(SceneEnum sEnum)
	{
		if(sceneDecorator.ContainsKey(sEnum))
			return sceneDecorator[sEnum];
		else
			return null;
	}
	
	/// <summary>
	/// Remove UI
	/// </summary>
	/// <param name="uiName">User interface name.</param>
	public void RemoveUI(SceneEnum sEnum)
	{
		if(sceneDecorator.ContainsKey(sEnum))
		{
			if(current == sceneDecorator[sEnum])
				return;
			
			sceneDecorator[sEnum].DestoryScene();
			sceneDecorator.Remove(sEnum);
		}
	}

	public void ChangeScene(SceneEnum sEnum)
	{
		if (baseScene.CurrentScene == sEnum) {
			return;		
		}
		else {
			if(current != null) {
				current.HideScene();
			}

			baseScene.SetScene(sEnum);
		}

		if(HasUIObject(sEnum))
			current = GetUI(sEnum);
		else
			current = CreatScene(sEnum);

		if (current != null) {
			current.ShowScene();		
		}
	}
	
	DecoratorBase CreatScene(SceneEnum sEnum)
	{

		DecoratorBase temp = null;
		switch (sEnum)
		{	
		case SceneEnum.Start:
			temp = new DecoratorUIScene(sEnum);
			break;
		case SceneEnum.Quest:
			temp = new TestUIQuest(sEnum);
			break;
//		case SceneEnum.Friends:
//			temp = new FriendsView(uiName);
//			break;
//		case SceneEnum.Scratch:
//			temp = new ScratchView(uiName);
//			break;
//		case SceneEnum.Shop:
//			temp = new ShopView(uiName);
//			break;
//		case SceneEnum.Others:
//			temp = new OthersView(uiName);
//			break;
//		case SceneEnum.Units:
//			temp = new UnitView(uiName);
//			break;
//		case SceneEnum.QuestSelect:
//			temp = new QuestSelectView(uiName);
//			break;
//		case SceneEnum.FriendSelect:
//			temp = new FriendSelectView(uiName);
//			break;
//		case SceneEnum.Party:
//			temp = new PartyView(uiName);
//			break;
//		case SceneEnum.LevelUp:
//			temp = new LevelUpView(uiName);
//			break;
//		case SceneEnum.UnitList:
//			temp = new UnitListView(uiName);
//			break;
//		case SceneEnum.Evolve:
//			temp = new EvolveView(uiName);
//			break;
//		case SceneEnum.Sell:
//			temp = new CatalogView(uiName);
//			break;
//		case SceneEnum.UnitCatalog:
//			temp = new CatalogView(uiName);
//			break;
//		case SceneEnum.Fight:
//			temp = new BattleQuest(uiName);
//			break;
//			
//		default:
//			temp = new UIBase("Null");
//			break;
		}
		if (temp != null) {
				temp.SetDecorator (baseScene);
				temp.DecoratorScene ();

				AddUIObject (sEnum, temp);
		}
		return temp;
	}

}
