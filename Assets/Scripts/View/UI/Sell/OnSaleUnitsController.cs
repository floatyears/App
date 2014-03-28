using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class OnSaleUnitsController : ConcreteComponent {
	int maxPickCount = 12;
	int totalSaleValue = 0;

	List<UnitItemViewInfo> onSaleUnitList = new List<UnitItemViewInfo>();
	List<TUserUnit> pickedUnitList = new List<TUserUnit>();

	public OnSaleUnitsController(string uiName):base(uiName) {
    }
	public override void CreatUI () { base.CreatUI (); }
	
	public override void ShowUI () {
		base.ShowUI ();
//		CreateOnSaleUnitViewList();
//		RefreshOwnedUnitCount();
	}
	
	public override void HideUI () {
		base.HideUI ();
//		DestoryOnSaleUnitViewList();
	}

	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "ClickItem" : 
				CallBackDispatcherHelper.DispatchCallBack(PickOnSaleUnit, cbdArgs);
				break;
			case "PressItem": 
				CallBackDispatcherHelper.DispatchCallBack(ViewUnitDetailInfo, cbdArgs);
				break;
			case "ClickSell" : 
				CallBackDispatcherHelper.DispatchCallBack(PlanToSell, cbdArgs);
				break;
			case "ClickClear" : 
				CallBackDispatcherHelper.DispatchCallBack(ClearPickedUnits, cbdArgs);
				break;
			case "ClickSellOk" : 
				CallBackDispatcherHelper.DispatchCallBack(SubmitSell, cbdArgs);
				break;
			case "ClickSellCancel" : 
				CallBackDispatcherHelper.DispatchCallBack(CancelSell, cbdArgs);
				break;
			default:
				break;
		}
	}

	void SubmitSell(object args){
//		Debug.Log("SubmitSell()......");
		CallbackReqSell(null);
	}
	void ClearSellConfirmWindow(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("BackToMainWindow", null);
		ExcuteCallback(cbdArgs);
	}

	void CancelSell(object args){
		ClearSellConfirmWindow();
	}

	private void OnRspSellUnit(object data) {
		if (data == null)
			return;

		bbproto.RspSellUnit rsp = data as bbproto.RspSellUnit;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
//			LogHelper.Log("RspSellUnit code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		int money = rsp.money;
		int gotMoney = rsp.gotMoney;
		List<UserUnit> unitList = rsp.unitList;
//		LogHelper.Log("OnRspSellUnit() finished, money {0}, gotMoney {1}, unitList {2}" , money, gotMoney, unitList);

		DataCenter.Instance.AccountInfo.Money = rsp.money;
		
//		LogHelper.LogError("before sell, userUnitList count {0}", DataCenter.Instance.MyUnitList.GetAll().Count);
		DataCenter.Instance.MyUnitList.DelMyUnitList(GetOnSaleUnitIDList());
		DataCenter.Instance.UserUnitList.DelMyUnitList(GetOnSaleUnitIDList());

//		LogHelper.LogError("after sell, userUnitList count {0}", DataCenter.Instance.MyUnitList.GetAll().Count);
		UpdateViewAfterRspSellUnit();

		RefreshOwnedUnitCount();
		AudioManager.Instance.PlayAudio(AudioEnum.sound_sold_out);
	}

	void UpdateViewAfterRspSellUnit(){
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPlayerCoin, null);
		HideUI();
		OnSaleUnitsView view = viewComponent as OnSaleUnitsView;
		view.ResetUIState();
		ShowUI();
	}

	void PlanToSell(object args){
		if(CheckPickedUnitRare()) 
			GiveRareWarning();
		else 
			GiveLastSaleEnsure();
	}

	void GiveLastSaleEnsure(){
//		Debug.LogError("GiveLastSaleEnsure...");
		List<TUserUnit> readySaleList = GetReadySaleUnitList();

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowLastSureWindow", readySaleList);
		ExcuteCallback(cbdArgs);
	}

	MsgWindowParams GetWarningMsgWindowParams(){
		MsgWindowParams msgParams = new MsgWindowParams();

		msgParams.titleText = TextCenter.Instace.GetCurrentText("BigRareWarning");
		msgParams.contentText = TextCenter.Instace.GetCurrentText("BigRareWarningText");
		msgParams.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
		msgParams.btnParams[0].callback = CallbackOnSaleLastEnsure;

		return msgParams;
	}

	void CallbackReqSell(object msg){
//		Debug.LogError("CallbackReqSell().....");
		SellUnit.SendRequest(OnRspSellUnit, GetOnSaleUnitIDList());
	}

	void CallbackOnSaleLastEnsure(object msg){
//		Debug.LogError("CallbackOnSaleLastEnsure()...");
		GiveLastSaleEnsure();
	}

	void GiveRareWarning(){
//		Debug.LogError("GiveRareWarning...");
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetWarningMsgWindowParams());
	}

	List<uint> GetOnSaleUnitIDList(){
		List<uint> idList = new List<uint>();
		for (int i = 0; i < pickedUnitList.Count; i++){
//			Debug.LogError("GetOnSaleUnitIDList(), idList.Add(pickedUnitList[ i ].UnitID " + pickedUnitList[ i ].UnitID);

			if(pickedUnitList[ i ] != null)
				idList.Add(pickedUnitList[ i ].ID);
		}
//		Debug.LogError("GetOnSaleUnitIDList(), count is " + idList.Count);
		return idList;
	}

	List<TUserUnit> GetReadySaleUnitList(){
		List<TUserUnit> readySaleList = new List<TUserUnit>();
		for (int i = 0; i < pickedUnitList.Count; i++){
			if(pickedUnitList[ i ] != null)
				readySaleList.Add(pickedUnitList[ i ]);
		}
		return readySaleList;
	}

	bool CheckPickedUnitRare(){
		bool noteSignal = false;
		for (int i = 0; i < pickedUnitList.Count; i++){
			if(pickedUnitList[ i ] == null) continue;
			if(pickedUnitList[ i ].UnitInfo.Rare >= 3){
				noteSignal = true;
				break;
			}
		}
//		Debug.LogError("NoteSigbal is : " + noteSignal);
		return noteSignal;
	}

	void ClearPickedUnits(object args){
		for (int i = 0; i < onSaleUnitList.Count; i++){
			UnitItemViewInfo viewInfo = onSaleUnitList[i];
			if (CanBeCancel(viewInfo.DataItem)){
				CancelPick(i, viewInfo.DataItem);
			}
		}
	}

	void RefreshOwnedUnitCount(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.Instace.GetCurrentText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.MyUnitList.Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}


	void GetUnitCellViewList(){
		List<TUserUnit> userUnitList = new List<TUserUnit>();	
		if (onSaleUnitList.Count > 0) onSaleUnitList.Clear();
		userUnitList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		for (int i = 0; i < userUnitList.Count; i++){
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(userUnitList [i], true);
			onSaleUnitList.Add(viewItem);
		}

//		Debug.LogError("GetUnitCellViewList(), onSaleUnitList count is : " + onSaleUnitList.Count);
	}
	
	void CreateOnSaleUnitViewList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", onSaleUnitList);
		ExcuteCallback(cbdArgs);
	}
	
	void DestoryOnSaleUnitViewList(){
		totalSaleValue = 0;
		pickedUnitList.Clear();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", onSaleUnitList);
		ExcuteCallback(cbdArgs);
	}

	void PickOnSaleUnit(object args){
		int viewItemPos = (int)args;
//		Debug.LogError("OnSaleUnitsController(), receive click, to pick the item index is "  + viewItemPos);
		UnitItemViewInfo clickItemInfo = onSaleUnitList[ viewItemPos ];
		TUserUnit tuu = clickItemInfo.DataItem;
//		Debug.LogError("TUserUnit : " + (tuu == null));
		if(CanBeCancel(tuu))	
			CancelPick(viewItemPos, tuu);
		else if(CanBePick(clickItemInfo)) 
			Pick(viewItemPos, tuu);
		else
			Debug.LogError("neither canbe cancel nor canbepick");

	}

	bool CanBeCancel(TUserUnit info){
//		Debug.LogError("CanBeCancel : .....");
		bool ret = false;
		ret = pickedUnitList.Contains(info);
//		Debug.LogError("CanBeCancel(), ret is " + ret);
		return ret;
	}

	void CancelPick(int clickPos, TUserUnit info){
//		Debug.LogError("CancelPick : .....");
		int poolPos = pickedUnitList.IndexOf(info);
		pickedUnitList[ poolPos ] = null;
		CancelShowUnit(clickPos, poolPos);
		ChangeTotalSaleValue( -info.UnitInfo.SaleValue );
	}

	void ChangeTotalSaleValue(int value){
//		Debug.LogError("ChangeTotalSaleValue(), before TotalValue is " + totalSaleValue);
		totalSaleValue += value;
//		Debug.LogError("ChangeTotalSaleValue(), after TotalValue is " + totalSaleValue);
		UpdateSaleValueView(totalSaleValue);
	}

	void UpdateSaleValueView(int value){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateCoinLabel", value);
		ExcuteCallback(cbdArgs);
	}

	void Pick(int clickPos, TUserUnit info){
//		Debug.LogError("Pick : .....");
		int firstEmptyIndex = -1;
		for (int i = 0; i < pickedUnitList.Count; i++){
			if(pickedUnitList[ i ] == null){
				firstEmptyIndex = i;
				break;
			}
		}

//		Debug.LogError("Pick() firstEmptyIndex " + firstEmptyIndex);
		int pickedIndex;
		if(firstEmptyIndex == -1){
			pickedUnitList.Add(info);
			pickedIndex = pickedUnitList.Count - 1;
		}	
		else {
			pickedUnitList[ firstEmptyIndex ] = info;
			pickedIndex = firstEmptyIndex;
		}
		ShowPickedUnit(clickPos, pickedIndex, info);
		ChangeTotalSaleValue(info.UnitInfo.SaleValue);
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
//		Debug.LogError("ShowPickedUnit....");
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

	void ViewUnitDetailInfo(object args){
		int position = (int)args;
		TUserUnit tuu = onSaleUnitList [position].DataItem;
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, tuu);
	}

	void RefreshCounter(){
		Debug.Log("OnSaleUnitList.RefreshCounter(), start...");
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = "" ;
		int current = 0;
		int max = 0;
		
		title = TextCenter.Instace.GetCurrentText("UnitCounterTitle");
		current = DataCenter.Instance.MyUnitList.Count;
		max = DataCenter.Instance.UserInfo.UnitMax;
		
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

    public void ResetUI(){
        DestoryOnSaleUnitViewList();
        GetUnitCellViewList();
        CreateOnSaleUnitViewList();
        RefreshOwnedUnitCount();
		RefreshCounter();
    }
    
}
