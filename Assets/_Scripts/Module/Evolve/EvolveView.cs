using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class EvolveView : ViewBase {
	public override void Init ( UIConfigItem config ) {
		base.Init (config);
		InitUI ();
	}
	
	public override void ShowUI () {
		bool b = friendWindow != null && friendWindow.isShow;
		if (b) {
			MsgCenter.Instance.Invoke(CommandEnum.HideSortView, false);
			friendWindow.gameObject.SetActive (true);
		} else {
			SetObjectActive(true);
		}

		base.ShowUI ();

		MsgCenter.Instance.AddListener (CommandEnum.selectUnitMaterial, selectUnitMaterial);
		MsgCenter.Instance.AddListener (CommandEnum.FriendBack, FriendBack);
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);

		topObject.transform.localPosition = new Vector3 (0, 1000, 0);
		iTween.MoveTo(topObject, iTween.Hash("y", 0, "time", 0.4f, "islocal", true));
	}
	
	public override void HideUI () {
//		if (UIManager.Instance.nextScene == ModuleEnum.UnitDetailModule) {
//			fromUnitDetail = true; 
//			if (friendWindow != null && friendWindow.gameObject.activeSelf) {
//				friendWindow.gameObject.SetActive (false);
//			}
//		} else if (friendWindow != null) {
//			friendWindow.HideUI ();
//		}

		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.selectUnitMaterial, selectUnitMaterial);
		MsgCenter.Instance.RemoveListener (CommandEnum.FriendBack, FriendBack);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void CallbackView(params object[] args) {
		Dictionary<string, object> dataDic = args[0] as Dictionary<string, object>;
		List<KeyValuePair<string,object>> datalist = new List<KeyValuePair<string, object>> ();

		foreach (var item in dataDic) {
			datalist.Add(new KeyValuePair< string, object >( item.Key, item.Value));
		}
		for (int i = datalist.Count - 1; i > -1; i--) {
			DisposeCallback(datalist[i]);
		}
	}

