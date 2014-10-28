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

	private List<MapItem> surroundedCells = new List<MapItem>();

	private MapItem currentItem;
	public const float itemWidth = 114f;

	private GameObject door;
	
	private Queue<MapItem> chainLikeMapItem = new Queue<MapItem> ();

	////--------------cell info
	public static float scaleTime = 0.4f;
	public static float showTime = 0.4f;
	private UILabel typeLabel;
	private UILabel nameLabel;
	private UILabel cateGoryLabel;
	private UISprite itemSprite;

	private UILabel nameTitleLabel;
	private UILabel categoryTitleLabel;

	private const string categoryTitle = "Category: ";
	private const string coinTitle = "Coins: ";
	private GameObject cellInfo;

	private GameObject role;

//	private bool ChainLinkBattle = false;
	
//	private bool checkOut = false;
	
	private string currentShowInfo;

	private bool isMoving = false;
//	private bool isInTips = false;
	private Vector3 targetPoint;
	private List<Coordinate> movePath = new List<Coordinate>();
	private Vector3 distance = Vector3.zero;

	private int guideType = 0;

	private Coordinate currentCoor;
	private Coordinate prevCoor;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);

//		NoviceGuideStepManager.Instance.StartStep ( NoviceGuideStartType.BATTLE );

		mapBGTexture = FindChild<UITexture>("BG");
		mapBGTexture.mainTexture = ResourceManager.Instance.LoadLocalAsset ("Texture/Map/battlemap_" + BattleConfigData.Instance.GetMapID().ToString (), null) as Texture2D;

		map = new MapItem[MapConfig.MapWidth, MapConfig.MapHeight];
		template = FindChild<MapItem>("SingleMap");
		template.Init("SingleMap");
		template.gameObject.SetActive(false);

		//----------init cell info
		nameTitleLabel = FindChild<UILabel> ("CellInfo/NameTitleLabel");
		categoryTitleLabel = FindChild<UILabel> ("CellInfo/CategoryTitleLabel");
		typeLabel = FindChild<UILabel> ( "CellInfo/TypeLabel");
		nameLabel = FindChild<UILabel> ("CellInfo/NameLabel");
		cateGoryLabel = FindChild<UILabel> ( "CellInfo/CatagoryLabel");
		itemSprite = FindChild<UISprite> ( "CellInfo/Trap");
		cellInfo = FindChild ("CellInfo");
		cellInfo.transform.localScale = new Vector3 (1, 0, 1);


		role = FindChild ("Role");
		door = FindChild("Door");
		door.SetActive (false);
		UIEventListenerCustom.Get (door).onClick = ClickDoor;

		RefreshMap ();
	}

	public override void CallbackView (params object[] args)
	{
		switch (args[0].ToString()) {
			//			case "rolecoor":
			//				RoleCoordinate((Coordinate)data[1]);
			//				break;
		case "player_dead":
			BattleFail();
			break;
		case "boss_dead":
			BossDead();
			break;
		case "reload":
			RefreshMap();
			break;
		case "guide":
			ToggleGuideTips((bool)args[1],(int)args[2]);
			break;
		case "clear_quest_highlight":
			foreach (var item in map) {
				if(item.isShadowShow()){
					item.Around(true);
				}

			}
			break;
		default:
			break;
		}
	}
	
	void HaveFriendExit() {
		ModuleManager.Instance.ExitBattle ();
		ModuleManager.Instance.ShowModule(ModuleEnum.ResultModule);
		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, BattleConfigData.Instance.BattleFriend);
	}
	
	void BattleFail() {
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("ResumeQuestTitle"), 
		                                    TextCenter.GetText ("ResumeQuestContent", DataCenter.resumeQuestStone),
		                                    TextCenter.GetText ("OK"), TextCenter.GetText ("Cancel"), 
		                                    BattleFailRecover, BattleFailExit);
	}
	
	void BattleFailRecover(object data) {
		if (DataCenter.Instance.UserData.AccountInfo.stone < DataCenter.redoQuestStone) {
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("NotEnoughStone"));
			return;
		}
		
		QuestController.Instance.ResumeQuest (o=>{
			Umeng.GA.Buy ("ResumeQuest" , 1, DataCenter.resumeQuestStone);
			BattleAttackManager.Instance.AddBlood (BattleAttackManager.Instance.MaxBlood);
			BattleAttackManager.Instance.RecoverEnergePoint (DataCenter.maxEnergyPoint);
			BattleConfigData.Instance.StoreMapData ();
		}, BattleConfigData.Instance.questDungeonData.questId);
		BattleAttackManager.Instance.ClearData ();
	}
	
	
	void BattleFailExit(object data) {
		QuestController.Instance.RetireQuest (o=>{
			AudioManager.Instance.PlayAudio (AudioEnum.sound_game_over);
			ModuleManager.Instance.HideModule(ModuleEnum.BattleMapModule);
			ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "over", (Callback)(()=>{
				
				ModuleManager.Instance.ExitBattle ();
				ModuleManager.Instance.EnterMainScene();
			}) );
			BattleConfigData.Instance.ClearData ();
		}, BattleConfigData.Instance.questDungeonData.questId, true);
	}

	void RefreshMap(){
		StartMap ();
		
		if(BattleConfigData.Instance.storeBattleData.isBattle == 1){
			SetName (BattleFullScreenTipsView.CheckOut);
		}else if(BattleConfigData.Instance.storeBattleData.hitKey){
			SetName (BattleFullScreenTipsView.BossBattle);
		}
		
		
		BattleAttackManager.Instance.InitData ();
		
		prevCoor = currentCoor = BattleConfigData.Instance.storeBattleData.roleCoordinate;
		CalcRoleDestPosByCoor(currentCoor);
		role.transform.localPosition = new Vector3 (targetPoint.x, targetPoint.y, 0f);
		
		//init role pos
		
		GameTimer.GetInstance ().AddCountDown (0.1f, RecoverBuff);
		
		if (BattleConfigData.Instance.trapPoison != null) {
			BattleConfigData.Instance.trapPoison.ExcuteByDisk();
		}
		
		if (BattleConfigData.Instance.trapEnvironment != null) {
			BattleConfigData.Instance.trapEnvironment.ExcuteByDisk();
		}
		
		StoreBattleData sbd = BattleConfigData.Instance.storeBattleData;
		BattleAttackManager.Instance.CheckPlayerDead();
		// 0 is not in fight.
		
		for (int i = 0; i < sbd.EnemyInfo.Count; i++) {
			EnemyInfo tei = sbd.EnemyInfo[i];
			tei.EnemySymbol = (uint)i;
			DataCenter.Instance.UnitData.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
		}

		currentItem = map [currentCoor.x, currentCoor.y];
		if (!currentItem.hasBeenReached) {
			currentItem.hasBeenReached = true;
			currentItem.ToggleGrid();
			
			BattleConfigData.Instance.storeBattleData.GetLastQuestData().hitGrid.Add ((uint)BattleConfigData.Instance.questDungeonData.GetGridIndex (currentCoor));
			
			if(currentCoor.x == MapConfig.characterInitCoorX && currentCoor.y == MapConfig.characterInitCoorY){
				GameTimer.GetInstance ().AddCountDown (0.2f, ()=>{
					//					isInTips = true;
					ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "readymove",ReadyMoveFunc as Callback, BattleAttackManager.Instance.CheckLeaderSkillCount() * BattleAttackManager.normalAttackInterv);
				});
			}
		}else{
			NoviceGuideStepManager.Instance.StartStep(NoviceGuideStartType.START_BATTLE);
		}

		QuestGrid currentFloorData = BattleConfigData.Instance.questDungeonData.GetCellDataByCoor (currentCoor);
		HighlightSurroundedCell (currentCoor);

		if(sbd.EnemyInfo.Count > 0){
			currentFloorData.Enemy = sbd.EnemyInfo;
			if(sbd.EnemyInfo[0].enemeyType == EEnemyType.BOSS){
				BattleAttackManager.Instance.InitBoss (sbd.EnemyInfo, BattleConfigData.Instance.questDungeonData.drop.Find (a => a.dropId == 0));
				AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
			}else{
				
				BattleAttackManager.Instance.InitEnemyInfo (currentFloorData);
				if(sbd.attackRound == 0) {	// 0 == first attack
					GameTimer.GetInstance ().AddCountDown (0.3f, StartBattleEnemyAttack);
				}
			}
			GameTimer.GetInstance ().AddCountDown (0.1f, ()=>{
				ShowEnemy(currentFloorData.Enemy);
			});
			
		}
		if (sbd.hitKey) {
			door.SetActive(true);	
		}
		BattleAttackManager.Instance.GetBaseData ();
	}

	void StartMap() {
		int x = map.GetLength(0);
		int y = map.GetLength(1);
		GameObject tempObject = null;
		MapItem temp = null;
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				if(map[i,j] == null) {
					tempObject = NGUITools.AddChild(gameObject, template.gameObject);
					tempObject.SetActive(true);
					float xp = template.InitPosition.x + i * itemWidth;
					float yp = template.InitPosition.y + j * itemWidth;
					tempObject.transform.localPosition = new Vector3(xp,yp,template.InitPosition.z);
					temp = tempObject.GetComponent<MapItem>();
					temp.Coor = new Coordinate(i, j);
					temp.Init(i+"|"+j);

					UIEventListenerCustom.Get(tempObject).onClick = OnClickMapItem;
					map[i,j] = temp;
				}

				map[i,j].RefreshData();
				if(BattleConfigData.Instance.storeBattleData.isBattle == 1){
					map[i,j].ToggleGrid();
				}else{
					map[i,j].ToggleGrid(true);
				}
			}
		}
		float xCoor =  template.InitPosition.x + (x / 2) * itemWidth;
		float yCoor = template.InitPosition.y + y * itemWidth;

		ClearQuestParam data = BattleConfigData.Instance.storeBattleData.GetLastQuestData();
		for (int i = 0; i < data.hitGrid.Count; i++) {
			Coordinate coor = BattleConfigData.Instance.questDungeonData.GetGridCoordinate(data.hitGrid[i]);
			if(coor.x < 0 || coor.y < 0 || coor.x >= MapConfig.MapWidth || coor.y >= MapConfig.MapHeight) {
				continue;
			}
			map[coor.x, coor.y].ToggleGrid();
		}
	}

	
	void OnClickMapItem(GameObject go) {
//		Debug.Log ("item name: " + go.name);
		if (isMoving)
			return;
		Coordinate endCoord = go.GetComponent<MapItem>().Coor;

		if (currentCoor.x == endCoord.x) {
			movePath.AddRange (CaculateY (endCoord));
		} else
		if (currentCoor.y == endCoord.y) {
			movePath.AddRange (CaculateX (endCoord));
		} else {
			movePath.AddRange(CaculateX(endCoord));
			movePath.AddRange(CaculateY(endCoord));
		}

		Move();
	}

	Vector3[] secondPath = new Vector3[4];
	
	float adjustTime = 0.0f;
	
	bool isVerticalMove = false;
	
	void Move() {
		
		Vector3 middlePoint = Vector3.zero;
		if(movePath.Count == 0) {
			return;
		}
		
		isMoving = true;
		CalcRoleDestPosByCoor(movePath[0]);
		
		Vector3 localposition = role.transform.localPosition;
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
		path [0] = role.transform.localPosition;
		path [1] = leftMiddlePoint;
		path [2] = middlePoint;
		
		secondPath [0] = middlePoint;
		secondPath [1] = rightMiddlePoint;
		secondPath [2] = rightMiddlePoint2;
		secondPath [3] = targetPoint;
		
		AudioManager.Instance.PlayAudio (AudioEnum.sound_walk);
		iTween.MoveTo (role, iTween.Hash ("path", path, "movetopath", false, "islocal", true, "time", 0.13f+adjustTime, "easetype", iTween.EaseType.easeOutSine, "oncomplete", "MoveRoleSecond", "oncompletetarget", gameObject));

		//每移动1步，技能CD计数减1
		foreach(SkillBase skill in DataCenter.Instance.BattleData.AllSkill.Values) {
			if( skill.IsActiveSkill() ) //It is ActiveSkill
				skill.RefreashCooling();
		}
	}
	
	void MoveRoleSecond() {
		//		Debug.LogError ("MoveRoleSecond");
		AudioManager.Instance.PlayAudio (AudioEnum.sound_chess_fall);
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
		bouncePath[0] = new Vector3(secondPath [3].x+moveX, secondPath [3].y, 0);
		bouncePath[1] = new Vector3(secondPath [3].x+moveX, secondPath [3].y, 0);
		bouncePath[2] = new Vector3(secondPath [3].x, secondPath [3].y, 0);
		iTween.MoveTo (role, iTween.Hash ("path", bouncePath, "movetopath", false, "islocal", true, "time", 0.1f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "MoveEnd", "oncompletetarget", gameObject));
	}
	
	void MoveEnd() {
		Coordinate tempCoor; 
		tempCoor = movePath[0];
		movePath.RemoveAt(0);
		HighlightSurroundedCell(tempCoor);
		SyncRoleCoordinate(tempCoor);
		if(movePath.Count > 0) {
			GameTimer.GetInstance().AddCountDown(0.1f, Move);
		}
	}
	
	void SyncRoleCoordinate(Coordinate coor) {
		
		MsgCenter.Instance.Invoke (CommandEnum.MoveToMapItem, coor);
		RoleCoordinate (coor);
	}
	
	void RoleCoordinate(Coordinate coor) {


		currentItem = map[coor.x,coor.y];
		BattleAttackManager.Instance.TrapTargetPoint (coor);
		if (!currentItem.hasBeenReached) {

			BattleConfigData.Instance.storeBattleData.GetLastQuestData().hitGrid.Add ((uint)BattleConfigData.Instance.questDungeonData.GetGridIndex (coor));
			movePath.Clear ();

			QuestGrid currentMapData = BattleConfigData.Instance.questDungeonData.GetCellDataByCoor (coor);


			if (currentMapData.star == EGridStar.GS_KEY) {
				//				BattleMapView.waitMove = true;
				BattleConfigData.Instance.storeBattleData.hitKey = true;
				RotateAnim (()=>{
					SetName (BattleFullScreenTipsView.BossBattle);
					AudioManager.Instance.PlayAudio (AudioEnum.sound_get_key);
//					isInTips = true;
					ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "gate",OpenGateFunc as Callback);
					ArriveAtCell ();
				});
				return;
			}
			
			AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);
