using UnityEngine;
using System.Collections;

public class OthersView : UIBase
{
	OthersUnity topUI;

	private DragUI scroller;
	private OthersScrollView sv;

	public OthersView(string uiName) : base(uiName)
	{

	}

	public override void CreatUI ()
	{
		sv = ViewManager.Instance.GetViewObject("OthersBottomWindow") as OthersScrollView; 
		sv.transform.localPosition = UIConfig.UI_Z_DOWN*Vector3.up;
		currentUIDic.Add(sv.UIName, sv);
		
		scroller = new DragUI(sv.Left, sv.Right, sv.Item);
		scroller.ShowData(9);

		topUI = ViewManager.Instance.GetViewObject("OthersTopWindow") as OthersUnity;

		currentUIDic.Add(topUI.UIName, topUI);

		topUI.gameObject.transform.localPosition = UIConfig.UI_Z_TOP*Vector3.up;
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
		scroller.insUIObject.SetActive(b);
		sv.gameObject.SetActive(b);
	}

}
