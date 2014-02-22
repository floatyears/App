using UnityEngine;
using System.Collections.Generic;

public class BattleMap : UIBaseUnity {
	private MapItem template;
	private MapItem[,] map;
	private MapItem temp;
	private BattleQuest bQuest;
	private List<MapItem> prevAround = new List<MapItem>();
	private List<MapItem> useMapItem = new List<MapItem>();
	private MapItem prevMapItem;
	private MapDoor door;
	private const float itemWidth = 127.5f;

	private static GameObject box;
	public static GameObject Box {
		get { return box; }
	}
	[HideInInspector]
	public bool waitMove =  false;
	
	public BattleQuest BQuest {
		set{ bQuest = value; }
	}

	public override void Init (string name) {
		base.Init (name);
		map = new MapItem[bQuest.MapWidth, bQuest.MapHeight];
		template = FindChild<MapItem>("SingleMap");
		template.Init("SingleMap");
		door = FindChild<MapDoor>("Door_1");
		door.Init ("Door_1");
		template.gameObject.SetActive(false);
		gameObject.transform.localPosition += Vector3.right * 5f;
		box = transform.Find ("magic_Box04").gameObject;
		box.SetActive (false);

	
	}

	public override void CreatUI ()
	{
		LogHelper.Log("battle map creat ui" );
		base.CreatUI ();
		//StartMap ();
	}

	void StartMap() {
		int x = map.GetLength(0);
		int y = map.GetLength(1);
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				if(map[i,j] == null) {
					tempObject = NGUITools.AddChild(gameObject, template.gameObject);
					tempObject.SetActive(true);
					float xp = template.InitPosition.x + i * itemWidth;//template.Width;
					float yp = template.InitPosition.y + j * itemWidth;//template.Height;
					tempObject.transform.localPosition = new Vector3(xp,yp,template.InitPosition.z);
					temp = tempObject.GetComponent<MapItem>();
					temp.Coor = new Coordinate(i, j);
					temp.Init(i+"|"+j);
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
		UIEventListener.Get (door.gameObject).onClick = ClickDoor;
	}

	public override void HideUI ()
	{
		base.HideUI ();
		for (int i = 0; i < useMapItem.Count; i++) {
			useMapItem[i].Reset();
				}
		useMapItem.Clear ();
		gameObject.SetActive (false);
		door.HideUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
//		MapConfig mc = ModelManager.Instance.GetData(ModelEnum.MapConfig,new ErrorMsg()) as MapConfig;
//		ReachMapItem (new Coordinate (mc.characterInitCoorX, mc.characterInitCoorY));
		door.ShowUI ();
		gameObject.SetActive (true);
		StartMap ();
	}

	void ClickDoor(GameObject go) {
		if (prevMapItem.Coor.x == MapConfig.endPointX && prevMapItem.Coor.y == MapConfig.endPointY) {
			bQuest.ClickDoor();
		}
	}
	  
	void OnClickMapItem(GameObject go) {
		if (!waitMove) {
			temp = go.GetComponent<MapItem>();
			bQuest.TargetItem(temp.Coor);
		}
	}

	public Vector3 GetPosition(int x, int y) {
		if(x > map.GetLength(0) || y > map.GetLength(1))
			return Vector3.zero;
		return map[x, y].transform.position;
	}

	public bool ReachMapItem(Coordinate coor) {
		prevMapItem = map[coor.x,coor.y];
		ChangeStyle(coor);
		if(!useMapItem.Contains(prevMapItem)) {
			useMapItem.Add(prevMapItem);
			prevMapItem.IsOld = true;
			return false;
		}

		return true;
	}
	private Callback callback = null;

	public void RotateAnim(Callback cb) {
		MsgCenter.Instance.AddListener (CommandEnum.RotateDown, RotateDown);
		callback = cb;
		prevMapItem.RotateAnim ();
	}

	public void ShowBox() {
		prevMapItem.ShowBox ();
	}

	void RotateDown(object data) {
		MsgCenter.Instance.RemoveListener (CommandEnum.RotateDown, RotateDown);
		if (callback != null) {
			callback ();	
		}
		callback = null;
	}

	void ChangeStyle(Coordinate coor) {
		if(prevAround.Count > 0) {
			for (int i = 0; i < prevAround.Count; i++) {
				prevAround[i].Around(false);
			}

			prevAround.Clear();
		}

		if(coor.x > 0)
			DisposeAround(map[coor.x - 1,coor.y]);

		if(coor.x < map.GetLength(0) - 1)
			DisposeAround(map[coor.x + 1,coor.y]);

		if(coor.y > 0)
			DisposeAround(map[coor.x,coor.y - 1]);

		if(coor.y < map.GetLength(1) - 1)
			DisposeAround(map[coor.x,coor.y + 1]);
	}

	void DisposeAround(MapItem item)
	{
		if(!item.IsOld)
		{
			item.Around(true);
			prevAround.Add(item);
		}
	}
}
