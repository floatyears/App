using UnityEngine;
using System.Collections.Generic;

public abstract class EffectBehaviorBase : IEffectBehavior {
	protected List<GameObject> effectAssetList = new List<GameObject> ();
	public List<GameObject> EffectAssetList {
		get { return effectAssetList; }
		set { effectAssetList = value; }
	}

	protected List<IEffectExcute> effectList = new List<IEffectExcute>();

	public abstract void CollectEffectExcute ();

	public abstract void Excute (List<Vector3> position);
}
