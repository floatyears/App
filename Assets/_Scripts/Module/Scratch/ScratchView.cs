using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ScratchView : ViewBase {

	private GachaType gachaType;
	private int gachaCount;

    private UIButton btnFriendGacha;
    private UIButton btnRareGacha;
    private UIButton btnEventGacha;

	private UILabel rareTimes;
	private UILabel currentFriendPoint;

	private GameObject infoPanelRoot;
	private GameObject windowRoot;
	private GameObject currentRoot;

	private UILabel scratchContent;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config,data);
		InitUI();
	}
	
	public override void ShowUI () {
		currentFriendPoint.text = DataCenter.Instance.UserData.AccountInfo.friendPoint + "";
		base.ShowUI ();
       	
		rareTimes.text = TextCenter.GetText("OneTimesDesc",DataCenter.Instance.GetGachaWillGot5StarCount().ToString()); 



		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.SCRATCH);
	}

	private void InitUI() {
		btnFriendGacha = FindChild<UIButton>("Gacha_Entrance/1");
		FindChild<UILabel> ("Gacha_Entrance/1/Title").text = TextCenter.GetText ("FriendScratch");
		btnRareGacha = FindChild<UIButton>("Gacha_Entrance/2");
		FindChild<UILabel> ("Gacha_Entrance/2/Title").text = TextCenter.GetText ("RareScratch");
		btnEventGacha = FindChild<UIButton>("Gacha_Entrance/3");
		FindChild<UILabel> ("Gacha_Entrance/3/Title").text = TextCenter.GetText ("EventScratch");

        UIEventListenerCustom.Get(btnFriendGacha.gameObject).onClick = OnClickFriendGacha;
        UIEventListenerCustom.Get(btnRareGacha.gameObject).onClick = OnClickRareGacha;
        UIEventListenerCustom.Get(btnEventGacha.gameObject).onClick = OnClickEventGacha;

		FindChild<UILabel>("Gacha_Entrance/1/Times").enabled = false;
		rareTimes = FindChild<UILabel>("Gacha_Entrance/2/Times");
		FindChild<UILabel> ("Gacha_Entrance/3/Times").text = TextCenter.GetText ("NineTimesDesc");
		FindChild<UILabel> ("Gacha_Entrance/1/CardStar").text = TextCenter.GetText ("ScratchStar1-3");
		FindChild<UILabel> ("Gacha_Entrance/3/CardStar").text = FindChild<UILabel> ("Gacha_Entrance/2/CardStar").text = TextCenter.GetText ("ScratchStar4-5");

		FindChild<UILabel> ("Gacha_Entrance/1/CostNum").text = TextCenter.GetText ("ScratchCost1");
		FindChild<UILabel> ("Gacha_Entrance/2/CostNum").text = TextCenter.GetText ("ScratchCost2");
		FindChild<UILabel> ("Gacha_Entrance/3/CostNum").text = TextCenter.GetText ("ScratchCost3");

		FindChild<UILabel> ("Current/Label").text = TextCenter.GetText ("CurrentOwnFriendPoint");

		scratchContent = FindChild<UILabel> ("Notice_Window/Content");
		scratchContent.text = DataCenter.Instance.CommonData.NoticeInfo.GachaNotice;

		infoPanelRoot = transform.FindChild("Notice_Window").gameObject;
		windowRoot = transform.FindChild("Gacha_Entrance").gameObject;
		currentRoot = transform.FindChild ("Current").gameObject;

		currentFriendPoint = FindChild<UILabel> ("Current/CurrentFriendPoint");


		FindChild<UILabel> ("Gacha_Entrance/1/CostLabel").text = FindChild<UILabel> ("Gacha_Entrance/2/CostLabel").text = FindChild<UILabel> ("Gacha_Entrance/3/CostLabel").text = TextCenter.GetText("ScratchCost");

	}

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);
			infoPanelRoot.transform.localPosition = new Vector3(-1000, -300, 0);
			windowRoot.transform.localPosition = new Vector3(1000, -570, 0);
			currentRoot.transform.localPosition = new Vector3(-1000,-775,0);

			 
			iTween.MoveTo(currentRoot, iTween.Hash("x", (100f - currentFriendPoint.localSize.x)/2, "time", 0.4f, "islocal", true));
			iTween.MoveTo(infoPanelRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
			iTween.MoveTo(windowRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
		}else{
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
		}
	}

    private void OnClickFriendGacha(GameObject btn){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		if (DataCenter.Instance.GetAvailableFriendGachaTimes() < 1) {
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FriendGachaFailed"),TextCenter.GetText("GachaFriendPointNotEnough", DataCenter.friendGachaFriendPoint),TextCenter.GetText("Back"));
			return;
		}

		gachaType = GachaType.FriendGacha;
		gachaCount = 1;
		Umeng.GA.Event ("Gacha1",gachaCount+"");
		UnitController.Instance.Gacha(OnRspGacha, (int)gachaType, gachaCount);
    }

    private void OnClickRareGacha(GameObject btn){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		if (DataCenter.Instance.GetAvailableOneGachaTimes() < 1) {
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("RareGachaFailed"),TextCenter.GetText("RareGachaStoneNotEnough", DataCenter.rareGachaStone),TextCenter.GetText("Back"));
			return;
		}

		gachaType = GachaType.RareGacha;
		gachaCount = 1;
		Umeng.GA.Event ("Gacha2",gachaCount+"");
		UnitController.Instance.Gacha(OnRspGacha, (int)gachaType, gachaCount);
		
	}

    private void OnClickEventGacha(GameObject btn){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		if (DataCenter.Instance.GetAvailableNineGachaTimes() < 9) {
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("RareGachaFailed"),TextCenter.GetText("EventGachaStoneNotEnough", DataCenter.eventGachaStone),TextCenter.GetText("Back"));
			return;
		}
		int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableNineGachaTimes());
		string content2 = TextCenter.GetText("EventGachaStatus", DataCenter.eventGachaStone, 
		                                     maxGachaTimes, maxGachaTimes * DataCenter.eventGachaStone,
		                                     DataCenter.Instance.UserData.AccountInfo.stone);
		
