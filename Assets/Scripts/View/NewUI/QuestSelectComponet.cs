using UnityEngine;
using System.Collections;

public class QuestSelectComponent : ConcreteComponent,IUICallback {

	private DragPanel questSelectScroller;
	private GameObject questItem;

	public QuestSelectComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();

		questItem = Resources.Load("Prefabs/UI/Quest/QuestScrollerItem") as GameObject;
		questSelectScroller = new DragPanel ("QuestSelectScroller", questItem);
		questSelectScroller.CreatUI();
		questSelectScroller.AddItem (3);
		questSelectScroller.RootObject.SetItemWidth(230);
		questSelectScroller.RootObject.gameObject.transform.localPosition = -630*Vector3.up;

		for(int i = 0; i < questSelectScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(questSelectScroller.ScrollItem[ i ].gameObject).onClick = PickQuestInfo;
		}
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		SetUIActive(true);

	}
	
	public override void HideUI () {
		base.HideUI ();
		SetUIActive(false);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void Callback (object data)
	{
		try {
			SceneEnum se = (SceneEnum)data;
			UIManager.Instance.ChangeScene(se);
			//Debug.Log("QuestSelectComponent  "+se.ToString());
		} 
		catch (System.Exception ex) {
			LogHelper.LogException(ex);
		}
	}

	private void PickQuestInfo(GameObject go)
	{
		if(viewComponent is IUICallback) {
			IUICallback call = viewComponent as IUICallback;
			call.Callback(true);
		}
	}

	private void SetUIActive(bool b)
	{
		questSelectScroller.RootObject.gameObject.SetActive(b);
	}
}
