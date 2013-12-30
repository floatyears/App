using UnityEngine;
using System.Collections;

public class ShopView : UIBase
{
	private ShopUnity window;
	private SceneInfoBar sceneInfoBar;

	public ShopView(string uiName) : base(uiName){}
	
	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;


		window = ViewManager.Instance.GetViewObject( UIConfig.shopPath + "ShopWindow") as ShopUnity;
		window.Init ("ShopWindow");
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

