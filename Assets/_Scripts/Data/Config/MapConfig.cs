using UnityEngine;
using System.Collections.Generic;

public class MapConfig {
//	public int mapXLength;
//	public int mapYLength;
	
	public SingleMapData[,] mapData;

	//=========== useful start===================

	public const int characterInitCoorX = 2;
	public const int characterInitCoorY = 0;
	public static Coordinate endCoor = new Coordinate (2, 4);
	public const int MapWidth = 5;
	public const int MapHeight = 5;

	//=========== useful end===================

	public const int endPointX = 2;
	public const int endPointY = 4;
}

