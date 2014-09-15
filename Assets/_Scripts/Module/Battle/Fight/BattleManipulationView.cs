using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManipulationView : ViewBase {
//	private static UIRoot uiRoot;
	private static Camera mainCamera;
	private UICamera nguiMainCamera;
	private RaycastHit[] rayCastHit;
	private CardItem tempCard;
	private GameObject tempObject;
	
	private GameObject battleCardPool;
	private GameObject battleCard;
	private GameObject battleCardArea;

//	private BattleEnemy battleEnemy;
	private GameObject countDownUI;

	private float ZOffset = -100f;
	private List<ItemData> allItemData = new List<ItemData>();
	private List<CardItem> selectTarget = new List<CardItem>();
	//temp-----------------------------------------------------------
	public static int colorIndex = 0;
	public static bool isShow = false;
	private List<int> currentColor = new List<int>();
	//end------------------------------------------------------------
	public int cardHeight = 0;
	private Vector3 localPosition = new Vector3 (-0.18f, -17f, 0f);

	////------------battle card pool start
	private UISprite templateBackTexture;
	
	private Vector3[] cardPosition;
	
	private UISprite[] backTextureIns;
	private int cardInterv = 0;
	private Vector3 initPosition = Vector3.zero;
	private float xStart = 0f;

	private float XRange {
		set { xStart = transform.localPosition.x - value / 2f; }
	}
	////------------battle card pool end

	////------------battle card area start
	private UISprite backTexture;
	private BattleCardAreaItem[] battleCardAreaItem;
	private UILabel stateLabel;
	private Vector3 sourcePosition;
	private const int oneWordSize = 42;

	private Dictionary<int,List<CardItem>> battleAttack = new Dictionary<int, List<CardItem>>();
	private static List<GameObject> battleCardIns = new List<GameObject>();
	private GameObject cardItem;
	public static Vector3 startPosition;
	public static Vector3 middlePosition;
	public static Vector3 endPosition;
	public static Vector3 activeSkillStartPosition;
	public static Vector3 activeSkillEndPosition;
	////------------battle card area end

	////------------battle card start
	public event Callback CallBack;
	
//	private Vector3[] cardPosition;
	
	private UISprite templateItemCard;
	
	private List<CardItem> moveItem = new List<CardItem>();
	
//	private float cardInterv = 0f;
	
	private List<TNormalSkill> normalSkill ;
	
//	private BattleUseData battleUseData;
	
	private GameObject fingerObject;

	private CardItem[] cardItemArray;
	/// ------------battle card end

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
//		uiRoot = ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>();
		nguiMainCamera = ViewManager.Instance.MainUICamera;
		mainCamera = nguiMainCamera.camera;

		GameInput.OnPressEvent += HandleOnPressEvent;
		GameInput.OnReleaseEvent += HandleOnReleaseEvent;
		GameInput.OnStationaryEvent += HandleOnStationaryEvent;
		GameInput.OnDragEvent += HandleOnDragEvent;


		////------------battle card pool start
		battleCardPool = FindChild("BattleCardPool");

		int count = Config.cardPoolSingle;
		cardPosition = new Vector3[count];
		backTextureIns = new UISprite[count];		
		templateBackTexture = FindChild<UISprite>("BattleCardPool/Back");
		cardInterv = templateBackTexture.width + Config.cardInterv;
		initPosition = Config.cardPoolInitPosition;

		for (int i = 0; i < cardPosition.Length; i++) {
			tempObject = NGUITools.AddChild(battleCardPool, templateBackTexture.gameObject);
			cardPosition[i] = new Vector3(initPosition.x + i * cardInterv,initPosition.y,initPosition.z);
			tempObject.transform.localPosition = cardPosition[i];
			backTextureIns[i] = tempObject.GetComponent<UISprite>();
		}
		templateBackTexture.gameObject.SetActive(false);

		NGUITools.AddWidgetCollider(battleCardPool);
		BoxCollider bc = battleCardPool.GetComponent<BoxCollider> ();
		XRange = bc.size.x;
		cardHeight = templateBackTexture.width;

		////------------battle card pool end

		////------------battle card area start
		battleCardArea = FindChild("BattleCardArea");

		backTexture = FindChild<UISprite>("BattleCardArea/Back"); 
		backTexture.gameObject.SetActive(false);
		stateLabel = FindChild<UILabel>("BattleCardArea/StateLabel");
		stateLabel.text = string.Empty;
		if (cardItem == null) {
//			LoadAsset.Instance.LoadAssetFromResources (Config.battleCardName, ResourceEuum.Prefab,o=>{
//				GameObject go = o as GameObject;
//				cardItem = go.transform.Find("Texture").gameObject;
//			});
		}
		CreatArea();

		////------------battle card area end

		////------------battle card start
		battleCard = FindChild("BattleCard");
		templateItemCard = FindChild<UISprite>("BattleCard/Texture");
		fingerObject = FindChild<UISprite> ("BattleCard/finger").gameObject;
		
		int cardcount = cardPosition.Length;
		
		cardItemArray = new CardItem[cardcount];
		for (int i = 0; i < cardcount; i++) {
			tempObject = NGUITools.AddChild(battleCard,templateItemCard.gameObject);
			tempObject.transform.localPosition = cardPosition[i];
			CardItem ci = tempObject.AddComponent<CardItem>();
			ci.location = i;
			ci.Init(i.ToString());
			cardItemArray[i] = ci;
		}
		
		templateItemCard.gameObject.SetActive(false);
		cardInterv = (int)Mathf.Abs(cardPosition[1].x - cardPosition[0].x);
//		battleCard.battleCardArea = battleCardArea;
		////------------battle card end


//		CreatEnemy ();

	}

	public override void ShowUI() {
		base.ShowUI();
		//		Debug.LogError ("NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION :" + (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION));
		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION) {
			AddGuideCard ();
			ConfigBattleUseData.Instance.storeBattleData.colorIndex = 0;
		}
		//		Debug.LogError ("isShow");
		if (!isShow) {
			isShow = true;
			GenerateShowCard();
		}
		
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttckEnd);
		MsgCenter.Instance.AddListener (CommandEnum.ChangeCardColor, ChangeCard);
