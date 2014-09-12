using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManipulationModule : ModuleBase {
	private static UIRoot uiRoot;
	private static Camera mainCamera;
	private UICamera nguiMainCamera;
	public GameObject battleRootGameObject;
	private RaycastHit[] rayCastHit;
	private CardItem tempCard;
	private GameObject tempObject;

//	private BattleCardPool battleCardPool;
//	private BattleCard battleCard;
//	public BattleCard BattleCardIns{ 
//		get { return battleCard; } 
//	}
//	private FightManipulationView battleCardArea;
//	private BattleEnemy battleEnemy;
//	private CountDownUnity countDownUI;
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

//	private ConfigBattleUseData battleData;

	public BattleManipulationModule(UIConfigItem config):base(  config) {
		CreateUI<BattleManipulationView> ();

		uiRoot = ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>();
		nguiMainCamera = ViewManager.Instance.MainUICamera;
		mainCamera = nguiMainCamera.camera;
		battleRootGameObject = NGUITools.AddChild(ViewManager.Instance.ParentPanel);
		battleRootGameObject.name = "Fight";
		battleRootGameObject.transform.localPosition = localPosition;

		Vector3 pos = battleRootGameObject.transform.localPosition;
		battleRootGameObject.transform.localPosition = new Vector3(pos.x,pos.y,pos.z + ZOffset);
		
		GameInput.OnPressEvent += HandleOnPressEvent;
		GameInput.OnReleaseEvent += HandleOnReleaseEvent;
		GameInput.OnStationaryEvent += HandleOnStationaryEvent;
		GameInput.OnDragEvent += HandleOnDragEvent;

//		battleData = ConfigBattleUseData.Instance;
	}

	private Callback initEndCallback = null;

	public void InitUI(Callback initEndCallback) {
		this.initEndCallback = initEndCallback;
		GameInput.OnUpdate += InitUpdate;
		InitUI ();
	}

	void InitUpdate() {
		if (initEnd == 5) {
			GameInput.OnUpdate -= InitUpdate;
			if(initEndCallback != null){
				initEndCallback();
			}
		}
	}

	private int initEnd = 0;

	public override void InitUI () {
		base.InitUI ();

		initEnd = 0;
	}

	public override void ShowUI() {
		base.ShowUI();
		ShowCard();
//		Debug.LogError ("NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION :" + (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION));
		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.ANIMATION) {
			AddGuideCard ();
			ConfigBattleUseData.Instance.storeBattleData.colorIndex = 0;
		}

//		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttckEnd);
		MsgCenter.Instance.AddListener (CommandEnum.ChangeCardColor, ChangeCard);
		MsgCenter.Instance.AddListener (CommandEnum.DelayTime, DelayTime);
		MsgCenter.Instance.AddListener (CommandEnum.ExcuteActiveSkill, ExcuteActiveSkillInfo);
		MsgCenter.Instance.AddListener (CommandEnum.UserGuideAnim, UserGuideAnim);
		MsgCenter.Instance.AddListener (CommandEnum.UserGuideCard, UserGuideCard);

//		UserGuideAnim (null);
	}

	
	public override void HideUI () {
		base.HideUI ();
		//		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttckEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.ChangeCardColor, ChangeCard);
		MsgCenter.Instance.RemoveListener (CommandEnum.DelayTime, DelayTime);
		MsgCenter.Instance.RemoveListener (CommandEnum.ExcuteActiveSkill, ExcuteActiveSkillInfo);
		MsgCenter.Instance.RemoveListener (CommandEnum.UserGuideAnim, UserGuideAnim);
		MsgCenter.Instance.RemoveListener (CommandEnum.UserGuideCard, UserGuideCard);
		battleRootGameObject.SetActive(false);
	}

	private byte[] indexArray = new byte[19]{ 3, 2, 2, 1, 1, 1, 2, 2, 2, 2, 1, 2, 3, 3, 3, 2, 3, 2, 1 };

	public void AddGuideCard () {
		for (int i = 0; i < indexArray.Length; i++) {
			ConfigBattleUseData.Instance.questDungeonData.Colors.Insert(0, indexArray[i]);
		}
	}

	int userGuideIndex = -1;
	void UserGuideCard(object data) {
		userGuideIndex = (int)data;
		if (userGuideIndex == -1) {
			return;
		}

		for (int i = 0; i < (view as BattleManipulationView).CardPosition.Length; i++) {
			(view as BattleManipulationView).GenerateSpriteCard(userGuideIndex, i);
		}

		(view as BattleManipulationView).RefreshLine();
	}

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
		GameObject target = (view as BattleManipulationView).cardItemArray [3].gameObject;

		Vector3 toPosition = (view as BattleManipulationView).cardItemArray [2].transform.position;
		selectTarget.Add ( (view as BattleManipulationView).cardItemArray [3] );
		iTween.MoveTo ( target, iTween.Hash ("position", toPosition, "time", moveTime) );
		MoveFinger (target.transform.position, toPosition, moveTime);

		gameTimer.AddCountDown ( dragTime, AnimStep1 );
	}

	void MoveFinger(Vector3 startPosition, Vector3 toPosition, float time) {
//		if (!(view as FightManipulationView).fingerObject.activeSelf) {
//			(view as FightManipulationView).fingerObject.SetActive (true);	
//		}
	
//		(view as FightManipulationView).fingerObject.transform.position = startPosition;
//
//		iTween.MoveTo ( (view as FightManipulationView).fingerObject, iTween.Hash ("position", toPosition, "time", time) );
	}

	void AttackEnemyEnd(object data) {
		MsgCenter.Instance.RemoveListener(CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		ConfigBattleUseData.Instance.NotDeadEnemy = false;
	}

	void AnimStep1() {
		Vector3 point = selectTarget[0].transform.localPosition;
		int indexID = (view as BattleManipulationView).CaculateSortIndex( point );
		(view as BattleManipulationView).SortCard (indexID, selectTarget);
		(view as BattleManipulationView).CallBack += AnimStep1End;
	}

	void AnimStep1End() {
		(view as BattleManipulationView).CallBack -= AnimStep1End;
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
//		(view as FightManipulationView).fingerObject.SetActive (false);

//		System.Action action = ;
		GameTimer.GetInstance().AddCountDown(1f, () => { MsgCenter.Instance.Invoke (CommandEnum.ShiledInput, false); } );
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
		CardItem ci = (view as BattleManipulationView).cardItemArray [fromIndex];
		GameObject target = ci.gameObject;
		Vector3 toPosition = (view as BattleManipulationView).battleCardAreaItem [generateIndex].transform.position;
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

		selectTarget.Add ((view as BattleManipulationView).cardItemArray [fromIndexCache[startIndex]]);

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
			iTween.MoveTo (target, (view as BattleManipulationView).battleCardAreaItem [generateIndex].transform.position, moveTime);

			gameTimer.AddCountDown(dragTime, MoveAllToPosition);

			MoveFinger(target.transform.position, (view as BattleManipulationView).battleCardAreaItem [generateIndex].transform.position, moveTime);
		} else {
			CardItem nextCi = (view as BattleManipulationView).cardItemArray [fromIndexCache [nextIndex]];

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
			selectTarget [i].OnDrag (selectTarget[startIndex].transform.localPosition, i);
		}
	}

	void MoveAllEnd() {
		if (cardIndex >= fromIndexCache.Count) {
			GameInput.OnUpdate -= OnUpdate;
			GenerateCardEnd();		
			return;
		}
		ClickCardItem ((view as BattleManipulationView).cardItemArray [fromIndexCache [cardIndex]]);
		MoveAll ();
	}
		                 
	void GenerateCardEnd () {
		int generateCount = (view as BattleManipulationView).battleCardAreaItem [generateIndex].GenerateCard(selectTarget);
		if(generateCount > 0) {
			MsgCenter.Instance.Invoke(CommandEnum.StateInfo,"");
			YieldStartBattle();
			if(showCountDown) {
				for(int i = 0;i < generateCount;i++) {
					(view as BattleManipulationView).GenerateSpriteCard(GenerateCardIndex(),selectTarget[i].location);
				}
				(view as BattleManipulationView).RefreshLine();
			}
		}

		for (int i = 0; i < selectTarget.Count; i++) {
			selectTarget[i].ActorTexture.depth = 6;
		}

		ResetClick();

		if (GenerateEnd != null) {
			gameTimer.AddCountDown(0.1f, GenerateEnd);
		}
	}


	public override void DestoryUI () {
		GameInput.OnPressEvent -= HandleOnPressEvent;
		GameInput.OnReleaseEvent -= HandleOnReleaseEvent;
		GameInput.OnStationaryEvent -= HandleOnStationaryEvent;
		GameInput.OnDragEvent -= HandleOnDragEvent;
		base.DestoryUI ();
		GameObject.Destroy (battleRootGameObject);
//		countDownUI.DestoryUI ();
	}

	public void StartBattle () {
		ResetClick();
		Attack();
		(view as BattleManipulationView).StartBattle (false);
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
//			GenerateShowCard();
		} 
		else {
			for (int i = 0; i < (view as BattleManipulationView).CardPosition.Length; i++) {
				(view as BattleManipulationView).ChangeSpriteCard(ccc.sourceType,ccc.targetType,i);
			}
			(view as BattleManipulationView).RefreshLine();
		}
	}

	void EnemyAttckEnd (object data) {
		(view as BattleManipulationView).StartBattle (true);
		ShieldInput (true);
	}

	void EnemyAttackEnd() {

	}

