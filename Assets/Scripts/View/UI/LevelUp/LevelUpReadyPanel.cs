using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;
public class LevelUpReadyPanel: UIComponentUnity {
	UILabel hpLabel;
	UILabel atkLabel;
	UILabel expNeedLabel;
	UILabel expCurGotLabel;
	UILabel cionNeedLabel;
	GameObject curFocusTab;
	GameObject baseTab;
	GameObject friendTab;
	GameObject materialTab;
	UIImageButton levelUpButton;
	List<GameObject> Tabs = new List<GameObject>();
	Dictionary<int,GameObject> materialPoolDic = new Dictionary<int,GameObject>();
	UITexture baseCollectorTex;
	UITexture friendCollectorTex;
	List<UITexture> materialCollectorTex = new List<UITexture>();
	UnitItemInfo baseUnitInfo;
	UnitItemInfo[] unitItemInfo = new UnitItemInfo[4];
	TUserUnit friendUnitInfo;
	private const int CoinBase = 100;
	int _devorExp = 0;
	int devorExp {
		get {return _devorExp;}
		set {
			_devorExp = value;
			if(expCurGotLabel != null) {
				expCurGotLabel.text = value.ToString();
			}
		}
	}
	int _coin = 0;
	int coin {
		get {
			return _coin;
		}
		set {
			_coin =value;
			if(cionNeedLabel != null) {
				cionNeedLabel.text = value.ToString ();
			}
		}
	}

	float _multiple = 1;
	float multiple {
		get {return _multiple;}
		set {
			_multiple = value;
		}
	}

	public override void Init(UIInsConfig config, IUICallback origin){
        MsgCenter.Instance.AddListener (CommandEnum.LevelUpSucceed, ResetAfterLevelUp);
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		FoucsOnTab( curFocusTab );
		AddListener();

	}

	public override void HideUI(){
		base.HideUI();
		RemoveListener();
	}

    public override void ResetUIState() {
        curFocusTab = null;
        levelUpButton.isEnabled = false;
        levelUpButton.gameObject.SetActive (false);
        ClearTexture();
        ClearLabel();
        ClearData();
    }

	void InitUI(){
		InitTab();
		InitButton();
		FindInfoPanelLabel();
		FindCollectorTexture();
	}

    void ResetAfterLevelUp(object args){
        //TODO 
        levelUpButton.isEnabled = false;
        levelUpButton.gameObject.SetActive (false);
        ClearTexture(false);
        ClearData(false);
        curFocusTab = Tabs[0];
    }
	
	void UpdateBaseInfoView( UnitItemInfo itemInfo){
		UITexture tex = Tabs [0].GetComponentInChildren<UITexture> ();
		if (itemInfo == null) {
			hpLabel.text = "0";
			atkLabel.text = "0";
			expNeedLabel.text = "0";
			cionNeedLabel.text = "0";
			tex.mainTexture = null;
			MsgCenter.Instance.Invoke(CommandEnum.BaseAlreadySelect, null);
		} else {
			TUserUnit tuu = itemInfo.userUnitItem;
			TUnitInfo tu = DataCenter.Instance.GetUnitInfo(tuu.UnitID);//UnitInfo[ tuu.UnitID ];
			tex.mainTexture = tu.GetAsset(UnitAssetType.Avatar);			
			int hp = DataCenter.Instance.GetUnitValue(tu.HPType,tuu.Level);
			hpLabel.text = hp.ToString();			
			int atk =  DataCenter.Instance.GetUnitValue(tu.AttackType, tuu.Level);
			atkLabel.text = atk.ToString();			
			expNeedLabel.text = tuu.NextExp.ToString();
            coin = LevelUpTotalMoney();
			for (int i = 0; i < unitItemInfo.Length; i++) {
				if(unitItemInfo[i] != null) {
					devorExp += unitItemInfo[i].userUnitItem.MultipleDevorExp(baseUnitInfo.userUnitItem);
				}
			}
			MsgCenter.Instance.Invoke(CommandEnum.BaseAlreadySelect, itemInfo);
			FoucsOnTab(Tabs[2]);
		}
	}

	void ClearLabel(){
		hpLabel.text = UIConfig.emptyLabelTextFormat;
		atkLabel.text = UIConfig.emptyLabelTextFormat;
		expNeedLabel.text = UIConfig.emptyLabelTextFormat;
		expCurGotLabel.text = UIConfig.emptyLabelTextFormat;
		cionNeedLabel.text = UIConfig.emptyLabelTextFormat;
	}
	UITexture tex ;
	void UpdateFriendInfo(TUserUnit unitInfo){
		if (tex == null) {
			tex = Tabs [1].GetComponentInChildren<UITexture> ();
		}

		if (friendUnitInfo == unitInfo) {
			friendUnitInfo = null;
			tex.mainTexture = null;	
			CaculateDevorExp (false);
		} 
		else{
			if(friendUnitInfo != null) {
				CaculateDevorExp (false);
			}

			friendUnitInfo = unitInfo;
			tex.mainTexture = DataCenter.Instance.GetUnitInfo (friendUnitInfo.UnitID).GetAsset (UnitAssetType.Avatar); //UnitInfo [unitInfo.UnitID].GetAsset (UnitAssetType.Avatar);
			CaculateDevorExp (true);
		} 
	}

