using UnityEngine;
using System.Collections;

public class DragChangViewItem : MonoBehaviour {
	public DragChangeView dragView;	

	void OnPress(bool isPress) {
		dragView.OnPress (isPress);
	}
 
	void OnDrag(Vector2 deltaPos) {
		dragView.OnDrag (deltaPos);
	}
}
