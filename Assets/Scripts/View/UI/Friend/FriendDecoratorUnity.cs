using UnityEngine;
using System.Collections.Generic;

public class FriendDecoratorUnity : UIComponentUnity {

    private Dictionary< GameObject, SceneEnum > btns = new Dictionary< GameObject, SceneEnum >();
    private SceneEnum nextScene;
	
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
        
        switch (cbdArgs.funcName) {
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

    private void ClickBtn(GameObject btn) {
        Debug.Log("default Scene is " + nextScene);
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
            AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
            break;
        }
    }

    private void SyncFriendListFromServer() {
        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("SyncFriendList", nextScene);
        ExcuteCallback(cbdArgs);
    }

    private void TurnToNextScene(object obj) {
        UIManager.Instance.ChangeScene(nextScene);
    }

    private void ShowTween() {
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
