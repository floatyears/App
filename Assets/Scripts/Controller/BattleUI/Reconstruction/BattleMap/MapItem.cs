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
	private FloorRotate floorRotate;
	private UISprite mapItemSprite;
	string spriteName = "";

	private Vector3 initPosition = Vector3.zero;
	private Vector3 initRotation = Vector3.zero;

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

	public Vector3 GetBoxPosition () {
		return floorRotate.currentPoint;
	}

	private UITexture alreayQuestTexture;
	public override void Init (string name) {
		base.Init (name);
		initPosition = transform.localPosition;
		initRotation = transform.rotation.eulerAngles;
		mapItemTexture = FindChild<UITexture>("Floor/MapItem");
		mapItemSprite = FindChild<UISprite>("Sprite");
		floorRotate = GetComponent<FloorRotate> ();
		floorRotate.Init ();

		if (name == "SingleMap") {
			return;
		}

		string[] info = name.Split('|');
		int x = System.Int32.Parse (info[0]);
		int y = System.Int32.Parse (info [1]);
		SingleMapData smd = BattleQuest.mapConfig.mapData [x, y];
		if (smd.ContentType == MapItemEnum.key) {
			spriteName = "key";
//			mapItemSprite.spriteName = "key";	
		} else if(smd.ContentType == MapItemEnum.Exclamation){
			spriteName = "d";
//			mapItemSprite.spriteName = "d";	
		} else{
//			mapItemSprite.spriteName = "";	
		}
		mapItemSprite.spriteName = spriteName;
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

	public void RotateAnim() {
		floorRotate.RotateFloor ();
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
