using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;

public static class GameDataAnalysis {

	public static Dictionary<GameDataAnalysisEventType,int> EventTime = new Dictionary<GameDataAnalysisEventType, int>();

	public static void Event(GameDataAnalysisEventType eventId,string? key = null,float? value1 = null, int? value2 = null,string? value3 = null,Dictionary<string, string> value = null){
		GA.Event (eventId.ToString(),key);
	}

	public static void EventBegin(GameDataAnalysisEventType eventId,string? key = null,float? value1 = null, int? value2 = null,string? value3 = null,Dictionary<string, string> value = null){
		EventTime [GameDataAnalysisEventType] = TimeHelper.MillionSecondsNow();
	}

	public static void EventEnd(GameDataAnalysisEventType eventId,string? key = null,float? value1 = null, int? value2 = null,string? value3 = null,Dictionary<string, string> value = null){
		EventTime [GameDataAnalysisEventType] = TimeHelper.MillionSecondsNow();
	}
	
}

public enum GameDataAnalysisEventType{

}
