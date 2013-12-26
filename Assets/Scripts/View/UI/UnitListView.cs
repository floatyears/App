using UnityEngine;
using System.Collections;

public class UnitListView : UIBase
{
	private UnitListUnity window;

	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UILabel sceneInfoLab;

	public UnitListView(string uiName):base(uiName)
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
		LogHelper.Log("11111111112222222333333" + sceneInfoBar.name);
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

	void SetActive(bool b)
	{
		sceneInfoBar.gameObject.SetActive(b);
	}
}
