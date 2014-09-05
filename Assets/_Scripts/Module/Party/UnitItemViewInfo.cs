using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitItemInfo : MonoBehaviour{
	private GameObject _scrollItem;
	public GameObject scrollItem {
		set { _scrollItem = value; Init(); }
		get { return _scrollItem; }
	}

	public UITexture mainTexture;

    public UILabel stateLabel;

	public UISprite mask;

	public UISprite star;

	private TUserUnit _userUnitItem;

	public TUserUnit userUnitItem {
		set { _userUnitItem = value; RefreshInfo();}
		get { return _userUnitItem; }
	}
	public UISprite hightLight;

    private bool _isCollect = false;
	public bool isCollect {
		get { return _isCollect; }
		set { _isCollect = value; }
	}

	private bool _isPartyItem = false;
	public bool isPartyItem {
		get { return _isPartyItem; }
		set { _isPartyItem = value; }
	}

    public bool isSelect = false;
    public bool isEnable = false;

	public void SetMask(bool b) {
		mask.enabled = b;
		if (star != null) {
			star.enabled = b;		
		}
	}

	public void SetLabelInfo (string info) {
		stateLabel.text = info;
	}

	public void IsFavorate (int value) {
		bool b = false;
		if (value == 1) {
			b = true;		
		}
		_isCollect = b;
		if (star != null) {
			star.enabled = b;
		}
	}

	public void IsPartyItem (bool b) {
		if (b && stateLabel != null) {
			stateLabel.text = "Party";	
		}
		isPartyItem = b;
	}

	void Init() {
//		Debug.LogError ("Init start ");
		Transform trans = scrollItem.transform;
		mainTexture = trans.Find ("Texture_Avatar").GetComponent<UITexture> ();
		stateLabel = trans.Find ("Label_Party").GetComponent<UILabel> ();
		mask = trans.Find ("Mask").GetComponent<UISprite> ();
		star = trans.Find ("StarMark").GetComponent<UISprite> ();
		hightLight = trans.Find ("HighLight").GetComponent<UISprite> ();
//		UIEventListenerCustom listener = UIEventListenerCustom.Get (scrollItem);
//		listener.onClick = ClickItem;
//		listener.LongPress = LongPress;
//		Debug.LogError ("Init end ");
	}

	void RefreshInfo() {
		userUnitItem.UnitInfo.GetAsset (UnitAssetType.Avatar, o=>{
			mainTexture.mainTexture = o as Texture2D;
		});
		IsFavorate (userUnitItem.IsFavorite);
		bool isParty = DataCenter.Instance.PartyInfo.UnitIsInParty (userUnitItem.ID);
		IsPartyItem(isParty);
		bbproto.EvolveInfo ei = userUnitItem.UnitInfo.evolveInfo;
		if (ei == null || ei.materialUnitId.Count == 0) {
			SetMask (true);	
			UIEventListenerCustom listener = UIEventListenerCustom.Get (scrollItem);
			listener.onClick = null;
			listener.LongPress = null;
		} else {
			SetMask(false);
			UIEventListenerCustom listener = UIEventListenerCustom.Get (scrollItem);
			listener.onClick = ClickItem;
			listener.LongPress = LongPress;
		}
	}

	public UICallback callback;

	public void ClickItem(GameObject go) {
		if (callback != null) {
			callback(go);
		}
	}

	public void LongPress(GameObject go) {
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule);
		ModuleManager.SendMessage(ModuleEnum.UnitDetailModule, userUnitItem);
	}
}

public class UnitItemViewInfo {
	private GameObject viewItem;
	public GameObject ViewItem{
		get{
			return viewItem;
		}
		set{
			viewItem = value;
		}
	}

    private bool isEnable;
    public bool IsEnable {
        get{ return isEnable;}
		set{ isEnable = value;}
    }

    private bool isParty;
    public bool IsParty {
        get{ return isParty; }
		set{ isParty = value;
		}
    }

    private bool isCollected;
    public bool IsCollected {
        get{ return isCollected;}
		set{ isCollected = value;}
    }
        
	private Color typeColor;
	public Color TypeColor{
		get{
			return typeColor;
		}
		set{
			typeColor = value;
		}
	}

    private string crossShowTextBefore;
    public string CrossShowTextBefore {
        get {
            return crossShowTextBefore;
        }
        set {
            crossShowTextBefore = value;
        }
    }

    private string crossShowTextAfter;
    public string CrossShowTextAfter {
        get {
            return crossShowTextAfter;
        }
        set {
            crossShowTextAfter = value;
        }
    }

    private UITexture avatar;
    public UITexture Avatar {
        get {
            return avatar;
        }
    }

    private TUserUnit dataItem;
    public TUserUnit DataItem {
        get {
            return dataItem;
        }
    }

