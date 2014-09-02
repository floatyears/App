using UnityEngine;
using System.Collections.Generic;

public class Fight : ModuleBase
{
	public static Transform dragParent;

	private static UIRoot uiRoot;

	private static Camera mainCamera;

	private UICamera nguiMainCamera;
	
	private CardItem[] actorCard;
	
	private RaycastHit[] raycastHit;

	private GameObject parentObject;

	private GameObject singlePoolParent;
	
	//private CardPoolSingleItem[] cardPSingleItem;

	private GameObject multiPoolParent;

	private CardPoolMutilItem[] cardMultiItem;

	private List<CardItem> selectTarget = new List<CardItem>();

	public Fight(UIConfigItem config) : base(  config)
	{
		uiRoot = ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>();

		nguiMainCamera = ViewManager.Instance.MainUICamera;

		mainCamera = nguiMainCamera.GetComponent<Camera>();

		parentObject = NGUITools.AddChild(ViewManager.Instance.ParentPanel);

		parentObject.name = "Fight";

		parentObject.layer = GameLayer.ActorCard;

		GameInput.OnPressEvent += HandleOnPressEvent;

		GameInput.OnReleaseEvent += HandleOnReleaseEvent;

		GameInput.OnStationaryEvent += HandleOnStationaryEvent;

		GameInput.OnDragEvent += HandleOnDragEvent;

//		ignoreLayer = GameLayer.LayerToInt(GameLayer.IgnoreCard);
	}

	void SwitchInput(bool isShield) {
		nguiMainCamera.useMouse = isShield;
		nguiMainCamera.useKeyboard = isShield;
		nguiMainCamera.useTouch = isShield;
		Main.Instance.GInput.IsCheckInput = !isShield;
	}

	public override void InitUI () {
		dragParent = NGUITools.AddChild(parentObject).transform;
		CreatSingle();
		CreateMulti();
	}

	void CreatSingle() {
		singlePoolParent = NGUITools.AddChild(parentObject);
		singlePoolParent.name = "singlePoolParent";
	}

	void CreateMulti()
	{
		ResourceManager.Instance.LoadLocalAsset ("Prefabs/Card",o=>{
			GameObject go = o as GameObject;
			multiPoolParent = NGUITools.AddChild(parentObject);
			
			multiPoolParent.transform.localPosition += new Vector3(0f,100f,0f);
			
			multiPoolParent.name = "multiPoolParent";
			
			cardMultiItem = new CardPoolMutilItem[Config.cardPoolSingle];
			
//			for (int i = 0; i < Config.cardPoolSingle; i++)
//			{
//				tempObject = NGUITools.AddChild(multiPoolParent,go);
//				
//				tempObject.layer = GameLayer.BattleCard;
//				
//				NGUITools.AddWidgetCollider(tempObject);
//				
//				CardPoolMutilItem cpmi = tempObject.AddComponent<CardPoolMutilItem>();
//				
//				cpmi.Init("CardMultiPool" + i);
//				
//				cpmi.SetInitPosition(i);
//				
//				cardMultiItem[i] = cpmi;
//			}
		});


	}

	public override void HideUI ()
	{
		SwitchInput(true);
	}

	public override void ShowUI ()
	{
		SwitchInput(false);

//		for (int i = 0; i < cardPSingleItem.Length; i++)
//		{
//			cardPSingleItem[i].GenerateCard(Config.Instance.GetCard().itemID);
//		}
	}

	GameObject tempObject;
	CardItem tempCard;

	void HandleOnDragEvent (Vector2 obj)
	{
		Vector3 vec = (Vector3)obj;

		for (int i = 0; i < selectTarget.Count; i++) 
		{
			selectTarget[i].OnDrag(ChangeDeltaPosition(vec),i);
		}

		if(Check(GameLayer.ActorCard))
		{
			for (int i = 0; i < raycastHit.Length; i++)
			{
				tempObject = raycastHit[i].collider.gameObject;

				ClickObject(tempObject);
			}
		}
	}
	
	void HandleOnStationaryEvent ()
	{
		
	}
	
	void HandleOnReleaseEvent ()
	{
		ReleasePress();
	}
	
	void HandleOnPressEvent ()
	{
		if(Check(GameLayer.ActorCard))
		{
			tempObject= raycastHit[0].collider.gameObject;

			ClickObject(tempObject);

			if(selectTarget.Count > 0)
				DisposeDrag(selectTarget[0].location,selectTarget[0].itemID);
		}
	}

	void ClickObject(GameObject go)
	{
		tempCard = go.GetComponent<CardItem>();

		if(tempCard != null)
		{
			if(tempCard.CanDrag)
			{
				tempCard.OnPress(true,selectTarget.Count);

				selectTarget.Add(tempCard);
			}
		}
	}

	void DisposeDrag(int sortID,int itemID)
	{
		SetFront(sortID,itemID);

		SetBehind(sortID,itemID);
	}