//		MsgCenter.Instance.AddListener (CommandEnum.DelayTime, DelayTime);
		MsgCenter.Instance.AddListener (CommandEnum.ExcuteActiveSkill, ExcuteActiveSkillInfo);
//		MsgCenter.Instance.AddListener (CommandEnum.UserGuideAnim, UserGuideAnim);
		MsgCenter.Instance.AddListener (CommandEnum.UserGuideCard, UserGuideCard);
		
		//		UserGuideAnim (null);
		ModuleManager.Instance.ShowModule (ModuleEnum.BattleEnemyModule);
	}
	
	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttckEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.ChangeCardColor, ChangeCard);
//		MsgCenter.Instance.RemoveListener (CommandEnum.DelayTime, DelayTime);
		MsgCenter.Instance.RemoveListener (CommandEnum.ExcuteActiveSkill, ExcuteActiveSkillInfo);
		MsgCenter.Instance.RemoveListener (CommandEnum.UserGuideAnim, UserGuideAnim);
		MsgCenter.Instance.RemoveListener (CommandEnum.UserGuideCard, UserGuideCard);
	}
	
	public override void DestoryUI () {
		GameInput.OnPressEvent -= HandleOnPressEvent;
		GameInput.OnReleaseEvent -= HandleOnReleaseEvent;
		GameInput.OnStationaryEvent -= HandleOnStationaryEvent;
		GameInput.OnDragEvent -= HandleOnDragEvent;
		base.DestoryUI ();

	}
	
	public void StartBattle () {
		ResetClick();
		Attack();
		StartBattle (false);
	}
	
	void ExcuteActiveSkillInfo(object data) {
		bool b = (bool)data;
		ShieldInput (!b);
	}
	
	void ChangeCard(object data) {
		ChangeCardColor ccc = data as ChangeCardColor;
		if (ccc == null) {
			return;	
		}
		
		if (ccc.targetType == -1) {
			GenerateShowCard();
		} 
		else {
			for (int i = 0; i < cardPosition.Length; i++) {
				ChangeSpriteCard(ccc.sourceType,ccc.targetType,i);
			}
			RefreshLine();
		}
	}
	
	void EnemyAttckEnd (object data) {
		StartBattle (true);
		ShieldInput (true);
	}
	
	void EnemyAttackEnd() {
		
	}
	
	void BattleEnd (object data) {
		ExitFight ();
	}
	
	public void ExitFight() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_battle_over);
		
		isShowEnemy = false;
		ShieldInput (true);
		HideUI ();
	}
	
	void Attack () {
		MsgCenter.Instance.Invoke (CommandEnum.StartAttack, null);
	}
	
	void GenerateShowCard () {
		//		Debug.LogError ("GenerateShowCard");
		for (int i = 0; i < cardPosition.Length; i++) {
			int cardIndex = GenerateCardIndex();
			GenerateSpriteCard(cardIndex, i);
		}
		RefreshLine ();
	}

	public bool IsBoss = false;
	public bool isShowEnemy = false;
	public void ShowEnemy(List<TEnemyInfo> count, bool isBoss = false) {
		isShowEnemy = true;
		IsBoss = isBoss;
		//		Debug.LogError ("IsBoss : " + IsBoss);
//		battleEnemy.Refresh(count);
		ModuleManager.SendMessage (ModuleEnum.BattleEnemyModule, "refresh", count);
		//		MsgCenter.Instance.Invoke (CommandEnum.ReduceActiveSkillRound);
		TStoreBattleData tsbd =  ConfigBattleUseData.Instance.storeBattleData;
		tsbd.tEnemyInfo = count;
		 ConfigBattleUseData.Instance.storeBattleData.attackRound ++;
		 ConfigBattleUseData.Instance.StoreMapData ();
		MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [0]);
		
		//		Debug.Log ("battle guide----------");
		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.BOSS_ATTACK_ONE) {
			if(IsBoss){
				Debug.Log("is boss -------------");
				NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.FIGHT);
			}
			
		} else {
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.FIGHT);
		}
		
	}
	
	int GenerateData() {
		ItemData id = Config.Instance.GetCard();
		allItemData.Add(id);
		return id.itemID;
	}
	
	int GenerateCardIndex () {
		int index =  ConfigBattleUseData.Instance.questDungeonData.Colors [ ConfigBattleUseData.Instance.storeBattleData.colorIndex];
		 ConfigBattleUseData.Instance.storeBattleData.colorIndex++;
		
		if (userGuideIndex != -1) {
			index = userGuideIndex;
		}
		
		currentColor.Add (index);
		if (currentColor.Count > 5) {
			currentColor.RemoveAt(0);
		}
		return index;
	}
	
	public void SwitchInput(bool isShield) {
		nguiMainCamera.useMouse = isShield;
		nguiMainCamera.useKeyboard = isShield;
		nguiMainCamera.useTouch = isShield;
//		Main.Instance.GInput.IsCheckInput = !isShield;
	}
	
	public void ShieldGameInput(bool isShield) {
//		Main.Instance.GInput.IsCheckInput = isShield;
	}
	
	public void ShieldNGUIInput(bool isShield) {
		nguiMainCamera.useMouse = isShield;
		nguiMainCamera.useKeyboard = isShield;
		nguiMainCamera.useTouch = isShield;
	}
	
	public void ShieldInput (bool isShield) {
		//		Debug.LogError ("ShieldInput : " + isShield);
		//		if (shileInputByNoviceGuide) {
		//			return;	
		//		}
		nguiMainCamera.enabled = isShield;
//		Main.Instance.GInput.IsCheckInput = isShield;
	}
	
	void HandleOnPressEvent () {
		if(Check(GameLayer.ActorCard)) {
			for (int i = 0; i < rayCastHit.Length; i++) {
				if(rayCastHit[i].collider.gameObject.layer == GameLayer.ActorCard) {
					tempObject= rayCastHit[i].collider.gameObject;
					break;
				}
				else
					continue;
			}
			ClickObject(tempObject);
			SetDrag();
		}
	}
	
	void HandleOnReleaseEvent () {
		if (!isShowEnemy) {
			return;
		}
		
		if(selectTarget.Count == 0) {
			ResetClick();
			return;
		}
		
		if(Check(GameLayer.BattleCard)) {
			GenerateCard();
		}
		else if(Check(GameLayer.ActorCard)) {
			SwitchCard();
		}
		else {
			ResetClick();
		}
	}
	
	void HandleOnStationaryEvent () {
		
	}
	
	void HandleOnDragEvent (Vector2 obj) {
		SetDrag();
		Vector3 vec = ChangeCameraPosition(obj) - ViewManager.Instance.ParentPanel.transform.localPosition;
		
		for (int i = 0; i < selectTarget.Count; i++) {
			selectTarget [i].OnDragHandler (vec, i);
		}
		
		bool b = Check(GameLayer.ActorCard);
		//		bool b = Check(GameLayer.Default);
		
		if(b) {
			for (int i = 0; i < rayCastHit.Length; i++) {
				tempObject = rayCastHit[i].collider.gameObject;
				
				ClickObject(tempObject);
			}
		}
	}
	
	void ResetClick() {
		for (int i = 0; i < selectTarget.Count; i++) {
			//			Debug.LogError ("ResetClick selectTarget.Count : " + selectTarget.Count + " selectTarget : " + selectTarget[i].name);
			selectTarget[i].OnPressHandler(false, -1);
		}
		selectTarget.Clear();
		ResetDrag();
	}

	void GenerateCard() {
		BattleCardAreaItem bcai = null;
		for (int i = 0; i < rayCastHit.Length; i++) {
			tempObject = rayCastHit[i].collider.gameObject;
			bcai = tempObject.GetComponent< BattleCardAreaItem >();
			if(bcai != null)
				break;
		}
		
		int generateCount = 0;
		if(bcai != null)
			generateCount = bcai.GenerateCard(selectTarget);
		
		if(generateCount > 0) {
			MsgCenter.Instance.Invoke(CommandEnum.StateInfo,"");
//			YieldStartBattle();
//			
//			if(showCountDown) {
//				for(int i = 0;i < generateCount;i++) {
//					GenerateSpriteCard(GenerateCardIndex(),selectTarget[i].location);
//				}
//				RefreshLine();
//			}
		}
		ResetClick();
	}
	
	void SwitchCard() {
		Vector3 point = selectTarget[0].transform.localPosition;
		int indexID = CaculateSortIndex(point);
		if(indexID >= 0) {
//			Main.Instance.GInput.IsCheckInput = false;
			if(SortCard(indexID, selectTarget)) {
				CallBack += HandleCallBack;
			}
			else {
//				Main.Instance.GInput.IsCheckInput = true; 
				ResetClick();
			}
		}
		else {
			ResetClick();
		}
	}
	
	void HandleCallBack () {
		CallBack -= HandleCallBack;
//		Main.Instance.GInput.IsCheckInput = true;
		ResetClick();
	}
	
	void TweenCallback(GameObject go) {
//		Main.Instance.GInput.IsCheckInput = true;
	}
	
	private BattleCardAreaItem prevTempBCA;
	
	void SetDrag() {
		if(selectTarget.Count > 0)
			DisposeDrag(selectTarget[0].location,selectTarget[0].itemID);
	}
	
	void ClickObject(GameObject go) {
		tempCard = go.GetComponent<CardItem>();
		ClickCardItem (tempCard);
	}
	
	void ClickCardItem(CardItem ci) {
		if(ci != null) {
			if(selectTarget.Contains(ci))
				return;
			if(ci.CanDrag) {
				ci.OnPressHandler(true, selectTarget.Count);
				ci.ActorTexture.depth = ci.InitDepth;
				selectTarget.Add(ci);
				
				if(selectTarget.Count == 1) { //one select not effect.
					AudioManager.Instance.PlayAudio(AudioEnum.sound_drag_tile);
					return;
				}
				AudioManager.Instance.PlayAudio(AudioEnum.sound_title_overlap);
				EffectManager.Instance.GetOtherEffect(EffectManager.EffectEnum.DragCard, returnValue => {
					GameObject prefab = returnValue as GameObject;
					GameObject effectIns = EffectManager.InstantiateEffect(ViewManager.Instance.EffectPanel, prefab);
					Transform card = ci.transform;
					effectIns.transform.localPosition = card.localPosition + card.parent.parent.localPosition;}
				);
			}
		}
	}
	
	bool Check(LayerMask mask) {
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		
		rayCastHit = Physics.RaycastAll(ray,100f, GameLayer.LayerToInt(mask));
		
		if(rayCastHit.Length > 0)
			return true;
		else
			return false;
	}
	
	public static Vector3 ChangeCameraPosition() {
		Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		
		float height = (float)Screen.height / 2;
		
		Vector3 reallyPoint = worldPoint * height * ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>().pixelSizeAdjustment;
		
		return reallyPoint;
	}
	
	public static Vector3 ChangeCameraPosition(Vector3 pos) {
		Vector3 worldPoint = mainCamera.ScreenToWorldPoint(pos);
		
		float height = (float)Screen.height / 2;
		
		Vector3 reallyPoint = worldPoint * height * ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>().pixelSizeAdjustment;
		
		return reallyPoint;
	}
	
	public static Vector3 ChangeDeltaPosition(Vector3 delta) {
		return delta * ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>().pixelSizeAdjustment;
	}
	
