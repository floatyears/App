using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaskView : ViewBase {
	UISprite background;
	GameObject connecting;
	UILabel tips;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config,data);
		InitUI();
	}

	public override void CallbackView(params object[] args){
//		base.CallbackView(data);
//		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (args[0].ToString()){

			case "block":
				ShowMask(args[1]);
				break;
			case "connect":
				SetConnectActive((bool)args[1]);
				break;
			default:
				break;
		}
	}
	
	void ShowMask(object msg){
		//LogHelper.LogError("MaskController.ShowMask(), start...");
//		Debug.Log ("show mask");
		BlockerMaskParams bmArgs = msg as BlockerMaskParams;
		
		TouchEventBlocker.Instance.SetState(bmArgs.reason, bmArgs.isBlocked);
		if (background == null) {
			return;	
		}
		
		background.enabled = bmArgs.isBlocked;
		gameObject.SetActive(bmArgs.isBlocked);
	}

	void InitUI(){
		background = FindChild<UISprite>("Sprite_Mask");
		connecting = FindChild("Tips");
		tips = FindChild<UILabel> ("Tips/Content");
		background.enabled = false;
		connecting.transform.parent.gameObject.SetActive(false);
	}

	void SetMaskActive(bool isActive){

	}

	void SetConnectActive(bool isActive){

		if (connecting == null) {
			CancelInvoke ("ShowTipText");
			return;	
		}
		FindChild<UILabel> ("Tips/Sprite_Connect/Label_Text").text = TextCenter.GetText ("Connecting");

		if (isActive) {
			ModuleManager.Instance.ShowModule(ModuleEnum.MaskModule);
			InvokeRepeating ("ShowTipText", 0, 3.0f);
		}
			
		else{
			ModuleManager.Instance.HideModule(ModuleEnum.MaskModule);
			CancelInvoke ("ShowTipText");
		}
	}

	
	private void ShowTipText(){
//		Debug.Log ("random: " +DGTools.RandomToInt(1,13));
		if (DataCenter.Instance.UserData.LoginInfo != null && DataCenter.Instance.UserData.LoginInfo.rank != null) {
			if (DataCenter.Instance.UserData.LoginInfo.rank < 5) {
				tips.text = TextCenter.GetText ("Tips_A_" + DGTools.RandomToInt(1,13));//.RandomToInt (1, 13));
			} else if (DataCenter.Instance.UserData.LoginInfo.rank < 10) {
				tips.text = TextCenter.GetText ("Tips_B_" + Utility.MathHelper.RandomToInt (1, 10));
			} else if (DataCenter.Instance.UserData.LoginInfo.rank < 20) {
				tips.text = TextCenter.GetText ("Tips_C_" + Utility.MathHelper.RandomToInt (1, 18));
			} else if (DataCenter.Instance.UserData.LoginInfo.rank < 30) {
				tips.text = TextCenter.GetText ("Tips_D_" + Utility.MathHelper.RandomToInt (1, 18));
			} else {
				tips.text = TextCenter.GetText ("Tips_E_" + Utility.MathHelper.RandomToInt (1, 24));
			}	
		} else {
			tips.text = TextCenter.GetText ("Tips_A_" + Utility.MathHelper.RandomToInt (1, 13));
		}     
	}
//
//	void Update(){
//		if (background.enabled) {
//
//		}
//	}
}
