using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitLevelupAndEvolveView : ViewBase {

	private UIButton levelUpButton;

	private UIButton autoSelect;

	private int moneyNeed = 0;
	private UILabel moneyNeedLabel;

	private UILabel getExpLabel;

	private UILabel needExpLabel;

	private UILabel beforeLvLabel;
	private UILabel afterLvLabel;
	private UILabel beforeAtkLabel;
	private UILabel afterAtkLabel;
	private UILabel beforeHpLabel;
	private UILabel afterHpLabel;
	private UITexture unitBodyTex;

	private UILabel titleLabel;

	private UserUnit baseUserUnit;
	private FriendInfo friendInfo;
	private const int CoinBase = 100;

	private List<LevelUpItem> selectedItem = new List<LevelUpItem>();

	private List<UserUnit> currMatList;

	enum ShowType{
		None,
		LevelUp,
		Evolve,
	}

	private GameObject levelUpEffect = null;
	private GameObject swallowEffect = null;
	private GameObject linhunqiuEffect = null;
	private GameObject evolveEffect = null;


	private ShowType currType = ShowType.None;

	//top
	private UISprite type;
	private UILabel cost;
	private UILabel number;
	private UILabel unitName;
	private UISprite lightStar;
	private UISprite grayStar;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		for (int i = 1; i <= 5; i++) {	//gameobject name is 1 ~ 6.

			LevelUpItem pui = FindChild<LevelUpItem>("Bottom/LevelUp/Items/" + i.ToString());
			selectedItem.Add(pui);

			pui.SetData<UserUnit>(null,OnClickDelItem as DataListener);

 			if(i == 5){
				pui.callback = SelectedFriendCallback;
				pui.PartyLabel.text = TextCenter.GetText("Text_Friend");
			}else{
				pui.callback = SelectedItemCallback;
				pui.PartyLabel.text = TextCenter.GetText("Text_Material");
			}
			

		}

		currMatList = new List<UserUnit> ();
		unitBodyTex = FindChild< UITexture >("Bottom/detailSprite");
		levelUpButton = FindChild<UIButton>("Bottom/LevelUp/Button_LevelUp/");
		autoSelect = FindChild<UIButton> ("Bottom/LevelUp/Button_AutoSelect");
		FindChild ("Bottom/LevelUp/Button_LevelUp/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_Level_Up");
		FindChild ("Bottom/LevelUp/Button_AutoSelect/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_AutoSelect");
		
		UIEventListenerCustom.Get (levelUpButton.gameObject).onClick = LevelUpCallback;
		UIEventListenerCustom.Get (autoSelect.gameObject).onClick = AutoSelectCallback;

		UIEventListenerCustom.Get (FindChild ("Top/Button_Back")).onClick = GoBack;
		UIEventListenerCustom.Get (FindChild ("Top/Button_Next")).onClick = GoNext;
//		levelUpButton.isEnabled = false;
//		autoSelect.isEnabled = false;

		moneyNeedLabel = FindChild<UILabel> ("Bottom/LevelUp/MoneyLabel");
		getExpLabel = FindChild<UILabel> ("Bottom/LevelUp/GetExpLabel");
		needExpLabel = FindChild<UILabel> ("Bottom/LevelUp/NeedExpLabel");
		beforeLvLabel = FindChild<UILabel> ("Top/BeforeLv");
		afterLvLabel = FindChild<UILabel> ("Top/AfterLv");
		beforeAtkLabel = FindChild<UILabel> ("Top/BeforeAtk");
		afterAtkLabel = FindChild<UILabel> ("Top/AfterAtk");
		beforeHpLabel = FindChild<UILabel> ("Top/BeforeHp");
		afterHpLabel = FindChild<UILabel> ("Top/AfterHp");
		titleLabel = FindChild<UILabel> ("Top/Title");

		//top
		cost = transform.FindChild("Top/Cost").GetComponent<UILabel>();
		number = transform.FindChild("Top/No").GetComponent<UILabel>();
		unitName = transform.FindChild("Top/Name").GetComponent<UILabel>();
		type = transform.FindChild ("Top/Type").GetComponent<UISprite> ();
		grayStar = transform.FindChild ("Top/Star2").GetComponent<UISprite> ();
		lightStar = transform.FindChild ("Top/Star2/Star1").GetComponent<UISprite> ();
		parent = FindChild("Bottom/LevelUp/Items");


		////---------------Effect
		if (levelUpEffect == null) {
			
			ResourceManager.Instance.LoadLocalAsset("Effect/effect/LevelUpEffect" , o =>{
				levelUpEffect = o as GameObject;
			});	
		}
		if (swallowEffect == null) {
			ResourceManager.Instance.LoadLocalAsset( "Effect/effect/level_up01" , o =>{
				swallowEffect = o as GameObject;
			});
		}
		if (linhunqiuEffect == null) {
			ResourceManager.Instance.LoadLocalAsset( "Effect/effect/linhunqiu1" , o =>{
				linhunqiuEffect = o as GameObject;
			});	
		}
		if (evolveEffect == null) {
			ResourceManager.Instance.LoadLocalAsset( "Effect/effect/evolve" , o =>{
				evolveEffect = o as GameObject;
			});	
		}
	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		if (viewData != null) {
			if(viewData.ContainsKey("unit_info")){
				SelectUnit(viewData["unit_info"] as UserUnit,(int)viewData["unit_index"]);
			}else if(viewData.ContainsKey("friend_info")){
				SelectFriend(viewData["friend_info"] as FriendInfo);
			}else if(viewData.ContainsKey("level_up")){
				baseUserUnit = viewData["level_up"] as UserUnit;
				ShowAvatar(baseUserUnit);
				ShowUIByType(ShowType.LevelUp);
				ResetLevelUpData();
				ShowUnitInfo();
			}else if(viewData.ContainsKey("evolve")){
				baseUserUnit = viewData["level_up"] as UserUnit;
				ResetLevelUpData();
				ShowUIByType(ShowType.Evolve);
			}
		}
		viewData.Clear ();
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}

	void SelectedItemCallback(object data){
		LevelUpItem item = data as LevelUpItem;
//		 = new List<UserUnit> ();
		ModuleManager.Instance.HideModule (ModuleEnum.UnitLevelupAndEvolveModule);
		ModuleManager.Instance.ShowModule(ModuleEnum.UnitSelectModule,"type","level_up","index",selectedItem.IndexOf (item),"list",currMatList);
	}

	void OnClickDelItem(object data){

		LevelUpItem item = data as LevelUpItem;
		int index = selectedItem.IndexOf (item);
		if (item.UserUnit != null) {
			currMatList.Remove(item.UserUnit);
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_success);
			item.SetData<UserUnit>(null);

		}else{
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click_invalid);
		}

		RefreshLevelUpInfo ();
	}

	void SelectedFriendCallback(object data){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		ModuleManager.Instance.HideModule (ModuleEnum.UnitLevelupAndEvolveModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.FriendSelectModule,"type","level_up");
	}

	void LevelUpCallback(GameObject go) {
		if (DataCenter.Instance. UserData.AccountInfo.money < moneyNeed) {
			TipsManager.Instance.ShowTipsLabel("not enough money");
			return;
		}

		List<uint> unitIds = new List<uint> ();
		foreach (var item in selectedItem.GetRange (0, 4)) {
			if(item.UserUnit == null)
				continue;
			unitIds.Add(item.UserUnit.uniqueId);
		}

		if (friendInfo == null) {
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("LevelUp_NeedTitle"),TextCenter.GetText("LevelUp_NeedFriend"),TextCenter.GetText("OK"));
			return;
		}
		if (unitIds.Count <= 0) {
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("LevelUp_NeedTitle"),TextCenter.GetText("LevelUp_NeedMaterial"),TextCenter.GetText("OK"));
			return;
		}
		UnitController.Instance.LevelUp (NetCallback,baseUserUnit.uniqueId,unitIds,friendInfo.userId,selectedItem[4].UserUnit);
		
	}
	
	void AutoSelectCallback(GameObject go){

		List<KeyValuePair<int, UserUnit>> sortDic = new List<KeyValuePair<int, UserUnit>> ();
		foreach (var item in DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ()) {
			if(!DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(item) && item.isFavorite == 0 && ! currMatList.Contains(item)){
				sortDic.Add(new KeyValuePair<int, UserUnit>(item.MultipleMaterialExp(baseUserUnit),item));
			}
		}
		sortDic.Sort ((KeyValuePair<int,UserUnit> v1, KeyValuePair<int,UserUnit> v2) => {
			return v1.Key.CompareTo(v2.Key);
		});
		foreach (var item in sortDic) {

		}
		sortDic.Clear ();
		
		
	}

	private int expGot = 0;
	void RefreshLevelUpInfo(){
		int totalMoney = 0;
		for (int i = 1; i < 5; i++) {	//material index range
			if (selectedItem[i-1].UserUnit != null){
				totalMoney ++;
			}
		}

		moneyNeed = baseUserUnit.level * CoinBase * totalMoney;
		moneyNeedLabel.text =  moneyNeed + "";


		expGot = System.Convert.ToInt32(LevelUpCurExp() * (friendInfo == null ? 1 : DGTools.AllMultiple (baseUserUnit,friendInfo.UserUnit)) ); 
		getExpLabel.text = expGot.ToString();
		
		UnitInfo tu = baseUserUnit.UnitInfo;
		int toLevel = tu.GetLevelByExp (expGot + baseUserUnit.exp);
		if (expGot == 0) {
			beforeHpLabel.text = baseUserUnit.Hp + "";// + "->" + tu.GetHpByLevel(toLevel);
			beforeAtkLabel.text =  baseUserUnit.Attack + "";// + "->" + tu.GetAtkByLevel(toLevel);
			beforeLvLabel.text = baseUserUnit.level + "";// + "->" + toLevel;
		}else{
			afterHpLabel.text =  tu.GetHpByLevel(toLevel).ToString() ;
			afterAtkLabel.text =   tu.GetAtkByLevel(toLevel).ToString();
			afterLvLabel.text =  toLevel + "";
		}
	}

	void ShowUnitInfo(){
		number.text = baseUserUnit.unitId.ToString();
		if (number.text.Length < 3) {
			number.text = (number.text.Length == 1) ? ("00" + number.text) : ("0" + number.text);
		}

		unitName.text = TextCenter.GetText ("UnitName_" + baseUserUnit.unitId);
		
		type.spriteName = "type_" + baseUserUnit.UnitInfo.UnitType;
		
		cost.text = baseUserUnit.UnitInfo.cost.ToString();
		
		int len = 0;
		if (baseUserUnit.UnitInfo.maxStar > baseUserUnit.UnitInfo.rare) {
			grayStar.enabled = true;
			grayStar.width = (baseUserUnit.UnitInfo.maxStar - baseUserUnit.UnitInfo.rare) * 28;
			len = 2*baseUserUnit.UnitInfo.rare - baseUserUnit.UnitInfo.maxStar;
		} else {
			grayStar.enabled = false;
			len = baseUserUnit.UnitInfo.rare;
		}
		lightStar.width = baseUserUnit.UnitInfo.rare*29;
		grayStar.transform.localPosition = new Vector3(len * 15,-82,0);
	}

	void SelectFriend(FriendInfo friendInfo) {
		if (friendInfo == null) {
			return;	
		}
		this.friendInfo = friendInfo;
		selectedItem [4].SetData<UserUnit> (friendInfo.UserUnit);
	}

	void SelectUnit(UserUnit unitInfo,int index){
		if (selectedItem [index].UserUnit != null) {
			currMatList.Remove(selectedItem [index].UserUnit);
		}
		selectedItem [index].SetData<UserUnit> (unitInfo);
		currMatList.Add (unitInfo);
		RefreshLevelUpInfo ();
	}
	
	int LevelUpCurExp () {
		int devorExp = 0;
		for (int i = 1; i < 6; i++) {	//material index range
			if (selectedItem[i-1].UserUnit != null){
				devorExp += selectedItem[i-1].UserUnit.MultipleMaterialExp(baseUserUnit);
			}
		}
		//		Debug.LogError (devorExp);
		return devorExp;
	}
	
	void NetCallback(object data) {
		//TODO: moving to logic
		if (data != null) {
			bbproto.RspLevelUp rspLevelUp = data as bbproto.RspLevelUp;
			if (rspLevelUp.header.code != (int)ErrorCode.SUCCESS) {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow (rspLevelUp.header.code);
				ResetLevelUpData();
				return;
			}
			
//			DataCenter.Instance.FriendData.useFriend.usedTime = GameTimer.GetInstance().GetCurrentSeonds();
			
			DataCenter.Instance.UserData.AccountInfo.money = (int)rspLevelUp.money;
//			uint userId = DataCenter.Instance.UserData.UserInfo.userId;
//			DataCenter.Instance.oldUserUnitInfo = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit (rspLevelUp.blendUniqueId);
//			DataCenter.Instance.levelUpMaterials.Clear();
			
			//删除消耗的材料
			for (int i = 0; i < rspLevelUp.partUniqueId.Count; i++) {
				uint uniqueID = rspLevelUp.partUniqueId[i];
				UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.Get(uniqueID);
//				DataCenter.Instance.levelUpMaterials.Add(tuu);
				//				Debug.LogError("NetCallback delete unit : " + uniqueID);
				DataCenter.Instance.UnitData.UserUnitList.DelMyUnit(uniqueID);
			}
			
			//更新强化后的base卡牌数据
			DataCenter.Instance.UnitData.UserUnitList.UpdateMyUnit(rspLevelUp.baseUnit);
			baseUserUnit = rspLevelUp.baseUnit;
			StartCoroutine(SwallowUserUnit());
			
//			ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"levelup",rspLevelUp);
			
			//			MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
			
			//			MsgCenter.Instance.Invoke (CommandEnum.LevelUpSucceed, rspLevelUp.blendUniqueId);
		}
	}

	void ShowAvatar(UserUnit info){
		ResourceManager.Instance.GetAvatar( UnitAssetType.Profile,info.unitId, o=>{
			unitBodyTex.alpha = 0;
			unitBodyTex.GetComponent<TweenAlpha>().enabled = true;
			unitBodyTex.GetComponent<TweenAlpha>().ResetToBeginning();
			unitBodyTex.GetComponent<TweenScale>().enabled = true;
			unitBodyTex.GetComponent<TweenScale>().ResetToBeginning();
			DGTools.ShowTexture(unitBodyTex, o as Texture2D);
		});
	}

	private void GoBack(GameObject obj){
		ModuleManager.Instance.HideModule (ModuleEnum.UnitLevelupAndEvolveModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitsListModule);

	}

	private void GoNext(GameObject obj){
		ShowUIByType (currType == ShowType.Evolve ? ShowType.LevelUp : ShowType.Evolve);
	}

	private void ShowUIByType(ShowType type){
		if (currType == type)
			return;
		currType = type;
		if (type == ShowType.Evolve) {
			titleLabel.text = TextCenter.GetText("Evolve_Title");
		}else if(type == ShowType.LevelUp){
			titleLabel.text = TextCenter.GetText("LevelUp_Title");
		}
	}

	private void ResetLevelUpData(){
		foreach (var item in selectedItem) {
			item.SetData<UserUnit>(null);
		}
		currMatList.Clear ();
		RefreshLevelUpInfo();
	}

	GameObject parent;

	//-----------------level up effect

	//center
	
	IEnumerator SwallowUserUnit () {
		Vector3 tarPos = unitBodyTex.transform.localPosition + new Vector3 (0, unitBodyTex.height / 2, 0);
		Debug.Log ("pos:------------" + tarPos);
		yield return new WaitForSeconds(1f);
		for (int i = 0; i < 5; i++) {
			if(selectedItem[i].UserUnit != null){
				GameObject obj = NGUITools.AddChild(parent,selectedItem[i].gameObject);
				obj.transform.localPosition = selectedItem[i].transform.localPosition;
				iTween.ScaleTo(obj, iTween.Hash("x",0f,"y", 0f, "time", 0.2f));
				selectedItem[i].SetData<UserUnit>(null);
				yield return new WaitForSeconds(0.2f);


				GameObject lq = NGUITools.AddChild(parent, linhunqiuEffect);
				lq.transform.localPosition = obj.transform.localPosition;
				lq.transform.localScale = Vector3.zero;
				Destroy(obj);
				iTween.ScaleTo(lq, iTween.Hash("x",1f,"y", 1f, "time", 0.2f));
				yield return new WaitForSeconds(0.2f);
				iTween.MoveTo(lq, iTween.Hash("position",tarPos , "time", 0.3f, "islocal", true));
				yield return new WaitForSeconds(0.3f);

				AudioManager.Instance.PlayAudio(AudioEnum.sound_devour_unit);
				
				Destroy(lq);
				GameObject se = NGUITools.AddChild(parent, swallowEffect);
				se.transform.localPosition = tarPos;
				yield return new WaitForSeconds(0.4f);
				Destroy(se);
			}
		}
		RefreshLevelUpInfo();
	}
	
}
