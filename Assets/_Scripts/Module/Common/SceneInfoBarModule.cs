using UnityEngine;
using System.Collections;

public class SceneInfoBarModule : ModuleBase {
	
	public SceneInfoBarModule(UIConfigItem config):base(  config) {
		CreateUI<SceneInfoBarView> ();
        MsgCenter.Instance.AddListener(CommandEnum.BackSceneEnable, BackSceneEnable);
    }
	
	public override void DestoryUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.BackSceneEnable, BackSceneEnable);
		base.DestoryUI ();
	}

	public void OnReceiveMessage (object data) {

//		if (DataCenter.gameState == GameState.Evolve) {
//			if(backScene == ModuleEnum.HomeModule) {
//				backScene = ModuleEnum.EvolveModule;
//			}
//
//			if(backScene == ModuleEnum.FriendSelectModule) {
//				backScene = ModuleEnum.QuestSelectModule;
//			}
//		}
//
//		MsgCenter.Instance.Invoke(CommandEnum.ReturnPreScene, backScene);
//		ModuleManger.Instance.ShowModule(backScene);
//		if ((GroupType)data [1] == GroupType.Module) {
//
//		}else if((GroupType)data [1] == GroupType.Scene){
//			(SceneEnum)data[0];
//		}
		(view as SceneInfoBarView).SetSceneName("Scene Name");
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
