using UnityEngine;
using System.Collections;

public class DragChangeView : MonoBehaviour {
	private BoxCollider dragCollider = null;
	void Awake() {

	}	

	void CheckMessageReceive() {
		if (dragCollider == null) {
			dragCollider = GetComponent<BoxCollider>();
		}

		if (dragCollider == null) {
			NGUITools.AddWidgetCollider(gameObject);	
			dragCollider = gameObject.GetComponent<BoxCollider>();
		}
	}

	bool pressState = false;
	float pressTime = 0f;
	float intervTime = 0f;

	float pressYPos = 0f;
	float dragYDistance = 0f;

	void OnPress(bool pressDown) {
		if (pressDown == pressState) { //same with prev click, invaild operate.
			return ;	
		}

		if (pressDown) {
			pressTime = RealTime.time;
			pressYPos = Input.mousePosition.y;
		} else {
			intervTime = RealTime.time - pressTime;
			dragYDistance = Input.mousePosition.y - pressYPos;
		}


	}
}
