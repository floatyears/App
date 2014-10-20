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


	private ShowType currType = ShowType.None;

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

		//center
		friendEffect = FindChild<UISprite>("Center/AE");
		friendSprite = FindChild<UISprite>("Center/AE/Avatar");
		friendEffect.gameObject.SetActive (false);
		
		initPos = FindChild<Transform> ("Center/InitPosition").localPosition;;
		endPos = FindChild<Transform> ("Center/EndPosition").localPosition;

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
				ResetData();
				RefreshInfo();
			}else if(viewData.ContainsKey("evolve")){
				baseUserUnit = viewData["level_up"] as UserUnit;
				ResetData();
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

		RefreshInfo ();
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
	void RefreshInfo(){
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
		RefreshInfo ();
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
				ResetData();
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

	private void ResetData(){
		foreach (var item in selectedItem) {
			item.SetData<UserUnit>(null);
		}
		currMatList.Clear ();
	}


	//-----------------level up effect
	void PlayLevelUp() {
		Umeng.GA.Event ("PowerUp");
		
		Transform effectTrans = friendEffect.transform;
		effectTrans.localPosition = initPos;
		Vector3 downEndPos = endPos + (-100f * Vector3.up);
		AudioManager.Instance.PlayAudio (AudioEnum.sound_friend_up);

		GameObject.Instantiate (selectedItem [4].gameObject);

		iTween.MoveTo(friendEffect.gameObject,iTween.Hash("position",endPos,"time",0.35f,"delay",1.5f,"easetype",iTween.EaseType.easeInQuart,"islocal",true));
		iTween.RotateFrom (friendEffect.gameObject, iTween.Hash ("z", 10, "time", 0.15f,"delay",1.5f, "easetype", iTween.EaseType.easeOutBack));
		iTween.MoveTo(friendEffect.gameObject,iTween.Hash("position",downEndPos,"time", 0.15f,"delay",1.65f,"easetype",iTween.EaseType.easeOutQuart,"islocal",true,"oncomplete","ShowLevelup","oncompletetarget",gameObject));
	}
	
	void ShowLevelup() {
		
		for (int i = 0; i < dataCenter.levelUpMaterials.Count; i++) {
			UnitInfo tui = dataCenter.levelUpMaterials[i].UnitInfo;
			GameObject go = NGUITools.AddChild(parent, materilItem);
			go.SetActive(true);

			UISprite sprite = go.transform.Find("Avatar").GetComponent<UISprite>();
			ResourceManager.Instance.GetAvatarAtlas(tui.id, sprite);
			go.transform.Find("Background").GetComponent<UISprite>().spriteName = DGTools.GetItemBackgroundName(tui.type);
			go.transform.Find("Sprite_Avatar_Border").GetComponent<UISprite>().spriteName = DGTools.GetItemBorderName(tui.type);
			material.Enqueue(go);
		}

		parent.GetComponent<UIGrid> ().Reposition ();
		count = material.Count;
		ResourceManager.Instance.GetAvatar (UnitAssetType.Profile,newBlendUnit.unitId, o => {
			DGTools.ShowTexture (unitBodyTex, o as Texture2D);
			ShowTexture = true;
			
			Vector3 localposition = unitBodyTex.transform.localPosition; 
			Vector3 tPos = new Vector3(localposition.x, localposition.y + unitBodyTex.height * 0.5f - 480f, localposition.z);
			targetPosition = tPos - parent.transform.localPosition; 
			ShowUnitScale();
			SetEffectCamera();
			StartCoroutine(SwallowUserUnit());
		});
	}

	//center
	Vector3 targetPosition;
	
	Queue<GameObject> material = new Queue<GameObject> ();
	int count = 0;
	List<GameObject> effectCache = new List<GameObject>();
	GameObject levelUpEffect = null;
	GameObject swallowEffect = null;
	GameObject linhunqiuEffect = null;
	GameObject evolveEffect = null;
	
	
	GameObject materilUse = null;
	GameObject linhunqiuIns = null;
	GameObject swallowEffectIns = null;
	GameObject evolveEffectIns = null;
	GameObject parent;
	
	IEnumerator SwallowUserUnit () {
		yield return new WaitForSeconds(1f);
		
		while (material.Count > 0) {
			materilUse = material.Dequeue();
			iTween.ScaleTo(materilUse, iTween.Hash("y", 0f, "time", 0.2f));
			yield return new WaitForSeconds(0.2f);
			Destroy(materilUse);
			linhunqiuIns = NGUITools.AddChild(parent, linhunqiuEffect);
			linhunqiuIns.transform.localPosition = materilUse.transform.localPosition;
			linhunqiuIns.transform.localScale = Vector3.zero;
			iTween.ScaleTo(linhunqiuIns, iTween.Hash("y", 1f, "time", 0.2f));
			yield return new WaitForSeconds(0.2f);
			iTween.MoveTo(linhunqiuIns, iTween.Hash("position", targetPosition, "time", 0.3f, "islocal", true));
			yield return new WaitForSeconds(0.3f);
			
			AudioManager.Instance.PlayAudio(AudioEnum.sound_devour_unit);
			
			Destroy(linhunqiuIns);
			swallowEffectIns = NGUITools.AddChild(gameObject, swallowEffect);
			yield return new WaitForSeconds(0.4f);
			Destroy(swallowEffectIns);
		}
		
		_curLevel = oldBlendUnit.level;
		levelLabel.text = _curLevel + " / " + oldBlendUnit.UnitInfo.maxLevel;
		gotExp = levelUpData.blendExp;
		
		levelDone = gotExp > 0;
		
		curExp = oldBlendUnit.CurExp;
		Calculate ();
	}
	
	void ClearEffectCache(){
		while (material.Count > 0) {
			Destroy(material.Dequeue());
		}
		
		//		DGTools.SafeDestory (materilUse);
		//		DGTools.SafeDestory (linhunqiuIns);
		//		DGTools.SafeDestory (swallowEffectIns);
		//		DGTools.SafeDestory (evolveEffectIns);
		
		for (int i = effectCache.Count - 1; i >= 0 ; i--) {
			GameObject go = effectCache[i];
			Destroy( go );
			effectCache.Remove(go);
		}
	}

	void PlayLevelupEffect () {
		AudioManager.Instance.PlayAudio(AudioEnum.sound_level_up);
		GameObject go = Instantiate (levelUpEffect) as GameObject;
		effectCache.Add (go);
	}

	void Update(){
		ExpRise();
	} 
	
	void LevelUpEnd() {
		gotExp = 0;
		
		RecoverEffectCamera ();
		AudioManager.Instance.StopAudio (AudioEnum.sound_get_exp);
		
		if (oldBlendUnit != null) {
			if(_curLevel >= oldBlendUnit.UnitInfo.maxLevel) {
				UnitController.Instance.UserguideEvoUnit(o=>{
					RspUserGuideEvolveUnit rsp = o as RspUserGuideEvolveUnit;
					if (rsp.header.code == ErrorCode.SUCCESS) {
						if (rsp != null ) {
							DataCenter.Instance.UnitData.UserUnitList.AddMyUnitList(rsp.addUnit);
						}
					}else {
						Debug.LogError("UserGuideEvolveUnit ret err:"+rsp.header.code);
					}
				},oldBlendUnit.unitId);
			}
			
			oldBlendUnit = null;	
		}
	}
	
	void LevelupExpRiseEnd() {
		isNoviceGUide = false;
		AudioManager.Instance.StopAudio (AudioEnum.sound_get_exp);
	}
	
	void ExpRise () {
		if (gotExp <= 0) {
			if(levelDone) {
				MsgCenter.Instance.Invoke(CommandEnum.levelDone);
				levelDone = false;
				GameTimer.GetInstance().AddCountDown(1f, LevelupExpRiseEnd);
			}
			return;	
		}	
		
		if(gotExp < expRiseStep){
			curExp += gotExp;
			gotExp = 0;
			
			if(AudioManager.Instance.GetPlayAuioInfo() != AudioEnum.sound_get_exp)
				AudioManager.Instance.PlayAudio(AudioEnum.sound_get_exp);
		} else {
			gotExp -= expRiseStep;
			curExp += expRiseStep;
			
			if(AudioManager.Instance.GetPlayAuioInfo() != AudioEnum.sound_get_exp)
				AudioManager.Instance.PlayAudio(AudioEnum.sound_get_exp);
		}
		
		if(curExp >= currMaxExp) {
			gotExp += curExp - currMaxExp;
			curExp = 0;
			if ( _curLevel < oldBlendUnit.UnitInfo.maxLevel ){
				_curLevel ++;
				PlayLevelupEffect();
			} else { // reach MaxLevel
				//TODO: show MAX on the progress bar
				curExp = currMaxExp;
				gotExp = 0;
			}
			
			//			LogHelper.LogError("=======gotExp:{0} curExp:{1} curLevel:{2} ",gotExp, curExp, curLevel);
			
			Calculate();
		}
		
		int needExp = currMaxExp - curExp;
		
		if ((_curLevel > oldBlendUnit.UnitInfo.maxLevel) 
		    || (_curLevel == oldBlendUnit.UnitInfo.maxLevel && needExp <= 0) ) {
			levelLabel.text = oldBlendUnit.UnitInfo.maxLevel.ToString() + "/" + oldBlendUnit.UnitInfo.maxLevel.ToString();
			needExpLabel.text = "Max";
			expSlider.value = 1.0f;
			return;
		} else {
			needExpLabel.text = TextCenter.GetText("Text_Next") +": " + needExp.ToString();
		}
		
		float progress = (float)curExp / (float)currMaxExp;
		if (progress == 0) {
			progress = 0.1f;
		}
		expSlider.value = progress;
	}

	
	void Calculate () {
		if( oldBlendUnit == null ) {
			Debug.LogError("Calculate() :: oldBlendUnit=null");
			return;
		}
		
		levelLabel.text = _curLevel.ToString () + " / " + oldBlendUnit.UnitInfo.maxLevel;
		
		currMaxExp = oldBlendUnit.UnitInfo.GetLevelExp(_curLevel); 
		
		expRiseStep = (int)(currMaxExp * 0.01f);
		if ( expRiseStep < 1 )
			expRiseStep = 1;
	}

	Vector3 initPos;
	Vector3 endPos;
	
	private const float yCoor = -142;
	

	
	UserUnit oldBlendUnit = null;
	UserUnit newBlendUnit = null;
	
	private UserUnit curUserUnit;
	
	bool levelDone = false;
	
	public void SetEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (15f, 0f, 0f);
		camera.orthographicSize = 1.3f;
	}
	
	public void RecoverEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		camera.orthographicSize = 1f;
	}
	
}
