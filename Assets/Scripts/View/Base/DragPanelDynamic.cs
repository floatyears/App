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

	private const int RedundancyLine = 2;

	protected DragPanelView dragPanelView;
	private List<GameObject> scrollItem = new List<GameObject> ();
	private List<TUserUnit> scrollItemData = new List<TUserUnit> ();
	private int startIndex = 0;
	private int maxLine = 1;
	private int maxPerLine = 1;
	private int firstShowIndex = 0;
	private int endShowIndex = 0;
	private GameObject sourceObject = null;

	public event UICallback callback;

	public DragPanelDynamic (GameObject parent, GameObject sourceObject, int maxLine, int maxPerLine) {
	 	this.maxLine = maxLine;
		this.maxPerLine= maxPerLine;
		this.sourceObject = sourceObject;
		CreatPanel (parent);
	}

	public void DestoryDranPanel() {
		if (dragPanelView != null) {
			GameObject.Destroy(dragPanelView.gameObject);
			dragPanelView = null;
			scrollItem.Clear();
			scrollItemData.Clear();
		}
	}

	/// <summary>
	/// Add reject item
	/// </summary>
	/// <param name="count">Count.</param>
	/// <param name="itemPrefab">Item prefab.</param>
	public void AddItem(GameObject itemPrefab) {
		if (dragPanelView == null) {
			return;
		}

		GameObject go = dragPanelView.AddObject(itemPrefab);
		go.name = startIndex.ToString ();
		if(go != null) {
			scrollItem.Add(go);
		}
		firstShowIndex = startIndex = 1;
		dragPanelView.grid.repositionNow = true;
	}

	/// <summary>
	/// Refreshs the item by data.
	/// </summary>
	/// <param name="tuuList">data list.</param>
	public void RefreshItem(List<TUserUnit> tuuList) {
		ResetDragPanelPosition (tuuList.Count);

//		scrollItemData = tuuList;

		if (scrollItem.Count == 0) {
			CreatItem(tuuList);
		}

		int haveDataCount = scrollItem.Count - startIndex;

//		int haveCount = ;
//
//		int maxEndIndex = maxLine * (maxPerLine + RedundancyLine) + startIndex;
//
//		int endIndex = haveCount > maxEndIndex ? maxEndIndex : haveCount;
//
//		int showIndex = maxLine * maxPerLine + startIndex;
//
//		endShowIndex = haveCount > showIndex ? showIndex : haveCount;

//		for (int i = startIndex; i < endIndex; i++) {
//			GameObject go = dragPanelView.AddObject(sourceObject);
//			go.name = i.ToString();
//			scrollItem.Add(go);
//		}

		dragPanelView.grid.repositionNow = true;
	}

	void CreatItem(List<TUserUnit> data) {
		int maxEndIndex = maxLine * (maxPerLine + RedundancyLine);
		int endIndex = data.Count > maxEndIndex ? maxEndIndex : data.Count;
		for (int i = startIndex; i <= endIndex; i++) {
			GameObject go = dragPanelView.AddObject(sourceObject);
			go.name = i.ToString();
			scrollItem.Add(go);
		}
	}

	void CreatPanel(GameObject parent) {
		dragPanelView = NGUITools.AddChild( parent, DragPanelPrefab ).GetComponent<DragPanelView>(); 
		dragPanelView.Init ( "DragPanelDynamic" );
		dragPanelView.grid.maxPerLine = this.maxPerLine;
	}

	void ResetDragPanelPosition(int count) {
		if (dragPanelView != null) {
			dragPanelView.scrollView.ResetPosition ();
		}
	}
}
