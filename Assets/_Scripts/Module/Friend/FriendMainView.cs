using UnityEngine;
using System.Collections.Generic;

public class FriendMainView : ViewBase {
    Dictionary< GameObject, ModuleEnum > btns = new Dictionary< GameObject, ModuleEnum >();
    ModuleEnum nextScene;
	
    public override void Init(UIConfigItem config) {
        base.Init(config);
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
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_FriendList");
        btns.Add(go, ModuleEnum.FriendListModule);

        go = FindChild("Btn_SearchFriend");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Search");
		btns.Add(go, ModuleEnum.SearchFriendModule);

        go = FindChild("Btn_Apply");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Apply");
		btns.Add(go, ModuleEnum.ApplyModule);

        go = FindChild("Btn_Reception");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Reception");
		btns.Add(go, ModuleEnum.ReceptionModule);

        foreach (var btn in btns.Keys) {
            UIEventListener.Get(btn).onClick = ClickBtn;
        }

		bbproto.StatHelperCount data = DataCenter.Instance.HelperCount;
		if(data != null){
			FindChild<UILabel>("Top/Info").text = string.Format(TextCenter.GetText("Notice_HelperContent"),DataCenter.Instance.LoginInfo.LoginDayTotal, data.helpFriendCount,data.helpHelperCount,data.friendPointGet,DataCenter.Instance.AccountInfo.FriendPoint);//nItem.message;
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
				ModuleManger.Instance.ShowModule(nextScene);
            	break;
        }
    }

    void SyncFriendListFromServer() {
//        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("SyncFriendList", nextScene);
//        ExcuteCallback(cbdArgs);
		ModuleManger.SendMessage (ModuleEnum.FriendMainModule, "SyncFriendList", nextScene);
    }

    void TurnToNextScene() {
        ModuleManger.Instance.ShowModule(nextScene);
    }

    void ShowUIAnimation(){
       	gameObject.transform.localPosition = new Vector3(-1000, -645, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
    }

}
