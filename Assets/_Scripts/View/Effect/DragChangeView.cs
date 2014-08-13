using UnityEngine;
using System.Collections.Generic;
using System;

public class DragChangeView : MonoBehaviour {
	public Transform moveParent = null;
	public Transform cacheLeftParent = null;
	public Transform cacheRightParent = null;

	private DragChangeViewSpring spring;
	private BoxCollider dragCollider = null;
	private Vector3 startPos;
	private Vector3 rightStartPos;
	private Vector3 leftStartPos;

	private IDragChangeView dragChangeViewData;
	private List<Transform> dataTrans = new List<Transform>();
	private Vector3 intervDistance;
	private int changeDistance = 0;

	private bool moveToRight;

	private bool stopOperate = false;

	void Awake() {
		CheckMessageReceive();

		startPos = moveParent.localPosition;
		rightStartPos = cacheRightParent.localPosition;
		leftStartPos = cacheLeftParent.localPosition;
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

	public void RefreshParty() {
		TUnitParty current = DataCenter.Instance.PartyInfo.CurrentParty;
		TUnitParty prev = DataCenter.Instance.PartyInfo.GetPrePartyData;
		TUnitParty next = DataCenter.Instance.PartyInfo.GetNextPartyData;

		moveParent.GetComponent<RefreshPartyInfo> ().RefreshView (current);
		cacheLeftParent.GetComponent<RefreshPartyInfo> ().RefreshView (prev);
		cacheRightParent.GetComponent<RefreshPartyInfo> ().RefreshView (next);
	}

	public void SetDataInterface (IDragChangeView idcv) {
		dragChangeViewData = idcv;

		intervDistance = new Vector3 (idcv.xInterv, 0f, 0f);
		changeDistance = idcv.xInterv >> 1;

		if (dragChangeViewData == null) {
			Debug.LogError("drag change view data is null");
		}
	}

	public void OnPress(bool pressDown) {
		if (stopOperate) {
			return;	
		}

		if (pressDown == pressState) { //same with prev click, invaild operate.
			return;
		}

		pressState = pressDown;

		if (pressDown) {
			OnDragBegin();
		} else {
			OnDragEnd();
		}
	}

	void OnDragBegin() {
		pressTime = RealTime.time;
		pressYPos = Input.mousePosition.y;
		if (spring != null && spring.enabled) {
			spring.StopSpring();
		}
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

	public void OnDrag(Vector2 deltaPos) {
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
