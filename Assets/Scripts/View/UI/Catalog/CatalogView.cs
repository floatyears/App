using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatalogView : UIComponentUnity {
	private int TOTAL_CATALOG_COUNT;
	private DragPanel dragPanel;
	private List<CatalogUnitItem> catalogUnitItemList = new List<CatalogUnitItem>();
	private List<UIWidget> catalogWidgetList = new List<UIWidget>();
    private List<Transform> catalogItemTrans	= new List<Transform>();
    private NewObjectPooler catalogPooler;
	private UIPanel uiPanel;

	int count = 0;
        
        public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitConfig();
		InitPooler();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		CreateDragPanel();
		RefreshItemCounter();
		RefreshCatalogView();
	}
	
	public override void HideUI () {
		base.HideUI ();
		DestoryDragPanel();
	}

	GameObject emptyItem;
	private void CreateDragPanel(){
		string sourcePath = "Prefabs/UI/UnitItem/CatalogUnitPrefab";
		emptyItem = Resources.Load(sourcePath) as GameObject;

		dragPanel = new DragPanel("CatalogDragPanel", emptyItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(TOTAL_CATALOG_COUNT);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.CatalogDragPanelArgs, transform);

		uiPanel = dragPanel.DragPanelView.gameObject.transform.FindChild("Scroll View").GetComponent<UIPanel>();


		for(int i = 0; i < TOTAL_CATALOG_COUNT; i++){
			GameObject dragItem = dragPanel.ScrollItem[ i ];
			catalogItemTrans.Add(dragItem.transform);
		}

		InitCatalogTrans(0, 40);

	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.Instace.GetCurrentText("CatalogCounterTitle"));
		countArgs.Add("current", GetCurGotUnitCount());
		countArgs.Add("max", TOTAL_CATALOG_COUNT);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private int GetTotalUnitCount(){
		//TODO LOAD FORM CONFIG FILE
		int totalUnitCount = 300;
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

	private void InitConfig(){
		TOTAL_CATALOG_COUNT = GetTotalUnitCount();
	}

	private void DestoryDragPanel(){
		catalogUnitItemList.Clear();
		dragPanel.DestoryUI();
	}

	GameObject catalogNode;
	private void InitPooler(){
		string path = "Prefabs/UI/UnitItem/CatalogNode";
		catalogNode = Resources.Load(path) as GameObject;
		catalogPooler = gameObject.AddComponent<NewObjectPooler>();
		catalogPooler.Init(catalogNode, 45);
		
		GameObject pooledObjectsParent = new GameObject();
		pooledObjectsParent.transform.parent = gameObject.transform;
		pooledObjectsParent.transform.localScale = new Vector3(1, 1, 1);
		
		foreach (var item in catalogPooler.pooledObjects) {
			item.transform.parent = pooledObjectsParent.transform;
            item.transform.localScale = new Vector3(1, 1, 1);
		}            
	}


	public const int CATALOG_ITEM_COUNT = 300;
	private void Update(){
		ShowItemInScreen();
	}

	private float itemWidth = 55;
	private bool CheckIsInScreen(Transform trans){
//		Vector3 worldPos = Camera.main.WorldToScreenPoint(trans.position);
//		Debug.Log(string.Format("worldPos.x {0}, itemWidth {1}, {2}" , worldPos.x , Screen.width, itemWidth));
//		if(worldPos.x >= -itemWidth && worldPos.x > (float)Screen.width + itemWidth){
//			return false;
//		}
//		else{
//			return true;
//		}
		Debug.Log("uiPanel " + uiPanel + "trans " + trans + "uiPanel.IsVisible : " + uiPanel.IsVisible(trans.GetComponent<UIWidget>()));
		return uiPanel.IsVisible(trans.GetComponent<UIWidget>());
	}

	private void InitCatalogTrans(int startPos, int onePageCount){
		GameObject obj;
		CatalogUnitItem catalogUnitItem;
		for (int i = startPos; i < onePageCount; i++){
			obj = catalogPooler.GetPooledObject();
			obj.transform.parent = catalogItemTrans[ i ];
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			CatalogUnitItem item = CatalogUnitItem.Inject(obj);
			obj.SetActive(true);
            item.Refresh(i + 1);
		}
		curStartPos = startPos;
	}

	private float prevFirstItemPos_X = -111111f;
	private int curStartPos;

	public enum MoveDirection{
		LEFT,
		RIGHT,
		NONE
	}

	private MoveDirection CalculateMoveDirection(){
		float nowFirstItemPos_X = Camera.main.WorldToScreenPoint( catalogItemTrans[ 0 ].position ).x;
		if (prevFirstItemPos_X == -111111f){
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
		Debug.Log("dmoveDir" + moveDir);
		if(moveDir == MoveDirection.LEFT){
			while(!CheckIsInScreen(catalogItemTrans[ curStartPos ])){
				Debug.Log("ShowItemInScreen do visble false");
				CancelShowOneRow(curStartPos, ITEM_COUNT_PER_COL);
				AddShowOneRow(curStartPos + ITEM_MAX_COUNT_PER_PAGE, ITEM_COUNT_PER_COL);
				curStartPos += ITEM_COUNT_PER_COL;
				Debug.Log("ShowItemInScreen() " + curStartPos);
			}
		}
		else if(moveDir == MoveDirection.RIGHT){
			while(!CheckIsInScreen(catalogItemTrans[ curStartPos + ITEM_MAX_COUNT_PER_PAGE])){
				if (curStartPos < ITEM_COUNT_PER_COL) break;
                      CancelShowOneRow(curStartPos + ITEM_MAX_COUNT_PER_PAGE , ITEM_COUNT_PER_COL);
				AddShowOneRow(curStartPos - ITEM_COUNT_PER_COL, ITEM_COUNT_PER_COL);
				curStartPos -= ITEM_COUNT_PER_COL;
			}
		}
		else{}
	}

	private void AddShowOneRow(int startPos, int count){
		GameObject obj;
		CatalogUnitItem catalogUnitItem;
		for (int i = startPos; i < startPos + count; i++){
			Debug.Log(i);
			if(catalogItemTrans[ i ].childCount == 1){
				obj = catalogItemTrans[ i ].FindChild("CatalogNode(Clone)").gameObject;
			}
			else {
				obj = catalogPooler.GetPooledObject();
				obj.transform.parent = catalogItemTrans[ i ];
				obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;            
        				}
        				CatalogUnitItem item = CatalogUnitItem.Inject(obj);
        				obj.SetActive(true);
			item.Refresh(i + 1);
		}
//		curStartPos = startPos + count;
	}
	
	private void CancelShowOneRow(int startPos, int count){
        for (int i = startPos; i < startPos + count; i++){
			Debug.Log("CancelShowOneRow(), startPos : " + startPos);
                GameObject childObj = catalogItemTrans[ i ].FindChild("CatalogNode(Clone)").gameObject;
                childObj.SetActive(false);
        }
	}
}
