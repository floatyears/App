using UnityEngine;
using System.Collections.Generic;

public class DragPanelNew : ConcreteComponent,IDragPanel {
    private event UICallback callback;
    private List<GameObject> item = new List<GameObject>();
    public List<GameObject> GetItem {
        get {
            return item;
        }
    }

    private DragPanelView drag;
    public DragPanelView DragPanelView {
        get {
            return drag;
        }
    }

    private GameObject sourceObject;

    public DragPanelNew(string name) : base(name) {
		ResourceManager.Instance.LoadLocalAsset(DragPanelView.DragPanelPath, o =>{drag = NGUITools.AddChild(ViewManager.Instance.TopPanel.transform.parent.gameObject, o as GameObject).GetComponent<DragPanelView>();});
        
    }

    public void AddItem(int count, GameObject source = null) {
        if (item != null) {
            sourceObject = source;
        }
        if (sourceObject == null) {
            Debug.LogError(drag.name + " scroll view item is null. don't creat drag panel");
            return;
        }
        for (int i = 0; i < count; i++) {
            GameObject go = drag.AddObject(this.sourceObject);
            item.Add(go);
            UIEventListener.Get(go).onClick = ClickObject;
        }
    }


    public void RemoveItem(GameObject target) {
        if (!item.Contains(target)) {
            return;		
        }
        item.Remove(target);
        GameObject.Destroy(target);
        drag.grid.Reposition();
        UIEventListener.Get(target).onClick = null;
    }

    public void SetPosition(Vector4 position) {
        drag.SetViewPosition(position);
    }
	
    void ClickObject(GameObject go) {
        if (callback != null) {
            callback(go);
        }
    }
	
    public override void DestoryUI() {
        base.DestoryUI();
        for (int i = 0; i < item.Count; i++) {
            GameObject.Destroy(item[i]);
            item.RemoveAt(i);
        }
//		GameObject.Destroy (scrollBar.gameObject);
//		GameObject.Destroy (itemContain.gameObject);

    }

    public override void HideUI() {
        //dragPanelView.HideUI ();
        base.HideUI();
    }

    public override void ShowUI() {
        base.ShowUI();
    }
}

public interface IDragPanel {
    List<GameObject> GetItem { get; } 
    void AddItem(int count, GameObject source);
    void RemoveItem(GameObject target);
    void SetPosition(Vector4 position);
}