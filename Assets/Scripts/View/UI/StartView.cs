using UnityEngine;
using System.Collections;

public class StartView : UIBase
{
	public static PlayerInfoBar playerInfoBar;
	public static MenuBtns menuBtns;
	public static MainBg mainBg;

	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UILabel sceneInfoLab;

	public StartView(string uiName):base(uiName)
	{

	}

	public override void CreatUI ()
	{
		//add Background
		mainBg = ViewManager.Instance.GetViewObject(UIConfig.sharePath + "MenuBg") as MainBg;
		mainBg.transform.parent = viewManager.CenterPanel.transform;
		mainBg.transform.localPosition = Vector3.zero;
		
		//add top Bar
		playerInfoBar = ViewManager.Instance.GetViewObject(UIConfig.sharePath + "PlayerInfoBar") as PlayerInfoBar;
		playerInfoBar.transform.parent = viewManager.TopPanel.transform;
		playerInfoBar.transform.localPosition = Vector3.zero;

		//add Bottom Btns
		menuBtns = ViewManager.Instance.GetViewObject(UIConfig.sharePath + "MenuBottom") as MenuBtns;
		menuBtns.transform.parent = viewManager.BottomPanel.transform.parent;
		menuBtns.transform.localPosition = Vector3.zero;

		//add scene info bar
		sceneInfoBar = ViewManager.Instance.GetViewObject(UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();
		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();
	}
	
	public static void SetActive(bool b)
	{
		playerInfoBar.gameObject.SetActive(b);
		menuBtns.gameObject.SetActive(b);
		mainBg.gameObject.SetActive(b);
	}
	
	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = false;
		sceneInfoLab.text = uiName;
	}
	
	public override void HideUI ()
	{
		sceneInfoBar.gameObject.SetActive(false);
	}
	
	public override void DestoryUI () {	

	}
}

public class StartScene : BaseComponent {
	StartDecorator dis;
	public StartScene(string uiName) : base(uiName) {

	}
	
	public override void CreatUI () {

	}

	public override void ShowUI () {

	}
	
	public override void HideUI () {

	}

	public override void DestoryUI () {

	}

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
		dis.ShowScene ();
	}

	public void HideBase () {
		dis.HideScene ();
	}

	void InitGame() {

	}
}
