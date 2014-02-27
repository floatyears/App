using UnityEngine;
using System.Collections.Generic;

public class MoveLineAndBoomEffect : EffectBehaviorBase {
	public MoveLineAndBoomEffect() {
		CollectEffectExcute ();
	}

	public override void CollectEffectExcute () {
		IEffectExcute ee = new MoveLineEffect ();
		effectList.Add (ee);
	}

	public override void Excute (List<Vector3> position) {
		if (position.Count != effectList.Count * 2) {
			Debug.LogError("effect position count is not match effect count : " + position.Count + "   effectList.Count : " +  effectList.Count);
			return;
		}
		effectList[0].StartPosition = position[0];
		effectList[0].EndPosition = position[1];
		effectList[0].TargetObject = effectAssetList[0];
		effectList[0].AnimTime = 1f;
		effectList[0].Excute (EffectEndCallback);
	}

	void EffectEndCallback () {
		IEffectExcute ee = effectList [0];
		effectList.Remove (ee);
		GameObject.Destroy (ee.TargetObject);
		effectAssetList.Remove (ee.TargetObject);
		if (effectList.Count == 0) {
			return;	
		}
		ee = effectList [0];
		ee.Excute (EffectEndCallback);
	}

	void StartBomb() {

	}
}
