using UnityEngine;
using System.Collections.Generic;
using System;

public class UIManager {
	private static UIManager instance;

	public bool forbidChangeScene {
				get;
				set;
	}

	public static UIManager Instance {
		get {
			if(instance == null)
				instance = new UIManager();

			return instance;
		}
	}

	private UIManager () {
//		InitUIManager ();
		baseScene = new StartScene ("Game Start");
	}

	public void InitUIManager () {
		baseScene = new StartScene ("Game Start");
		baseScene.CreatUI ();
		baseScene.ShowUI ();
		ChangeScene (SceneEnum.Start);
	}

	/// <summary>
	/// The base scene. when the game start, instance it.
	/// </summary>
	public StartScene baseScene; 

	/// <summary>
	/// current scene class
	/// </summary>
	public DecoratorBase current;

	private DecoratorBase currentPopUp;

	private SceneEnum storePrevScene = SceneEnum.None;

//	public SceneEnum prevScene = SceneEnum.None;

	private Dictionary<SceneEnum,DecoratorBase> sceneDecorator = new Dictionary<SceneEnum, DecoratorBase>();

//	private Dictionary<SceneEnum,DecoratorBase> showedPopUpWindow = new Dictionary<SceneEnum, DecoratorBase>();
	
	/// <summary>
	/// add ui to uimanager
	/// </summary>
	/// <param name="uiName">ui name.</param>
	/// <param name="ui">ui object.</param>
	public void AddUIObject(SceneEnum sEnum, DecoratorBase decorator) {
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
	public bool HasUIObject(SceneEnum sEnum) {
		if(sceneDecorator.ContainsKey(sEnum))
			return true;
		
		return false;
	}
	
	/// <summary>
	/// get this ui object
	/// </summary>
	/// <returns>ui object.</returns>
	/// <param name="uiName">ui name.</param>
	public DecoratorBase GetUI(SceneEnum sEnum) {
		if(sceneDecorator.ContainsKey(sEnum))
			return sceneDecorator[sEnum];
		else
			return null;
	}
	
	/// <summary>
	/// Remove UI
	/// </summary>
	/// <param name="uiName">User interface name.</param>
	public void RemoveUI(SceneEnum sEnum) {
		if(sceneDecorator.ContainsKey(sEnum)) {
			if(current == sceneDecorator[sEnum] || currentPopUp == sceneDecorator[sEnum])
				return;
			sceneDecorator[sEnum].DestoryScene();
			sceneDecorator.Remove(sEnum);
		}
	}

	/// <summary>
	/// Removes the U.
	/// </summary>
	public void RemoveUI() {
		if(sceneDecorator.ContainsKey(storePrevScene)) {
			sceneDecorator.Remove(storePrevScene);
		}
		storePrevScene = SceneEnum.None;
	}

	public void EnterBattle () {
		baseScene.CurrentScene = SceneEnum.Fight;
		ClearAllUIObject ();
		Resources.UnloadUnusedAssets ();
		MsgCenter.Instance.Invoke (CommandEnum.EnterBattle, null);
		ControllerManager.Instance.ChangeScene(SceneEnum.Fight);
	}

	public void ExitBattle () {
		Resources.UnloadUnusedAssets ();
		ChangeScene (SceneEnum.Home);
	}

	public void HideBaseScene () {
		baseScene.HideBase ();
	}

	public void ShowBaseScene () {
		baseScene.ShowBase ();
	}

	public SceneEnum nextScene;

//	public void ShowPopUpWindow(SceneEnum sEnum){
//		ConcreteComponent component = null;
//		switch(sEnum){
//			case SceneEnum.Music:
//			component = CreatComponent<MusicComponent>(UIConfig.otherMusicSettingName);
//				break;
//
//		}
//		component.CreatUI ();
//	}
//
//	private T CreatComponent<T>(string name) where T : ConcreteComponent {
//		T component = ViewManager.Instance.GetComponent (name) as T;
//		if (component == null) {
//			component = Activator.CreateInstance(typeof(T), name) as T;
//		}
//		LogHelper.Log ("component: " + component);
//		return component;
//	}

	public void ChangeScene(SceneEnum sEnum) {
		if (forbidChangeScene) {
			return;		
		}

//		if (!CheckIsPopUpWindow (sEnum, prevScene)) {
//		Debug.Log ("baseScene.CurrentScene : " + baseScene.CurrentScene + " current scene: " + sEnum);
		if (baseScene.CurrentScene == sEnum) {
			return;		
		} else {
			nextScene = sEnum;

			InvokeSceneClear (sEnum);

			if(CheckIsPopUpWindow(sEnum)){
//				Debug.LogError("is pop up window : " + (currentPopUp == null));
				if(currentPopUp != null) {
//					Debug.Log ("currentPopUp != null current hide: " + currentPopUp.CurrentDecoratorScene);
					currentPopUp.HideScene ();
				}
			} else{
//				Debug.LogError("not pop up window");
				if(currentPopUp != null) {
//					Debug.Log ("currentPopUp != null current hide: " + currentPopUp.CurrentDecoratorScene);
					currentPopUp.HideScene ();
					currentPopUp = null;
				}

				if (current != null) {
					if(current.CurrentDecoratorScene == sEnum){
//						Debug.Log ("current != null current hide: " + current.CurrentDecoratorScene);
						baseScene.SetScene (sEnum);
						storePrevScene = sEnum;
						return;
					}
					current.HideScene ();
				}
			}

			baseScene.SetScene (sEnum);
			storePrevScene = sEnum;
		}

		if (HasUIObject (sEnum)) {
//			Debug.Log("HasUIObject : " + sEnum);
			if(CheckIsPopUpWindow(sEnum)){
				currentPopUp = GetUI (sEnum);	
				if (currentPopUp != null) {
					currentPopUp.ShowScene ();
				}
			} else{
				current = GetUI (sEnum);	
				if (current != null) {
					current.ShowScene ();
				}
			}

		} else {
//			Debug.Log("not HasUIObject : " + sEnum);
			DecoratorBase db = CreatScene (sEnum);
			if(CheckIsPopUpWindow(sEnum)){
				currentPopUp = db;
			}else{
				current = db;
			}
		}
		if(current != null)
//		Debug.LogError ("change scene end current : " + current.CurrentDecoratorScene);
		MsgCenter.Instance.Invoke (CommandEnum.ChangeSceneComplete, sEnum);	
	}

	public static bool CheckIsPopUpWindow(SceneEnum sEnum){
//		if (prevScene == SceneEnum.None) {
			if (sEnum == SceneEnum.Music || sEnum == SceneEnum.NickName || sEnum == SceneEnum.OperationNotice || sEnum == SceneEnum.Reward || sEnum == SceneEnum.UnitDetail)
				return true;
			return false;	
//		}else{
//			if(CheckIsPopUpWindow(sEnum) && !CheckIsPopUpWindow(prevScene))
//				return true;
//			return false;
//		}
//		return false;
	}
	
	DecoratorBase CreatScene(SceneEnum sEnum) {
//		Debug.LogError ("CreatScene senum : " + sEnum);
		DecoratorBase temp = null;
		switch (sEnum)
		{

        case SceneEnum.Loading:
            temp = new LoadingDecorator( sEnum );
            break;

		case SceneEnum.Home:
			temp = new HomeDecorator( sEnum );
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
//			Debug.LogError("SceneEnum.Units");
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

		case SceneEnum.StageSelect:
			temp = new StageSelectDecorrator( sEnum );
			break;
			
		case SceneEnum.FriendSelect:
			temp = new FriendSelectDecorator( sEnum );
			break;

		case SceneEnum.FriendList:
			temp = new FriendListDecorator( sEnum );
			break;
			
		case SceneEnum.SearchFriend:
			temp = new FriendSearchDecorator( sEnum );
			break;
			
		case SceneEnum.Information:
			temp = new InformationDecorator( sEnum );
			break;
			
		case SceneEnum.Apply:
			temp = new ApplyDecorator( sEnum );
			break;
			
		case SceneEnum.Reception:
			temp = new ReceptionDecorator( sEnum );
			break;
			
		case SceneEnum.YourID:
			temp = new UserIDDecorator( sEnum );
			break;

		case SceneEnum.UnitDetail:
			temp = new UnitDetailDecorator( sEnum );
			break;

        case SceneEnum.FriendScratch:
            temp = new GachaWindowDecorator( sEnum );
            break;

        case SceneEnum.RareScratch:
            temp = new GachaWindowDecorator( sEnum );
            break;

        case SceneEnum.EventScratch:
            temp = new GachaWindowDecorator( sEnum );
            break;

		case SceneEnum.SelectRole:
			temp = new SelectRoleDecorator( sEnum );
			break;

		case SceneEnum.Result:
			temp = new ResultDecorator( sEnum );
			break;

		case SceneEnum.FightReady:
			temp = new FightReadyDecorator( sEnum );
			break;

		case SceneEnum.QuestSelect:
			temp = new QuestSelectDecorator( sEnum );
			break;

		case SceneEnum.OperationNotice:
			temp = new OperationNoticeDecorator(sEnum);
			break;
		case SceneEnum.Reward:
			temp = new RewardDecorator(sEnum);
			break;
		case SceneEnum.Raider:
			temp = new GameRaiderDecorator(sEnum);
			break;
		case SceneEnum.Currency:
			temp = new GameCurrencyDecorator(sEnum);
			break;
		case SceneEnum.Music:
			temp = new MusicDecorator(sEnum);
			break;
		case SceneEnum.NickName:
			temp = new NicknameDecorator(sEnum);
			break;

		case SceneEnum.ShowCardEffect:
			temp = new ShowNewCardDecorator(sEnum);
			break;

		case SceneEnum.Victory:
			temp = new VictoryDecorator(sEnum);
			break;
        }

		if (temp != null) {
//			Debug.LogError ("CreatScene temp 1" );
			temp.SetDecorator (baseScene);
//			Debug.LogError ("CreatScene temp 2" );
			temp.DecoratorScene ();
//			Debug.LogError ("CreatScene temp 3" );
			AddUIObject (sEnum, temp);
//			Debug.LogError ("CreatScene temp 4" );
		}
		return temp;
	}

	void SwitchGameState(SceneEnum nextScene) {
		if (DataCenter.gameState == GameState.Normal) {
			return;	
		}

		if (nextScene == SceneEnum.QuestSelect ||
			nextScene == SceneEnum.FightReady || 
			nextScene == SceneEnum.StageSelect || 
			nextScene == SceneEnum.Evolve || 
			nextScene == SceneEnum.UnitDetail ||
		    nextScene == SceneEnum.Victory ||
		    nextScene == SceneEnum.ShowCardEffect)  {
			return;
		}

		DataCenter.gameState = GameState.Normal;
	}

    private void InvokeSceneClear(SceneEnum nextScene){
        if (baseScene.CurrentScene == SceneEnum.FriendSelect && nextScene == SceneEnum.StageSelect){
            MsgCenter.Instance.Invoke(CommandEnum.QuestSelectSaveState);
		} else if (baseScene.CurrentScene == SceneEnum.UnitDetail) {
            if (nextScene == SceneEnum.LevelUp){
                MsgCenter.Instance.Invoke(CommandEnum.LevelUpSaveState);
			} else if (nextScene == SceneEnum.Sell) {
                MsgCenter.Instance.Invoke(CommandEnum.SellUnitSaveState);
			} else if (nextScene == SceneEnum.Party) {
                MsgCenter.Instance.Invoke(CommandEnum.PartySaveState);
			} else if (nextScene == SceneEnum.Evolve) {
				MsgCenter.Instance.Invoke (CommandEnum.EvolveSaveState);
			} else if(nextScene == SceneEnum.EventScratch || nextScene == SceneEnum.FriendScratch || nextScene == SceneEnum.RareScratch) {
				MsgCenter.Instance.Invoke(CommandEnum.ShowGachaWindow);
			}
		} else if(baseScene.CurrentScene == SceneEnum.ShowCardEffect) {
			if(nextScene == SceneEnum.EventScratch || nextScene == SceneEnum.FriendScratch || nextScene == SceneEnum.RareScratch) {
				MsgCenter.Instance.Invoke(CommandEnum.ShowGachaWindow);
			}
		} else if (DataCenter.gameState == GameState.Evolve) {
			if(nextScene == SceneEnum.QuestSelect || nextScene == SceneEnum.FightReady || nextScene == SceneEnum.StageSelect) {
				MsgCenter.Instance.Invoke(CommandEnum.EvolveSaveState);
			}
        }
    }

	public void ClearAllUIObject () {
		sceneDecorator.Clear ();
		ViewManager.Instance.CleartComponent ();
	}
}
