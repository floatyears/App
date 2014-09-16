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

	private List<MapItem> prevAround = new List<MapItem>();

	private MapItem currentItem;
	public const float itemWidth = 114f;

	private GameObject door;
	
	private Queue<MapItem> chainLikeMapItem = new Queue<MapItem> ();

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
	private GameObject cellInfo;

	private GameObject role;

	private bool ChainLinkBattle = false;
	
	private bool checkOut = false;
	
	private string currentShowInfo;

	private bool isMoving = false;
	private Vector3 targetPoint;
	private const int xOffset = -5;
	private const int YOffset = 20;
	private const int ZOffset = -40;
	private Vector3 scale = new Vector3(30f, 25f, 30f);
	private Vector3 angle = new Vector3(330f, 0f, 0f);
	private List<Coordinate> movePath = new List<Coordinate>();
	private Vector3 distance = Vector3.zero;

	
	private Coordinate currentCoor;
	private Coordinate prevCoor;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);

		NoviceGuideStepEntityManager.Instance ().StartStep ( NoviceGuideStartType.BATTLE );

		mapBGTexture = FindChild<UITexture>("BG");
		mapBGTexture.mainTexture = ResourceManager.Instance.LoadLocalAsset ("Texture/Map/battlemap_" + ConfigBattleUseData.Instance.GetMapID().ToString (), null) as Texture2D;

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
		UIEventListener.Get (door).onClick = ClickDoor;

		StartMap ();

		if (ConfigBattleUseData.Instance.hasBattleData () > 0) {
			
			Coordinate coor = ConfigBattleUseData.Instance.storeBattleData.roleCoordinate;
			BattleUseData.Instance.Els.CheckLeaderSkillCount();
			BattleUseData.Instance.InitBattleUseData (ConfigBattleUseData.Instance.storeBattleData);
			
			if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				return;	
			}
			
			TQuestGrid currentMapData = ConfigBattleUseData.Instance.questDungeonData.GetCellInfo (coor);
			ChangeStyle (coor);
			Stop ();
			
			if (ConfigBattleUseData.Instance.trapPoison != null) {
				ConfigBattleUseData.Instance.trapPoison.ExcuteByDisk();
			}
			
			if (ConfigBattleUseData.Instance.trapEnvironment != null) {
				ConfigBattleUseData.Instance.trapEnvironment.ExcuteByDisk();
			}
			
			TStoreBattleData sbd = ConfigBattleUseData.Instance.storeBattleData;
			
			// 0 is not in fight.
			if (sbd.isBattle == 0) {
				if (sbd.recoveBattleStep == RecoveBattleStep.RB_BossDead) {
					BossDead();
					return;
				}
				BattleUseData.Instance.CheckPlayerDead();
				return;
			}
			
			//		BattleMapView.waitMove = false;
			List<TEnemyInfo> temp = new List<TEnemyInfo> ();
			for (int i = 0; i < sbd.enemyInfo.Count; i++) {
				TEnemyInfo tei = new TEnemyInfo(sbd.enemyInfo[i]);
				tei.EnemySymbol = (uint)i;
				temp.Add (tei);
				DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
			}
			
			if (sbd.isBattle == 1) {		// 1 == battle enemy
				currentMapData.Enemy = temp;
				BattleUseData.Instance.InitEnemyInfo (currentMapData);
				if(sbd.attackRound == 0) {	// 0 == first attack
					GameTimer.GetInstance ().AddCountDown (0.3f, StartBattleEnemyAttack);
				}
			} else if (sbd.isBattle == 2) {	// 2 == battle boss
				battleEnemy = true;
				//			battle.ShieldInput (true);
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
				ConfigBattleUseData.Instance.questDungeonData.Boss= temp;
				TDropUnit bossDrop = ConfigBattleUseData.Instance.questDungeonData.DropUnit.Find (a => a.DropId == 0);
				BattleUseData.Instance.InitBoss (ConfigBattleUseData.Instance.questDungeonData.Boss, bossDrop);
				AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_boss_battle);
			}
			//		battle.ShowEnemy(temp);
			
			GameTimer.GetInstance ().AddCountDown (0.1f, RecoverBuff);
			
			BattleUseData.Instance.CheckPlayerDead();	
		}else{
			BattleUseData.Instance.InitBattleUseData(null);
			//init role
			prevCoor = currentCoor = ConfigBattleUseData.Instance.roleInitCoordinate;
			
			CalcRoleDestPosByCoor(currentCoor);
			
			role.transform.localPosition = new Vector3 (targetPoint.x, targetPoint.y, 0f);
			SyncRoleCoordinate(currentCoor);
			Stop();
		}
	}

	public override void HideUI () {
		base.HideUI ();
		prevAround.Clear ();

		checkOut = false;
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

					UIEventListener.Get(tempObject).onClick = OnClickMapItem;
					map[i,j] = temp;
				}
			}
		}
		float xCoor =  template.InitPosition.x + (x / 2) * itemWidth;
		float yCoor = template.InitPosition.y + y * itemWidth;

		TClearQuestParam data = ConfigBattleUseData.Instance.storeBattleData.GetLastQuestData();
		for (int i = 0; i < data.hitGrid.Count; i++) {
			Coordinate coor = ConfigBattleUseData.Instance.questDungeonData.GetGridCoordinate(data.hitGrid[i]);
			if(coor.x < 0 || coor.y < 0 || coor.x >= MapConfig.MapWidth || coor.y >= MapConfig.MapHeight) {
				continue;
			}
			map[coor.x, coor.y].HideGridNoAnim();
		}
	}

	
	void OnClickMapItem(GameObject go) {
		Debug.Log ("item name: " + go.name);
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
		bouncePath[0] = new Vector3(secondPath [3].x+moveX, secondPath [3].y, secondPath [3].y + moveY);
		bouncePath[1] = new Vector3(secondPath [3].x+moveX, secondPath [3].y, secondPath [3].y + bounceOffset);
		bouncePath[2] = new Vector3(secondPath [3].x, secondPath [3].y, secondPath [3].y);
		iTween.MoveTo (role, iTween.Hash ("path", bouncePath, "movetopath", false, "islocal", true, "time", 0.1f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "MoveEnd", "oncompletetarget", gameObject));
	}
	
	void MoveEnd() {
		Coordinate tempCoor; 
		if(!isMoving) {
			movePath.Clear();
		}
		else {
			tempCoor = movePath[0];
			movePath.RemoveAt(0);
			ChangeStyle(tempCoor);
			SyncRoleCoordinate(tempCoor);
			if(movePath.Count > 0) {
				GameTimer.GetInstance().AddCountDown(0.1f, Move);
			}
			else
				isMoving = false;
		}
	}
	
	void Stop() {
		isMoving = false;
		movePath.Clear ();
	}
	
	void SyncRoleCoordinate(Coordinate coor) {
		
		MsgCenter.Instance.Invoke (CommandEnum.MoveToMapItem, coor);
		RoleCoordinate (coor);
	}
	
	void RoleCoordinate(Coordinate coor) {
		
		currentItem = map[coor.x,coor.y];
		if (!currentItem.hasBeenReached) {
			if (coor.x == MapConfig.characterInitCoorX && coor.y == MapConfig.characterInitCoorY) {
				currentItem.HideGridNoAnim ();
				GameTimer.GetInstance ().AddCountDown (0.2f, ()=>{
					
					ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "readymove", BattleUseData.Instance.Els.CheckLeaderSkillCount() * AttackController.normalAttackInterv);
				});
				ConfigBattleUseData.Instance.StoreMapData();
				return;
			}

//			ConfigBattleUseData.Instance.questDungeonData.currentFloor = 

			int index = ConfigBattleUseData.Instance.questDungeonData.GetGridIndex (coor);
			
			if (index != -1) {
				ConfigBattleUseData.Instance.storeBattleData.GetLastQuestData().hitGrid.Add ((uint)index);
			}
			
			TQuestGrid currentMapData = ConfigBattleUseData.Instance.questDungeonData.GetCellInfo (coor);
			
			Stop ();
			if (currentMapData.Star == EGridStar.GS_KEY) {
				//				BattleMapView.waitMove = true;
				ConfigBattleUseData.Instance.storeBattleData.HitKey = true;
				RotateAnim (()=>{
					AudioManager.Instance.PlayAudio (AudioEnum.sound_get_key);
					//		battle.ShieldInput (false);
					ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
					ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "gate");
					//		BattleMapView.waitMove = false;
					ArriveAtCell ();
				});
				return;
			}
			
			AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);
