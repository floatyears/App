using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScratchView : ViewBase {

    private UIButton btnFriendGacha;
    private UIButton btnRareGacha;
    private UIButton btnEventGacha;

    private UISprite friendGachaTimesParent;
    private UISprite rareGachaTimesParent;
    private UISprite eventGachaTimesParent;


    private UILabel friendGachaTimes;
    private UILabel rareGachaTimes;
    private UILabel eventGachaTimes;

	private GameObject infoPanelRoot;
	private GameObject windowRoot;

	private UILabel scratchContent;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config,data);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
        UpdateGachaTimes();
		ShowUIAnimation();

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.SCRATCH);
	}

	private void InitUI() {
		btnFriendGacha = FindChild<UIButton>("Gacha_Entrance/1");
		FindChild ("Gacha_Entrance/1/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("FriendScratch");
		btnRareGacha = FindChild<UIButton>("Gacha_Entrance/2");
		FindChild ("Gacha_Entrance/2/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("RareScratch");
		btnEventGacha = FindChild<UIButton>("Gacha_Entrance/3");
		FindChild ("Gacha_Entrance/3/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("EventScratch");

        UIEventListenerCustom.Get(btnFriendGacha.gameObject).onClick = OnClickFriendGacha;
        UIEventListenerCustom.Get(btnRareGacha.gameObject).onClick = OnClickRareGacha;
        UIEventListenerCustom.Get(btnEventGacha.gameObject).onClick = OnClickEventGacha;

		friendGachaTimesParent = FindChild<UISprite>("Gacha_Entrance/1/TimesParent");
		rareGachaTimesParent = FindChild<UISprite>("Gacha_Entrance/2/TimesParent");
		eventGachaTimesParent = FindChild<UISprite>("Gacha_Entrance/3/TimesParent");

		friendGachaTimes = FindChild<UILabel>("Gacha_Entrance/1/TimesParent/Times");
		rareGachaTimes = FindChild<UILabel>("Gacha_Entrance/2/TimesParent/Times");
		eventGachaTimes = FindChild<UILabel>("Gacha_Entrance/3/TimesParent/Times");

		scratchContent = FindChild<UILabel> ("Notice_Window/Content");
		scratchContent.text = DataCenter.Instance.CommonData.NoticeInfo.GachaNotice;

		infoPanelRoot = transform.FindChild("Notice_Window").gameObject;
		windowRoot = transform.FindChild("Gacha_Entrance").gameObject;
	}

	private void ShowUIAnimation(){
		infoPanelRoot.transform.localPosition = new Vector3(-1000, -300, 0);
		windowRoot.transform.localPosition = new Vector3(1000, -570, 0);
		iTween.MoveTo(infoPanelRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
		iTween.MoveTo(windowRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));

	}

	private void onTweenFinished(){
		NoviceGuideStepEntityManager.Instance().StartStep(NoviceGuideStartType.UNITS);
	}

    private void OnClickButton(GameObject btn){
        AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
    }

    private void OnClickFriendGacha(GameObject btn){
//        LogHelper.Log("OnClickFriendGacha");
        OnClickButton(btn);
//        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("OpenFriendGachaWindow", null);
//        ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.ScratchModule, "OpenFriendGachaWindow");
    }

    private void OnClickRareGacha(GameObject btn){
//        LogHelper.Log("OnClickRareGacha");
		OnClickButton(btn);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("OpenRareGachaWindow", null);
//		ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.ScratchModule, "OpenRareGachaWindow");
	}

    private void OnClickEventGacha(GameObject btn){
//        LogHelper.Log("OnClickEventGacha");
        OnClickButton(btn);
//        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("OpenEventGachaWindow", null);
//        ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.ScratchModule, "OpenEventGachaWindow");
    }

    private void UpdateGachaTimes(){
        UpdateFriendGachaTimes();
        UpdateRareGachaTimes();
        UpdateEventGachaTimes();
    }

    private void UpdateFriendGachaTimes(){
        if (DataCenter.Instance.GetAvailableFriendGachaTimes() == 0){
            friendGachaTimesParent.gameObject.SetActive(false);
            return;
        }
        friendGachaTimesParent.gameObject.SetActive(true);
        friendGachaTimes.text = DataCenter.Instance.GetAvailableFriendGachaTimes().ToString();
    }

    private void UpdateRareGachaTimes(){
        if (DataCenter.Instance.GetAvailableRareGachaTimes() == 0){
            rareGachaTimesParent.gameObject.SetActive(false);
            return;
        }
        rareGachaTimesParent.gameObject.SetActive(true);
        rareGachaTimes.text = DataCenter.Instance.GetAvailableRareGachaTimes().ToString(); 
    }

    private void UpdateEventGachaTimes(){
        if (DataCenter.Instance.GetAvailableEventGachaTimes() == 0){
            eventGachaTimesParent.gameObject.SetActive(false);
            return;
        }
        eventGachaTimesParent.gameObject.SetActive(true);
        eventGachaTimes.text = DataCenter.Instance.GetAvailableEventGachaTimes().ToString();  
    }
	
	public UIButton BtnRareGacha{
		get{ return btnRareGacha;}
		private set{btnRareGacha = value;}
	}

}