//			Debug.Log("cell type: " + currentMapData.Type);
			switch (currentMapData.type) {
			case EQuestGridType.Q_NONE:
				//				BattleMapView.waitMove = true;
				RotateAnim (ArriveAtCell);
				break;
			case EQuestGridType.Q_ENEMY:
				//				BattleMapView.waitMove = true;
				RotateAnim (()=>{
					isMoving = false;
					AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_battle);
					//		BattleMapView.waitMove = false;
					//		ShowBattle();
//					List<EnemyInfo> temp = new List<EnemyInfo> ();
					for (int i = 0; i < currentMapData.Enemy.Count; i++) {
						EnemyInfo tei = currentMapData.Enemy[i];
						tei.EnemySymbol = (uint)i;
//						temp.Add(tei);
						DataCenter.Instance.UnitData.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
					}
					BattleAttackManager.Instance.InitEnemyInfo (currentMapData);
					ShowEnemy(currentMapData.Enemy);
					ArriveAtCell();
				});
				break;
			case EQuestGridType.Q_KEY:
				break;
			case EQuestGridType.Q_TREATURE:
				//				BattleMapView.waitMove = true;
				ShowCoin(currentMapData.coins);
				GameTimer.GetInstance().AddCountDown(0f, ()=>{
					RotateAnim (()=>{
						AudioManager.Instance.PlayAudio (AudioEnum.sound_get_treasure);
						BattleConfigData.Instance.storeBattleData.GetLastQuestData ().getMoney += currentMapData.coins;
						ModuleManager.SendMessage(ModuleEnum.BattleTopModule,"refresh");
						NoviceGuideStepManager.Instance.StartStep(NoviceGuideStartType.GOLD_BOX);
						ArriveAtCell ();
					});
				});
				break;
			case EQuestGridType.Q_TRAP:
				//				BattleMapView.waitMove = true;
				//					MsgCenter.Instance.Invoke(CommandEnum.ShowTrap, currentMapData.TrapInfo);
				ShowTrap(currentMapData.TrapInfo);
				GameTimer.GetInstance().AddCountDown(BattleMapView.showTime + BattleMapView.scaleTime, ()=>{
					RotateAnim (()=>{
						AudioManager.Instance.PlayAudio (AudioEnum.sound_trigger_trap);
						//		BattleMapView.waitMove = false;
						TrapBase tb = currentMapData.TrapInfo;
//						MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
						BattleAttackManager.Instance.DisposeTrapEvent(tb);
						//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
						ArriveAtCell ();
					});
				});
				break;
			case EQuestGridType.Q_QUESTION:
				RotateAnim (()=>{
					ArriveAtCell();
				});
				break;
			case EQuestGridType.Q_EXCLAMATION:
				RotateAnim (()=>{
					ArriveAtCell();
				});
				break;
			default:
				ArriveAtCell();
				break;
			}
		} else {
			ArriveAtCell();
		}
	}

	void OpenGateFunc() {
		//		battle.ShieldInput (true);
		//key step
//		isInTips = false;
		door.SetActive (true);
		ToggleGuideTips (false, 1);
		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.GET_KEY);
	}

	
	void ReadyMoveFunc() {
//		isInTips = false;
		NoviceGuideStepManager.Instance.StartStep ( NoviceGuideStartType.START_BATTLE );
	}

	void MeetBoss () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);
		//		AudioManager.Instance.StopBackgroundMusic (true);
		//		battle.ShieldInput ( true );
		//		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		//		BattleMapView.waitMove = false;
		//		ShowBattle();
