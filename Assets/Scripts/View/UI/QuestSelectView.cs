using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectView : UIBase
{
	private QuestSelectUnity window;
	private UIImageButton selectBtn;

	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UILabel sceneInfoLab;

	private DragPanel questSelectScroller;
	private GameObject questItem;

	public QuestSelectView(string uiName):base(uiName)
	{

	}
	public override void CreatUI ()
	{
		//add Share UI -- SceneInfoBar
		sceneInfoBar = ViewManager.Instance.GetViewObject("SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();
		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();

		window = ViewManager.Instance.GetViewObject( "QuestSelectWindow" ) as QuestSelectUnity;
		window.Init ("QuestSelectWindow");
		currentUIDic.Add( window.UIName, window );

		selectBtn = window.gameObject.transform.Find("btn_friend_select").GetComponent<UIImageButton>();
		selectBtn.isEnabled = false;

		//Add Scroller -- QuestSelectScroller
		questItem = Resources.Load("Prefabs/QuestScrollerItem") as GameObject;
		questSelectScroller = new DragPanel ("QuestSelectScroller", questItem);
		questSelectScroller.CreatUI();
		questSelectScroller.AddItem (3);
		questSelectScroller.RootObject.SetItemWidth(230);
		questSelectScroller.RootObject.gameObject.transform.localPosition = -630*Vector3.up;
	
	}


	private void PickQuestInfo(GameObject go)
	{
		LogHelper.Log("[SCENE JUMP]: " + "Click one Quest in the ScrollBar");
		selectBtn.isEnabled = true;
		window.UpdatePanelInfo();
	}

	private void TurnToFriendSelect(GameObject go)
	{
		ChangeScene(SceneEnum.FriendSelect);
	}

	private void BackToPreScene(GameObject btn)
	{
		window.CleanPanelInfo();
		selectBtn.isEnabled = false;
		ChangeScene(SceneEnum.Quest);
	}

	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = true;
		sceneInfoLab.text = uiName;

		for(int i = 0; i < questSelectScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(questSelectScroller.ScrollItem[ i ].gameObject).onClick += PickQuestInfo;
		}

		UIEventListener.Get(backBtn.gameObject).onClick += BackToPreScene;
		UIEventListener.Get(selectBtn.gameObject).onClick += TurnToFriendSelect;
	}

	public override void HideUI ()
	{
		SetActive(false);
		for(int i = 0; i < questSelectScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(questSelectScroller.ScrollItem[ i ].gameObject).onClick -= PickQuestInfo;
		}

		UIEventListener.Get(backBtn.gameObject).onClick  -=  BackToPreScene;
		UIEventListener.Get(selectBtn.gameObject).onClick -= TurnToFriendSelect;
	}

	public override void DestoryUI ()
	{

	}

	void SetActive(bool b)
	{
		window.gameObject.SetActive(b);
		questSelectScroller.RootObject.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

}