//	public override void ResetUIState () {
//		state = 1;
//		if(baseItem != null)
//			baseItem.Refresh( null);
//		if(friendItem != null)
//			friendItem.Refresh( null);
//		if (materialItem != null) {
//			foreach (var item in materialItem.Values) {
//				if(item == null) {
//					continue;
//				}
//				item.Refresh(null);
//			}
//		}
//		if (materialUnit != null) 
//			materialUnit.Clear ();	
//		prevItem = null;
//	}
	
	public void SetUnitDisplay(GameObject go) {
		unitDisplay = go;
	}

	//==========================================interface end ==========================

	public const string BaseData = "SelectData";
	public const string MaterialData = "MaterialData";
	private const string hp = "HP";
	private const string type = "Type";
	private const string atk = "ATK";
	private const string race = "Race";
	private const string lv = "Lv";
	private const string coins = "Coins";
	private string rootPath = "Window";
	private Dictionary<string,UILabel> showInfoLabel = new Dictionary<string, UILabel>();
	/// <summary>
	/// 1: base. 2, 3, 4: material. 5: friend
	/// </summary>
	private Dictionary<GameObject,EvolveItem> evolveItem = new Dictionary<GameObject, EvolveItem> ();
	private Dictionary<int,EvolveItem> materialItem = new Dictionary<int, EvolveItem> ();
	private UIButton evolveButton;
	private EvolveItem baseItem;
	private EvolveItem friendItem;
	private TFriendInfo friendInfo;
	private EvolveItem prevItem = null;
	private List<TUserUnit> materialUnit = new List<TUserUnit>();
	private int ClickIndex = 0;
	private FriendWindows friendWindow;
	private bool fromUnitDetail = false;
	private GameObject unitDisplay;

	private GameObject topObject;
	
	void PickFriendUnitInfo(object data) {
		TFriendInfo tuu = data as TFriendInfo;
		friendInfo = tuu;
		friendItem.Refresh (tuu.UserUnit);
		CheckCanEvolve ();
	}

	void CheckCanEvolve () {
		bool haveBase = baseItem.userUnit != null; 
		bool haveFriend = friendItem.userUnit != null;
		bool haveMaterial = true;

		foreach (var item in materialItem.Values) {
			if(item.userUnit == null){
				continue;
			}
			if(!item.HaveUserUnit) {
				haveMaterial = false;
				break;
			}
		}

		if (haveBase && haveFriend && haveMaterial) {
			evolveButton.isEnabled = true;
		} else {
			evolveButton.isEnabled = false;
		}
	}

	void selectUnitMaterial(object data) {
		if (data == null) {
			return;	
		}
		List<TUserUnit> hasMaterial = data as List<TUserUnit>;
		if (hasMaterial == null) {
			TUserUnit hasUnit = data as TUserUnit;
//			Debug.LogError("selectUnitMaterial hasunit");
			materialItem[state].Refresh(hasUnit);
			List<TUserUnit> materialList = new List<TUserUnit>();
			for (int i = 2; i < 5; i++) {
				materialList.Add(materialItem[i].userUnit);
			}
			MsgCenter.Instance.Invoke(CommandEnum.UnitMaterialList, materialList);
			return;
		}

		DisposeMaterial (hasMaterial);
	}

	void DisposeCallback (KeyValuePair<string, object> keyValue) {
		switch (keyValue.Key) {
		case BaseData:
			TUserUnit tuu = keyValue.Value as TUserUnit;
			DisposeSelectData(tuu);
			break;
		case MaterialData:
			List<TUserUnit> itemInfo = keyValue.Value as List<TUserUnit>;
			if(itemInfo != null) {
				DisposeMaterial(itemInfo);
			}
			else{
				TUserUnit userUnit = keyValue.Value as TUserUnit;
				materialItem[state].Refresh(userUnit);
				List<TUserUnit> temp = new List<TUserUnit>();
				for (int i = 2; i < 5; i++) {
					temp.Add(materialItem[i].userUnit);
				}
				MsgCenter.Instance.Invoke(CommandEnum.UnitMaterialList, temp);
			}
			break;
		default:
				break;
		}
	}

	void FriendBack(object data) {
		if (friendWindow == null) {
			return;	
		}

		friendWindow.Back (null);
	}

	void DisposeMaterial (List<TUserUnit> itemInfo) {
		if (itemInfo == null || baseItem == null) {
			return;	
		}
		List<uint> evolveNeedUnit = new List<uint> (baseItem.userUnit.UnitInfo.evolveInfo.materialUnitId);

		for (int i = 0; i < evolveNeedUnit.Count ; i++) {
			TUserUnit material = null;
			uint ID = evolveNeedUnit[i];
			bool isHave = true;

			for (int j = 0; j < itemInfo.Count; j++) {
				if(itemInfo[j] != null && itemInfo[j].UnitInfo.ID == ID) {
					material = itemInfo[j];
					itemInfo.Remove(material);
					break;
				}
			}

			if(material == null) {
				bbproto.UserUnit uu = new bbproto.UserUnit();
				uu.unitId = ID;
				material = TUserUnit.GetUserUnit(DataCenter.Instance.UserInfo.UserId, uu);
				isHave = false;
			}
			materialItem[i + 2].Refresh(material, isHave);
		}
		CheckCanEvolve ();
	}

	void DisposeSelectData (TUserUnit tuu) {
		if(tuu == null ) {
			return;
		}

		if (baseItem.userUnit != null && baseItem.userUnit.ID == tuu.ID) {
			return;	
		}
	
		if (state == 1 && tuu.UnitInfo.evolveInfo != null) {
			ClearMaterial();
			baseItem.Refresh(tuu);
			ShowEvolveInfo(tuu);
			MsgCenter.Instance.Invoke(CommandEnum.UnitDisplayBaseData, tuu);
			CheckCanEvolve();
		}
	}

	void ShowEvolveInfo (TUserUnit tuu) {
		uint evolveUnitID = tuu.UnitInfo.evolveInfo.evolveUnitId;
		TUnitInfo tui = DataCenter.Instance.GetUnitInfo (evolveUnitID);

		showInfoLabel [hp].text = tuu.Hp + " -> [FF6666]" + tuu.CalculateHP (tui) + "[-]";
		showInfoLabel [atk].text = tuu.Attack + " -> [FF6666]" + tuu.CalculateATK (tui) + "[-]";
		showInfoLabel [lv].text = tuu.UnitInfo.MaxLevel + " -> [FF6666]" + (tui == null ? "no max info": tui.MaxLevel.ToString()) + "[-]";
		showInfoLabel [type].text = tui.UnitTypeText;
		showInfoLabel [race].text = tui.UnitRace;//GetRaceText(tui.Race);
		showInfoLabel [coins].text = (tui.MaxLevel * 500).ToString ();
	}

	private string GetRaceText(EUnitRace race){
		switch (race) {
		case EUnitRace.HUMAN:
			return TextCenter.GetText("RACE_Human");
		case EUnitRace.DRAGON:
			return TextCenter.GetText("RACE_Dragon");
		case EUnitRace.EVOLVEPARTS:
			return TextCenter.GetText("RACE_Evolveparts");
		case EUnitRace.BEAST:
			return TextCenter.GetText("RACE_Beast");
		case EUnitRace.LEGEND:
			return TextCenter.GetText("RACE_Legend");
		case EUnitRace.MONSTER:
			return TextCenter.GetText("RACE_Monster");
		case EUnitRace.MYTHIC:
			return TextCenter.GetText("RACE_Mythic");
		case EUnitRace.SCREAMCHEESE:
			return TextCenter.GetText("RACE_Screamcheese");
		case EUnitRace.UNDEAD:
			return TextCenter.GetText("RACE_Undead");
		default:
				break;
		}
		return "";
	}

	void ClearMaterial () {
		int index = 0;
		foreach (var item in evolveItem.Values) {
			if(index > 0 && index < 4)
				item.Refresh(null);
			index++;
		}
		materialUnit.Clear();
	}
 
	void LongPress (GameObject go) {
		EvolveItem ei = evolveItem [go];

		ModuleManger.Instance.ShowModule(ModuleEnum.UnitDetailModule );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, ei.userUnit);
	}

	int state = 0;
	void ClickItem (GameObject go) {
		if ( baseItem.userUnit == null) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("base_Item_Null"));
			return;
		}

		ClickIndex = System.Int32.Parse (go.name);

		if (state != 1 && state == ClickIndex) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
			return;
		}

		state = ClickIndex;

		if (state == 5) {
			ShieldEvolveButton (true);
		}

		CheckCanEvolve();

		HighLight (go);

		MsgCenter.Instance.Invoke (CommandEnum.UnitDisplayState, state);

		if (state == 5) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
			EnterFriend();	
		} else {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
		}
	}

	void HighLight(GameObject go) {
		if (prevItem != null) {
			prevItem.highLight.enabled = false;	
		}

		if (go == null) {
			return;	
		}

		EvolveItem ei = evolveItem [go];
		ei.highLight.enabled = true;
		prevItem = ei;
	}

	void InitUI () {
		InitItem ();
		InitLabel ();
	}

	void EnterFriend () {
		if (friendWindow == null) {
			friendWindow = DGTools.CreatFriendWindow();
			if(friendWindow == null) {
				return;
			}

//			friendWindow.transform.localPosition -= new Vector3(0f, 50f, 0f);
		}

		friendWindow.evolveItem = baseItem;
		SetObjectActive (false);
		friendWindow.selectFriend = SelectFriend;
		friendWindow.ShowUI ();
		state = 0;
	}

	void SetObjectActive(bool active) {
		MsgCenter.Instance.Invoke (CommandEnum.HideSortView, active);

		if (gameObject.activeSelf != active) {
			gameObject.SetActive (active);
		}

		if (unitDisplay != null && unitDisplay.activeSelf != active) {
			unitDisplay.SetActive(active);
		}
	}

	void SelectFriend(TFriendInfo friendInfo) {
		SetObjectActive (true);
//		state = 1;
		foreach (var item in evolveItem) {
			ClickItem(item.Key);
			break;
		}

		if (friendInfo == null) {
			return;	
		}

		this.friendInfo = friendInfo;
		friendItem.Refresh (friendInfo.UserUnit);
		CheckCanEvolve ();
	}
	
	void InitItem () {
		string path = rootPath + "/title/";

		topObject = FindChild (rootPath);

		for (int i = 1; i < 6; i++) {
			GameObject go = FindChild(path + i);
			UIEventListenerCustom ui = UIEventListenerCustom.Get(go);
			ui.LongPress = LongPress;
			ui.onClick = ClickItem;

			EvolveItem ei = new EvolveItem(i, go);
			evolveItem.Add(ei.itemObject, ei);
			if(i == 1 ) {
				baseItem = ei;
				ei.highLight.enabled = true;
				state = 1;
				prevItem = ei;
				continue;
			}
			else if (i == 5) {
				friendItem = ei;
				continue;
			}
			else{
				materialItem.Add(i,ei);
			}
			
//			ei.haveLabel = go.transform.Find("HaveLabel").GetComponent<UILabel>();
//			ei.maskSprite = go.transform.Find("Mask").GetComponent<UISprite>();
		}
	}

	void InitLabel () {
//		Debug.LogError("initlabel 1 ");
		string path = rootPath + "/info_panel/";
		string suffixPath = "/Info";
		UILabel temp = transform.Find(path + hp + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (hp, temp);

		temp = transform.Find(path + atk + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (atk, temp);

		temp = transform.Find(path + lv + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (lv, temp);

		temp = transform.Find(path + type + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (type, temp);

		temp = transform.Find(path + race + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (race, temp);

		temp = transform.Find(path + coins + suffixPath).GetComponent<UILabel>();
		showInfoLabel.Add (coins, temp);

		evolveButton = FindChild<UIButton> ("Evolve");
		FindChild ("Evolve/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_Evolve");
		ShieldEvolveButton (false);

		FindChild<UILabel> ("Window/info_panel/HP/label").text = TextCenter.GetText ("Text_HP");
		FindChild<UILabel> ("Window/info_panel/ATK/label").text = TextCenter.GetText ("Text_ATK");
		FindChild<UILabel> ("Window/info_panel/Lv/label").text = TextCenter.GetText ("Text_Level");
		FindChild<UILabel> ("Window/info_panel/Type/label").text = TextCenter.GetText ("Text_TYPE");
		FindChild<UILabel> ("Window/info_panel/Race/label").text = TextCenter.GetText ("Text_RACE");
		FindChild<UILabel> ("Window/info_panel/Coins/label").text = TextCenter.GetText ("Text_Coins");
		FindChild<UILabel> ("Window/title/1/Sprite_Base").text = TextCenter.GetText ("Text_BASE");
		FindChild<UILabel> ("Window/title/4/Text").text = TextCenter.GetText ("Text_Material");
		FindChild<UILabel> ("Window/title/5/Text").text = TextCenter.GetText ("Text_Friend");


		UIEventListener.Get (evolveButton.gameObject).onClick = Evolve;
	}
	
	void Evolve(GameObject go) {
		TUserUnit baseUserUnit = baseItem.userUnit;
		if (baseUserUnit.Level < baseUserUnit.UnitInfo.MaxLevel) {
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("notmaxleveltips"));
			return;
		}

		TipsManager.Instance.ShowMsgWindow( TextCenter.GetText("DownloadResourceTipTile"),TextCenter.GetText("DownloadResourceTipContent"),TextCenter.GetText("OK"),o=>{
			MsgCenter.Instance.AddListener(CommandEnum.ResourceDownloadComplete,o1 =>{
				List<ProtobufDataBase> evolveInfoList = new List<ProtobufDataBase> ();
				evolveInfoList.Add (baseItem.userUnit);
				evolveInfoList.Add (friendInfo);
				foreach (var item in materialItem.Values) {
					TUserUnit tuu = item.userUnit;
					if(tuu != null) {
						evolveInfoList.Add(tuu);
					}
				}
				//				ExcuteCallback (evolveInfoList);
			});
			ModuleManger.Instance.ShowModule(ModuleEnum.ResourceDownloadModule);
		});
		return;


	}

	void ShieldEvolveButton (bool b) {
		evolveButton.isEnabled = b;
	}
}

public class EvolveItem {
	public GameObject itemObject;
//	public BoxCollider boxCollider;
	public TUserUnit userUnit;
	public UISprite showTexture;
	public UILabel haveLabel;
	public UISprite maskSprite;
	public UISprite highLight;
	public UISprite borderSprite;
	public UISprite bgprite;
	public int index;
	public bool HaveUserUnit = true;
	private UIEventListenerCustom listener ;

	public EvolveItem (int index, GameObject target) {
		index = index;
		itemObject = target;
		Transform trans = target.transform;
		showTexture = trans.Find("Texture").GetComponent<UISprite>();
		highLight = trans.Find("Light").GetComponent<UISprite>();
		borderSprite = trans.Find("Sprite_Avatar_Border").GetComponent<UISprite>();
		bgprite = trans.Find("Sprite_Avatar_Bg").GetComponent<UISprite>(); 
//		boxCollider = target.GetComponent<BoxCollider>();
		highLight.enabled = false;
		listener = UIEventListenerCustom.Get (target);
		if (index == 1 || index == 5) {
			return;		
		}

		haveLabel = trans.Find ("HaveLabel").GetComponent<UILabel> ();
		maskSprite = trans.Find ("Mask").GetComponent<UISprite> ();
	}

	public void Refresh (TUserUnit tuu, bool isHave = true) {
		userUnit = tuu;
		HaveUserUnit = isHave;
		ShowShield (!isHave);
		if (tuu == null) {
			showTexture.spriteName = "";
			borderSprite.enabled = false;
			bgprite.spriteName = "unit_empty_bg";
			listener.LongPress = null;
		} else {
			listener.LongPress = LongPress;
			borderSprite.enabled = true;
			ShowUnitType();
//			userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
//				showTexture.mainTexture = o as Texture2D;
//			});
			ResourceManager.Instance.GetAvatarAtlas(userUnit.UnitInfo.ID, showTexture);
		}
	}

	void LongPress(GameObject target) {
		ModuleManger.Instance.ShowModule (ModuleEnum.UnitDetailModule);
		MsgCenter.Instance.Invoke (CommandEnum.ShowUnitDetail, userUnit);
	}

	void ShowShield(bool show) {
		if(maskSprite != null && maskSprite.enabled != show) {
			maskSprite.enabled = show;
		}
		if(haveLabel != null && haveLabel.enabled != show) {
			haveLabel.enabled = show;
		}
//		if (boxCollider != null && boxCollider.enabled == show) {
//			boxCollider.enabled = !show;
//		}
	}

	private void ShowUnitType(){
		switch (userUnit.UnitInfo.Type){
		case EUnitType.UFIRE :
			bgprite.spriteName = "avatar_bg_fire";
			borderSprite.spriteName = "avatar_border_fire";
			break;
		case EUnitType.UWATER :
			bgprite.spriteName = "avatar_bg_water";
			borderSprite.spriteName = "avatar_border_water";
			
			break;
		case EUnitType.UWIND :
			bgprite.spriteName = "avatar_bg_wind";
			borderSprite.spriteName = "avatar_border_wind";
			
			break;
		case EUnitType.ULIGHT :
			bgprite.spriteName = "avatar_bg_light";
			borderSprite.spriteName = "avatar_border_light";
			
			break;
		case EUnitType.UDARK :
			bgprite.spriteName = "avatar_bg_dark";
			borderSprite.spriteName = "avatar_border_dark";
			
			break;
		case EUnitType.UNONE :
			bgprite.spriteName = "avatar_bg_none";
			borderSprite.spriteName = "avatar_border_none";
			
			break;
		default:
			break;
		}
	}
}