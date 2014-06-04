using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class EvolveDecoratorUnity : UIComponentUnity {
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI ();
	}
	
	public override void ShowUI () {
		bool b = friendWindow != null && friendWindow.isShow;
		if (b) {
			friendWindow.gameObject.SetActive (true);
	
		} else {
			SetObjectActive(true);
		}

		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.selectUnitMaterial, selectUnitMaterial);
		NoviceGuideStepEntityManager.Instance ().StartStep ();
	}
	
	public override void HideUI () {
		if (UIManager.Instance.nextScene == SceneEnum.UnitDetail) {
			fromUnitDetail = true; 
			if (friendWindow != null && friendWindow.gameObject.activeSelf) {
				friendWindow.gameObject.SetActive (false);
			}
		}else if (friendWindow != null) {
			friendWindow.HideUI ();
		}
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.selectUnitMaterial, selectUnitMaterial);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void CallbackView (object data) {
		Dictionary<string, object> dataDic = data as Dictionary<string, object>;
		List<KeyValuePair<string,object>> datalist = new List<KeyValuePair<string, object>> ();

		foreach (var item in dataDic) {
			datalist.Add(new KeyValuePair< string, object >( item.Key, item.Value));
		}
		for (int i = datalist.Count - 1; i > -1; i--) {
			DisposeCallback(datalist[i]);
		}
	}

	public override void ResetUIState () {
		state = 1;
		if(baseItem != null)
			baseItem.Refresh( null);
		if(friendItem != null)
			friendItem.Refresh( null);
		if (materialItem != null) {
			foreach (var item in materialItem.Values) {
				if(item == null) {
					continue;
				}
				item.Refresh(null);
			}
		}
		if (materialUnit != null) 
			materialUnit.Clear ();	
		prevItem = null;


	}
	
	public void SetUnitDisplay(GameObject go) {
		unitDisplay = go;
	}

	//==========================================interface end ==========================

	public const string BaseData = "SelectData";
	public const string MaterialData = "MaterialData";
	private const string preAtkLabel = "PrevAtkLabel";
	private const string preHPLabel = "PrevHPLabel";
	private const string evolveAtkLabel = "NextAtkLabel";
	private const string evolveHPLabel = "NextHPLabel";
	private const string needLabel = "NeedLabel";
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
//		Debug.LogError ("haveBase : " + haveBase + " havefriend : " + haveFriend + " havematerial : " + haveMaterial);
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

	void DisposeMaterial (List<TUserUnit> itemInfo) {
		if (itemInfo == null || baseItem == null) {
			return;	
		}
		List<uint> evolveNeedUnit = new List<uint> (baseItem.userUnit.UnitInfo.evolveInfo.materialUnitId);

//		for (int i = 0; i < evolveNeedUnit.Count; i++) {
//			Debug.LogError("evolveNeedUnit id : " + evolveNeedUnit[i]);
//		}
//
//		Debug.LogError ("itemInfo count : " + itemInfo.Count);
//
//		for (int i = 0; i < itemInfo.Count; i++) {
//			if(itemInfo[i] == null) 
//				continue;
//			Debug.LogError("itemInfo id : " + itemInfo[i].UnitInfo.ID);
//		}

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
//		Debug.LogError ("DisposeSelectData : " + tuu);
		if(tuu == null ) {
			return;
		}

		if (baseItem.userUnit != null && baseItem.userUnit.ID == tuu.ID) {
			return;	
		}
	
		if (state == 1 && tuu.UnitInfo.evolveInfo != null) {
			ClearMaterial();
			baseItem.Refresh(tuu);
			showInfoLabel[preAtkLabel].text = tuu.Attack.ToString();
			showInfoLabel[preHPLabel].text = tuu.Hp.ToString();
			MsgCenter.Instance.Invoke(CommandEnum.UnitDisplayBaseData, tuu);
			CheckCanEvolve();
		}
	}

	void ClearMaterial () {
		foreach (var item in evolveItem.Values) {
			item.Refresh(null);
		}
		materialUnit.Clear();
	}
 
	void LongPress (GameObject go) {
		EvolveItem ei = evolveItem [go];

		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, ei.userUnit);
	}

	int state = 0;
	void ClickItem (GameObject go) {
		if (baseItem.userUnit == null) {
			return;	
		}

		ClickIndex = System.Int32.Parse (go.name);
		switch (go.name) {
		case "1":
			if(state == 1) {
				return;
			}
			state = 1;
			break;
		case "2":
			if(baseItem == null) {
				return;
			}
			state =2;
			break;
		case "3":
			if(baseItem == null) {
				return;
			}
			state =3;
			break;
		case "4":
			if(baseItem == null) {
				return;
			}
			state =4;
			break;
		case "5":
			if(state == 5) {
				return;
			}
			TUserUnit tuu = null;
			if(baseItem != null) {
				tuu = baseItem.userUnit;
			}
			ShieldEvolveButton(true);
			state =5;
			break;
		}
		CheckCanEvolve();
		if (prevItem != null) {
			prevItem.highLight.enabled = false;	
		}
		EvolveItem ei = evolveItem [go];
		ei.highLight.enabled = true;
		prevItem = ei;
		MsgCenter.Instance.Invoke (CommandEnum.UnitDisplayState, state);
		if (state == 5) {
			EnterFriend();	
		}
	}
	
	void InitUI () {
		InitItem ();
		InitLabel ();
	}

	void EnterFriend() {
		if (friendWindow == null) {
			friendWindow = DGTools.CreatFriendWindow();
			if(friendWindow == null) {
				return;
			}
		}
		SetObjectActive (false);
		friendWindow.selectFriend = SelectFriend;
		friendWindow.ShowUI ();
		state = 0;
	}

	void SetObjectActive(bool active) {
		if (gameObject.activeSelf != active) {
			gameObject.SetActive (active);
		}

		if (unitDisplay != null && unitDisplay.activeSelf != active) {
			unitDisplay.SetActive(active);
		}
	}

	void SelectFriend(TFriendInfo friendInfo) {
		SetObjectActive (true);
		this.friendInfo = friendInfo;
		friendItem.Refresh (friendInfo.UserUnit);
		CheckCanEvolve ();
	}
	
	void InitItem () {
		string path = rootPath + "/title/";
		for (int i = 1; i < 6; i++) {
			GameObject go = FindChild(path + i);
			UIEventListenerCustom ui = UIEventListenerCustom.Get(go);
			ui.LongPress = LongPress;
			ui.onClick = ClickItem;

			EvolveItem ei = new EvolveItem(i, go);
//			ei.index = i;
//			ei.itemObject = go;
//			ei.showTexture = go.transform.Find("Texture").GetComponent<UITexture>();
//			ei.highLight = go.transform.Find("Light").GetComponent<UISprite>();
//			ei.borderSprite = go.transform.Find("Sprite_Avatar_Border").GetComponent<UISprite>();
//			ei.bgprite = go.transform.Find("Sprite_Avatar_Bg").GetComponent<UISprite>(); 
//			ei.boxCollider = go.GetComponent<BoxCollider>();
//			ei.highLight.enabled = false;
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
		string path = rootPath + "/info_panel/";

		UILabel temp = transform.Find(path + preAtkLabel).GetComponent<UILabel>();
		showInfoLabel.Add (preAtkLabel, temp);

		temp = transform.Find(path + preHPLabel).GetComponent<UILabel>();
		showInfoLabel.Add (preHPLabel, temp);

		temp = transform.Find(path + evolveAtkLabel).GetComponent<UILabel>();
		showInfoLabel.Add (evolveAtkLabel, temp);

		temp = transform.Find(path + evolveHPLabel).GetComponent<UILabel>();
		showInfoLabel.Add (evolveHPLabel, temp);

		temp = transform.Find(path + needLabel).GetComponent<UILabel>();
		showInfoLabel.Add (needLabel, temp);

		evolveButton = FindChild<UIButton> ("Evolve");
		ShieldEvolveButton (false);
		UIEventListener.Get (evolveButton.gameObject).onClick = Evolve;
	}


	void Evolve(GameObject go) {
		List<ProtobufDataBase> evolveInfoList = new List<ProtobufDataBase> ();
		evolveInfoList.Add (baseItem.userUnit);
		evolveInfoList.Add (friendInfo);
		foreach (var item in materialItem.Values) {
			TUserUnit tuu = item.userUnit;
			if(tuu != null) {
				evolveInfoList.Add(tuu);
			}
		}

		ExcuteCallback (evolveInfoList);
	}

	void ShieldEvolveButton (bool b) {
		evolveButton.isEnabled = b;
	}
}

