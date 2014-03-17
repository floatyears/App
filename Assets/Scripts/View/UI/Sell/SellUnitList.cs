using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SellUnitList : ConcreteComponent
{

	List<UnitItemViewInfo> myUnitList = new List<UnitItemViewInfo>();

	public SellUnitList(string uiName) : base( uiName )
	{
	}

	public override void ShowUI()
	{
		base.ShowUI();
	}

	public override void HideUI()
	{
		base.HideUI();
	}

	public override void Callback(object data)
	{
		base.Callback(data);
	}

	void GetUnitCellViewList()
	{
		List<TUserUnit> userUnitList = new List<TUserUnit>();

		if (myUnitList.Count > 0){
			//keep clear state before every refresh list
			myUnitList.Clear();
		}

		userUnitList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		for (int i = 0; i < userUnitList.Count; i++)
		{
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(userUnitList [i]);
			myUnitList.Add(viewItem);
		}
			
	}

	void CreateUnitList()
	{
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", myUnitList);
		ExcuteCallback(cbdArgs);
	}

	
	void DestoryUnitList()
	{
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", myUnitList);
		ExcuteCallback(cbdArgs);
	}
        
}

