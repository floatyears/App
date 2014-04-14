using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitListLogic : ConcreteComponent {
	public MyUnitListLogic(string uiName):base(uiName){}

	public override void ShowUI(){
		base.ShowUI();
		CreateOwnedUnitListView();
		RefreshItemCounter();
	}

	public override void HideUI(){
		base.HideUI();
		DestoryOwnedUnitListView();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
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
		//Debug.Log("GenerateOwnedUnitListView(), the total count of this player owned is " + tuuList.Count);
		return tuuList;
	}

	void CreateOwnedUnitListView(){
		if(GetOwnedUnitViewList() == null) return;
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("CreateDragPanelView", GetOwnedUnitDataList());
		ExcuteCallback(call);
	}

	void DestoryOwnedUnitListView(){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("DestoryDragPanelView", null);
		ExcuteCallback(call);
	}

	void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.Instace.GetCurrentText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.MyUnitList.Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}


}