	void SetFront(int sID,int itemID)
	{
//		int countID = sID - 1;
//
//		if(countID < 0)
//			return;

//		if(cardPSingleItem[countID].SetDrag(itemID))
//		{
//			SetFront(countID,itemID);
//		}
//		else
//		{
//			while(countID > -1)
//			{
//				cardPSingleItem[countID].SetNotDrag(false);
//				countID --;
//			}
//		}
	}

	void SetBehind(int sID,int itemID)
	{
//		int countID = sID + 1;
//
//		if(countID >= cardPSingleItem.Length)
//			return;
//
//		if(cardPSingleItem[countID].SetDrag(itemID))
//		{
//			SetBehind(countID,itemID);
//		}
//		else
//		{
//			for (int i = countID + 1; i < cardPSingleItem.Length; i++)
//			{
//				cardPSingleItem[i].SetNotDrag(false);
//			}
//		}
	}

//	int tempCount = 0;

	void ReleasePress()
	{
		if(selectTarget.Count == 0)
			return;

		if(Check(GameLayer.BattleCard))
		{
			CardPoolMutilItem targetGO = null;
			
			for (int i = 0; i < raycastHit.Length; i++) 
			{
				if(raycastHit[i].collider.gameObject.layer == GameLayer.BattleCard)
				{
					targetGO = raycastHit[i].collider.gameObject.GetComponent<CardPoolMutilItem>();
					
					if(targetGO != null)
						break;
				}
			}
			
			if(targetGO != null )
			{
//				tempCount = Config.cardPoolSingle - targetGO.GetCount();
				
				targetGO.GenerateCard(selectTarget);
			}

			ResetDragSuccess();
		}
		else if(Check(GameLayer.ActorCard))
		{
			Vector3 point = selectTarget[0].transform.localPosition;

			int sortID = GetLocationByPosition(point,tempIndexList);
			LogHelper.LogError(sortID);
			if(sortID != -1)
			{
				for (int i = 0; i < selectTarget.Count; i++)
				{
					if(sortID >= Config.cardPoolSingle)
						break;

//					selectTarget[i].transform.localPosition = cardPSingleItem[sortID].transform.localPosition;

//					CardPoolSingleItem temp = cardPSingleItem[sortID];
//
//					cardPSingleItem[sortID] = cardPSingleItem[selectTarget[i].location];
//
//					cardPSingleItem[selectTarget[i].location] = temp;

					sortID++;
				}

//				for (int i = 0; i < cardPSingleItem.Length; i++)
//				{
//					cardPSingleItem[i].ChangePosition(i);
//				}
			}

			ResetDragSuccess();
		}
		else
			ResetDragSuccess();
	}

	List<int> tempIndexList = new List<int>();

	int GetLocationByPosition(Vector3 point,List<int> ignorIndex)
	{
//		int endIndex = Config.cardPoolSingle - 1;

//		if(point.x <= cardPSingleItem[0].transform.localPosition.x)
//			return 0;
//		else if(point.x > cardPSingleItem[endIndex].transform.localPosition.x)
//			return endIndex;
//
//		for (int i = 0; i < cardPSingleItem.Length - 2; i++) 
//		{
//			int j = i + 1;
//
//			if(point.x > cardPSingleItem[i].transform.localPosition.x && point.x <= cardPSingleItem[j].transform.localPosition.x)
//				return j;
//		}

		return -1;
	}

	void DisposeGenerate()
	{
//		if(tempCount < 0)
//			tempCount = 0;
//		
//		for (int i = 0; i < tempCount; i++) 
//		{
//			if(selectTarget.Count > i)
//			{
//				cardPSingleItem[selectTarget[i].location].GenerateCard(Config.Instance.GetCard().itemID);
//			}
//		}
//		
//		tempCount = 0;
	}

	void ResetDragSuccess()
	{
		for (int i = 0; i < selectTarget.Count; i++) 
		{
			selectTarget[i].OnPress(false,-1);
		}
		
		DisposeGenerate();

		Reset();
	}

	void Reset()
	{
//		for(int i = 0;i<cardPSingleItem.Length;i++)
//		{
//			cardPSingleItem[i].SetNotDrag(true);
//		}

		selectTarget.Clear();
	}

	bool Check(LayerMask mask)
	{
		ChangeCameraPosition();

		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

		raycastHit = Physics.RaycastAll(ray,100f, GameLayer.LayerToInt(mask));
		
		if(raycastHit.Length > 0)
			return true;
		else
			return false;
	}

	public static Vector3 ChangeCameraPosition()
	{
		Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

		float height = (float)Screen.height / 2;

		Vector3 reallyPoint = worldPoint * height * uiRoot.pixelSizeAdjustment;

		return reallyPoint;
	}

	public static Vector3 ChangeDeltaPosition(Vector3 delta)
	{
		return delta * uiRoot.pixelSizeAdjustment;
	}
}
