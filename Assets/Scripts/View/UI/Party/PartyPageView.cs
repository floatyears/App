using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPageView : UIComponentUnity {
	int currentPos = -1;
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
		Debug.Log("PartyPagePanel.SetUIElement() : Start...");
		UIEventListener.Get(leftButton.gameObject).onClick = PagePrev;
		UIEventListener.Get(rightButton.gameObject).onClick = PageNext;
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

		if( !itemDic.ContainsKey(go) ){
			Debug.Log("PartyPagePanel.ClickItem(), itemDic NOT ContainsKey : " + go.name);
			return;
		}

		string callName = "ClickItem";
		int pos = itemDic[ go ];
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs( callName, pos );
		LogHelper.Log("PartyPagePanel.ClickItem(), click the item" + itemDic[ go ].ToString() + ", wait respone...");
		ExcuteCallback( cbd );

	}

	void PressItem(GameObject go){
		if( !itemDic.ContainsKey(go) ){
			Debug.Log("PartyPagePanel.ClickItem(), PressItem NOT ContainsKey : " + go.name);
			return;
		}
		LogHelper.Log("PartyPageView.PressItem(), press the item" + itemDic[ go ].ToString() + ", wait respone...");

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("PressItem", itemDic[ go ]);

		ExcuteCallback(cbdArgs);
	}


	void PagePrev(GameObject button){
		Debug.Log("PartyPagePanel.PagePrev() : Start");

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs( "TurnPage", "prev" );
		LogHelper.Log("PartyPagePanel.ClickItem(), click the BackArrow, wait respone...");

		ExcuteCallback( cbd );

		Debug.Log("PartyPagePanel.ExcuteCallback() : End");
	} 

	void PageNext(GameObject go){
		Debug.Log("PartyPagePanel.PageNext() : Start");

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs( "TurnPage", "next" );
		LogHelper.Log("PartyPagePanel.ClickItem(), click the BackArrow, wait respone...");
		ExcuteCallback( cbd );

		Debug.Log("PartyPagePanel.PageNext() : End");
	}
	
	void ResetUIElement(){
		Debug.Log("PartyPagePanel.ResetUIElement() : Start");
		Debug.Log("PartyPagePanel.ResetUIElement() : End");
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
	
	void LoseFocus(object args){
		Debug.Log("PartyPagePanel.LoseFocus() : Start...");

		foreach (var item in itemDic) {
			item.Key.transform.FindChild("High_Light").gameObject.SetActive(false);
		}

		Debug.Log("PartyPagePanel.LoseFocus() : End...");
	}  

	//Light the click Item
	void SetHighLight(object args){
		int pos = (int)args;
		Debug.Log("PartyPagePanel.SetHighLight() : Sprite Pos is : " + pos);
		foreach (var item in itemDic) {
			if( pos == item.Value ){
				currentPos = pos;
				item.Key.transform.FindChild("High_Light").gameObject.SetActive(true);
			} else {
				item.Key.transform.FindChild("High_Light").gameObject.SetActive(false);
			}
		}

		Debug.Log("PartyPagePanel.SetHighLight() : End...");
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
		Debug.Log("PartyPagePanel.RefreshIndexLabel(), index is " + index);	
		
		curPartyPrefixLabel.text = index.ToString();
		curPartysuffixLabel.text = partyIndexDic[ index ].ToString();
		curPartyIndexLabel.text = index.ToString();
	}
	
	void RefreshItemView(object args){
		List<Texture2D> tex2dList = args as List<Texture2D>;
		Debug.Log("PartyPagePanel.UpdateTexture(), Start...");
		for (int i = 0; i < tex2dList.Count; i++) {
			if(tex2dList[ i ] == null){
				Debug.LogError(string.Format("PartyPagePanel.UpdateTexture(), Pos[{0}] data is null, clear!", i));
				texureList[ i ].mainTexture = null;
				continue;
			} 
			else {
				texureList[ i ].mainTexture = tex2dList[ i ];
				Debug.Log(string.Format("PartyPagePanel.UpdateTexture(), Pos[{0}] texture is showing", i));
			}
		}

		foreach (var item in itemDic){
			OnLightSprite(item.Key);
		}

		Debug.Log("PartyPagePanel.UpdateTexture(), End...");
	}

	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch ( cbdArgs.funcName ){
			case "RefreshPartyIndexView" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshIndexView, cbdArgs);
				break;
			case "RefreshPartyItemView" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshItemView, cbdArgs);
                        	break;
               	 	case "LightCurSprite" :
				CallBackDispatcherHelper.DispatchCallBack(SetHighLight, cbdArgs);
                        	break;
                	case "DrakAllSprite" :
				CallBackDispatcherHelper.DispatchCallBack(LoseFocus, cbdArgs);
                        	break;
			case "ReplaceItemView" :
				CallBackDispatcherHelper.DispatchCallBack(ReplaceItemView, cbdArgs);
				break;
			case "ClearItem" :
				CallBackDispatcherHelper.DispatchCallBack(ClearItemView, cbdArgs);
				break;
                default:
                        break;
                }
                
        }

	void ClearItemView(object args){
		int position = (int)args;
		Debug.LogError("ClearItemView, to clear position : " + position);
		foreach (var item in itemDic){
			if(item.Value == position){
				item.Key.transform.FindChild("role").GetComponent<UITexture>().mainTexture = null;
			}
		}

		Debug.Log("PartyPagePanel.ClearItemView(), receive the call, to clear the view of item " + position);
	}

	void ReplaceItemView(object args){
		Dictionary<string,object> argsDic = args as Dictionary<string, object>;

		int position = (int)argsDic["position"] ;
		TUserUnit tuu = argsDic["unit"] as TUserUnit;
		Texture2D texture = tuu.UnitInfo.GetAsset( UnitAssetType.Avatar );
	
		ChangeTexure(position, texture);

		Debug.Log("PartyPagePanel.ReplaceItemView(), End...");
	}
	
}
