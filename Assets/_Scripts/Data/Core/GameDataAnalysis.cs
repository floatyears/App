using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Umeng;

public static class GameDataAnalysis {

	public static Dictionary<GameDataAnalysisEventType,int> EventTime = new Dictionary<GameDataAnalysisEventType, int>();

	public static void Event(GameDataAnalysisEventType eventId){
		GA.Event (eventId.ToString());

		bbproto.EventData ed = new bbproto.EventData ();
		ed.eventId = eventId.ToString ();
		List<bbproto.EventData> data = new List<bbproto.EventData> ();
		data.Add (ed);
		UploadStat.SendRequest (null, data);
	}

	public static void Event(GameDataAnalysisEventType eventId, string key){
		GA.Event (eventId.ToString (), key);

		bbproto.EventData ed = new bbproto.EventData ();
		ed.eventId = eventId.ToString ();
		ed.sValue = key;
		List<bbproto.EventData> data = new List<bbproto.EventData> ();
		data.Add (ed);
		UploadStat.SendRequest (null, data);
	}

	public static void Event(GameDataAnalysisEventType eventId, Dictionary<string,string> attr){
		GA.Event (eventId.ToString (), attr);

		bbproto.EventData ed = new bbproto.EventData ();
		ed.eventId = eventId.ToString ();
		List<bbproto.EventDataParam> eventP = new List<bbproto.EventDataParam> ();
		foreach(var item in attr){
			bbproto.EventDataParam ep = new bbproto.EventDataParam();
			ep.key = item.Key;
			ep.sValue = item.Value;
			eventP.Add(ep);
			//ed.values.Add(item.Key
		}
		ed.values.AddRange (eventP);
		List<bbproto.EventData> data = new List<bbproto.EventData> ();
		data.Add (ed);
		UploadStat.SendRequest (null, data);
	}

	
//	public static void EventCount(GameDataAnalysisEventType eventId,string key,Dictionary<string,string> attr){
//		GA.Event (eventId.ToString (), key, attr);
//	}

	public static void EventCount(GameDataAnalysisEventType eventId, Dictionary<string,string> attr, int count){
		GA.Event (eventId.ToString (), attr, count);

		bbproto.EventData ed = new bbproto.EventData ();
		ed.eventId = eventId.ToString ();
		ed.iValue = count;
		List<bbproto.EventDataParam> eventP = new List<bbproto.EventDataParam> ();
		foreach(var item in attr){
			bbproto.EventDataParam ep = new bbproto.EventDataParam();
			ep.key = item.Key;
			ep.sValue = item.Value;
			eventP.Add(ep);
			//ed.values.Add(item.Key
		}
		ed.values.AddRange(eventP);
		List<bbproto.EventData> data = new List<bbproto.EventData> ();
		data.Add (ed);
		UploadStat.SendRequest (null, data);
	}


	public static void EventBegin(GameDataAnalysisEventType eventId){
		//EventTime [eventId] = Utility.TimeHelper.MillionSecondsNow();


	}

	public static void EventEnd(GameDataAnalysisEventType eventId,Dictionary<string,string> value){
		//EventTime [eventId] = Utility.TimeHelper.MillionSecondsNow();


	}

	public static void Buy(string item,int amount,double price){
		GA.Buy(item,amount,price);

		bbproto.EventData ed = new bbproto.EventData ();
		ed.eventId = "Buy";

		bbproto.EventDataParam ep = new bbproto.EventDataParam();
		ep.key = "amount";
		ep.iValue = amount;
		ed.values.Add(ep);

		bbproto.EventDataParam ep1 = new bbproto.EventDataParam();
		ep1.key = "price";
		ep1.fValue = (float)price;
		ed.values.Add(ep1);

		List<bbproto.EventData> data = new List<bbproto.EventData> ();
		data.Add (ed);
		UploadStat.SendRequest (null, data);
	}

	public static void Pay(double cash,GA.PaySource source,double coin){
		GA.Pay(cash,source,coin);

		bbproto.EventData ed = new bbproto.EventData ();
		ed.eventId = "Pay";
		
		bbproto.EventDataParam ep = new bbproto.EventDataParam();
		ep.key = "source";
		ep.sValue = source.ToString();
		ed.values.Add(ep);
		
		bbproto.EventDataParam ep1 = new bbproto.EventDataParam();
		ep1.key = "coin";
		ep1.fValue = (float)coin;
		ed.values.Add(ep1);

		List<bbproto.EventData> data = new List<bbproto.EventData> ();
		data.Add (ed);
		UploadStat.SendRequest (null, data);
	}
	
}

public enum GameDataAnalysisEventType{
	FirstStart,
	GetVersion,
	DownloadStart,
	DownloadEnd,
	SelectRole,
	Login,
}
