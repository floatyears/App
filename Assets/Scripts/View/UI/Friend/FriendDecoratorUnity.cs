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
        ShowTween();
    }
	
    public override void HideUI() {
        base.HideUI();
    }
	
    public override void DestoryUI() {
        base.DestoryUI();
    }

    public override void Callback(object data) {
        base.Callback(data);
        
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

        go = FindChild("BtnList_Window/ImgBtn_FriendList");
        btns.Add(go, SceneEnum.FriendList);

        go = FindChild("BtnList_Window/ImgBtn_Information");
        btns.Add(go, SceneEnum.Information);

        go = FindChild("BtnList_Window/ImgBtn_SearchFriend");
        btns.Add(go, SceneEnum.SearchFriend);

        go = FindChild("BtnList_Window/ImgBtn_Apply");
        btns.Add(go, SceneEnum.Apply);

        go = FindChild("BtnList_Window/ImgBtn_Reception");
        btns.Add(go, SceneEnum.Reception);

        go = FindChild("BtnList_Window/ImgBtn_YourID");
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

    void ShowTween() {
        TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
        if (list == null)
            return;
        foreach (var tweenPos in list) {		
            if (tweenPos == null)
                continue;
            tweenPos.Reset();
            tweenPos.PlayForward();
        }
    }

}
