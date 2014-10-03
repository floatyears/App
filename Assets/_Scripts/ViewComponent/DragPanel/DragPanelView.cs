using UnityEngine;
using System.Collections.Generic;

public class DragPanelView : ViewBase {
	private const string DragPanelPath = "Prefabs/UI/Common/DragPanelView";

	private UIGrid grid;

	private UIPanel clip;

	private UIScrollBar scrollBar;

	private UIScrollView scrollView;

	private GameObject pool;

	private List<DragPanelItemBase> scrollItem = new List<DragPanelItemBase> ();

	private Queue<DragPanelItemBase> itemPool = new Queue<DragPanelItemBase>(); 

	private DragPanelConfigItem dragConfig;

	private Transform parentObj;

	private GameObject sourceObj;

	private System.Type itemType;

	public void Init(string name, Transform parent, string url, System.Type type)
	{
		base.Init (config);
		clip = FindChild<UIPanel>("Scroll View");
		scrollView = FindChild<UIScrollView>("Scroll View");
		scrollBar = FindChild<UIScrollBar>("Scroll Bar");
		grid = FindChild<UIGrid>("Scroll View/UIGrid");
		pool = FindChild ("Pool");
		pool.SetActive (false);
		gameObject.name = name;
		dragConfig = DataCenter.Instance.GetConfigDragPanelItem (name);
		parentObj = parent;
		sourceObj = ResourceManager.Instance.LoadLocalAsset(url, null) as GameObject;
		itemType = type;
		SetScrollView ();

	}


	public override void ShowUI (){
		base.ShowUI ();
	}

	public override void HideUI (){
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		int count = scrollItem.Count;
		for (int i = 0; i < count; i++) {
			GameObject.Destroy(scrollItem[i].gameObject);
		}
		scrollItem.Clear();
		while (itemPool.Count > 0) {
			GameObject.Destroy(itemPool.Dequeue().gameObject);
		}

	}

	public void SetData<T>(List<T> data, params object[] args){
		int i = 0;
		int len = scrollItem.Count;
		GameObject obj = null;
		foreach (var item in data) {
			if(len <= i){
				GetDragItem().SetData<T>(item,args);
			}else{
				scrollItem[i].SetData<T>(item,args);
			}
			i++;
		}
		Debug.Log ("len: " + len + " i: " + i);
		for (int j = len - 1; j >= i ;j-- ) {
			itemPool.Enqueue(scrollItem[j]);
			scrollItem[j].transform.parent = pool.transform;
			scrollItem.RemoveAt(j);

		}
		grid.Reposition ();
	}

	
	void SetScrollView(){

		scrollView.GetComponent<UIPanel>().depth = dragConfig.depth;
		scrollBar.GetComponent<UIPanel>().depth = dragConfig.depth + 1;

		scrollView.movement = dragConfig.scrollMovement;
		transform.parent = parentObj;
		transform.localScale = Vector3.one;
		gameObject.transform.localPosition = dragConfig.scrollerLocalPos;
		scrollView.transform.localPosition = dragConfig.position;
		clip.clipRange = dragConfig.clipRange;
		scrollBar.transform.localPosition = dragConfig.scrollBarPosition;

		grid.arrangement = dragConfig.gridArrage;
		grid.maxPerLine = dragConfig.maxPerLine;
		grid.cellWidth = dragConfig.cellWidth;
		grid.cellHeight = dragConfig.cellHeight;

		Transform fg = scrollBar.transform.FindChild ("Foreground");
		if (dragConfig.scrollMovement == UIScrollView.Movement.Vertical ) {
			scrollView.horizontalScrollBar = null;	
			scrollView.verticalScrollBar = scrollBar;
//			scrollBar.fillDirection = UIProgressBar.FillDirection.RightToLeft;

			fg.Rotate (0, 0, 0);
			fg.GetComponent<UISprite> ().alpha = 1;
			fg.GetComponent<UISprite> ().width = (int)dragConfig.clipRange.w;

		}else {
			scrollView.horizontalScrollBar = scrollBar;	
			scrollView.verticalScrollBar = null;

			fg.Rotate (0, 0, -90);
			fg.GetComponent<UISprite> ().alpha = 1;
			fg.GetComponent<UISprite> ().width = (int)dragConfig.clipRange.z;

			scrollBar.fillDirection = UIProgressBar.FillDirection.RightToLeft;
		}

		scrollView.ResetPosition ();
//		scrollBar.alpha = 1;
	}

	private DragPanelItemBase GetDragItem(){
		DragPanelItemBase item = null;
		if(itemPool.Count > 0){
			item = itemPool.Dequeue();
		}else {
			GameObject obj = NGUITools.AddChild(grid.gameObject,sourceObj);
			if(obj.GetComponent(itemType) == null){
				obj.AddComponent(itemType);
			}
			item = obj.GetComponent<DragPanelItemBase>();
		}
		item.transform.parent = grid.transform;
		scrollItem.Add(item);
		return item;
	}

	
	void ItemCallback(GameObject target) {
//		if (DragCallback != null) {
//			DragCallback (target);
//		}
	}

	public void ClearPool(){
		foreach (var item in scrollItem) {
			item.transform.parent = pool.transform;
			itemPool.Enqueue(item);
		}
		scrollItem.Clear ();
	}
	
}
