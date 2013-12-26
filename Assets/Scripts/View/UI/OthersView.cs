using UnityEngine;
using System.Collections;

public class OthersView : UIBase
{
	OthersUnity window;

	private SceneInfoBar sceneInfoBar;
	private UILabel sceneInfoLab;
	private UIImageButton backBtn;

	private GameObject scrollerItem;
	private DragPanel othersScroller;
	
	public OthersView(string uiName) : base(uiName)
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

		window = ViewManager.Instance.GetViewObject("OthersWindow") as OthersUnity;
		window.Init ("OthersWindow");
		currentUIDic.Add(window.UIName, window);

		//window.gameObject.transform.localPosition = 230*Vector3.up;

		//Add Scroller -- OthersScroller
		scrollerItem = Resources.Load("Prefabs/OthersScrollerItem") as GameObject;
		
		othersScroller = new DragPanel ("OthersScroller", scrollerItem);
		othersScroller.CreatUI ();
		othersScroller.AddItem (15);
		othersScroller.RootObject.SetItemWidth(150);
		othersScroller.RootObject.gameObject.transform.localPosition = -765*Vector3.up;

		//Add Event Listener
		for(int i = 0; i < othersScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(othersScroller.ScrollItem[ i ].gameObject).onClick = ShowInfo;
		}
	}

	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = false;
		sceneInfoLab.text = uiName;

	}
	void ShowInfo(GameObject go)
	{
		LogHelper.Log("Show Some information!");
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
		othersScroller.RootObject.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

}
