using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigAudio {
//	private ;
//	public static ;
	public ConfigAudio(){
		AudioConfigFile audioConfigfile = new AudioConfigFile();
//		audioList = ;
		Config(audioConfigfile.audioConfig);
	}

	private void Config(List<AudioConfigItem> audioList){

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

//		audioItem= new AudioConfigItem();
//		audioItem.version 			= 1;
//		audioItem.id 					= 2;
//		audioItem.name 				= "bgm_002_enemy_battle";
//		audioItem.resourcePath 	= basePath + audioItem.name;
//		audioItem.type 				= EPlayType.LOOP;
//		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 3;
		audioItem.name 				= "bgm_003_boss_battle";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.LOOP;
		audioList.Add(audioItem);
	
//		audioItem= new AudioConfigItem();
//		audioItem.version 			= 1;
//		audioItem.id 					= 4;
//		audioItem.name 				= "bgm_004_victory";
//		audioItem.resourcePath 	= basePath + audioItem.name;
//		audioItem.type 				= EPlayType.LOOP;
//		audioList.Add(audioItem);

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

//		audioItem= new AudioConfigItem();
//		audioItem.version 			= 1;
//		audioItem.id 					= 102;
//		audioItem.name 				= "se_102_dungeon_ready";
//		audioItem.resourcePath 	= basePath + audioItem.name;
//		audioItem.type 				= EPlayType.ONCE;
//		audioList.Add(audioItem);	

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
		audioItem.name 				= "se_104_click_invalid";
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
		audioItem.name 				= "se_106_chess_fall";
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
		audioItem.name 				= "se_111_click_success";
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
		audioItem.name 				= "se_116_card_4";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 117;
		audioItem.name 				= "se_117_title_invalid";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 118;
		audioItem.name 				= "se_118_title_success";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
			
//		audioItem= new AudioConfigItem();
//		audioItem.version 			= 1;
//		audioItem.id 					= 119;
//		audioItem.name 				= "se_119_text";
//		audioItem.resourcePath 	= basePath + audioItem.name;
//		audioItem.type 				= EPlayType.ONCE;
//		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 120;
		audioItem.name 				= "se_120_text_appear";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 121;
		audioItem.name 				= "se_121_battle_over";
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
		audioItem.type 				= EPlayType.LOOP;
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
		audioItem.name 				= "se_135_attack_increase_1";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 136;
		audioItem.name 				= "se_136_attack_increase_2";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 137;
		audioItem.name 				= "se_137_attack_increase_3";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 138;
		audioItem.name 				= "se_138_attack_increase_4";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 139;
		audioItem.name 				= "se_139_attack_increase_5";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 140;
		audioItem.name 				= "se_140_attack_increase_6";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 141;
		audioItem.name 				= "se_141_attack_increase_7";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 142;
		audioItem.name 				= "se_142_as_appear";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 143;
		audioItem.name 				= "se_143_as_fly";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 144;
		audioItem.name 				= "se_144_quest_clear";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 145;
		audioItem.name 				= "se_145_sp_limited_over";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 146;
		audioItem.name 				= "se_146_rank_up";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 147;
		audioItem.name 				= "se_147_friend_up";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 148;
		audioItem.name 				= "se_148_star_appear";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 149;
		audioItem.name 				= "se_149_sp_recover";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 150;
		audioItem.name 				= "se_150_tile_overlap";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 151;
		audioItem.name 				= "se_151_game_over";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 152;
		audioItem.name 				= "se_152_card_evo";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 153;
		audioItem.name 				= "se_153_ns_single1";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 154;
		audioItem.name 				= "se_154_ns_single2";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 155;
		audioItem.name 				= "se_155_as_all1";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 156;
		audioItem.name 				= "se_156_as_all2";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 157;
		audioItem.name 				= "se_157_as_single1";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 158;
		audioItem.name 				= "se_158_as_single2";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 159;
		audioItem.name 				= "se_159_ns_all1";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 160;
		audioItem.name 				= "se_160_ns_all2";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 161;
		audioItem.name 				= "se_161_as_single1_blood";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 162;
		audioItem.name 				= "se_162_as_delay";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 163;
		audioItem.name 				= "se_163_as_slow";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 164;
		audioItem.name 				= "se_164_as_def_down";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 165;
		audioItem.name 				= "se_165_as_poison";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 166;
		audioItem.name 				= "se_166_as_color_change";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 167;
		audioItem.name 				= "se_167_as_damage_down";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 168;
		audioItem.name 				= "se_168_ps_counter";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 169;
		audioItem.name 				= "se_169_ls_chase";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 170;
		audioItem.name 				= "se_170_ls_activate";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 171;
		audioItem.name 				= "se_171_as_activate";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 					= 172;
		audioItem.name 				= "se_172_ps_dodge_trap";
		audioItem.resourcePath 	= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);

		DataCenter.Instance.ConfigAudioList = audioList;;
//		Debug.Log("Audio's configging has completed!, the audio clip's COUNT is: " + audioList.Count);
	}
}
