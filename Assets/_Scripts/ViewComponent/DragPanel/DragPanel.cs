using UnityEngine;
using System.Collections.Generic;

public class DragPanel : ModuleBase{
	public event UICallback DragCallback;

	protected DragPanelView dragPanelView;

	public List<GameObject> ScrollItem {
		get{ return null;}//dragPanelView.scrollItem; }
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
	}

	public void Clear(){
		dragPanelView.ClearPool ();
	}

	public GameObject GetDragViewObject(){
		if (dragPanelView != null) {
			return dragPanelView.gameObject;
		}
		else{
			return null;
		}
	}

	public void AddItemToGrid(GameObject obj, int index){
		dragPanelView.AddItemToGrid (obj, index);
	}

	public void ItemCallback(params object[] data){
		dragPanelView.ItemCallback (data);
	}
	
}