//		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("EventGacha"), new string[2] {TextCenter.GetText ("EventGachaDescription"),content2}, 
//		TextCenter.GetText ("ConfirmOneEventGacha"), 
//		TextCenter.GetText ("ConfirmMaxEventGacha", maxGachaTimes), 
//		CallbackEventGacha, CallbackEventGacha, 1, maxGachaTimes);;
		gachaType = GachaType.RareGacha;
		gachaCount = 9;
		Umeng.GA.Event ("Gacha3",gachaCount+"");
		UnitController.Instance.Gacha(OnRspGacha, (int)gachaType, gachaCount);
    }

	void OnRspGacha(object data) {
		if (data == null)
			return;
		
		LogHelper.Log(data);
		bbproto.RspGacha rsp = data as bbproto.RspGacha;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code,ClickOK);
			return;
		}
		
		if(rsp.unitList.Count == 0) {
			TipsManager.Instance.ShowTipsLabel("server return data is null");
			return;
		}
		
		DataCenter.Instance.UserData.AccountInfo.friendPoint = rsp.friendPoint;
		DataCenter.Instance.UserData.AccountInfo.stone = rsp.stone;

		DataCenter.Instance.UserData.DataCount.gachaWillGet5Star -= 1;
		if( DataCenter.Instance.UserData.DataCount.gachaWillGet5Star <= 0) {
			DataCenter.Instance.UserData.DataCount.gachaWillGet5Star = 9;
		}

		LogHelper.Log("OnRspGacha() finished, friendPoint {0}, stone {1}"
		              ,  DataCenter.Instance.UserData.AccountInfo.friendPoint, DataCenter.Instance.UserData.AccountInfo.stone);
		
		// record
		List<uint> blankList = rsp.blankUnitId;
		List<UserUnit> unitList = rsp.unitList;
		
		LogHelper.LogError("before gacha, userUnitList count {0}", DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count);
		// delete unit;
		
		List<uint> newUnitIdList = DataCenter.Instance.UnitData.UserUnitList.FirstGetUnits(unitList);
		DataCenter.Instance.UnitData.UserUnitList.AddMyUnitList(unitList);
		
		LogHelper.LogError("after gacha, userUnitList count {0}", DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count);
		
		LogHelper.Log("MsgCenter.Instance.Invoke(CommandEnum.EnterGachaWindow");
		ModuleManager.Instance.HideModule (ModuleEnum.ScratchModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.GachaModule, "type", gachaType, "chances", gachaCount, "unit", rsp.unitUniqueId, "blank", blankList, "new", newUnitIdList);// GetGachaWindowInfo (gachaType, gachaCount, rsp.unitUniqueId, blankList, newUnitIdList));
		MsgCenter.Instance.Invoke(CommandEnum.SyncChips, null);
		
	}
	
	private void ClickOK(object data){
		Debug.Log ("scratch: ");
		ModuleManager.Instance.ShowModule (ModuleEnum.ScratchModule);
	}


}
