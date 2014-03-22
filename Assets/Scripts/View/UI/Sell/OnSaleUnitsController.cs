using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnSaleUnitsController : ConcreteComponent {
	int maxPickCount = 12;

	List<UnitItemViewInfo> onSaleUnitList = new List<UnitItemViewInfo>();
	List<TUserUnit> pickedUnitList = new List<TUserUnit>();

	public OnSaleUnitsController(string uiName):base(uiName) {}
	public override void CreatUI () { base.CreatUI (); }
	
	public override void ShowUI () {
		base.ShowUI ();
		GetUnitCellViewList();
		CreateOnSaleUnitViewList();
	}
	
	public override void HideUI () {
		base.HideUI ();
		DestoryOnSaleUnitViewList();
	}
	
	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "ClickItem" : 
				CallBackDispatcherHelper.DispatchCallBack(PickOnSaleUnit, cbdArgs);
				break;
			case "ClickSell" : 
				CallBackDispatcherHelper.DispatchCallBack(GiveSellNote, cbdArgs);
				break;
			case "ClickClear" : 
				CallBackDispatcherHelper.DispatchCallBack(ClearPickedUnits, cbdArgs);
				break;
			default:
				break;
		}
	}


	void GiveSellNote(object args){

	}

	void ClearPickedUnits(object args){
//		pickedUnitList.Clear();
//		for (int i = 0; i < pickedUnitList.Count; i++){
//			PickOnSaleUnit(i);
////			PickOnSaleUnit(i);
//		}
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
			//Debug.LogError(string.Format("ViewItem{[0]}'s IsEnable is : ", viewItem.IsEnable));
		}

		Debug.LogError("GetUnitCellViewList(), onSaleUnitList count is : " + onSaleUnitList.Count);
		
	}
	
	void CreateOnSaleUnitViewList(){

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", onSaleUnitList);
		ExcuteCallback(cbdArgs);
	}
	
	void DestoryOnSaleUnitViewList(){
		pickedUnitList.Clear();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", onSaleUnitList);
		ExcuteCallback(cbdArgs);
	}

	void PickOnSaleUnit(object args){
		int viewItemPos = (int)args;
		Debug.LogError("OnSaleUnitsController(), receive click, to pick the item index is "  + viewItemPos);
		UnitItemViewInfo clickItemInfo = onSaleUnitList[ viewItemPos ];
		TUserUnit tuu = clickItemInfo.DataItem;
		Debug.LogError("TUserUnit : " + (tuu == null));
		if(CanBeCancel(tuu))	
			CancelPick(viewItemPos, tuu);
		else if(CanBePick(clickItemInfo)) 
			Pick(viewItemPos, tuu);
		else
			Debug.LogError("neither canbe cancel nor canbepick");

	}

	bool CanBeCancel(TUserUnit info){
		Debug.LogError("CanBeCancel : .....");
		bool ret = false;
		ret = pickedUnitList.Contains(info);
		Debug.LogError("CanBeCancel(), ret is " + ret);
		return ret;

	}

	void CancelPick(int clickPos, TUserUnit info){
		Debug.LogError("CancelPick : .....");
		int poolPos = pickedUnitList.IndexOf(info);
		pickedUnitList[ poolPos ] = null;
		CancelShowUnit(clickPos, poolPos);
	}

	void Pick(int clickPos, TUserUnit info){
		Debug.LogError("Pick : .....");

		int firstEmptyIndex = -1;
		for (int i = 0; i < pickedUnitList.Count; i++){
			if(pickedUnitList[ i ] == null){
				firstEmptyIndex = i;
				break;
			}
		}
		Debug.LogError("Pick() firstEmptyIndex " + firstEmptyIndex);

		if(firstEmptyIndex == -1){
			pickedUnitList.Add(info);
			ShowPickedUnit(clickPos, pickedUnitList.Count - 1, info);
		}	
		else {
			pickedUnitList[ firstEmptyIndex ] = info;
			ShowPickedUnit(clickPos, firstEmptyIndex, info);
		}

	}

	bool CanActivateSellBtn(){
		bool canActivate = false;
		if(GetCurPickedUnitCount() > 0)	return true;
		else
			return false;
	}
	
	bool CanBePick(UnitItemViewInfo info){
		bool ret = true;
		if(info.IsParty || info.IsCollected) ret = false;
		else{
			if(GetCurPickedUnitCount() >= maxPickCount) ret = false;
		}
		return ret;
	}

	int GetCurPickedUnitCount(){
		int pickedCount = 0;
		for (int i = 0; i < pickedUnitList.Count; i++){
			if(pickedUnitList[ i ] !=null) 
				pickedCount++;
		}
		return pickedCount;
	}

	void ShowPickedUnit(int clickPos, int poolPos, TUserUnit tuu){
		Debug.LogError("ShowPickedUnit....");
		Texture2D tex2d = tuu.UnitInfo.GetAsset(UnitAssetType.Avatar);
		string level = tuu.Level.ToString();
		Dictionary<string, object> viewInfo = new Dictionary<string, object>();
		viewInfo.Add("poolPos", poolPos);
		viewInfo.Add("clickPos", clickPos);
		viewInfo.Add("texture", tex2d);
		viewInfo.Add("label", level);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("AddViewItem", viewInfo);
		ExcuteCallback(cbdArgs);

		CallBackDispatcherArgs canSellInfo = new CallBackDispatcherArgs("ButtonActive", CanActivateSellBtn());
		ExcuteCallback(canSellInfo);
	}

	void CancelShowUnit(int clickPos,int poolPos){
		Debug.LogError("CancelShowUnit....");
		Dictionary<string, int> info = new Dictionary<string, int>();
		info.Add("clickPos", clickPos);
		info.Add("poolPos", poolPos);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RmvViewItem", info);
		ExcuteCallback(cbdArgs);

		CallBackDispatcherArgs canSellInfo = new CallBackDispatcherArgs("ButtonActive", CanActivateSellBtn());
		ExcuteCallback(canSellInfo);
	}

}
