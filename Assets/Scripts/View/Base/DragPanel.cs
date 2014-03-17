using UnityEngine;
using System.Collections.Generic;

public class DragPanel : UIBase 
{
	public event UICallback DragCallback;
	private DragPanelView dragPanelView;
	public DragPanelView DragPanelView{
		get{
			return dragPanelView;
		}
	}

	private List<GameObject> scrollItem = new List<GameObject> ();
	public List<GameObject> ScrollItem {
		get{ return scrollItem; }
	}
	private GameObject sourceObject;
	public GameObject SetResourceObject {
		set{ sourceObject = value; }
		get{ return sourceObject; }
	}
	public static GameObject dragObject;
	public DragPanel(string name,GameObject obj) : base(name){
		sourceObject = obj;
		if(dragObject == null){
			dragObject = Resources.Load("Prefabs/UI/Common/DragPanelView") as GameObject;
		}
	}
	public override void CreatUI () {
		base.CreatUI ();
		CreatPanel ();
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
		for (int i = 0; i < scrollItem.Count; i++) {
			GameObject.Destroy(scrollItem[i]);
			scrollItem.RemoveAt(i);
		}
		GameObject.Destroy (dragPanelView.gameObject);
	}

	void CreatPanel() {
		dragPanelView = NGUITools.AddChild(
			viewManager.TopPanel.transform.parent.gameObject, dragObject).GetComponent<DragPanelView>(); 
		dragPanelView.Init (uiName);
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
			LogHelper.LogError (dragPanelView.name + 
			                    " scroll view item is null. don't creat drag panel");
			return ;
		}
		if (dragPanelView == null) {
			CreatPanel();		
		}
		for (int i = 0; i < count; i++) {
//			Debug.LogError(sourceObject.name);
			GameObject go = dragPanelView.AddObject(sourceObject);
			if(go != null){
				scrollItem.Add(go);
			}
		}
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
		if (!scrollItem.Contains (target)) {
			return;		
		}
		scrollItem.Remove (target);
		GameObject.Destroy (target);
		dragPanelView.grid.enabled = true;
		dragPanelView.grid.Reposition ();
		UIEventListener.Get (target).onClick = null;
	}
	
	public void SetPosition(Vector4 position)
	{
		dragPanelView.SetViewPosition (position);
	}

	void ItemCallback(GameObject target)
	{
		if (DragCallback != null) {
			DragCallback (target);
		}
	}

	void AddEvent()
	{
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