//	void BattleEnd (object data) {
//		ExitFight ();
//	}
//
//	public void ExitFight() {
//		AudioManager.Instance.PlayAudio (AudioEnum.sound_battle_over);
//
//		isShowEnemy = false;
//		ShieldInput (true);
//		HideUI ();
//	}

	void Attack () {
		MsgCenter.Instance.Invoke (CommandEnum.StartAttack, null);
	}
	
	void ShowCard () {
		if(!battleRootGameObject.activeSelf)
			battleRootGameObject.SetActive(true);
	}

	public bool IsBoss = false;
	public bool isShowEnemy = false;
	public void ShowEnemy(List<TEnemyInfo> count, bool isBoss = false) {
		isShowEnemy = true;
		IsBoss = isBoss;
//		Debug.LogError ("IsBoss : " + IsBoss);
		ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh",count);
//		MsgCenter.Instance.Invoke (CommandEnum.ReduceActiveSkillRound);
		ConfigBattleUseData.Instance.storeBattleData.tEnemyInfo = count;
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


	void GetPrefabsObject(string name,ResourceCallback callback) {
		ResourceManager.Instance.LoadLocalAsset ("Prefabs/" + name,o =>{
			tempObject = o  as GameObject;
			GameObject go = GameObject.Instantiate(tempObject) as GameObject;
			
			if (go != null && battleRootGameObject != null) {
				Transform t = go.transform;
				t.parent = battleRootGameObject.transform;
				t.localPosition = Vector3.zero;
				t.localRotation = Quaternion.identity;
				t.localScale = Vector3.one;
			}
			callback(go);
		});

	}

	int GenerateData() {
		ItemData id = Config.Instance.GetCard();
		allItemData.Add(id);
		return id.itemID;
	}

	int GenerateCardIndex () {
		int index = ConfigBattleUseData.Instance.questDungeonData.Colors [ConfigBattleUseData.Instance.storeBattleData.colorIndex];
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
		Main.Instance.GInput.IsCheckInput = !isShield;
	}

	public void ShieldGameInput(bool isShield) {
		Main.Instance.GInput.IsCheckInput = isShield;
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
		Main.Instance.GInput.IsCheckInput = isShield;
	}

	void HandleOnPressEvent () {
		DisposePress();
	}

	void HandleOnReleaseEvent () {
		DisposeReleasePress();
	}

	void HandleOnStationaryEvent () {
		
	}

	void HandleOnDragEvent (Vector2 obj) {
		DisposeOnDrag(obj);
	}

	void ResetClick() {
		for (int i = 0; i < selectTarget.Count; i++) {
//			Debug.LogError ("ResetClick selectTarget.Count : " + selectTarget.Count + " selectTarget : " + selectTarget[i].name);
			selectTarget[i].OnPress(false, -1);
		}
		selectTarget.Clear();
		(view as BattleManipulationView).ResetDrag();
	}

	void DisposeReleasePress() {
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
			YieldStartBattle();
			
			if(showCountDown) {
				for(int i = 0;i < generateCount;i++) {
					(view as BattleManipulationView).GenerateSpriteCard(GenerateCardIndex(),selectTarget[i].location);
				}
				(view as BattleManipulationView).RefreshLine();
			}
		}
		ResetClick();
	}

	void SwitchCard() {
		Vector3 point = selectTarget[0].transform.localPosition;
		int indexID = (view as BattleManipulationView).CaculateSortIndex(point);
		if(indexID >= 0) {
			Main.Instance.GInput.IsCheckInput = false;
			if((view as BattleManipulationView).SortCard(indexID, selectTarget)) {
				(view as BattleManipulationView).CallBack += HandleCallBack;
			}
			else {
				Main.Instance.GInput.IsCheckInput = true; 
				ResetClick();
			}
		}
		else {
			ResetClick();
		}
	}
	
	void HandleCallBack () {
		(view as BattleManipulationView).CallBack -= HandleCallBack;
		Main.Instance.GInput.IsCheckInput = true;
		ResetClick();
	}

	void TweenCallback(GameObject go) {
		Main.Instance.GInput.IsCheckInput = true;
	}
	
	private BattleCardAreaItem prevTempBCA;

	void DisposeOnDrag(Vector2 obj) {
		SetDrag();
		Vector3 vec = ChangeCameraPosition(obj) - ViewManager.Instance.ParentPanel.transform.localPosition;

		for (int i = 0; i < selectTarget.Count; i++) {
			selectTarget [i].OnDrag (vec, i);
		}

		bool b = Check(GameLayer.ActorCard);

		if(b) {
			for (int i = 0; i < rayCastHit.Length; i++) {
				tempObject = rayCastHit[i].collider.gameObject;
				
				ClickObject(tempObject);
			}
		}
	}

	void DisposePress() {
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
	
	void SetDrag() {
		if(selectTarget.Count > 0)
			(view as BattleManipulationView).DisposeDrag(selectTarget[0].location,selectTarget[0].itemID);
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
				ci.OnPress(true, selectTarget.Count);
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
		
		Vector3 reallyPoint = worldPoint * height * uiRoot.pixelSizeAdjustment;

		return reallyPoint;
	}

	public static Vector3 ChangeCameraPosition(Vector3 pos) {
		Vector3 worldPoint = mainCamera.ScreenToWorldPoint(pos);
		
		float height = (float)Screen.height / 2;
		
		Vector3 reallyPoint = worldPoint * height * uiRoot.pixelSizeAdjustment;
		
		return reallyPoint;
	}

	public static Vector3 ChangeDeltaPosition(Vector3 delta) {
		return delta * uiRoot.pixelSizeAdjustment;
	}
	
	void DelayTime(object data) {
		activeDelay =  (float)data;
	}

	public bool showCountDown = false;
	float time = 5f;
	float countDownTime = 1f;
	float activeDelay = 0f;
	public void YieldStartBattle () {
		if (showCountDown) {
			return ;		
		} 
		ShieldNGUIInput (false);
		time = BattleUseData.CountDown + activeDelay;
//		countDownUI.ShowUI();
		activeDelay = 0f;
		CountDownBattle ();
	}
	
	void CountDownBattle () {
		int temp = (int)time;
//		countDownUI.SetCurrentTime (temp);
		if (time > 0) {
			BattleBottomView.notClick = true;
			showCountDown = true;
			time -= countDownTime;
			GameTimer.GetInstance ().AddCountDown (countDownTime, CountDownBattle);
		} else {
			ResetClick();
			ShieldInput (false);
//			MsgCenter.Instance.Invoke(CommandEnum.ReduceActiveSkillRound);
			showCountDown = false;
			(view as BattleManipulationView).ShowCountDown (false, (int)time);
			ShieldNGUIInput (true);
			StartBattle();
			time =  BattleUseData.CountDown;
		}
	}
}