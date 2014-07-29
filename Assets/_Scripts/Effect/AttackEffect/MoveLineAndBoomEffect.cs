using UnityEngine;
using System.Collections.Generic;

public class MoveLineAndBoomEffect : EffectBehaviorBase {
	public MoveLineAndBoomEffect() {
		CollectEffectExcute ();
	}

	public override void CollectEffectExcute () {
		IEffectExcute ee = new MoveLineEffect ();
		effectList.Add (ee);
		ee = new BombEffect ();
		effectList.Add (ee);
	}

	public override void Excute (List<Vector3> position) {
		if (position.Count != effectList.Count * 2) {
			Debug.LogError("effect position count is not match effect count : " + position.Count + "   effectList.Count : " +  effectList.Count);
			return;
		}
//		Debug.LogError ("effectAssetList : " + effectAssetList.Count + " effectList.Count : " + effectList.Count);
		for (int i = 0; i < effectList.Count; i++) {
			effectList[i].StartPosition = position[i * 2];
			effectList[i].EndPosition = position[i* 2 + 1];
			effectList[i].TargetObject = effectAssetList[i];
		}
	
		effectList[0].AnimTime = 1f;
		effectList [1].AnimTime = 0.5f;
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
}
