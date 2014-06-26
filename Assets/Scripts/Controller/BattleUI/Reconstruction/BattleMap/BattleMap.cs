using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleMap : UIBaseUnity {
	private MapItem template;
	private MapItem[,] map;
	private MapItem temp;
	private List<MapItem> prevAround = new List<MapItem>();
	[HideInInspector]
	public MapItem prevMapItem;
	private static bool wMove = false;
	public const float itemWidth = 114f;

	[HideInInspector]
	public BattleQuest bQuest;
	[HideInInspector]
	public MapDoor door;

	[HideInInspector]
	public static bool waitMove {
		set{ wMove = value; }
		get{ return wMove; }
	}
	
	public BattleQuest BQuest {
		set{ bQuest = value; }
	}

	public override void Init (string name) {
		base.Init (name);
		map = new MapItem[MapConfig.MapWidth, MapConfig.MapHeight];
		template = FindChild<MapItem>("SingleMap");
		template.Init("SingleMap");
		door = FindChild<MapDoor>("Door_1");
		door.Init ("Door_1");
		door.battleMap = this;
		template.gameObject.SetActive(false);
		gameObject.transform.localPosition += Vector3.right * 5f;
		wMove = false;
	}

	public override void CreatUI () {
		LogHelper.Log("battle map creat ui" );
		base.CreatUI ();
	}

	void StartMap() {
		int x = map.GetLength(0);
		int y = map.GetLength(1);
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
					temp.battleMap = this;
					UIEventListener.Get(tempObject).onClick = OnClickMapItem;
					map[i,j] = temp;
				}
				else {
					map[i,j].ShowUI();
				}
			}
		}
		float xCoor =  template.InitPosition.x + (x / 2) * itemWidth;
		float yCoor = template.InitPosition.y + y * itemWidth;
		door.SetPosition (new Vector3 (xCoor, yCoor, door.transform.localPosition.z));
		RefreshMap (bQuest.questData);
	}

	public override void HideUI () {
		base.HideUI ();
//		useMapItem.Clear ();
		prevAround.Clear ();
		gameObject.SetActive (false);
		for (int i = 0; i < map.GetLength(0); i++) {
			for (int j = 0; j < map.GetLength(1); j++) {
				Destroy(map[i,j].gameObject);
				map[i,j] = null;
			}
		}

		door.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.ShieldMap, ShieldMap);
	}

	public override void ShowUI () {
		base.ShowUI ();
		door.ShowUI ();
		gameObject.SetActive (true);
		StartMap ();
		MsgCenter.Instance.AddListener (CommandEnum.ShieldMap, ShieldMap);
	}

	public void RefreshMap(TClearQuestParam cqp) {
		for (int i = 0; i < cqp.hitGrid.Count; i++) {
			Coordinate coor = bQuest.questDungeonData.GetGridCoordinate(cqp.hitGrid[i]);
			map[coor.x,coor.y].HideGridNoAnim();
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
			temp = go.GetComponent<MapItem>();
			bQuest.TargetItem(temp.Coor);
		}
	}

	public Vector3 GetPosition(int x, int y) {
		if(x > map.GetLength(0) || y > map.GetLength(1))
			return Vector3.zero;
		return map[x, y].transform.localPosition;
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
		bool allShow = false;
		if (cb == null) {
			allShow = true;
		}
						
		StartCoroutine (EndRotate (allShow));
	}

	IEnumerator EndRotate (bool allShow) {
		for (int i = 0; i < map.GetLength(0); i++) {
			for (int j = 0; j < map.GetLength(1); j++) {
				if(i == map.GetLength(0) - 1 && j == map.GetLength(1) - 1){
					map[i,j].RotateAll(cb,allShow);
				} else{
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
}
