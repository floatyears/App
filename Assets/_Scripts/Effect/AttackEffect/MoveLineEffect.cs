using System.Collections;
using UnityEngine;

public class MoveLineEffect : SingleEffectBase {
	public override void Excute (Callback endCallback) {
//		Debug.LogError (startPosition + "  endPosition : " + endPosition);
		base.Excute (endCallback);
		targetObject.SetActive (true);
		targetObject.transform.localPosition = startPosition;
		StartPlay ();
		GameInput.OnUpdate += UpdatePlayEffect;
		iTween.MoveTo(TargetObject,iTween.Hash("position",endPosition,"time",animTime,"easetype",iTween.EaseType.easeInQuart,"islocal",true));
	}



}