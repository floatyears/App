using UnityEngine;
using System.Collections;

public class ShopView : UIBase
{
	ShopUnity window;
	ShopUnity imgBtns;

	public ShopView(string uiName) : base(uiName)
	{
		
	}
	
	public override void CreatUI ()
	{
		window = ViewManager.Instance.GetViewObject("ShopPropCard") as ShopUnity;
		imgBtns = ViewManager.Instance.GetViewObject("ShopPropChip") as ShopUnity;
		
		currentUIDic.Add(window.UIName, window);
		currentUIDic.Add(imgBtns.UIName, imgBtns);
		
		window.gameObject.transform.localPosition = new Vector3(0, 70, 0);
		imgBtns.gameObject.transform.localPosition = new Vector3(0, -100, 0);
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
		window.gameObject.SetActive(b);
		imgBtns.gameObject.SetActive(b);
	}
}

