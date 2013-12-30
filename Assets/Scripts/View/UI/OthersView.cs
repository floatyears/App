using UnityEngine;
using System.Collections;

public class OthersView : UIBase
{
	private OthersUnity window;
	private SceneInfoBar sceneInfoBar;

	private GameObject scrollerItem;
	private DragPanel othersScroller;
	
	public OthersView(string uiName) : base(uiName){}

	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject( UIConfig.othersPath + "OthersWindow") as OthersUnity;
		window.Init ("OthersWindow");
		currentUIDic.Add(window.UIName, window);

		scrollerItem = Resources.Load("Prefabs/UI/Others/OthersScrollerItem") as GameObject;
		othersScroller = new DragPanel ("OthersScroller", scrollerItem);
		othersScroller.CreatUI ();
		othersScroller.AddItem (15);
		othersScroller.RootObject.SetItemWidth(150);
		othersScroller.RootObject.gameObject.transform.localPosition = -765*Vector3.up;
	}

	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = false;
		sceneInfoBar.UITitleLab.text = UIName;

		for(int i = 0; i < othersScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(othersScroller.ScrollItem[ i ].gameObject).onClick += ShowInfo;
		}
	}

	public override void HideUI ()
	{
		SetUIActive(false);

		for(int i = 0; i < othersScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(othersScroller.ScrollItem[ i ].gameObject).onClick -= ShowInfo;
		}
	}
	
	private void SetUIActive(bool b)
	{
		window.gameObject.SetActive(b);
		othersScroller.RootObject.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

	//add to self script
	private void ShowInfo(GameObject go)
	{
		LogHelper.Log("Show Some information!");
	}

}
