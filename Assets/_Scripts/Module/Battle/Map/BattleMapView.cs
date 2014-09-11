using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleMapView : ViewBase {

	public const string trapSpriteName = "icon_trap"; //private const string trapName = "";
	public const string chestSpriteName = "icon_chest";

	private UITexture mapBGTexture;

	private MapItem template;
	private MapItem[,] map;
//	private MapItem temp;

	private List<MapItem> prevAround = new List<MapItem>();
	[HideInInspector]
	public MapItem prevMapItem;
	private static bool wMove = false;
	public const float itemWidth = 114f;

	private GameObject door;

	[HideInInspector]
	public static bool waitMove {
		set{ wMove = value; }
		get{ return wMove; }
	}

	////--------------cell info
	public static float scaleTime = 0.5f;
	public static float showTime = 0.5f;
	private UILabel typeLabel;
	private UILabel nameLabel;
	private UILabel cateGoryLabel;
	private UISprite itemSprite;

	private UILabel nameTitleLabel;
	private UILabel categoryTitleLabel;

	private const string categoryTitle = "Category: ";
	private const string coinTitle = "Number: ";	

	private GameObject role;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);

		mapBGTexture = FindChild<UITexture>("BG");

		string path = "Texture/Map/battlemap_" + ConfigBattleUseData.Instance.GetMapID().ToString ();
		//		Debug.LogError ("path : " + path);
		mapBGTexture.mainTexture = ResourceManager.Instance.LoadLocalAsset (path, null) as Texture2D;

		map = new MapItem[MapConfig.MapWidth, MapConfig.MapHeight];
		template = FindChild<MapItem>("SingleMap");
//		template.Init("SingleMap");
		door = FindChild("Door");
		template.gameObject.SetActive(false);
//		gameObject.transform.localPosition += Vector3.right * 5f;
		wMove = false;

		//----------init cell info
		nameTitleLabel = FindChild<UILabel> ("CellInfo/NameTitleLabel");//transform.Find("NameTitleLabel").GetComponent<UILabel>();
		categoryTitleLabel = FindChild<UILabel> ("CellInfo/CategoryTitleLabel");
		typeLabel = FindChild<UILabel> ( "CellInfo/TypeLabel");
		nameLabel = FindChild<UILabel> ("CellInfo/NameLabel");
		cateGoryLabel = FindChild<UILabel> ( "CellInfo/CatagoryLabel");
		itemSprite = FindChild<UISprite> ( "CellInfo/Trap");

		gameObject.layer = GameLayer.BottomInfo;

		topLabel = FindChild<UILabel> ("Door/Top");
		bottomLabel = FindChild<UILabel> ("Door/Bottom");
		arrowSprite = FindChild<UISprite> ("Door/Sprite");
		topAlpha = topLabel.GetComponent<TweenAlpha>();
		bottomAlpha = bottomLabel.GetComponent<TweenAlpha>();
		arrowAlpha = arrowSprite.GetComponent<TweenAlpha>();
		
		UIEventListener.Get (gameObject).onClick = ClickDoor;

		role = FindChild ("Role");
	}

	
	public override void ShowUI () {
		base.ShowUI ();
		//		door.ShowUI ();
		StartMap ();
		MsgCenter.Instance.AddListener (CommandEnum.ShieldMap, ShieldMap);
		
		doorOpen = ConfigBattleUseData.Instance.storeBattleData.HitKey;
		MsgCenter.Instance.AddListener (CommandEnum.OpenDoor, OpenDoor);
		MsgCenter.Instance.AddListener (CommandEnum.QuestEnd, QuestEnd);
		
		prevCoor = currentCoor = ConfigBattleUseData.Instance.roleInitCoordinate;
		//		bQuest.currentCoor = prevCoor;
		//		TargetPoint = bQuest.GetPosition(currentCoor);
		//		jump.Init (GetInitPosition());
		//		jump.GameStart (targetPoint);	
		SyncRoleCoordinate(currentCoor);
		Stop();

		MsgCenter.Instance.AddListener (CommandEnum.TrapMove, TrapMove);
		MsgCenter.Instance.AddListener (CommandEnum.NoSPMove, NoSPMove);
		
	}

	public override void HideUI () {
		base.HideUI ();
		prevAround.Clear ();
//		gameObject.SetActive (false);
//		for (int i = 0; i < map.GetLength(0); i++) {
//			for (int j = 0; j < map.GetLength(1); j++) {
//				Destroy(map[i,j].gameObject);
//				map[i,j] = null;
//			}
//		}
		
		//		door.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.ShieldMap, ShieldMap);
		
		MsgCenter.Instance.RemoveListener (CommandEnum.OpenDoor, OpenDoor);
		MsgCenter.Instance.RemoveListener (CommandEnum.QuestEnd, QuestEnd);
		doorOpen = false;
		canEnterDoor = false;
		checkOut = false;
		
		MsgCenter.Instance.RemoveListener (CommandEnum.TrapMove, TrapMove);
		MsgCenter.Instance.RemoveListener (CommandEnum.NoSPMove, NoSPMove);
	}

	void StartMap() {
		int x = map.GetLength(0);
		int y = map.GetLength(1);
		GameObject tempObject = null;
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				if(map[i,j] == null) {
					tempObject = NGUITools.AddChild(gameObject, template.gameObject);
					tempObject.SetActive(true);
					float xp = template.InitPosition.x + i * itemWidth;
					float yp = template.InitPosition.y + j * itemWidth;
					tempObject.transform.localPosition = new Vector3(xp,yp,template.InitPosition.z);
					MapItem temp = tempObject.GetComponent<MapItem>();
					temp.Coor = new Coordinate(i, j);
					temp.Init(i+"|"+j);
//					temp.battleMap = this;
					UIEventListener.Get(tempObject).onClick = OnClickMapItem;
					map[i,j] = temp;
				} else {
					map[i,j].ShowUI();
				}
			}
		}
		float xCoor =  template.InitPosition.x + (x / 2) * itemWidth;
		float yCoor = template.InitPosition.y + y * itemWidth;
