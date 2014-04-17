using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatalogView : UIComponentUnity {
	private int startPageIndex = 0;
	private int endPageIndex;
	private int totalPageCount;


	private DragPanel dragPanel;
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitCatalogDragPanel();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		RefreshItemCounter();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	private int GetTotalPageNum(){
		//TODO GET FROM DATACENTER
		int num = 0;
		num = 3;
		return num;
	}

	private void InitCatalogDragPanel(){
		dragPanel = new DragPanel("CatalogDragPanel", CatalogUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		int catalogCount = 66;
		dragPanel.AddItem(catalogCount);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.CatalogDragPanelArgs, transform);
	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.Instace.GetCurrentText("CatalogCounterTitle"));
		countArgs.Add("current", dragPanel.ScrollItem.Count);
		countArgs.Add("max", 508);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

}
