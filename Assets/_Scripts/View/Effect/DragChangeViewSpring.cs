using UnityEngine;
using System.Collections.Generic;

public class DragChangeViewSpring : MonoBehaviour {
	public Transform targetObject;
	public Vector3 toPos;
	public const int speed = 10;
	public Callback callback;
	
	void Update () {
		Vector3 befoure = targetObject.localPosition;
		Vector3 after = Vector3.Lerp (befoure, toPos, RealTime.deltaTime * speed);
		if ((after - toPos).sqrMagnitude <= 0.01f) {
			after = toPos;
			if(callback != null) {
				callback();
			}

			enabled = false;
		}

		targetObject.localPosition = after;
	}

	public static DragChangeViewSpring Begin(Transform target, Vector3 toPos, Callback cb = null) {
		DragChangeViewSpring dcvs = target.GetComponent<DragChangeViewSpring> ();
		if (dcvs == null) {
			dcvs = target.gameObject.AddComponent<DragChangeViewSpring>();	
		}
		dcvs.targetObject = target;
		dcvs.toPos = toPos;
		dcvs.callback = cb;
		dcvs.enabled = true;
		return dcvs;
	}

	public void StopSpring() {
		enabled = false;
	}
}
