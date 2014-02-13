using UnityEngine;
using System.Collections;

public class StartScene : BaseComponent {
	StartDecorator dis;
	public StartScene(string uiName) : base(uiName) {}
	
	public override void CreatUI () {}

	public override void ShowUI () {}
	
	public override void HideUI () {}

	public override void DestoryUI () {}

	private SceneEnum currentScene = SceneEnum.None;

	public SceneEnum CurrentScene {
		get {return currentScene ;}
	}

	private SceneEnum prevScene;
	public SceneEnum PrevScene {
		get { return prevScene; }

	}

	public void SetScene(SceneEnum sEnum) {
		prevScene = currentScene;
		currentScene = sEnum;

		if (sEnum == SceneEnum.Start) {
			dis = new StartDecorator (sEnum);
			dis.SetDecorator (this);
			dis.DecoratorScene ();
			dis.ShowScene ();
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
