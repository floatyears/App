using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MapItem : UIBaseUnity {
	private Coordinate coor; 
	public Coordinate Coor {
		get{ return coor; }
		set{ coor = value; }
	}

	private GameObject mapBack;
	private GameObject effectPanel;
	private UISprite mapBackSprite;
	private UISprite mapItemSprite;
	private UISprite gridItemSprite;
	private UISprite footTips;
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
	private UITexture alreayQuestTexture;

	public override void Init (string name) {
		base.Init (name);
		initPosition = transform.localPosition;
		initRotation = transform.rotation.eulerAngles;
		gridItemSprite = FindChild<UISprite>("GridBackground");
		footTips = FindChild<UISprite> ("FootTips");
		footTips.gameObject.SetActive (false);
		mapBackSprite = FindChild<UISprite>("Shadow");
		mapBack = mapBackSprite.gameObject;
		mapItemSprite = FindChild<UISprite>("Sprite");
		effectPanel = FindChild<UIPanel> ("Effect").gameObject;

		if (name == "SingleMap") {
			mapBackSprite.spriteName = string.Empty;
			mapItemSprite.spriteName = string.Empty;
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
				spriteName = "gantanhao";
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
				if(gridItem.Enemy.Count != 0) {
					uint unitID = gridItem.Enemy [0].UnitID;
					TUnitInfo tui = DataCenter.Instance.GetUnitInfo (unitID);
					if (tui != null) {
						UITexture tex = mapBack.AddComponent<UITexture>();
						tex.depth = 4;
						Destroy(mapBackSprite);
						tex.mainTexture = tui.GetAsset (UnitAssetType.Avatar);
						tex.width = 104;
						tex.height = 104;
					}
				}
				break;
			case bbproto.EQuestGridType.Q_TRAP:
				backSpriteName = TrapBase.GetTrapSpriteName(gridItem.TrapInfo);
				break;
			case bbproto.EQuestGridType.Q_TREATURE:
				backSpriteName = "S";
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

	void OnDestory() {
		showStarSprite.Clear ();
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

		for (int i = showStarSprite.Count - 1; i >= 0; i--) {
			if(showStarSprite[i] == null) {
				showStarSprite.RemoveAt(i);
			}
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
//		GameObject parent = transform.Find("Floor").gameObject;
//		floorObject = Instantiate (go) as GameObject;
//		floorObject.transform.parent = parent.transform;
//		floorObject.transform.localPosition = Vector3.zero;
//		floorObject.SetActive (true);
	}

	void OnDisable () {

	}

	void OnEnable () {
		if (gridItemSprite == null) {
			return;		
		}
		TweenAlpha ta = gridItemSprite.GetComponent<TweenAlpha>();
		ta.Reset ();
		ShowFootTips ();
	}

	public override void ShowUI() {
		isOld = false;
	}

	public void HideEnvirment(bool b) {
		if (!isOld) {
			HideStarSprite(!b);
			if(b) {
				DGTools.ShowSprite(mapItemSprite, "EnvirmentTrap"); 
			}else{
				DGTools.ShowSprite(mapItemSprite, spriteName);
			}
		}
	}

	public void RotateSingle(Callback cb) {
		animEnd = cb;
		StartCoroutine (MeetEffect ());
	}

	public void RotateAll(Callback cb) {
		animEnd = cb;
		GridAnim ( "AllRotateEnd");
	}
	
	IEnumerator MeetEffect () {
		if (gridItem == null) {
			GridAnim ("RotateEnd");	
			yield break;
		}
		Object obj = DataCenter.Instance.GetMapEffect (gridItem.Type.ToString ());
		if (obj == null) {
			yield return 0;
			GridAnim ("RotateEnd");	
		} else {
			GameObject effect = obj as GameObject;
			GameObject go = NGUITools.AddChild(effectPanel, effect);
			go.transform.localScale = new Vector3(100f,100f,1f);
			yield return new WaitForSeconds(0.5f);
			Destroy(go);
			GridAnim ("RotateEnd");	
		}
	}
	
	Callback animEnd;
	List<GameObject> gridAnim = new List<GameObject> ();
	public void GridAnim(string function) {
		if (isOld) {
			if(animEnd != null) {
				Invoke(function, 0.5f);
			}	
			return;
		}
			
		isOld = true;
		showStarSprite.Clear ();

		if(!mapBack.activeSelf) {
			mapBack.SetActive(true);
		}

		float time = 0.5f;
		GameObject go = gridItemSprite.gameObject;
		go.GetComponent<TweenAlpha> ().enabled = false;
		for (int i = 0; i < 3; i++) {
			GameObject temp = NGUITools.AddChild(go.transform.parent.gameObject, go);
			TweenAlpha ta = temp.GetComponent<TweenAlpha> ();
			ta.enabled = true;
			ta.duration =time;
			ta.delay = 0.15f * i;
			ta.style = UITweener.Style.Once;

			TweenScale ts = gridItemSprite.GetComponent<TweenScale> ();
			ts.enabled = true;
			ts.duration = time;
			ts.delay = 0.15f * i;
			ts.to = Vector3.one * 2f;
			gridAnim.Add(temp);

		}

		go.SetActive (false);
		TweenScale tws = gridAnim [2].GetComponent<TweenScale> ();
		TweenAlpha twa = mapBack.GetComponent<TweenAlpha> ();
		twa.enabled = true;
		twa.duration = time;
		if (!string.IsNullOrEmpty (mapItemSprite.spriteName)) {
			tws = mapItemSprite.GetComponent<TweenScale> ();
			tws.Reset ();
			tws.style = UITweener.Style.Once;
			tws.duration = time;
			tws.to = new Vector3 (2f, 2f, 2f);
			
			twa = mapItemSprite.GetComponent<TweenAlpha> ();
			twa.enabled = true;
			twa.duration = time;	
		}
		tws = mapBack.GetComponent<TweenScale> ();
		tws.enabled = true;
		tws.duration = time;
		tws.eventReceiver = gameObject;
		tws.callWhenFinished = function;
	}     

	void EndCallback () {
		if (animEnd != null) {
			animEnd ();	
		}
	}

	void AllRotateEnd () {
		HideGrid ();
	}

	void RotateEnd () {
		mapBack.SetActive(false);
		HideGrid ();
	}

	void HideGrid () {
		for (int i = gridAnim.Count - 1; i >= 0; i--) {
			Destroy( gridAnim[i]);
		}
		gridAnim.Clear ();
		mapItemSprite.enabled = false;
		HideStarSprite (false);
		gridItemSprite.gameObject.SetActive (false);

		EndCallback ();
	}

	public void ShowGrid() {

	}

	public void Reset () {
		gameObject.transform.localPosition = initPosition;
		gameObject.transform.rotation = Quaternion.Euler (initRotation);
	}

	public void EnemyAttack () {

	}

	public bool isLockAttack = false;
	public EnemyAttackEnum TriggerAttack() {
		EnemyAttackEnum eae = EnemyAttackEnum.None;
		switch (countShow) {
			case 0:
				eae = EnemyAttackEnum.FirstAttack;
				break;
			case 1:
				eae = EnemyAttackEnum.None;
				break;
			case 2:
				float value = DGTools.RandomToFloat();
				float temp = 1f;
				if(isLockAttack) {
					temp =0.01f;
				}
				if(value <= temp) {
					eae = EnemyAttackEnum.BackAttack;
				}else{
					eae = EnemyAttackEnum.None;
				}
				break;
		}
		return eae;
	}

	int countShow = -1;
	void ShowFootTips () {
		if (!footTips.gameObject.activeSelf) {
			return;		
		}
		TweenAlpha ta = gridItemSprite.GetComponent<TweenAlpha> ();
		TweenAlpha currentTa = footTips.GetComponent<TweenAlpha> ();
		ta.Reset ();
		currentTa.alpha = ta.alpha;
		currentTa.duration = ta.duration;
		currentTa.from = ta.from;
		currentTa.to = ta.to;
	}

	public bool GetChainLinke() {
		if (isOld) {
			return false;	
		}

		if (countShow == 2 && gridItem.Type == bbproto.EQuestGridType.Q_ENEMY) {
			return true;
		}

		return false;
	}

	public void AddSecurityLevel() {
		if(countShow < 2) {
			countShow++;
			string name = GetStarSpriteName ();
			for (int i = 0; i < showStarSprite.Count; i++) {
				showStarSprite [i].spriteName = name;
			}
		}
	}

	public void Around(bool isAround) {
//		Debug.LogError (" gameobject : " + gameObject + "aroungd : " + footTips);
		footTips.gameObject.SetActive (isAround);
		ShowFootTips ();
		if(isOld)
			return;
		if (isAround) {
			HideShowSprite(true);
		}
		else {
			HideShowSprite(false);	
		}
	}

	string GetStarSpriteName() {
		if (countShow == -1) {
			countShow = DGTools.RandomToInt(2, 3);	
		}
//		Debug.LogError (" GetStarSpriteName: " + countShow);
		string name = "";
		switch (countShow) {
		case 0:
			name = "8";	// 8 == blue
			break;
		case 1:
			name = "9"; // 9 == yellow
			break;
		case 2:
			name = "10"; // 1 == red
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
			index.Add(5);
			index.Add(6);
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
