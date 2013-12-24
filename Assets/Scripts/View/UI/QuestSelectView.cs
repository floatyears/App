using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectView : UIBase
{
	private QuestSelectUnity window;

	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UILabel sceneInfoLab;

	public QuestSelectView(string uiName):base(uiName)
	{

	}
	public override void CreatUI ()
	{
		//add scene info bar
		sceneInfoBar = ViewManager.Instance.GetViewObject("SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();
		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();

		window = ViewManager.Instance.GetViewObject( "QuestSelectWindow" ) as QuestSelectUnity;
		window.Init ("QuestSelectWindow");
		currentUIDic.Add( window.UIName, window );
	}

	private void BackToPreScene(GameObject btn)
	{
		ChangeScene(SceneEnum.Quest);
	}

	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = true;
		sceneInfoLab.text = uiName;
		UIEventListener.Get(backBtn.gameObject).onClick += BackToPreScene;
	}

	public override void HideUI ()
	{
		SetActive(false);
		UIEventListener.Get(backBtn.gameObject).onClick  -=  BackToPreScene;
	}

	public override void DestoryUI ()
	{

	}

	void SetActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

}
