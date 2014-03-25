using UnityEngine;
using System.Collections;

public class SceneInfoComponent : ConcreteComponent, IUICallback {
	
	public SceneInfoComponent(string uiName):base(uiName) {}
	
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
		base.DestoryUI ();
	}

	void Output(string sEnum) {
		if(viewComponent is IUICallback) {
			IUICallback uicall = viewComponent as IUICallback;
			uicall.Callback(sEnum);
		}
	}

	public void Callback (object data) {
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
}
