using UnityEngine;
using System.Collections;

public class StartModule : ModuleBase {
//	StartDecorator dis;
    StartScene dis;
	public StartModule(UIConfigItem config) : base( config) {}
	
	public override void InitUI () {}

	public override void ShowUI () {
//		MsgCenter.Instance.AddListener(CommandEnum.RspAuthUser, Login );
	}

	public override void HideUI () {}

	public override void DestoryUI () {}

	private ModuleEnum currentScene = ModuleEnum.None;

	public ModuleEnum CurrentScene {
		get {return currentScene ;}
		set {currentScene = value;}
	}

	private ModuleEnum prevScene = ModuleEnum.None;
	public ModuleEnum PrevScene {
		get { return prevScene; }
		set { prevScene = value; }

	}

	public void SetScene(ModuleEnum sEnum) {
//		Debug.LogError ("senum : " + sEnum);
		if (prevScene != ModuleEnum.None && prevScene == currentScene) {
			return;		
		}

		prevScene = currentScene;
		currentScene = sEnum;

		if (sEnum == ModuleEnum.StartModule) {
			dis = new StartScene (sEnum);
//			dis.SetDecorator (this);
			dis.InitSceneList ();
//			dis.ShowScene ();
		}
	}

	public void ShowBase () {
//		Debug.LogError (dis);
		dis.ShowScene ();
	}

	public void HideBase () {
		dis.HideScene ();
	}

	void InitGame() {}
}
