using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class DragSliderBase : MonoBehaviour {
	protected Transform moveParent = null;
	protected Transform cacheLeftParent = null;
	protected Transform cacheRightParent = null;

	protected BoxCollider dragCollider;
	protected Vector3 startPos;
	protected Vector3 rightStartPos;
	protected Vector3 leftStartPos;
	protected IDragChangeView dragChangeViewData;
	protected Vector3 intervDistance;
	protected int changeDistance = 0;
	protected bool moveToRight;

	protected bool pressState = false;
	protected float pressTime = 0f;
	protected float intervTime = 0f;
	protected float pressYPos = 0f;
	protected float dragYDistance = 0f;

	protected bool stopOperate = false;
	public bool StopOperate {
		get { return stopOperate; }
		set { stopOperate = value; }
	}

	private void Init() {
		InitTrans ();

		SetPosition ();

//		DragChangViewItem[] dcvi = moveParent.GetComponentsInChildren<DragChangViewItem> ();
//		for (int i = 0; i < 4; i++) {
//			Debug.LogError("drag change view item : " + moveParent.FindChild(i.ToString()).GetComponent<DragChangViewItem>());
////			Debug.LogError ("dcvi : " + dcvi.Length + " transform : " + gameObject.activeInHierarchy + " self active : " + gameObject.activeSelf);
//		}
//	
////		DragChangViewItem[] dcvi = transform.GetComponentsInChildren<DragChangViewItem>();
//		Debug.LogError ("dcvi : " + dcvi.Length);
//		foreach (var item in dcvi) {
//			item.Init(this);
//		}
	}

	protected virtual void InitTrans() { }

	void SetPosition() {
		if (moveParent == null) {
			return;
		}

		startPos = moveParent.localPosition;
		leftStartPos = cacheLeftParent.localPosition;
		rightStartPos = cacheRightParent.localPosition;
	}

	public virtual void RefreshData() { }
	public virtual void RefreshData(UnitParty tup) { }

	public void RefreshCustomData(UnitParty tup) {
		stopOperate = true;
		RefreshData (tup);
	}

	public void SetDataInterface (IDragChangeView idcv) {
		Init ();

		dragChangeViewData = idcv;
		
		intervDistance = new Vector3 (idcv.xInterv, 0f, 0f);
		changeDistance = System.Convert.ToInt32 (idcv.xInterv * 0.3f);
		
		if (dragChangeViewData == null) {
			Debug.LogError("drag change view data is null");
		}
	}

	public virtual void OnPress(bool pressDown) {
		if (stopOperate) {
			return;	
		}
		
		if (pressDown ) {
			OnDragBegin();
		} 
		if(!pressDown){
			OnDragEnd();
		}
	}

	void OnDragBegin() {
		pressTime = RealTime.time;
		pressYPos = Input.mousePosition.y;
	}
	
	void OnDragEnd() {
		intervTime = RealTime.time - pressTime;
		dragYDistance = Input.mousePosition.y - pressYPos;
		
		bool isChange = Mathf.Abs (moveParent.localPosition.x - startPos.x) >= changeDistance;

		if (isChange) {
			stopOperate = true;
			if(moveToRight) {
				cacheRightParent.localPosition = leftStartPos;
				DragChangeViewSpring.Begin(cacheLeftParent, startPos);
				DragChangeViewSpring.Begin (moveParent, rightStartPos, RightMoveEnd);
			} else {
				cacheLeftParent.localPosition = rightStartPos;
				DragChangeViewSpring.Begin(cacheRightParent, startPos);
				DragChangeViewSpring.Begin (moveParent, leftStartPos, LeftMoveEnd);
			}
			
		} else {
			DragChangeViewSpring.Begin (moveParent, startPos);
			DragChangeViewSpring.Begin(cacheLeftParent, leftStartPos);
			DragChangeViewSpring.Begin(cacheRightParent, rightStartPos);
		}
	}

	void RightMoveEnd() {
		Transform temp = cacheRightParent;
		cacheRightParent = moveParent;
		moveParent = cacheLeftParent;
		cacheLeftParent = temp;
		dragChangeViewData.RefreshParty (true);
		
		MoveEnd ();
	}
	
	void LeftMoveEnd() {
		Transform temp = cacheLeftParent;
		cacheLeftParent = moveParent;
		moveParent = cacheRightParent;
		cacheRightParent = temp;
		dragChangeViewData.RefreshParty (false);
		
		MoveEnd ();
	}

	void MoveEnd() {
		stopOperate = false;
		
	}

	
	public virtual void OnDrag(Vector2 deltaPos) {
		if (stopOperate) {
			return;	
		}
		
		float deltaX = deltaPos.x;
		moveParent.localPosition += new Vector3 (deltaX, 0f, 0f);
		moveToRight = deltaX > 0;
		cacheLeftParent.localPosition = moveParent.localPosition - intervDistance;
		cacheRightParent.localPosition = moveParent.localPosition + intervDistance;
	}
}
