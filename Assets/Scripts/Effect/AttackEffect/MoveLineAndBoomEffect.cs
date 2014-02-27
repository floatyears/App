using UnityEngine;
using System.Collections.Generic;

public class MoveLineAndBoomEffect : EffectBehaviorBase {
	public override void CollectEffectExcute () {
		IEffectExcute ee = new MoveLineEffect ();
		effectList.Add (ee);

	}

	public override void Excute (List<Vector3> position) {
		if (position.Count != effectList.Count * 2) {
			Debug.LogError("effect position count is not match effect count");
			return;
		}

//		for (int i = 0; i < effectList.Count; i++) {
//			int j = i * 2;
		effectList[0].StartPosition = position[0];
		effectList[0].EndPosition = position[1];
		effectList[0].TargetObject = effectAssetList[0];
		effectList [0].AnimTime = 1f;
		EffectEndCallback();
//		}


	}

	void EffectEndCallback () {
		if (effectList.Count == 0) {
			return;	
		}
		IEffectExcute ee = effectList [0];
		effectList.Remove (ee);
		ee.Excute (EffectEndCallback);
	}
}
