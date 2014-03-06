using UnityEngine;
using System.Collections;

public class MapItem : UIBaseUnity
{
	private Coordinate coor; 
	public Coordinate Coor {
		get{ return coor; }
		set{ coor = value; }
	}
	private UITexture mapItemTexture;
	private UISprite mapBackSprite;

	private FloorRotate floorRotate;
	private UISprite mapItemSprite;
	string spriteName = "";

	private Vector3 initPosition = Vector3.zero;
	private Vector3 initRotation = Vector3.zero;

	private TQuestGrid gridItem ;

	public int  Width {
		get{ return mapItemTexture.width; }
	}
		
	public int Height {
		get{return mapItemTexture.height;}
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
		mapItemTexture = FindChild<UITexture>("Floor/MapItem/Texture");
		mapBackSprite = mapItemTexture.GetComponent<UISprite> ();
		mapItemTexture.enabled = false;
		mapBackSprite.enabled = false;
		mapItemSprite = FindChild<UISprite>("Sprite");
		floorRotate = GetComponent<FloorRotate> ();
		floorRotate.Init ();
		if (name == "SingleMap") {
			return;
		}
		string[] info = name.Split('|');
		int x = System.Int32.Parse (info[0]);
		int y = System.Int32.Parse (info [1]);
		gridItem = BattleQuest.questDungeonData.GetSingleFloor (new Coordinate (x, y));
		if (gridItem != null) {
//			if (gridItem.Type == bbproto.EQuestGridType.Q_KEY) {
//				spriteName = "key";
//			} else if (gridItem.Type == bbproto.EQuestGridType.Q_EXCLAMATION) {
//				spriteName = "d";
//			} else if (gridItem.Type == bbproto.EQuestGridType.Q_ENEMY) {
//				uint unitID = gridItem.Enemy [0].UnitID;
//				TUnitInfo tui = GlobalData.Instance.GetUnitInfo (unitID);
//				if (tui != null) {
//					mapItemTexture.enabled = true;
//					mapItemTexture.mainTexture = tui.GetAsset (UnitAssetType.Avatar);
//				}
//			}
//			else if(){
//
//			}
			switch (gridItem.Star) {
			case bbproto.EGridStar.GS_KEY:
				spriteName = "key";
				break;
			case bbproto.EGridStar.GS_QUESTION:
				break;
			case bbproto.EGridStar.GS_EXCLAMATION:
				spriteName = "d";
				break;
			default:
				break;
			}
			mapItemSprite.spriteName = spriteName;
			switch (gridItem.Type) {
			case bbproto.EQuestGridType.Q_KEY:

				break;
			case bbproto.EQuestGridType.Q_EXCLAMATION:

				break;
			case bbproto.EQuestGridType.Q_ENEMY:
				uint unitID = gridItem.Enemy [0].UnitID;
				TUnitInfo tui = GlobalData.Instance.GetUnitInfo (unitID);
				if (tui != null) {
					mapItemTexture.enabled = true;
					mapItemTexture.mainTexture = tui.GetAsset (UnitAssetType.Avatar);
				}
				break;
			case bbproto.EQuestGridType.Q_TRAP:
				mapBackSprite.enabled = true;
				mapBackSprite.spriteName = TrapBase.GetTrapSpriteName(gridItem.TrapInfo);
				break;
			case bbproto.EQuestGridType.Q_TREATURE:
				mapBackSprite.spriteName = "s";
				break;
			default:
				break;
			}
		}
	}

	public override void ShowUI() {
		isOld = false;

		//mapItemTexture.color = Color.white;
	}

	public void HideEnvirment(bool b) {
//		Debug.Log ("isOld : " + isOld + " b : " + b);
		if (!isOld) {
			if(b) {
				mapItemSprite.spriteName = "6";
			}else{
				mapItemSprite.spriteName = spriteName;
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
			floorRotate.RotateFloor ();	
		}
	}     

	public void ShowBox() {
		floorRotate.isShowBox = true;
	}

	public void Reset () {
		gameObject.transform.localPosition = initPosition;
		gameObject.transform.rotation = Quaternion.Euler (initRotation);
	}

	public void Around(bool isAround)
	{
		if(isOld)
			return;

//		if(isAround)
//			mapItemTexture.color = Color.yellow;
//		else
//			mapItemTexture.color = Color.white;
	}
}
