using UnityEngine;
using System.Collections;

public class FightReadyDragView : DragSliderBase {

	protected override void InitTrans () {
		moveParent = transform.Find("MoveParent");
		cacheRightParent = transform.Find("RightParent");
		cacheLeftParent = transform.Find ("LeftParent");
	}

	public override void RefreshData () {

	}
}
