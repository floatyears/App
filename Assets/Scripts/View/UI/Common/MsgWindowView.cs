using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BtnParam {
    public DataListener callback;
    public string text = "";
    public object args;
}

public class MsgWindowParams {
    public BtnParam btnParam;
    public BtnParam[] btnParams;
    public string titleText;
    public string contentText;
    public string[] contentTexts;
}

public class MsgWindowView : UIComponentUnity{
    GameObject window;

    UILabel titleLabel;

    UILabel msgLabelCenter;
    UILabel msgLabelTop;
    UILabel msgLabelBottom;

    UIButton btnCenter;
    UIButton btnLeft;
    UIButton btnRight;

    BtnParam btnCenterParam;
    BtnParam btnLeftParam;
    BtnParam btnRightParam;

//    int originLayer;
    
    public override void Init(UIInsConfig config, IUICallback origin)
    {
        FindUIElement();
        
        base.Init(config, origin);
    }
    
    public override void ShowUI()
    {
        base.ShowUI();
		gameObject.SetActive (true);
        SetUIElement();
    }
    
    public override void HideUI()
    {
//        base.HideUI();
        ResetUIElement();
        ShowSelf(false);
    }
    
    public override void DestoryUI()
    {
        base.DestoryUI();
    }
    
    void FindUIElement()
    {
        window = FindChild("Window");
        btnLeft = FindChild<UIButton>("Window/Button_Left");
        btnRight = FindChild<UIButton>("Window/Button_Right");
        btnCenter = FindChild<UIButton>("Window/Button_Center");
        titleLabel = FindChild<UILabel>("Window/Label_Title");
        msgLabelCenter = FindChild<UILabel>("Window/Label_Msg_Center");

        msgLabelTop = FindChild<UILabel>("Window/Label_Msg_Top");
        msgLabelBottom = FindChild<UILabel>("Window/Label_Msg_Bottom");

        UIEventListener.Get(btnRight.gameObject).onClick = ClickRightButton;
        UIEventListener.Get(btnLeft.gameObject).onClick = ClickLeftButton;
        UIEventListener.Get(btnCenter.gameObject).onClick = ClickCenterButton;
//        originLayer = Main.Instance.NguiCamera.eventReceiverMask;
    }
    
    void ShowSelf(bool canShow){
        this.gameObject.SetActive(canShow);

        if (canShow){
			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.MessageWindow, true));
            window.transform.localScale = new Vector3(1f, 0f, 1f);
            iTween.ScaleTo(window, iTween.Hash("y", 1, "time", 0.4f, "easetype", iTween.EaseType.easeOutBounce));
        } 
		else{
//			Debug.LogError("ShowSelf false ");
            Reset();
			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.MessageWindow, false));
                        
        }
    }

    void Reset(){
        btnCenterParam = null;
        btnLeftParam = null;
        btnRightParam = null;

        btnLeft.gameObject.SetActive(false);
        btnRight.gameObject.SetActive(false);
        btnCenter.gameObject.SetActive(false);

        SetButtonLabelText(btnLeft, TextCenter.Instace.GetCurrentText("OK"));
        SetButtonLabelText(btnRight, TextCenter.Instace.GetCurrentText("Cancel"));
        SetButtonLabelText(btnCenter, TextCenter.Instace.GetCurrentText("OK"));

        msgLabelCenter.text = string.Empty;
        msgLabelTop.text = string.Empty;
        msgLabelBottom.text = string.Empty;
    }
	    
    void SetUIElement(){
        this.gameObject.SetActive(false);
        Reset();
    }
    
    void ResetUIElement(){
        titleLabel.text = string.Empty;
    }
    
    public override void CallbackView(object data){
        ShowSelf(true);  
        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        switch (cbdArgs.funcName){
        case "ShowMsg": 
            CallBackDispatcherHelper.DispatchCallBack(UpdateNotePanel, cbdArgs);
            break;
        default:
            break;
        }
    }
    
    void ClickRightButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        if (btnRightParam != null){
            DataListener callback = btnRightParam.callback;
            if (callback != null){
                callback(btnRightParam.args);
            }
        }
        ShowSelf(false);
    }
    
    void ClickLeftButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        if (btnLeftParam != null){
            DataListener callback = btnLeftParam.callback;
            if (callback != null){
                callback(btnLeftParam.args);
            }
        }
        ShowSelf(false);
    }

    void ClickCenterButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        if (btnCenterParam != null){
            DataListener callback = btnCenterParam.callback;
            if (callback != null){
                callback(btnCenterParam.args);
            }
        }
        ShowSelf(false);
    }

    void UpdateCenterLabel(string text){
        msgLabelCenter.text = text;
    }

    void UpdateTopBottomLabel(string[] texts){
        if (texts.Length != 2){
            return;
        }
        msgLabelTop.text = texts[0];
        msgLabelBottom.text = texts[1];
    }

    void SetButtonLabelText(UIButton button, string text){
        if (text == null || text == ""){
            return;
        }
        UILabel label = button.transform.FindChild("Label").GetComponent<UILabel>();
        label.text = text;
    }

    void UpdateBtnCenterCallback(BtnParam btnParam){
        btnCenter.gameObject.SetActive(true);
        btnCenterParam = btnParam;
        SetButtonLabelText(btnCenter, btnParam.text);
    }
    
    void ResetBtnCallback(){
        btnCenter.gameObject.SetActive(false);
        btnLeft.gameObject.SetActive(false);
        btnRight.gameObject.SetActive(false);
        btnCenterParam = null;
        btnLeftParam = null;
        btnRightParam = null;
    }


    void UpdateBtnLeftRightCallback(BtnParam[] btnParam){

        btnLeft.gameObject.SetActive(true);
        btnRight.gameObject.SetActive(true);

        if (btnParam == null || btnParam.Length != 2){
            return;
        }

        btnLeftParam = btnParam[0];
        btnRightParam = btnParam[1];
        SetButtonLabelText(btnLeft, btnParam[0].text);
        SetButtonLabelText(btnRight, btnParam[1].text);
    }
    
    void UpdateLabels(MsgWindowParams msgWindowParams){
        string text = msgWindowParams.contentText as string;
        string[] texts = msgWindowParams.contentTexts as string[];
        if (text != null) {
            UpdateCenterLabel(text);
        }
        else if (texts != null) {
            UpdateTopBottomLabel(texts);
        }
    }

    void UpdateBtnParams(MsgWindowParams msgWindowParams){
        BtnParam btnParam = msgWindowParams.btnParam as BtnParam;
        BtnParam[] btnParams = msgWindowParams.btnParams as BtnParam[];
        LogHelper.Log("btnParam {0}, btnParams {1}", btnParam, btnParams);
        ResetBtnCallback();
        if (btnParam != null){
            UpdateBtnCenterCallback(btnParam);
        }
        else{
            UpdateBtnLeftRightCallback(btnParams);
        }
    }
    
    void UpdateNotePanel(object args)
    {
        LogHelper.Log("UpdateNotePanel() start");
        MsgWindowParams msgWindowParams = args as MsgWindowParams;
//        Dictionary<string, object> msgWindowParams = args as Dictionary<string, object>;
        titleLabel.text = msgWindowParams.titleText;

        UpdateLabels(msgWindowParams);
        UpdateBtnParams(msgWindowParams);
    }
}
