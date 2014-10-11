using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

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
//	private List<ItemData> allItemData = new List<ItemData>();
	private List<CardItem> selectTarget = new List<CardItem>();
	//temp-----------------------------------------------------------
	private List<int> currentColor = new List<int>();
	//end------------------------------------------------------------
	public int cardHeight = 0;
	private Vector3 localPosition = new Vector3 (-0.18f, -17f, 0f);

	////------------battle card pool start
	private UISprite templateBackTexture;
	
	private Vector3[] cardPosition;
	
	private UISprite[] backTextureIns;
	private int cardWidth = 0;
	private Vector3 initPosition = Vector3.zero;
	private float xStart = 0f;
	
	////------------battle card pool end

	////------------battle card area start
	private UISprite backTexture;
	private BattleCardAreaItem[] battleCardAreaItem;
	private UILabel stateLabel;
	private Vector3 sourcePosition;
	private const int oneWordSize = 42;

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
	
	private List<NormalSkill> normalSkill ;
	
//	private BattleUseData battleUseData;
	
	private GameObject fingerObject;

	private CardItem[] cardItemArray;
	/// ------------battle card end
	/// 

	////------------count down start
	private UILabel numberLabel;
	private UISprite circleSprite;
	private float countDownValue = 1f;

	////------------count down end
	/// 

	private bool isCardDragable = true;
	private bool isInCountingDown = false;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
//		uiRoot = ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>();
		nguiMainCamera = ViewManager.Instance.MainUICamera;
		mainCamera = nguiMainCamera.camera;

		battleCard = FindChild("BattleCard");
		////------------battle card pool start
		battleCardPool = FindChild("BattleCardPool");

		int count = BattleConfigData.cardPoolSingle;
		cardPosition = new Vector3[count];
		backTextureIns = new UISprite[count];		
		templateBackTexture = FindChild<UISprite>("BattleCardPool/Back");
		cardWidth = templateBackTexture.width + BattleConfigData.cardSep;
		initPosition = BattleConfigData.cardPoolInitPosition;

		for (int i = 0; i < cardPosition.Length; i++) {
			tempObject = NGUITools.AddChild(battleCardPool, templateBackTexture.gameObject);
			cardPosition[i] = new Vector3(initPosition.x + i * cardWidth,initPosition.y,initPosition.z);
			tempObject.transform.localPosition = cardPosition[i];
			backTextureIns[i] = tempObject.GetComponent<UISprite>();
		}
		templateBackTexture.gameObject.SetActive(false);

//		NGUITools.AddWidgetCollider(battleCardPool);
		BoxCollider bc = battleCardPool.GetComponent<BoxCollider> ();
		xStart = battleCard.transform.localPosition.x - bc.size.x / 2f;
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

		templateItemCard = FindChild<UISprite>("BattleCard/Texture");
		fingerObject = FindChild<UISprite> ("BattleCard/finger").gameObject;
		
		int cardcount = cardPosition.Length;
		
		cardItemArray = new CardItem[cardcount];
		for (int i = 0; i < cardcount; i++) {
			tempObject = NGUITools.AddChild(battleCard,templateItemCard.gameObject);
			tempObject.transform.localPosition = cardPosition[i];
			CardItem ci = tempObject.AddComponent<CardItem>();
			ci.index = i;
			ci.Init(i.ToString());
			cardItemArray[i] = ci;
		}
		
		templateItemCard.gameObject.SetActive(false);
		cardWidth = (int)Mathf.Abs(cardPosition[1].x - cardPosition[0].x);
//		battleCard.battleCardArea = battleCardArea;
		////------------battle card end

		countDownUI = FindChild ("CountDown");
		countDownUI.SetActive (false);
		numberLabel = FindChild<UILabel>("CountDown/Number");
		circleSprite = FindChild<UISprite>("CountDown/Circle");
