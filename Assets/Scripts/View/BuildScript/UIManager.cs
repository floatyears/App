using UnityEngine;
using System.Collections.Generic;

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

	private SceneEnum storePrevScene = SceneEnum.None;

	private Dictionary<SceneEnum,DecoratorBase> sceneDecorator = new Dictionary<SceneEnum, DecoratorBase>();
	
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
			if(current == sceneDecorator[sEnum])
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
//			Debug.LogWarning("storePrevScene : " + storePrevScene);
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

	public void ChangeScene(SceneEnum sEnum) {
		if (forbidChangeScene) {
			return;		
		}
		if (baseScene.CurrentScene == sEnum) {
			return;		
		}
		else {
			nextScene = sEnum;

            InvokeSceneClear(sEnum);

			if(current != null) {
				current.HideScene();
			}

			baseScene.SetScene(sEnum);

			storePrevScene = sEnum;
		}

		if (HasUIObject (sEnum)) {

			current = GetUI(sEnum);	
//			Debug.LogWarning("HasUIObject : " + sEnum + " current : " + current);
			if (current != null) {
				current.ShowScene();
			}
		}
		else{
//			Debug.LogWarning("not has ui object : " + sEnum);
			DecoratorBase db = CreatScene(sEnum);
			current = db;
			return;
		}
	}
	
	DecoratorBase CreatScene(SceneEnum sEnum) {
//		Debug.LogError ("senum : " + sEnum);
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
        }

		if (temp != null) {
			temp.SetDecorator (baseScene);
			temp.DecoratorScene ();

			AddUIObject (sEnum, temp);
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
			nextScene == SceneEnum.UnitDetail)  {
			return;
		}

		DataCenter.gameState = GameState.Normal;
	}

    private void InvokeSceneClear(SceneEnum nextScene){
        if (baseScene.CurrentScene == SceneEnum.FriendSelect && nextScene == SceneEnum.StageSelect){
            MsgCenter.Instance.Invoke(CommandEnum.QuestSelectSaveState);
		} else if (baseScene.CurrentScene == SceneEnum.UnitDetail) {
            if (nextScene == SceneEnum.LevelUp){
//				Debug.LogError ("InvokeSceneClear : " + baseScene.CurrentScene + " nextScene : " + nextScene);
                MsgCenter.Instance.Invoke(CommandEnum.LevelUpSaveState);
			} else if (nextScene == SceneEnum.Sell) {
                MsgCenter.Instance.Invoke(CommandEnum.SellUnitSaveState);
			} else if (nextScene == SceneEnum.Party) {
                MsgCenter.Instance.Invoke(CommandEnum.PartySaveState);
			} else if (nextScene == SceneEnum.Evolve) {
				MsgCenter.Instance.Invoke (CommandEnum.EvolveSaveState);
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
