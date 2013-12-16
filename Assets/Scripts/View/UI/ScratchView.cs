using UnityEngine;
using System.Collections;

public class ScratchView : UIBase 
{
	ScratchUnity topUI;
	ScratchUnity bottomUI;
	
	public ScratchView(string uiName) : base(uiName)
	{

	}

	public override void CreatUI ()
	{
		topUI = ViewManager.Instance.GetViewObject("ScratchTopWindow") as ScratchUnity;
		bottomUI = ViewManager.Instance.GetViewObject("ScratchBottomWindow") as ScratchUnity;

		currentUIDic.Add(topUI.UIName, topUI);
		currentUIDic.Add(bottomUI.UIName, bottomUI);

		topUI.gameObject.transform.localPosition = UIConfig.UI_Z_TOP*Vector3.up;
		bottomUI.gameObject.transform.localPosition = UIConfig.UI_Z_DOWN*Vector3.up;
	}

	public override void ShowUI ()
	{
		SetActive(true);
	}
	
	public override void HideUI ()
	{
		SetActive(false);
	}
	
	public override void DestoryUI ()
	{
		
	}
	
	void SetActive(bool b)
	{
		topUI.gameObject.SetActive(b);
		bottomUI.gameObject.SetActive(b);
	}

}
