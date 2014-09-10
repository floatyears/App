using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleManipulationView : ViewBase {
	private UISprite backTexture;
	[HideInInspector]
	public BattleCardAreaItem[] battleCardAreaItem;
	private UILabel stateLabel;
	private Vector3 sourcePosition;
	private const int oneWordSize = 42;

//	private FightManipulationModule battle;
//	public FightManipulationModule BQuest {
//		set{ battle = value; }
//	}
	private Dictionary<int,List<CardItem>> battleAttack = new Dictionary<int, List<CardItem>>();
	private static List<GameObject> battleCardIns = new List<GameObject>();
	private GameObject cardItem;
	public static Vector3 startPosition;
	public static Vector3 middlePosition;
	public static Vector3 endPosition;
	public static Vector3 activeSkillStartPosition;
	public static Vector3 activeSkillEndPosition;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);

		InitParameter ();

		InitData();
		GameObject tempObject = null;
		
		for (int i = 0; i < cardPosition.Length; i++) {
			tempObject = NGUITools.AddChild(gameObject, templateBackTexture.gameObject);
			cardPosition[i] = new Vector3(initPosition.x + i * cardInterv,initPosition.y,initPosition.z);
			tempObject.transform.localPosition = cardPosition[i];
			backTextureIns[i] = tempObject.GetComponent<UISprite>();
		}
		templateBackTexture.gameObject.SetActive(false);
		tempObject = null;

		backTexture = FindChild<UISprite>("Back"); 
		backTexture.gameObject.SetActive(false);
		stateLabel = FindChild<UILabel>("StateLabel");
		stateLabel.text = string.Empty;
		if (cardItem == null) {
			ResourceManager.Instance.LoadLocalAsset ("Prefabs/" + Config.battleCardName,o=>{
				GameObject go = o as GameObject;
				cardItem = go.transform.Find("Texture").gameObject;
			});
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		for (int i = 0; i < battleCardAreaItem.Length; i++)  {
			battleCardAreaItem[i].ShowUI();
		}
		MsgCenter.Instance.AddListener (CommandEnum.StateInfo, StateInfo);
		MsgCenter.Instance.AddListener (CommandEnum.ExcuteActiveSkill, RecoverStateInfo);
	}

	public override void HideUI () {
		base.HideUI ();
		for (int i = 0; i < battleCardAreaItem.Length; i++)  {
			battleCardAreaItem[i].HideUI();
		}
		gameObject.SetActive (false);

		MsgCenter.Instance.RemoveListener (CommandEnum.StateInfo, StateInfo);
		MsgCenter.Instance.RemoveListener (CommandEnum.ExcuteActiveSkill, RecoverStateInfo);
	}

	Vector3 HidePosition;
	Vector3 showPosition;

	string prevInfo = "";

	int boostIndex = -1;

	int maxBoostRandom = 0;

	void SetBoost () {
		maxBoostRandom = NoviceGuideStepEntityManager.isInNoviceGuide() ? 5 : 10;

		if (boostIndex > -1 && boostIndex < 5) {
			battleCardAreaItem[boostIndex].isBoost = false;
		}

		boostIndex = Random.Range (0, maxBoostRandom);
		if(boostIndex < 5) 
			battleCardAreaItem[boostIndex].isBoost = true;
	}

	void StateInfo(object data) {
		string info = (string)data;
		if (string.IsNullOrEmpty (info) && !string.IsNullOrEmpty(stateLabel.text)) {
			HideStateLabel(string.Empty);
			return;
		}			

		if (info == DGTools.stateInfo [0]) {
			SetBoost();
		}

		if (stateLabel.text == info) {
			return;	
		}

		if (info == DGTools.stateInfo [4]) {
			prevInfo = stateLabel.text;
		}

		Color32[] colors;

		if (info == DGTools.stateInfo [0] || info == DGTools.stateInfo [1]) {
			colors = BattleFullScreenTipsView.thirdGroupColor;		
		} else {
			colors = BattleFullScreenTipsView.secondGroupColor;
		}

		BattleFullScreenTipsView.SetLabelGradient (stateLabel, colors);

		if (stateLabel.text == string.Empty) {
			stateLabel.transform.localPosition = HidePosition;	
			ShowStateLabel ();
		} else {
			HideStateLabel("ShowStateLabel");
		}

		stateLabel.text = info;
	}

	void RecoverStateInfo(object data) {
		bool b = (bool)data;
		if (!b) {
			StateInfo (prevInfo);
			prevInfo = "";
		}
	}

	void HideStateLabel (string nextFunction) {
		float x = showPosition.x - stateLabel.width;
		iTween.MoveTo(stateLabel.gameObject, iTween.Hash("x", x, "islocal", true,"time",0.15f,"easetype",iTween.EaseType.easeOutCubic,"oncompletetarget",gameObject,"oncomplete","ClearTexture"));
	}

	void ClearTexture () {
		stateLabel.text = string.Empty;
	}

	void ShowStateLabel () {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_text_appear);

		iTween.MoveTo (stateLabel.gameObject, iTween.Hash ("position", showPosition, "islocal", true, "time", 0.15f));
	}	       

	public void CreatArea(Vector3[] position,int height) {
		if (position == null)
				return;
		battleCardAreaItem = new BattleCardAreaItem[position.Length];
		float xOffset = backTexture.width * -0.5f;
		float yOffset = backTexture.height * 1.7f;
		stateLabel.transform.localPosition = position [0] + new Vector3 (xOffset, yOffset, 0f);
		showPosition = stateLabel.transform.localPosition;
		HidePosition = stateLabel.transform.localPosition + Vector3.right * -(stateLabel.mainTexture.width + Screen.width * 0.5f);
		stateLabel.enabled = true;
		int length = position.Length;

		GameObject tempObject = null;

		for (int i = 0; i < length; i++) {
				tempObject = NGUITools.AddChild (gameObject, backTexture.gameObject);
				tempObject.SetActive (true);
				tempObject.layer = GameLayer.BattleCard;
				tempObject.transform.localPosition = new Vector3 (position [i].x + 5f, position [i].y + 3f + height, position [i].z);
				BattleCardAreaItem bca = tempObject.AddComponent<BattleCardAreaItem> ();
//				bca.Init (tempObject.name);
				bca.AreaItemID = i;
				battleCardAreaItem [i] = bca;
		}

		BattleCardAreaItem bcai = battleCardAreaItem [length - 1];
		//normal skill is from right top to left bottom.
		Vector3 pos = bcai.transform.localPosition;		// get last area item position.

		startPosition = new Vector3 (pos.x + height, pos.y - height * 0.5f, pos.z); //normal skill start position.

		middlePosition = battleCardAreaItem [2].transform.localPosition;

		pos = battleCardAreaItem [0].transform.localPosition;	// get first area item position.

		endPosition = new Vector3 (pos.x - height * 0.5f, pos.y - height * 1.5f, pos.z);	//normal skill end position.

		//active skill is from left top to right top. normal skill start position is active skill end position.
		activeSkillStartPosition = new Vector3 (pos.x - height * 0.5f - 640f, pos.y - height * 0.5f, pos.z);	//active skill from position.

		Vector3 actveiPosition = battleCardAreaItem[2].transform.localPosition;

		activeSkillEndPosition = new Vector3 (actveiPosition.x , startPosition.y, startPosition.z);
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



	//---------battle card pool
	[HideInInspector]
	public UISprite templateBackTexture;
	
	private Vector3[] cardPosition;
	
	public Vector3[] CardPosition {
		get{return cardPosition;}
	}
	
	private UISprite[] backTextureIns;
	private int cardInterv = 0;
	private Vector3 initPosition = Vector3.zero;
	private float xStart = 0f;
	
	public float XRange {
		set { xStart = transform.localPosition.x - value / 2f; }
	}
	
	void InitData() {
		int count = Config.cardPoolSingle;
		cardPosition = new Vector3[count];
		backTextureIns = new UISprite[count];		
		templateBackTexture = FindChild<UISprite>("Back");
		cardInterv = templateBackTexture.width + Config.cardInterv;
		initPosition = Config.cardPoolInitPosition;
	}
	
	public int CaculateSortIndex(Vector3 point)
	{
		float x = point.x - xStart;
		
		for (int i = 0; i < cardPosition.Length; i++) 
		{
			if(x > cardInterv * i && x <= cardInterv * (i + 1))
				return i;
		}
		
		return -1;
	}


	//---battle card
	public event Callback CallBack;

	
	private UISprite templateItemCard;
	
	private List<CardItem> moveItem = new List<CardItem>();
	
	private List<TNormalSkill> normalSkill ;
	
//	public FightManipulationView battleCardArea;
	
	private BattleUseData battleUseData;
	
	private GameObject fingerObject;
	
	[HideInInspector]
	public CardItem[] cardItemArray;
	
	public override void DestoryUI () {
		base.DestoryUI ();
		for (int i = 0; i < cardItemArray.Length; i++) {
			Destroy(cardItemArray[i].gameObject);
		}
		cardItemArray = null;
		moveItem.Clear ();
	}
	
	void InitParameter() {
		templateItemCard = FindChild<UISprite>("Texture");
		fingerObject = FindChild<UISprite> ("finger").gameObject;
		
		int count = cardPosition.Length;
		
		cardItemArray = new CardItem[count];
		
		GameObject tempObject = null;
		
		for (int i = 0; i < count; i++) {
			tempObject = NGUITools.AddChild(gameObject,templateItemCard.gameObject);
			tempObject.transform.localPosition = cardPosition[i];
			CardItem ci = tempObject.AddComponent<CardItem>();
			ci.location = i;
			//			ci.Init(i.ToString());
			cardItemArray[i] = ci;
		}
		
		templateItemCard.gameObject.SetActive(false);
		cardInterv = (int)Mathf.Abs(cardPosition[1].x - cardPosition[0].x);
	}
	
	/// <summary>
	/// new
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="locationID">Location I.</param>
	public void ChangeSpriteCard(int source, int index, int locationID) {
		CardItem ci = cardItemArray [locationID];
		if (ci.itemID == source) {
			ci.SetSprite (index,CheckGenerationAttack (index));
		}
	}
	
	/// <summary>
	/// new 
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="locationID">Location I.</param>
	public void GenerateSpriteCard(int index,int locationID) {
		CardItem ci = cardItemArray [locationID];
		ci.SetSprite (index, CheckGenerationAttack (index));
	}
	
	public void RefreshLine() {
		foreach (var item in cardItemArray) {
			GenerateLinkSprite (item, item.itemID);
		}
	}
	
	void GenerateLinkSprite(CardItem ci,int index) {
		if (battleUseData == null) {
			battleUseData = BattleMapModule.bud;
		}
		List<Transform> trans = new List<Transform> ();
		for (int i = 0; i < battleCardAreaItem.Length; i++) {
			if(battleCardAreaItem[i] == null) 
				continue;
			if(battleUseData.upi.CalculateNeedCard(battleCardAreaItem[i].AreaItemID, index )) {
				trans.Add(battleCardAreaItem[i].transform);
			}
		}
		ci.SetTargetLine (trans);
	}
	
	/// <summary>
	/// t
	/// </summary>
	/// <param name="b">If set to <c>true</c> b.</param>
	public void StartBattle(bool b) {
		if (!b) {
			foreach (var item in cardItemArray) {
				item.Clear();
			}
		} else {
			RefreshLine();
		}
	}
	
	bool CheckGenerationAttack (int index) {
		if (index == 7) {
			return true;
		}
		if(normalSkill == null)
			normalSkill = BattleMapModule.bud.upi.GetNormalSkill ();
		foreach (var item in normalSkill) {
			if(item.Blocks.Contains((uint)index)) {
				return true;
			}
		}
		
		return false;
	}
	
	public void IgnoreCollider(bool isIgnore) {
		LayerMask layer;
		
		if(isIgnore)
			layer = GameLayer.ActorCard;
		else
			layer = GameLayer.IgnoreCard;
		
		for (int i = 0; i < cardItemArray.Length; i++) {
			cardItemArray[i].gameObject.layer = layer;
		}
	}
	
	public void DisposeDrag(int location,int itemID) {
		SetFront(location,itemID);
		SetBehind(location,itemID);
	}
	
	public void ResetDrag() {
		for (int i = 0; i < cardItemArray.Length; i++) {
			if(cardItemArray[i] == null) {
				continue;
			}
			cardItemArray[i].CanDrag = true;
		}
	}
	
	public bool SortCard(int sortID,List<CardItem> ci) {
		CardItem firstCard = ci[0];
		
		int index = ci.FindIndex(a =>a.location == sortID);
		
		if(index >= 0)
			return false;
		
		bool bigger = sortID > firstCard.location;
		
		CheckMove(sortID,ci,bigger);
		
		if(moveItem.Count == 0)
			return false;
		
		int moveCount = bigger ? -ci.Count : ci.Count;
		
		for (int i = 0; i < moveItem.Count; i++) {
			Vector3 position = moveItem[i].transform.localPosition;
			
			Vector3 to = new Vector3(position.x + moveCount * cardInterv,position.y,position.z);
			
			moveItem[i].Move(to);
			
			moveItem[i].location += moveCount;
		}
		
		for (int i = 0; i < ci.Count; i++) 
		{
			Vector3 pos = cardItemArray[sortID].transform.localPosition;
			Vector3 to;
			
			int plus = bigger ? -i : i;
			
			to = new Vector3(pos.x + plus * cardInterv,pos.y,pos.z);
			
			ci[i].location = sortID + plus;
			if(i == 0)
				ci[i].SetPos (to);	
			else
				ci[i].Move(to);
		}
		
		if (ci.Count > 1) {
			ci [ci.Count - 1].tweenCallback += HandletweenCallback;
		}
		else {
			moveItem[moveItem.Count - 1].tweenCallback += HandletweenCallback;;
		}
		
		moveItem.Clear();
		
		return true;
	}
	
	void HandletweenCallback (CardItem arg1) {
		arg1.tweenCallback -= HandletweenCallback;
		
		for (int i = 0; i < cardItemArray.Length - 1; i++) {
			for (int j = i; j < cardItemArray.Length; j++) 
			{
				if(cardItemArray[i].location > cardItemArray[j].location)
				{
					CardItem temp = cardItemArray[i];
					cardItemArray[i] = cardItemArray[j];
					cardItemArray[j] = temp;
				}
			}
		}
		
		if(CallBack != null)
			CallBack();
	}
	
	void CheckMove(int startID, List<CardItem> firstCard,bool bigger) {
		if(firstCard.FindIndex(a => a.location == startID) == -1)
		{
			moveItem.Add(cardItemArray[startID]);
			
			if(bigger)
				startID --;
			else
				startID ++;
			
			CheckMove(startID,firstCard,bigger);
		}
		else
			return;
	}
	
	void SetFront(int sID,int itemID) {
		int countID = sID - 1;
		
		if(countID < 0)
			return;
		
		if(cardItemArray[countID].SetCanDrag(itemID))
		{
			SetFront(countID,itemID);
		}
		else
		{
			while(countID > -1)
			{
				cardItemArray[countID].CanDrag = false;
				countID --;
			}
		}
	}
	
	void SetBehind(int sID,int itemID) {
		int countID = sID + 1;
		
		if(countID >= cardItemArray.Length)
			return;
		
		if(cardItemArray[countID].SetCanDrag(itemID))
		{
			SetBehind(countID,itemID);
		}
		else
		{
			while(countID < cardItemArray.Length)
			{
				cardItemArray[countID].CanDrag = false;
				countID ++;
			}
		}
	}
}
