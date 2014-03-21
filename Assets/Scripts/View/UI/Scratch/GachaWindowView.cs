using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GachaWindowView : UIComponentUnity {

    UILabel totalChancesLabel;
    UILabel titleLabel;
    bool displayingResult = false; // when 
    int tryCount = 0;
    private Dictionary<GameObject, int> buttonDic = new Dictionary<GameObject, int>();

    public override void Init ( UIInsConfig config, IUICallback origin ) {
        base.Init (config, origin);
        InitUI();
    }
    
    public override void ShowUI () {
        base.ShowUI ();
    }
    
    public override void HideUI () {
        base.HideUI ();
        Reset();
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
        case "SetTitleView": 
            CallBackDispatcherHelper.DispatchCallBack(SetTitleLabel, cbdArgs);
            break;
        default:
            break;
        }
    }
    
    private void InitUI() {
        titleLabel = FindChild<UILabel>("TitleBackground/TitleLabel");
        UIButton[] buttons = FindChild("Board/Buttons").GetComponentsInChildren< UIButton >();
        for (int i = 0; i < buttons.Length; i++){
            buttonDic.Add( buttons[i].gameObject, i );
            UIEventListener.Get( buttons[i].gameObject ).onClick = ClickButton;
        }
    }

    private void SetTitleLabel(object args){
        string titleText = args as string;
        titleLabel.text = titleText;
    }

    private void SyncGachaInfos(object args){
        Dictionary <string, int> gachaInfoDict = args as Dictionary<string, int>;
        if (gachaInfoDict != null){

        }
    }

    private void ClickButton(GameObject btn){
        int index = buttonDic[btn];
    }

    private void Reset(){
        displayingResult = false;
        tryCount = 0;
    }
}
