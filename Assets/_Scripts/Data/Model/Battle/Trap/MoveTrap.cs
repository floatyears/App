using UnityEngine;
using System.Collections;
using bbproto;

public class MoveTrap : TrapBase {
//	private TrapInfo instance;
	public MoveTrap(object instance) : base (instance) {
//		this.instance = instance as TrapInfo;
	}

	public override void Excute () {
		Coordinate cd = default(Coordinate);
		switch (instance.effectType) {
		case 1:
			break;
		case 2:
			cd = new Coordinate(Random.Range(0, MapConfig.MapWidth),Random.Range(0, MapConfig.MapHeight));
			break;
		case 3:
			cd = new Coordinate(MapConfig.characterInitCoorX, MapConfig.characterInitCoorY);
			break;
		}
		BattleAttackManager.Instance.TrapMove(cd);
	}
}
