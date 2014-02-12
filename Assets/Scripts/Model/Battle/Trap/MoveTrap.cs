using UnityEngine;
using System.Collections;
using bbproto;

public class MoveTrap : TrapBase, ITrapExcute{
	public MoveTrap(object instance) : base (instance) {

	}

	public void Excute () {
		TrapInfo ti = DeserializeData<TrapInfo> ();
		MapConfig mc;
		Coordinate cd;
		switch (ti.effectType) {
		case 1:
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, null);
			break;
		case 2:
			mc = ModelManager.Instance.GetData(ModelEnum.MapConfig,new ErrorMsg()) as MapConfig;
			int xCoor = Random.Range(0, mc.mapXLength);
			int yCoor = Random.Range(0, mc.mapYLength);

			cd = new Coordinate(xCoor,yCoor);
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, cd);
			break;
		case 3:
			mc = ModelManager.Instance.GetData(ModelEnum.MapConfig,new ErrorMsg()) as MapConfig;
			cd = new Coordinate(mc.characterInitCoorX, mc.characterInitCoorY);
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, cd);
			break;
		}
	}
}