	void FindInfoPanelLabel(){
		hpLabel = FindChild< UILabel >("InfoPanel/Label_Vaule/0");
		atkLabel = FindChild< UILabel >("InfoPanel/Label_Vaule/1");
		expNeedLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/2");
		expCurGotLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/3");
		cionNeedLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/4");
	}
	
	void FindCollectorTexture(){
		baseCollectorTex = FindChild<UITexture>("Tab_Base/role");
		friendCollectorTex = FindChild<UITexture>("Tab_Friend/role");

		for( int i = 1; i <= 4; i++ ){
			string path = string.Format( "Tab_Material/Material{0}/Avatar", i );
			UITexture tex = FindChild< UITexture >( path );
			materialCollectorTex.Add( tex );
		}
	}

	void ClearTexture(bool clearBase = true){
        if (clearBase){
            baseCollectorTex.mainTexture = null;
        }
		friendCollectorTex.mainTexture = null;
		foreach (var item in materialCollectorTex)
			item.mainTexture = null;
	}

    void ClearData(bool clearBase = true){
        if (clearBase){
            baseUnitInfo = null;
        }
		friendUnitInfo = null;
		CaculateDevorExp(false);
		devorExp = 0;
		multiple = 1;
		for (int i = 0; i < unitItemInfo.Length; i++) {
			unitItemInfo[i] = null;
		}
        coin = 0;
	}

	void InitTab()	{
		GameObject tab;

		tab = FindChild("Tab_Base");
		Tabs.Add(tab);
		
		tab = FindChild("Tab_Friend");
		Tabs.Add(tab);

		tab = FindChild("Tab_Material");
		Tabs.Add(tab);

		for (int i = 1; i < 5; i++){
			GameObject item = tab.transform.FindChild("Material" + i.ToString()).gameObject;
                        materialPoolDic.Add( i, item);
        }
                
		foreach (var item in Tabs) {
			UIEventListener.Get(item.gameObject).onClick = ClickTab;	
		}           
	}
	
