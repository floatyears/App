using System.Collections;
using UnityEngine;

public class MoveLineEffect : SingleEffectBase {
	public override void Excute (Callback endCallback) {
		base.Excute (endCallback);
		targetObject.transform.localPosition = startPosition;
		GameInput.OnUpdate += MoveOnUpdate;
		iTween.MoveTo(TargetObject,iTween.Hash("position",endPosition,"time",animTime,"easetype","islocal",true));
	}

}