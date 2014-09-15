using UnityEngine;
using System.Collections;

public class DragChangViewItem : MonoBehaviour {
	private DragSliderBase dragView;	

	public void Init(DragSliderBase dsb) {
//		dragView = dsb;
	}

	void Awake() {
		DragSliderBase dsb = GetComponentInParent<DragSliderBase> ();
		dragView = dsb;
//		Debug.LogError ("DragChangViewItem Awake dsb : " + dsb);
	}

	void OnPress(bool isPress) {
		dragView.OnPress (isPress);
	}
 
	void OnDrag(Vector2 deltaPos) {
		dragView.OnDrag (deltaPos);
	}
}
