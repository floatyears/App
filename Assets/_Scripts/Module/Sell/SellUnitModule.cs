using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class SellUnitModule : ModuleBase {
	int maxPickCount = 12;
	int totalSaleValue = 0;
	List<TUserUnit> pickedUnitList = new List<TUserUnit>();

	public SellUnitModule(UIConfigItem config):base(  config) {
		CreateUI<SellUnitView> ();
	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		ResetUI();
	}

	public override void OnReceiveMessages(params object[] data){
		switch (data[0].ToString()){
			case "ClickSell" : 
				PlanToSell(data[1]);
				break;
			case "ClickSellOk" : 
				UnitController.Instance.SellUnit(OnRspSellUnit,GetOnSaleUnitIDList());
				break;
			case "ClickSellCancel" : 
				view.CallbackView("BackToMainWindow");
				break;
			default:
				break;
		}
	}

	private void OnRspSellUnit(object data) {
		if (data == null)
			return;

		bbproto.RspSellUnit rsp = data as bbproto.RspSellUnit;

		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		int money = rsp.money;
		int gotMoney = rsp.gotMoney;
		List<UserUnit> unitList = rsp.unitList;

		DataCenter.Instance.AccountInfo.Money = rsp.money;

		DataCenter.Instance.UserUnitList.DelMyUnitList(GetOnSaleUnitIDList());

		UpdateViewAfterRspSellUnit();

		AudioManager.Instance.PlayAudio(AudioEnum.sound_sold_out);
	}

	void UpdateViewAfterRspSellUnit(){
		pickedUnitList.Clear();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPlayerCoin, null);
//		base.HideUI ();
////		SellView view = view as SellView;
////		view.ResetUIState();
//		base.ShowUI ();

		ResetUI();

		view.CallbackView("BackToMainWindow");
		base.ShowUI ();
	}

	void PlanToSell(object args){
		pickedUnitList = args as List<TUserUnit>;
		if(CheckPickedUnitRare()) 
			GiveRareWarning();
		else 
			view.CallbackView("ShowLastSureWindow",pickedUnitList);
	}

	void GiveRareWarning(){
		//Debug.LogError("GiveRareWarning...");
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetWarningMsgWindowParams());
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("BigRareWarning"), TextCenter.GetText ("BigRareWarningText"), TextCenter.GetText ("OK"), TextCenter.GetText ("CANCEL"), o=>{
			view.CallbackView ("ShowLastSureWindow", pickedUnitList);
		});
	}

	List<uint> GetOnSaleUnitIDList(){
		List<uint> idList = new List<uint>();
		for (int i = 0; i < pickedUnitList.Count; i++){

			if(pickedUnitList[ i ] != null)
				idList.Add(pickedUnitList[ i ].ID);
		}
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
		return noteSignal;
	}


	private List<TUserUnit> curUseUnitList;
	void GetUnitCellViewList(){
//		Debug.LogError("GetUnitCellViewList()...");
		List<TUserUnit> userUnitList = DataCenter.Instance.UserUnitList.GetAllMyUnit(); //new List<TUserUnit>();	
		//userUnitList.AddRange(DataCenter.Instance.UserUnitList.GetAllMyUnit());
		if(curUseUnitList == null){
			curUseUnitList = userUnitList;
//			CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", curUseUnitList);
			view.CallbackView("CreateDragView", curUseUnitList);
		}
		else if(!curUseUnitList.Equals(userUnitList)){
			curUseUnitList = userUnitList;
//			CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", curUseUnitList);
			view.CallbackView("CreateDragView", curUseUnitList);
		}
		else{
			Debug.LogError("CurUserUnitList NOT CHANGED, do nothing!");
			return;
		}
	}
	
	bool CanBeCancel(TUserUnit info){
		bool ret = false;
		ret = pickedUnitList.Contains(info);
		return ret;
	}

	void CancelPick(int clickPos, TUserUnit info){
		int poolPos = pickedUnitList.IndexOf(info);
		pickedUnitList[ poolPos ] = null;
		CancelShowUnit(clickPos, poolPos);
		ChangeTotalSaleValue( -info.UnitInfo.SaleValue );
	}

	void ChangeTotalSaleValue(int value){
		totalSaleValue += value;
		UpdateSaleValueView(totalSaleValue);
	}

	void UpdateSaleValueView(int value){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateCoinLabel", value);
		view.CallbackView("UpdateCoinLabel", value);
	}

	void Pick(int clickPos, TUserUnit info){
		int firstEmptyIndex = -1;
		for (int i = 0; i < pickedUnitList.Count; i++){
			if(pickedUnitList[ i ] == null){
				firstEmptyIndex = i;
				break;
			}
		}

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
		ResourceManager.Instance.GetAvatar(UnitAssetType.Avatar,tuu.ID, o=>{
			Texture2D tex2d = o as Texture2D;
			string level = tuu.Level.ToString();
			Dictionary<string, object> viewInfo = new Dictionary<string, object>();
			viewInfo.Add("poolPos", poolPos);
			viewInfo.Add("clickPos", clickPos);
			viewInfo.Add("texture", tex2d);
			viewInfo.Add("label", level);
//			CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("AddViewItem", viewInfo);
			view.CallbackView("AddViewItem", viewInfo);
			
//			CallBackDispatcherArgs canSellInfo = new CallBackDispatcherArgs("ButtonActive", CanActivateSellBtn());
			view.CallbackView("ButtonActive", CanActivateSellBtn());
		});

	}

	void CancelShowUnit(int clickPos,int poolPos){
		Debug.LogError("CancelShowUnit....");
		Dictionary<string, int> info = new Dictionary<string, int>();
		info.Add("clickPos", clickPos);
		info.Add("poolPos", poolPos);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RmvViewItem", info);
		view.CallbackView("RmvViewItem", info);

//		CallBackDispatcherArgs canSellInfo = new CallBackDispatcherArgs("ButtonActive", CanActivateSellBtn());
		view.CallbackView("ButtonActive", CanActivateSellBtn());
	}

    void ResetUI(){
		totalSaleValue = 0;
		pickedUnitList.Clear();

        GetUnitCellViewList();
    }
    
}
