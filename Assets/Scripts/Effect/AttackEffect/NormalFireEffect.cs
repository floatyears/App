using UnityEngine;
using System.Collections.Generic;

public class NormalFireEffect : IEffectConcrete {

	private EffectManager effectManager;
	public NormalFireEffect () {
		effectManager = EffectManager.Instance;
	}

	public void Play (List<GameObject> effect,AttackInfo ai) {
		List<GameObject> tempGo = effectManager.InitGameObject (effect);
//		Debug.LogError ("tempGo : " + tempGo.Count);
		List<Vector3> position = new List<Vector3>();
		Vector3 temp = effectManager.DisposeActorPosition(BattleBackground.ActorTransform[ai.UserUnitID]);
		position.Add(temp);
		temp = effectManager.DisposeEnemyPosition(BattleEnemy.Monster[ai.EnemyID].transform);
		position.Add(temp);
		temp = effectManager.DisposeEnemyPosition(BattleEnemy.Monster[ai.EnemyID].transform);
		position.Add(temp);
		temp = effectManager.DisposeEnemyPosition(BattleEnemy.Monster[ai.EnemyID].transform);
		position.Add(temp);
//		PlayAttackEffect(EffectConstValue.NormalFire1,position,tempGo);
		IEffectBehavior mlabe = new MoveLineAndBoomEffect ();
		mlabe.EffectAssetList = tempGo;
		mlabe.Excute (position);
	}


}
