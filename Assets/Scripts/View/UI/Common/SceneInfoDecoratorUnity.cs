using UnityEngine;
using System.Collections;

public class SceneInfoDecoratorUnity : UIComponentUnity ,IUICallback, IUISetBool{
	
	private UILabel labelSceneName;
	private UIImageButton btnBackScene;

	private IUICallback iuiCallback; 
	private bool temp = false;
	
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();

		temp = origin is IUICallback;
	}
	
	public override void ShowUI () {
        LogHelper.Log("SceneInfobar.ShowUI()");
		base.ShowUI ();
		ShowTween();

	}
	
	public override void HideUI () {
        LogHelper.Log("SceneInfobar.HideUI()");
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		labelSceneName = FindChild< UILabel >( "ImgBtn_Back_Scene/Label_Scene_Name" );
		btnBackScene =  FindChild< UIImageButton >( "ImgBtn_Back_Scene" );

		UIEventListener.Get( btnBackScene.gameObject ).onClick = BackPreScene;
	}
	
	public void CallbackView (object data) {
		string info = string.Empty;
		try {
			info = (string)data;
		} 
		catch (System.Exception ex) {
		}
		if(!string.IsNullOrEmpty(info)){
			labelSceneName.text = info;
		}
	}

	public void SetEnable (bool b)
	{
		btnBackScene.isEnabled = b;
	}

	void BackPreScene (GameObject go) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );
		if( UIManager.Instance.baseScene.CurrentScene == SceneEnum.UnitDetail ) {
			SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
			MsgCenter.Instance.Invoke(CommandEnum.ReturnPreScene, preScene);
			UIManager.Instance.ChangeScene( preScene );
			return;
		}

		if(temp) {
			IUICallback call = origin as IUICallback;
			call.CallbackView(go);
		}
	}

	private void ShowTween()
	{
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list)
		{		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}
}
