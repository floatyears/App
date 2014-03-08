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

		if( !itemDic.ContainsKey(go) ){
			Debug.Log("PartyPagePanel.ClickItem(), itemDic NOT ContainsKey : " + go.name);
			return;
		}

		string callName = "ClickItem";
		int pos = itemDic[ go ];
		CallBackDeliver cbd = new CallBackDeliver( callName, pos );
		LogHelper.Log("PartyPagePanel.ClickItem(), click the item" + itemDic[ go ].ToString() + ", wait respone...");
		ExcuteCallback( cbd );

	}


	void PageBack(GameObject button){
		Debug.Log("PartyPagePanel.PageBack() : Start");

		CallBackDeliver cbd = new CallBackDeliver( "PageBack", null );
		LogHelper.Log("PartyPagePanel.ClickItem(), click the BackArrow, wait respone...");

		ExcuteCallback( cbd );

		Debug.Log("PartyPagePanel.ExcuteCallback() : End");
	} 

	void PageForward(GameObject go){
		Debug.Log("PartyPagePanel.PageForward() : Start");

		CallBackDeliver cbd = new CallBackDeliver( "PageForward", null );
		LogHelper.Log("PartyPagePanel.ClickItem(), click the BackArrow, wait respone...");
		ExcuteCallback( cbd );

		Debug.Log("PartyPagePanel.PageForward() : End");
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
	
	void DarkAllItem(){
		Debug.Log("PartyPagePanel.DarkAllItem() : Start...");
		foreach (var item in itemDic) {
			item.Key.transform.FindChild("High_Light").gameObject.SetActive(false);
		}

		Debug.Log("PartyPagePanel.SetHighLight() : End...");
	}

	//Light the click Item
	void SetHighLight(int pos){

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

	public override void Callback(object data){
		base.Callback(data);
		
		CallBackDeliver cbd = data as CallBackDeliver;
		
		switch ( cbd.callBackName ){
			case "RefreshPartyIndex" : 
				int index = ( int )cbd.callBackContent;
				UpdateLabel( index );
				break;
			case "RefreshPartyTexture" : 
				List<Texture2D> t2dList = cbd.callBackContent as List<Texture2D>;
				UpdateTexture( t2dList );
                        break;
               	 	case "LightCurSprite" :
                        	int pos = ( int )cbd.callBackContent;
                        	SetHighLight( pos );
                        	break;
                	case "DrakAllSprite" :
                       		DarkAllItem();
                        	break;
			case "Replace1" :
				TUserUnit tuu1 = cbd.callBackContent as TUserUnit;
				ReplaceItemView(1, tuu1 );
				break;
			case "Replace2" :
				TUserUnit tuu2 = cbd.callBackContent as TUserUnit;
				ReplaceItemView(2, tuu2 );
                        	break;
			case "Replace3" :
				TUserUnit tuu3 = cbd.callBackContent as TUserUnit;
				ReplaceItemView(3, tuu3 );
                        	break;
			case "Replace4" :
				TUserUnit tuu4 = cbd.callBackContent as TUserUnit;
				ReplaceItemView(4, tuu4 );
                        	break;
                default:
                        break;
                }
                
        }

	void ReplaceItemView(int pos, TUserUnit tuu){
		Debug.Log("PartyPagePanel.ReplaceItemView(), Start...");

		ChangeTexure( pos , tuu.UnitInfo.GetAsset(UnitAssetType.Avatar));

		Debug.Log("PartyPagePanel.ReplaceItemView(), End...");
	}
	
}
