using UnityEngine;
using System.Collections.Generic;

public class BattleCard : UIBaseUnity {
	public event Callback CallBack;

	private Vector3[] cardPosition;

	public Vector3[] CardPosition {
		set{cardPosition = value;}
	}

	private UISprite templateItemCard;

	private CardItem[] cardItemArray;

	private List<CardItem> moveItem = new List<CardItem>();

	private float cardInterv = 0f;

	private List<TNormalSkill> normalSkill ;

	public BattleCardArea battleCardArea;

	private BattleUseData battleUseData;

	public override void Init (string name) {
		base.Init (name);
		InitParameter();
	}

	public override void ShowUI () {
//		LogHelper.Log("battle card ShowUI");
		base.ShowUI ();
		gameObject.SetActive(true);
	}

	public override void HideUI () {
//		LogHelper.Log("battle card HideUI");
		base.HideUI ();
		gameObject.SetActive(false);
	}

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
		int count = cardPosition.Length;
		
		cardItemArray = new CardItem[count];
		for (int i = 0; i < count; i++) {
			tempObject = NGUITools.AddChild(gameObject,templateItemCard.gameObject);
			tempObject.transform.localPosition = cardPosition[i];
			CardItem ci = tempObject.AddComponent<CardItem>();
			ci.location = i;
			ci.Init(i.ToString());
			cardItemArray[i] = ci;
		}

		templateItemCard.gameObject.SetActive(false);
		cardInterv = Mathf.Abs(cardPosition[1].x - cardPosition[0].x);
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
//		Debug.LogError ("RefreshLine ");
		foreach (var item in cardItemArray) {
			GenerateLinkSprite (item, item.itemID);
		}
	}

	void GenerateLinkSprite(CardItem ci,int index) {
	
		if (battleUseData == null) {
			battleUseData = BattleQuest.bud;
		}
		List<Transform> trans = new List<Transform> ();
		for (int i = 0; i < battleCardArea.battleCardAreaItem.Length; i++) {
			if(battleCardArea.battleCardAreaItem[i] == null) 
				continue;
			if(battleUseData.upi.CalculateNeedCard(battleCardArea.battleCardAreaItem[i].AreaItemID, index)) {
				trans.Add(battleCardArea.battleCardAreaItem[i].transform);
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

//	void CheckNeedSprite(List<int> haveSprite) {
//
//		for (int i = 0; i < normalSkill.Count; i++) {
//
//		}
//	}

	bool CheckGenerationAttack (int index) {
		if (index == 7) {
			return true;
		}
		if(normalSkill == null)
			normalSkill = BattleQuest.bud.upi.GetNormalSkill ();
		foreach (var item in normalSkill) {
			if(item.Blocks.Contains((uint)index)) {
				return true;
			}
		}

		return false;
	}

	public void IgnoreCollider(bool isIgnore)
	{
		LayerMask layer;

		if(isIgnore)
			layer = GameLayer.ActorCard;
		else
			layer = GameLayer.IgnoreCard;

		for (int i = 0; i < cardItemArray.Length; i++) {
			cardItemArray[i].gameObject.layer = layer;
		}
	}

	public void DisposeDrag(int location,int itemID)
	{
		SetFront(location,itemID);
		SetBehind(location,itemID);
	}

	public void ResetDrag()
	{
		for (int i = 0; i < cardItemArray.Length; i++)  {
//			Debug.LogError("ResetDrag : " + cardItemArray[i] +  " i : " + i + " BattleCard : " + this);
			if(cardItemArray[i] == null) {
				continue;
			}
			cardItemArray[i].CanDrag = true;
		}
	}

	public bool SortCard(int sortID,List<CardItem> ci)
	{
		CardItem firstCard = ci[0];

		int index = ci.FindIndex(a =>a.location == sortID);

		if(index >= 0)
			return false;

		bool bigger = sortID > firstCard.location;

		CheckMove(sortID,ci,bigger);

		if(moveItem.Count == 0)
			return false;

		int moveCount = bigger ? -ci.Count : ci.Count;

		for (int i = 0; i < moveItem.Count; i++)
		{
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

	void HandletweenCallback (CardItem arg1)
	{
		arg1.tweenCallback -= HandletweenCallback;

		for (int i = 0; i < cardItemArray.Length - 1; i++) 
		{
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
	
	void CheckMove(int startID, List<CardItem> firstCard,bool bigger)
	{
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

	void SetFront(int sID,int itemID)
	{
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

	void SetBehind(int sID,int itemID)
	{
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
