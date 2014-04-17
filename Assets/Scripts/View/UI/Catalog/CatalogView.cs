using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatalogView : UIComponentUnity {
	private int TOTAL_CATALOG_COUNT;
	private DragPanel dragPanel;
	private List<CatalogUnitItem> catalogUnitItemList = new List<CatalogUnitItem>();

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitCatalogDragPanel();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		RefreshItemCounter();
		RefreshCatalogView();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}

	private void InitCatalogDragPanel(){
		TOTAL_CATALOG_COUNT = GetTotalUnitCount();
		dragPanel = new DragPanel("CatalogDragPanel", CatalogUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(TOTAL_CATALOG_COUNT);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.CatalogDragPanelArgs, transform);

		for (int i = 0; i < TOTAL_CATALOG_COUNT; i++){
			GameObject dragItem = dragPanel.ScrollItem[ i ];
			CatalogUnitItem catalogItem = CatalogUnitItem.Inject(dragItem);
			catalogUnitItemList.Add(catalogItem);
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
		int totalUnitCount = 200;
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

}
