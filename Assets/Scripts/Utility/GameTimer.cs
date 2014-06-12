using UnityEngine;
using System.Collections.Generic;
using System;

public class GameTimer : MonoBehaviour {
	private static GameTimer instance;
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
			}
			else{
				countDown[i].countDownTime -= Time.deltaTime;
			}
		}
	}

	private List<CountDownUtility> freeCountDown = new List<CountDownUtility> ();

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
		} 
		else {
			return false;
		}
	}

	void CountDownDone(CountDownUtility countDownUtility) {
		countDown.Remove (countDownUtility);
		countDownUtility.callback ();
		if (freeCountDown.Count < 10) {
			freeCountDown.Add (countDownUtility);	
		}
	}

	CountDownUtility AllocationCountDown (float time, Callback callback) {
		CountDownUtility temp = null;
		if (freeCountDown.Count > 0) {
			temp = freeCountDown[0];
			freeCountDown.RemoveAt(0);
		} 
		else {
			temp = new CountDownUtility();
		}

		temp.countDownTime = time;
		temp.callback = callback;
		return temp;
	}

	public uint Seconds = 0;
	private float addSeconds = 0f;
	private bool startTime = false;

	public const uint TenMinuteSeconds = 600;

	public void InitDateTime(uint seconds) {
		Seconds = seconds;
		addSeconds = 0;
		startTime = true;
	}

	public uint GetCurrentSeonds() {
		return Seconds += (uint)addSeconds;
	}

	public DateTime GenerateTimeBySeconds() {
		return ChangeSecondsToTime ( GetCurrentSeonds () );
	}

	public DateTime ChangeSecondsToTime(uint seconds) {
		DateTime dt = new DateTime (1970, 1, 1);
		DateTime currentTime = dt.AddSeconds (Seconds);
		return currentTime;
	}

//	public uint ChangeNowTimeToSeconds() {
//		DateTime dt = DateTime.Now;
//		dt - new DateTime(1970,1,1);
//	}
}

public class CountDownUtility {
	public float countDownTime = 0f;
	public Callback callback;
}