//		CreatEnemy ();
	}

	public override void ShowUI() {
		base.ShowUI();
		//		Debug.LogError ("NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION :" + (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION));
//		if (NoviceGuideStepManager.Instance.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION) {
////			AddGuideCard ();
//			BattleConfigData.Instance.storeBattleData.colorIndex = 0;
//		}
		//		Debug.LogError ("isShow");
		GenerateShowCard();

		GameInput.OnUpdate += HandleOnUpdate;
		
		//		UserGuideAnim (null);
		GameInput.OnPressEvent += HandleOnPressEvent;
		GameInput.OnReleaseEvent += HandleOnReleaseEvent;
		GameInput.OnStationaryEvent += HandleOnStationaryEvent;
		GameInput.OnDragEvent += HandleOnDragEvent;
		isCardDragable = true;
	}
	
	public override void HideUI () {
		base.HideUI ();

		GameInput.OnUpdate -= HandleOnUpdate;
		GameInput.OnPressEvent -= HandleOnPressEvent;
		GameInput.OnReleaseEvent -= HandleOnReleaseEvent;
		GameInput.OnStationaryEvent -= HandleOnStationaryEvent;
		GameInput.OnDragEvent -= HandleOnDragEvent;
	}
	
	public override void DestoryUI () {
		GameInput.OnUpdate -= HandleOnUpdate;
		GameInput.OnPressEvent -= HandleOnPressEvent;
		GameInput.OnReleaseEvent -= HandleOnReleaseEvent;
		GameInput.OnStationaryEvent -= HandleOnStationaryEvent;
		GameInput.OnDragEvent -= HandleOnDragEvent;
		base.DestoryUI ();
	}

	public override void CallbackView (params object[] args)
	{

		switch (args[0].ToString()) {
			case "attack_enemy_end":
	//			StartBattle(false);
				Debug.Log("area clear start: "+ battleCardAreaItem.Length);
				foreach (var item in battleCardAreaItem) {
					item.ClearCard();
				}
				break;
			case "attack_enemy":
				foreach (var item in battleCardAreaItem) {
					item.AttackEnemy(args[1]);
				}
				break;
			case "ban_click":
				break;
			case "enemy_attack_end":
				isCardDragable = true;
				break;
			case "state_info":
				StateInfo(args[1]);
				break;
			case "guide":
			switch((int)args[1]){
			case 1:
				break;
			case 2:
				break;
			}
			break;
			default:
					break;
		}
	}
	
	void GenerateShowCard () {
		//		Debug.LogError ("GenerateShowCard");
		for (int i = 0; i < cardPosition.Length; i++) {
			GenerateSpriteCard(GenerateCardIndex(), i);
		}
//		RefreshLine ();
	}

	int GenerateCardIndex () {
		int index =  BattleConfigData.Instance.ResumeColorIndex();
		
		currentColor.Add (index);
		if (currentColor.Count > 5) {
			currentColor.RemoveAt(0);
		}
		return index;
	}
	
	void HandleOnPressEvent () {
		if (!isCardDragable)
			return;
		if(Check(GameLayer.ActorCard)) {
			for (int i = 0; i < rayCastHit.Length; i++) {
				if(rayCastHit[i].collider.gameObject.layer == GameLayer.ActorCard) {
					tempObject= rayCastHit[i].collider.gameObject;
					break;
				}
				else
					continue;
			}
			AddToSelect(tempObject.GetComponent<CardItem>());
			SetCardItemDragState();
		}
	}
	
	void HandleOnReleaseEvent () {
//		Debug.Log ("press release");
		if(selectTarget.Count == 0 || !isCardDragable) {
			ResetSelect();
			return;
		}
		
		if(Check(GameLayer.BattleCard)) {
			GenerateCard();
		}
		else if(Check(GameLayer.ActorCard)) {
			SwitchCard();
		}
		else {
			ResetSelect();
		}
	}

	void HandleOnStationaryEvent () {
		
	}
	
	void HandleOnDragEvent (Vector2 obj) {
//		Debug.Log ("drag");
		if (!isCardDragable) {
			return;		
		}
		SetCardItemDragState();
		Vector3 vec = ChangeCameraPosition(obj) - ViewManager.Instance.ParentPanel.transform.localPosition;
		
		for (int i = 0; i < selectTarget.Count; i++) {
			selectTarget [i].OnDragHandler (vec, i);
		}

		//		bool b = Check(GameLayer.Default);
		
		if(Check(GameLayer.ActorCard)) {
			for (int i = 0; i < rayCastHit.Length; i++) {
				AddToSelect(rayCastHit[i].collider.gameObject.GetComponent<CardItem>());
			}
		}
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
			generateCount = bcai.UpdateCardAndAttackInfo(selectTarget);
		
		if(generateCount > 0) {
			StateInfo("");
//			if (showCountDown) {
//				return ;		
//			} 

			//		countDownUI.ShowUI();
			if(!isInCountingDown){
				StartCoroutine(StartCountDown ());
				isInCountingDown = true;
			}
			for(int i = 0;i < generateCount;i++) {
				GenerateSpriteCard(GenerateCardIndex(),selectTarget[i].index);
//				RefreshLine();
			}
		}
		ResetSelect();
	}

	IEnumerator StartCountDown () {
		time = (int)BattleAttackManager.CountDown + dynamicDelay;
		dynamicDelay = 0;
		countDownUI.SetActive(true);
		while (time > 0) {
			AudioManager.Instance.PlayAudio (AudioEnum.sound_count_down);
			iTween.ScaleFrom (countDownUI,  iTween.Hash("islocal", true, "time",0.3f, "easetype",iTween.EaseType.easeOutBack,"scale",new Vector3 (1.4f, 1.4f, 1.4f)) );
			countDownValue = 1f;
			numberLabel.text = time.ToString ();
			yield return new WaitForSeconds(1);
			time -= countDownTime;
		}
		countDownUI.SetActive(false);
		ResetSelect();
		//			ShieldInput (false);
		//			MsgCenter.Instance.Invoke(CommandEnum.ReduceActiveSkillRound);
		//			showCountDown = false;
		BattleAttackManager.Instance.StartAttack (null);
//		RefreshLine();
		isCardDragable = false;
		time =  BattleAttackManager.CountDown;
		isInCountingDown = false;
	}

	void ResetSelect() {
//		Debug.Log ("reset select");
		for (int i = 0; i < selectTarget.Count; i++) {
//						Debug.LogError ("ResetClick selectTarget.Count : " + selectTarget.Count + " selectTarget : " + selectTarget[i].name);
			selectTarget[i].AddCardToSelect(false, -1);
		}
		selectTarget.Clear();
		for (int i = 0; i < cardItemArray.Length; i++) {
			if(cardItemArray[i] == null) {
				continue;
			}
			cardItemArray[i].IsDraggable = true;
		}
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
				ResetSelect();
			}
		}
		else {
			ResetSelect();
		}
	}
	
	void HandleCallBack () {
		CallBack -= HandleCallBack;
//		Main.Instance.GInput.IsCheckInput = true;
		ResetSelect();
	}
	
	void TweenCallback(GameObject go) {
//		Main.Instance.GInput.IsCheckInput = true;
	}
	
	private BattleCardAreaItem prevTempBCA;
	
	void SetCardItemDragState() {
		if (selectTarget.Count > 0) {
			int index = selectTarget[0].index;
			int colorType = selectTarget[0].colorType;
			SetFront(index,colorType);
			SetBehind(index,colorType);
		}
	}
	
	void AddToSelect(CardItem ci) {
		if(ci != null) {
			if(selectTarget.Contains(ci))
				return;
			if(ci.IsDraggable) {
				ci.AddCardToSelect(true, selectTarget.Count);
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
	
	void DelayTime(object data) {
		dynamicDelay =  (float)data;
	}
	
	bool showCountDown = false;
	float time = 5f;
	float countDownTime = 1f;
	float dynamicDelay = 0f;

	////------------battle card pool start
	
	private int CaculateSortIndex(Vector3 point)
	{
		float x = point.x - xStart;
		
		for (int i = 0; i < cardPosition.Length; i++) 
		{
			if(x > cardWidth * i && x <= cardWidth * (i + 1))
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
		maxBoostRandom = NoviceGuideStepManager.Instance.isInNoviceGuide() ? 5 : 10;
		
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
		
		if (info == BattleAttackManager.stateInfo [0]) { //"Player Phase"
			SetBoost();
		}
		
		if (stateLabel.text == info) {
			return;	
		}
		
		if (info == BattleAttackManager.stateInfo [4]) { //"Active Skill"
			prevInfo = stateLabel.text;
		}
		
		Color32[] colors;
		
		if (info == BattleAttackManager.stateInfo [0]){ // "Player Phase"
			colors = BattleFullScreenTipsView.firstGroupColor;
		} else if( info == BattleAttackManager.stateInfo [1]) { // "Enemy Phase"
			colors = BattleFullScreenTipsView.thirdGroupColor;		
		} else {
			colors = BattleFullScreenTipsView.secondGroupColor;
		}
		
		BattleFullScreenTipsView.SetLabelGradient (stateLabel, colors);
		ModuleManager.SendMessage (ModuleEnum.BattleFullScreenTipsModule, "label_gradient", stateLabel);

		string preText = stateLabel.text;
		stateLabel.text = info;

		if (preText == string.Empty) {
			stateLabel.transform.localPosition = HidePosition;	
			ShowStateLabel ();
		} else {
			HideStateLabel("ShowStateLabel");
		}		
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
		if ( nextFunction == "")
			nextFunction = "ClearTexture";
		iTween.MoveTo(stateLabel.gameObject, iTween.Hash("x", x, "islocal", true,"time",0.25f,"easetype",iTween.EaseType.easeInCubic,"oncompletetarget",gameObject,"oncomplete", nextFunction));
	}
	
	void ClearTexture () {
		stateLabel.text = string.Empty;
	}
	
	void ShowStateLabel () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_text_appear);
		stateLabel.gameObject.transform.localScale = new Vector3(0.5f, 1.0f, 1.0f);
//		Debug.LogWarning("ShowStateLabel()  >>> from:"+stateLabel.gameObject.transform.localPosition.x+","+stateLabel.gameObject.transform.localPosition.y+" to: "+showPosition.x+","+showPosition.y );
		iTween.ScaleTo(stateLabel.gameObject, iTween.Hash ("x", 1.0f, "islocal", true,"delay", 0.05f, "time", 0.3f, "easetype",iTween.EaseType.easeOutBack));
		iTween.MoveTo (stateLabel.gameObject, iTween.Hash ("position", showPosition, "islocal", true, "time", 0.3f,"easetype",iTween.EaseType.easeOutBack));
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
		
		startPosition = new Vector3 (pos.x + cardHeight, pos.y + cardHeight * 0.0f, pos.z); //normal skill start position.
		
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
		if (ci.colorType == source) {
			ci.SetSprite (index,CheckGenerationAttack (index));
		}
	}
	
	/// <summary>
	/// new 
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="locationID">Location I.</param>
	void GenerateSpriteCard(int index,int locationID) {
		cardItemArray [locationID].SetSprite (index, CheckGenerationAttack (index));
	}
	
//	void RefreshLine() {
//		foreach (var item in cardItemArray) {
//			GenerateLinkSprite (item, item.itemID);
//		}
//	}
	
	void GenerateLinkSprite(CardItem ci,int index) {
		List<Transform> trans = new List<Transform> ();
		for (int i = 0; i < battleCardAreaItem.Length; i++) {
			if(battleCardAreaItem[i] == null) 
				continue;
			if(BattleAttackManager.Instance.upi.CalculateNeedCard( battleCardAreaItem[i].AreaItemID, index )) {
				trans.Add(battleCardAreaItem[i].transform);
			}
		}
		ci.SetTargetLine (trans);
	}

	bool CheckGenerationAttack (int index) {
		if (index == 7) {
			return true;
		}
		if(normalSkill == null)
			normalSkill = BattleAttackManager.Instance.upi.GetNormalSkill ();
		foreach (var item in normalSkill) {
			if(item.Blocks.Contains((uint)index)) {
				return true;
			}
		}
		
		return false;
	}
	
	bool SortCard(int sortID,List<CardItem> ci) {
		CardItem firstCard = ci[0];
		
		int index = ci.FindIndex(a =>a.index == sortID);
		
		if(index >= 0)
			return false;
		
		bool bigger = sortID > firstCard.index;
		
		CheckMove(sortID,ci,bigger);
		
		if(moveItem.Count == 0)
			return false;
		
		int moveCount = bigger ? -ci.Count : ci.Count;
		
		for (int i = 0; i < moveItem.Count; i++) {
			Vector3 position = moveItem[i].transform.localPosition;
			
			Vector3 to = new Vector3(position.x + moveCount * cardWidth,position.y,position.z);
			
			moveItem[i].Move(to);
			
			moveItem[i].index += moveCount;
		}
		
		for (int i = 0; i < ci.Count; i++) 
		{
			Vector3 pos = cardItemArray[sortID].transform.localPosition;
			Vector3 to;
			
			int plus = bigger ? -i : i;
			
			to = new Vector3(pos.x + plus * cardWidth,pos.y,pos.z);
			
			ci[i].index = sortID + plus;
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
				if(cardItemArray[i].index > cardItemArray[j].index)
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
		if(firstCard.FindIndex(a => a.index == startID) == -1)
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
	
	void SetFront(int index,int colorType) {
		int i = index - 1;
		
		if(i < 0)
			return;
		
		if(cardItemArray[i].SetCanDrag(colorType))
		{
			SetFront(i,colorType);
		}
		else
		{
			while(i > -1)
			{
				cardItemArray[i].IsDraggable = false;
				i --;
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
				cardItemArray[countID].IsDraggable = false;
				countID ++;
			}
		}
	}
	////------------battle card end
	/// 
	/// 
	////-----------count down start
	
	
	void HandleOnUpdate () {
		if (countDownUI.activeSelf) {
			countDownValue -= Time.deltaTime;
			circleSprite.fillAmount = countDownValue;
		}
		
	}

	////---------------count down end'

	///---------------guide
	void StartGuide(int type){

	}
	
}
