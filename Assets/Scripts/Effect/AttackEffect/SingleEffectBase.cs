using UnityEngine;
using System.Collections;

public class SingleEffectBase : IEffectExcute {
	protected float animTime = 0f;
	public float AnimTime {
		get { return animTime; }
		set { animTime = value; }
	}

	protected Vector3 startPosition;	
	public Vector3 StartPosition {
		set { startPosition = value; }
		get { return startPosition; }
	}
	
	protected Vector3 endPosition ;
	public Vector3 EndPosition {
		set { endPosition = value; }
		get { return endPosition; }
	}
	
	protected GameObject targetObject;
	public GameObject TargetObject {
		get { return targetObject; }
		set { targetObject = value; }
	}

	private Callback startCallback;
	public Callback EffectStartCallback {
		set { startCallback = value; }
	}

	private Callback updateCallback;
	public Callback EffectUpdateCallback {
		set { updateCallback = value; }
	}

	protected Callback endCallback;
	
	public virtual void Excute (Callback endCallback) {
		this.endCallback = endCallback;
	}

	protected void StartPlay() {
		if (startCallback != null) {
			startCallback();	
		}
	}

	protected void UpdatePlay() {
		if (updateCallback != null) {
			updateCallback();	
		}
	}

	protected void UpdatePlayEffect () {
		if (animTime > 0) {
			animTime -= Time.deltaTime;	
			UpdatePlay();
		} else {
//			Debug.LogError("MoveOnUpdate : " + (endCallback == null));
			if(endCallback != null) {
				endCallback();
				GameInput.OnUpdate -= UpdatePlayEffect;
			}
		}
	}
}