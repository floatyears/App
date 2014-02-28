using UnityEngine;
using System.Collections.Generic;

public class MapConfig : IOriginModel {
	public int mapXLength;
	public int mapYLength;
	
	public SingleMapData[,] mapData;
	
	public int characterInitCoorX = 2;
	public int characterInitCoorY = 0;
	public const int endPointX = 2;
	public const int endPointY = 4;
	public List<uint> BossID = new List<uint> () {3};
	private List<string> mapItemPath = new List<string>();
	private int mapID;

	public string GetMapPath() {
		int index = Random.Range (1, mapItemPath.Count);
		return mapItemPath [index];
	}

	public ErrorMsg SerializeData (object instance) {
		throw new System.NotImplementedException ();
	}

	public object DeserializeData () {
		throw new System.NotImplementedException ();

	}

	public MapConfig () {
		ConfigTrap ct = new ConfigTrap ();
		mapXLength = 5;
		mapYLength = 5;
		mapID = 1;
		mapData = new SingleMapData[mapXLength,mapYLength];
		for (int i = 0; i < mapXLength; i++) {
			for (int j = 0; j < mapYLength; j++) {
				SingleMapData smd = new SingleMapData();
				smd.StarLevel = Random.Range(0,5);
				smd.CoordinateX = i;
				smd.CoordinateY = j;
				smd.ContentType = MapItemEnum.Enemy;
//				for (int k = 0; k < smd.StarLevel; k++) 
//				{
					smd.MonsterID.Add(1);
					smd.MonsterID.Add(2);
					smd.MonsterID.Add(3);
//					smd.MonsterID.Add(2);
//					smd.MonsterID.Add(3);
				//				}
				
				mapData[i,j] = smd;
			}	
		}

		SingleMapData singleMapItem = mapData [1, 0];
		singleMapItem.ContentType = MapItemEnum.Trap;
		singleMapItem.TypeValue = 2;
		singleMapItem = mapData [1, 1];
		singleMapItem.ContentType = MapItemEnum.Trap;
		singleMapItem.TypeValue = 1;
		singleMapItem = mapData [2, 1];
		singleMapItem.ContentType = MapItemEnum.Coin;
		singleMapItem.TypeValue = 0;
		singleMapItem = mapData [2, 0];
		singleMapItem.ContentType = MapItemEnum.Start;
		mapData[characterInitCoorX,characterInitCoorY].MonsterID.Clear();   
//		mapData [2, 4].MonsterID.Clear ();
//		mapData [2, 4].MonsterID.Add (100);
		for (int i = 1; i < 4; i++) {
			mapItemPath.Add("Texture/fight_sprites/map_"+mapID+"_"+i);
		}

	}
}

