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

	private List<LevelUpItem> levelupItem = new List<LevelUpItem>();
	private List<EvolveItem> evolveItem = new List<EvolveItem>();

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

	private bool isLevelIncrease = false;
	private int expGot = 0;

	private GameObject levelupRoot;
	private GameObject evolveRoot;

	private GameObject nextBtn;

	private GameObject evolveBtn;
	private bool canEvolve;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		for (int i = 0; i <= 5; i++) {	//gameobject name is 1 ~ 6.

			LevelUpItem pui = FindChild<LevelUpItem>("Bottom/LevelUp/Items/" + i.ToString());
			levelupItem.Add(pui);

			pui.SetData<UserUnit>(null,OnClickDelItem as DataListener);

 			if(i == 5){
				pui.callback = SelectedFriendCallback;
				pui.PartyLabel.text = TextCenter.GetText("Text_Friend");
			}else{
				pui.callback = ClickToSelectedItem;
				pui.PartyLabel.text = TextCenter.GetText("Text_Material");
			}

		}

		for (int i = 0; i < 3; i++) {
			EvolveItem ei = FindChild<EvolveItem>("Bottom/Evolve/Items/" + i.ToString());
			evolveItem.Add(ei);
		}

		currMatList = new List<UserUnit> ();
		unitBodyTex = FindChild< UITexture >("Bottom/detailSprite");
		levelUpButton = FindChild<UIButton>("Bottom/LevelUp/Button_LevelUp");
		autoSelect = FindChild<UIButton> ("Bottom/LevelUp/Button_AutoSelect");
		evolveBtn = FindChild ("Bottom/Evolve/Button_Evolve");
		FindChild<UILabel> ("Bottom/LevelUp/Button_LevelUp/Label").text = TextCenter.GetText ("Btn_Level_Up");
		FindChild <UILabel>("Bottom/LevelUp/Button_AutoSelect/Label").text = TextCenter.GetText ("Btn_AutoSelect");
		FindChild<UILabel> ("Bottom/Evolve/Button_Evolve/Label").text = TextCenter.GetText ("Btn_Evolve");
		FindChild<UILabel> ("Top/CostLabel").text = TextCenter.GetText ("Text_Cost");

		nextBtn = FindChild ("Top/Button_Next");
		UIEventListenerCustom.Get (levelUpButton.gameObject).onClick = ClickLevelUp;
		UIEventListenerCustom.Get (autoSelect.gameObject).onClick = ClickAutoSelect;
		UIEventListenerCustom.Get (evolveBtn).onClick = ClickEvolve;

		UIEventListenerCustom.Get (FindChild ("Top/Button_Back")).onClick = GoBack;
		UIEventListenerCustom.Get (nextBtn).onClick = GoNext;
//		levelUpButton.isEnabled = false;
//		autoSelect.isEnabled = false;

		moneyNeedLabel = FindChild<UILabel> ("Top/MoneyLabel");
		getExpLabel = FindChild<UILabel> ("Bottom/LevelUp/GetExpLabel");
		needExpLabel = FindChild<UILabel> ("Bottom/LevelUp/NeedExpLabel");
		beforeLvLabel = FindChild<UILabel> ("Bottom/LevelUp/BeforeLv");
		afterLvLabel = FindChild<UILabel> ("Bottom/LevelUp/AfterLv");
		beforeAtkLabel = FindChild<UILabel> ("Bottom/LevelUp/BeforeAtk");
		afterAtkLabel = FindChild<UILabel> ("Bottom/LevelUp/AfterAtk");
		beforeHpLabel = FindChild<UILabel> ("Bottom/LevelUp/BeforeHp");
		afterHpLabel = FindChild<UILabel> ("Bottom/LevelUp/AfterHp");
		titleLabel = FindChild<UILabel> ("Top/Title");

		evolveRoot = FindChild ("Bottom/Evolve");
		levelupRoot = FindChild ("Bottom/LevelUp");

		//top
		cost = transform.FindChild("Top/Cost").GetComponent<UILabel>();
		number = transform.FindChild("Top/No").GetComponent<UILabel>();
		unitName = transform.FindChild("Top/Name").GetComponent<UILabel>();
		type = transform.FindChild ("Top/Type").GetComponent<UISprite> ();
		grayStar = transform.FindChild ("Top/Star2").GetComponent<UISprite> ();
		lightStar = transform.FindChild ("Top/Star2/Star1").GetComponent<UISprite> ();
