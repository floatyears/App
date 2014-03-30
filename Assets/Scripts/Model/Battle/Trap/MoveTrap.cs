﻿using UnityEngine;
using System.Collections;
using bbproto;

public class MoveTrap : TrapBase, ITrapExcute{
//	private TrapInfo instance;
	public MoveTrap(object instance) : base (instance) {
//		this.instance = instance as TrapInfo;
	}

	public void Excute () {
//		TrapInfo ti = DeserializeData<TrapInfo> ();
//		MapConfig mc;
		Coordinate cd;
		switch (instance.effectType) {
		case 1:
//			ViewManager.Instance.TrapLabel.text = "move trap,return prev step ";
//			Debug.LogError("MoveTrap");
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, null);
			break;
		case 2:
//			ViewManager.Instance.TrapLabel.text = "move trap,random position ";
//			mc = ModelManager.Instance.GetData(ModelEnum.MapConfig,new ErrorMsg()) as MapConfig;
			int xCoor = Random.Range(0, MapConfig.MapWidth);
			int yCoor = Random.Range(0, MapConfig.MapHeight);
			cd = new Coordinate(xCoor,yCoor);
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, cd);
//			Debug.LogError("random position TrapMove");
			break;
		case 3:
//			ViewManager.Instance.TrapLabel.text = "move trap,return start position ";
//			mc = ModelManager.Instance.GetData(ModelEnum.MapConfig,new ErrorMsg()) as MapConfig;
			cd = new Coordinate(MapConfig.characterInitCoorX, MapConfig.characterInitCoorY);
			MsgCenter.Instance.Invoke(CommandEnum.TrapMove, cd);
			break;
		}
	}
}