	void ClickTab(GameObject tab) {
		FoucsOnTab(tab);
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	void CheckCanLevelUp() {
		if(!levelUpButton.gameObject.activeSelf) {
			levelUpButton.gameObject.SetActive(true);
		}

		bool baseBool = baseUnitInfo != null;
		bool materialBool = false;
		foreach (var item in unitItemInfo) {
			if(item != null) {
				materialBool = true;
				break;
			}
		}
		bool firendBool = friendUnitInfo != null;
		if (baseBool && materialBool && firendBool) {
		levelUpButton.isEnabled = true;
		} else {
			levelUpButton.isEnabled = false;
		}
	}

	void FoucsOnTab(GameObject focus){
        if (focus == null){
            focus = Tabs[0];
        }
		if(focus.name == "Tab_Friend") {
			if(!levelUpButton.gameObject.activeSelf)
				levelUpButton.gameObject.SetActive(true);
			CheckCanLevelUp();
		}
		else{ 
			if(levelUpButton.gameObject.activeSelf)
				levelUpButton.gameObject.SetActive(false);
		}


		foreach (var tab in Tabs){
			tab.transform.FindChild("Light_Frame").gameObject.SetActive(false);
			tab.transform.FindChild("Label_Title").GetComponent< UILabel >().color = Color.white;
		}

		curFocusTab = focus.gameObject;
		curFocusTab.transform.FindChild("Light_Frame").gameObject.SetActive(true);
		curFocusTab.transform.FindChild("Label_Title").GetComponent< UILabel >().color = Color.yellow;
		MsgCenter.Instance.Invoke(CommandEnum.PanelFocus, curFocusTab.name );
	}
	
	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.PickBaseUnitInfo, PickBaseUnitInfo );
		MsgCenter.Instance.AddListener(CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo );
	}
	
	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.PickBaseUnitInfo, PickBaseUnitInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo );
	}
	
	void InitButton(){
		levelUpButton = FindChild<UIImageButton>("Button_LevelUp");
		UIEventListener.Get( levelUpButton.gameObject ).onClick = ClickLevelUpButton;
		levelUpButton.isEnabled = false;
	}

    int LevelUpTotalMoney(){

        if (baseUnitInfo == null){
            return 0;
        }
        int totalMoney = 0;
        foreach (var item in unitItemInfo) {
            if (item != null){
                totalMoney += CoinBase * baseUnitInfo.userUnitItem.Level;
            }
        }
        return totalMoney;
    }



    bool CheckMoneyEnough(int totalMoney){
        return DataCenter.Instance.AccountInfo.Money >= totalMoney;
    }

    MsgWindowParams GetMoneyNotEnoughMsgWindowParams(int totalMoney){
        MsgWindowParams winParams = new MsgWindowParams();
        winParams.titleText = TextCenter.Instace.GetCurrentText("MoneyNotEnoughTitle");
        winParams.contentText = TextCenter.Instace.GetCurrentText("LevelUpMoneyNotEnough", totalMoney);
        winParams.btnParam = new BtnParam();
        return winParams;
    }

	void ClickLevelUpButton(GameObject go){
		List<TUserUnit> temp = PackUserUnitInfo ();
        int totalMoney = LevelUpTotalMoney();
        if (!CheckMoneyEnough(totalMoney)){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetMoneyNotEnoughMsgWindowParams(totalMoney));
            return;
        }
		ExcuteCallback (temp);
		levelUpButton.isEnabled = false;
	}
	
	List<TUserUnit> PackUserUnitInfo(){
		List<TUserUnit> pickedUserUnitInfo = new List<TUserUnit>();
		pickedUserUnitInfo.Add (baseUnitInfo.userUnitItem);
		pickedUserUnitInfo.Add (friendUnitInfo);
		//TODO add base unit info .....
		foreach (var item in unitItemInfo){
			if(item != null) {
				pickedUserUnitInfo.Add(item.userUnitItem);
			}
		}

		return pickedUserUnitInfo;
	}

	void PickBaseUnitInfo(object info){
		if( curFocusTab.name == "Tab_Base" ){
			if(info == null) {
				baseUnitInfo = null;
				CaculateDevorExp(false);
			}
			else{
				UnitItemInfo uui = info as UnitItemInfo;
				baseUnitInfo = uui;
				CaculateDevorExp(true);
			}
			UpdateBaseInfoView(baseUnitInfo);
		}
		else{
			UnitItemInfo uui = info as UnitItemInfo;
			if(!CancelMaterialClick(uui)) {
				MaterialClick(uui);
			}
			MsgCenter.Instance.Invoke(CommandEnum.ShieldMaterial, unitItemInfo);
		}
	}

	bool CancelMaterialClick(UnitItemInfo uui) {
		for (int i = 0; i < unitItemInfo.Length; i++) {
			if(unitItemInfo[i] == null) {
				continue;
			}
			if(unitItemInfo[i].Equals(uui)) {
				devorExp -= unitItemInfo[i].userUnitItem.MultipleDevorExp(baseUnitInfo.userUnitItem);
				if(baseUnitInfo != null) {
					coin -= CoinBase * baseUnitInfo.userUnitItem.Level;
				}
				unitItemInfo[i] = null;
				MsgCenter.Instance.Invoke(CommandEnum.MaterialSelect, false);
				UpdateMaterialInfoView(null,i + 1);
				return true;
			}
		}
		return false;
	}

	bool MaterialClick (UnitItemInfo uui) {
		for (int i = 0; i < unitItemInfo.Length; i++) {
			if(unitItemInfo[i] == null) {
				unitItemInfo[i] = uui;
				devorExp += unitItemInfo[i].userUnitItem.MultipleDevorExp(baseUnitInfo.userUnitItem);
				if(baseUnitInfo != null) {
					coin += CoinBase * baseUnitInfo.userUnitItem.Level;
				}
				MsgCenter.Instance.Invoke(CommandEnum.MaterialSelect, true);
				UpdateMaterialInfoView(uui,i + 1);
				return true;
			}
		}
		return false;
	}

	void UpdateMaterialInfoView( UnitItemInfo uui, int index){
		GameObject materialTab = materialPoolDic[index];
		UITexture tex = materialTab.GetComponentInChildren<UITexture>();
		if (uui == null) {
			tex.mainTexture = null;
		} else {
			tex.mainTexture = DataCenter.Instance.GetUnitInfo(uui.userUnitItem.UnitID).GetAsset(UnitAssetType.Avatar);
		}
	}

	void PickFriendUnitInfo(object info){
		TUserUnit tuu =  info as TUserUnit;
		UpdateFriendInfo(tuu);
		CheckCanLevelUp ();
	}

	protected virtual void CaculateDevorExp (bool Add) {
		if (friendUnitInfo == null || baseUnitInfo == null) {
			devorExp = System.Convert.ToInt32(_devorExp / multiple);
			multiple = 1;
			return;	
		}

		if (Add) {
			float value = DGTools.AllMultiple (baseUnitInfo.userUnitItem, friendUnitInfo);
			devorExp = System.Convert.ToInt32( _devorExp * value);
			multiple = value;	
		} 
		else {
			devorExp = System.Convert.ToInt32(_devorExp / multiple);
			multiple = 1;
		}
	}

}


