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
		window = ViewManager.Instance.GetViewObject("ShopWindow") as ShopUnity;
		imgBtns = ViewManager.Instance.GetViewObject("ShopChip") as ShopUnity;
		window.Init ("ShopWindow");
		imgBtns.Init ("ShopChip");
		currentUIDic.Add(window.UIName, window);
		currentUIDic.Add(imgBtns.UIName, imgBtns);
		
		window.gameObject.transform.localPosition = 220*Vector3.up;
		imgBtns.gameObject.transform.localPosition = -100*Vector3.up;
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

