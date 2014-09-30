using UnityEngine;
using System.Collections.Generic;

public class FriendMainView : ViewBase {
    Dictionary< GameObject, ModuleEnum > btns = new Dictionary< GameObject, ModuleEnum >();
    ModuleEnum nextScene;
	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null) {
        base.Init(config,data);
        InitUI();
    }

    private void InitUI() {
        GameObject go;
		UILabel btnLabel;
        go = FindChild("Btn_FriendList");
		btnLabel = FindChild<UILabel>("Btn_FriendList/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_FriendList");
        btns.Add(go, ModuleEnum.FriendListModule);

        go = FindChild("Btn_SearchFriend");
		btnLabel = FindChild<UILabel>("Btn_SearchFriend/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Search");
		btns.Add(go, ModuleEnum.SearchFriendModule);

        go = FindChild("Btn_Apply");
		btnLabel = FindChild<UILabel>("Btn_Apply/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Apply");
		btns.Add(go, ModuleEnum.ApplyModule);

        go = FindChild("Btn_Reception");
		btnLabel = FindChild<UILabel>("Btn_Reception/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Reception");
		btns.Add(go, ModuleEnum.ReceptionModule);

        foreach (var btn in btns.Keys) {
            UIEventListenerCustom.Get(btn).onClick = ClickBtn;
        }

		bbproto.StatHelperCount data = DataCenter.Instance.FriendData.HelperInfo;
		if(data != null){
			FindChild<UILabel>("Top/Info").text = string.Format(TextCenter.GetText("Notice_HelperContent"),DataCenter.Instance.UserData.LoginInfo.loginDayTotal, data.helpFriendCount,data.helpHelperCount,data.friendPointGet,DataCenter.Instance.UserData.AccountInfo.friendPoint);//nItem.message;
		}
    }

    void ClickBtn(GameObject btn) {
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

        nextScene = btns[btn];
        switch (nextScene) {
		case ModuleEnum.ApplyModule:
		case ModuleEnum.FriendListModule:
		case ModuleEnum.ReceptionModule:
			SyncFriendList();
	    	break;
		default:
			ModuleManager.Instance.ShowModule(nextScene);
	    	break;
        }
    }

	void SyncFriendList(){
		if (DataCenter.Instance.FriendData.Friend != null){
			TurnToNextScene ();
			return;
		}
		FriendController.Instance.GetFriendList(OnSyncFriendList,true,false);
	}
	
	void OnSyncFriendList(object data){
		Debug.Log("OnSyncFriendList, data = " + data);
		if (data == null)
			return;
		
		LogHelper.Log("TFriendList.Refresh() begin");
		LogHelper.Log(data);
		bbproto.RspGetFriend rsp = data as bbproto.RspGetFriend;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			Debug.LogError("Rsp code: "+rsp.header.code+", error:"+rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
		
		if (rsp.friends == null){
			LogHelper.Log("RspGetFriend getFriend null");
			return;
		}
		
		DataCenter.Instance.FriendData.RefreshFriendList(inst);
		TurnToNextScene ();
	}


    void TurnToNextScene() {
        ModuleManager.Instance.ShowModule(nextScene);
    }

    void ShowUIAnimation(){
       	gameObject.transform.localPosition = new Vector3(-1000, -645, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
    }

}
