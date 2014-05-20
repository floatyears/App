// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MsgWindowLogic : ConcreteComponent{
    public MsgWindowLogic(string uiName):base(uiName){
		AddListener ();
	}

    public override void ShowUI(){
        base.ShowUI();
      
    }
    
    public override void HideUI(){
        base.HideUI();
    }
    
    public override void DestoryUI(){
        base.DestoryUI();
		RemoveListener ();
    }
    
    void AddListener(){
//		Debug.LogError ("MsgWindowLogic : AddListener ");
        MsgCenter.Instance.AddListener(CommandEnum.OpenMsgWindow, OpenMsgWindow);
        MsgCenter.Instance.AddListener(CommandEnum.CloseMsgWindow, CloseMsgWindow);

    }
    
    
    void RemoveListener(){
//		Debug.LogError ("MsgWindowLogic : RemoveListener ");
        MsgCenter.Instance.RemoveListener(CommandEnum.OpenMsgWindow, OpenMsgWindow);
		MsgCenter.Instance.RemoveListener(CommandEnum.OpenMsgWindow, OpenMsgWindow);
    }
    
   void OpenMsgWindow(object msg){ 
//		Debug.LogError ("MsgWindowLogic : OpenMsgWindow ");
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowMsg", msg);
        ExcuteCallback(cbdArgs);
    }

    void CloseMsgWindow(object msg){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs ("CloseMsg", msg);
		ExcuteCallback (cbdArgs);
	}

}

