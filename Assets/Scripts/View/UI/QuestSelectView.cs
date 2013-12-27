using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSelectView : UIBase
{
	private QuestSelectUnity window;
	private SceneInfoBar sceneInfoBar;

	private UIImageButton selectBtn;

	private DragPanel questSelectScroller;
	private GameObject questItem;

	public QuestSelectView(string uiName):base(uiName){}
	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject( UIConfig.questPath + "QuestSelectWindow" ) as QuestSelectUnity;
		window.Init ("QuestSelectWindow");
		currentUIDic.Add( window.UIName, window );

		selectBtn = window.gameObject.transform.Find("btn_friend_select").GetComponent<UIImageButton>();
		selectBtn.isEnabled = false;

		questItem = Resources.Load("Prefabs/UI/Quest/QuestScrollerItem") as GameObject;
		questSelectScroller = new DragPanel ("QuestSelectScroller", questItem);
		questSelectScroller.CreatUI();
		questSelectScroller.AddItem (3);
		questSelectScroller.RootObject.SetItemWidth(230);
		questSelectScroller.RootObject.gameObject.transform.localPosition = -630*Vector3.up;
	}
	
	private void PickQuestInfo(GameObject go)
	{
		selectBtn.isEnabled = true;
		window.UpdatePanelInfo();
	}

	private void JumpToFriendSelect(GameObject go)
	{
		ChangeScene(SceneEnum.FriendSelect);
	}

	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = true;
		sceneInfoBar.UITitleLab.text = UIName;
		UIEventListener.Get(sceneInfoBar.BackBtn.gameObject).onClick += BackUI;
		UIEventListener.Get(selectBtn.gameObject).onClick += JumpToFriendSelect;
		for(int i = 0; i < questSelectScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(questSelectScroller.ScrollItem[ i ].gameObject).onClick += PickQuestInfo;
		}
	}

	public override void HideUI ()
	{
		SetUIActive(false);
		selectBtn.isEnabled = false;
		window.CleanPanelInfo();
		UIEventListener.Get(sceneInfoBar.BackBtn.gameObject).onClick  -=  BackUI;
		UIEventListener.Get(selectBtn.gameObject).onClick -= JumpToFriendSelect;
		for(int i = 0; i < questSelectScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(questSelectScroller.ScrollItem[ i ].gameObject).onClick -= PickQuestInfo;
		}
	}

	private void SetUIActive(bool b)
	{
		window.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
		questSelectScroller.RootObject.gameObject.SetActive(b);
	}

	private void BackUI(GameObject btn)
	{
		window.CleanPanelInfo();
		selectBtn.isEnabled = false;
		controllerManger.BackToPrevScene();
	}
}
