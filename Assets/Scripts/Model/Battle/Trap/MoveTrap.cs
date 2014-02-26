using UnityEngine;
using System.Collections;
using bbproto;

public class MoveTrap : TrapBase, ITrapExcute{
//	private TrapInfo instance;
	public MoveTrap(object instance) : base (instance) {
//		this.instance = instance as TrapInfo;
	}

	public void Excute () {
//		TrapInfo ti = DeserializeData<TrapInfo> ();
		MapConfig mc;
		Coordinate cd;
		switch (instance.effectType) {
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