//		SetPosition (new Vector3 (xCoor, yCoor, door.transform.localPosition.z));
		List<TClearQuestParam> _data = ConfigBattleUseData.Instance.storeBattleData.questData;
		RefreshMap (_data[_data.Count - 1]);
	}


	public void RefreshMap(TClearQuestParam cqp) {
		for (int i = 0; i < cqp.hitGrid.Count; i++) {
			Coordinate coor = ConfigBattleUseData.Instance.questDungeonData.GetGridCoordinate(cqp.hitGrid[i]);
			if(coor.x < 0 || coor.y < 0 || coor.x >= MapConfig.MapWidth || coor.y >= MapConfig.MapHeight) {
				continue;
			}
			map[coor.x, coor.y].HideGridNoAnim();
		}

		Coordinate roleCoor = ConfigBattleUseData.Instance.storeBattleData.roleCoordinate;

		if(roleCoor.x != MapConfig.characterInitCoorX || roleCoor.y != MapConfig.characterInitCoorY) {
			map[MapConfig.characterInitCoorX,MapConfig.characterInitCoorY].HideGridNoAnim();
		}
	}

	public MapItem GetMapItem(Coordinate coor) {
		return map [coor.x, coor.y];
	}
	
	void ShieldMap(object data) {
		int b = (int)data;

		for (int i = 0; i < map.GetLength(0); i++) {
			for (int j = 0; j < map.GetLength(1); j++) {
				map[i,j].HideEnvirment(b > 0);
			}
		}
	}
	  
	void OnClickMapItem(GameObject go) {
		if (!wMove) {
			StartMove(go.GetComponent<MapItem>().Coor);// bQuest.TargetItem(temp.Coor);
		}
	}

	public Vector3 GetPosition(int x, int y) {
		if(x > map.GetLength(0) || y > map.GetLength(1))
			return transform.localPosition;
		Vector3 pos = map[x, y].transform.localPosition + transform.localPosition;

		return pos;
	}

	public bool ReachMapItem(Coordinate coor) {
		prevMapItem = map[coor.x,coor.y];
		return prevMapItem.IsOld;
	}
	private Callback callback = null;
	
	public void RotateAnim(Callback cb) {
		prevMapItem.RotateSingle (cb);
	}

	public void RotateAll(Callback cb) {
		prevMapItem.RotateAll (cb, false);
	}

	public EnemyAttackEnum FirstOrBackAttack() {
		return prevMapItem.TriggerAttack ();
	}

	private Callback cb;
	public void BattleEndRotate (Callback callback) {
		cb = callback;
		bool allShow = cb == null ? true : false;
		StartCoroutine (EndRotate (allShow));
	}

	IEnumerator EndRotate (bool allShow) {
		for (int i = 0; i < map.GetLength(0); i++) {
			for (int j = 0; j < map.GetLength(1); j++) {
				if(i == map.GetLength(0) - 1 && j == map.GetLength(1) - 1){
					map[i,j].RotateAll(cb,allShow);
				} else {
					map[i,j].RotateAll(null,allShow);
				}
				yield return 10;
			}
		}
	}

	void RotateDown(object data) { }

	public Queue<MapItem> AttakAround(Coordinate coor) {
		List<MapItem> temp = GetAround (coor);
		Queue<MapItem> mapItem = new Queue<MapItem> ();
		foreach (var item in temp) {
			if(item.GetChainLinke()) {
				item.isLockAttack = true;
				mapItem.Enqueue(item);
			}
		}
		return mapItem;
	}

	public void AddMapSecuritylevel(Coordinate coor) {
		List<MapItem> temp = GetAround (coor);
		foreach (var item in temp) {
			item.AddSecurityLevel();
		}
	}

	List<MapItem> GetAround(Coordinate coor) {
		List<MapItem> aroundList = new List<MapItem> ();
		if(coor.x < map.GetLength(0) - 1)				//right map grid 
			aroundList.Add(map[coor.x + 1, coor.y]);
		
		if(coor.x > 0)									//left map grid
			aroundList.Add(map[coor.x - 1, coor.y]);
		
		if(coor.y > 0)									//bottom map grid
			aroundList.Add(map[coor.x, coor.y - 1]);
		
		if(coor.y < map.GetLength(1) - 1)				//top map grid
			aroundList.Add(map[coor.x, coor.y + 1]);

		return aroundList;
	}

	public void ChangeStyle(Coordinate coor) {
		if(prevAround.Count > 0) {
			for (int i = 0; i < prevAround.Count; i++) {
				prevAround[i].Around(false);
			}

			prevAround.Clear();
		}

		//map grid Priority : right > left > bottom > top
		if(coor.x < map.GetLength(0) - 1)				//right map grid 
			DisposeAround(map[coor.x + 1, coor.y]);

		if(coor.x > 0)									//left map grid
			DisposeAround(map[coor.x - 1, coor.y]);

		if(coor.y > 0)									//bottom map grid
			DisposeAround(map[coor.x, coor.y - 1]);

		if(coor.y < map.GetLength(1) - 1)				//top map grid
			DisposeAround(map[coor.x, coor.y + 1]);
	}

	void DisposeAround(MapItem item) {
		item.Around(true);
		prevAround.Add(item);
	}

	public void ShowTrap(TrapBase tb) {
		if (tb == null) {
			return;	
		}

		if (!nameTitleLabel.enabled) {
			nameTitleLabel.enabled = true;		
		}

		categoryTitleLabel.text = categoryTitle;
		typeLabel.text = "Trap";
		itemSprite.spriteName = BattleMapView.trapSpriteName; //tb.GetTrapSpriteName ();
		nameLabel.text = tb.GetItemName ();
		cateGoryLabel.text = tb.GetTypeName () + " : Lv." + tb.GetLevel;
		iTween.ScaleTo(gameObject,iTween.Hash("y", 1f, "time", scaleTime,"oncompletetarget",gameObject,"oncomplete","ShowEnd"));
	}

	public void ShowCoin(int number) {
		nameLabel.text = "";
		typeLabel.text = "Coin";
		itemSprite.spriteName = BattleMapView.chestSpriteName; // S  is coin sprite name in atlas.
		cateGoryLabel.text = number.ToString ();
		categoryTitleLabel.text = coinTitle;
		if (nameTitleLabel.enabled) {
			nameTitleLabel.enabled = false;
		}

		iTween.ScaleTo(gameObject,iTween.Hash("y", 1f, "time", scaleTime,"oncompletetarget",gameObject,"oncomplete","ShowEnd"));
	}

	void ShowEnd() {
		GameTimer.GetInstance ().AddCountDown (showTime, ()=>{
			nameLabel.text = "";
			cateGoryLabel.text = "";
			transform.localScale = new Vector3 (1f, 0f, 1f);
		});
	}


	//------door
	
	private UILabel topLabel;
	private UILabel bottomLabel;
	private UISprite arrowSprite;
	
	private TweenAlpha topAlpha;
	private TweenAlpha bottomAlpha;
	private TweenAlpha arrowAlpha;
	
	private bool _doorOpen = false;
	
	[HideInInspector]
	public bool doorOpen {
		get {return _doorOpen; }
		set { _doorOpen = value;  ShowTapToBattle(); }
	}
	
	[HideInInspector]
	public bool canEnterDoor = false;
	private bool checkOut = false;
	
	private string currentShowInfo;
	//
	//	public override void CreatUI () {
	//		base.CreatUI ();
	//	}
	
	void OnEnable() {
		if(topAlpha != null)
			topAlpha.ResetToBeginning ();
		if(bottomAlpha != null)
			bottomAlpha.ResetToBeginning ();
		if(arrowAlpha != null)
			arrowAlpha.ResetToBeginning ();
	}
	
	void SetName(string name) {
		currentShowInfo = name;
		
		string[] info = currentShowInfo.Split('|');
		topLabel.text = info [0];
		bottomLabel.text = info [1];
	}
	
	public void SetPosition(Vector3 pos) {
		transform.localPosition = pos;
	}
	
	void OpenDoor (object data) {
		doorOpen = true;
	}
	
	void QuestEnd(object data) {
		//		Debug.LogError ("QuestEnd : " + data);
		canEnterDoor = (bool)data;
	}
	
	public void ShowTapToBattle () {
		topLabel.enabled = doorOpen;	
		bottomLabel.enabled = doorOpen;
		topAlpha.enabled = doorOpen;
		bottomAlpha.enabled = doorOpen;
		arrowSprite.enabled = doorOpen;
		arrowAlpha.enabled = doorOpen;
		SetName (BattleFullScreenTipsView.BossBattle);
	}
	
	void ClickDoor(GameObject go) {
		//		Debug.LogError ("topLabel.enabled : " + topLabel.enabled + " content equal : " + (currentShowInfo == QuestFullScreenTips.BossBattle) + " canEnterDoor : " + canEnterDoor);
		if (!topLabel.enabled) {
			return;	
		}
		
		if (currentShowInfo == BattleFullScreenTipsView.BossBattle && topLabel.enabled) {
			if(!canEnterDoor) {
				return;
			}
//			battleMap.bQuest.ClickDoor();
			topLabel.enabled = false;
			bottomLabel.enabled = false;
			topAlpha.enabled = false;
			bottomAlpha.enabled = false;
			arrowSprite.enabled = false;
			arrowAlpha.enabled = false;
			return;
		}
		
		if (currentShowInfo == BattleFullScreenTipsView.CheckOut && checkOut) {
			checkOut = false;
			topLabel.enabled = checkOut;	
			topAlpha.enabled = checkOut;
			topLabel.enabled = checkOut;	
			topAlpha.enabled = checkOut;
//			battleMap.bQuest.CheckOut();
		}
	}
	
	public void ShowTapToCheckOut () {
		topLabel.enabled = true;
		topAlpha.enabled = true;
		bottomLabel.enabled = true;
		bottomAlpha.enabled = true;
		arrowSprite.enabled = true;
		arrowAlpha.enabled = true;
		
		SetName (BattleFullScreenTipsView.CheckOut);
		checkOut = true;
	}



	private Coordinate currentCoor;
	public Coordinate CurrentCoor {
		get{ return currentCoor; }
	}
	
	private Coordinate prevCoor;
	public Coordinate PrevCoor {
		get { return prevCoor; }
	}
	[HideInInspector]
	public bool isMove = false;
	private Vector3 targetPoint;
	private const int xOffset = -5;
	private const int YOffset = 20;
	private const int ZOffset = -40;
	private Vector3 scale = new Vector3(30f, 25f, 30f);
	private Vector3 angle = new Vector3(330f, 0f, 0f);
	private List<Coordinate> firstWay = new List<Coordinate>();
	private Vector3 distance = Vector3.zero;
	//	private Vector3 parentPosition = Vector3.zero;
	
	public Vector3 TargetPoint {
		set {
			//			Vector3 pos = value + parentPosition;
			targetPoint.x = value.x + xOffset;
			targetPoint.y = value.y + YOffset;
			targetPoint.z = transform.localPosition.z;
		}
	}
	
	//	private BattleQuest bQuest;
	//	public BattleQuest BQuest {
	//		set{ bQuest = value; }
	//	}
	
	//	private Jump jump;
	private Vector3 initPosition = new Vector3 (-1115f, 340f, -20f);
	
	Vector3 GetInitPosition() {
		return new Vector3 (targetPoint.x, targetPoint.y, targetPoint.z - 100f);
	}
	
	Vector3 GetRolePosition(Vector3 pos) {
		Vector3 reallyPosition = new Vector3 (pos.x + 7f, pos.y + 30f, pos.z - 50f);
		return reallyPosition;
	}
	
	void NoSPMove(object data) {
		//		bQuest.battle.ShieldInput(false);
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick", false);
		
		Coordinate cd;
		if (data == null) {
			cd = prevCoor;
		} else {
			cd = (Coordinate)data;	
		}
		
		SetTarget (cd);
		//		bQuest.RoleCoordinate(cd);
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "rolecoor", cd);
		StartCoroutine (MoveByTrap ());
	}
	
	void TrapMove(object data) {
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick", true);
		if (data == null) {
			SetTarget (currentCoor);
			StartCoroutine(MoveByTrap());
			return;
		}
		Coordinate cd = (Coordinate)data;
		SetTarget (cd);
		MsgCenter.Instance.Invoke(CommandEnum.TrapTargetPoint, cd);
		//		bQuest.RoleCoordinate(cd);
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "rolecoor", cd);
		GoTarget ();
	}
	
	void GoTarget() {
		//		bQuest.battleMap.ChangeStyle (currentCoor);
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "style", currentCoor);
		transform.localPosition = targetPoint;
		//		bQuest.battle.ShieldInput (true);
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick", true);
	}
	
	IEnumerator MoveByTrap() {
		while (true) {
			//			jump.JumpAnim ();
			Stop ();	
			transform.localPosition = Vector3.Lerp(transform.localPosition,targetPoint,Time.deltaTime * 20);
			distance = transform.localPosition - targetPoint;
			yield return Time.deltaTime;
			if (distance.magnitude < 1f) {
				//				bQuest.battle.ShieldInput(true);
				ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick", true);
				//				bQuest.battleMap.ChangeStyle (currentCoor);
				ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "style", currentCoor);
				yield break;
				
			}
		}
	}
	
	void Move() {
		if(firstWay.Count == 0) {
			return;
		}
		
		isMove = true;
		SetTarget(firstWay[0]);
		
		MoveRole ();
	}
	
	void QuestCoorEnd(Coordinate coor) {
		//		bQuest.currentCoor = coor;
	}
	
	void SetTarget(Coordinate tc) {
		QuestCoorEnd (tc);
		prevCoor = currentCoor;
		currentCoor = tc;
		//		TargetPoint = bQuest.GetPosition(tc);
		////		Debug.LogError ("TargetPoint : " + TargetPoint);
		//		if (isMove) {
		//			jump.JumpAnim ();
		//		}
	}

	Vector3[] secondPath = new Vector3[4];
	//-15 -659 -100
	//	float y =  100f;
	//	float time = 0.5f;
	Vector3 middlePoint = Vector3.zero;
	
	bool isVerticalMove = false;
	float adjustTime = 0.0f;
	
	void MoveRole() {
		Vector3 localposition = transform.localPosition;
		Vector3 leftMiddlePoint = Vector3.zero;
		Vector3 leftMiddlePoint2 = Vector3.zero;
		Vector3 rightMiddlePoint = Vector3.zero;
		Vector3 rightMiddlePoint2 = Vector3.zero;
		Vector3 rightFristMiddlePoint = Vector3.zero;
		
		if (Mathf.FloorToInt(localposition.x) == Mathf.FloorToInt(targetPoint.x) ) {
			float offsetY = 1.2f;
			if ( targetPoint.y < localposition.y ) {
				offsetY = 0.6f;					//(float)BattleMap.itemWidth * 0.4f;
			}
			
			middlePoint = new Vector3 (localposition.x, localposition.y + BattleMapView.itemWidth * 1.4f * offsetY, localposition.z);
			leftMiddlePoint = localposition; 	//new Vector3(middlePoint.x, middlePoint.y * 0.9f, middlePoint.z);
			leftMiddlePoint2 = localposition; 	//new Vector3(middlePoint.x, middlePoint.y * 0.9f, middlePoint.z);
			rightMiddlePoint = targetPoint;
			rightMiddlePoint2 = targetPoint;
			
			isVerticalMove = true;
			adjustTime = 0.05f;
		} else {
			float x = targetPoint.x - localposition.x;
			leftMiddlePoint = new Vector3 (localposition.x + x * 0.1f , localposition.y + BattleMapView.itemWidth* 1.2f * 0.5f, localposition.z);
			leftMiddlePoint2 = new Vector3 (localposition.x + x * 0.3f , localposition.y + BattleMapView.itemWidth* 1.2f * 0.8f, localposition.z);
			middlePoint = new Vector3 (localposition.x + x * 0.4f , localposition.y + BattleMapView.itemWidth * 1.2f, localposition.z);
			rightMiddlePoint = new Vector3(localposition.x + x * 0.6f,localposition.y + BattleMapView.itemWidth * 1.0f , localposition.z);
			rightMiddlePoint2 = new Vector3(localposition.x + x * 0.8f,localposition.y + BattleMapView.itemWidth * 0.65f , localposition.z);
			
			isVerticalMove = false;
			adjustTime = -0.02f;
		}
		
		Vector3[] path = new Vector3[3];
		path [0] = transform.localPosition;
		path [1] = leftMiddlePoint;
		path [2] = middlePoint;
		
		secondPath [0] = middlePoint;
		secondPath [1] = rightMiddlePoint;
		secondPath [2] = rightMiddlePoint2;
		secondPath [3] = targetPoint;
		
		AudioManager.Instance.PlayAudio (AudioEnum.sound_walk);
		iTween.MoveTo (role, iTween.Hash ("path", path, "movetopath", false, "islocal", true, "time", 0.13f+adjustTime, "easetype", iTween.EaseType.easeOutSine, "oncomplete", "MoveRoleSecond", "oncompletetarget", gameObject));
	}
	
	void MoveRoleSecond() {
		//		Debug.LogError ("MoveRoleSecond");
		AudioManager.Instance.PlayAudio (AudioEnum.sound_chess_fall);
		//		iTween.MoveTo (gameObject, iTween.Hash ("position", targetPoint, "islocal", true, "time", 0.35f, "easetype", iTween.EaseType.easeInCubic, "oncomplete", "MoveEnd", "oncompletetarget", gameObject));
		iTween.MoveTo (role, iTween.Hash ("path", secondPath, "movetopath", false, "islocal", true, "time", 0.18+adjustTime, "easetype", iTween.EaseType.easeInSine, "oncomplete", "MoveRoleBounce", "oncompletetarget", gameObject));
	}
	
	void MoveRoleBounce() {
		float bounceOffset = 0.05f*BattleMapView.itemWidth;
		Vector3[] bouncePath = new Vector3[3];
		float moveX = bounceOffset, moveY = bounceOffset;
		if ( isVerticalMove ) {
			moveX = 0;
			moveY = 0;
		} else {
			moveY = 0;
		}
		bouncePath[0] = new Vector3(secondPath [3].x+moveX, secondPath [3].y, secondPath [3].y + moveY);
		bouncePath[1] = new Vector3(secondPath [3].x+moveX, secondPath [3].y, secondPath [3].y + bounceOffset);
		bouncePath[2] = new Vector3(secondPath [3].x, secondPath [3].y, secondPath [3].y);
		//		Debug.LogError(">>>>>>>>>>> bounce...");
		iTween.MoveTo (role, iTween.Hash ("path", bouncePath, "movetopath", false, "islocal", true, "time", 0.1f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "MoveEnd", "oncompletetarget", gameObject));
	}
	
	Coordinate tempCoor; 
	
	void MoveEnd() {
		if(!isMove) {
			firstWay.Clear();
		}
		else {
			tempCoor = firstWay[0];
			firstWay.RemoveAt(0);
			//			bQuest.battleMap.ChangeStyle (tempCoor);
			ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "style", tempCoor);
			SyncRoleCoordinate(tempCoor);
			if(firstWay.Count > 0) {
				GameTimer.GetInstance().AddCountDown(0.1f, Move);
			}
			else
				isMove = false;
		}
	}
	
	public void Stop() {
		isMove = false;
		firstWay.Clear ();
	}
	
	public void StartMove(Coordinate coor) {
		if (isMove)
			return;
		//		Debug.LogError ("Coordinate : " + coor.x + " y : " + coor.y);
		GenerateWayPoint(coor);
		Move();
	}
	
	public void SyncRoleCoordinate(Coordinate coor) {
		MsgCenter.Instance.Invoke (CommandEnum.MoveToMapItem, coor);
		//		if (!bQuest.ChainLinkBattle) {
		//			MsgCenter.Instance.Invoke(CommandEnum.ReduceActiveSkillRound);	
		//		}
		
		RoleCoordinate(coor);
		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "rolecoor", coor);
	}

	void RoleCoordinate(Coordinate coor) {
		
		//		BattleMapView v = view as BattleMapView;
		//		Debug.LogError ("coor : " + coor.x + " coor : " + coor.y);
		if (!ReachMapItem (coor)) {
			if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				prevMapItem.HideGridNoAnim ();
				GameTimer.GetInstance ().AddCountDown (0.2f, YieldShowAnim);
				ConfigBattleUseData.Instance.StoreMapData();
				return;
			}
			
			int index = ConfigBattleUseData.Instance.questDungeonData.GetGridIndex (coor);
			
			if (index != -1) {
				questData.hitGrid.Add ((uint)index);
			}
			
		 	TQuestGrid currentMapData = ConfigBattleUseData.Instance.questDungeonData.GetSingleFloor (coor);
			
			Stop ();
			if (currentMapData.Star == EGridStar.GS_KEY) {
				BattleMapView.waitMove = true;
				ConfigBattleUseData.Instance.storeBattleData.HitKey = true;
				RotateAnim (MapItemKey);
				return;
			}
			
			AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);
			
			switch (currentMapData.Type) {
			case EQuestGridType.Q_NONE:
				BattleMapView.waitMove = true;
				RotateAnim (MapItemNone);
				break;
			case EQuestGridType.Q_ENEMY:
				BattleMapView.waitMove = true;
				RotateAnim (MapItemEnemy);
				break;
			case EQuestGridType.Q_KEY:
				break;
			case EQuestGridType.Q_TREATURE:
				BattleMapView.waitMove = true;
				ShowCoin(currentMapData.Coins);
				MapItemCoin();
				//					GameTimer.GetInstance().AddCountDown(ShowBottomInfo.showTime + ShowBottomInfo.scaleTime, MapItemCoin);
				break;
			case EQuestGridType.Q_TRAP:
				BattleMapView.waitMove = true;
				//					MsgCenter.Instance.Invoke(CommandEnum.ShowTrap, currentMapData.TrapInfo);
				ShowTrap(currentMapData.TrapInfo);
				GameTimer.GetInstance().AddCountDown(BattleMapView.showTime + BattleMapView.scaleTime, ()=>{
					RotateAnim (RotateEndTrap);
				});
				break;
			case EQuestGridType.Q_QUESTION:
				BattleMapView.waitMove = true;
				RotateAnim (MeetQuestion);
				break;
			case EQuestGridType.Q_EXCLAMATION:
				BattleMapView.waitMove = true;
				RotateAnim (MapItemExclamation);
				break;
			default:
				BattleMapView.waitMove = false;
				BattleEnd();
				QuestCoorEnd ();
				break;
			}
		} else {
			QuestCoorEnd ();
			ConfigBattleUseData.Instance.StoreMapData();
		}
	}

	void MeetQuestion () {
		BattleMapView.waitMove = false;
		//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		BattleEnd ();
		
	}
	
	void MapItemExclamation() {
		BattleMapView.waitMove = false;
		//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		BattleEnd ();
		
	}
	
	void RotateEndTrap() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_trigger_trap);
		BattleMapView.waitMove = false;
		TrapBase tb = currentMapData.TrapInfo;
		MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
		//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		BattleEnd ();
	}
	
	void MapItemCoin() {
		RotateAnim (RotateEndCoin);
	}
	
	void RotateEndCoin() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_get_treasure);
		BattleMapView.waitMove = false;
		questData.getMoney += currentMapData.Coins;
		//		topUI.Coin = GetCoin ();//questData.getMoney;
		ModuleManager.SendMessage(ModuleEnum.BattleTopModule,"coin",GetCoin ());
		
		MsgCenter.Instance.Invoke (CommandEnum.MeetCoin, currentMapData);
		//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		BattleEnd ();
	}
	
	void MapItemKey() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_get_key);
		//		battle.ShieldInput (false);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "gate", OpenGate as Callback);
		BattleMapView.waitMove = false;
		//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		BattleEnd ();
		MsgCenter.Instance.Invoke (CommandEnum.OpenDoor, null);
	}
	
	int GetCoin() {
		int coin = 0;
		foreach (var item in ConfigBattleUseData.Instance.storeBattleData.questData) {
			coin += item.getMoney;
		}
		return coin;
	}
	
	void OpenGate() {
		//		battle.ShieldInput (true);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
	}
	
	void MapItemNone() {
		BattleMapView.waitMove = false;
		//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
		BattleEnd ();
		
	}

	void GenerateWayPoint(Coordinate endCoord) {
		if(currentCoor.x == endCoord.x) {
			firstWay.AddRange(CaculateY(endCoord));
			return;
		}
		if(currentCoor.y == endCoord.y) {
			firstWay.AddRange(CaculateX(endCoord));
			return;
		}
		firstWay.AddRange(CaculateX(endCoord));
		firstWay.AddRange(CaculateY(endCoord));
	}
	
	List<Coordinate> CaculateX(Coordinate endCoord) {
		List<Coordinate> xWay = new List<Coordinate>();
		if(currentCoor.x < endCoord.x) {
			int i = currentCoor.x + 1;
			while(i <= endCoord.x) {
				xWay.Add(new Coordinate(i,currentCoor.y));
				i++;
			}
		}
		else if(currentCoor.x > endCoord.x) {
			int i = currentCoor.x - 1;
			while(i >= endCoord.x) {
				xWay.Add(new Coordinate(i,currentCoor.y));
				i--;
			}
		}
		return xWay;
	}
	
	List<Coordinate> CaculateY(Coordinate endCoord)
	{
		List<Coordinate> yWay = new List<Coordinate>();
		if(currentCoor.y < endCoord.y) {
			int i = currentCoor.y +1 ;
			while(i <= endCoord.y) {
				yWay.Add(new Coordinate(endCoord.x,i));
				i++;
			}
		}
		else {
			int i = currentCoor.y - 1;
			while(i >= endCoord.y)	{
				yWay.Add(new Coordinate(endCoord.x,i));
				i--;
			}
		}
		
		return yWay;
	}
}
