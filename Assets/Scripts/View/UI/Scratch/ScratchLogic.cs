using UnityEngine;
using System.Collections;

public class ScratchView : ConcreteComponent {
	
	public ScratchLogic(string uiName):base(uiName) {}

    public override void CreatUI () {
        LogHelper.Log("ScratchDecoratorUnity CreatUI Decddddddd1");
        base.CreatUI ();
        LogHelper.Log("ScratchDecoratorUnity CreatUI Decddddddd2");
    }

    public override void ShowUI () {
        LogHelper.Log("ScratchDecoratorUnity ShowUI Decddddddd1");
		base.ShowUI ();
        LogHelper.Log("ScratchDecoratorUnity ShowUI Decddddddd2");
	}
	
	public override void HideUI () {
		base.HideUI ();
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
        case "OpenFriendGachaWindow": 
            CallBackDispatcherHelper.DispatchCallBack(OpenFriendGachaWindow, cbdArgs);
            break;
        case "OpenRareGachaWindow": 
            CallBackDispatcherHelper.DispatchCallBack(OpenRareGachaWindow, cbdArgs);
            break;
        case "OpenEventGachaWindow": 
            CallBackDispatcherHelper.DispatchCallBack(OpenEventGachaWindow, cbdArgs);
            break;
        default:
            break;
        }
    }

    private void OpenFriendGachaWindow(object args){
        LogHelper.Log("OnFriendGacha() start");
    }

    private void OpenRareGachaWindow(object args){
        LogHelper.Log("OnRareGacha() start");
    }

    private void OpenEventGachaWindow(object args){
        LogHelper.Log("OnEventGacha() start");
    }
}