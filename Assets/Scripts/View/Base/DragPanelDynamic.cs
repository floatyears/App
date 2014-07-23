using UnityEngine;
using System.Collections.Generic;

public class DragPanelDynamic {
	private static GameObject dragPanelPrefab;
	public static GameObject DragPanelPrefab {
		get {
			if(dragPanelPrefab == null) {
				dragPanelPrefab = ResourceManager.Instance.LoadLocalAsset(DragPanelView.DragPanelPath,null) as GameObject;
			}
			return dragPanelPrefab;
		}
	}

	public DragPanelView dragPanelView;
	public List<MyUnitItem> scrollItem = new List<MyUnitItem> ();
	private List<TUserUnit> scrollItemData = new List<TUserUnit> ();

	private int maxLine = 1;
	private int maxPerLine = 1;
	private int maxIndex = 0;
	private int startIndex = 1;
	private int endIndex = 0;
	private GameObject sourceObject = null;

	private Vector4 OffsetPos;
	private int sourceIndex;
	private int targetIndex;

	private bool isReject = false;

	public event UICallback callback;

	public DragPanelDynamic (GameObject parent, GameObject sourObject, int maxLine, int maxPerLine) {
	 	this.maxLine = maxLine;
		this.maxPerLine= maxPerLine;
		sourceObject = sourObject;
		sourceObject.SetActive (false);
		sourceObject.transform.localPosition = new Vector3 (0f, 10000f, 0f);
		CreatPanel (parent);
		maxIndex = maxLine * maxPerLine;
		GameInput.OnLateUpdate += OnLateUpdate;
	}

	public void DestoryDragPanel() {
		GameInput.OnLateUpdate -= OnLateUpdate;
		GameObject.Destroy (sourceObject);
		if (dragPanelView != null) {
			GameObject.Destroy(dragPanelView.gameObject);
			dragPanelView = null;
			scrollItem.Clear();
			scrollItemData.Clear();
		}
	}

