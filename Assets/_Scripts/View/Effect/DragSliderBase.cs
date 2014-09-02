using UnityEngine;
using System.Collections;

public class DragSliderBase : MonoBehaviour {
	protected Transform moveParent = null;
	protected Transform cacheLeftParent = null;
	protected Transform cacheRightParent = null;
	protected DragChangeViewSpring spring;
	protected BoxCollider dragCollider;
	protected Vector3 startPos;
	protected Vector3 rightStartPos;
	protected Vector3 leftStartPos;
	protected IDragChangeView dragChangeViewData;
	protected Vector3 intervDistance;
	protected int changeDistance = 0;
	protected UIPanel panel = null;
	protected bool moveToRight;
	protected bool stopOperate = false;

	public virtual void Init() {
		panel = GetComponent<UIPanel> ();
		if (panel == null) {
			panel = gameObject.AddComponent<UIPanel>();	
		}
	}
}
