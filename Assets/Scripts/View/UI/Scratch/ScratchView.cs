using UnityEngine;
using System.Collections;

public class ScratchView : UIComponentUnity {

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

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
        UpdateGachaTimes();
		ShowTween();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
        btnFriendGacha = FindChild<UIButton>("bottom_panel/1");
        btnRareGacha = FindChild<UIButton>("bottom_panel/2");
        btnEventGacha = FindChild<UIButton>("bottom_panel/3");

        UIEventListener.Get(btnFriendGacha.gameObject).onClick = OnClickFriendGacha;
        UIEventListener.Get(btnRareGacha.gameObject).onClick = OnClickRareGacha;
        UIEventListener.Get(btnEventGacha.gameObject).onClick = OnClickEventGacha;

        friendGachaTimesParent = FindChild<UISprite>("bottom_panel/1/TimesParent");
        rareGachaTimesParent = FindChild<UISprite>("bottom_panel/2/TimesParent");
        eventGachaTimesParent = FindChild<UISprite>("bottom_panel/3/TimesParent");

        friendGachaTimes = FindChild<UILabel>("bottom_panel/1/TimesParent/Times");
        rareGachaTimes = FindChild<UILabel>("bottom_panel/2/TimesParent/Times");
        eventGachaTimes = FindChild<UILabel>("bottom_panel/3/TimesParent/Times");

		infoPanelRoot = transform.FindChild("info_panel").gameObject;
		windowRoot = transform.FindChild("bottom_panel").gameObject;
	}

	private void ShowTween(){
//		TweenPosition[ ] list = 
//			gameObject.GetComponentsInChildren< TweenPosition >();
//		if (list == null)
//			return;
//		foreach (var tweenPos in list)
//		{		
//			if (tweenPos == null)
//				continue;
//			//tweenPos.onFinished.Add(new EventDelegate(onTweenFinished));
//			tweenPos.Reset();
//			tweenPos.PlayForward();
//		}
		infoPanelRoot.transform.localPosition = new Vector3(-1000, -325, 0);
		windowRoot.transform.localPosition = new Vector3(1000, -600, 0);
		iTween.MoveTo(infoPanelRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
		iTween.MoveTo(windowRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));

	}

	private void onTweenFinished()
	{
		NoviceGuideStepEntityManager.Instance().StartStep();
	}

    private void OnClickButton(GameObject btn){
        AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
    }

    private void OnClickFriendGacha(GameObject btn){
//        LogHelper.Log("OnClickFriendGacha");
        OnClickButton(btn);
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("OpenFriendGachaWindow", null);
        ExcuteCallback(cbdArgs);
    }

    private void OnClickRareGacha(GameObject btn){
//        LogHelper.Log("OnClickRareGacha");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("OpenRareGachaWindow", null);
		ExcuteCallback(cbdArgs);
	}

    private void OnClickEventGacha(GameObject btn){
//        LogHelper.Log("OnClickEventGacha");
        OnClickButton(btn);
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("OpenEventGachaWindow", null);
        ExcuteCallback(cbdArgs);
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
