using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class CatalogView : ViewBase {
	private int TOTAL_CATALOG_COUNT;
	private DragPanel dragPanel;
	private List<CatalogUnitItem> catalogUnitItemList = new List<CatalogUnitItem>();
	private List<UIWidget> catalogWidgetList = new List<UIWidget>();
    private List<Transform> catalogItemTrans	= new List<Transform>();
//    private NewObjectPooler catalogPooler;
	private UIPanel uiPanel;
	private const float UN_INTIALIZED_POS = -111111f;

	private bool canDoLeftCacheMove = true;
	private bool canDoRightCacheMove = true;

	private DragPanelConfigItem dragConfig;

       
	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config, data);
		TOTAL_CATALOG_COUNT = GetTotalUnitCount();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		RefreshItemCounter();
		StartCoroutine("InitDragPanel");

		transform.localPosition = new Vector3 (-1000, -260, 0);
		iTween.MoveTo (gameObject, iTween.Hash ("x", 0, "time", 0.4f, "islocal", true));
	}
	
	public override void HideUI () {
		base.HideUI ();
		DestoryDragPanel();
	}

	private GameObject emptyItem;
//	private void CreateDragPanel(){
//		string sourcePath = "Prefabs/UI/UnitItem/CatalogUnitPrefab";
//		ResourceManager.Instance.LoadLocalAsset(sourcePath, o =>{
//			emptyItem = o  as GameObject;
//			dragPanel = new DragPanel("CatalogDragPanel", emptyItem,transform);
////			dragPanel.CreatUI();
//			dragPanel.AddItem(TOTAL_CATALOG_COUNT);
//			
////			uiPanel = dragPanel.DragPanelView.gameObject.transform.FindChild("Scroll View").GetComponent<UIPanel>();
//			
//			for(int i = 0; i < TOTAL_CATALOG_COUNT; i++){
//				GameObject dragItem = dragPanel.ScrollItem[ i ];
//				catalogItemTrans.Add(dragItem.transform);
//			}
//			InitCatalogTrans(0, 40);
//		});
//
//	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("CatalogCounterTitle"));
		countArgs.Add("current", GetCurGotUnitCount());
		countArgs.Add("max", TOTAL_CATALOG_COUNT);
		countArgs.Add("posy", -736);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private int GetTotalUnitCount(){
		//TODO LOAD FORM CONFIG FILE
		int totalUnitCount = 226;
		return totalUnitCount;
	}

	private int GetCurGotUnitCount(){
		int gotCount = 0;
		for (int i = 1; i <= TOTAL_CATALOG_COUNT; i++){
			if(DataCenter.Instance.CatalogInfo.IsHaveUnit((uint)i)){
				gotCount ++;
			}
		}
		return gotCount;
	}

	private void RefreshCatalogView(){
		for (int i = 1; i <= TOTAL_CATALOG_COUNT; i++){
			catalogUnitItemList[ i - 1 ].Refresh( i );
		}
	}

	private void DestoryDragPanel(){
		dynamicDragPanel.DestoryDragPanel();
	}

	GameObject catalogNode;
	private void InitPooler(){
//		string path = "Prefabs/UI/UnitItem/CatalogNode";
//		catalogNode = ResourceManager.Instance.LoadLocalAsset(path) as GameObject;
//		catalogPooler = gameObject.AddComponent<NewObjectPooler>();
//		catalogPooler.Init(catalogNode, 45);
//		
//		GameObject pooledObjectsParent = new GameObject();
//		pooledObjectsParent.transform.parent = gameObject.transform;
//		pooledObjectsParent.transform.localScale = new Vector3(1, 1, 1);
//		
//		foreach (var item in catalogPooler.pooledObjects) {
//			item.transform.parent = pooledObjectsParent.transform;
//            item.transform.localScale = new Vector3(1, 1, 1);
//		}            
	}


	public const int CATALOG_ITEM_COUNT = 300;
	private void Update(){
		//ShowItemInScreen();
	}

	private float itemWidth = 55;
	private bool CheckIsInScreen(Transform trans){
		//Debug.Log("uiPanel " + uiPanel + "trans " + trans + "uiPanel.IsVisible : " + uiPanel.IsVisible(trans.GetComponent<UIWidget>()));
		return uiPanel.IsVisible(trans.GetComponent<UIWidget>());
	}


	private void InitCatalogTrans(int startPos, int onePageCount){
//		GameObject obj;
//		CatalogUnitItem catalogUnitItem;
//		for (int i = startPos; i < onePageCount; i++){
//			obj = catalogPooler.GetPooledObject();
//			obj.transform.parent = catalogItemTrans[ i ];
//			obj.transform.localScale = Vector3.one;
//			obj.transform.localPosition = Vector3.zero;
//			CatalogUnitItem item = CatalogUnitItem.Inject(obj);
//			obj.SetActive(true);
//            item.Refresh(i + 1);
//		}
//		curStartPos = startPos;
	}

	private float prevFirstItemPos_X = UN_INTIALIZED_POS;
	private int curStartPos;

	public enum MoveDirection{
		LEFT,
		RIGHT,
		NONE
	}

	private MoveDirection CalculateMoveDirection(){
		float nowFirstItemPos_X = Camera.main.WorldToScreenPoint( catalogItemTrans[ 0 ].position ).x;
		if (prevFirstItemPos_X == UN_INTIALIZED_POS){
			prevFirstItemPos_X = nowFirstItemPos_X;
			return MoveDirection.NONE;
		}
		int result = nowFirstItemPos_X > prevFirstItemPos_X ? 1 : nowFirstItemPos_X < prevFirstItemPos_X ? - 1: 0;
		prevFirstItemPos_X = nowFirstItemPos_X;
		return result == 1 ? MoveDirection.RIGHT : result == -1 ? MoveDirection.LEFT : MoveDirection.NONE;

	}

	private const int ITEM_MAX_COUNT_PER_PAGE = 30;
	private const int ITEM_COUNT_PER_COL = 5;
	private void ShowItemInScreen(){
		MoveDirection moveDir = CalculateMoveDirection();
//		Debug.Log("dmoveDir" + moveDir);
//		if (!CheckPosAvaliable(curStartPos) || !CheckPosAvaliable(curStartPos + ITEM_MAX_COUNT_PER_PAGE)){
//			return;
//		}
		if(moveDir == MoveDirection.LEFT){
//			int judgePos = curStartPos;
//			if (judgePos < 0) {
//				curStartPos =  0;
//				return;
//			}
//			if (!CheckPosAvaliable(curStartPos)){
//				return;
//			}
//			curStartPos = AdjustPos(curStartPos);
			while(!CheckIsInScreen(catalogItemTrans[ curStartPos ])){

//				Debug.Log("ShowItemInScreen do visble false");
				int cancelCount = AddShowOneRow(curStartPos + ITEM_MAX_COUNT_PER_PAGE, ITEM_COUNT_PER_COL);


//				Debug.Log("ShowItemInScreen() left,  pos " + cancelCount);
//				AddShowOneRow(curStartPos + ITEM_MAX_COUNT_PER_PAGE, cancelCount);
				if (cancelCount == 0) break;
				CancelShowOneRow(curStartPos, cancelCount, moveDir);

				curStartPos += cancelCount;
//				Debug.Log("ShowItemInScreen() " + curStartPos);
//				curStartPos = AdjustPos(curStartPos);

			}
		}
		else if(moveDir == MoveDirection.RIGHT){
//			curStartPos = AdjustPos(curStartPos);
//			if (!canDoRightCacheMove) return;
			int judgePos = curStartPos + ITEM_MAX_COUNT_PER_PAGE;
//			if (judgePos >= CATALOG_ITEM_COUNT) {
//				curStartPos = CATALOG_ITEM_COUNT - ITEM_MAX_COUNT_PER_PAGE - 1;
//				return;
//			}
			while(!CheckIsInScreen(catalogItemTrans[ judgePos])){

//				if (curStartPos < ITEM_COUNT_PER_COL) break;
				int cancelCount = CancelShowOneRow( judgePos - ITEM_COUNT_PER_COL, ITEM_COUNT_PER_COL, moveDir);
//				Debug.Log("ShowItemInScreen() right, pos " + cancelCount);
				if (cancelCount == 0) break;
				AddShowOneRow(curStartPos - ITEM_COUNT_PER_COL, cancelCount);
				curStartPos -= cancelCount;
//				curStartPos = AdjustPos(curStartPos);


			}
		}
		else{}
	}

	private int AddShowOneRow(int startPos, int count){
//		GameObject obj;
//		CatalogUnitItem catalogUnitItem;
//		for (int i = 0; i < count; i++){
//			int pos = startPos + i;
//			Debug.Log(pos);
//			if (!CheckPosAvaliable(pos)) return i;
//
//			if(catalogItemTrans[ pos ].childCount == 1){
//				obj = catalogItemTrans[ pos ].FindChild("CatalogNode(Clone)").gameObject;
//			}
//			else {
//				obj = catalogPooler.GetPooledObject();
//				obj.transform.parent = catalogItemTrans[ pos ];
//				obj.transform.localScale = Vector3.one;
//                obj.transform.localPosition = Vector3.zero;            
//			}
//			CatalogUnitItem item = CatalogUnitItem.Inject(obj);
//			obj.SetActive(true);
//			item.Refresh(pos + 1);
//		}
		return ITEM_COUNT_PER_COL;
	}

	private bool CheckPosAvaliable(int pos){
//		Debug.Log("CheckPosAvaliable() pos is " + pos);
		return pos >= 0 && pos < CATALOG_ITEM_COUNT;
	}

	private int AdjustPos(int pos){
		
		return pos < ITEM_COUNT_PER_COL  ? ITEM_COUNT_PER_COL :  pos  +  ITEM_COUNT_PER_COL > CATALOG_ITEM_COUNT ?  
			CATALOG_ITEM_COUNT - 1 - ITEM_COUNT_PER_COL: pos;
		
	}


	private int CancelShowOneRow(int startPos, int count, MoveDirection dir){
//		if(dir == MoveDirection.NONE){
//			return 0;
//		}
//		else if(dir == MoveDirection.LEFT){
	        for (int i = 0; i < count; i++){
				int pos = startPos + i;
//				Debug.Log("CancelShowOneRow(), startPos : " + pos);
				if (!CheckPosAvaliable(pos)) return i;
				GameObject childObj = catalogItemTrans[ pos ].FindChild("CatalogNode(Clone)").gameObject;
	            childObj.SetActive(false);
	        }
//		}
//		else{
//			for (int i = 0; i < count ; i++){
//				int pos = startPos - count + i;
//
//				Debug.Log("CancelShowOneRow(), startPos : " + pos);
//				if (!CheckPosAvaliable(pos)) return i;
//				GameObject childObj = catalogItemTrans[ pos ].FindChild("CatalogNode(Clone)").gameObject;
//				childObj.SetActive(false);
//			}
//		}
		return ITEM_COUNT_PER_COL;

	}


	//--------------------------------NEW---------------------------------
	DragPanelDynamic dynamicDragPanel;

	IEnumerator InitDragPanel() {
		GameObject go = Instantiate(CatalogUnitItem.ItemPrefab) as GameObject;
		CatalogUnitItem.Inject(go);

		dragConfig = DataCenter.Instance.GetConfigDragPanelItem ("CatalogDragPanel");
		dynamicDragPanel = new DragPanelDynamic(gameObject, go, 12, 5);
		dynamicDragPanel.SetScrollView(dragConfig, transform);

//		DragPanelSetInfo setter = new DragPanelSetInfo();
//		setter.parentTrans = transform;
//		setter.clipRange = new Vector4 (0, -200, 640, 560);
//		setter.gridArrange = UIGrid.Arrangement.Vertical;
//		setter.scrollBarPosition = new Vector3 (320, -460, 0);
//		setter.maxPerLine = 5;
//		setter.depth = 2;	
//		dynamicDragPanel.SetDragPanel (setter);

		List<TUserUnit> catalogDataList =  new List<TUserUnit>();
		for (int i = 0; i < TOTAL_CATALOG_COUNT; i++){
			UserUnit userUnit = new UserUnit();
			userUnit.level = 1;
			userUnit.exp = 0;
			userUnit.unitId = (uint)(i + 1);
			catalogDataList.Add(new TUserUnit(userUnit));
		}
		dynamicDragPanel.RefreshItem(catalogDataList);
		yield return null;
		ShowUIAnimation();
	}

	void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, -256, 0);
		iTween.MoveTo(gameObject, iTween.Hash("time", 0.4f, "x", 0, "islocal", true));
	}

}
