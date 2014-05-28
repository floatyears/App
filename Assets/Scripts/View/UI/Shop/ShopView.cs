using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopView : UIComponentUnity {

	private Dictionary<string,UIButton> buttonDic = new Dictionary<string, UIButton>();

    private UIButton btnFriendsExpansion;

    private UIButton btnStaminaRecover;

    private UIButton btnUnitExpansion;

	private GameObject infoPanelRoot;
	private GameObject windowRoot;

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowUIAnimation();
	}
	
	public override void HideUI () {
		base.HideUI ();

	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		UIButton[] buttons = FindChild("btns").GetComponentsInChildren< UIButton >();
		for (int i = 0; i < buttons.Length; i++){
			buttonDic.Add( string.Format("Chip{0}", i), buttons[ i ] );
			UIEventListener.Get( buttons[ i ].gameObject ).onClick = ClickButton;
		}

        btnFriendsExpansion = FindChild<UIButton>("top/FriendsExpansion");
        btnStaminaRecover = FindChild<UIButton>("top/StaminaRecover");
        btnUnitExpansion = FindChild<UIButton>("top/UnitExpansion");

        UIEventListener.Get(btnFriendsExpansion.gameObject).onClick = OnClickFriendExpansion;
        UIEventListener.Get(btnStaminaRecover.gameObject).onClick = OnClickStaminaRecover;
        UIEventListener.Get(btnUnitExpansion.gameObject).onClick = OnClickUnitExpansion;

		infoPanelRoot = transform.FindChild("top").gameObject;
		windowRoot = transform.FindChild("btns").gameObject;
	}

	private void ClickButton( GameObject button) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
	}

    private void OnClickFriendExpansion( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoFriendExpansion", null);
        ExcuteCallback(cbdArgs);
    }

    private void OnClickStaminaRecover( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoStaminaRecover", null);
        ExcuteCallback(cbdArgs);
    }

    private void OnClickUnitExpansion( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoUnitExpansion", null);
        ExcuteCallback(cbdArgs);
    }

	private void ShowUIAnimation(){
		infoPanelRoot.transform.localPosition = new Vector3(-1000, -325, 0);
		windowRoot.transform.localPosition = new Vector3(1000, -630, 0);
		iTween.MoveTo(infoPanelRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
		iTween.MoveTo(windowRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
	}
}
