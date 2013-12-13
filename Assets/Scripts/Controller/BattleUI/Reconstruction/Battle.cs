using UnityEngine;
using System.Collections.Generic;

public class Battle : UIBase
{
	private static UIRoot uiRoot;
	private static Camera mainCamera;
	private UICamera nguiMainCamera;

	private GameObject battleRootGameObject;

	private RaycastHit[] rayCastHit;

	private CardItem tempCard;
	private GameObject tempObject;

	private BattleCardPool battleCardPool;
	private BattleCard battleCard;
	private BattleCardArea battleCardArea;
	
	private List<ItemData> allItemData = new List<ItemData>();
	private List<CardItem> selectTarget = new List<CardItem>();

	public Battle(string name):base(name)
	{
		uiRoot = ViewManager.Instance.MainUIRoot.GetComponent<UIRoot>();
		nguiMainCamera = ViewManager.Instance.MainUICamera;
		mainCamera = nguiMainCamera.camera;

		battleRootGameObject = NGUITools.AddChild(ViewManager.Instance.ParentPanel);
		battleRootGameObject.name = "Fight";
		battleRootGameObject.layer = GameLayer.ActorCard;
		
		GameInput.OnPressEvent += HandleOnPressEvent;
		GameInput.OnReleaseEvent += HandleOnReleaseEvent;
		GameInput.OnStationaryEvent += HandleOnStationaryEvent;
		GameInput.OnDragEvent += HandleOnDragEvent;
	}

	public override void CreatUI ()
	{
		CreatBack();

		CreatCard();

		CreatArea();
	}

	void CreatBack()
	{
		string backName = "BattleCardPool";

		tempObject = GetPrefabsObject(backName);

		battleCardPool = tempObject.AddComponent<BattleCardPool>();
		
		battleCardPool.Init(backName);
		
		BoxCollider bc = NGUITools.AddWidgetCollider(tempObject);

		battleCardPool.XRange = bc.size.x;
	}

	void CreatCard()
	{
		string cardName = "BattleCard";

		tempObject = GetPrefabsObject(cardName);

		tempObject.layer = GameLayer.ActorCard;

		battleCard = tempObject.AddComponent<BattleCard>();

		battleCard.CardPosition = battleCardPool.CardPosition;

		battleCard.Init(cardName);

		for (int i = 0; i < battleCardPool.CardPosition.Length; i++)
		{
			battleCard.GenerateCard(GenerateData(),i);
		}
	}

	void CreatArea()
	{
		string areaName = "BattleCardArea";

		tempObject = GetPrefabsObject(areaName);

		Vector3 pos = battleCardPool.transform.localPosition;

		tempObject.transform.localPosition = new Vector3(pos.x,pos.y + 200,pos.z);

		tempObject.layer = GameLayer.BattleCard;

		battleCardArea = tempObject.AddComponent<BattleCardArea>();

		battleCardArea.Init(areaName);

		battleCardArea.CreatArea(battleCardPool.CardPosition);
	}

	GameObject GetPrefabsObject(string name)
	{
		tempObject = LoadAsset.Instance.LoadAssetFromResources(name, ResourceEuum.Prefab) as GameObject;

		GameObject go = NGUITools.AddChild(battleRootGameObject,tempObject);

		go.AddComponent<UIPanel>();

		return go;
	}

	int GenerateData()
	{
		ItemData id = Config.Instance.GetCard();

		allItemData.Add(id);

		return id.itemID;
	}


	void SwitchInput(bool isShield)
	{
		nguiMainCamera.useMouse = isShield;
		nguiMainCamera.useKeyboard = isShield;
		nguiMainCamera.useTouch = isShield;
		
		main.GInput.IsCheckInput = !isShield;
	}

	void HandleOnPressEvent ()
	{
		DisposePress();
	}

	void HandleOnReleaseEvent ()
	{
		DisposeReleasePress();
	}

	void HandleOnStationaryEvent ()
	{
		
	}

	void HandleOnDragEvent (Vector2 obj)
	{
		DisposeOnDrag(obj);
	}

	void ResetClick()
	{
		for (int i = 0; i < selectTarget.Count; i++)
		{
			selectTarget[i].OnPress(false,-1);			
		}

		selectTarget.Clear();

		battleCard.ResetDrag();
	}
	

	void DisposeReleasePress()
	{
		if(selectTarget.Count == 0)
		{
			ResetClick();

			return;
		}

		IgnoreLayer(false);

		if(Check(GameLayer.BattleCard))
		{
			BattleCardAreaItem bcai = null;

			for (int i = 0; i < rayCastHit.Length; i++) 
			{
				tempObject = rayCastHit[i].collider.gameObject;
				bcai = tempObject.GetComponent<BattleCardAreaItem>();

				if(bcai != null)
					break;
			}

			if(bcai != null)
				bcai.GenerateCard(selectTarget);

			for(int i = 0;i<selectTarget.Count;i++)
			{
				battleCard.GenerateCard(GenerateData(),selectTarget[i].location);
			}

			ResetClick();
		}
		else if(Check(GameLayer.ActorCard))
		{
			Vector3 point = selectTarget[0].transform.localPosition;

			int indexID =  battleCardPool.CaculateSortIndex(point);

			if(indexID >= 0)
			{
				main.GInput.IsCheckInput = false;
				if(battleCard.SortCard(indexID,selectTarget))
				{
					battleCard.CallBack += HandleCallBack;
				}
				else
				{
					main.GInput.IsCheckInput = true;

					ResetClick();
				}
			}
		}
		else
		{
			ResetClick();
		}
	}

	void HandleCallBack ()
	{
		main.GInput.IsCheckInput = true;
		ResetClick();
	}

	void TweenCallback(GameObject go)
	{
		main.GInput.IsCheckInput = true;
	}

	bool tempCheckDragFirst = false;

	void DisposeOnDrag(Vector2 obj)
	{
		Vector3 vec = (Vector3)obj;

		SetDrag();

		for (int i = 0; i < selectTarget.Count; i++) 
		{
			selectTarget[i].OnDrag(ChangeDeltaPosition(vec));
		}

		if(Check(GameLayer.ActorCard))
		{
			for (int i = 0; i < rayCastHit.Length; i++)
			{
				tempObject = rayCastHit[i].collider.gameObject;
				
				ClickObject(tempObject);
			}
		}
	}

	void DisposePress()
	{
		IgnoreLayer(true);

		if(Check(GameLayer.ActorCard))
		{
			tempObject= rayCastHit[0].collider.gameObject;
			
			ClickObject(tempObject);
			
			SetDrag();
		}
	}

	void SetDrag()
	{
		if(selectTarget.Count > 0)
			battleCard.DisposeDrag(selectTarget[0].location,selectTarget[0].itemID);
	}

	void IgnoreLayer(bool isPress)
	{
		battleCard.IgnoreCollider(isPress);
		battleCardPool.IgnoreCollider(isPress);
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

	bool Check(LayerMask mask)
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		
		rayCastHit = Physics.RaycastAll(ray,100f, GameLayer.LayerToInt(mask));

		if(rayCastHit.Length > 0)
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
