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

public class MaskController : ConcreteComponent {

	public MaskController(string name) : base(name){}
	public override void ShowUI(){
		base.ShowUI();
		AddCommandListener();
	}

	public override void HideUI(){
		base.HideUI();
		RemoveCommandListener();
	} 

	public override void Callback(object data){
		base.Callback(data);
	}


	void ShowMask(object msg){
		BlockerMaskParams bmArgs = msg as BlockerMaskParams;

		SetBlocker(bmArgs.reason, bmArgs.isBlocked);
        SetMaskActive(TouchEventBlocker.Instance.IsBlocked);
	}

	void ShowConnect(object msg){
		SetConnectActive((bool)msg);
    }
        
    void SetBlocker(BlockerReason reason, bool isBlocker){
		TouchEventBlocker.Instance.SetState(reason, isBlocker);
	}
	
	void SetMaskActive(bool isActive){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowMask", isActive);
        ExcuteCallback(call);
	}

	void SetConnectActive(bool isActive){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowConnect", isActive);
		ExcuteCallback(call);
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
