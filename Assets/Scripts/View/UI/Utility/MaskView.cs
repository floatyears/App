using UnityEngine;
using System.Collections;

public class MaskView : UIComponentUnity {
	UISprite background;
	GameObject connecting;
	UILabel tips;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
//		base.HideUI();
		SetMaskActive (false);
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (call.funcName){
			case "ShowMask" :
				CallBackDispatcherHelper.DispatchCallBack(SetMaskActive, call);
				break;
			case "ShowConnect" :
				CallBackDispatcherHelper.DispatchCallBack(SetConnectActive, call);
				break;
			default:
				break;
		}
	}

	void InitUI(){
		background = FindChild<UISprite>("Sprite_Mask");
		connecting = FindChild("Tips");
		tips = FindChild<UILabel> ("Tips/Content");
		background.enabled = false;
		connecting.transform.parent.gameObject.SetActive(false);
	}

	void SetMaskActive(object args){
		if (background == null) {
			return;	
		}
		bool isActive = (bool)args;

		background.enabled = isActive;
		connecting.SetActive (false);
		gameObject.SetActive(isActive);
	}

	void SetConnectActive(object args){

		if (connecting == null) {
			CancelInvoke ("ShowTipText");
			return;	
		}
		FindChild<UILabel> ("Tips/Sprite_Connect/Label_Text").text = TextCenter.GetText ("Connecting");
		bool isActive = (bool)args;

		if (isActive) {
			NGUITools.AdjustDepth (gameObject, 5);
			InvokeRepeating ("ShowTipText", 0, 3.0f);
		}
			
		else{
			NGUITools.AdjustDepth (gameObject, -5);
			CancelInvoke ("ShowTipText");
		}
			

		connecting.SetActive(isActive);
		gameObject.SetActive(isActive);


	}

	
	private void ShowTipText(){
//		Debug.Log ("random: " +DGTools.RandomToInt(1,13));
		if (DataCenter.Instance.LoginInfo != null && DataCenter.Instance.LoginInfo.Data != null) {
			if (DataCenter.Instance.LoginInfo.Data.Rank < 5) {
				tips.text = TextCenter.GetText ("Tips_A_" + DGTools.RandomToInt(1,13));//.RandomToInt (1, 13));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 10) {
				tips.text = TextCenter.GetText ("Tips_B_" + MathHelper.RandomToInt (1, 10));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 20) {
				tips.text = TextCenter.GetText ("Tips_C_" + MathHelper.RandomToInt (1, 18));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 30) {
				tips.text = TextCenter.GetText ("Tips_D_" + MathHelper.RandomToInt (1, 18));
			} else {
				tips.text = TextCenter.GetText ("Tips_E_" + MathHelper.RandomToInt (1, 24));
			}	
		} else {
			tips.text = TextCenter.GetText ("Tips_A_" + MathHelper.RandomToInt (1, 13));
		}     
	}
//
//	void Update(){
//		if (background.enabled) {
//
//		}
//	}
}
