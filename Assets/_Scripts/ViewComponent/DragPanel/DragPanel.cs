using UnityEngine;
using System.Collections.Generic;

public class DragPanel : ModuleBase{
	public event UICallback DragCallback;

	protected DragPanelView dragPanelView;
//	public DragPanelView DragPanelView{
//		get{
//			return dragPanelView;
//		}
//	}

	protected List<GameObject> scrollItem = new List<GameObject> ();
	public List<GameObject> ScrollItem {
		get{ return scrollItem; }
	}

	protected GameObject sourceObject;
	public GameObject SetResourceObject {
		set{ sourceObject = value; }
		get{ return sourceObject; }
	}

	public static GameObject dragObject;

	private DragPanelConfigItem dragConfig;

	private Transform parent;

	public DragPanel(string name, GameObject obj,Transform parentTransform):base(null){
		dragConfig = DataCenter.Instance.GetConfigDragPanelItem (name);
		sourceObject = obj;
		parent = parentTransform;
		Debug.Log("panel create: " + name + " config: " + dragConfig.configName);
		if(dragObject == null){
			ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Common/DragPanelView", o => {
				dragObject = o as GameObject;
				dragPanelView = (GameObject.Instantiate(dragObject) as GameObject).GetComponent<DragPanelView>();
				dragPanelView.name = name;
				dragPanelView.Init(null);
				dragPanelView.SetScrollView (dragConfig, parent);
			});
		}else{
			dragPanelView = (GameObject.Instantiate(dragObject) as GameObject).GetComponent<DragPanelView>();
			dragPanelView.name = name;
			dragPanelView.Init(null);
			dragPanelView.SetScrollView (dragConfig, parent);
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		AddEvent ();
		dragPanelView.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
		RemoveEvent ();
		dragPanelView.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		RemoveEvent ();
		int count = scrollItem.Count;
		for (int i = 0; i < count; i++) {
			GameObject go = scrollItem[i];
			GameObject.Destroy(go);
		}
		scrollItem.Clear();
		if (dragPanelView != null) {
			GameObject.Destroy (dragPanelView.gameObject);	
		}
	}
	
	public void AddItem(int count,GameObject obj = null ,bool isClean = false) {
		if (obj != null) {
			sourceObject = obj;	
		}				
		if (isClean) {
			for (int i = 0; i < scrollItem.Count; i++) {
				GameObject.Destroy(scrollItem[i]);
				scrollItem.RemoveAt(i);
			}
		}
		if (sourceObject == null) {
			LogHelper.LogError (dragPanelView.name + " scroll view item is null. don't creat drag panel ");
			return ;
		}
		if (dragPanelView == null) {
			dragPanelView = NGUITools.AddChild(ViewManager.Instance.TopPanel.transform.parent.gameObject, dragObject).GetComponent<DragPanelView>(); 		
		}
		for (int i = 0; i < count; i++) {
			//Debug.Log("source Object: " +sourceObject);
			GameObject go = dragPanelView.AddObject(sourceObject);
			if(go != null){
				scrollItem.Add(go);
			}
		}
//		dragPanelView.grid.sorted = true;
		dragPanelView.grid.repositionNow = true;
		dragPanelView.scrollView.Press (false);
	}

	public GameObject AddScrollerItem( GameObject obj ,bool isClean = false) {
		if (obj != null) {
			sourceObject = obj;	
		}

		if (isClean) {
			for (int i = 0; i < scrollItem.Count; i++) {
				GameObject.Destroy(scrollItem[i]);
				scrollItem.RemoveAt(i);
			}
		}
		
		if (sourceObject == null)
			return obj;

		GameObject go = dragPanelView.AddObject(sourceObject);
		if(go != null)
			scrollItem.Add(go);

		return go;
	}

	public void Refresh() {
		for (int i = 0; i < scrollItem.Count; i++) {
			scrollItem[i].name = i.ToString();
		}
		dragPanelView.grid.Reposition ();
	}
	
	public void RemoveItem (GameObject target){
		bool b = scrollItem.Contains (target);
		if (!b) {
			return;		
		}
		scrollItem.Remove (target);
		GameObject.Destroy (target);
		dragPanelView.grid.enabled = true;
		dragPanelView.grid.Reposition ();
		UIEventListener.Get (target).onClick = null;
	}
	
	public void SetPosition(Vector4 position) {
		dragPanelView.SetViewPosition (position);
	}

	public GameObject GetDragViewObject(){
		if (dragPanelView != null) {
			return dragPanelView.gameObject;
		}
		else{
			return null;
		}
	}

	void ItemCallback(GameObject target) {
		if (DragCallback != null) {
			DragCallback (target);
		}
	}

	void AddEvent() {
		for (int i = 0; i < scrollItem.Count; i++) {
			UIEventListener.Get(scrollItem[i]).onClick += ItemCallback;
		}
	}
	
	void RemoveEvent()
	{
		for (int i = 0; i < scrollItem.Count; i++) {
			UIEventListener ui = UIEventListener.Get(scrollItem[i]);
			if(ui.onClick != null)
				ui.onClick -= ItemCallback;
		}
	}
}
