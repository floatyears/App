using UnityEngine;
using System.Collections;

public class SceneInfoComponent : ModuleBase {
	
	public SceneInfoComponent(string uiName):base(uiName) {
        MsgCenter.Instance.AddListener(CommandEnum.BackSceneEnable, BackSceneEnable);
    }
	
	public override void CreatUI () {
		base.CreatUI ();
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

	public void CallbackView (object data) {
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
		UIManager.Instance.ChangeScene(backScene);
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
