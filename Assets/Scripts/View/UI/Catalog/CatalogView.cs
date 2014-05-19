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

	private void CreateDragPanel(){
		string sourcePath = "Prefabs/UI/UnitItem/CatalogUnitPrefab";
		GameObject emptyItem = Resources.Load(sourcePath) as GameObject;

		dragPanel = new DragPanel("CatalogDragPanel", emptyItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(TOTAL_CATALOG_COUNT);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.CatalogDragPanelArgs, transform);

		uiPanel = dragPanel.DragPanelView.gameObject.transform.FindChild("Scroll View").GetComponent<UIPanel>();


		for (int i = 0; i < TOTAL_CATALOG_COUNT; i++){
			GameObject dragItem = dragPanel.ScrollItem[ i ];
//			CatalogUnitItem catalogItem = CatalogUnitItem.Inject(dragItem);
			catalogWidgetList.Add(emptyItem.GetComponent<UIWidget>());
			catalogItemTrans.Add(dragItem.transform);
		}

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

	private void InitPooler(){
		string path = "Prefabs/UI/UnitItem/CatalogNode";
		GameObject pooledSymbol = Resources.Load(path) as GameObject;
		catalogPooler = gameObject.AddComponent<NewObjectPooler>();
		catalogPooler.Init(pooledSymbol, 40);
		
		GameObject pooledObjectsParent = new GameObject();
		pooledObjectsParent.transform.parent = gameObject.transform;
		pooledObjectsParent.transform.localScale = new Vector3(1, 1, 1);
		
		foreach (var item in catalogPooler.pooledObjects) {
			item.transform.parent = pooledObjectsParent.transform;
            item.transform.localScale = new Vector3(1, 1, 1);
		}            
	}

	private bool CheckVisible(Transform trans){
		float x = trans.localPosition.x;
		float offset = uiPanel.clipRange.x / 2;
		return x - (offset - 320) >= 0 &&  x - (offset - 320) <= 640;
	}
	private void Update(){
		//Debug.Log("Update...");
		int debugCount = 300;
//		if (count >= debugCount) return;
		for(int i = 0; i < 300; i++){

//			bool itemVisible = uiPanel.IsVisible(catalogWidgetList[ i ]);
			bool itemVisible = CheckVisible(catalogItemTrans[ i ]);
                        
//			Debug.Log("ItemVisuble : " + i + itemVisible);
//			count ++;
			if(itemVisible){// need to display
				GameObject obj;
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
			else{
				if(catalogItemTrans[ i ].childCount >=1){
					GameObject childObj = catalogUnitItemList[ i ].transform.FindChild("CatalogNode").gameObject;
					childObj.SetActive(false);
				}
			}

		}
	}
        
}
