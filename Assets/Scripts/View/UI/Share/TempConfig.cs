using UnityEngine;
using System.Collections.Generic;

public class TempConfig
{
	public const string storyTextureSourcePath = "Quest/StoryQuests/";
	public const string eventTextureSourcePath = "Quest/EventQuests/";
	public const string questItemSourcePath = "Quest/QuestScrollerItem/";

	public static Dictionary< int, string > playerUnitDic = new Dictionary< int, string>();
	public static Dictionary< int, string > eventQuestDic = new Dictionary<int, string>();
	public static Dictionary< int, string > storyQuestDic = new Dictionary<int, string>();
	public static Dictionary< string, string > unitAvatarSprite = new Dictionary<string, string>();
	
	public static void InitUnitAvatarSprite() {
		unitAvatarSprite.Add( "role001","role001");
		unitAvatarSprite.Add( "role002","role002");

		unitAvatarSprite.Add( "role003","role003");
		unitAvatarSprite.Add( "role004","role004");

		unitAvatarSprite.Add( "role005","role005");
		unitAvatarSprite.Add( "role006","role006");

		unitAvatarSprite.Add( "role007","role007");
		unitAvatarSprite.Add( "role008","role008");

		unitAvatarSprite.Add( "role009","role009");
		unitAvatarSprite.Add( "role010","role010");


		unitAvatarSprite.Add( "role011","role011");
		unitAvatarSprite.Add( "role012","role012");
		
		unitAvatarSprite.Add( "role013","role013");
		unitAvatarSprite.Add( "role014","role014");
		
		unitAvatarSprite.Add( "role015","role015");
		unitAvatarSprite.Add( "role016","role016");
		
		unitAvatarSprite.Add( "role017","role017");
		unitAvatarSprite.Add( "role018","role018");
		
		unitAvatarSprite.Add( "role019","role019");
		unitAvatarSprite.Add( "role020","role020");


	}


	public static void InitPlayerUnits() {
		playerUnitDic.Add( 1,"role001");
		playerUnitDic.Add( 2,"role002");
		playerUnitDic.Add( 3,"role003");
		//Debug.LogError("Player's Unit Count:  " + playerUnitDic.Count);
	}
	
	public static void InitStoryQuests() {
		storyQuestDic.Add( 1,"water");
	}

	public static void InitEventQuests() {
		eventQuestDic.Add( 1,"fire");
	}
}
