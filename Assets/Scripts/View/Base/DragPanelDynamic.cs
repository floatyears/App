using UnityEngine;
using System.Collections.Generic;

public class DragPanelDynamic {
	private static GameObject dragPanelPrefab;
	public static GameObject DragPanelPrefab {
		get {
			if(dragPanelPrefab == null) {
				dragPanelPrefab = Resources.Load(DragPanelView.DragPanelPath) as GameObject;
			}

			return dragPanelPrefab;
		}
	}

	public DragPanelView dragPanelView;
	private List<MyUnitItem> scrollItem = new List<MyUnitItem> ();
	private List<TUserUnit> scrollItemData = new List<TUserUnit> ();

	private int maxLine = 1;
	private int maxPerLine = 1;
	private int startIndex = 1;
	private int endIndex = 0;
	private GameObject sourceObject = null;

	private Vector4 OffsetPos;
	private int sourceIndex;
	private int targetIndex;

	public event UICallback callback;

	public DragPanelDynamic (GameObject parent, GameObject sourceObject, int maxLine, int maxPerLine) {
	 	this.maxLine = maxLine;
		this.maxPerLine= maxPerLine;
		this.sourceObject = sourceObject;
		CreatPanel (parent);

		GameInput.OnLateUpdate += OnLateUpdate;
	}


	public void DestoryDranPanel() {
		GameInput.OnLateUpdate -= OnLateUpdate;
		if (dragPanelView != null) {
			GameObject.Destroy(dragPanelView.gameObject);
			dragPanelView = null;
			scrollItem.Clear();
			scrollItemData.Clear();
//			dragItem.Clear();
		}
	}

	/// <summary>
	/// Add reject item
	/// </summary>
	/// <param name="count">Count.</param>
	/// <param name="itemPrefab">Item prefab.</param>
	public GameObject AddRejectItem(GameObject itemPrefab) {
		if (dragPanelView == null) {
			return null;
		}

		GameObject go = dragPanelView.AddObject(itemPrefab);
		go.name = "0";
		return go;
		dragPanelView.grid.repositionNow = true;
	}

	public void AddGameObject(int count) {
		if (dragPanelView == null) {
			return;
		}

		for (int i = 0; i < count; i++) {
			GameObject go = dragPanelView.AddObject(sourceObject);
			scrollItem.Add(go.GetComponent<MyUnitItem>());
			go.name = scrollItem.Count.ToString ();
		}
		dragPanelView.grid.Reposition ();
	}

	/// <summary>
	/// Refreshs the item by data.
	/// </summary>
	/// <param name="tuuList">data list.</param>
	public void RefreshItem(List<TUserUnit> tuuList) {
		ResetDragPanelPosition ();
		scrollItemData = tuuList;
		endIndex = scrollItemData.Count;
		int count = Mathf.Abs (tuuList.Count - scrollItem.Count);
		endIndex = tuuList.Count;

		if (count == 0) {
			for (int i = 0; i < scrollItem.Count; i++) {
				scrollItem[i].UserUnit = tuuList[i];
			}	
			return;
		}

		if (tuuList.Count > scrollItem.Count) {
			AddGameObject (count);
			for (int i = 0; i < tuuList.Count; i++) {
				scrollItem [i].UserUnit = tuuList [i];
			}

		} else {
			for (int i = scrollItem.Count -1; i >= tuuList.Count; i++) {
				GameObject go = scrollItem[i].gameObject;
				GameObject.Destroy(go);
				scrollItem.RemoveAt(i);
			}

			for (int i = 0; i < tuuList.Count; i++) {
				scrollItem[i].UserUnit = tuuList[i];
			}
		}

		dragPanelView.grid.repositionNow = true;
		dragPanelView.scrollView.Press (false);
	}

	
	void OnLateUpdate () {
		if (scrollItem.Count >= scrollItemData.Count || scrollItem.Count == 0) {
			return;	
		}

		bool firstItemVisible = dragPanelView.clip.IsVisible (scrollItem [0].Widget);//.isVisible;
		bool endItemVisible = dragPanelView.clip.IsVisible (scrollItem [scrollItem.Count - 1].Widget);// scrollItem [scrollItem.Count - 1].isVisible;
		if (firstItemVisible == endItemVisible) {
			return;	
		}

		for (int i = 0; i < maxPerLine; i++) {
			if (firstItemVisible) {
				sourceIndex = scrollItem.Count - 1;
				targetIndex = 0;
			} else {
				sourceIndex = 0;
				targetIndex = scrollItem.Count - 1;
			}
			CheckAndSwitchItem(firstItemVisible);
		}
	}

