using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfoBarView : ViewBase{
	private UILabel sceneNameLabel;
	private GameObject backBtn;
	private UILabel backBtnLabel;
//	private IUICallback iuiCallback; 
	private bool temp = false;
//	private bool isTweenDone = false;
	
	private Stack<ModuleEnum> backModules = new Stack<ModuleEnum>();
	
	public override void Init ( UIConfigItem config, Dictionary<string, object> data = null ) {
		base.Init (config, data);
		InitUI();

//		temp = origin is IUICallback;
	}

	private void InitUI() {
		sceneNameLabel = FindChild< UILabel >( "SceneTip/Label" );
		backBtn =  FindChild("Button_Back");
		backBtnLabel = FindChild<UILabel> ("Button_Back/Label");
		backBtnLabel.text = TextCenter.GetText("Btn_SceneBack");
		UIEventListenerCustom.Get(backBtn).onClick = BackPreScene;
	}
	
	public void CallbackView(params object[] args) {
		string info = string.Empty;
		try {
			info = (string)args[0];
		}  
		catch (System.Exception ex) {
		}

		if(!string.IsNullOrEmpty(info)){
			sceneNameLabel.text = info;
		}
	}

	public void SetBackBtnActive (bool canBack,ModuleEnum name = ModuleEnum.None){
		if (!canBack) {
			backModules.Clear();
		}
		if(backModules.Count <= 0 || backModules.Peek() != name){
			backModules.Push(name);
			Debug.Log("back: " + name);
		}
		SetSceneName (name);
		backBtn.SetActive( canBack );
	}

	public void BackPreScene (GameObject go) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );

		if(backModules.Count > 0)
			ModuleManager.Instance.HideModule (backModules.Pop());
		ModuleManager.Instance.ShowModule (backModules.Peek());
	}

	public void SetSceneName(ModuleEnum name){
		sceneNameLabel.text = TextCenter.GetText(name.ToString());
	}
	public void SetSceneName(string name){
//		currModule = name;
		sceneNameLabel.text = name;
	}

	public void NoBackScene(ModuleEnum name){
		if (backModules.Count > 0 && backModules.Peek() == name) {
			backModules.Pop();
		}
	}
}
