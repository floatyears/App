using UnityEngine;
using System.Collections.Generic;
using System;
using bbproto;

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
//			RequestLoginToServer.Login();
			UserController.Instance.Login(LoginSuccess);
		}
	}

	void LoginSuccess(object data) {
		if (data != null) {
			RspAuthUser rspAuthUser = data as bbproto.RspAuthUser;
			if (rspAuthUser == null) {
				Debug.LogError("authUser response rspAuthUser == null");
				return;
			}
			
			if (rspAuthUser.header.code != 0) {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspAuthUser.header.code);
				Debug.LogError("rspAuthUser return code: "+rspAuthUser.header.code+" error:" + rspAuthUser.header.error);
				return;
			}
			
			uint userId = rspAuthUser.user.userId;
			
			if (rspAuthUser.isNewUser == 1) {
				LogHelper.Log("New user registeed, save userid:" + userId);
				GameDataPersistence.Instance.StoreData(GameDataPersistence.USER_ID, rspAuthUser.user.userId);
			}
			
			//TODO: update localtime with servertime
			//localTime = rspAuthUser.serverTime
			
			//save to GlobalData
			GameTimer.GetInstance().InitDateTime(rspAuthUser.serverTime);
			
			if (rspAuthUser.account != null) {
				DataCenter.Instance.UserData.AccountInfo = rspAuthUser.account;
			}
			
			if (rspAuthUser.user != null) {
				Debug.Log("authUser response userId:" + rspAuthUser.user.userId);
				
				DataCenter.Instance.UserData.UserInfo = rspAuthUser.user;
				if (rspAuthUser.evolveType != null) {
					DataCenter.Instance.UserData.UserInfo.EvolveType = rspAuthUser.evolveType;
				}
			}
			else {
				Debug.LogError("authUser response rspAuthUser.user == null");
			}
			
			if (rspAuthUser.friends != null) {
				List<FriendInfo> supportFriends = new List<FriendInfo>();
				foreach (FriendInfo fi in rspAuthUser.friends) {
					supportFriends.Add(fi);
					DataCenter.Instance.UnitData.UserUnitList.Add(fi.userId, fi.UserUnit.uniqueId, fi.UserUnit);
				}
				DataCenter.Instance.FriendData.AddSupportFriend (supportFriends);
			}
			else {
				Debug.LogError("rsp.friends==null");
			}
			
			DataCenter.Instance.QuestData.EventStageList = new List<StageInfo>();
			if (rspAuthUser.eventList != null) {
				foreach (StageInfo stage in rspAuthUser.eventList) {
					DataCenter.Instance.QuestData.EventStageList.Add(stage);
				}
			}
			
			if (rspAuthUser.unitList != null) {
				foreach (UserUnit unit in rspAuthUser.unitList) {
					//					DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, TUserUnit.GetUserUnit(userId,unit));
					DataCenter.Instance.UnitData.UserUnitList.Add(userId, unit.uniqueId, unit);
				}
				LogHelper.Log("rspAuthUser add to myUserUnit.count: {0}", rspAuthUser.unitList.Count);
			}
			
			if (rspAuthUser.party != null && rspAuthUser.party.partyList != null) {
				DataCenter.Instance.UnitData.PartyInfo = rspAuthUser.party;
				//TODO: replace ModelManager.GetData(UnitPartyInfo) with DataCenter.Instance.UnitData.PartyInfo.CurrentParty
				DataCenter.Instance.SetData(ModelEnum.UnitPartyInfo, DataCenter.Instance.UnitData.PartyInfo.CurrentParty);
			}
			
			if (rspAuthUser.questClear != null) {
				DataCenter.Instance.QuestData.QuestClearInfo = rspAuthUser.questClear;
			}
			
			DataCenter.Instance.UnitData.CatalogInfo = new UnitCatalogInfo(rspAuthUser.meetUnitFlag, rspAuthUser.haveUnitFlag);
			
			if( rspAuthUser.notice != null){
				DataCenter.Instance.CommonData.NoticeInfo = rspAuthUser.notice;
			}
			
			if( rspAuthUser.login != null){
				DataCenter.Instance.UserData.LoginInfo = rspAuthUser.login;
			}
			NoviceGuideStepEntityManager.InitGuideStage(rspAuthUser.userGuideStep);
			
			
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
