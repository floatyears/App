using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class CatalogView : ViewBase {
	private int TOTAL_CATALOG_COUNT = 50;

	private DragPanel dynamicDragPanel;
       
	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config, data);
		dynamicDragPanel = new DragPanel("CatalogDragPanel","Prefabs/UI/UnitItem/CatalogUnitPrefab",typeof(CatalogUnitItem), transform);

		List<UserUnit> catalogDataList =  new List<UserUnit>();
		for (int i = 0; i < TOTAL_CATALOG_COUNT; i++){
			UserUnit userUnit = new UserUnit();
			userUnit.level = 1;
			userUnit.exp = 0;
			userUnit.unitId = (uint)(i + 1);
			catalogDataList.Add(userUnit);
		}
		dynamicDragPanel.SetData<UserUnit>(catalogDataList);
	}
	
	public override void ShowUI () {
		base.ShowUI ();

	}
	
	public override void HideUI () {
		base.HideUI ();
	}

	public override void DestoryUI ()
	{
		dynamicDragPanel.DestoryUI ();
		base.DestoryUI ();
	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("CatalogCounterTitle"));
		countArgs.Add("current", GetCurGotUnitCount());
		countArgs.Add("max", TOTAL_CATALOG_COUNT);
		countArgs.Add("posy", -736);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private int GetCurGotUnitCount(){
		int gotCount = 0;
		for (int i = 1; i <= TOTAL_CATALOG_COUNT; i++){
			if(DataCenter.Instance.UnitData.CatalogInfo.IsHaveUnit((uint)i)){
				gotCount ++;
			}
		}
		return gotCount;
	}


	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			//			Debug.Log("Show Module!: [[[---" + config.moduleName + "---]]]pos: " + config.localPosition.x + " " + config.localPosition.y);
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
			iTween.MoveTo(gameObject, iTween.Hash("time", 0.4f, "x", config.localPosition.x, "islocal", true,"oncomplete","OnAniEnd","oncompletetarget",gameObject));
			//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
			//			Debug.Log("Hide Module!: [[[---" + config.moduleName + "---]]]");
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
			//			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true,"oncomplete","AnimationComplete","oncompletetarget",gameObject));
		}
	}

	void OnAniEnd(){
		dynamicDragPanel.RefreshUIPanel();
	}

}
