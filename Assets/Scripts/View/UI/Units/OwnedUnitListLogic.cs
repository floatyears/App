﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OwnedUnitListLogic : ConcreteComponent {
	public OwnedUnitListLogic(string uiName):base(uiName){}

	public override void ShowUI(){
		base.ShowUI();
		CreateOwnedUnitListView();
	}

	public override void HideUI(){
		base.HideUI();
		DestoryOwnedUnitListView();
	}

	public override void Callback(object data){
		base.Callback(data);
	}

	List<UnitItemViewInfo> GetOwnedUnitViewList(){
		if(GetOwnedUnitDataList() == null)	return null;

		List<TUserUnit> dataList = GetOwnedUnitDataList();
		List<UnitItemViewInfo> viewList = new List<UnitItemViewInfo>();
		for (int i = 0; i < dataList.Count; i++){
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(dataList[ i ]);
			viewList.Add(viewItem);
		}
		return viewList;
	}

	List<TUserUnit> GetOwnedUnitDataList(){
		List<TUserUnit> tuuList = new List<TUserUnit>();
		if(DataCenter.Instance.MyUnitList == null){
			Debug.LogError("!!!Data Read Error!!! DataCenter.Instance.MyUnitList is NULL, return null!");
			return null;
		}
		
		if(DataCenter.Instance.MyUnitList.GetAll() == null){
			Debug.LogError("!!!Data Read Error!!! DataCenter.Instance.MyUnitList.GetAll() is return null!");
			return null;
		}
		
		tuuList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		Debug.Log("GenerateOwnedUnitListView(), the total count of this player owned is " + tuuList.Count);
		return tuuList;
	}

	void CreateOwnedUnitListView(){
		if(GetOwnedUnitViewList() == null) return;
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("CreateDragPanelView", GetOwnedUnitViewList());
		ExcuteCallback(call);
	}

	void DestoryOwnedUnitListView(){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("DestoryDragPanelView", null);
		ExcuteCallback(call);
	}

}
