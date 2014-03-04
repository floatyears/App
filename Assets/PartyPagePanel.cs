using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPagePanel : UIComponentUnity {

	UILabel curPartyIndexLabel;
	UILabel partyCountLabel;
	UILabel curPartyPrefixLabel;
	UILabel curPartysuffixLabel;
	UIButton leftButton;
	UIButton rightButton;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		FindUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		SetUIElement();
	}

	public override void HideUI(){
     
		base.HideUI();
		ResetUIElement();
	}

	void FindUIElement(){
		Debug.Log("PartyPagePanel.FindUIElement() : Start");

		curPartyIndexLabel = FindChild<UILabel>("Label_Cur_Party");
		partyCountLabel = FindChild<UILabel>("Label_Party_Count");
		curPartyPrefixLabel = FindChild<UILabel>("Label_Party_Index_Prefix");
		curPartysuffixLabel = FindChild<UILabel>("Label_Party_Index_Suffix");
		leftButton = FindChild<UIButton>("Button_Left");
		rightButton = FindChild<UIButton>("Button_Right");

		Debug.Log("PartyPagePanel.FindUIElement() : End");
	}

	void SetUIElement(){
		Debug.Log("PartyPagePanel.SetUIElement() : Start");

		UIEventListener.Get(leftButton.gameObject).onClick = PageBack;
		UIEventListener.Get(rightButton.gameObject).onClick = PageForward;


		Debug.Log("PartyPagePanel.SetUIElement() : End");
	}

	void PageBack(GameObject button){
		Debug.Log("PartyPagePanel.PageBack() : Start");
		//call logic 
		string callerName = "PageBack";
		ExcuteCallback(callerName);
		Debug.Log("PartyPagePanel.PageBack() : CallerName is : " + callerName);

		Debug.Log("PartyPagePanel.ExcuteCallback() : End");
	} 

	void PageForward(GameObject go){
		Debug.Log("PartyPagePanel.PageForward() : Start");

		Debug.Log("PartyPagePanel.PageForward() : End");
	}

	void ResetUIElement(){
//		ExcuteCallback();
		Debug.Log("PartyPagePanel.ResetUIElement() : Start");
		Debug.Log("PartyPagePanel.ResetUIElement() : End");
	}

	public override void Callback(object data){
		base.Callback(data);
		Dictionary<string,object> callBack = data as Dictionary<string,object>;
		if(callBack.ContainsKey("PageBack")){
			Debug.Log("Receive UILogic call of");
		}

		if(callBack.ContainsKey("PageForward")){

		}


	}



}
