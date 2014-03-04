using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPagePanel : UIComponentUnity {

	int pageIndexOrigin = 1;
	int currentPartyIndex;
	int partyTotalCount;
	UILabel curPartyIndexLabel;
	UILabel partyCountLabel;
	UILabel curPartyPrefixLabel;
	UILabel curPartysuffixLabel;
	UIButton leftButton;
	UIButton rightButton;
	Dictionary< int, string > partyIndexDic = new Dictionary< int, string >();
	Dictionary< int, UITexture > unitTexureDic = new Dictionary< int, UITexture>();

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

		FindLabel();
		FindButton();
		FindAvatarTexture();

		Debug.Log("PartyPagePanel.FindUIElement() : End");
	}

	void FindLabel(){
		curPartyIndexLabel = FindChild<UILabel>("Label_Cur_Party");
		partyCountLabel = FindChild<UILabel>("Label_Party_Count");
		curPartyPrefixLabel = FindChild<UILabel>("Label_Party_Index_Prefix");
		curPartysuffixLabel = FindChild<UILabel>("Label_Party_Index_Suffix");
	}

	void FindButton(){
		leftButton = FindChild<UIButton>("Button_Left");
		rightButton = FindChild<UIButton>("Button_Right");
	}

	void FindAvatarTexture() {
		UITexture temp;
		for( int i = 1; i < 5; i++) {
			temp = FindChild< UITexture >("PartyPages/Unit" + i.ToString() + "/role" );
			temp.enabled = false;
			unitTexureDic.Add(i, temp);
		}
	}

	void SetUIElement(){
		Debug.Log("PartyPagePanel.SetUIElement() : Start");
		SetIndexTextDic();
		UIEventListener.Get(leftButton.gameObject).onClick = PageBack;
		UIEventListener.Get(rightButton.gameObject).onClick = PageForward;


		Debug.Log("PartyPagePanel.SetUIElement() : End");
	}

	void SetIndexTextDic() {
		partyIndexDic.Add( 1, "st");
		partyIndexDic.Add( 2, "nd");
		partyIndexDic.Add( 3, "rd");
		partyIndexDic.Add( 4, "th");
		partyIndexDic.Add( 5, "th");
	}

	void PageBack(GameObject button){
		Debug.Log("PartyPagePanel.PageBack() : Start");

		currentPartyIndex = Mathf.Abs( (currentPartyIndex - 1) % partyTotalCount );
		if( currentPartyIndex == 0 )
			currentPartyIndex = partyTotalCount ;
		curPartyIndexLabel.text = currentPartyIndex.ToString();
		curPartyPrefixLabel.text = currentPartyIndex.ToString();
		curPartysuffixLabel.text = partyIndexDic[ currentPartyIndex ].ToString();

		//call logic 
		string callerName = "PageBack";
		ExcuteCallback(callerName);
		Debug.Log("PartyPagePanel.PageBack() : CallerName is : " + callerName);

		Debug.Log("PartyPagePanel.ExcuteCallback() : End");
	} 

	void PageForward(GameObject go){
		Debug.Log("PartyPagePanel.PageForward() : Start");

		currentPartyIndex++;
		if (currentPartyIndex > partyTotalCount) {
			currentPartyIndex = pageIndexOrigin;
		} 
		curPartyPrefixLabel.text = currentPartyIndex.ToString();
		curPartyPrefixLabel.text = currentPartyIndex.ToString();
		curPartysuffixLabel.text = partyIndexDic[ currentPartyIndex ].ToString();


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
