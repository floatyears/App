using UnityEngine;
using System.Collections;

public class UnitView : UIBase
{
	UnitUnity topUI;
	UnitUnity centerUI;
	UnitUnity bottomUI;
	private GameObject partyBtn;

	public UnitView(string uiName) : base(uiName)
	{
		
	}
	
	public override void CreatUI ()
	{
		topUI = ViewManager.Instance.GetViewObject("UnitTopWindow") as UnitUnity;
		centerUI = ViewManager.Instance.GetViewObject("UnitCenterWindow") as UnitUnity;
		bottomUI = ViewManager.Instance.GetViewObject("UnitBottomWindow") as UnitUnity;
		topUI.Init ("UnitTopWindow");
		centerUI.Init ("UnitCenterWindow");
		bottomUI.Init ("UnitBottomWindow");

		partyBtn = bottomUI.transform.FindChild("Party").gameObject;
		UIEventListener.Get(partyBtn.gameObject).onClick = TurnToParty;

		currentUIDic.Add(topUI.UIName, topUI);
		currentUIDic.Add(bottomUI.UIName, bottomUI);
		currentUIDic.Add(centerUI.UIName, centerUI);
		
		topUI.gameObject.transform.localPosition = 330*Vector3.up;
		centerUI.gameObject.transform.localPosition = 160*Vector3.up;
		bottomUI.gameObject.transform.localPosition = -90*Vector3.up;
	}

	void TurnToParty(GameObject go)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Party);
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
