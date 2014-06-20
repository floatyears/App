using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class SellController : ConcreteComponent {
	int maxPickCount = 12;
	int totalSaleValue = 0;
	List<TUserUnit> pickedUnitList = new List<TUserUnit>();

	public SellController(string uiName):base(uiName) {}
	public override void CreatUI () { 
		Debug.LogError("SellController.CreatUI()...1"); 
		base.CreatUI (); 
		Debug.LogError("SellController.CreatUI()...2");}
	
	public override void ShowUI () {
		Debug.LogError("SellController.ShowUI()...1");
		base.ShowUI ();
		Debug.LogError("SellController.ShowUI()...2");
	}
	
	public override void HideUI () {
		base.HideUI ();
		//base.DestoryUI ();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "ClickSell" : 
				CallBackDispatcherHelper.DispatchCallBack(PlanToSell, cbdArgs);
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
		base.HideUI ();
		SellView view = viewComponent as SellView;
		view.ResetUIState();
		base.ShowUI ();
	}

	void PlanToSell(object args){
		pickedUnitList = args as List<TUserUnit>;
		if(CheckPickedUnitRare()) 
			GiveRareWarning();
		else 
			GiveLastSaleEnsure();
	}

	void GiveLastSaleEnsure(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowLastSureWindow", pickedUnitList);
		ExcuteCallback(cbdArgs);
	}

	MsgWindowParams GetWarningMsgWindowParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("BigRareWarning");
		msgParams.contentText = TextCenter.GetText("BigRareWarningText");
		msgParams.btnParams = new BtnParam[ 2 ]{new BtnParam(), new BtnParam()};
		msgParams.btnParams[ 0 ].callback = CallbackOnSaleLastEnsure;
		return msgParams;
	}

	void CallbackReqSell(object msg){
		//Debug.LogError("CallbackReqSell().....");
		SellUnit.SendRequest(OnRspSellUnit, GetOnSaleUnitIDList());
	}

	void CallbackOnSaleLastEnsure(object msg){
		//Debug.LogError("CallbackOnSaleLastEnsure()...");
		GiveLastSaleEnsure();
	}

	void GiveRareWarning(){
		//Debug.LogError("GiveRareWarning...");
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetWarningMsgWindowParams());
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
		Debug.LogError("GetUnitCellViewList()...");
		List<TUserUnit> userUnitList = new List<TUserUnit>();	
		userUnitList.AddRange(DataCenter.Instance.UserUnitList.GetAllMyUnit());
		if(curUseUnitList == null){
			curUseUnitList = userUnitList;
			CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", curUseUnitList);
			ExcuteCallback(cbdArgs);
		}
		else if(!curUseUnitList.Equals(userUnitList)){
			curUseUnitList = userUnitList;
			CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", curUseUnitList);
			ExcuteCallback(cbdArgs);
		}
		else{
			Debug.LogError("CurUserUnitList NOT CHANGED, do nothing!");
			return;
		}


	}

	void DestoryOnSaleUnitViewList(){
		totalSaleValue = 0;
		pickedUnitList.Clear();
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
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateCoinLabel", value);
		ExcuteCallback(cbdArgs);
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
		tuu.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
			Texture2D tex2d = o as Texture2D;
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
		});

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

    public void ResetUI(){
        DestoryOnSaleUnitViewList();
        GetUnitCellViewList();
    }
    
}
