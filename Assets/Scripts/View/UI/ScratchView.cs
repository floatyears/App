using UnityEngine;
using System.Collections;

public class ScratchView : UIBase 
{
	private ScratchUnity window;
	private SceneInfoBar sceneInfoBar;

	public ScratchView(string uiName) : base(uiName){}

	public override void CreatUI ()
	{	
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject( UIConfig.scratchPath + "ScratchWindow") as ScratchUnity;
		window.Init ("ScratchWindow");
		currentUIDic.Add(window.UIName, window);
	}

	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = false;
		sceneInfoBar.UITitleLab.text = UIName;
	}
	
	public override void HideUI ()
	{
		SetUIActive(false);
	}

	private void SetUIActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

}
