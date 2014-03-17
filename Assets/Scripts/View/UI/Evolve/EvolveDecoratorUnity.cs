using UnityEngine;
using System.Collections.Generic;

public class EvolveDecoratorUnity : UIComponentUnity {
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo);
	}
	
	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void Callback (object data) {
		Dictionary<string, object> dataDic = data as Dictionary<string, object>;
		List<KeyValuePair<string,object>> datalist = new List<KeyValuePair<string, object>> ();

		foreach (var item in dataDic) {
			datalist.Add(new KeyValuePair< string, object >( item.Key, item.Value));
		}
		for (int i = datalist.Count - 1; i > -1; i--) {
			DisposeCallback(datalist[i]);
		}
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
	private UIImageButton evolveButton ;
	private EvolveItem baseItem;
	private EvolveItem friendItem;
	private EvolveItem prevItem = null;
	private EvolveState clickState = EvolveState.BaseState;
	private List<TUserUnit> materialUnit = new List<TUserUnit>();
	private int ClickIndex = 0;

	void PickFriendUnitInfo(object data) {
		TUserUnit tuu = data as TUserUnit;
		friendItem.Refresh (tuu);
		CheckCanEvolve ();
	}

	void CheckCanEvolve () {
		bool haveBase = baseItem.userUnit != null; 
		bool haveFriend = friendItem.userUnit != null;
		bool haveMaterial = false;
		foreach (var item in materialItem) {
			if(item.Value.userUnit != null) {
				haveMaterial = true;
				break;
			}
		}
		if (haveBase && haveFriend && haveMaterial) {
			evolveButton.isEnabled = true;
		} else {
			evolveButton.isEnabled = false;
		}
	}

	void DisposeCallback (KeyValuePair<string, object> keyValue) {
		switch (keyValue.Key) {
		case BaseData:
			TUserUnit tuu = keyValue.Value as TUserUnit;
			DisposeSelectData(tuu);
			break;
		case MaterialData:
			List<TUserUnit> itemInfo = keyValue.Value as List<TUserUnit>;
			DisposeMaterial(itemInfo);
			break;
		default:
				break;
		}
	}

	void DisposeMaterial (List<TUserUnit> itemInfo) {
		if (itemInfo == null || baseItem == null) {
			return;	
		}

		for (int i = 0; i < itemInfo.Count; i++) {
			materialItem[i + 2].Refresh(itemInfo[i]);
		}
	}

	void DisposeSelectData (TUserUnit tuu) {
		if(tuu == null) {
			return;
		}
	
		if (clickState == EvolveState.BaseState && tuu.UnitInfo.evolveInfo != null) {
			baseItem.Refresh(tuu);
			MsgCenter.Instance.Invoke(CommandEnum.UnitDisplayBaseData, tuu);
		}
	}

	void ClearMaterial () {
		foreach (var item in evolveItem.Values) {
			item.Refresh(null);
		}
		materialUnit.Clear();
	}
 
	void LongPress (GameObject go) {
//		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
//		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);
	}

	void ClickItem (GameObject go) {
		ClickIndex = System.Int32.Parse (go.name);
		switch (go.name) {
		case "1":
			if(clickState == EvolveState.BaseState) {
				return;
			}
			if(evolveButton.gameObject.activeSelf) {
				evolveButton.gameObject.SetActive(false);
			}
			clickState = EvolveState.BaseState;
			break;
		case "2":
		case "3":
		case "4":
			if(evolveButton.gameObject.activeSelf) {
				evolveButton.gameObject.SetActive(false);
			}
			if(baseItem == null) {
				return;
			}
			clickState = EvolveState.MaterialState;
			break;
		case "5":
			if(clickState == EvolveState.FriendState) {
				return;
			}
			CheckCanEvolve();
			clickState = EvolveState.FriendState;
			TUserUnit tuu = null;
			if(baseItem != null) {
				tuu = baseItem.userUnit;
			}

			if(!evolveButton.gameObject.activeSelf) {
				evolveButton.gameObject.SetActive(true);
			}

			MsgCenter.Instance.Invoke(CommandEnum.EvolveFriend, tuu);
			break;
		}
		if (prevItem != null) {
			prevItem.highLight.enabled = false;	
		}
		EvolveItem ei = evolveItem [go];
		ei.highLight.enabled = true;
		prevItem = ei;
		MsgCenter.Instance.Invoke (CommandEnum.UnitDisplayState, clickState);
	}


	void InitUI () {
		InitItem ();
		InitLabel ();
	}
	
	void InitItem () {
		string path = rootPath + "/title/";
		for (int i = 1; i < 6; i++) {
			GameObject go = FindChild(path + i);
			UIEventListenerCustom ui = UIEventListenerCustom.Get(go);
			ui.LongPress = LongPress;
			ui.onClick = ClickItem;
			EvolveItem ei = new EvolveItem();
			ei.index = i;
			ei.itemObject = go;
			ei.showTexture = go.transform.Find("Texture").GetComponent<UITexture>();
			ei.highLight = go.transform.Find("Light").GetComponent<UISprite>();
			ei.highLight.enabled = false;
			evolveItem.Add(ei.itemObject, ei);
			if(i == 1 ) {
				baseItem = ei;
				ei.highLight.enabled = true;
				clickState = EvolveState.BaseState;
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
			
			ei.haveLabel = go.transform.Find("HaveLabel").GetComponent<UILabel>();
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

		evolveButton = FindChild<UIImageButton> ("Window/Evolve");
		evolveButton.gameObject.SetActive (false);
	}
}

public enum EvolveState {
	BaseState = 0,
	MaterialState = 1,
	FriendState = 2,
}

public class EvolveItem {
	public GameObject itemObject;
	public TUserUnit userUnit;
	public UITexture showTexture;
	public UILabel haveLabel;
	public UISprite highLight;
	public int index;

	public void Refresh (TUserUnit tuu) {
		userUnit = tuu;
		if (tuu == null) {
			showTexture.mainTexture = null;
		} else {
			Texture2D tex = userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
			showTexture.mainTexture = tex;
		}
	}
}