	public void RefreshItem(TUserUnit tuu) {
		int index = scrollItemData.FindIndex (a => a.MakeUserUnitKey () == tuu.MakeUserUnitKey ());
		if (index > -1) {
			scrollItemData[index] = tuu;
		}

		index = scrollItem.FindIndex (a => a.UserUnit.MakeUserUnitKey () == tuu.MakeUserUnitKey ());
		if (index > -1) {
			scrollItem[index].UserUnit = tuu;
//			scrollItem[index].IsEnable = false;
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
		dragPanelView.grid.repositionNow = true;
		isReject = true;
		return go;
	}

	public void AddGameObject(int count) {
		if (dragPanelView == null || count == 0) {
			return;
		}

		int lastIndex = 1;
		if (scrollItem.Count > 0) {
			lastIndex = int.Parse(scrollItem[scrollItem.Count - 1].gameObject.name) + 1;
		}
		sourceObject.SetActive (true);
		for (int i = 0; i < count; i++) {
			GameObject go = dragPanelView.AddObject(sourceObject);
			scrollItem.Add(go.GetComponent<MyUnitItem>());
			go.name = lastIndex.ToString ();
			lastIndex++;
		}
		sourceObject.SetActive (false);
		dragPanelView.grid.Reposition ();
	}

	/// <summary>
	/// Refreshs the item by data.
	/// </summary>
	/// <param name="tuuList">data list.</param>
	public List<MyUnitItem> RefreshItem(List<TUserUnit> tuuList) {
		endIndex = tuuList.Count;
		if (scrollItem.Count == 0 && tuuList.Count > 0) {
			CreatItem(tuuList);
			scrollItemData = tuuList;
			dragPanelView.grid.repositionNow = true;
			dragPanelView.scrollView.Press (false);
			return scrollItem;
		}

		int realStartIndex = int.Parse(scrollItem[0].gameObject.name) - 1;
		int realEndIndex = int.Parse(scrollItem[scrollItem.Count - 1].gameObject.name) - 1;

		if (scrollItemData.Count == tuuList.Count) {	
//			Debug.LogError("realStartIndex : " + realStartIndex + " realEndIndex : " + realEndIndex);
			for (int i = realStartIndex; i <= realEndIndex; i++) {
				scrollItem[i - realStartIndex].UserUnit = tuuList[i];
//				Debug.LogError(scrollItem[i - realStartIndex] + " tuuList[i] : " + tuuList[i].isEnable);
			}	
			scrollItemData = tuuList;
			return scrollItem;
		}  

		int count;
		if(tuuList.Count > scrollItemData.Count) {
			count = tuuList.Count - scrollItemData.Count;
			int number = maxIndex - scrollItem.Count;
			if(count > number) {
				count = number;
			}
			AddGameObject(count);
			for (int i = realStartIndex; i < realEndIndex; i++) {
				scrollItem[i].UserUnit = tuuList[i];
			}
		} else {
			dragPanelView.scrollView.ResetPosition();
			if( tuuList.Count >= scrollItem.Count ) {
				realStartIndex = 0;			//int.Parse(scrollItem[0].gameObject.name) - 1;
				realEndIndex = maxIndex;	//int.Parse(scrollItem[scrollItem.Count - 1].gameObject.name) - 1;
//				Debug.LogError(realStartIndex + " realEndIndex : " + realEndIndex);
				for (int i = realStartIndex; i < realEndIndex; i++) {
					scrollItem[i].UserUnit = tuuList[i];
				}
			} else {
				for (int i = scrollItem.Count - 1; i >=  tuuList.Count; i--) {
					GameObject go = scrollItem[i].gameObject;
					GameObject.Destroy(go);
					scrollItem.RemoveAt(i);
				}

				realStartIndex = 0;				//int.Parse(scrollItem[0].gameObject.name) - 1;
				realEndIndex = tuuList.Count;	//int.Parse(scrollItem[scrollItem.Count - 1].gameObject.name);

				for (int i = realStartIndex; i < realEndIndex; i++) {
					scrollItem[i].UserUnit = tuuList[i];
				}
			}
		}
		scrollItemData = tuuList;
		dragPanelView.grid.repositionNow = true;
		dragPanelView.scrollView.Press (false);

		return scrollItem;
	}
	
	void OnLateUpdate () {
		if (scrollItem.Count >= scrollItemData.Count || scrollItem.Count == 0) {
			return;	
		}

		bool firstItemVisible = dragPanelView.clip.IsVisible (scrollItem [0].Widget);
		bool endItemVisible = dragPanelView.clip.IsVisible (scrollItem [scrollItem.Count - 1].Widget);

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

		int xCoord = 0;
		if (isReject) {
			xCoord = nowIndex / maxPerLine;
		} else {
			xCoord = (nowIndex - 1) / maxPerLine;
		}

		float x = xCoord * OffsetPos.x;
		float y = 0f;
		if (isReject) {
			y = nowIndex % maxPerLine * OffsetPos.y;		
		} else {
			y = (nowIndex - xCoord * maxPerLine - 1) * OffsetPos.y;
		}

		Vector3 pos = new Vector3 (x, y, 0f);
		movedWidget.Widget.cachedTransform.localPosition = pos;
		int dataIndex = nowIndex - 1;

		scrollItem [targetIndex].UserUnit = scrollItemData [dataIndex];
	}

	void CreatItem(List<TUserUnit> data) {
		int endItemIndex = data.Count > maxIndex ? maxIndex : data.Count;
		AddGameObject (endItemIndex);
		for (int i = 0; i < scrollItem.Count; i++) {
			scrollItem[i].UserUnit = data[i];

		}
	}

	void CreatPanel(GameObject parent) {
		dragPanelView = NGUITools.AddChild( parent, DragPanelPrefab ).GetComponent<DragPanelView>(); 
		dragPanelView.Init ( "DragPanelDynamic" );
		dragPanelView.grid.maxPerLine = this.maxPerLine;
		dragPanelView.dragPanelDynamic = this;
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
