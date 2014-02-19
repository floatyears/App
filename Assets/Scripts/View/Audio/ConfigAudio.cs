using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigAudio {
	private AudioConfigFile audioConfigfile;
	public static List<AudioConfigItem> audioList;
	public ConfigAudio(){
		audioConfigfile = new AudioConfigFile();
		audioList = audioConfigfile.audioConfig;
		Config();
	}

	private void Config(){
		AudioConfigItem audioItem;
		string basePath = "Audio/";

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 18;
		audioItem.name 				= "active_skill";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 4;
		audioItem.name 				= "back";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 17;
		audioItem.name 				= "back_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 32;
		audioItem.name 				= "boss_battle";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 34;
		audioItem.name 				= "card_swallow";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 37;
		audioItem.name 				= "check_role";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 3;
		audioItem.name 				= "click";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 27;
		audioItem.name 				= "combo";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 25;
		audioItem.name 				= "count_down";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 23;
		audioItem.name 				= "dark attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 14;
		audioItem.name 				= "door_open";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 26;
		audioItem.name 				= "drag_tile";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 5;
		audioItem.name 				= "dungeon_ready";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 29;
		audioItem.name 				= "enemy_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 15;
		audioItem.name 				= "enemy_battle";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 30;
		audioItem.name 				= "enemy_die";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 5;
		audioItem.name 				= "explore_done";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 7;
		audioItem.name 				= "explore_go";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 6;
		audioItem.name 				= "explore_ready";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 20;
		audioItem.name 				= "fire_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 16;
		audioItem.name 				= "first_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 31;
		audioItem.name 				= "get_chess";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 35;
		audioItem.name 				= "get_exp";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 13;
		audioItem.name 				= "get_key";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 11;
		audioItem.name 				= "get_treasure";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 10;
		audioItem.name 				= "grid_turn";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 0;
		audioItem.name 				= "home";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.LOOP;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 28;
		audioItem.name 				= "hp_recover";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 36;
		audioItem.name 				= "level_up";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 22;
		audioItem.name 				= "light_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 12;
		audioItem.name 				= "trigger_trap";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 8;
		audioItem.name 				= "walk";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 9;
		audioItem.name 				= "walk_hurt";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 19;
		audioItem.name 				= "water_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 21;
		audioItem.name 				= "wind_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 24;
		audioItem.name 				= "zero_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

//		Debug.Log("Audio's configging has completed!, the audio clip's COUNT is: " + audioList.Count);
	}
}
