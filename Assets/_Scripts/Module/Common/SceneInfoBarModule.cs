using UnityEngine;
using System.Collections;

public class SceneInfoBarModule : ModuleBase {
	
	public SceneInfoBarModule(UIConfigItem config):base(  config) {
        MsgCenter.Instance.AddListener(CommandEnum.BackSceneEnable, BackSceneEnable);
    }
	
	public override void InitUI () {
		base.InitUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
//		ModuleEnum se = UIManager.Instance.baseScene.CurrentScene;
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.BackSceneEnable, BackSceneEnable);
		base.DestoryUI ();
	}
		
//	public ICheckUIState checkUiState;

	public void OnReceiveMessage (object data) {
//		if (checkUiState != null) {
//			ModuleEnum current = ModuleEnum.None;//UIManager.Instance.baseScene;
//			if (current == ModuleEnum.LevelUp || current == ModuleEnum.Evolve) {
//				if(!checkUiState.CheckState()) {
//					MsgCenter.Instance.Invoke(CommandEnum.FriendBack);
//					return;
//				}
//			}	
//		}

		if (DataCenter.gameState == GameState.Evolve) {
			if(backScene == ModuleEnum.HomeModule) {
				backScene = ModuleEnum.EvolveModule;
			}

			if(backScene == ModuleEnum.FriendSelectModule) {
				backScene = ModuleEnum.QuestSelectModule;
			}
		}

		MsgCenter.Instance.Invoke(CommandEnum.ReturnPreScene, backScene);
		ModuleManger.Instance.ShowModule(backScene);
	}

	public ModuleEnum backScene = ModuleEnum.None;

	public void SetBackScene(ModuleEnum scene) {
//		if( viewComponent is IUISetBool) {
//			IUISetBool sb = viewComponent as IUISetBool;
//			if(scene == ModuleEnum.None) {
//				sb.SetBackBtnActive(false);
//			} else {
//				backScene = scene;
//				sb.SetBackBtnActive(true);
//			}
//		}
	}

	private string sceneName;
	public void SetCurSceneName(string name) {
//		sceneName = name;
//		if(viewComponent is IUICallback) {
//			IUICallback uicall = viewComponent as IUICallback;
//			uicall.CallbackView(sceneName);
//		}
	}

    public void BackSceneEnable(object args) {
//        IUISetBool sb = viewComponent as IUISetBool;
////		Debug.LogError ("BackSceneEnable args : " + args);
//        sb.SetBackBtnActive((bool)args);
    }
}
