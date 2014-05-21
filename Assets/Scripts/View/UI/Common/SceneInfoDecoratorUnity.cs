using UnityEngine;
using System.Collections;

public class SceneInfoDecoratorUnity : UIComponentUnity ,IUICallback, IUISetBool{
	
	private UILabel sceneNameLabel;
	private UIButton backBtn;

	private IUICallback iuiCallback; 
	private bool temp = false;
	
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();

		temp = origin is IUICallback;
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTween();

	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		sceneNameLabel = FindChild< UILabel >( "SceneTip/Label" );
		backBtn =  FindChild< UIButton >( "Button_Back" );

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
		if( UIManager.Instance.baseScene.CurrentScene == SceneEnum.UnitDetail ) {
			SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
//			Debug.LogError("BackPreScene SceneInfoDecoratorUnity : " + preScene);
			MsgCenter.Instance.Invoke(CommandEnum.ReturnPreScene, preScene);
			UIManager.Instance.ChangeScene( preScene );
			return;
		}

		if(temp) {
			IUICallback call = origin as IUICallback;
			call.CallbackView(go);
		}
	}

	private void ShowTween(){
		iTween.MoveFrom(gameObject, iTween.Hash("y", 50.0f, "time", 0.3f));
	}
}
