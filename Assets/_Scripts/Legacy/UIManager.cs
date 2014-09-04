//using UnityEngine;
//using System.Collections.Generic;
//using System;
//
//public class UIManager {
//	private static UIManager instance;
//
//	public bool forbidChangeScene {
//				get;
//				set;
//	}
//
//	public static UIManager Instance {
//		get {
//			if(instance == null)
//				instance = new UIManager();
//
//			return instance;
//		}
//	}
//
//	private UIManager () {
////		InitUIManager ();
//		camera = Main.Instance.NguiCamera;
////		baseScene = new StartScene ("Game Start");
//	}
//
//	public void InitUIManager () {
////		baseScene = new StartScene ("Game Start");
////		baseScene.CreatUI ();
////		baseScene.ShowUI ();
////		ChangeScene (ModuleEnum.StartModule);
//	}
//
//	/// <summary>
//	/// The base scene. when the game start, instance it.
//	/// </summary>
////	public StartScene baseScene; 
//
//	/// <summary>
//	/// current scene class
//	/// </summary>
//	public SceneBase current;
//
//	private SceneBase currentPopUp;
//
//	private ModuleEnum storePrevScene = ModuleEnum.None;
//
//	private Dictionary<ModuleEnum,SceneBase> sceneDecorator = new Dictionary<ModuleEnum, SceneBase>();
//
//	/// <summary>
//	/// add ui to uimanager
//	/// </summary>
//	/// <param name="uiName">ui name.</param>
//	/// <param name="ui">ui object.</param>
//	public void AddUIObject(ModuleEnum sEnum, SceneBase decorator) {
//		if(!sceneDecorator.ContainsKey(sEnum))
//			sceneDecorator.Add(sEnum,decorator);
//		else
//			sceneDecorator[sEnum] = decorator;
//	}
//	
//	/// <summary>
//	/// have this ui object?
//	/// </summary>
//	/// <returns><c>true</c> if this instance has user interface object the specified uiName; otherwise, <c>false</c>.</returns>
//	/// <param name="uiName">ui name.</param>
//	public bool HasUIObject(ModuleEnum sEnum) {
//		if(sceneDecorator.ContainsKey(sEnum))
//			return true;
//		
//		return false;
//	}
//	
//	/// <summary>
//	/// get this ui object
//	/// </summary>
//	/// <returns>ui object.</returns>
//	/// <param name="uiName">ui name.</param>
//	public SceneBase GetUI(ModuleEnum sEnum) {
//		if(sceneDecorator.ContainsKey(sEnum))
//			return sceneDecorator[sEnum];
//		else
//			return null;
//	}
//	
//	/// <summary>
//	/// Remove UI
//	/// </summary>
//	/// <param name="uiName">User interface name.</param>
//	public void RemoveUI(ModuleEnum sEnum) {
//		if(sceneDecorator.ContainsKey(sEnum)) {
//			if(current == sceneDecorator[sEnum] || currentPopUp == sceneDecorator[sEnum])
//				return;
//			sceneDecorator[sEnum].DestoryScene();
//			sceneDecorator.Remove(sEnum);
//		}
//	}
//
//	/// <summary>
//	/// Removes the U.
//	/// </summary>
//	public void RemoveUI() {
//		if(sceneDecorator.ContainsKey(storePrevScene)) {
//			sceneDecorator.Remove(storePrevScene);
//		}
//		storePrevScene = ModuleEnum.None;
//	}
//
//	public void EnterBattle () {
////		baseScene.CurrentScene = ModuleEnum.Fight;
//		ClearAllUIObject ();
//		Resources.UnloadUnusedAssets ();
//		MsgCenter.Instance.Invoke (CommandEnum.EnterBattle, null);
//		ModuleManger.Instance.ShowModule(ModuleEnum.FightModule);
//	}
//
//	public void ExitBattle () {
//		Resources.UnloadUnusedAssets ();
//		DGTools.ChangeToQuest();
//	}
//
////	public void HideBaseScene () {
//////		baseScene.HideBase ();
////	}
////
////	public void ShowBaseScene () {
//////		baseScene.ShowBase ();
////	}
//
//	public ModuleEnum nextScene;	
//	private UICamera camera;
//
//	public void ChangeScene(ModuleEnum sEnum) {
//		camera.enabled = false;
//
//		if (forbidChangeScene) {
//			camera.enabled = true;
//			return;		
//		}
//
//		SwitchGameState (sEnum);
//
////		if (baseScene.CurrentScene == sEnum) {
////			camera.enabled = true;
////			return;
////		} else {
//			nextScene = sEnum;
//			InvokeSceneClear (sEnum);
//			if(CheckIsPopUpWindow(sEnum)) {
//				if(currentPopUp != null) {
//					currentPopUp.HideScene ();
//				}
//			} else{
//				if(currentPopUp != null) {
//					currentPopUp.HideScene ();
//					currentPopUp = null;
//				}
//
////				if (current != null) {
////					if(current.CurrentDecoratorScene == sEnum){
//////						baseScene.SetScene (sEnum);
////						storePrevScene = sEnum;
////						MsgCenter.Instance.Invoke (CommandEnum.ChangeSceneComplete, sEnum);	
////
////						camera.enabled = true;
////						return;
////					}
////					current.HideScene ();
////				}
//			}
//
////			baseScene.SetScene (sEnum);
//			storePrevScene = sEnum;
////		}
//
//		if (HasUIObject (sEnum)) {
//			if(CheckIsPopUpWindow(sEnum)){
//				currentPopUp = GetUI (sEnum);	
//				if (currentPopUp != null) {
//					currentPopUp.ShowScene ();
//				}
//			} else {
//				current = GetUI (sEnum);	
//				if (current != null) {
//					current.ShowScene ();
//				}
//			}
//		} else {
//			SceneBase db = CreatScene (sEnum);
//			if(CheckIsPopUpWindow(sEnum)){
//				currentPopUp = db;
//			}else{
//				current = db;
//			}
//		}
//
//		MsgCenter.Instance.Invoke (CommandEnum.ChangeSceneComplete, sEnum);	
//
//		camera.enabled = true;
//	}
//
//	public void GoBackToPrevScene(){
//
//	}
//
//	public static bool CheckIsPopUpWindow(ModuleEnum sEnum){
////		if (prevScene == ModuleEnum.None) {
//		if (sEnum == ModuleEnum.MusicModule || sEnum == ModuleEnum.NicknameModule || sEnum == ModuleEnum.OperationNoticeModule || sEnum == ModuleEnum.RewardModule || sEnum == ModuleEnum.UnitDetailModule || sEnum == ModuleEnum.ResourceDownloadModule)
//				return true;
//			return false;	
////		}else{
////			if(CheckIsPopUpWindow(sEnum) && !CheckIsPopUpWindow(prevScene))
////				return true;
////			return false;
////		}
////		return false;
//	}
//	
//	SceneBase CreatScene(ModuleEnum sEnum) {
////		Debug.LogError ("CreatScene senum : " + sEnum);
//		SceneBase temp = null;
//		switch (sEnum)
//		{
//
//        case ModuleEnum.LoadingModule:
//            temp = new LoadingDecorator( sEnum );
//            break;
//
//		case ModuleEnum.HomeModule:
//			temp = new HomeScene( sEnum );
//			break;
//
//		case ModuleEnum.FriendsModule:
//			temp = new FriendScene( sEnum );
//			break;
//
//		case ModuleEnum.ScratchModule:
//			temp = new ScratchScene( sEnum );
//			break;
//
//		case ModuleEnum.ShopModule:
//			temp = new ShopScene( sEnum );
//			break;
//
//		case ModuleEnum.OthersModule:
//			temp = new OthersScene( sEnum );
//			break;
//
//		case ModuleEnum.UnitsModule:
////			Debug.LogError("ModuleEnum.Units");
//			temp = new UnitsScene( sEnum );
//			break;
//
//		case ModuleEnum.PartyModule:
//			temp = new PartyScene( sEnum );
//			break;
//
//		case ModuleEnum.SellModule:
//			temp = new SellScene( sEnum );
//			break;
//
//		case ModuleEnum.EvolveModule:
//			temp = new EvolveScene( sEnum );
//			break;
//
//		case ModuleEnum.UnitListModule:
//			temp = new UnitListScene( sEnum );
//			break;
//
//		case ModuleEnum.LevelUpModule:
//			temp = new LevelUpScene( sEnum );
//			break;
//
//		case ModuleEnum.UnitCatalogModule:
//			temp = new CatalogScene( sEnum );
//			break;
//
//		case ModuleEnum.StageSelectModule:
//			temp = new StageSelectScene( sEnum );
//			break;
//			
//		case ModuleEnum.FriendSelectModule:
//			temp = new FriendSelectScene( sEnum );
//			break;
//
//		case ModuleEnum.FriendListModule:
//			temp = new FriendListScene( sEnum );
//			break;
//			
//		case ModuleEnum.SearchFriendModule:
//			temp = new FriendSearchScene( sEnum );
//			break;
//			
//		case ModuleEnum.InformationModule:
//			temp = new InformationScene( sEnum );
//			break;
//			
//		case ModuleEnum.ApplyModule:
//			temp = new ApplyScene( sEnum );
//			break;
//			
//		case ModuleEnum.ReceptionModule:
//			temp = new ReceptionScene( sEnum );
//			break;
//			
//		case ModuleEnum.YourIDModule:
//			temp = new UserIDScene( sEnum );
//			break;
//
//		case ModuleEnum.UnitDetailModule:
//			temp = new UnitDetailScene( sEnum );
//			break;
//
//        case ModuleEnum.FriendScratchModule:
//            temp = new GachaWindowScene( sEnum );
//            break;
//
//		case ModuleEnum.RareScratchModule:
//            temp = new GachaWindowScene( sEnum );
//            break;
//
//		case ModuleEnum.EventScratchModule:
//            temp = new GachaWindowScene( sEnum );
//            break;
//
//		case ModuleEnum.SelectRoleModule:
//			temp = new SelectRoleScene( sEnum );
//			break;
//
//		case ModuleEnum.ResultModule:
//			temp = new ResultScene( sEnum );
//			break;
//
//		case ModuleEnum.FightReadyModule:
//			temp = new FightReadyScene( sEnum );
//			break;
//
//		case ModuleEnum.QuestSelectModule:
//			temp = new QuestSelectScene( sEnum );
//			break;
//
//		case ModuleEnum.OperationNoticeModule:
//			temp = new OperationNoticeScene(sEnum);
//			break;
//		case ModuleEnum.RewardModule:
//			temp = new RewardScene(sEnum);
//			break;
//		case ModuleEnum.RaiderModule:
//			temp = new GameRaiderScene(sEnum);
//			break;
////		case ModuleEnum.CurrencyModule:
////			temp = new GameCurrencyScene(sEnum);
////			break;
//		case ModuleEnum.MusicModule:
//			temp = new MusicScene(sEnum);
//			break;
//		case ModuleEnum.NicknameModule:
//			temp = new NicknameScene(sEnum);
//			break;
//
//		case ModuleEnum.ShowCardEffectModule:
//			temp = new ShowNewCardScene(sEnum);
//			break;
//
//		case ModuleEnum.VictoryModule:
//			temp = new VictoryScene(sEnum);
//			break;
//
//		case ModuleEnum.PrefaceModule:
//			temp = new PrefaceScene(sEnum);
//			break;
//
//		case ModuleEnum.ResourceDownloadModule:
//			temp = new ResourceDownloadScene(sEnum);
//			break;
//        }
//
//		if (temp != null) {
////			temp.SetDecorator (baseScene);
////			temp.InitSceneList ();
//			AddUIObject (sEnum, temp);
//		}
//		return temp;
//	}
//
//	void SwitchGameState(ModuleEnum nextScene) {
//		if (DataCenter.gameState == GameState.Normal) {
//			return;	
//		}
////		Debug.LogError("DataCenter.gameState 1 : " + DataCenter.gameState + " nextScene : " + nextScene);
//		if (nextScene == ModuleEnum.QuestSelectModule ||
//		    nextScene == ModuleEnum.FightReadyModule || 
//		    nextScene == ModuleEnum.StageSelectModule || 
//		    nextScene == ModuleEnum.EvolveModule || 
//		    nextScene == ModuleEnum.UnitDetailModule ||
//		    nextScene == ModuleEnum.VictoryModule ||
//		    nextScene == ModuleEnum.ShowCardEffectModule)  {
//			return;
//		}
//		DataCenter.gameState = GameState.Normal;
////		Debug.LogError ("DataCenter.gameState 2 : " + DataCenter.gameState + " nextScene : " + nextScene);
//	}
//
//    private void InvokeSceneClear(ModuleEnum nextScene){
////        if (baseScene.CurrentScene == ModuleEnum.FriendSelect && nextScene == ModuleEnum.StageSelect){
////            MsgCenter.Instance.Invoke(CommandEnum.QuestSelectSaveState);
////		} else if (baseScene.CurrentScene == ModuleEnum.UnitDetail) {
////            if (nextScene == ModuleEnum.LevelUp){
////                MsgCenter.Instance.Invoke(CommandEnum.LevelUpSaveState);
////			} else if (nextScene == ModuleEnum.Sell) {
////                MsgCenter.Instance.Invoke(CommandEnum.SellUnitSaveState);
////			} else if (nextScene == ModuleEnum.Party) {
////                MsgCenter.Instance.Invoke(CommandEnum.PartySaveState);
////			} else if (nextScene == ModuleEnum.Evolve) {
////				MsgCenter.Instance.Invoke (CommandEnum.EvolveSaveState);
////			} else if(nextScene == ModuleEnum.EventScratch || nextScene == ModuleEnum.FriendScratch || nextScene == ModuleEnum.RareScratch) {
////				MsgCenter.Instance.Invoke(CommandEnum.ShowGachaWindow);
////			}
////		} else if(baseScene.CurrentScene == ModuleEnum.ShowCardEffect) {
////			if(nextScene == ModuleEnum.EventScratch || nextScene == ModuleEnum.FriendScratch || nextScene == ModuleEnum.RareScratch) {
////				MsgCenter.Instance.Invoke(CommandEnum.ShowGachaWindow);
////			}
////		} else if (DataCenter.gameState == GameState.Evolve) {
////			if(nextScene == ModuleEnum.QuestSelect || nextScene == ModuleEnum.FightReady || nextScene == ModuleEnum.StageSelect) {
////				MsgCenter.Instance.Invoke(CommandEnum.EvolveSaveState);
////			}
////        }
//    }
//
//	public void ClearAllUIObject () {
//		sceneDecorator.Clear ();
//		ModuleManger.Instance.ClearModules ();
//	}
//}
