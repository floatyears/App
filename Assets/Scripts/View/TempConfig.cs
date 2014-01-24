using UnityEngine;
using System.Collections.Generic;

public class TempConfig
{
	public const string storyTextureSourcePath = "Quest/StoryQuests/";
	public const string eventTextureSourcePath = "Quest/EventQuests/";
	public const string questItemSourcePath = "Quest/QuestScrollerItem/";

	public static Dictionary< int, string > playerUnitDic = new Dictionary< int, string>();

	public static void InitPlayerUnits() 
	{
		playerUnitDic.Add( 1,"role001");
		playerUnitDic.Add( 2,"role002");
		playerUnitDic.Add( 3,"role003");

		//Debug.LogError("Player's Unit Count:  " + playerUnitDic.Count);
	}



	public static Dictionary< int, string > storyQuestDic = new Dictionary<int, string>();

	public static void InitStoryQuests()
	{
		storyQuestDic.Add( 1,"water");
	}

	public static Dictionary< int, string > eventQuestDic = new Dictionary<int, string>();
	
	public static void InitEventQuests()
	{
		eventQuestDic.Add( 1,"fire");
	}
}
