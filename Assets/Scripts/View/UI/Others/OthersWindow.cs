using UnityEngine;
using System.Collections.Generic;

public class OthersWindow : UIComponentUnity {

//	GameObject scrollerItem;
//	DragPanel othersScroller;
//	UILabel titleLabel;

//	GameObject nickNamePanel;
//	UIButton okButton;
//	UIInput nickNameInput;
	
//	GameObject musicPanel;
//	UIButton bgmOnBtn;
//	UIButton bgmOffBtn;
//	UISprite maskOn;
//	UISprite maskOff;
//
//    GameObject resetOption;

	Dictionary< GameObject, GameObject > options = new Dictionary< GameObject, GameObject>();

	public override void Init ( UIInsConfig config, IUICallback origin ){
		FindUIElement();
//		SetOption();
		base.Init (config, origin);
	}
	
	public override void ShowUI(){
		base.ShowUI ();
//		SetUIElement();
	}
	
	public override void HideUI(){
		base.HideUI ();
//		ResetUIElement();
	}
	
	public override void DestoryUI(){
		UIEventListenerCustom.Get (FindChild ("OptionItems/Music")).onClick = null;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Nickname")).onClick = null;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Raider")).onClick = null;
		UIEventListenerCustom.Get (FindChild ("OptionItems/ResetData")).onClick = null;

		base.DestoryUI ();
	}

	void FindUIElement(){
		UIEventListenerCustom.Get (FindChild ("OptionItems/Music")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Nickname")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/Raider")).onClick = ClicItems;
		UIEventListenerCustom.Get (FindChild ("OptionItems/ResetData")).onClick = ClicItems;
	}

	void ClicItems(GameObject obj){
		switch (obj.name) {
		case "Music":
			UIManager.Instance.ChangeScene(SceneEnum.Music);
			break;
		case "Nickname":
			UIManager.Instance.ChangeScene(SceneEnum.NickName);
			break;
		case "Raider":
			UIManager.Instance.ChangeScene(SceneEnum.Raider);
			break;
		case "ResetData":
			GameDataStore.Instance.StoreData(GameDataStore.UUID, "");
			GameDataStore.Instance.StoreData(GameDataStore.USER_ID, 0);
			UIManager.Instance.ChangeScene(SceneEnum.Loading);
			break;
		}
	}
//	void SetOption() {
//		string itemPath = "Prefabs/UI/Others/OtherOptions";
//		ResourceManager.Instance.LoadLocalAsset( itemPath ,CallbackFunc);
//	}
//
//	void CallbackFunc(Object o){
//		GameObject item = o as GameObject;
//		othersScroller = new DragPanel ( "OthersScroller", scrollerItem );
//		othersScroller.CreatUI ();
//		
//		GameObject musicOption = othersScroller.AddScrollerItem( item );
//		musicOption.name = "MusicOption";
//		musicOption.GetComponentInChildren<UILabel>().text = "Music";
//		options.Add( musicOption, musicPanel );
//		
//		GameObject nickNameOption = othersScroller.AddScrollerItem( item );
//		nickNameOption.name = "NickNameOption";
//		nickNameOption.GetComponentInChildren<UILabel>().text = "NickName";
//		options.Add( nickNameOption, nickNamePanel );
//		
//		//raider
//		GameObject raiderOption = othersScroller.AddScrollerItem( item );
//		raiderOption.name = "RaiderOption";
//		raiderOption.GetComponentInChildren<UILabel>().text = "Raider";
//		options.Add( raiderOption, nickNamePanel );
//		
//		//currency
//		GameObject currencyOption = othersScroller.AddScrollerItem( item );
//		currencyOption.name = "CurrencyOption";
//		currencyOption.GetComponentInChildren<UILabel>().text = "Currency";
//		options.Add( currencyOption, nickNamePanel );
//		
//		resetOption = othersScroller.AddScrollerItem( item );
//		resetOption.name = "ResetOption";
//		resetOption.GetComponentInChildren<UILabel>().text = "Reset Data";
//		options.Add( resetOption, nickNamePanel );
//		
//		
//		
//		Transform parentTrans = FindChild("OptionItems").transform;
//		othersScroller.DragPanelView.SetScrollView(ConfigDragPanel.OthersDragPanelArgs, parentTrans);
//		
//		for(int i = 0; i < othersScroller.ScrollItem.Count; i++)
//			UIEventListener.Get( othersScroller.ScrollItem[ i ].gameObject ).onClick = ClickOption;
//	}



       



}
