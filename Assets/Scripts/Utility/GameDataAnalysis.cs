using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;

public static class GameDataAnalysis {

	public static Dictionary<GameDataAnalysisEventType,int> EventTime = new Dictionary<GameDataAnalysisEventType, int>();

	public static void Event(GameDataAnalysisEventType eventId){
		GA.Event (eventId.ToString());
	}

	public static void Event(GameDataAnalysisEventType eventId, string key){
		GA.Event (eventId.ToString (), key);
	}

	public static void Event(GameDataAnalysisEventType eventId, Dictionary<string,string> attr){
		GA.Event (eventId.ToString (), attr);
	}

	
	public static void EventCount(GameDataAnalysisEventType eventId,string key,Dictionary<string,string> attr){
		GA.Event (eventId.ToString (), key, attr);
	}

	public static void EventCount(GameDataAnalysisEventType eventId, Dictionary<string,string> attr, int count){
		GA.Event (eventId.ToString (), attr, count);
	}


	public static void EventBegin(GameDataAnalysisEventType eventId){
		EventTime [eventId] = TimeHelper.MillionSecondsNow();
	}

	public static void EventEnd(GameDataAnalysisEventType eventId,Dictionary<string,string> value){
		EventTime [eventId] = TimeHelper.MillionSecondsNow();
	}

	public static void Buy(string item,int amount,double price){
		GA.Buy(item,amount,price);
	}

	public static void Pay(double cash,GA.PaySource source,double coin){
		GA.Pay(cash,source,coin);
	}
	
}

public enum GameDataAnalysisEventType{
	Login
}
