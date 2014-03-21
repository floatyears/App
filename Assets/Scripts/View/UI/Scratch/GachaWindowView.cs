using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GachaWindowView : UIComponentUnity {

    private UILabel chancesLabel;
    private UILabel titleLabel;
    private int originLayer;
    private bool displayingResult = false; // when 
    private int tryCount = 0;
    private GachaWindowInfo gachaInfo;

    private Dictionary<GameObject, int> buttonDic = new Dictionary<GameObject, int>();

    public override void Init ( UIInsConfig config, IUICallback origin ) {
        base.Init (config, origin);
        InitUI();
    }
    
    public override void ShowUI () {
        base.ShowUI ();
        SetMenuBtnEnable(false);
        AddListener();
    }
    
    public override void HideUI () {
        base.HideUI ();
        Reset();
        SetMenuBtnEnable(true);
        RemoveListener();
    }
    
    public override void DestoryUI () {
        base.DestoryUI ();
    }

    public void AddListener(){
        MsgCenter.Instance.AddListener(CommandEnum.EnterGachaWindow, Enter);
    }

    public void RemoveListener(){
        MsgCenter.Instance.RemoveListener(CommandEnum.EnterGachaWindow, Enter);
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
        chancesLabel = FindChild<UILabel>("TitleBackground/ChancesLabel");

        UIButton[] buttons = FindChild("Board/Buttons").GetComponentsInChildren< UIButton >();
        for (int i = 0; i < buttons.Length; i++){
            buttonDic.Add( buttons[i].gameObject, i );
            UIEventListener.Get( buttons[i].gameObject ).onClick = ClickButton;
        }

        originLayer = Main.Instance.NguiCamera.eventReceiverMask;
    }
    
    
    private void SetMenuBtnEnable(bool enable){
//        if (enable){
//            Main.Instance.NguiCamera.eventReceiverMask = LayerMask.NameToLayer("ScreenShelt") << 15;
//        }
//        else {
//            Main.Instance.NguiCamera.eventReceiverMask = originLayer;
//        }
    }

    private void SetTitleLabel(object args){
        string titleText = args as string;
        titleLabel.text = titleText;
    }

    private void Enter(object args){
        LogHelper.Log("Enter invoke SyncGachaInfos()");

        GachaWindowInfo gachaWindowInfo = args as GachaWindowInfo;
        if (gachaWindowInfo != null){
            gachaInfo = gachaWindowInfo;
            SyncGachaInfos(gachaWindowInfo);
        }
    }

    private void SyncGachaInfos(){
        chancesLabel.text = TextCenter.Instace.GetCurrentText("GachaChances", 0, gachaInfo.totalChances);
    }

    private void ClickButton(GameObject btn){
        int index = buttonDic[btn];
        LogHelper.Log("ClickButton() {0}", index);
    }

    private void Reset(){
        displayingResult = false;
        tryCount = 0;
    }
}
