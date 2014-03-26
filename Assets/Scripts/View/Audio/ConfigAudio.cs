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
		audioItem.id 					= 0;
		audioItem.name 				= "bgm_000_home";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.LOOP;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 1;
		audioItem.name 				= "bgm_001_dungeon";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.LOOP;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 2;
		audioItem.name 				= "bgm_002_enemy_battle";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.LOOP;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 3;
		audioItem.name 				= "bgm_003_boss_battle";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.LOOP;
		audioList.Add(audioItem);
	
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 4;
		audioItem.name 				= "bgm_004_victory";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.LOOP;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 100;
		audioItem.name 				= "se_100_click";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem = new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 101;
		audioItem.name 				= "se_101_back";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 102;
		audioItem.name 				= "se_102_dungeon_ready";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 103;
		audioItem.name 				= "se_103_quest_ready";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);
		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 104;
		audioItem.name 				= "se_104_quest_go";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 105;
		audioItem.name 				= "se_105_walk";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 106;
		audioItem.name 				= "se_106_walk_hurt";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 107;
		audioItem.name 				= "se_107_grid_turn";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);
		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 108;
		audioItem.name 				= "se_108_get_treasure";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 109;
		audioItem.name 				= "se_109_trigger_trap";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 110;
		audioItem.name 				= "se_110_get_key";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 111;
		audioItem.name 				= "se_111_door_open";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 112;
		audioItem.name 				= "se_112_enemy_battle";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);
		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 113;
		audioItem.name 				= "se_113_first_attack";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem = new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 114;
		audioItem.name 				= "se_114_back_attack";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
		
		audioItem = new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 115;
		audioItem.name 				= "se_115_active_skill";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 116;
		audioItem.name 				= "se_116_water_attack";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 117;
		audioItem.name 				= "se_117_fire_attack";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 118;
		audioItem.name 				= "se_118_wind_attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
			
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 119;
		audioItem.name 				= "se_119_light_attack";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 120;
		audioItem.name 				= "se_120_dark_attack";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 121;
		audioItem.name 				= "se_121_zero_attack";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
			
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 122;
		audioItem.name 				= "se_122_count_down";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 123;
		audioItem.name 				= "se_123_drag_tile";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 124;
		audioItem.name 				= "se_124_combo";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 125;
		audioItem.name 				= "se_125_hp_recover";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
			
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 126;
		audioItem.name 				= "se_126_enemy_attack";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 127;
		audioItem.name 				= "se_127_enemy_die";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 128;
		audioItem.name 				= "se_128_get_chess";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 129;
		audioItem.name 				= "se_129_boss_battle";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem = new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 130;
		audioItem.name 				= "se_130_devour_unit";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 131;
		audioItem.name 				= "se_131_get_exp";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 132;
		audioItem.name 				= "se_132_level_up";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
			
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 133;
		audioItem.name 				= "se_133_check_role";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 134;
		audioItem.name 				= "se_134_sold_out";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 135;
		audioItem.name 				= "se_135_use_chip";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

//		Debug.Log("Audio's configging has completed!, the audio clip's COUNT is: " + audioList.Count);
	}
}
