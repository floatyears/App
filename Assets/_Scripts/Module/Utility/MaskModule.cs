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
    }

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data[0].ToString()) {
			case "block":
				ShowMask(data[1]);
				break;
			case "wait":
				view.CallbackView("ShowConnect", data[1]);
				break;
			default:
					break;
			}
	}

	void ShowMask(object msg){
		//LogHelper.LogError("MaskController.ShowMask(), start...");
		Debug.Log ("show mask");
		BlockerMaskParams bmArgs = msg as BlockerMaskParams;

		TouchEventBlocker.Instance.SetState(bmArgs.reason, bmArgs.isBlocked);
		view.CallbackView("ShowMask", bmArgs.isBlocked);
	}
}
