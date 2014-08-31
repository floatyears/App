using UnityEngine;
using System.Collections.Generic;

public class FriendDecoratorUnity : UIComponentUnity {
    Dictionary< GameObject, SceneEnum > btns = new Dictionary< GameObject, SceneEnum >();
    SceneEnum nextScene;
	
    public override void Init(UIInsConfig config, IUICallback origin) {
        base.Init(config, origin);
        InitUI();
    }
	
    public override void ShowUI() {
        base.ShowUI();
//		Debug.LogError ("ShowUI : time :  " + Time.realtimeSinceStartup);
        ShowUIAnimation();
    }
	
    public override void HideUI() {
//		Debug.LogError ("HideUI : time :  " + Time.realtimeSinceStartup);
//		iTween.Stop (gameObject);
        base.HideUI();
    }
	
    public override void DestoryUI() {
        base.DestoryUI();
    }

    public override void CallbackView(object data) {
        base.CallbackView(data);
        
        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
        switch (cbdArgs.funcName){
        	case "TurnRequiredFriendListScene": 
            	CallBackDispatcherHelper.DispatchCallBack(TurnToNextScene, cbdArgs);
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
        btns.Add(go, SceneEnum.FriendList);

        go = FindChild("Btn_SearchFriend");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Search");
        btns.Add(go, SceneEnum.SearchFriend);

        go = FindChild("Btn_Apply");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Apply");
        btns.Add(go, SceneEnum.Apply);

        go = FindChild("Btn_Reception");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Reception");
        btns.Add(go, SceneEnum.Reception);

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
			case SceneEnum.Apply:
				SyncFriendListFromServer();
            	break;
        	case SceneEnum.FriendList:
            	SyncFriendListFromServer();
            	break;
        	case SceneEnum.Reception:
            	SyncFriendListFromServer();
            	break;
        	default:
				UIManager.Instance.ChangeScene(nextScene);
            	break;
        }
    }

    void SyncFriendListFromServer() {
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("SyncFriendList", nextScene);
        ExcuteCallback(cbdArgs);
    }

    void TurnToNextScene(object obj) {
        UIManager.Instance.ChangeScene(nextScene);
    }

    void ShowUIAnimation(){
       	gameObject.transform.localPosition = new Vector3(-1000, -645, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
    }

}
