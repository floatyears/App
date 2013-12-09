using UnityEngine;
using System.Collections;

public class DGTools
{
	public int RandomToInt(int min,int max)
	{
		return Random.Range(min,max);
	}



	
}

public class GameLayer
{

	public static LayerMask Default = 0;
	
	public static LayerMask TransparentFX = 1;
	
	public static LayerMask IgnoreRaycast = 2;
	
	//null = 3,
	
	public static LayerMask Water = 4;
	
	//null = 5,
	
	//null = 6,
	
	//null = 7,
	
	public static LayerMask ActorCard = 8;
	
	public static LayerMask BattleCard = 9;

}
