using UnityEngine;
using System.Collections;

public class DragChangViewItem : MonoBehaviour {
	private DragSliderBase dragView;	

	public void Init(DragSliderBase dsb) {
		dragView = dsb;
	}

	void OnPress(bool isPress) {
		dragView.OnPress (isPress);
	}
 
	void OnDrag(Vector2 deltaPos) {
		dragView.OnDrag (deltaPos);
	}
}