	private TFriendInfo helperItem;
	public TFriendInfo HelperItem {
		get {
			return helperItem;
		}
	}

    private UnitItemViewInfo(){}

    public static UnitItemViewInfo Create(TUserUnit dataItem) {
        UnitItemViewInfo partyUnitItemView = new UnitItemViewInfo();
        partyUnitItemView.InitWithTUserUnit(dataItem);
        return partyUnitItemView;
    }
	public static UnitItemViewInfo Create(TUserUnit dataItem, bool inAllParty) {
		UnitItemViewInfo partyUnitItemView = new UnitItemViewInfo();
		partyUnitItemView.InitWithTUserUnit(dataItem);
		if (inAllParty){
			partyUnitItemView.SetStateInAllParty();
		}
		return partyUnitItemView;
	}

	public void InitView(GameObject viewItem){
		this.ViewItem = viewItem;

		if(ViewItem == null) return;
		avatar = ViewItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
		dataItem.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
			avatar.mainTexture = o as Texture2D;
		});
	}



	public static UnitItemViewInfo Create(TFriendInfo friendItem){
		UnitItemViewInfo partyUnitItemView = new UnitItemViewInfo();
		partyUnitItemView.InitWithTFriendInfo(friendItem);
		return partyUnitItemView;
	}

	public void SetStateInAllParty(){
		IsParty = DataCenter.Instance.PartyInfo.UnitIsInParty(dataItem.ID);
	}

    public void RefreshStates(Dictionary <string, object> statesDic) {
        foreach (var key in statesDic.Keys) {
            switch (key) {
            case "collect":
                RefreshMarkState((bool)statesDic[key]);
                break;
            case "enable":
                RefreshEnableState((bool)statesDic[key]);
                break;
            case "party":
                RefreshPartyState((bool)statesDic[key]);
                break;
            case "cross":
                RefreshCrossTextState(statesDic[key] as List<string>);
                break;
            default:
                break;
            }
        }
    }
    private void RefreshCrossTextState(List<string> textList) {
        this.crossShowTextBefore = textList[ 0 ];
        this.crossShowTextAfter = textList[ 1 ];
    }

    private void InitWithTUserUnit(TUserUnit dataItem) {
        InitDataItem(dataItem);
        InitWithArgs();
//        GetAvatar();
		GetTypeColor();
    }

	private void InitWithTFriendInfo(TFriendInfo dataItem) {
		this.dataItem = dataItem.UserUnit;
		this.helperItem = dataItem;
		InitWithArgs();
//		GetAvatar();
		GetTypeColor();
	}
       
    private void InitDataItem(TUserUnit dataItem) {
        this.dataItem = dataItem;
    }

    private void InitCrossShowText() {
        crossShowTextBefore = dataItem.Level.ToString();
        crossShowTextAfter = (dataItem.AddHP + dataItem.AddAttack).ToString();
    }

    private void InitWithArgs() {
        Dictionary <string, object> initArgs = new Dictionary<string, object>();
        initArgs.Add("collect", false);
        initArgs.Add("enable", false);
        if (DataCenter.Instance.PartyInfo == null || dataItem == null) {
            Debug.LogError("InitWithArgs(), GlobalData.PartyInfo == null, return");
            return;
        }
        initArgs.Add("party", DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(dataItem.ID));

        List<string> textList = new List<string>();
        textList.Add(dataItem.Level.ToString());
        textList.Add((dataItem.AddHP + dataItem.AddAttack).ToString());
        initArgs.Add("cross", textList);
        RefreshStates(initArgs);
    }
	
    private void GetAvatar() {
//        avatar = dataItem.UnitInfo.GetAsset(UnitAssetType.Avatar);
    }

	private void GetTypeColor(){
		switch (dataItem.UnitInfo.Type){
			case bbproto.EUnitType.UFIRE : 
				typeColor = Color.red;
				break;
			case bbproto.EUnitType.ULIGHT : 
				typeColor = Color.yellow;
				break;
			case bbproto.EUnitType.UNONE :
				typeColor = Color.gray;
				break;
			case bbproto.EUnitType.UDARK :
				typeColor = Color.grey;
				break;
			case bbproto.EUnitType.UWATER : 
				typeColor = Color.cyan;
				break;
			case bbproto.EUnitType.UWIND : 
				typeColor = Color.green;
				break;
			default:
				typeColor = Color.white;
				break;
		}
		typeColor.a = 0.65f;
//		Debug.LogError("typeColor : " + typeColor.ToString());

	}
	
    private void RefreshPartyState(bool state) {
        this.isParty = state;
    }

    private void RefreshMarkState(bool state) {
        this.isCollected = state;
	}
		
    private void RefreshEnableState(bool state) {
        this.isEnable = state;
    }

}

