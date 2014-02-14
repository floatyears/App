using UnityEngine;
using System.Collections;

public class LevelUpFriendWindow : UIComponentUnity {
	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
	}

	public override void ShowUI(){
		base.ShowUI();
		this.gameObject.SetActive( false );
		MsgCenter.Instance.AddListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}

	public override void HideUI() {
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}

	void InitUI(){

	}

	void FocusOnPanel(object data) {
		string message = (string)data;
		if(message == "Tab_Friend"){
			//Debug.Log(string.Format("{0} Receive a message: {1}", this.gameObject.name, message));
			this.gameObject.SetActive(true);
		}
		else{
			this.gameObject.SetActive(false);
		}
	}
}