public class EvolveItem {
	public GameObject itemObject;
	public BoxCollider boxCollider;
	public TUserUnit userUnit;
	public UITexture showTexture;
	public UILabel haveLabel;
	public UISprite maskSprite;
	public UISprite highLight;
	public UISprite borderSprite;
	public UISprite bgprite;
	public int index;
	public bool HaveUserUnit = true;

	public EvolveItem (int index, GameObject target) {
		index = index;
		itemObject = target;
		Transform trans = target.transform;
		showTexture = trans.Find("Texture").GetComponent<UITexture>();
		highLight = trans.Find("Light").GetComponent<UISprite>();
		borderSprite = trans.Find("Sprite_Avatar_Border").GetComponent<UISprite>();
		bgprite = trans.Find("Sprite_Avatar_Bg").GetComponent<UISprite>(); 
		boxCollider = target.GetComponent<BoxCollider>();
		highLight.enabled = false;

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
			showTexture.mainTexture = null;
			borderSprite.enabled = false;

			bgprite.spriteName = "unit_empty_bg";
		} else {
			borderSprite.enabled = true;
			ShowUnitType();
			Texture2D tex = userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
			showTexture.mainTexture = tex;
		}
	}

	void ShowShield(bool show) {
		if(maskSprite != null && maskSprite.enabled != show) {
			maskSprite.enabled = show;
		}
		if(haveLabel != null && haveLabel.enabled != show) {
			haveLabel.enabled = show;
		}
		if (boxCollider != null && boxCollider.enabled == show) {
			boxCollider.enabled = !show;
		}
	}

	private void ShowUnitType(){
		switch (userUnit.UnitInfo.Type){
		case EUnitType.UFIRE :
			bgprite.spriteName = "avatar_bg_1";
			borderSprite.spriteName = "avatar_border_1";
			break;
		case EUnitType.UWATER :
			bgprite.spriteName = "avatar_bg_2";
			borderSprite.spriteName = "avatar_border_2";
			
			break;
		case EUnitType.UWIND :
			bgprite.spriteName = "avatar_bg_3";
			borderSprite.spriteName = "avatar_border_3";
			
			break;
		case EUnitType.ULIGHT :
			bgprite.spriteName = "avatar_bg_4";
			borderSprite.spriteName = "avatar_border_4";
			
			break;
		case EUnitType.UDARK :
			bgprite.spriteName = "avatar_bg_5";
			borderSprite.spriteName = "avatar_border_5";
			
			break;
		case EUnitType.UNONE :
			bgprite.spriteName = "avatar_bg_6";
			borderSprite.spriteName = "avatar_border_6";
			
			break;
		default:
			break;
		}
	}
}