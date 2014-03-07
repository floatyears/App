using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPagePanel : UIComponentUnity {
	int currentPos = -1;
	int partyTotalCount = 5;
	UILabel curPartyIndexLabel;
	UILabel partyCountLabel;
	UILabel curPartyPrefixLabel;
	UILabel curPartysuffixLabel;
	UIButton leftButton;
	UIButton rightButton;
	Dictionary< int, string > partyIndexDic = new Dictionary< int, string >();
	Dictionary<GameObject, int> itemDic = new Dictionary<GameObject, int>();

	List<UITexture> texureList = new List<UITexture>();
	bool InitSymbol = false;
	public override void Init(UIInsConfig config, IUICallback origin){

		base.Init(config, origin);
		FindUIElement();
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		currentPos = -1;
		SetUIElement();
		ShowTween();

	}

	public override void HideUI(){
     
		base.HideUI();
		ResetUIElement();
	}


	void FindUIElement(){
		Debug.Log("PartyPagePanel.FindUIElement() : Start...");

		FindLabel();
		FindButton();
		FindTexture();

		Debug.Log("PartyPagePanel.FindUIElement() : End...");
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

	void FindItem(){
//		GameObject go;

	}

	void FindTexture() {
		UITexture tex;
		GameObject go;
		for( int i = 1; i < 5; i++) {
			tex = FindChild< UITexture >("Unit" + i.ToString() + "/role" );
			texureList.Add(tex);
			go = transform.FindChild("Unit" + i.ToString() ).gameObject;
			UIEventListener.Get(go).onClick = ClickItem;
			itemDic.Add( go, i );
		}
	}

	void InitUIElement(){
		InitIndexTextDic();
		partyCountLabel.text = partyTotalCount.ToString();
	}

	
	void UpdateLabel(int index){
		Debug.Log("PartyPagePanel.UpdateLabel(), index is " + index);
		curPartyPrefixLabel.text = index.ToString();
		curPartysuffixLabel.text = partyIndexDic[ index ].ToString();
		curPartyIndexLabel.text = index.ToString();
	}
	
	void UpdateTexture(List<Texture2D> tex2dList){
		Debug.Log("PartyPagePanel.UpdateTexture(), Start...");
		for (int i = 0; i < tex2dList.Count; i++) {
			if(tex2dList[ i ] == null){
				Debug.LogError(string.Format("PartyPagePanel.UpdateTexture(), Pos[{0}] source is null, do nothing!", i));
				continue;
			} else {
				texureList[ i ].mainTexture = tex2dList[ i ];
				Debug.Log(string.Format("PartyPagePanel.UpdateTexture(), Pos[{0}] texture is showing", i));
			}
		}
		Debug.Log("PartyPagePanel.UpdateTexture(), End...");
	}

	void SetUIElement(){
		Debug.Log("PartyPagePanel.SetUIElement() : Start...");
		UIEventListener.Get(leftButton.gameObject).onClick = PageBack;
		UIEventListener.Get(rightButton.gameObject).onClick = PageForward;
		Debug.Log("PartyPagePanel.SetUIElement() : End...");
	}

	void InitIndexTextDic() {
		partyIndexDic.Add( 1, "st");
		partyIndexDic.Add( 2, "nd");
		partyIndexDic.Add( 3, "rd");
		partyIndexDic.Add( 4, "th");
		partyIndexDic.Add( 5, "th");
	}

	void ClickItem(GameObject go){
		Debug.Log("PartyPagePanel.ClickItem(), item name is : " + go.name);
		if(!itemDic.ContainsKey(go)){
			Debug.Log("PartyPagePanel.ClickItem(), itemDic NOT ContainsKey : " + go.name);
			return;
		}
		ExcuteCallback("ClickItem" + itemDic[ go ]);
	}


	void PageBack(GameObject button){
		Debug.Log("PartyPagePanel.PageBack() : Start");

		ExcuteCallback("PageBack");

		Debug.Log("PartyPagePanel.ExcuteCallback() : End");
	} 

	void PageForward(GameObject go){
		Debug.Log("PartyPagePanel.PageForward() : Start");

		ExcuteCallback("PageForward");

		Debug.Log("PartyPagePanel.PageForward() : End");
	}
	
	void ResetUIElement(){
		Debug.Log("PartyPagePanel.ResetUIElement() : Start");
		Debug.Log("PartyPagePanel.ResetUIElement() : End");
	}

	public override void Callback(object data){
		base.Callback(data);
		Dictionary<string,object> viewInfoDic = data as Dictionary<string,object>;
		if( viewInfoDic == null ){
			Debug.LogError("PartyPagePanel.Callback(), ViewInfo is Null!");
			return;
		}	
		object tex2dList;
		object curPartyIndex;

		if(viewInfoDic.TryGetValue("index",out curPartyIndex)){
			UpdateLabel((int)curPartyIndex);
		}

		if(viewInfoDic.TryGetValue("texture",out tex2dList)){
			List<Texture2D> temp = tex2dList as List<Texture2D>;
			UpdateTexture(temp);
		}

		object pos;
		if(viewInfoDic.TryGetValue("LightSprite", out pos)){
			SetHighLight((int)pos);
		}

		object avatarChange;
		if(viewInfoDic.TryGetValue("changeTexture", out avatarChange)){
			if(currentPos <= 0)	return;
			ChangeTexure(currentPos,avatarChange as Texture2D);
		}

		
	}

	void ChangeTexure(int pos,Texture2D tex){
		if(tex == null ){
			return;
		}
	
		foreach (var item in itemDic) {
			if( pos == item.Value ){
				texureList[ pos-1 ].mainTexture = tex;
			} 
		}
	}

	void SetHighLight(int pos){
		foreach (var item in itemDic) {
			if( pos == item.Value ){
				currentPos = pos;
				item.Key.transform.FindChild("High_Light").gameObject.SetActive(true);
			} else {
				item.Key.transform.FindChild("High_Light").gameObject.SetActive(false);
			}
		}
	}

	void ShowTween()
	{
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list)
		{		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}
	
}
