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
    public bool inputEnable = false;
	public bool fullScreenClick = false;
}

public class MsgWindowView : ViewBase{
    GameObject window;
    UILabel titleLabel;

    protected UILabel msgLabelCenter;
    protected UILabel msgLabelTop;
    protected UILabel msgLabelBottom;

    UIButton btnCenter;
    UIButton btnLeft;
    UIButton btnRight;

    BtnParam btnCenterParam;
    BtnParam btnLeftParam;
    BtnParam btnRightParam;

	BoxCollider clider;

    MsgWindowParams msgWindowParams = new MsgWindowParams();
    
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
        FindUIElement();
        base.Init(config, data);

		Reset();
    }
    
	public override void ShowUI ()
	{
		base.ShowUI ();

		if (!msgWindowParams.inputEnable){
			LogHelper.Log("open msgWindow and block input");
			ModuleManager.SendMessage(ModuleEnum.MaskModule, "block", new BlockerMaskParams(BlockerReason.MessageWindow, true));
		}
		else {
			SetLayerToBlocker(false);
		}
	}

    public override void HideUI(){

		base.HideUI ();
		Reset ();

		if (!msgWindowParams.inputEnable){
			LogHelper.Log("close msgWindow and resume input");
			ModuleManager.SendMessage(ModuleEnum.MaskModule, "block",  
			                          new BlockerMaskParams(BlockerReason.MessageWindow, false));
		}
		SetLayerToBlocker(true);
    }

	protected override void ToggleAnimation (bool isShow)
	{

		if (isShow) {
			Debug.Log("Show Module: " + config.moduleName);
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);
			window.transform.localScale = new Vector3(1f, 0f, 1f);
			iTween.ScaleTo(window, iTween.Hash("y", 1, "time", 0.4f, "easetype", iTween.EaseType.easeOutBounce));
			//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
			Debug.Log("Hide Module: " + config.moduleName);
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
			//			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true,"oncomplete","AnimationComplete","oncompletetarget",gameObject));
		}


	}
    
    protected virtual void FindUIElement(){
        window = FindChild("Window");
        btnLeft = FindChild<UIButton>("Window/Button_Left");
        btnRight = FindChild<UIButton>("Window/Button_Right");
        btnCenter = FindChild<UIButton>("Window/Button_Center");
        titleLabel = FindChild<UILabel>("Window/Label_Title");
        msgLabelCenter = FindChild<UILabel>("Window/Label_Msg_Center");

        msgLabelTop = FindChild<UILabel>("Window/Label_Msg_Top");
        msgLabelBottom = FindChild<UILabel>("Window/Label_Msg_Bottom");
		clider = GetComponent<BoxCollider> ();

        UIEventListener.Get(btnRight.gameObject).onClick = ClickRightButton;
        UIEventListener.Get(btnLeft.gameObject).onClick = ClickLeftButton;
        UIEventListener.Get(btnCenter.gameObject).onClick = ClickCenterButton;

    }


    protected virtual void Reset(){
		titleLabel.text = string.Empty;
		SetLayerToBlocker(true);

        btnCenterParam = null;
        btnLeftParam = null;
        btnRightParam = null;

        btnLeft.gameObject.SetActive(false);
        btnRight.gameObject.SetActive(false);
        btnCenter.gameObject.SetActive(false);

        SetButtonLabelText(btnLeft, TextCenter.GetText("OK"));
        SetButtonLabelText(btnRight, TextCenter.GetText("CANCEL"));
        SetButtonLabelText(btnCenter, TextCenter.GetText("OK"));

        msgLabelCenter.text = string.Empty;
        msgLabelTop.text = string.Empty;
        msgLabelBottom.text = string.Empty;
    }
    
    public override void CallbackView(params object[] args){
//        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (args[0].ToString()){
        case "show": 
            ShowMsgWindow(args[1]);
            break;
        case "hide": 
			HideUI ();
            break;
        default:
            break;
        }
    }

    void SetLayerToBlocker(bool toBlocker){
        LogHelper.Log("SetLayerToBlocker(), {0}", toBlocker);
        if (toBlocker){
            btnLeft.gameObject.layer = TouchEventBlocker.blockerLayer;
            btnRight.gameObject.layer = TouchEventBlocker.blockerLayer;
            btnCenter.gameObject.layer = TouchEventBlocker.blockerLayer;
        }
        else {
            btnLeft.gameObject.layer = TouchEventBlocker.defaultLayer;
            btnRight.gameObject.layer = TouchEventBlocker.defaultLayer;
            btnCenter.gameObject.layer = TouchEventBlocker.defaultLayer;
        }
    }
    
    void ClickRightButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        if (btnRightParam != null){
			BtnParam bp = btnRightParam;
//			ShowSelf(false);
			HideUI();
			DataListener callback = bp.callback;

            if (callback != null){
				callback(bp.args);
            }
        }
        
    }
    
    void ClickLeftButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        if (btnLeftParam != null){
			BtnParam bp = btnLeftParam;
			HideUI();
			DataListener callback = bp.callback;

            if (callback != null){
				callback(bp.args);
            }
        }
       
    }

    void ClickCenterButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        if (btnCenterParam != null){
			BtnParam bp = btnCenterParam;
			HideUI();
			DataListener callback = bp.callback;
            if (callback != null){
				callback(bp.args);
            }
        }
       
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
		UIEventListener.Get (gameObject).onClick = ClickCenterButton;

        btnCenter.gameObject.SetActive(true);
        btnCenterParam = btnParam;
        SetButtonLabelText(btnCenter, btnParam.text);
    }
    
    void ResetBtnCallback(){
		UIEventListener.Get (gameObject).onClick = null;

        btnCenter.gameObject.SetActive(false);
        btnLeft.gameObject.SetActive(false);
        btnRight.gameObject.SetActive(false);
        btnCenterParam = null;
        btnLeftParam = null;
        btnRightParam = null;
    }


    void UpdateBtnLeftRightCallback(BtnParam[] btnParam){
		UIEventListener.Get (gameObject).onClick = ClickLeftButton;

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
    
    void UpdateLabels(){
        string text = msgWindowParams.contentText as string;
        string[] texts = msgWindowParams.contentTexts as string[];
        if (text != null) {
            UpdateCenterLabel(text);
        }
        else if (texts != null) {
            UpdateTopBottomLabel(texts);
        }
    }

    void UpdateBtnParams(){
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

    void UpdateTitleLabel(){
        titleLabel.text = msgWindowParams.titleText;
    }

    public void ShowMsgWindow(object args){
        MsgWindowParams nextMsgWindowParams = args as MsgWindowParams;
        if (nextMsgWindowParams == null){
            return;
        }
        msgWindowParams = nextMsgWindowParams;
		if (msgWindowParams.fullScreenClick) {
			clider.enabled = true;
		} else {
			clider.enabled = false;
		}
        LogHelper.Log("UpdateNotePanel() start");
        titleLabel.text = msgWindowParams.titleText;

        UpdateTitleLabel();
        UpdateLabels();
        UpdateBtnParams();

		ShowUI ();
    }

	public UIButton BtnLeft
	{
		get{return btnLeft;}
		private set{ btnLeft = value;}
	}

	public UIButton BtnRight
	{
		get{return btnRight;}
		private set{ btnRight = value;}
	}

}
