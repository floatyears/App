using UnityEngine;
using System.Collections;

public class ShopComponent : ConcreteComponent {
	
	public ShopComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
        AddListener();
	}
	
	public override void HideUI () {
		base.HideUI ();
        RemoveListener();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

    public override void Callback(object data)
    {
        base.Callback(data);
        
        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
        switch (cbdArgs.funcName)
        {
        case "DoFriendExpansion": 
            CallBackDispatcherHelper.DispatchCallBack(OnFriendExpansion, cbdArgs);
            break;
        case "DoStaminaRecover": 
            CallBackDispatcherHelper.DispatchCallBack(OnStaminaRecover, cbdArgs);
            break;
        case "DoUnitExpansion": 
            CallBackDispatcherHelper.DispatchCallBack(OnUnitExpansion, cbdArgs);
            break;
        default:
            break;
        }
    }

    void AddListener(){
        MsgCenter.Instance.AddListener(CommandEnum.FriendExpansion, DoFriendExpansion);
        MsgCenter.Instance.AddListener(CommandEnum.StaminaRecover, DoStaminaRecover);          
        MsgCenter.Instance.AddListener(CommandEnum.UnitExpansion, DoUnitExpansion);
    }

    void RemoveListener(){
        MsgCenter.Instance.RemoveListener(CommandEnum.FriendExpansion, DoFriendExpansion);
        MsgCenter.Instance.RemoveListener(CommandEnum.StaminaRecover, DoStaminaRecover);          
        MsgCenter.Instance.RemoveListener(CommandEnum.UnitExpansion, DoUnitExpansion);
    }


    MsgWindowParams GetFriendExpansionMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();

        msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("FriendExpand");

        string context1 = TextCenter.Instace.GetCurrentText("ConfirmFriendExpansion");
        string context2 = TextCenter.Instace.GetCurrentText("FriendExpansionInfo", DataCenter.Instance.FriendList.Friend.Count,
                                                          DataCenter.Instance.UserInfo.FriendMax);

        msgWindowParam.contentTexts = new string[2]{ context1, context2 };
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackFriendExpansion;
        msgWindowParam.btnParams[0].text = TextCenter.Instace.GetCurrentText("DoFriendExpand");
        return msgWindowParam;
    }

    void CallbackFriendExpansion(object args){
        MsgCenter.Instance.Invoke(CommandEnum.FriendExpansion);
    }

    void OnFriendExpansion(object args){
        LogHelper.Log("start OnFriendExpansion()");
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetFriendExpansionMsgWindowParams());
    }

    void DoFriendExpansion(object args){
        FriendMaxExpand.SendRequest(OnRspFriendExpansion);
    }

    void OnRspFriendExpansion(object data){
        if (data == null)
            return;
        
        LogHelper.Log("OnRspFriendExpansion begin");
        LogHelper.Log(data);
        bbproto.RspFriendMaxExpand rsp = data as bbproto.RspFriendMaxExpand;
        
        if (rsp.header.code != (int)ErrorCode.SUCCESS)
        {
            LogHelper.Log("OnRspFriendExpansion code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            return;
        }
        
        DataCenter.Instance.UserInfo.FriendMax = rsp.friendMax;
        DataCenter.Instance.AccountInfo.Stone = rsp.stone;

        HideUI();
        ShowUI(); 
    }

    void OnStaminaRecover(object args){
        LogHelper.Log("start OnStaminaRecover()");
    }

    void DoStaminaRecover(object args){
        RestoreStamina.SendRequest(OnRspStartminaRecover);
    }

    void OnRspStartminaRecover(object data){
        if (data == null)
            return;
        
        LogHelper.Log("OnRspStartminaRecover() begin");
        LogHelper.Log(data);
        bbproto.RspRestoreStamina rsp = data as bbproto.RspRestoreStamina;
        
        if (rsp.header.code != (int)ErrorCode.SUCCESS)
        {
            LogHelper.Log("OnRspStartminaRecover code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            return;
        }
        
        DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;
        DataCenter.Instance.UserInfo.StaminaMax = rsp.staminaMax;
        DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;

        DataCenter.Instance.AccountInfo.Stone = rsp.stone;
        
        HideUI();
        ShowUI(); 
    }

    void OnUnitExpansion(object args){
        LogHelper.Log("start OnUnitExpansion()");
    }

    void DoUnitExpansion(object args){
        UnitMaxExpand.SendRequest(OnRspUnitExpansion);
    }

    void OnRspUnitExpansion(object data){
        if (data == null)
            return;
        
        LogHelper.Log("OnRspUnitExpansion() begin");
        LogHelper.Log(data);
        bbproto.RspUnitMaxExpand rsp = data as bbproto.RspUnitMaxExpand;
        
        if (rsp.header.code != (int)ErrorCode.SUCCESS)
        {
            LogHelper.Log("RspUnitMaxExpand code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            return;
        }
        
        DataCenter.Instance.UserInfo.UnitMax = rsp.unitMax;
        DataCenter.Instance.AccountInfo.Stone = rsp.stone;
        
        HideUI();
        ShowUI(); 
    }

}
