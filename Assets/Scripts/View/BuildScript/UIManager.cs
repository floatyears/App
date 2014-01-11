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
		case SceneEnum.Quest:
			temp = new QuestDecorator( sEnum );
			break;

		case SceneEnum.Friends:
			temp = new FriendDecorator( sEnum );
			break;

		case SceneEnum.Scratch:
			temp = new ScratchDecorator( sEnum );
			break;

		case SceneEnum.Shop:
			temp = new ShopDecorator( sEnum );
			break;

		case SceneEnum.Others:
			temp = new OthersDecorator( sEnum );
			break;

		case SceneEnum.Units:
			temp = new UnitsDecorator( sEnum );
			break;

		case SceneEnum.Party:
			temp = new PartyDecorator( sEnum );
			break;

		case SceneEnum.Sell:
			temp = new SellDecorator( sEnum );
			break;

		case SceneEnum.Evolve:
			temp = new EvolveDecorator( sEnum );
			break;

		case SceneEnum.UnitList:
			temp = new UnitListDecorator( sEnum );
			break;

		case SceneEnum.LevelUp:
			temp = new LevelUpDecorator( sEnum );
			break;

		case SceneEnum.UnitCatalog:
			temp = new CatalogDecorator( sEnum );
			break;
		}
		if (temp != null) {
				temp.SetDecorator (baseScene);
				temp.DecoratorScene ();

				AddUIObject (sEnum, temp);
		}
		return temp;
	}

}