//	void DelayTime(object data) {
//		activeDelay =  (float)data;
//	}
	
//	public bool showCountDown = false;
//	float time = 5f;
//	float countDownTime = 1f;
//	float activeDelay = 0f;
//	public void YieldStartBattle () {
//		if (showCountDown) {
//			return ;		
//		} 
//		ShieldNGUIInput (false);
//		time = BattleUseData.CountDown + activeDelay;
////		countDownUI.ShowUI();
//		ModuleManager.Instance.ShowModule (ModuleEnum.BattleEnemyModule, "coutndown");
//		activeDelay = 0f;
//		CountDownBattle ();
//	}
//	
//	void CountDownBattle () {
//		int temp = (int)time;
//		countDownUI.SetCurrentTime (temp);
//		if (time > 0) {
//			BattleBottom.notClick = true;
//			showCountDown = true;
//			time -= countDownTime;
//			GameTimer.GetInstance ().AddCountDown (countDownTime, CountDownBattle);
//		} else {
//			ResetClick();
//			ShieldInput (false);
//			//			MsgCenter.Instance.Invoke(CommandEnum.ReduceActiveSkillRound);
//			showCountDown = false;
//			battleCardArea.ShowCountDown (false, (int)time);
//			ShieldNGUIInput (true);
//			StartBattle();
//			time =  BattleUseData.CountDown;
//		}
//	}

	////------------battle card pool start
	public void IgnoreCollider(bool isIgnore)
	{
		if(isIgnore)
			gameObject.layer = GameLayer.IgnoreCard;
		else
			gameObject.layer = GameLayer.ActorCard;
	}
	
	public int CaculateSortIndex(Vector3 point)
	{
		float x = point.x - xStart;
		
		for (int i = 0; i < cardPosition.Length; i++) 
		{
			if(x > cardInterv * i && x <= cardInterv * (i + 1))
				return i;
		}
		
		return -1;
	}
	////------------battle card pool end

	/// ------------battle card area start
	Vector3 HidePosition;
	Vector3 showPosition;
	
	string prevInfo = "";
	
	int boostIndex = -1;
	
	int maxBoostRandom = 0;
	
	void SetBoost () {
		maxBoostRandom = NoviceGuideStepEntityManager.isInNoviceGuide() ? 5 : 10;
		
		if (boostIndex > -1 && boostIndex < 5) {
			battleCardAreaItem[boostIndex].isBoost = false;
		}
		
		boostIndex = Random.Range (0, maxBoostRandom);
		if(boostIndex < 5) 
			battleCardAreaItem[boostIndex].isBoost = true;
	}
	
	void StateInfo(object data) {
		string info = (string)data;
		if (string.IsNullOrEmpty (info) && !string.IsNullOrEmpty(stateLabel.text)) {
			HideStateLabel(string.Empty);
			return;
		}			
		
		if (info == DGTools.stateInfo [0]) {
			SetBoost();
		}
		
		if (stateLabel.text == info) {
			return;	
		}
		
		if (info == DGTools.stateInfo [4]) {
			prevInfo = stateLabel.text;
		}
		
//		Color32[] colors;
		
//		if (info == DGTools.stateInfo [0] || info == DGTools.stateInfo [1]) {
//			colors = QuestFullScreenTips.thirdGroupColor;		
//		} else {
//			colors = QuestFullScreenTips.secondGroupColor;
//		}
		
//		QuestFullScreenTips.SetLabelGradient (stateLabel, colors);
		ModuleManager.SendMessage (ModuleEnum.BattleFullScreenTipsModule, "label_gradient", stateLabel);
		
		if (stateLabel.text == string.Empty) {
			stateLabel.transform.localPosition = HidePosition;	
			ShowStateLabel ();
		} else {
			HideStateLabel("ShowStateLabel");
		}
		
		stateLabel.text = info;
	}
	
	void RecoverStateInfo(object data) {
		bool b = (bool)data;
		if (!b) {
			StateInfo (prevInfo);
			prevInfo = "";
		}
	}
	
	void HideStateLabel (string nextFunction) {
		float x = showPosition.x - stateLabel.width;
		iTween.MoveTo(stateLabel.gameObject, iTween.Hash("x", x, "islocal", true,"time",0.15f,"easetype",iTween.EaseType.easeOutCubic,"oncompletetarget",gameObject,"oncomplete","ClearTexture"));
	}
	
	void ClearTexture () {
		stateLabel.text = string.Empty;
	}
	
	void ShowStateLabel () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_text_appear);
		
		iTween.MoveTo (stateLabel.gameObject, iTween.Hash ("position", showPosition, "islocal", true, "time", 0.15f));
	}	       
	
	void CreatArea() {
		battleCardAreaItem = new BattleCardAreaItem[cardPosition.Length];
		float xOffset = backTexture.width * -0.5f;
		float yOffset = backTexture.height * 1.7f;
		stateLabel.transform.localPosition = cardPosition [0] + new Vector3 (xOffset, yOffset, 0f);
		showPosition = stateLabel.transform.localPosition;
		HidePosition = stateLabel.transform.localPosition + Vector3.right * -(stateLabel.mainTexture.width + Screen.width * 0.5f);
		stateLabel.enabled = true;
		int length = cardPosition.Length;
		for (int i = 0; i < length; i++) {
			tempObject = NGUITools.AddChild (battleCardArea, backTexture.gameObject);
			tempObject.SetActive (true);
//			tempObject.layer = GameLayer.BattleCard;
			tempObject.transform.localPosition = new Vector3 (cardPosition [i].x + 5f, cardPosition [i].y + 3f + cardHeight, cardPosition [i].z);
			BattleCardAreaItem bca = tempObject.AddComponent<BattleCardAreaItem> ();
			bca.Init (tempObject.name);
			bca.AreaItemID = i;
			battleCardAreaItem [i] = bca;
		}
		
		BattleCardAreaItem bcai = battleCardAreaItem [length - 1];
		//normal skill is from right top to left bottom.
		Vector3 pos = bcai.transform.localPosition;		// get last area item position.
		
		startPosition = new Vector3 (pos.x + cardHeight, pos.y - cardHeight * 0.5f, pos.z); //normal skill start position.
		
		middlePosition = battleCardAreaItem [2].transform.localPosition;
		
		pos = battleCardAreaItem [0].transform.localPosition;	// get first area item position.
		
		endPosition = new Vector3 (pos.x - cardHeight * 0.5f, pos.y - cardHeight * 1.5f, pos.z);	//normal skill end position.
		
		//active skill is from left top to right top. normal skill start position is active skill end position.
		activeSkillStartPosition = new Vector3 (pos.x - cardHeight * 0.5f - 640f, pos.y - cardHeight * 0.5f, pos.z);	//active skill from position.
		
		Vector3 actveiPosition = battleCardAreaItem[2].transform.localPosition;
		
		activeSkillEndPosition = new Vector3 (actveiPosition.x , startPosition.y, startPosition.z);
	}
	
	static int count = 0;
	public static GameObject GetCard() {
		if (count == battleCardIns.Count) {
			count = 0;
		}
		GameObject go = battleCardIns [count];
		count ++;
		return go;
	}
