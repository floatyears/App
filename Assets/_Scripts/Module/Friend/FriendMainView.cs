using UnityEngine;
using System.Collections.Generic;

public class FriendMainView : ViewBase {
    Dictionary< GameObject, ModuleEnum > btns = new Dictionary< GameObject, ModuleEnum >();
    ModuleEnum nextScene;
	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null) {
        base.Init(config,data);
        InitUI();
    }

    public override void CallbackView(params object[] args) {
//        base.CallbackView(data);
//        
//        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
        switch (args[0].ToString()){
        	case "TurnRequiredFriendListScene": 
            	TurnToNextScene();
           		break;
        	default:
            	break;
        }
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
				SyncFriendListFromServer();
            	break;
        	case ModuleEnum.FriendListModule:
            	SyncFriendListFromServer();
            	break;
			case ModuleEnum.ReceptionModule:
            	SyncFriendListFromServer();
            	break;
        	default:
				ModuleManager.Instance.ShowModule(nextScene);
            	break;
        }
    }

    void SyncFriendListFromServer() {
//        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("SyncFriendList", nextScene);
//        ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.FriendMainModule, "SyncFriendList", nextScene);
    }

    void TurnToNextScene() {
        ModuleManager.Instance.ShowModule(nextScene);
    }

    void ShowUIAnimation(){
       	gameObject.transform.localPosition = new Vector3(-1000, -645, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
    }

}
