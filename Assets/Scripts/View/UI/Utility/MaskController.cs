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

		bmArgs.isMaskActive = !bmArgs.isBlocked ? false : bmArgs.isMaskActive;
		SetMaskActive(bmArgs.isMaskActive);
		SetBlocker(bmArgs.reason, bmArgs.isBlocked);

	}

	void SetBlocker(BlockerReason reason, bool isBlocker){
		TouchEventBlocker.Instance.SetState(reason, isBlocker);
	}
	
	void SetMaskActive(bool isActive){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ShowMask", isActive);
        ExcuteCallback(call);
	}

	private void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.SetBlocker, ShowMask);
	}
	
	private void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.SetBlocker, ShowMask);
	}

}
