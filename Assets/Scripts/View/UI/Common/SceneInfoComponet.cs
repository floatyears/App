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
		Output(se.ToString());
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
//		Debug.LogError ("SCENEINFO COMPONENT : ");
		base.DestoryUI ();
	}

	void Output(string sEnum) {
		if(viewComponent is IUICallback) {
			IUICallback uicall = viewComponent as IUICallback;
			uicall.CallbackView(sEnum);
		}
	}

	public void CallbackView (object data) {
		MsgCenter.Instance.Invoke(CommandEnum.ReturnPreScene, backScene);
		UIManager.Instance.ChangeScene(backScene);
	}

	private SceneEnum backScene = SceneEnum.None;

	public void SetBackScene(SceneEnum se) {
		if( viewComponent is IUISetBool) {
			IUISetBool sb = viewComponent as IUISetBool;
			if(se == SceneEnum.None) {
				sb.SetEnable(false);
			}
			else {
				backScene = se;
				sb.SetEnable(true);
			}
		}
	}

    public void BackSceneEnable(object args){
        IUISetBool sb = viewComponent as IUISetBool;
        sb.SetEnable((bool)args);
    }
}
