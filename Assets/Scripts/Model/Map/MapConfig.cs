using UnityEngine;
using System.Collections.Generic;

public class MapConfig : IOriginModel
{
	public int mapXLength;
	public int mapYLength;
	
	public SingleMapData[,] mapData;
	
	public int characterInitCoorX = 2;
	public int characterInitCoorY = 0;

	private List<string> mapItemPath = new List<string>();

	private int mapID;

	public string GetMapPath()
	{
		int index = Random.Range (1, mapItemPath.Count);
		return mapItemPath [index];
	}

	public ErrorMsg SerializeData (object instance)
	{
		throw new System.NotImplementedException ();
	}

	public object DeserializeData ()
	{
		throw new System.NotImplementedException ();
	}

	public MapConfig ()
	{
		ConfigTrap ct = new ConfigTrap ();
		mapXLength = 5;
		mapYLength = 5;
		mapID = 1;
		mapData = new SingleMapData[mapXLength,mapYLength];
		for (int i = 0; i < mapXLength; i++) 
		{
			for (int j = 0; j < mapYLength; j++) 
			{
				SingleMapData smd = new SingleMapData();
				smd.StarLevel = Random.Range(0,5);
				smd.CoordinateX = i;
				smd.CoordinateY = j;
				smd.ContentType = MapItemEnum.Enemy;
//				for (int k = 0; k < smd.StarLevel; k++) 
//				{
					smd.MonsterID.Add(1);
//					smd.MonsterID.Add(2);
//				}
				
				mapData[i,j] = smd;
			}	
		}

//		SingleMapData singleMapItem = mapData [2, 1];
//		singleMapItem.ContentType = MapItemEnum.Trap;
//		singleMapItem.TypeValue = 1;
//		singleMapItem = mapData [2, 2];
//		singleMapItem.ContentType = MapItemEnum.Trap;
//		singleMapItem.TypeValue = 5;
//		singleMapItem = mapData [2, 3];
//		singleMapItem.ContentType = MapItemEnum.Trap;
//		singleMapItem.TypeValue = 6;
		SingleMapData singleMapItem = mapData [1, 0];
		singleMapItem.ContentType = MapItemEnum.Trap;
		singleMapItem.TypeValue = 2;

		mapData[characterInitCoorX,characterInitCoorY].MonsterID.Clear();   
		mapData [characterInitCoorX, characterInitCoorY].ContentType = MapItemEnum.None;
		mapData [2, 4].MonsterID.Clear ();
		mapData [2, 4].MonsterID.Add (100);
		for (int i = 1; i < 4; i++)
		{
			mapItemPath.Add("Texture/fight_sprites/map_"+mapID+"_"+i);
		}

	}
}

