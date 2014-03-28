using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleCardArea : UIBaseUnity {
	private UITexture backTexture;
	private BattleCardAreaItem[] battleCardAreaItem;

	private UILabel stateLabel;
	private Vector3 sourcePosition;
	private const int oneWordSize = 42;

	private Battle battle;
	public Battle BQuest {
		set{ battle = value; }
	}
	private Dictionary<int,List<CardItem>> battleAttack = new Dictionary<int, List<CardItem>>();
	private static List<GameObject> battleCardIns = new List<GameObject>();
	private GameObject cardItem;

	public static Vector3 startPosition;
	public static Vector3 endPosition;

	public override void Init (string name) {
		base.Init (name); 
		backTexture = FindChild<UITexture>("Back"); 
		backTexture.gameObject.SetActive(false);
		stateLabel = FindChild<UILabel>("StateLabel");
		stateLabel.enabled = false;
		if (cardItem == null) {
			GameObject go = LoadAsset.Instance.LoadAssetFromResources (Config.battleCardName, ResourceEuum.Prefab) as GameObject;
			cardItem = go.transform.Find("Texture").gameObject;
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		for (int i = 0; i < battleCardAreaItem.Length; i++)  {
			battleCardAreaItem[i].ShowUI();
		}
		MsgCenter.Instance.AddListener (CommandEnum.StateInfo, StateInfo);
	}

	public override void HideUI () {
		base.HideUI ();
		for (int i = 0; i < battleCardAreaItem.Length; i++)  {
			battleCardAreaItem[i].HideUI();
		}
		gameObject.SetActive (false);
		MsgCenter.Instance.RemoveListener (CommandEnum.StateInfo, StateInfo);
	}

	void StateInfo(object data) {
		string info = (string)data;
		stateLabel.text = info;
	}

	public void CreatArea(Vector3[] position,int height) {
		if(position == null)
			return;
		battleCardAreaItem = new BattleCardAreaItem[position.Length];
		float xOffset = backTexture.width * -0.5f;
		float yOffset = backTexture.height * 0.5f + stateLabel.height + height;
		stateLabel.transform.localPosition = position [0] + new Vector3 (xOffset, yOffset, 0f);
		stateLabel.enabled = true;
		int length = position.Length;
		for (int i = 0; i < length; i++) {
			tempObject = NGUITools.AddChild(gameObject,backTexture.gameObject);
			tempObject.SetActive(true);
			tempObject.layer = GameLayer.BattleCard;
			tempObject.transform.localPosition = new Vector3(position[i].x,position[i].y + height,position[i].z) ;
			BattleCardAreaItem bca = tempObject.AddComponent<BattleCardAreaItem>();
			bca.Init(tempObject.name);
			bca.AreaItemID = i;
			battleCardAreaItem[i] = bca;
		}

		BattleCardAreaItem bcai = battleCardAreaItem [length - 1];
		Vector3 pos = bcai.transform.localPosition;
		startPosition = new Vector3 (pos.x + height, pos.y - height / 2f, pos.z);
		pos = battleCardAreaItem [0].transform.localPosition;
		endPosition = new Vector3 (pos.x - height * 0.5f, pos.y - height * 1.5f, pos.z);
	}
	
	static int count = 0;
	public static GameObject GetCard() {
		if (count == battleCardIns.Count) {
			count = 0;
		}
		GameObject go = battleCardIns [count];
		count ++;
		return go;
	}
	
	bool showCountDown = false;
	int time = 0;
	public void ShowCountDown (bool isShow,int time) {
		showCountDown = isShow;
		this.time = time;
	}
}