//			Debug.Log("cell type: " + currentMapData.Type);
			switch (currentMapData.Type) {
			case EQuestGridType.Q_NONE:
				//				BattleMapView.waitMove = true;
				RotateAnim (ArriveAtCell);
				break;
			case EQuestGridType.Q_ENEMY:
				//				BattleMapView.waitMove = true;
				RotateAnim (()=>{
					AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_battle);
					//		BattleMapView.waitMove = false;
					//		ShowBattle();
					List<TEnemyInfo> temp = new List<TEnemyInfo> ();
					for (int i = 0; i < currentMapData.Enemy.Count; i++) {
						TEnemyInfo tei = currentMapData.Enemy[i];
						tei.EnemySymbol = (uint)i;
						temp.Add(tei);
						DataCenter.Instance.CatalogInfo.AddMeetNotHaveUnit(tei.UnitID);
					}
					BattleUseData.Instance.InitEnemyInfo (currentMapData);
					ConfigBattleUseData.Instance.storeBattleData.isBattle = 1;	// 1 == battle enemy
					//		battle.ShowEnemy (temp);
					ModuleManager.Instance.ShowModule(ModuleEnum.BattleManipulationModule);
					ShowEnemy(temp);
					ModuleManager.Instance.HideModule(ModuleEnum.BattleMapModule);
//					ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
					GameTimer.GetInstance ().AddCountDown ( 0.3f, StartBattleEnemyAttack );
				});
				break;
			case EQuestGridType.Q_KEY:
				break;
			case EQuestGridType.Q_TREATURE:
				//				BattleMapView.waitMove = true;
				ShowCoin(currentMapData.Coins);
				GameTimer.GetInstance().AddCountDown(showTime + scaleTime, ()=>{
					RotateAnim (()=>{
						AudioManager.Instance.PlayAudio (AudioEnum.sound_get_treasure);
						//		BattleMapView.waitMove = false;
						//		questData.getMoney += currentMapData.Coins;
								ConfigBattleUseData.Instance.storeBattleData.GetLastQuestData ().getMoney += currentMapData.Coins;
						//		topUI.Coin = GetCoin ();//questData.getMoney;
						ModuleManager.SendMessage(ModuleEnum.BattleTopModule,"updatecoin");

						
						//		MsgCenter.Instance.Invoke (CommandEnum.MeetCoin, currentMapData);
						//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
						//		BattleEnd ();
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
								MsgCenter.Instance.Invoke(CommandEnum.MeetTrap, tb);
						//		MsgCenter.Instance.Invoke (CommandEnum.BattleEnd, null);
						ArriveAtCell ();
					});
				});
				break;
			case EQuestGridType.Q_QUESTION:
				//				BattleMapView.waitMove = true;
				RotateAnim (()=>{
					ArriveAtCell();
				});
				break;
			case EQuestGridType.Q_EXCLAMATION:
				//				BattleMapView.waitMove = true;
				RotateAnim (()=>{
					ArriveAtCell();
				});
				break;
			default:
				//				BattleMapView.waitMove = false;
				ArriveAtCell();
				break;
			}
		} else {
			IfArriveAtTheDoor ();
			ConfigBattleUseData.Instance.StoreMapData();
		}
	}

	void ShowEnemy(List<TEnemyInfo> count, bool isBoss = false) {
		//		isShowEnemy = true;
//		IsBoss = isBoss;
		//		Debug.LogError ("IsBoss : " + IsBoss);
		//		battleEnemy.Refresh(count);
		ModuleManager.Instance.ShowModule (ModuleEnum.BattleEnemyModule, "enemy", count);
		//		MsgCenter.Instance.Invoke (CommandEnum.ReduceActiveSkillRound);
		TStoreBattleData tsbd =  ConfigBattleUseData.Instance.storeBattleData;
		tsbd.tEnemyInfo = count;
		ConfigBattleUseData.Instance.storeBattleData.attackRound ++;
		ConfigBattleUseData.Instance.StoreMapData ();
		MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [0]);
		
		//		Debug.Log ("battle guide----------");
		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.BOSS_ATTACK_ONE) {
//			if(IsBoss){
//				Debug.Log("is boss -------------");
//				NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.FIGHT);
//			}
			
		} else {
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.FIGHT);
		}
		
	}
	
	private void IfArriveAtTheDoor() {
		if ( DGTools.EqualCoordinate (currentCoor, MapConfig.endCoor)) {
			door.SetActive(true);
		} else {
			door.SetActive(false);
		}
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
			targetPoint = map[coor.x, coor.y].transform.localPosition;
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

	void ChangeStyle(Coordinate coor) {
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

	
	void OpenDoor (object data) {
		ConfigBattleUseData.Instance.storeBattleData.HitKey = true;
	}
	
	void ShowTapToBattle () {
		door.SetActive (true);
		SetName (BattleFullScreenTipsView.BossBattle);
	}
	
	void ClickDoor(GameObject go) {
		
		if (currentShowInfo == BattleFullScreenTipsView.BossBattle) {
			if( ConfigBattleUseData.Instance.questDungeonData.isLastCell() ) {
				AudioManager.Instance.PlayAudio (AudioEnum.sound_boss_battle);
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
				ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "boss");
				Stop();
				battleEnemy = true;
			} else {
				//			battleMap.door.isClick = false;
				ConfigBattleUseData.Instance.questDungeonData.currentFloor ++;
				ClearQuestParam clear = new ClearQuestParam ();
				TClearQuestParam cqp = new TClearQuestParam (clear);
				ConfigBattleUseData.Instance.storeBattleData.questData.Add (cqp);
				ModuleManager.SendMessage(ModuleEnum.BattleTopModule,"setfloor");
				if (BattleUseData.maxEnergyPoint >= 10) {
					BattleUseData.maxEnergyPoint = DataCenter.maxEnergyPoint;
				} else {
					BattleUseData.maxEnergyPoint += 10;
				}
				ConfigBattleUseData.Instance.ResetRoleCoordinate();
				ConfigBattleUseData.Instance.StoreMapData ();
				
				battleEnemy = false;
				BattleUseData.Instance.RemoveListen ();
				BattleUseData.Instance.Reset ();
				ModuleManager.SendMessage (ModuleEnum.BattleTopModule, "refresh");
				//			BattleMapView.waitMove = false;
				//		MsgCenter.Instance.Invoke (CommandEnum.InquiryBattleBaseData);
				BattleUseData.Instance.GetBaseData ();
				//		if (questFullScreenTips == null) {
				//			CreatBoosAppear();
				//		}
				
			}
			return;
		}
		else if (currentShowInfo == BattleFullScreenTipsView.CheckOut && checkOut) {
			checkOut = false;
			door.SetActive(false);
//			battleMap.bQuest.CheckOut();
		}
	}


	bool battleEnemy = false;

	void ShowTapToCheckOut () {
		door.SetActive (true);
		
		SetName (BattleFullScreenTipsView.CheckOut);
		checkOut = true;
	}


	public void ArriveAtCell(object data = null) {
		
		IfArriveAtTheDoor ();
		ShieldAllInput (false);
		bool b = data != null ? (bool)data : false;
		if (battleEnemy && b) {
			BossDead();
			ConfigBattleUseData.Instance.storeBattleData.recoveBattleStep = RecoveBattleStep.RB_BossDead;
			ConfigBattleUseData.Instance.StoreMapData ();
			
			ShieldAllInput (true);
			return;
		}
		
		ConfigBattleUseData.Instance.storeBattleData.recoveBattleStep = RecoveBattleStep.RB_None;
		ConfigBattleUseData.Instance.StoreMapData ();
		
		int index = ConfigBattleUseData.Instance.questDungeonData.GetGridIndex (currentCoor);
		
		if (index == -1) {
			ShieldAllInput (true);
			return;	
		}
		
		uint uIndex = (uint)index;
		TClearQuestParam questData = ConfigBattleUseData.Instance.storeBattleData.GetLastQuestData ();
		if (questData.hitGrid.Contains (uIndex)) {
			index = questData.hitGrid.FindIndex(a=>a == uIndex);
			if(index != questData.hitGrid.Count - 1) {
				if(ChainLinkBattle ){
					ChainLinkBattle = false;
				}
				ShieldAllInput (true);
				return;
			}
		}
		
		TQuestGrid tqg = ConfigBattleUseData.Instance.questDungeonData.GetCellInfo (currentCoor);
		
		if (tqg == null || tqg.Type != EQuestGridType.Q_ENEMY) {
			ShieldAllInput (true);
			return;
		}
		
		AddMapSecuritylevel (currentCoor);
		chainLikeMapItem = AttakAround (currentCoor);
		
		if (chainLikeMapItem.Count == 0) {
			ChainLinkBattle = false;
		} else {
			ChainLinkBattle = true;
			SyncRoleCoordinate (chainLikeMapItem.Dequeue ().Coor);
		}
		ShieldAllInput (true);
	}

	
	void ShieldAllInput (bool shiled) {
//		Main.Instance.NguiCamera.enabled = shiled;
//		Main.Instance.GInput.IsCheckInput = shiled;
//		BattleBottomView.notClick = !shiled;
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
		MsgCenter.Instance.Invoke(CommandEnum.TrapTargetPoint, cd);
//		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "rolecoor", cd);
		RoleCoordinate (cd);

		role.transform.localPosition = targetPoint;
//		ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "style", currentCoor);
		ChangeStyle (currentCoor);
	}
	
	IEnumerator MoveByTrap() {
		while (true) {
			Stop ();	
			role.transform.localPosition = Vector3.Lerp(role.transform.localPosition,targetPoint,Time.deltaTime * 20);
			distance = role.transform.localPosition - targetPoint;
			yield return Time.deltaTime;
			if (distance.magnitude < 1f) {
				ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick", true);
//				ModuleManager.SendMessage (ModuleEnum.BattleMapModule, "style", currentCoor);
				ChangeStyle(currentCoor);
				yield break;
				
			}
		}
	}


	public static bool recoverPosion = false;
	public static bool reduceHurt = false;
	public static bool reduceDefense = false;
	public static bool strengthenAttack = false;

	void RecoverBuff() {
		ExcuteDiskActiveSkill(ConfigBattleUseData.Instance.posionAttack, ref recoverPosion);
		ExcuteDiskActiveSkill(ConfigBattleUseData.Instance.reduceHurtAttack, ref reduceHurt);
		ExcuteDiskActiveSkill(ConfigBattleUseData.Instance.reduceDefenseAttack, ref reduceDefense);
		ExcuteDiskActiveSkill(ConfigBattleUseData.Instance.strengthenAttack, ref strengthenAttack);
	}

	void ExcuteDiskActiveSkill (AttackInfo ai, ref bool excute) {
		if (ai != null) {
			IActiveSkillExcute iase = BattleUseData.Instance.excuteActiveSkill.GetActiveSkill (ai.UserUnitID);
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
	
	void BossDead() {
		
		TDropUnit bossDrop = ConfigBattleUseData.Instance.questDungeonData.DropUnit.Find (a => a.DropId == 0);
		if (bossDrop != null) {
			ConfigBattleUseData.Instance.storeBattleData.GetLastQuestData().getUnit.Add(bossDrop.DropId);
		}
		
		//		battle.ShieldInput (false);
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
		BattleBottomView.notClick = true;
		
		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_die);
		
		ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "clear", QuestClear as Callback);
	}
	
	public void StartBattleEnemyAttack() {
		EnemyAttackEnum eae = currentItem.TriggerAttack ();
		switch (eae) {
		case EnemyAttackEnum.BackAttack:
			ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "back");
			AudioManager.Instance.PlayAudio(AudioEnum.sound_back_attack);
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
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
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "banclick",true);
		ModuleManager.SendMessage (ModuleEnum.BattleTopModule, "banclick",false);
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
		typeLabel.text = "Coin";
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
}