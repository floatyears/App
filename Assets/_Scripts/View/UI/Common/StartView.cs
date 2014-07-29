﻿using UnityEngine;
using System.Collections;

public class StartScene : BaseComponent {
//	StartDecorator dis;
    StartDecorator dis;
	public StartScene(string uiName) : base(uiName) {}
	
	public override void CreatUI () {}

	public override void ShowUI () {
//		MsgCenter.Instance.AddListener(CommandEnum.RspAuthUser, Login );
	}

	public override void HideUI () {}

	public override void DestoryUI () {}

	private SceneEnum currentScene = SceneEnum.None;

	public SceneEnum CurrentScene {
		get {return currentScene ;}
		set {currentScene = value;}
	}

	private SceneEnum prevScene = SceneEnum.None;
	public SceneEnum PrevScene {
		get { return prevScene; }
		set { prevScene = value; }

	}

	public void SetScene(SceneEnum sEnum) {
//		Debug.LogError ("senum : " + sEnum);
		if (prevScene != SceneEnum.None && prevScene == currentScene) {
			return;		
		}

		prevScene = currentScene;
		currentScene = sEnum;

		if (sEnum == SceneEnum.Start) {
			dis = new StartDecorator (sEnum);
			dis.SetDecorator (this);
			dis.DecoratorScene ();
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