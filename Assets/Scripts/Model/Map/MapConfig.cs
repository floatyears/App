using UnityEngine;
using System.Collections;

public class MapConfig 
{
	public int mapXLength;
	public int mapYLength;
	
	public SingleMapData[,] mapData;
	
	public int characterInitCoorX = 2;
	public int characterInitCoorY = 0;
	
	public MapConfig ()
	{
		mapXLength = 5;
		mapYLength = 5;
		
		mapData = new SingleMapData[mapXLength,mapYLength];

		for (int i = 0; i < mapXLength; i++) 
		{
			for (int j = 0; j < mapYLength; j++) 
			{
				SingleMapData smd = new SingleMapData();
				smd.StarLevel = Random.Range(0,5);
				smd.CoordinateX = i;
				smd.CoordinateY = j;
				
				for (int k = 0; k < smd.StarLevel; k++) 
				{
					smd.MonsterID.Add(k);
				}
				
				mapData[i,j] = smd;
			}	
		}

		mapData[characterInitCoorX,characterInitCoorY].MonsterID.Clear();   
	}
}
