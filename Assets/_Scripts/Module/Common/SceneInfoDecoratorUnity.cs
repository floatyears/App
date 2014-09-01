using UnityEngine;
using System.Collections;

public class SceneInfoDecoratorUnity : ViewBase{
	private UILabel sceneNameLabel;
	private UIButton backBtn;
	private UILabel backBtnLabel;
//	private IUICallback iuiCallback; 
	private bool temp = false;
//	private bool isTweenDone = false;
	
	public override void Init ( UIConfigItem config ) {
		base.Init (config);
		InitUI();

//		temp = origin is IUICallback;
	}
	
	public override void ShowUI () {
//		SetGameObjectActive (true);

//		isTweenDone = false;
		GameTimer.GetInstance ().AddCountDown (0.5f, RefreshState);
		base.ShowUI ();
		ShowTween();
	}
	
	public override void HideUI () {
		GameTimer.GetInstance ().ExitCountDonw (RefreshState);
		iTween.Stop ();
		base.HideUI ();
//		SetGameObjectActive (false);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void RefreshState () {
//		isTweenDone = true;
	}

	private void InitUI() {
		sceneNameLabel = FindChild< UILabel >( "SceneTip/Label" );
		backBtn =  FindChild< UIButton >( "Button_Back" );
		backBtnLabel = FindChild<UILabel> ("Button_Back/Label");
		backBtnLabel.text = TextCenter.GetText("Btn_SceneBack");
		UIEventListener.Get( backBtn.gameObject ).onClick = BackPreScene;
	}
	
	public void CallbackView (object data) {
		string info = string.Empty;
		try {
			info = (string)data;
		}  
		catch (System.Exception ex) {
		}

		if(!string.IsNullOrEmpty(info)){
			sceneNameLabel.text = info;
		}
	}

	public void SetBackBtnActive (bool canBack){
		backBtn.gameObject.SetActive( canBack );
	}

	public void BackPreScene (GameObject go) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );
//		if( UIManager.Instance.baseScene.CurrentScene == ModuleEnum.UnitDetail ) {
//			ModuleEnum preScene = UIManager.Instance.baseScene.PrevScene;
//			MsgCenter.Instance.Invoke(CommandEnum.ReturnPreScene, preScene);
//			UIManager.Instance.ChangeScene( preScene );
//			return;
//		}

//		if(temp) {
//			IUICallback call = origin as IUICallback;
//			call.CallbackView(go);
//		}
	}

	int zOffset = -1;
	bool isAdd = false;
	private void ShowTween(){
		if (UIManager.Instance.current == null) {
			return;		
		}

		if (UIManager.Instance.current.CurrentDecoratorScene == ModuleEnum.UnitCatalogModule) {
			isAdd = true;
			backBtnLabel.transform.localPosition += zOffset * Vector3.forward;
		} else if(isAdd) {
			backBtnLabel.transform.localPosition -= zOffset * Vector3.forward;
		}

		gameObject.transform.localPosition = new Vector3(0, 500, 0);
		iTween.MoveTo(gameObject, iTween.Hash("y", -150.0f, "time", 0.2f, "islocal", true, "oncomplete", "TweenDone", "oncompletetarget", gameObject));
	}

	void TweenDone() { }

	public void SetSceneName(string name){
		sceneNameLabel.text = name;
	}
}
