using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MapItem : MonoBehaviour {
	private Coordinate coor; 
	public Coordinate Coor {
		get{ return coor; }
		set{ coor = value; }
	}

	private GameObject mapBack;
	private GameObject effectPanel;
	private UISprite mapBackSprite;
	private UITexture mapBackTexture;
	private UISprite mapItemSprite;
	private UISprite gridItemSprite;
	private UISprite footTips;
	List<UISprite> showStarSprite = new List<UISprite>();
	UISprite[] allStarSprite = new UISprite[7];
	
	string spriteName = "";
	string backSpriteName = "";

	private Vector3 initPosition = Vector3.zero;
	private Vector3 initRotation = Vector3.zero;

	private TQuestGrid gridItem;

	public int  Width {
		get{ return mapItemSprite.width; }
	}
		
	public int Height {
		get{return mapItemSprite.height;}
	}

	public Vector3 InitPosition {
		get { return transform.localPosition; }
	}

	private bool _hasBeenReached = false;
	public bool hasBeenReached {
		set {
			_hasBeenReached = value; 
		}
		get{
			return _hasBeenReached;
		}
	}

	private bool isRotate = false;
	private UITexture alreayQuestTexture;

	public void Init (string name)
	{
//	}
//		base.Init (name);
		gameObject.name = name;
		initPosition = transform.localPosition;
		initRotation = transform.rotation.eulerAngles;
		gridItemSprite = transform.FindChild("GridBackground").GetComponent<UISprite>();
		footTips = transform.FindChild("FootTips").GetComponent<UISprite>();
		footTips.gameObject.SetActive (false);
		mapBackSprite = transform.FindChild("Shadow").GetComponent<UISprite>();
		mapBack = mapBackSprite.gameObject;
		mapItemSprite = transform.FindChild("Sprite").GetComponent<UISprite>();
		effectPanel = transform.FindChild("Effect").gameObject;

		if (name == "SingleMap") {
			mapBackSprite.spriteName = string.Empty;
			mapItemSprite.spriteName = string.Empty;
			return;
		}

		string[] info = name.Split('|');
		int x = System.Int32.Parse (info[0]);
		int y = System.Int32.Parse (info [1]);
		gridItem = BattleConfigData.Instance.questDungeonData.GetFloorDataByCoor (new Coordinate (x, y));
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
				if(mapBackSprite != null) {
					mapBackSprite.spriteName = backSpriteName;
				}
				break;
			case bbproto.EQuestGridType.Q_KEY:
				if(mapBackSprite != null) {
					mapBackSprite.spriteName = backSpriteName;
				}
				break;
			case bbproto.EQuestGridType.Q_EXCLAMATION:
				if(mapBackSprite != null) {
					mapBackSprite.spriteName = backSpriteName;
				}
				break;
			case bbproto.EQuestGridType.Q_ENEMY:
				if(gridItem.Enemy.Count != 0) {
					uint unitID = gridItem.Enemy [0].UnitID;
					TUnitInfo tui = DataCenter.Instance.GetUnitInfo (unitID);
					if (tui != null) {
						ResourceManager.Instance.GetAvatarAtlas(tui.ID, mapBackSprite);
					}
				}
				break;
			case bbproto.EQuestGridType.Q_TRAP:
//				backSpriteName = gridItem.TrapInfo.GetTrapSpriteName();
				if(mapBackSprite != null) {
					mapBackSprite.spriteName = gridItem.TrapInfo.GetTrapSpriteName();
				}
				break;
			case bbproto.EQuestGridType.Q_TREATURE:
//				backSpriteName = BattleMap.chestSpriteName; //"S";
				if(mapBackSprite != null) {
					mapBackSprite.spriteName = BattleMapView.chestSpriteName;
				}
				break;
			default:
				if(mapBackSprite != null) {
					mapBackSprite.spriteName = backSpriteName;
				}
				break;
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
				UISprite temp = transform.FindChild("star" + i).GetComponent<UISprite>();
				temp.enabled = show;
				allStarSprite[i - 1] = temp;
			} else{
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
			UISprite tmep = transform.FindChild("star" + i).GetComponent<UISprite>();
			int index = i - 1;

			if(spriteIndex.Contains(index)) {
				tmep.spriteName = spriteName;
				showStarSprite.Add(tmep);
			} else {
				tmep.spriteName = "";
			}
		}

		HideShowSprite (false);
	}

	GameObject floorObject = null;

	public void ShowObject(GameObject go) { }

	void OnDisable () { }

	void OnEnable () {
		if (gridItemSprite == null) {
			return;		
		}

		if (_hasBeenReached) {
			return;	
		}

		ShowFootTips ();
	}

	public void HideEnvirment(bool hide) {
		if (!_hasBeenReached) {
			if(hide && mapItemSprite.spriteName != TrapBase.environmentSpriteName) {
				DGTools.ShowSprite(mapItemSprite, TrapBase.environmentSpriteName);
				return;
			}

			if(!hide && mapItemSprite.spriteName == TrapBase.environmentSpriteName){
				DGTools.ShowSprite(mapItemSprite, spriteName);
				return;
			}
		}
	}

	public void RotateSingle(Callback cb) {
		animEnd = cb;

//		gameObject.SetActive (true);
		EffectManager.Instance.GetMapEffect (gridItem.Type, returnValue => {
			StartCoroutine (MeetEffect (returnValue)); }
		);
	}

	public const string rotateAllEnd = "HideGrid";
	public const string rotateSingleEnd = "RotateEnd";
	public void RotateAll(Callback cb, bool allShow) {
		animEnd = cb;
		if (_hasBeenReached && allShow) {
			ShowBattleEnd( rotateAllEnd );
		}
		else{
			GridAnim ( rotateAllEnd );
		}
	}
	
	IEnumerator MeetEffect (Object obj) {
		if (gridItem == null) {
			GridAnim (rotateSingleEnd);	
			yield break;
		}

		if (obj == null) {
//			yield return 0;
			GridAnim (rotateSingleEnd);	
		} else {
			GameObject effect = obj as GameObject;
			GameObject go = NGUITools.AddChild(effectPanel, effect);
			go.transform.localScale = new Vector3(100f,100f,1f);
			yield return new WaitForSeconds(0.5f);
			Destroy(go);
			GridAnim (rotateSingleEnd);	
		}
	}

	public void HideGridNoAnim() {
//		Debug.Log ("hide no anim");
		hasBeenReached = true;
		HideShowSprite (false);
		mapBackSprite.enabled = gridItemSprite.enabled = mapItemSprite.enabled = false;
	}

	void ShowBattleEnd(string funciton) {
		GameObject go = null;
		if (mapBackSprite == null) {
			if(mapBackTexture != null && !mapBackTexture.gameObject.activeSelf) {
				go = mapBackTexture.gameObject;
				go.SetActive(true);
				mapBackTexture.enabled = true;
			}
		} else if (!mapBackSprite.gameObject.activeSelf) {
			go = mapBackSprite.gameObject;
			go.SetActive(true);
			mapBackSprite.enabled = true;
		}
		if (go == null) {
			return;	
		}
		TweenAlpha ta = go.GetComponent<TweenAlpha> ();
		ta.enabled = true;
		ta.ResetToBeginning ();

		if(animEnd != null) {
			Invoke(funciton, 0.5f);
		}	
	}

	Callback animEnd;

	List<GameObject> gridAnim = new List<GameObject> ();

	public void GridAnim(string function) {
		if (_hasBeenReached) {
			if(animEnd != null) {
				Invoke(function, 0.5f);
			}
			return;
		}
			
		hasBeenReached = true;
		showStarSprite.Clear ();
		float time = 0.5f;

		if(!mapBack.activeSelf) {
			mapBack.SetActive(true);
		}

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
			tws.ResetToBeginning ();
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

		if (gridItem != null &&  gridItem.Star != bbproto.EGridStar.GS_KEY && gridItem.Type == bbproto.EQuestGridType.Q_TREATURE && function != rotateAllEnd) {
			UILabel coinLabel = transform.FindChild("CoinLabel").GetComponent<UILabel>();//FindChild<UILabel>("CoinLabel");
			coinLabel.text = gridItem.Coins.ToString();
			flyCoin = coinLabel.gameObject;
			flyCoin.SetActive(true);
			flyCoin.SetActive (true);
			Destroy (flyCoin.GetComponent<TweenScale> ());
			Destroy (flyCoin.GetComponent<TweenAlpha> ());
			Vector3 endPosition = ModuleManager.Instance.GetModule<BattleTopModule>(ModuleEnum.BattleTopModule).GetCoinPos();//battleMap.bQuest.GetTopUITarget ().position;
			callBack = function;
			float flyTime = Vector3.Distance(flyCoin.transform.position, endPosition) / 1.5f; // 1f = fly speed.
			iTween.MoveTo (flyCoin, iTween.Hash ("position", endPosition, "oncompletetarget", gameObject, "oncomplete", "FlyEnd", "time", flyTime, "easetype", iTween.EaseType.easeInQuad));
		} else {
			tws.callWhenFinished = function;
		}
	}     

	string callBack = string.Empty;
	GameObject flyCoin = null;

	void FlyEnd() {
		Destroy (flyCoin);
		Invoke (callBack, 0f);
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

		if (animEnd != null) {
			animEnd ();	
		}
	}

	public void Reset () {
		gameObject.transform.localPosition = initPosition;
		gameObject.transform.rotation = Quaternion.Euler (initRotation);
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
				float temp = 0.3f;
				if(isLockAttack) {
					temp = 0.01f;
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

	}

	public bool GetChainLinke() {
		if (_hasBeenReached) {
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
		footTips.gameObject.SetActive (isAround);
		ShowFootTips ();
		if(_hasBeenReached)
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
			countShow = DGTools.RandomToInt(0, 3);
		}
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

	public bool IsKey(){
		return mapItemSprite.spriteName == "key";
	}
}
