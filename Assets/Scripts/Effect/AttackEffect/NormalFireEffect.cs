//using UnityEngine;
//using System.Collections.Generic;
//
//public class NormalFireEffect : IEffectConcrete {
//	private EffectManager effectManager;
//	public NormalFireEffect () {
//		effectManager = EffectManager.Instance;
//	}
//
//	public void Play (List<GameObject> effect,AttackInfo ai) {
//		List<GameObject> tempGo = effectManager.InitGameObject (effect);
//		List<Vector3> position = new List<Vector3>();
//		Vector3 temp = BattleBackground.AttackPosition[ai.UserUnitID];
//		position.Add(temp);
//		temp = effectManager.DisposeEnemyPosition(BattleEnemy.Monster[ai.EnemyID].transform);
//		position.Add(temp);
//		temp = effectManager.DisposeEnemyPosition(BattleEnemy.Monster[ai.EnemyID].transform);
//		position.Add(temp);
//		temp = effectManager.DisposeEnemyPosition(BattleEnemy.Monster[ai.EnemyID].transform);
//		position.Add(temp);
//		IEffectBehavior mlabe = new MoveLineAndBoomEffect ();
//		mlabe.EffectAssetList = tempGo;
//		mlabe.Excute (position);
//	}
//}
