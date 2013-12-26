using UnityEngine;
using System.Collections;

public class UnitView : UIBase
{
	UnitUnity window;
	private SceneInfoBar sceneInfoBar;
	private UILabel sceneInfoLab;
	private UIImageButton backBtn;

	public UnitView(string uiName) : base(uiName)
	{
		
	}
	
	public override void CreatUI ()
	{
		//Add Share UI -- SceneInfoBar
		sceneInfoBar = ViewManager.Instance.GetViewObject("SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;
		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();
		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();

		window = ViewManager.Instance.GetViewObject("UnitWindow") as UnitUnity;
		window.Init ("UnitWindow");

		currentUIDic.Add(window.UIName, window);
		
		window.gameObject.transform.localPosition = 315*Vector3.up;
	}

	void SetActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = false;
		sceneInfoLab.text = uiName;
	}
	
	public override void HideUI ()
	{
		SetActive(false);
	}
	
	public override void DestoryUI ()
	{
		
	}
}

