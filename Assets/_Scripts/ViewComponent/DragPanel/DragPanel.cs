using UnityEngine;
using System.Collections.Generic;

public class DragPanel : ModuleBase{
	public event UICallback DragCallback;

	protected DragPanelView dragPanelView;

	private List<GameObject> scrollItem = null;
	public List<GameObject> ScrollItem {
		get{ 
			return scrollItem; 
		}
	}

	public DragPanel(string name, string sourceObjUrl,System.Type type, Transform parentTransform):base(null){
		ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Common/DragPanelView", o => {
			dragPanelView = (GameObject.Instantiate(o as GameObject) as GameObject).GetComponent<DragPanelView>();
			dragPanelView.Init(name,parentTransform,sourceObjUrl,type);
		});
	}

	public override void ShowUI () {
		base.ShowUI ();
		dragPanelView.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
		dragPanelView.HideUI ();
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
		dragPanelView.DestoryUI ();
	}

	public void SetData<T>(List<T> data, params object[] args){
		dragPanelView.SetData<T>(data, args);

		if( scrollItem == null ) {
			scrollItem = new List<GameObject>();
		}
	
		scrollItem.Clear();
		foreach(var item in dragPanelView.ScrollItem) {
			scrollItem.Add(item.gameObject);
		}
	}

	public void Clear(){
		dragPanelView.ClearPool ();
	}
//
//	public GameObject GetDragViewObject(){
//		if (dragPanelView != null) {
//			return dragPanelView.gameObject;
//		}
//		else{
//			return null;
//		}
//	}

	public void RefreshUIPanel(){
		if (dragPanelView != null)
			dragPanelView.RefreshUIPanel ();
	}

	public void AddItemToGrid(GameObject obj, int index){
		dragPanelView.AddItemToGrid (obj, index);
	}

	public void ItemCallback(params object[] data){
		dragPanelView.ItemCallback (data);
	}
	
}
