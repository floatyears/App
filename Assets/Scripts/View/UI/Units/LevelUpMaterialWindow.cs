using UnityEngine;
using System.Collections;

public class LevelUpMaterialWindow : UIComponentUnity {
	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
	}

	public override void ShowUI(){
		base.ShowUI();
		this.gameObject.SetActive(false);
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
		if(message == "Tab_Material" ){
			//Debug.Log(string.Format("{0} Receive a message: {1}", this.gameObject.name, message));
			this.gameObject.SetActive(true);
		}
		else if(message == "Tab_Base"){
			this.gameObject.SetActive(true);
		}
		else{
			this.gameObject.SetActive(false);
		}
	}
}
