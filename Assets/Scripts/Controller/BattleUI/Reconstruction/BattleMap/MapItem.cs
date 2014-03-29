using UnityEngine;
using System.Collections.Generic;

public class MapItem : UIBaseUnity {
	private Coordinate coor; 
	public Coordinate Coor {
		get{ return coor; }
		set{ coor = value; }
	}
	private FloorRotate floorRotate;
	private GameObject mapBack;
	private UISprite mapBackSprite;
	private UISprite mapItemSprite;
	List<UISprite> showStarSprite = new List<UISprite>();
	UISprite[] allStarSprite = new UISprite[7];
	
	string spriteName = "";
	string backSpriteName = "";

	private Vector3 initPosition = Vector3.zero;
	private Vector3 initRotation = Vector3.zero;

	private TQuestGrid gridItem ;

	public int  Width {
		get{ return mapItemSprite.width; }
	}
		
	public int Height {
		get{return mapItemSprite.height;}
	}

	public Vector3 InitPosition {
		get { return transform.localPosition; }
	}

	private bool isOld = false;
	public bool IsOld {
		set {
			isOld = value; 
		}
		get{return isOld;}
	}

	private bool isRotate = false;

	public Vector3 GetBoxPosition () {
		return floorRotate.currentPoint;
	}

	private UITexture alreayQuestTexture;
	public override void Init (string name) {
		base.Init (name);
		initPosition = transform.localPosition;
		initRotation = transform.rotation.eulerAngles;
		mapBackSprite = FindChild<UISprite>("Floor/Texture");
		mapBack = mapBackSprite.gameObject;
		mapItemSprite = FindChild<UISprite>("Sprite");
//		mapItemSprite.enabled = false;

		floorRotate = GetComponent<FloorRotate> ();
		floorRotate.Init ();
		if (name == "SingleMap") {
			return;
		}
		string[] info = name.Split('|');
		int x = System.Int32.Parse (info[0]);
		int y = System.Int32.Parse (info [1]);
		gridItem = BattleQuest.questDungeonData.GetSingleFloor (new Coordinate (x, y));

		InitStar ();

		if (gridItem != null) {
			switch (gridItem.Star) {
			case bbproto.EGridStar.GS_KEY:
				spriteName = "key";
				break;
			case bbproto.EGridStar.GS_QUESTION:
				spriteName = "key";
				break;
			case bbproto.EGridStar.GS_EXCLAMATION:
				spriteName = "d";
				break;
			default:
				spriteName = "";
				break;
			}
		
			DGTools.ShowSprite(mapItemSprite, spriteName);

			backSpriteName = "";
			switch (gridItem.Type) {
			case bbproto.EQuestGridType.Q_NONE:
				break;
			case bbproto.EQuestGridType.Q_KEY:
				break;
			case bbproto.EQuestGridType.Q_EXCLAMATION:
				break;
			case bbproto.EQuestGridType.Q_ENEMY:
				if(gridItem.Enemy.Count > 0) {
					uint unitID = gridItem.Enemy [0].UnitID;
					TUnitInfo tui = DataCenter.Instance.GetUnitInfo (unitID);
					if (tui != null) {
						UITexture tex = mapBack.AddComponent<UITexture>();
						tex.depth = -1;
						Destroy(mapBackSprite);
						tex.mainTexture = tui.GetAsset (UnitAssetType.Avatar);
						tex.width = 110;
						tex.height = 110;
					}
				} 
				break;
			case bbproto.EQuestGridType.Q_TRAP:
				backSpriteName = TrapBase.GetTrapSpriteName(gridItem.TrapInfo);
				break;
			case bbproto.EQuestGridType.Q_TREATURE:
				backSpriteName = "s";
				break;
			default:
				break;
			}
			if(mapBackSprite != null) {
				mapBackSprite.spriteName = backSpriteName;
			}

			mapBack.SetActive(false);
		}
	}

	void HideStarSprite (bool show) {
		for (int i = 1; i < 8; i++) {
			if(allStarSprite[i - 1] == null) {
				UISprite temp = FindChild<UISprite>("star" + i);
				temp.enabled = show;
				allStarSprite[i - 1] = temp;
			}
			else{
				allStarSprite[i - 1].enabled = show;
			}
		}
	}