	void CheckAndSwitchItem(bool firstVisible) {
		int realSourceIndex = int.Parse (scrollItem [sourceIndex].transform.name);
		int realTargetIndex = int.Parse (scrollItem [targetIndex].transform.name);

		if (realTargetIndex >= endIndex || realTargetIndex <= startIndex) {
			return;
		}

		if (realSourceIndex > endIndex || realSourceIndex < 0) {
			return;
		}
		
		ChangeItem(realSourceIndex, realTargetIndex);
	}

	void ChangeItem(int realSourceIndex, int realTargetIndex) {
		MyUnitItem movedWidget = scrollItem [ sourceIndex ];
		scrollItem.RemoveAt ( sourceIndex );
		scrollItem.Insert ( targetIndex, movedWidget );
		int nowIndex = (realSourceIndex > realTargetIndex ? (realTargetIndex - 1) : (realTargetIndex + 1));
		movedWidget.name = nowIndex.ToString ();
		float x = nowIndex / maxPerLine * OffsetPos.x;
		float y = nowIndex % maxPerLine * OffsetPos.y;
		movedWidget.Widget.cachedTransform.localPosition = new Vector3 (x, y, 0f);

		int dataIndex = nowIndex - 1;
		scrollItem [targetIndex].UserUnit = scrollItemData [dataIndex];
	}

	void CreatItem(List<TUserUnit> data) {
		int maxEndIndex = maxLine * maxPerLine;
		int endItemIndex = data.Count > maxEndIndex ? maxEndIndex : data.Count;
		AddGameObject (endItemIndex);
		for (int i = 0; i < scrollItem.Count; i++) {
			scrollItem[i].UserUnit = data[i];
		}
	}

	void CreatPanel(GameObject parent) {
//		Debug.LogError("creat panel 1 " + DragPanelPrefab);
		dragPanelView = NGUITools.AddChild( parent, DragPanelPrefab ).GetComponent<DragPanelView>(); 
//		Debug.LogError("creat panel 2 ");
		dragPanelView.Init ( "DragPanelDynamic" );
		dragPanelView.grid.maxPerLine = this.maxPerLine;
		dragPanelView.dragPanelDynamic = this;
//		Debug.LogError("creat panel 3 ");
	}

	void ResetDragPanelPosition() {
//		if (dragPanelView != null) {
//			dragPanelView.scrollView.ResetPosition ();
//		}
	}

	public void SetDragPanel(DragPanelSetInfo dpsi) {
		dragPanelView.scrollView.GetComponent<UIPanel> ().depth = dpsi.depth;
		dragPanelView.transform.parent = dpsi.parentTrans;
		dragPanelView.transform.localPosition = dpsi.scrollerLocalPos;
		dragPanelView.transform.localScale = dpsi.scrollerScale;
		dragPanelView.transform.localPosition = dpsi.position;
		dragPanelView.clip.clipRange = dpsi.clipRange;
		dragPanelView.scrollBar.transform.localPosition = dpsi.scrollBarPosition;
		UIGrid grid = dragPanelView.grid;
		grid.arrangement = dpsi.gridArrange;
		grid.maxPerLine = dpsi.maxPerLine;
		grid.cellWidth = dpsi.cellWidth;
		grid.cellHeight = dpsi.cellHeight;
		grid.enabled = true;
		grid.Reposition ();

		OffsetPos = new Vector4 (dpsi.cellWidth, -dpsi.cellHeight, 0f, 0f);
	}
}