//		levelupParent = FindChild("Bottom/LevelUp/Items");


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
			}else if(viewData.ContainsKey("level_up") || viewData.ContainsKey("evolve")){
				if(viewData.ContainsKey("level_up")){
					baseUserUnit = viewData["level_up"] as UserUnit;
					ShowUIByType(ShowType.LevelUp);
					if(baseUserUnit.UnitInfo.evolveInfo == null){
						nextBtn.SetActive(false);
						for (int j = 0; j < 3; j++) {
							evolveItem[j].RefreshData(0,0,0);
						}
					}else{
						nextBtn.SetActive(true);
						ShowEvolveInfo();
					}
				}else if(viewData.ContainsKey("evolve")){
					nextBtn.SetActive(false);
					baseUserUnit = viewData["evolve"] as UserUnit;
					ShowUIByType(ShowType.Evolve);
					ShowEvolveInfo();
				}
				ShowAvatar(baseUserUnit);
				ResetLevelUpData();
				ShowUnitInfo();
			}
			NoviceGuideStepManager.Instance.StartStep(NoviceGuideStartType.UNIT_LEVELUP_EVOLVE);
		}
		viewData.Clear ();
	}

	public override void DestoryUI ()
	{
//		Destroy (levelUpEffect);
//		Destroy (swallowEffect);
//		Destroy (linhunqiuEffect);
//		Destroy (evolveEffect);

		currMatList.Clear ();
		levelupItem.Clear ();
		base.DestoryUI ();
	}

	
	private void GoBack(GameObject obj){
//		ModuleManager.Instance.HideModule (ModuleEnum.UnitLevelupAndEvolveModule);
//		ModuleManager.Instance.ShowModule (ModuleEnum.UnitsListModule);

		ModuleManager.SendMessage (ModuleEnum.SceneInfoBarModule, "back_scene");
	}
	
	private void GoNext(GameObject obj){
		ShowUIByType (currType == ShowType.Evolve ? ShowType.LevelUp : ShowType.Evolve);
	}


	void ClickToSelectedItem(object data){
		LevelUpItem item = data as LevelUpItem;
//		 = new List<UserUnit> ();
		ModuleManager.Instance.HideModule (ModuleEnum.UnitLevelupAndEvolveModule);
		ModuleManager.Instance.ShowModule(ModuleEnum.UnitSelectModule,"type","level_up","index",levelupItem.IndexOf (item),"list",currMatList,"base_info",baseUserUnit);
	}

	void OnClickDelItem(object data){

		LevelUpItem item = data as LevelUpItem;
		int index = levelupItem.IndexOf (item);
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

	void ClickLevelUp(GameObject go) {
		if (DataCenter.Instance. UserData.AccountInfo.money < moneyNeed) {
			TipsManager.Instance.ShowTipsLabel("not enough money");
			return;
		}

		List<uint> unitIds = new List<uint> ();
		foreach (var item in levelupItem.GetRange (0, 5)) {
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
		UnitController.Instance.LevelUp (NetCallback,baseUserUnit.uniqueId,unitIds,friendInfo.userId,levelupItem[5].UserUnit);
		
	}
	
	void ClickAutoSelect(GameObject go){

		List<UserUnit> sortDic = new List<UserUnit> ();
		List<UserUnit> sortDic1 = new List<UserUnit> ();
		foreach (var item in DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ()) {
			if((DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty(item) <= 0) && (item.isFavorite == 0) && !currMatList.Contains(item) && (item != baseUserUnit)){
				if(item.UnitRace == (int)EUnitRace.SCREAMCHEESE){
					sortDic.Add(item);
				}else{
					if(item.UnitInfo.rare <= 3) {
						sortDic1.Add(item);
					}
				}

			}
		}
		sortDic.Sort ((v1, v2)=> {
			return v1.MultipleMaterialExp(baseUserUnit) - v2.MultipleMaterialExp(baseUserUnit);//CompareTo(v2.Key);
		});
		sortDic1.Sort ((v1,v2) => {
			if(v1.UnitInfo.rare == v2.UnitInfo.rare)
			{
				return v1.Attack - v2.Attack;
			}
			return v1.UnitInfo.rare - v2.UnitInfo.rare;
		});

		int count = sortDic.Count;
		int count1 = sortDic1.Count + count;
		for (int i = 0; i < 5; i++) {
			if(i < count){
				SelectUnit(sortDic[i],i);
			}else if(i < count1){
				SelectUnit(sortDic1[i-count],i);
			}
		}

		List<FriendInfo> fl = DataCenter.Instance.FriendData.GetSupportFriend ();
		FriendInfo data = null;
		foreach (var item in fl) {
			if(item.UserUnit.UnitRace == baseUserUnit.UnitRace){
				if(data == null)
					data = item;
				if(item.UserUnit.UnitType == baseUserUnit.UnitType){
					data = item;
				}
			}
			Debug.Log("race: " + item.UserUnit.UnitRace + " type: " +  item.UserUnit.UnitType);
		}
		if (data == null)
			data = fl [0];
		SelectFriend (data);
	}


	void RefreshLevelUpInfo(){
		int totalMoney = 0;
		for (int i = 0; i < 5; i++) {	//material index range
			if (levelupItem[i].UserUnit != null){
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
			afterLvLabel.text = afterAtkLabel.text = afterHpLabel.text = "";
		}else{
			afterHpLabel.text =  tu.GetHpByLevel(toLevel).ToString() ;
			afterAtkLabel.text =  tu.GetAtkByLevel(toLevel).ToString();
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

		beforeHpLabel.text = baseUserUnit.Hp + "";// + "->" + tu.GetHpByLevel(toLevel);
		beforeAtkLabel.text =  baseUserUnit.Attack + "";// + "->" + tu.GetAtkByLevel(toLevel);
		beforeLvLabel.text = baseUserUnit.level + "";// + "->" + toLevel;
	}

	void SelectFriend(FriendInfo friendInfo) {
		if (friendInfo == null) {
			return;	
		}
		this.friendInfo = friendInfo;
		levelupItem [5].SetData<UserUnit> (friendInfo.UserUnit);
	}

	void SelectUnit(UserUnit unitInfo,int index){
		if (levelupItem [index].UserUnit != null) {
			currMatList.Remove(levelupItem [index].UserUnit);
		}
		levelupItem [index].SetData<UserUnit> (unitInfo);
		currMatList.Add (unitInfo);
		RefreshLevelUpInfo ();
	}
	
	int LevelUpCurExp () {
		int devorExp = 0;
		for (int i = 0; i < 6; i++) {	//material index range
			if (levelupItem[i].UserUnit != null){
				devorExp += levelupItem[i].UserUnit.MultipleMaterialExp(baseUserUnit);
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
			if(rspLevelUp.baseUnit.level > baseUserUnit.level){
				isLevelIncrease = true;
			}
			baseUserUnit = rspLevelUp.baseUnit;
			StartCoroutine(SwallowUserUnit());
			
//			ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"levelup",rspLevelUp);
			
			//			MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
			
			//			MsgCenter.Instance.Invoke (CommandEnum.LevelUpSucceed, rspLevelUp.blendUniqueId);
		}
	}

	void ShowAvatar(UserUnit info){
		if (unitBodyTex != null && (unitBodyTex.mainTexture != null) && (unitBodyTex.mainTexture.name == info.unitId.ToString ())) {
			return;
		}
		ResourceManager.Instance.GetAvatar( UnitAssetType.Profile,info.unitId, o=>{
			unitBodyTex.alpha = 0;
			unitBodyTex.GetComponent<TweenAlpha>().enabled = true;
			unitBodyTex.GetComponent<TweenAlpha>().ResetToBeginning();
			unitBodyTex.GetComponent<TweenScale>().enabled = true;
			unitBodyTex.GetComponent<TweenScale>().ResetToBeginning();
			DGTools.ShowTexture(unitBodyTex, o as Texture2D);
		});
	}

	private void ShowUIByType(ShowType type){

		currType = type;
		if (type == ShowType.Evolve) {
			evolveRoot.SetActive(true);
			levelupRoot.SetActive(false);
			titleLabel.text = TextCenter.GetText("Evolve_Title");
		}else if(type == ShowType.LevelUp){
			evolveRoot.SetActive(false);
			levelupRoot.SetActive(true);
			titleLabel.text = TextCenter.GetText("LevelUp_Title");
		}
	}

	private void ResetLevelUpData(){
		foreach (var item in levelupItem) {
			item.SetData<UserUnit>(null);
		}
		currMatList.Clear ();
		RefreshLevelUpInfo();
	}

//	GameObject levelupParent;

	//-----------------level up effect

	//center
	
	IEnumerator SwallowUserUnit () {
		Vector3 tarPos = unitBodyTex.transform.localPosition + new Vector3 (0, unitBodyTex.height / 2, 0);
		yield return new WaitForSeconds(1f);
		int count = 0;
		for (int i = 0; i < 6; i++) {
			if(levelupItem[i].UserUnit != null){
				count++;
				StartCoroutine(SwallowOneUnit(levelupItem[i],tarPos));
				yield return new WaitForSeconds(0.35f);
			}
		}
		yield return new WaitForSeconds((count-2)*0.35f + 0.6f);
		GameObject se = NGUITools.AddChild(levelupRoot, swallowEffect);
		se.transform.localPosition = tarPos;
		yield return new WaitForSeconds(0.4f);
		Destroy(se);

		if (isLevelIncrease) {
			isLevelIncrease = false;
//			GameObject le = NGUITools.AddChild(parent, levelUpEffect);
//			le.transform.localPosition = tarPos;
//			le.layer = GameLayer.EffectLayer;
//			yield return new WaitForSeconds(1f);
//			Destroy(le);
		} 
		ShowUnitInfo ();
		RefreshLevelUpInfo();
	}

	IEnumerator SwallowOneUnit(LevelUpItem item,Vector3 tarPos){
		GameObject obj = NGUITools.AddChild(levelupRoot,item.gameObject);
		obj.transform.localPosition = item.transform.localPosition;
		iTween.ScaleTo(obj, iTween.Hash("x",0f,"y", 0f, "time", 0.3f));
		item.SetData<UserUnit>(null);
		yield return new WaitForSeconds(0.3f);
		Destroy(obj);

		GameObject lq = NGUITools.AddChild(levelupRoot, linhunqiuEffect);
		lq.transform.localPosition = obj.transform.localPosition;
//		lq.transform.localScale = Vector3.zero;
//		iTween.ScaleTo(lq, iTween.Hash("x",1f,"y", 1f, "time", 2f));
//		yield return new WaitForSeconds(2f);
		iTween.MoveTo(lq, iTween.Hash("position",tarPos , "time", 0.3f, "islocal", true));
		yield return new WaitForSeconds(0.3f);
		
		AudioManager.Instance.PlayAudio(AudioEnum.sound_devour_unit);
		Destroy(lq);
	}


	///-----------evolve
	private List<uint> partIds = new List<uint>();
	private Dictionary<uint, int> materialCount = new Dictionary<uint, int>();
	private UserUnit partyItem = null;
	void ShowEvolveInfo () {
		int i = 0;
		bool isHave = false;
		materialCount.Clear ();
		foreach (var item in baseUserUnit.UnitInfo.evolveInfo.materialUnitId) {
			if(materialCount.ContainsKey(item)){
				materialCount[item]++;
			}else{
				materialCount[item] = 1;
			}
		}

		canEvolve = true;
		partIds.Clear ();
		foreach (var id in materialCount) {
			int count = 0;
			foreach (var item in DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ()) {
				if(item.unitId == id.Key){
					count++;
					if(DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty(item) > 0){
						partyItem = item;
					}
					if(partIds.Count < id.Value)
						partIds.Add(item.uniqueId);
				}
			}
			evolveItem[i].RefreshData(id.Key, count, materialCount[id.Key]);
			canEvolve = canEvolve && (count >= materialCount[id.Key]);
			i++;
		}
		for (int j = i; j < 3; j++) {
			evolveItem[j].RefreshData(0,0,0);
		}
	}


	private void ClickEvolve(GameObject obj){

		if (baseUserUnit.level < baseUserUnit.UnitInfo.maxLevel) {
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("notmaxleveltips"));
			return;
		}
		if (!canEvolve) {
			TipsManager.Instance.ShowTipsLabel("notEnoughmaterialTips");
			return;
		}

		UnitController.Instance.Evolve (EvolveNetCallback, baseUserUnit.uniqueId, partIds);

//
//		TipsManager.Instance.ShowMsgWindow( TextCenter.GetText("DownloadResourceTipTile"),TextCenter.GetText("DownloadResourceTipContent"),TextCenter.GetText("OK"),o=>{
//			MsgCenter.Instance.AddListener(CommandEnum.ResourceDownloadComplete,o1 =>{
//				List<ProtoBuf.IExtensible> evolveInfoList = new List<ProtoBuf.IExtensible> ();
//				evolveInfoList.Add (baseItem.userUnit);
//				evolveInfoList.Add (friendInfo);
//				foreach (var item in materialItem.Values) {
//					UserUnit tuu = item.userUnit;
//					if(tuu != null) {
//						evolveInfoList.Add(tuu);
//					}
//				}
//				//				ExcuteCallback (evolveInfoList);
//			});
//			ModuleManager.Instance.ShowModule(ModuleEnum.ResourceDownloadModule);
//		});
		return;

	}

	void EvolveNetCallback(object data){
		if (data != null) {
			RspEvolve evolveData = data as RspEvolve;

			if (evolveData.header.code != (int)ErrorCode.SUCCESS) {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow (evolveData.header.code);
				ResetLevelUpData();
				return;
			}

			//			uint userId = DataCenter.Instance.UserData.UserInfo.userId;
			//			DataCenter.Instance.oldUserUnitInfo = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit (rspLevelUp.blendUniqueId);
			//			DataCenter.Instance.levelUpMaterials.Clear();
			
			//删除消耗的材料
			foreach (var item in partIds) {
				DataCenter.Instance.UnitData.UserUnitList.DelMyUnit(item);
			}
			
			//更新强化后的base卡牌数据
			DataCenter.Instance.UnitData.UserUnitList.DelMyUnit(baseUserUnit.uniqueId);
			baseUserUnit = DataCenter.Instance.UnitData.UserUnitList.AddMyUnit(evolveData.evolvedUnit);
			StartCoroutine(SwallowEvolveUnit());
		}

	}

	IEnumerator SwallowEvolveUnit () {
		Vector3 tarPos = unitBodyTex.transform.localPosition + new Vector3 (0, unitBodyTex.height / 2, 0);
		Debug.Log ("pos:------------" + tarPos);
		yield return new WaitForSeconds(1f);
		for (int i = 0; i < 3; i++) {
			if(evolveItem[i].unitId != 0){
				GameObject obj = NGUITools.AddChild(evolveRoot,evolveItem[i].gameObject);
				obj.transform.localPosition = evolveItem[i].transform.localPosition;
				iTween.ScaleTo(obj, iTween.Hash("x",0f,"y", 0f, "time", 0.2f));
				evolveItem[i].RefreshData(0,0,0);
				yield return new WaitForSeconds(0.2f);
				
				
				GameObject lq = NGUITools.AddChild(evolveRoot, linhunqiuEffect);
				lq.transform.localPosition = obj.transform.localPosition;
				lq.transform.localScale = Vector3.zero;
				Destroy(obj);
				iTween.ScaleTo(lq, iTween.Hash("x",1f,"y", 1f, "time", 0.2f));
				yield return new WaitForSeconds(0.2f);
				iTween.MoveTo(lq, iTween.Hash("position",tarPos , "time", 0.3f, "islocal", true));
				yield return new WaitForSeconds(0.3f);
				
				AudioManager.Instance.PlayAudio(AudioEnum.sound_devour_unit);
				
				Destroy(lq);
				GameObject se = NGUITools.AddChild(evolveRoot, swallowEffect);
				se.transform.localPosition = tarPos;
				yield return new WaitForSeconds(0.4f);
				Destroy(se);
			}
		}

		GameObject le = NGUITools.AddChild(evolveRoot, evolveEffect);
		le.transform.localPosition = tarPos;
		le.layer = GameLayer.EffectLayer;
		yield return new WaitForSeconds(2f);
		Destroy(le);

		ShowAvatar (baseUserUnit);
		ShowUnitInfo ();
		RefreshLevelUpInfo();
	}
}
