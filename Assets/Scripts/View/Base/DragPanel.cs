using UnityEngine;
using System.Collections.Generic;

public class DragPanel : UIBase {

	public event UICallback DragCallback;

	private DragPanelView rootObject;

	public DragPanelView RootObject{
		get{
			return rootObject;
		}
	}

	private UIScrollBar scrollBar;

	private UIGrid itemContain;

	private List<GameObject> scrollItem = new List<GameObject> ();

	public List<GameObject> ScrollItem
	{
		get{ return scrollItem; }
	}

	private GameObject sourceObject;

	public GameObject SetResourceObject
	{
		set{ sourceObject = value; }
		get{ return sourceObject; }
	}

	public DragPanel(string name, GameObject obj) : base(name){
		sourceObject = obj;
	}

	public override void CreatUI () {
		base.CreatUI ();
		rootObject = viewManager.GetViewObject ("DragPanelView") as DragPanelView;
		rootObject.transform.parent = viewManager.TopPanel.transform;
		rootObject.Init (uiName);
	}

	public override void ShowUI () {
		base.ShowUI ();
		AddEvent ();
		rootObject.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
		RemoveEvent ();
		rootObject.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		RemoveEvent ();
		for (int i = 0; i < scrollItem.Count; i++) {
			GameObject.Destroy(scrollItem[i]);
			scrollItem.RemoveAt(i);
		}
		GameObject.Destroy (scrollBar.gameObject);
		GameObject.Destroy (itemContain.gameObject);
	}

	public void AddItem(int count,bool isClean = false,GameObject obj = null) {
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
			LogHelper.LogError (rootObject.name + " scroll view item is null. don't creat drag panel");
			return;
		}
						
		for (int i = 0; i < count; i++) {
			GameObject go = rootObject.AddObject(sourceObject,scrollItem.Count);
			if(go != null){
				scrollItem.Add(go);
			}
		}
	}

	/// <summary>
	/// Sets the position.
	/// </summary>
	/// <param name="position">Position. x is center x; y is center y; z is size x, w is size y</param>
	public void SetPosition(Vector4 position)
	{
		rootObject.SetViewPosition (position);
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
