using UnityEngine;
using System.Collections;

public class BombEffect : SingleEffectBase {
	public override void Excute (Callback endCallback) {
		base.Excute (endCallback);
		targetObject.SetActive (true);
		targetObject.transform.localPosition = startPosition;
		StartPlay ();
		GameInput.OnUpdate += UpdatePlayEffect;
	}
}