//
//	public void ShowCountDown (bool isShow,int time) {
//		showCountDown = isShow;
//		this.time = time;
//	}
	////------------battle card area end

	////------------battle card start
	
	/// <summary>
	/// new
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="locationID">Location I.</param>
	public void ChangeSpriteCard(int source, int index, int locationID) {
		CardItem ci = cardItemArray [locationID];
		if (ci.itemID == source) {
			ci.SetSprite (index,CheckGenerationAttack (index));
		}
	}
	
	/// <summary>
	/// new 
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="locationID">Location I.</param>
	void GenerateSpriteCard(int index,int locationID) {
		CardItem ci = cardItemArray [locationID];
		ci.SetSprite (index, CheckGenerationAttack (index));
	}
	
	void RefreshLine() {
		foreach (var item in cardItemArray) {
			GenerateLinkSprite (item, item.itemID);
		}
	}
	
	void GenerateLinkSprite(CardItem ci,int index) {
		List<Transform> trans = new List<Transform> ();
		for (int i = 0; i < battleCardAreaItem.Length; i++) {
			if(battleCardAreaItem[i] == null) 
				continue;
			if(BattleUseData.Instance.upi.CalculateNeedCard( battleCardAreaItem[i].AreaItemID, index )) {
				trans.Add(battleCardAreaItem[i].transform);
			}
		}
		ci.SetTargetLine (trans);
	}
	
	/// <summary>
	/// t
	/// </summary>
	/// <param name="b">If set to <c>true</c> b.</param>
	public void StartBattle(bool b) {
		if (!b) {
			foreach (var item in cardItemArray) {
				item.Clear();
			}
		} else {
			RefreshLine();
		}
	}
	
	bool CheckGenerationAttack (int index) {
		if (index == 7) {
			return true;
		}
		if(normalSkill == null)
			normalSkill = BattleUseData.Instance.upi.GetNormalSkill ();
		foreach (var item in normalSkill) {
			if(item.Blocks.Contains((uint)index)) {
				return true;
			}
		}
		
		return false;
	}
	
	public void IgnoreAreaCollider(bool isIgnore) {
		LayerMask layer;
		
		if(isIgnore)
			layer = GameLayer.ActorCard;
		else
			layer = GameLayer.IgnoreCard;
		
		for (int i = 0; i < cardItemArray.Length; i++) {
			cardItemArray[i].gameObject.layer = layer;
		}
	}
	
	public void DisposeDrag(int location,int itemID) {
		SetFront(location,itemID);
		SetBehind(location,itemID);
	}
	
	public void ResetDrag() {
		for (int i = 0; i < cardItemArray.Length; i++) {
			if(cardItemArray[i] == null) {
				continue;
			}
			cardItemArray[i].CanDrag = true;
		}
	}
	
	public bool SortCard(int sortID,List<CardItem> ci) {
		CardItem firstCard = ci[0];
		
		int index = ci.FindIndex(a =>a.location == sortID);
		
		if(index >= 0)
			return false;
		
		bool bigger = sortID > firstCard.location;
		
		CheckMove(sortID,ci,bigger);
		
		if(moveItem.Count == 0)
			return false;
		
		int moveCount = bigger ? -ci.Count : ci.Count;
		
		for (int i = 0; i < moveItem.Count; i++) {
			Vector3 position = moveItem[i].transform.localPosition;
			
			Vector3 to = new Vector3(position.x + moveCount * cardInterv,position.y,position.z);
			
			moveItem[i].Move(to);
			
			moveItem[i].location += moveCount;
		}
		
		for (int i = 0; i < ci.Count; i++) 
		{
			Vector3 pos = cardItemArray[sortID].transform.localPosition;
			Vector3 to;
			
			int plus = bigger ? -i : i;
			
			to = new Vector3(pos.x + plus * cardInterv,pos.y,pos.z);
			
			ci[i].location = sortID + plus;
			if(i == 0)
				ci[i].SetPos (to);	
			else
				ci[i].Move(to);
		}
		
		if (ci.Count > 1) {
			ci [ci.Count - 1].tweenCallback += HandletweenCallback;
		}
		else {
			moveItem[moveItem.Count - 1].tweenCallback += HandletweenCallback;;
		}
		
		moveItem.Clear();
		
		return true;
	}
	
	void HandletweenCallback (CardItem arg1) {
		arg1.tweenCallback -= HandletweenCallback;
		
		for (int i = 0; i < cardItemArray.Length - 1; i++) {
			for (int j = i; j < cardItemArray.Length; j++) 
			{
				if(cardItemArray[i].location > cardItemArray[j].location)
				{
					CardItem temp = cardItemArray[i];
					cardItemArray[i] = cardItemArray[j];
					cardItemArray[j] = temp;
				}
			}
		}
		
		if(CallBack != null)
			CallBack();
	}
	
	void CheckMove(int startID, List<CardItem> firstCard,bool bigger) {
		if(firstCard.FindIndex(a => a.location == startID) == -1)
		{
			moveItem.Add(cardItemArray[startID]);
			
			if(bigger)
				startID --;
			else
				startID ++;
			
			CheckMove(startID,firstCard,bigger);
		}
		else
			return;
	}
	
	void SetFront(int sID,int itemID) {
		int countID = sID - 1;
		
		if(countID < 0)
			return;
		
		if(cardItemArray[countID].SetCanDrag(itemID))
		{
			SetFront(countID,itemID);
		}
		else
		{
			while(countID > -1)
			{
				cardItemArray[countID].CanDrag = false;
				countID --;
			}
		}
	}
	
	void SetBehind(int sID,int itemID) {
		int countID = sID + 1;
		
		if(countID >= cardItemArray.Length)
			return;
		
		if(cardItemArray[countID].SetCanDrag(itemID))
		{
			SetBehind(countID,itemID);
		}
		else
		{
			while(countID < cardItemArray.Length)
			{
				cardItemArray[countID].CanDrag = false;
				countID ++;
			}
		}
	}
	////------------battle card end
	/// 
	/// 

	////---------------user guide animation
	void UserGuideAnim(object data) {
		if (data == null) {
			ShowGuideAnim ();
		} else {
			bool b = (bool)data;
			ShowGuideAnim(b);
		}
	}
	
	bool shileInputByNoviceGuide = false;
	public void ShowGuideAnim(bool rePlay = false) {
		//		Debug.LogError ("ShowGuideAnim : " + rePlay);
		MsgCenter.Instance.Invoke (CommandEnum.ShiledInput, true);
		shileInputByNoviceGuide = true;
		if (rePlay) {
			ConfigBattleUseData.Instance.storeBattleData.colorIndex -= 20;
			GenerateShowCard();
		}
		ConfigBattleUseData.Instance.NotDeadEnemy = true;
		GameTimer.GetInstance ().AddCountDown (1f, GuideCardAnim);
	}
	/// <summary>
	/// drag time must bigger than move time
	/// </summary>
	const float dragTime = 0.25f;
	const float moveTime = 0.20f;
	
	GameTimer gameTimer;
	public void GuideCardAnim() {
		MsgCenter.Instance.AddListener(CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		ConfigBattleUseData.Instance.NotDeadEnemy = true;
		gameTimer = GameTimer.GetInstance ();
		GameObject target = cardItemArray [3].gameObject;
		
		Vector3 toPosition = cardItemArray [2].transform.position;
		selectTarget.Add ( cardItemArray [3] );
		iTween.MoveTo ( target, iTween.Hash ("position", toPosition, "time", moveTime) );
		MoveFinger (target.transform.position, toPosition, moveTime);
		
		gameTimer.AddCountDown ( dragTime, AnimStep1 );
	}
	
	void MoveFinger(Vector3 startPosition, Vector3 toPosition, float time) {
		if (!fingerObject.activeSelf) {
			fingerObject.SetActive (true);	
		}
		
		fingerObject.transform.position = startPosition;
		
		iTween.MoveTo ( fingerObject, iTween.Hash ("position", toPosition, "time", time) );
	}
	
	void AttackEnemyEnd(object data) {
		MsgCenter.Instance.RemoveListener(CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		ConfigBattleUseData.Instance.NotDeadEnemy = false;
	}

	private byte[] indexArray = new byte[19]{ 3, 2, 2, 1, 1, 1, 2, 2, 2, 2, 1, 2, 3, 3, 3, 2, 3, 2, 1 };
	
	public void AddGuideCard () {
		for (int i = 0; i < indexArray.Length; i++) {
			ConfigBattleUseData.Instance.questDungeonData.Colors.Insert(0, indexArray[i]);
		}
	}

	void AnimStep1() {
		Vector3 point = selectTarget[0].transform.localPosition;
		int indexID = CaculateSortIndex( point );
		SortCard (indexID, selectTarget);
		CallBack += AnimStep1End;
	}
	
	void AnimStep1End() {
		CallBack -= AnimStep1End;
		ResetClick ();
		AnimStep2 ();
	}
	
	List<int> indexCache = new List<int>();
	
	void AnimStep2 () {
		//		Debug.LogError("AnimStep2");
		indexCache.Clear ();
		indexCache.Add (3);
		GenerateAllCard (indexCache, 3 , AnimStep2End);
	}
	
	void AnimStep2End() {
		//		Debug.LogError("AnimStep2 end");
		GenerateAllCard (new List<int> { 3 }, 3 , AnimStep3);
	}
	
	// 1) 4 - 4; 
	// 2) 4 - 4;
	// 3) 2,3-3;
	// 4) 1-1;
	// 5) 1-1;
	// 6) 3-1; 
	// 7) 1,2,3-3;
	// 8) 4,5-5;
	// 9) 2,3,4-2
	
	void AnimStep3() {
		//		Debug.LogError("AnimStep3");
		GenerateAllCard (new List<int> { 1, 2 }, 2 , AnimStep4);
	}
	
	void AnimStep4() {
		//		Debug.LogError("AnimStep4");
		GenerateAllCard (new List<int> { 0 }, 0 , AnimStep5);
	}
	
	void AnimStep5() {
		//		Debug.LogError("AnimStep5");
		GenerateAllCard (new List<int> { 0 }, 0 , AnimStep6);
	}
	
	void AnimStep6() {
		GenerateAllCard (new List<int> { 2 }, 0 , AnimStep7);
	}
	
	void AnimStep7() {
		GenerateAllCard (new List<int> { 0, 1, 2 }, 2 , AnimStep8);
	}
	
	void AnimStep8() {
		GenerateAllCard (new List<int> { 3, 4 }, 4 , AnimStep9);
	}
	
	void AnimStep9() {
		GenerateAllCard (new List<int> { 1, 2, 3 }, 1 , AnimEnd);
	}
	
	void AnimEnd() {
		fingerObject.SetActive (false);
		
		//		System.Action action = ;
		GameTimer.GetInstance().AddCountDown(1f, () => { MsgCenter.Instance.Invoke (CommandEnum.ShiledInput, false); } );
	}

	int userGuideIndex = -1;
	void UserGuideCard(object data) {
		userGuideIndex = (int)data;
		if (userGuideIndex == -1) {
			return;
		}
		
		for (int i = 0; i < cardPosition.Length; i++) {
			GenerateSpriteCard(userGuideIndex, i);
		}
		
		RefreshLine();
	}
	
	
	
	int generateIndex = 0;
	Callback GenerateEnd ;
	void GenerateAllCard (List<int> fromIndex, int toIndex, Callback cb) {
		if (fromIndex.Count == 0) {
			return;	
		}
		
		generateIndex = toIndex;
		GenerateEnd = cb;
		
		if (fromIndex.Count == 1) {
			GenerateCard(fromIndex[0]);
			return;
		}
		
		GenerateAll (fromIndex);
	}
	
	void GenerateCard(int fromIndex) {
		CardItem ci = cardItemArray [fromIndex];
		GameObject target = ci.gameObject;
		Vector3 toPosition = battleCardAreaItem [generateIndex].transform.position;
		selectTarget.Add (ci);
		MoveFinger (target.transform.position, toPosition, moveTime);
		iTween.MoveTo (target, iTween.Hash ("position", toPosition, "time", moveTime));
		gameTimer.AddCountDown (dragTime, GenerateCardEnd);
	} 
	
	int cardIndex = 0;
	int startIndex = 0;
	List<int> fromIndexCache;
	
	void GenerateAll(List<int> fromIndex) {
		fromIndexCache = fromIndex;
		
		cardIndex = 0;
		
		selectTarget.Add (cardItemArray [fromIndexCache[startIndex]]);
		
		MoveAll ();
	}
	
	void MoveAll() {
		GameInput.OnUpdate += OnUpdate;
		
		for (int i = 0; i < selectTarget.Count; i++) {
			selectTarget[i].ActorTexture.depth += selectTarget.Count ;
		}
		
		int nextIndex = cardIndex + 1;
		GameObject target = selectTarget [startIndex].gameObject;
		if (nextIndex == fromIndexCache.Count) {
			iTween.MoveTo (target, battleCardAreaItem [generateIndex].transform.position, moveTime);
			
			gameTimer.AddCountDown(dragTime, MoveAllToPosition);
			
			MoveFinger(target.transform.position, battleCardAreaItem [generateIndex].transform.position, moveTime);
		} else {
			CardItem nextCi = cardItemArray [fromIndexCache [nextIndex]];
			
			cardIndex = nextIndex;
			
			iTween.MoveTo (target, nextCi.transform.position, 0.2f);
			
			gameTimer.AddCountDown (dragTime, MoveAllEnd);
			
			MoveFinger(target.transform.position, nextCi.transform.position, moveTime);
		}
	}
	
	void MoveAllToPosition() {
		GameInput.OnUpdate -= OnUpdate;
		GenerateCardEnd ();
	}
	
	void OnUpdate () {
		if (selectTarget.Count == 0) {
			return;	
		}
		
		for (int i = 0; i < selectTarget.Count; i++) {
			selectTarget [i].OnDragHandler (selectTarget[startIndex].transform.localPosition, i);
		}
	}
	
	void MoveAllEnd() {
		if (cardIndex >= fromIndexCache.Count) {
			GameInput.OnUpdate -= OnUpdate;
			GenerateCardEnd();		
			return;
		}
		ClickCardItem (cardItemArray [fromIndexCache [cardIndex]]);
		MoveAll ();
	}
	
	void GenerateCardEnd () {
		int generateCount = battleCardAreaItem [generateIndex].GenerateCard(selectTarget);
		if(generateCount > 0) {
			MsgCenter.Instance.Invoke(CommandEnum.StateInfo,"");
//			YieldStartBattle();
//			if(showCountDown) {
//				for(int i = 0;i < generateCount;i++) {
//					GenerateSpriteCard(GenerateCardIndex(),selectTarget[i].location);
//				}
//				RefreshLine();
//			}
		}
		
		for (int i = 0; i < selectTarget.Count; i++) {
			selectTarget[i].ActorTexture.depth = 6;
		}
		
		ResetClick();
		
		if (GenerateEnd != null) {
			gameTimer.AddCountDown(0.1f, GenerateEnd);
		}
	}
}
