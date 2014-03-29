using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageView : UIComponentUnity {
	UILabel curPartyIndexLabel;
	UILabel partyCountLabel;
	UILabel curPartyPrefixLabel;
	UILabel curPartysuffixLabel;
	UIButton leftButton;
	UIButton rightButton;

	GameObject itemLeft;
	GameObject labelLeft;
	
	List<UITexture> texureList = new List<UITexture>();
	Dictionary< int, string > partyIndexDic = new Dictionary< int, string >();
	Dictionary<GameObject, int> itemDic = new Dictionary<GameObject, int>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		FindUIElement();
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
//		SetUIElement();
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
//		ResetUIElement();
	}

    public override void ResetUIState() {
        ResetUIElement();
        SetUIElement();
    }
	
	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch ( cbdArgs.funcName ){
			case "RefreshPartyIndexView" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshIndexView, cbdArgs);
				CallBackDispatcherHelper.DispatchCallBack(ShowLabelLeft, cbdArgs);
				break;
			case "EnableLabelLeft" : 
				CallBackDispatcherHelper.DispatchCallBack(EnableLabelLeft, cbdArgs);
				break;
			case "ShowLabelLeft" : 
				CallBackDispatcherHelper.DispatchCallBack(ShowLabelLeft, cbdArgs);
				break;
			case "EnableItemLeft" : 
				CallBackDispatcherHelper.DispatchCallBack(EnableItemLeft, cbdArgs);
				break;
			case "ShowItemLeft" : 
				CallBackDispatcherHelper.DispatchCallBack(ShowItemLeft, cbdArgs);
				break;
			case "RefreshPartyItemView" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshItemView, cbdArgs);
				break;
			case "ReplaceItemView" :
				CallBackDispatcherHelper.DispatchCallBack(ReplaceItemView, cbdArgs);
				break;
			case "ClearItem" :
				CallBackDispatcherHelper.DispatchCallBack(ClearItemView, cbdArgs);
				break;
			case "LightCurSprite" : 
				CallBackDispatcherHelper.DispatchCallBack(LightCurSprite, cbdArgs);
				break;
			default:
				break;
		}  
	}
	
	void FindUIElement(){
		FindLabel();
		FindButton();
		FindTexture();
		FindItemLeft();
	}

	void FindLabel(){
		labelLeft = transform.FindChild("Label_Left").gameObject;
		curPartyPrefixLabel = FindChild<UILabel>("Label_Left/Label_Bofore");
		curPartysuffixLabel = FindChild<UILabel>("Label_Left/Label_After");

		curPartyIndexLabel = FindChild<UILabel>("Label_Cur_Party");
		partyCountLabel = FindChild<UILabel>("Label_Party_Count");

	}

	void FindButton(){
		leftButton = FindChild<UIButton>("Button_Left");
		rightButton = FindChild<UIButton>("Button_Right");
	}

	void FindItemLeft(){
		itemLeft = transform.FindChild("Item_Left").gameObject;
	}

	void FindTexture() {
		UITexture tex;
		GameObject go;
		for( int i = 1; i < 5; i++) {
			tex = FindChild< UITexture >("Unit" + i.ToString() + "/role" );
			texureList.Add(tex);
			go = transform.FindChild("Unit" + i.ToString() ).gameObject;
			UIEventListenerCustom.Get(go).onClick = ClickItem;
			UIEventListenerCustom.Get(go).LongPress = PressItem;
			itemDic.Add( go, i );
		}
	}

	void InitUIElement(){
		InitIndexTextDic();
		partyCountLabel.text = UIConfig.PartyMaxCount.ToString();
	}

	void SetUIElement(){
		UIEventListener.Get(leftButton.gameObject).onClick = PagePrev;
		UIEventListener.Get(rightButton.gameObject).onClick = PageNext;
	}
	
	void EnableLabelLeft(object args){
		labelLeft.SetActive(true);
	}

	void ShowLabelLeft(object args){
		curPartyPrefixLabel.text = ((int)args).ToString();
		curPartysuffixLabel.text = partyIndexDic[(int)args].ToString();
	}
	
	void EnableItemLeft(object args){
		itemLeft.SetActive(true);
	}

	void ShowItemLeft(object args){
		Texture2D tex2d = args as Texture2D;
		UITexture uiTexture = itemLeft.transform.FindChild("Texture").GetComponent<UITexture>();
		uiTexture.mainTexture = tex2d;
	}

	void InitIndexTextDic() {
		partyIndexDic.Add( 1, "st");
		partyIndexDic.Add( 2, "nd");
		partyIndexDic.Add( 3, "rd");
		partyIndexDic.Add( 4, "th");
		partyIndexDic.Add( 5, "th");
	}

	void ClickItem(GameObject go){
		if( !itemDic.ContainsKey(go) ){
			Debug.Log("PartyPagePanel.ClickItem(), itemDic NOT ContainsKey : " + go.name);
			return;
		}

		string callName = "ClickItem";
		int pos = itemDic[ go ];
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs( callName, pos );
		//LogHelper.Log("PartyPagePanel.ClickItem(), click the item" + itemDic[ go ].ToString() + ", wait respone...");
		ExcuteCallback( cbd );
	}

	void PressItem(GameObject go){
		if( !itemDic.ContainsKey(go) ){
			Debug.Log("PartyPagePanel.ClickItem(), PressItem NOT ContainsKey : " + go.name);
			return;
		}
		//LogHelper.Log("PartyPageView.PressItem(), press the item" + itemDic[ go ].ToString() + ", wait respone...");

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("PressItem", itemDic[ go ]);

		ExcuteCallback(cbdArgs);
	}
	
	void PagePrev(GameObject button){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs( "TurnPage", "prev" );
		//LogHelper.Log("PartyPagePanel.ClickItem(), click the BackArrow, wait respone...");

		ExcuteCallback( cbd );
	} 

	void PageNext(GameObject go){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs( "TurnPage", "next" );
		//LogHelper.Log("PartyPagePanel.ClickItem(), click the BackArrow, wait respone...");
		ExcuteCallback( cbd );
	}
	
	void ResetUIElement(){
		ClearItemLeft();
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
	
//	void LoseFocus(object args){
//		foreach (var item in itemDic) {
//			item.Key.transform.FindChild("High_Light").gameObject.SetActive(false);
//		}
//	}  

	void LightCurSprite(object args){
		int posiotion = (int)args;
		foreach (var item in itemDic){
			if(item.Value == posiotion){
				Debug.LogError("Find the high light position : " + posiotion);
				item.Key.transform.FindChild("High_Light").gameObject.SetActive(true);
			}
			else{
				item.Key.transform.FindChild("High_Light").gameObject.SetActive(false);
			}
		}
	}

	void OnLightSprite(GameObject target){
		target.transform.FindChild("High_Light").gameObject.SetActive(false);
	}

	void ShowTween(){
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	void RefreshIndexView(object args){
		int index = ( int )args;
		curPartyIndexLabel.text = index.ToString();
	}

	void RefreshItemView(object args){
		List<Texture2D> tex2dList = args as List<Texture2D>;
		for (int i = 0; i < tex2dList.Count; i++) {
			if(tex2dList[ i ] == null){
				texureList[ i ].mainTexture = null;
				continue;
			} 
			else {
				texureList[ i ].mainTexture = tex2dList[ i ];
			}
		}

		foreach (var item in itemDic){
			OnLightSprite(item.Key);
		}
	}
	
	void ClearItemView(object args){
		int position = (int)args;
		foreach (var item in itemDic){
			if(item.Value == position){
				item.Key.transform.FindChild("role").GetComponent<UITexture>().mainTexture = null;
			}
		}

	}

	void ReplaceItemView(object args){
		Dictionary<string,object> argsDic = args as Dictionary<string, object>;

		int position = (int)argsDic["position"] ;
		TUserUnit tuu = argsDic["unit"] as TUserUnit;
		Texture2D texture = tuu.UnitInfo.GetAsset( UnitAssetType.Avatar );
	
		ChangeTexure(position, texture);
	}

	
	void ClearItemLeft(){
		UITexture uiTexture = itemLeft.transform.FindChild("Texture").GetComponent<UITexture>();
		uiTexture.mainTexture = null;
	}

	
}
