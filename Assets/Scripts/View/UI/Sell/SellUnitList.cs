using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SellUnitList : ConcreteComponent{

	List<UnitItemViewInfo> onSaleUnitList = new List<UnitItemViewInfo>();

	public SellUnitList(string uiName) : base( uiName ){}

	public override void ShowUI(){
		base.ShowUI();
		GetUnitCellViewList();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void Callback(object data){
		base.Callback(data);
	}

	void GetUnitCellViewList(){
		List<TUserUnit> userUnitList = new List<TUserUnit>();

		if (onSaleUnitList.Count > 0){
			//keep clear state before every refresh list
			onSaleUnitList.Clear();
		}

		userUnitList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		for (int i = 0; i < userUnitList.Count; i++){
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(userUnitList [i]);
			onSaleUnitList.Add(viewItem);
		}
		Debug.LogError("GetUnitCellViewList(), onSaleUnitList count is : " + onSaleUnitList.Count);
			
	}

	void CreateUnitList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", onSaleUnitList);
		ExcuteCallback(cbdArgs);
	}
	
	void DestoryUnitList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", onSaleUnitList);
		ExcuteCallback(cbdArgs);
	}
        
}

