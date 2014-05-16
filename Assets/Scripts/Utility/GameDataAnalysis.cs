using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;

public static class GameDataAnalysis {


	public static void Event(GameDataAnalysisEventType eventId, string value){
		GA.Event (eventId.ToString(),value);
	}

	public static void Event(GameDataAnalysisEventType eventId, float value){
		GA.Event (eventId.ToString(),value.ToString());
	}

	public static void Event(GameDataAnalysisEventType eventId, int value){
		GA.Event (eventId.ToString(),value.ToString());
	}

	public static void Event(GameDataAnalysisEventType eventId, Dictionary<string, string> value){
		GA.Event (eventId.ToString(),value);
	}

	public static void EventBegin(GameDataAnalysisEventType eventId){
		
	}

	public static void EventBegin(GameDataAnalysisEventType eventId,string value){

	}

	public static void EventBegin(GameDataAnalysisEventType eventId,int value){
		
	}

	public static void EventBegin(GameDataAnalysisEventType eventId,float value){
		
	}

	public static void EventEnd(GameDataAnalysisEventType eventId,string value){
		eventId.ToString ();
	}
}

public enum GameDataAnalysisEventType{

}
