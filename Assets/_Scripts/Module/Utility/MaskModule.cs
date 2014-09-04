using UnityEngine;
using System.Collections;

public class BlockerMaskParams{
	public BlockerMaskParams(BlockerReason reason, bool isBlocked, bool isMaskActive = true){
		this.reason = reason;
		this.isBlocked = isBlocked;
		this.isMaskActive = isMaskActive;
	}

	public BlockerReason reason;
	public bool isBlocked;
	public bool isMaskActive = false;
}

public class MaskModule : ModuleBase {
	public MaskModule(UIConfigItem config) : base(  config){
		CreateUI<MaskView> ();
        AddCommandListener();
    }
	public override void ShowUI(){
		base.ShowUI();
		//Debug.LogError("MaskController.ShowUI()...");
	}

	public override void HideUI(){
		base.HideUI();
		//Debug.LogError("MaskController.HideUI()...");
	} 
	
//	public override void CallbackView(params object[] args){
//		base.CallbackView(data);
//	}

	void ShowMask(object msg){
		//LogHelper.LogError("MaskController.ShowMask(), start...");
		BlockerMaskParams bmArgs = msg as BlockerMaskParams;

		SetBlocker(bmArgs.reason, bmArgs.isBlocked);
		SetMaskActive(bmArgs.isBlocked);
	}

	void ShowConnect(object msg){
		SetConnectActive((bool)msg);
    }
        
    void SetBlocker(BlockerReason reason, bool isBlocker){
		TouchEventBlocker.Instance.SetState(reason, isBlocker);
	}
	
	void SetMaskActive(bool isActive){
		//Debug.LogError("SetMaskActive() " + isActive);
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowMask", isActive);
		view.CallbackView("ShowMask", isActive);
	}

	void SetConnectActive(bool isActive){
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowConnect", isActive);
		view.CallbackView("ShowConnect", isActive);
    }
            
    void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.SetBlocker, ShowMask);
		MsgCenter.Instance.AddListener(CommandEnum.WaitResponse, ShowConnect);
	}
	
	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.SetBlocker, ShowMask);
		MsgCenter.Instance.RemoveListener(CommandEnum.WaitResponse, ShowConnect);
    }

}
