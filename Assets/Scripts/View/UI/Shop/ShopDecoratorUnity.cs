using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopDecoratorUnity : UIComponentUnity {

	private Dictionary<string,UIButton> buttonDic = new Dictionary<string, UIButton>();

    private UIButton btnFriendsExpansion;

    private UIButton btnStaminaRecover;

    private UIButton btnUnitExpansion;

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTween();
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
//
        UIEventListener.Get(btnFriendsExpansion.gameObject).onClick = OnClickFriendExpansion;
        UIEventListener.Get(btnStaminaRecover.gameObject).onClick = OnClickStaminaRecover;
        UIEventListener.Get(btnUnitExpansion.gameObject).onClick = OnClickUnitExpansion;
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

	private void ShowTween(){
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list)
		{		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}
}