	void HideShowSprite (bool show) {
		foreach (var item in showStarSprite) {
			item.enabled = show;
		}
	}

	void InitStar () {
		if (gridItem == null) {
			HideStarSprite(false);
			return;
		}
		List<int> spriteIndex = GetSpritIndex ();
		string spriteName = GetStarSpriteName ();
		for (int i = 1; i < 8; i++) {
			UISprite tmep = FindChild<UISprite>("star" + i);
			int index = i - 1;
			if(spriteIndex.Contains(index)) {
				tmep.spriteName = spriteName;
				showStarSprite.Add(tmep);
			}
			else {
				tmep.spriteName = "";
			}
		}

		HideShowSprite (false);
	}

	GameObject floorObject = null;

	public void ShowObject(GameObject go) {
		GameObject parent = transform.Find("Floor").gameObject;
		floorObject = Instantiate (go) as GameObject;
		floorObject.transform.parent = parent.transform;
		floorObject.transform.localPosition = Vector3.zero;
		floorObject.SetActive (true);
	}

	public override void ShowUI() {
		isOld = false;
	}

	public void HideEnvirment(bool b) {
		if (!isOld) {
			HideStarSprite(!b);
			if(b) {
				DGTools.ShowSprite(mapItemSprite,"EnvirmentTrap"); // "EnvirmentTrap" is hide envirment sprite name.
//				mapItemSprite.spriteName = "EnvirmentTrap";
			}else{
				DGTools.ShowSprite(mapItemSprite,spriteName);
//				mapItemSprite.spriteName = spriteName;
			}
		}
	}

	public void RotateOneCircle() {
		if (!isRotate) {
			isRotate = true;
			floorRotate.RotateOne ();
		}
	}

	public void RotateAnim() {
		if (!isRotate) {
			isRotate = true;
			floorRotate.RotateFloor (RotateEnd);	
			if(!mapBack.activeSelf) {
				mapBack.SetActive(true);
			}
			if(mapItemSprite.enabled) {
				mapItemSprite.enabled = false;
			}
		}
	}     

	void RotateEnd () {
		mapBack.SetActive(false);
		mapItemSprite.enabled = false;
		floorObject.SetActive (false);
		HideStarSprite (false);
	}

	public void ShowBox() {
		floorRotate.isShowBox = true;
	}

	public void Reset () {
		gameObject.transform.localPosition = initPosition;
		gameObject.transform.rotation = Quaternion.Euler (initRotation);
	}

	int countShow = -1;
	public void Around(bool isAround) {
		if(isOld)
			return;
	
		if (isAround) {
			HideShowSprite(true);

			countShow++;
			if (countShow == 3) {
					countShow = 0;
			}

			string name = GetStarSpriteName ();
			for (int i = 0; i < showStarSprite.Count; i++) {
					showStarSprite [i].spriteName = name;
			}	
		} 
		else {
			HideShowSprite(false);	
		}

	}

	string GetStarSpriteName() {
		if (countShow == -1) {
			countShow = DGTools.RandomToInt(0, 3);	
		}
		string name = "";
		switch (countShow) {
		case 0:
			name = "8";	// 8 == blue
			break;
		case 1:
			name = "10"; // 10 == yellow
			break;
		case 2:
			name = "9"; // 9 == red
			break;
		}

		return name;
	}
	 
	List<int> GetSpritIndex () {
		List<int> index = new List<int> ();
		switch (gridItem.Star) {
		case  bbproto.EGridStar.GS_STAR_1:
			index.Add(2);
			break;
		case bbproto.EGridStar.GS_STAR_2:
			index.Add(6);
			index.Add(7);
			break;
		case bbproto.EGridStar.GS_STAR_3:
			index.Add(0);
			index.Add(2);
			index.Add(4);
			break;
		case bbproto.EGridStar.GS_STAR_4:
			index.Add(0);
			index.Add(1);
			index.Add(3);
			index.Add(4);
			break;
		case bbproto.EGridStar.GS_STAR_5:
			index.Add(0);
			index.Add(1);
			index.Add(2);
			index.Add(3);
			index.Add(4);
			break;
		case bbproto.EGridStar.GS_STAR_6:
			index.Add(0);
			index.Add(1);
			index.Add(3);
			index.Add(4);
			index.Add(5);
			index.Add(6);
			break;
		}

		return index;
	}
}
