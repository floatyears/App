using UnityEngine;
using System.Collections;

public class SceneInfoComponent : ConcreteComponent, IUICallback {
	
	public SceneInfoComponent(string uiName):base(uiName) {
        MsgCenter.Instance.AddListener(CommandEnum.BackSceneEnable, BackSceneEnable);
    }
	
	public override void CreatUI () {
		base.CreatUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		SceneEnum se = UIManager.Instance.baseScene.CurrentScene;
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.BackSceneEnable, BackSceneEnable);
		base.DestoryUI ();
	}
		
	public ICheckUIState checkUiState;

	public void CallbackView (object data) {
		if (checkUiState != null) {
			SceneEnum current = UIManager.Instance.baseScene.CurrentScene;
			if (current == SceneEnum.LevelUp || current == SceneEnum.Evolve) {
				if(!checkUiState.CheckState()) {
					MsgCenter.Instance.Invoke(CommandEnum.FriendBack);
					return;
				}
			}	
		}

		if (DataCenter.gameState == GameState.Evolve) {
			if(backScene == SceneEnum.Home) {
				backScene = SceneEnum.Evolve;
			}

			if(backScene == SceneEnum.FriendSelect) {
				backScene = SceneEnum.QuestSelect;
			}
		}

		MsgCenter.Instance.Invoke(CommandEnum.ReturnPreScene, backScene);
		UIManager.Instance.ChangeScene(backScene);
	}

	public SceneEnum backScene = SceneEnum.None;

	public void SetBackScene(SceneEnum scene) {
		if( viewComponent is IUISetBool) {
			IUISetBool sb = viewComponent as IUISetBool;
			if(scene == SceneEnum.None) {
				sb.SetBackBtnActive(false);
			} else {
				backScene = scene;
				sb.SetBackBtnActive(true);
			}
		}
	}

	private string sceneName;
	public void SetCurSceneName(string name) {
		sceneName = name;
		if(viewComponent is IUICallback) {
			IUICallback uicall = viewComponent as IUICallback;
			uicall.CallbackView(sceneName);
		}
	}

    public void BackSceneEnable(object args) {
        IUISetBool sb = viewComponent as IUISetBool;
//		Debug.LogError ("BackSceneEnable args : " + args);
        sb.SetBackBtnActive((bool)args);
    }
}
