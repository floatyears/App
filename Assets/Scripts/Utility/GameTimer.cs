using UnityEngine;
using System.Collections.Generic;

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
}

public class CountDownUtility {
	public float countDownTime = 0f;
	public Callback callback;
}
