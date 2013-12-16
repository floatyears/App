using UnityEngine;
using System.Collections;

public class UnitView : UIBase
{
	UnitUnity topUI;
	UnitUnity centerUI;
	UnitUnity bottomUI;

	public UnitView(string uiName) : base(uiName)
	{
		
	}
	
	public override void CreatUI ()
	{
		topUI = ViewManager.Instance.GetViewObject("UnitTopWindow") as UnitUnity;
		centerUI = ViewManager.Instance.GetViewObject("UnitCenterWindow") as UnitUnity;
		bottomUI = ViewManager.Instance.GetViewObject("UnitBottomWindow") as UnitUnity;
		
		currentUIDic.Add(topUI.UIName, topUI);
		currentUIDic.Add(bottomUI.UIName, bottomUI);
		currentUIDic.Add(centerUI.UIName, centerUI);
		
		topUI.gameObject.transform.localPosition = (UIConfig.UI_Z_TOP+30)*Vector3.up;
		centerUI.gameObject.transform.localPosition = (UIConfig.UI_Z_CENTER+20)*Vector3.up;
		bottomUI.gameObject.transform.localPosition = (UIConfig.UI_Z_DOWN-20)*Vector3.up;
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
		centerUI.gameObject.SetActive(b);
		bottomUI.gameObject.SetActive(b);
	}
}