//		List<EnemyInfo> temp = new List<EnemyInfo> ();
		for (int i = 0; i < BattleConfigData.Instance.questDungeonData.Boss.Count; i++) {
			EnemyInfo tei = BattleConfigData.Instance.questDungeonData.Boss [i];
			tei.EnemySymbol = (uint)i;
//			temp.Add (tei);
		}
		DropUnit bossDrop = BattleConfigData.Instance.questDungeonData.drop.Find (a => a.dropId == 0);
		BattleAttackManager.Instance.InitBoss (BattleConfigData.Instance.questDungeonData.Boss, bossDrop);
		
		//		BattleConfigData.Instance.storeBattleData.isBattle = 2; // 2 == battle boss. 
		//		battle.ShowEnemy (temp, true);
		//		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		ShowEnemy(BattleConfigData.Instance.questDungeonData.Boss,true);
		
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
	}

	void ShowEnemy(List<EnemyInfo> count, bool isBoss = false) {

		ModuleManager.Instance.HideModule(ModuleEnum.BattleMapModule);

		ModuleManager.Instance.ShowModule(ModuleEnum.BattleManipulationModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.BattleEnemyModule, "enemy", count);
		//		MsgCenter.Instance.Invoke (CommandEnum.ReduceActiveSkillRound);

		BattleConfigData.Instance.storeBattleData.EnemyInfo = count;
		BattleConfigData.Instance.storeBattleData.attackRound ++;
		BattleConfigData.Instance.StoreMapData ();
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule, "state_info",BattleAttackManager.stateInfo [0]);

		GameTimer.GetInstance ().AddCountDown ( 0.3f, StartBattleEnemyAttack );
		//		Debug.Log ("battle guide----------");
		
	}

	void ShieldMap(object data) {
		int b = (int)data;

		for (int i = 0; i < map.GetLength(0); i++) {
			for (int j = 0; j < map.GetLength(1); j++) {
				map[i,j].HideEnvirment(b > 0);
			}
		}
	}


	void CalcRoleDestPosByCoor(Coordinate coor) {
		prevCoor = currentCoor;
		currentCoor = coor;

		if (coor.x > map.GetLength (0) || coor.y > map.GetLength (1))
			targetPoint = transform.localPosition;
		else {
			Vector3 pos = map[coor.x, coor.y].transform.localPosition;
			targetPoint = new Vector3(pos.x, pos.y + 25, pos.z);
		}

	}

	void RotateAnim(Callback cb) {
//		Debug.Log ("item active: " + currentItem.gameObject.activeSelf + " name: " + currentItem.gameObject.name);
		currentItem.RotateSingle (cb);
	}

	void RotateAll(Callback cb) {
		currentItem.RotateAll (cb, false);
	}

	Queue<MapItem> AttakAround(Coordinate coor) {
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

	void AddMapSecuritylevel(Coordinate coor) {
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

	void HighlightSurroundedCell(Coordinate coor) {
		if(surroundedCells.Count > 0) {
			for (int i = 0; i < surroundedCells.Count; i++) {
				surroundedCells[i].Around(false);
			}

			surroundedCells.Clear();
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
		surroundedCells.Add(item);
	}

	void ClickDoor(GameObject go) {

		if ( DGTools.EqualCoordinate (currentCoor, MapConfig.endCoor) && currentShowInfo != null) {
			if (BattleConfigData.Instance.storeBattleData.isBattle != 1) {
				if( BattleConfigData.Instance.questDungeonData.isLastCell() ) {
					AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);
					ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "boss",MeetBoss as Callback);
				} else {
					BattleConfigData.Instance.questDungeonData.currentFloor ++;
					BattleConfigData.Instance.storeBattleData.questData.Add (new ClearQuestParam ());
					if (BattleAttackManager.maxEnergyPoint >= 10) {
						BattleAttackManager.maxEnergyPoint = DataCenter.maxEnergyPoint;
					} else {
						BattleAttackManager.maxEnergyPoint += 10;
					}
					BattleConfigData.Instance.ResetRoleCoordinate();
					BattleConfigData.Instance.StoreMapData ();
					
					BattleAttackManager.Instance.Reset ();
					ModuleManager.SendMessage (ModuleEnum.BattleTopModule, "refresh");

					
				}
				return;
			}
			else if (currentShowInfo == BattleFullScreenTipsView.CheckOut) {
				door.SetActive(false);
				BattleConfigData.Instance.ClearData();
				ModuleManager.SendMessage(ModuleEnum.BattleTopModule,"clear_quest");

			}
		} else {
			OnClickMapItem(map[2,4].gameObject);
		}


	}


	void ShowTapToCheckOut () {
		door.SetActive (true);
		
		SetName (BattleFullScreenTipsView.CheckOut);
	}


	public void ArriveAtCell() {
		isMoving = false;
		BattleConfigData.Instance.StoreMapData ();
		
		int index = BattleConfigData.Instance.questDungeonData.GetGridIndex (currentCoor);
		
		uint uIndex = (uint)index;
		ClearQuestParam questData = BattleConfigData.Instance.storeBattleData.GetLastQuestData ();
		if (questData.hitGrid.Contains (uIndex)) {
			index = questData.hitGrid.FindIndex(a=>a == uIndex);
			if(index != questData.hitGrid.Count - 1) {
//				if(ChainLinkBattle ){
//					ChainLinkBattle = false;
//				}
				return;
			}
		}
		
		QuestGrid tqg = BattleConfigData.Instance.questDungeonData.GetCellDataByCoor (currentCoor);
		
		if (tqg == null || tqg.type != EQuestGridType.Q_ENEMY) {
			return;
		}
		
		AddMapSecuritylevel (currentCoor);
		chainLikeMapItem = AttakAround (currentCoor);
		
		if (chainLikeMapItem.Count > 0) {
			SyncRoleCoordinate (chainLikeMapItem.Dequeue ().Coor);
		}
	}
	
	void NoSPMove(object data) {
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick", false);
		
		Coordinate cd;
		if (data == null) {
			cd = prevCoor;
		} else {
			cd = (Coordinate)data;	
		}
		
		CalcRoleDestPosByCoor (cd);
//		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "rolecoor", cd);

		RoleCoordinate (cd);
		StartCoroutine (MoveByTrap ());
	}
	
	void TrapMove(object data) {
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick", true);
		if (data == null) {
			CalcRoleDestPosByCoor (currentCoor);
			StartCoroutine(MoveByTrap());
			return;
		}
		Coordinate cd = (Coordinate)data;
		CalcRoleDestPosByCoor (cd);
//		MsgCenter.Instance.Invoke(CommandEnum.TrapTargetPoint, cd);
		BattleAttackManager.Instance.TrapTargetPoint (cd);
//		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "rolecoor", cd);
		RoleCoordinate (cd);

		role.transform.localPosition = targetPoint;
//		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "style", currentCoor);
		HighlightSurroundedCell (currentCoor);
	}
	
	IEnumerator MoveByTrap() {
		while (true) {
			role.transform.localPosition = Vector3.Lerp(role.transform.localPosition,targetPoint,Time.deltaTime * 20);
			distance = role.transform.localPosition - targetPoint;
			yield return Time.deltaTime;
			if (distance.magnitude < 1f) {
				HighlightSurroundedCell(currentCoor);
				yield break;
				
			}
		}
	}


	public static bool recoverPosion = false;
	public static bool reduceHurt = false;
	public static bool reduceDefense = false;
	public static bool strengthenAttack = false;

	void RecoverBuff() {
		ExcuteDiskActiveSkill(BattleConfigData.Instance.posionAttack, ref recoverPosion);
		ExcuteDiskActiveSkill(BattleConfigData.Instance.reduceHurtAttack, ref reduceHurt);
		ExcuteDiskActiveSkill(BattleConfigData.Instance.reduceDefenseAttack, ref reduceDefense);
		ExcuteDiskActiveSkill(BattleConfigData.Instance.strengthenAttack, ref strengthenAttack);
	}

	void ExcuteDiskActiveSkill (AttackInfoProto ai, ref bool excute) {
		if (ai != null) {
			ActiveSkill iase = BattleAttackManager.Instance.GetActiveSkill (ai.userUnitID);
			if (iase != null) {
				excute = true;
				iase.ExcuteByDisk (ai);
			} else { 
				excute = false;
			}
		} else {
			excute = false;
		}
	}
	
	public void BossDead() {
		
		DropUnit bossDrop = BattleConfigData.Instance.questDungeonData.drop.Find (a => a.dropId == 0);
		if (bossDrop != null) {
			BattleConfigData.Instance.storeBattleData.GetLastQuestData().getUnit.Add(bossDrop.dropId);
		}
		
		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_die);
		
		ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "clear", QuestClear as Callback);
	}
	
	public void StartBattleEnemyAttack() {
		EnemyAttackEnum eae = currentItem.TriggerAttack ();
		switch (eae) {
		case EnemyAttackEnum.BackAttack:
			AudioManager.Instance.PlayAudio(AudioEnum.sound_back_attack);
			break;
		case EnemyAttackEnum.FirstAttack:
			ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "first");
			AudioManager.Instance.PlayAudio(AudioEnum.sound_first_attack);
			break;
		default:
			break;
		}
	}


	void QuestClear() {
		StartCoroutine (EndRotate (ShowTapToCheckOut));
	}

	
	IEnumerator EndRotate (Callback cb) {
		bool allShow = cb == null ? true : false;

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
		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.QUEST_CLEAR);
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

	
	void ShowTrap(TrapBase tb) {
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
		iTween.ScaleTo(cellInfo,iTween.Hash("y", 1f, "time", scaleTime,"oncompletetarget",gameObject,"oncomplete","ShowEnd"));
	}
	
	void ShowCoin(int number) {
		nameLabel.text = "";
		typeLabel.text = "";
		itemSprite.spriteName = BattleMapView.chestSpriteName; // S  is coin sprite name in atlas.
		cateGoryLabel.text = number.ToString ();
		categoryTitleLabel.text = coinTitle;
		if (nameTitleLabel.enabled) {
			nameTitleLabel.enabled = false;
		}
		
		iTween.ScaleTo(cellInfo,iTween.Hash("y", 1f, "time", scaleTime,"oncompletetarget",gameObject,"oncomplete","ShowEnd"));
	}
	
	void ShowEnd() {
		GameTimer.GetInstance ().AddCountDown (showTime, ()=>{
			nameLabel.text = "";
			cateGoryLabel.text = "";
			cellInfo.transform.localScale = new Vector3 (1f, 0f, 1f);
		});
	}
	
	void SetName(string name) {
		currentShowInfo = name;
		
		string[] info = currentShowInfo.Split('|');
		door.transform.Find("Top").GetComponent<UILabel>().text = info [0];
		door.transform.Find("Bottom").GetComponent<UILabel>().text = info [1];
	}

	public GameObject GetMapItem(int x, int y){
		return map [x, y].gameObject;
	}

	void ToggleGuideTips(bool isShow, int type){
		if (isShow) {
			guideType = type;
			switch (type) {
			case 1: //key
				NoviceGuideUtil.ShowArrow(GameObject.FindWithTag("map_key"),new Vector3(0,0,1),false,false);
				break;
			case 2: // door
				NoviceGuideUtil.ShowArrow(door, new Vector3(0,0,3),false);
				break;
			default:
				break;
			}
		}else{
			guideType = 0;
			switch (type) {
			case 1: //key
				NoviceGuideUtil.RemoveAllArrows();
				break;
			case 2: // door
				break;
			default:
				break;
			}
		}

	}
}
