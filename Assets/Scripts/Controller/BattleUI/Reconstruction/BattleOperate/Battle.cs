using UnityEngine;
using System.Collections.Generic;

public class Battle : UIBase {
	private static UIRoot uiRoot;
	private static Camera mainCamera;
	private UICamera nguiMainCamera;
	private GameObject battleRootGameObject;
	private RaycastHit[] rayCastHit;
	private CardItem tempCard;
	private GameObject tempObject;
	private BattleCardPool battleCardPool;
	private BattleCard battleCard;
	private BattleCardArea battleCardArea;
	private BattleEnemy battleEnemy;
	private CountDownUnity countDownUI;
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

	private AttackEffect attackEffect;

	public Battle(string name):base(name) {
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
	}
	
	public override void CreatUI () {
		CreatEffect ();
		CreatBack();
		CreatCard();
		CreatArea();
		CreatEnemy();
		CreatCountDown ();

		AddSelfObject (battleCardPool);
		AddSelfObject (battleCard);
		AddSelfObject (battleCardArea);
		AddSelfObject (battleEnemy);
	}

	public override void ShowUI() {
		SwitchInput(false);
		base.ShowUI();
		ShowCard();
		if (!isShow) {
			isShow = true;
			GenerateShowCard();
		}
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, EnemyAttckEnd);
		MsgCenter.Instance.AddListener (CommandEnum.ChangeCardColor, ChangeCard);
		MsgCenter.Instance.AddListener (CommandEnum.DelayTime, DelayTime);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
	}

	public override void HideUI () {
		SwitchInput(true);
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, EnemyAttckEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.ChangeCardColor, ChangeCard);
		MsgCenter.Instance.RemoveListener (CommandEnum.DelayTime, DelayTime);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
		battleRootGameObject.SetActive(false);
	}

	void CreatEffect () {
		GameObject go = Resources.Load("Effect/AttackEffect") as GameObject;
		go = NGUITools.AddChild (battleRootGameObject, go);
		go.transform.localPosition = ViewManager.HidePos;
		attackEffect = go.GetComponent<AttackEffect> ();
	}

	void AttackEnemy (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}

		attackEffect.RefreshItem (ai);
	}

	public void StartBattle () {
		ResetClick();
		Attack();
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
			for (int i = 0; i < battleCardPool.CardPosition.Length; i++) {
				battleCard.ChangeSpriteCard(ccc.sourceType,ccc.targetType,i);
			}
		}
	}

	void EnemyAttckEnd (object data) {
		SwitchInput(false);
		MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [0]);
	}

	void BattleEnd (object data) {
		ShieldInput (true);
		SwitchInput(true);
		HideUI ();
	}

	void Attack() {
		MsgCenter.Instance.Invoke (CommandEnum.StartAttack, null);
		ShieldInput (false);
	}

	void CreatCountDown () {
		string name = "CountDown";
		tempObject = GetPrefabsObject (name);
		tempObject.transform.parent = viewManager.CenterPanel.transform;
		countDownUI = tempObject.GetComponent<CountDownUnity> ();
		countDownUI.Init ("CountDown");
	}

	void CreatBack() {
		string backName = "BattleCardPool";
		tempObject = GetPrefabsObject(backName);
		battleCardPool = tempObject.AddComponent<BattleCardPool>();
		battleCardPool.Init(backName);
		BoxCollider bc = NGUITools.AddWidgetCollider(tempObject);
		battleCardPool.XRange = bc.size.x;
		cardHeight = battleCardPool.templateBackTexture.width;
	}

	void CreatCard() {
		tempObject = GetPrefabsObject(Config.battleCardName);
		tempObject.layer = GameLayer.ActorCard;
		battleCard = tempObject.AddComponent<BattleCard>();
		battleCard.CardPosition = battleCardPool.CardPosition;
		battleCard.Init(Config.battleCardName);
	}

	void ShowCard() {
		if(!battleRootGameObject.activeSelf)
			battleRootGameObject.SetActive(true);
	}

	void GenerateShowCard() {
		for (int i = 0; i < battleCardPool.CardPosition.Length; i++) {
			battleCard.GenerateSpriteCard(GenerateCardIndex(),i);
		}
	}

	void CreatArea() {
		string areaName = "BattleCardArea";
		tempObject = GetPrefabsObject(areaName);
		tempObject.layer = GameLayer.BattleCard;
		battleCardArea = tempObject.AddComponent<BattleCardArea>();
		battleCardArea.BQuest = this;
		battleCardArea.Init(areaName);
		battleCardArea.CreatArea(battleCardPool.CardPosition,cardHeight);
	}

	void CreatEnemy() {
		string enemyName = "BattleEnemy";

		tempObject = GetPrefabsObject(enemyName);
		tempObject.layer = GameLayer.EnemyCard;
		battleEnemy = tempObject.AddComponent<BattleEnemy>();
		battleEnemy.battle = this;
		battleEnemy.Init(enemyName);
		battleEnemy.ShowUI ();
	}

	public void ShowEnemy(List<TEnemyInfo> count) {
		battleEnemy.Refresh(count);
		MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [0]);
	}

	GameObject GetPrefabsObject(string name)
	{
		tempObject = LoadAsset.Instance.LoadAssetFromResources(name, ResourceEuum.Prefab) as GameObject;

		GameObject go = NGUITools.AddChild(battleRootGameObject,tempObject);

		go.AddComponent<UIPanel>();

		return go;
	}

	int GenerateData()
	{
		ItemData id = Config.Instance.GetCard();

		allItemData.Add(id);

		return id.itemID;
	}

	int GenerateCardIndex () {
		int index = BattleQuest.questDungeonData.Colors [colorIndex];
		currentColor.Add (index);
		if (currentColor.Count > 5) {
			currentColor.RemoveAt(0);
		}
		colorIndex++;
		return index;
	}


	public void SwitchInput(bool isShield) {
		nguiMainCamera.useMouse = isShield;
		nguiMainCamera.useKeyboard = isShield;
		nguiMainCamera.useTouch = isShield;
		//Debug.LogError ("SwitchInput : " + nguiMainCamera);
		main.GInput.IsCheckInput = !isShield;
	}

	void ShieldInput (bool isShield) {
//		nguiMainCamera.useMouse = isShield;
//		nguiMainCamera.useKeyboard = isShield;
//		nguiMainCamera.useTouch = isShield;
		nguiMainCamera.enabled = isShield;
		main.GInput.IsCheckInput = isShield;

	}

	void HandleOnPressEvent () {
		DisposePress();
	}

	void HandleOnReleaseEvent () {
		DisposeReleasePress();
	}

	void HandleOnStationaryEvent ()
	{
		
	}

	void HandleOnDragEvent (Vector2 obj)
	{
		DisposeOnDrag(obj);
	}

	void ResetClick()
	{
		for (int i = 0; i < selectTarget.Count; i++)
		{
			//selectTarget[i].gameObject.layer = GameLayer.ActorCard;
		
			selectTarget[i].OnPress(false,-1);			
		}

		selectTarget.Clear();

		battleCard.ResetDrag();
	}
	

	void DisposeReleasePress() {
		//IgnoreLayer(false);
//		Debug.LogError ("battle : DisposeReleasePress : " + selectTarget.Count);
		if(selectTarget.Count == 0) {
			ResetClick();
			return;
		}

		if(Check(GameLayer.BattleCard)) {
//			Debug.LogError("battle : Check(GameLayer.BattleCard)");
			BattleCardAreaItem bcai = null;

			for (int i = 0; i < rayCastHit.Length; i++) 
			{
				tempObject = rayCastHit[i].collider.gameObject;
				bcai = tempObject.GetComponent<BattleCardAreaItem>();

				if(bcai != null)
					break;
			}
			int generateCount = 0;
			if(bcai != null)
				generateCount = bcai.GenerateCard(selectTarget);

			if(generateCount > 0)
			{
				MsgCenter.Instance.Invoke(CommandEnum.StateInfo,"");
				YieldStartBattle();
				if(showCountDown) {
					for(int i = 0;i < generateCount;i++) {
//						battleCard.GenerateCard(GenerateData(),selectTarget[i].location);
						battleCard.GenerateSpriteCard(GenerateCardIndex(),selectTarget[i].location);
					}
				}
			}
			ResetClick();
		}
		else if(Check(GameLayer.ActorCard)) {
			Vector3 point = selectTarget[0].transform.localPosition;
			int indexID =  battleCardPool.CaculateSortIndex(point);
			if(indexID >= 0) {
				main.GInput.IsCheckInput = false;
				if(battleCard.SortCard(indexID,selectTarget)) {
					battleCard.CallBack += HandleCallBack;
				}
				else {
					main.GInput.IsCheckInput = true; 
					ResetClick();
				}
			}
			else {
				ResetClick();
			}
		}
		else {
			ResetClick();
		}
	}

	void HandleCallBack ()
	{
		main.GInput.IsCheckInput = true;
		ResetClick();
	}

	void TweenCallback(GameObject go) {
		main.GInput.IsCheckInput = true;
	}
	
	private BattleCardAreaItem prevTempBCA;

	void DisposeOnDrag(Vector2 obj) {
		SetDrag();
		Vector3 vec = ChangeCameraPosition(Input.mousePosition) - viewManager.ParentPanel.transform.localPosition;

		for (int i = 0; i < selectTarget.Count; i++) {
			selectTarget[i].OnDrag(vec,i);
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
			battleCard.DisposeDrag(selectTarget[0].location,selectTarget[0].itemID);
	}

	void IgnoreLayer(bool isPress) {
		battleCard.IgnoreCollider(isPress);
		battleCardPool.IgnoreCollider(isPress);
	}

	void ClickObject(GameObject go)
	{
		tempCard = go.GetComponent<CardItem>();
		if(tempCard != null)
		{
			if(selectTarget.Contains(tempCard))
				return;
			if(tempCard.CanDrag)
			{
				tempCard.OnPress(true,selectTarget.Count);
				tempCard.ActorTexture.depth = 5;
				//tempCard.gameObject.layer =  GameLayer.IgnoreCard;
				//tempCard.transform.parent = dragLayer
				selectTarget.Add(tempCard);
//				if(selectTarget.Count > 1) {
//					for (int i = 1; i < selectTarget.Count; i++) {
//						Vector3 pos = selectTarget[i-1].transform.localPosition;
//						Vector3 newPos = new Vector3(pos.x + 62,pos.y - 62,pos.z);
//						Debug.LogError("newPos : " + newPos);
//						selectTarget[i].transform.localPosition = newPos;
//					}
//				}
			}
		}
	}

	bool Check(LayerMask mask)
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		
		rayCastHit = Physics.RaycastAll(ray,100f, GameLayer.LayerToInt(mask));

		if(rayCastHit.Length > 0)
			return true;
		else
			return false;
	}

	public static Vector3 ChangeCameraPosition()
	{
		Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

		float height = (float)Screen.height / 2;
		
		Vector3 reallyPoint = worldPoint * height * uiRoot.pixelSizeAdjustment;

		return reallyPoint;
	}

	public static Vector3 ChangeCameraPosition(Vector3 pos)
	{
		Vector3 worldPoint = mainCamera.ScreenToWorldPoint(pos);
		
		float height = (float)Screen.height / 2;
		
		Vector3 reallyPoint = worldPoint * height * uiRoot.pixelSizeAdjustment;
		
		return reallyPoint;
	}

	public static Vector3 ChangeDeltaPosition(Vector3 delta)
	{
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
		time = BattleUseData.CountDown + activeDelay;
		countDownUI.ShowUI();
		activeDelay = 0f;
		CountDownBattle ();
	}
	
	void CountDownBattle () {
		//battleCardArea.ShowCountDown (true, (int)time);
		countDownUI.SetCurrentTime ((int)time);
		if (time > 0) {
			showCountDown = true;
			time -= countDownTime;
			GameTimer.GetInstance ().AddCountDown (countDownTime, CountDownBattle);
		} 
		else {
			battleCardArea.ShowCountDown (false, (int)time);
			showCountDown = false;
			StartBattle();
			time =  BattleUseData.CountDown;
		}
	}
}
