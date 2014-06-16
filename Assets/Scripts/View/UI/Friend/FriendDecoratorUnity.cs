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
        ShowUIAnimation();
    }
	
    public override void HideUI() {
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
        go = FindChild("ImgBtn_FriendList");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_FriendList");
        btns.Add(go, SceneEnum.FriendList);

        go = FindChild("ImgBtn_Information");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Information");
        btns.Add(go, SceneEnum.Information);

        go = FindChild("ImgBtn_SearchFriend");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Search");
        btns.Add(go, SceneEnum.SearchFriend);

        go = FindChild("ImgBtn_Apply");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Apply");
        btns.Add(go, SceneEnum.Apply);

        go = FindChild("ImgBtn_Reception");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Reception");
        btns.Add(go, SceneEnum.Reception);

        go = FindChild("ImgBtn_YourID");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_UserID");
        btns.Add(go, SceneEnum.YourID);

        foreach (var btn in btns.Keys) {
            UIEventListener.Get(btn).onClick = ClickBtn;
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
