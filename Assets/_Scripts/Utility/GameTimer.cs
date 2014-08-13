﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class GameTimer : MonoBehaviour {
	private static GameTimer instance;

	public uint recovertime;


	public static GameTimer GetInstance() {
		if(instance == null) {
			instance = FindObjectOfType(typeof(GameTimer)) as GameTimer;
		}
		return instance;
	}

	void Awake () {
		instance = this;
	} 

	void Update () {
		if (startTime) {
			addSeconds += Time.deltaTime;
		}

		if (countDown.Count == 0) {
			return;	
		}

		for (int i = 0; i < countDown.Count; i++) {
			if(countDown[i].countDownTime < 0f) {
				CountDownDone(countDown[i]);
			} else {
				countDown[i].countDownTime -= Time.deltaTime;
			}
		}
	}

	private Queue<CountDownUtility> freeCountDown = new Queue<CountDownUtility> ();

	private List<CountDownUtility> countDown = new List<CountDownUtility> ();

	public void AddCountDown (float time, Callback callback) {
		if (time < 0 || callback == null) {
			return;		
		}

		CountDownUtility task = AllocationCountDown (time,callback);
		countDown.Add (task);
	}

	public bool ExitCountDonw(Callback callback) {
		CountDownUtility cdu = countDown.Find( a=>a.callback == callback) ;
		if (cdu != null) {
			countDown.Remove (cdu);
			return true;
		} else {
			return false;
		}
	}

	void CountDownDone(CountDownUtility countDownUtility) {
		countDown.Remove (countDownUtility);
		countDownUtility.callback ();
		if (freeCountDown.Count < 10) {
			freeCountDown.Enqueue (countDownUtility);	
		}
	}

	CountDownUtility AllocationCountDown (float time, Callback callback) {
		CountDownUtility temp = null;
		if (freeCountDown.Count > 0) {
			temp = freeCountDown.Dequeue();
//			freeCountDown.RemoveAt(0);
		} else {
			temp = new CountDownUtility();
		}

		temp.countDownTime = time;
		temp.callback = callback;
		return temp;
	}

	public uint Seconds = 0;
	private float addSeconds = 0f;
	private bool startTime = false;

	public const uint TenMinuteSeconds = 3600;

	public DateTime currentDateTime;

	public void InitDateTime(uint seconds) {
		Seconds = seconds;
		addSeconds = 0;
		startTime = true;

		currentDateTime = GenerateTimeBySeconds ();
	}

	public void CheckRefreshServer() {
		if (!CheckDay ()) {
			RequestLoginToServer.Login();
		}
	}

	public bool CheckDay() {
		DateTime dt = GenerateTimeBySeconds ();
		if (currentDateTime.Day != dt.Day) {
			return false;
		}

		return true;
	}

	public uint GetCurrentSeonds() {
		uint currentTime = Seconds + (uint)addSeconds;
		return currentTime;
	}

	public DateTime GenerateTimeBySeconds() {
		return ChangeSecondsToTime ( GetCurrentSeonds () );
	}

	public DateTime ChangeSecondsToTime(uint seconds) {
		DateTime dt = new DateTime (1970, 1, 1);
		DateTime currentTime = dt.AddSeconds (Seconds);
		return currentTime;
	}

	public static string GetTimeBySeconds(uint seconds){
		int hr = (int)(seconds / 3600);
		int min = (int)(seconds % 3600 / 60);
		int sec = (int)(seconds %60);
		return hr + ":" + min + ":" + sec;
	}

	public static string GetMinSecBySeconds(uint seconds){
		int min = (int)(seconds % 3600 / 60);
		int sec = (int)(seconds %60);
		return ((min < 10) ? ("0"+ min) : "" + min) + ":" + ((sec < 10) ? ("0"+ sec) : "" + sec);
	}

	public static string GetFormatRemainTime(uint seconds){
		uint hr = seconds / 3600;
		uint min = seconds % 3600 / 60;
		uint sec = seconds % 60;

		if (hr > 23) {
			return (uint)hr / 24 + TextCenter.GetText ("Time_Day");// + (uint)hr % 24 + TextCenter.GetText ("Time_Hour");
		} else if (hr > 0) {
			return hr + TextCenter.GetText ("Time_Hour");// + min + TextCenter.GetText ("Time_Min");
		} else {
			return min + TextCenter.GetText ("Time_Min");
		}
	}
}

public class CountDownUtility {
	public float countDownTime = 0f;
	public Callback callback;
}
