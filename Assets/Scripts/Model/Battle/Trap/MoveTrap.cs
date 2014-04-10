using UnityEngine;
using System.Collections;
using bbproto;

public class MoveTrap : TrapBase, ITrapExcute{
//	private TrapInfo instance;
	public MoveTrap(object instance) : base (instance) {
//		this.instance = instance as TrapInfo;
	}

	public void Excute () {
		Coordinate cd;
		switch (instance.effectType) {
		case 1:
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, null);
			break;
		case 2:
			int xCoor = Random.Range(0, MapConfig.MapWidth);
			int yCoor = Random.Range(0, MapConfig.MapHeight);
			cd = new Coordinate(xCoor,yCoor);
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, cd);
			break;
		case 3:
			cd = new Coordinate(MapConfig.characterInitCoorX, MapConfig.characterInitCoorY);
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, cd);
			break;
		}
	}
}
