using UnityEngine;
using System.Collections;

public class SingleEffectBase : IEffectExcute {
	private Vector3 startPosition;	
	public Vector3 StartPosition {
		set { startPosition = value; }
		get { return startPosition; }
	}

	private Vector3 endPosition ;
	public Vector3 EndPosition {
		set { endPosition = value; }
		get { return endPosition; }
	}

	private GameObject targetObject;
	public GameObject TargetObject {
		get { return targetObject; }
		set { targetObject = value; }
	}

	private Callback endCallback;

	public void Excute (Callback endCallback) {
		this.endCallback = endCallback;
	}


}
