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
		audioItem.name 				= "active skill";
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
		audioItem.name 				= "back attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 32;
		audioItem.name 				= "boss battle";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 34;
		audioItem.name 				= "card swallow";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 37;
		audioItem.name 				= "check role";
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
		audioItem.name 				= "count down";
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
		audioItem.name 				= "door open";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 26;
		audioItem.name 				= "drag tile";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 5;
		audioItem.name 				= "dungeon ready";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 29;
		audioItem.name 				= "enemy attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	
		
		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 15;
		audioItem.name 				= "enemy battle";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 30;
		audioItem.name 				= "enemy die";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 5;
		audioItem.name 				= "explore done";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 7;
		audioItem.name 				= "explore go";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 6;
		audioItem.name 				= "explore ready";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 20;
		audioItem.name 				= "fire attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 16;
		audioItem.name 				= "first attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 31;
		audioItem.name 				= "get chess";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 35;
		audioItem.name 				= "get exp";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 13;
		audioItem.name 				= "get key";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 11;
		audioItem.name 				= "get treasure";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 10;
		audioItem.name 				= "grid turn";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 0;
		audioItem.name 				= "home";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 28;
		audioItem.name 				= "hp recover";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 36;
		audioItem.name 				= "level up";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 22;
		audioItem.name 				= "light attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 12;
		audioItem.name 				= "trigger trap";
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
		audioItem.name 				= "walk hurt";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 19;
		audioItem.name 				= "water attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 21;
		audioItem.name 				= "wind attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		audioItem= new AudioConfigItem();
		audioItem.version 			= 1;
		audioItem.id 				= 24;
		audioItem.name 				= "zero attack";
		audioItem.resourcePath 		= basePath + audioItem.name;
		audioItem.type 				= EPlayType.ONCE;
		audioList.Add(audioItem);	

		Debug.Log("Audio's configging has completed!, the audio clip's COUNT is: " + audioList.Count);
	}
}
