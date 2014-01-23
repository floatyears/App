using UnityEngine;
using System.Collections.Generic;

public class Battle : UIBase
{
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

	private float ZOffset = -100f;
	
	private List<ItemData> allItemData = new List<ItemData>();
	private List<CardItem> selectTarget = new List<CardItem>();

	public int cardHeight = 0;

	public Battle(string name):base(name)
	{
		uiRoot = ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>();
		nguiMainCamera = ViewManager.Instance.MainUICamera;
		mainCamera = nguiMainCamera.camera;

		battleRootGameObject = NGUITools.AddChild(ViewManager.Instance.ParentPanel);
		battleRootGameObject.name = "Fight";
		Vector3 pos = battleRootGameObject.transform.localPosition;
		battleRootGameObject.transform.localPosition = new Vector3(pos.x,pos.y,pos.z + ZOffset);
		
		GameInput.OnPressEvent += HandleOnPressEvent;
		GameInput.OnReleaseEvent += HandleOnReleaseEvent;
		GameInput.OnStationaryEvent += HandleOnStationaryEvent;
		GameInput.OnDragEvent += HandleOnDragEvent;
	}
	
	public override void CreatUI ()
	{
		CreatBack();

		CreatCard();

		CreatArea();

		CreatEnemy();

		AddSelfObject (battleCardPool);
		AddSelfObject (battleCard);
		AddSelfObject (battleCardArea);
		AddSelfObject (battleEnemy);
	}

	public override void ShowUI()
	{
		SwitchInput(false);
		base.ShowUI();
		ShowCard();
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.EnemyAttackEnd, AttckEnd);
	}

	public override void HideUI ()
	{
		SwitchInput(true);
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnemyAttackEnd, AttckEnd);

		battleRootGameObject.SetActive(false);
	}

	public void StartBattle ()
	{
		ResetClick();

		Attack();

	}

	void AttckEnd (object data) {
		SwitchInput(false);
	}

	void BattleEnd (object data) {
		ShieldInput (true);
		SwitchInput(true);
		HideUI ();
	}

	void Attack() {
		MsgCenter.Instance.Invoke (CommandEnum.StartAttack, null);
		//SwitchInput(true);
		ShieldInput (false);
	}

	void CreatBack()
	{
		string backName = "BattleCardPool";

		tempObject = GetPrefabsObject(backName);

		battleCardPool = tempObject.AddComponent<BattleCardPool>();
		
		battleCardPool.Init(backName);
		
		BoxCollider bc = NGUITools.AddWidgetCollider(tempObject);

		battleCardPool.XRange = bc.size.x;

		cardHeight = battleCardPool.templateBackTexture.width;
	}

	void CreatCard()
	{
		tempObject = GetPrefabsObject(Config.battleCardName);

		tempObject.layer = GameLayer.ActorCard;

		battleCard = tempObject.AddComponent<BattleCard>();

		battleCard.CardPosition = battleCardPool.CardPosition;

		battleCard.Init(Config.battleCardName);

	}

	void ShowCard()
	{
		battleRootGameObject.SetActive(true);

		for (int i = 0; i < battleCardPool.CardPosition.Length; i++)
		{
			battleCard.GenerateCard(GenerateData(),i);
		}
	}

	void CreatArea()
	{
		string areaName = "BattleCardArea";

		tempObject = GetPrefabsObject(areaName);

		tempObject.layer = GameLayer.BattleCard;

		battleCardArea = tempObject.AddComponent<BattleCardArea>();
		battleCardArea.BQuest = this;
		battleCardArea.Init(areaName);

		battleCardArea.CreatArea(battleCardPool.CardPosition,cardHeight);
	}

	void CreatEnemy()
	{
		string enemyName = "BattleEnemy";

		tempObject = GetPrefabsObject(enemyName);

		tempObject.layer = GameLayer.EnemyCard;

		battleEnemy = tempObject.AddComponent<BattleEnemy>();
		battleEnemy.battle = this;
		battleEnemy.Init(enemyName);
		battleEnemy.ShowUI ();
	}

	public void ShowEnemy(List<ShowEnemyUtility> count)
	{
		battleEnemy.Refresh(count);
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


	void SwitchInput(bool isShield)
	{
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

	void HandleOnPressEvent ()
	{
		DisposePress();
	}

	void HandleOnReleaseEvent ()
	{
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
	

	void DisposeReleasePress()
	{
		//IgnoreLayer(false);
		if(selectTarget.Count == 0)
		{
			ResetClick();

			return;
		}

		if(Check(GameLayer.BattleCard))
		{
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
				YieldStartBattle();
				if(showCountDown) {
					for(int i = 0;i < generateCount;i++) {
						battleCard.GenerateCard(GenerateData(),selectTarget[i].location);
					}
				}
			}
			ResetClick();
		}
		else if(Check(GameLayer.ActorCard)) {
			Vector3 point = selectTarget[0].transform.localPosition;

			int indexID =  battleCardPool.CaculateSortIndex(point);

			if(indexID >= 0)
			{
				main.GInput.IsCheckInput = false;
				if(battleCard.SortCard(indexID,selectTarget))
				{
					battleCard.CallBack += HandleCallBack;
				}
				else
				{
					main.GInput.IsCheckInput = true;

					ResetClick();
				}
			}

//			Debug.LogError(" check ActorCard card  indexID " + indexID);
		}
		else {
//			Debug.LogError(" check nothing  ");
			ResetClick();
		}
	}

	void HandleCallBack ()
	{
		main.GInput.IsCheckInput = true;
		ResetClick();
	}

	void TweenCallback(GameObject go)
	{
		main.GInput.IsCheckInput = true;
	}


	private BattleCardAreaItem prevTempBCA;

	void DisposeOnDrag(Vector2 obj)
	{
		SetDrag();
		Vector3 vec = ChangeCameraPosition(Input.mousePosition) - viewManager.ParentPanel.transform.localPosition;

		for (int i = 0; i < selectTarget.Count; i++) 
		{
			selectTarget[i].OnDrag(vec,i);
		}
		bool b = Check(GameLayer.ActorCard);

		if(b)
		{
			for (int i = 0; i < rayCastHit.Length; i++)
			{
				tempObject = rayCastHit[i].collider.gameObject;
				
				ClickObject(tempObject);
			}
		}
	}

	void DisposePress()
	{
		//IgnoreLayer(true);

		if(Check(GameLayer.ActorCard))
		{
			for (int i = 0; i < rayCastHit.Length; i++)
			{
				if(rayCastHit[i].collider.gameObject.layer == GameLayer.ActorCard)
				{
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

	void SetDrag()
	{
		if(selectTarget.Count > 0)
			battleCard.DisposeDrag(selectTarget[0].location,selectTarget[0].itemID);
	}

	void IgnoreLayer(bool isPress)
	{
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

	#region countdown
	
	public bool showCountDown = false;
	float time = 5f;
	float countDownTime = 1f;
	
	public void YieldStartBattle () {
		if (showCountDown) {
			return ;		
		} 
		
		CountDownBattle ();
	}
	
	void CountDownBattle () {
		battleCardArea.ShowCountDown (true, (int)time);
		if (time > 1) {
			showCountDown = true;
			time -= countDownTime;
			GameTimer.GetInstance ().AddCountDown (countDownTime, CountDownBattle);
		} 
		else {
			battleCardArea.ShowCountDown (false, (int)time);
			showCountDown = false;
			StartBattle();
			time = 5f;
		}
	}
	
	
//	void StartBattle() {
//		StartBattle();
//	}
	

	#endregion
}
