using UnityEngine;
using System.Collections;

public sealed class GuidePicPath{

	private readonly string path;

	public static readonly GuidePicPath GoldBox = new GuidePicPath ("GoldBox");
	public static readonly GuidePicPath StarMove= new GuidePicPath ("StarMove");
	public static readonly GuidePicPath ColorStarMove= new GuidePicPath ("ColorStarMove");
	public static readonly GuidePicPath FullBlock= new GuidePicPath ("FullBlock");
	public static readonly GuidePicPath FindKey= new GuidePicPath ("FindKey");
	public static readonly GuidePicPath ChangeBlockOrder= new GuidePicPath ("ChangeBlockOrder");
	public static readonly GuidePicPath HealBlock= new GuidePicPath ("HealBlock");
	public static readonly GuidePicPath HealSkill= new GuidePicPath ("HealSkill");
	public static readonly GuidePicPath Boost= new GuidePicPath ("Boost");


	private GuidePicPath(string p){
		path = p;
	}

	public override string ToString ()
	{
		return path;
	}
}

public class GuideWindowParams{
	public BtnParam btnParam;
	public BtnParam[] btnParams;
	public string titleText;
	public string contentText;
	public string[] contentTexts;
	public bool inputEnable = false;
	public GuidePicPath guidePic;
	public bool fullScreenClick = true;
}

public class NoviceMsgWindowView : ViewBase{

	UISprite guidePicTex;
	GameObject guideAtlas;

	GameObject window;
	
	UILabel titleLabel;
	
	UILabel msgLabelCenter;
	UILabel msgLabelTop;
	UILabel msgLabelBottom;
	
	UIButton btnCenter;
	UIButton btnLeft;
	UIButton btnRight;
	
	UISprite mask;
	
	BtnParam btnCenterParam;
	BtnParam btnLeftParam;
	BtnParam btnRightParam;

	BoxCollider clider;
	GameObject maskObj;

	public bool isShow;

	GuideWindowParams msgWindowParams = new GuideWindowParams();

	private int originWidth = 560;
	private int chagnedWidth = 360;

	
	public override void Init(UIConfigItem config){
		FindUIElement();
		base.Init(config);
	}
	
	public override void ShowUI(){
		base.ShowUI();
		SetUIElement();
	}
	
	public override void HideUI(){
		//        base.HideUI();
		//		base.ShowUI ();
		ResetUIElement();
		ShowSelf(false);
	}

	public override void DestoryUI () {
//		Debug.LogError ("DestoryUI : " + guideAtlas);
//		DestroyImmediate (guideAtlas);
//		Resources.UnloadAsset (guideAtlas);
	}
	
	void FindUIElement(){
		window = FindChild("Window");
		mask = FindChild<UISprite>("Mask");

		guidePicTex = FindChild<UISprite> ("Window/TipPic");
		guideAtlas = guidePicTex.atlas.gameObject;

		btnLeft = FindChild<UIButton>("Window/Button_Left");
		btnRight = FindChild<UIButton>("Window/Button_Right");
		btnCenter = FindChild<UIButton>("Window/Button_Center");
		titleLabel = FindChild<UILabel>("Window/Label_Title");
		msgLabelCenter = FindChild<UILabel>("Window/Label_Msg_Center");
		
		msgLabelTop = FindChild<UILabel>("Window/Label_Msg_Top");
		msgLabelBottom = FindChild<UILabel>("Window/Label_Msg_Bottom");

		maskObj = FindChild ("Mask").gameObject;
		clider = maskObj.GetComponent<BoxCollider> ();


		UIEventListener.Get(btnRight.gameObject).onClick = ClickRightButton;
		UIEventListener.Get(btnLeft.gameObject).onClick = ClickLeftButton;
		UIEventListener.Get(btnCenter.gameObject).onClick = ClickCenterButton;
	}
	
	void ShowSelf(bool canShow){
		this.gameObject.SetActive(canShow);
		if (canShow){
//			if (!msgWindowParams.inputEnable){
//				LogHelper.Log("open msgWindow and block input");
//				MsgCenter.Instance.Invoke(CommandEnum.SetBlocker,
//				                          new BlockerMaskParams(BlockerReason.MessageWindow, true));
			if(!isShow)
				TouchEventBlocker.Instance.SetState(BlockerReason.NoviceGuide,true);
//			}
//			else {
//				SetLayerToBlocker(false);
//			}
			LogHelper.Log("open msgWindow showSelf true");
			window.transform.localScale = new Vector3(1f, 0f, 1f);
			iTween.ScaleTo(window, iTween.Hash("y", 1, "time", 0.4f, "easetype", iTween.EaseType.easeOutBounce));
		} 
		else{
			Reset();
//			if (!msgWindowParams.inputEnable){
//				LogHelper.Log("close msgWindow and resume input");
//				MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, 
//				                          new BlockerMaskParams(BlockerReason.MessageWindow, false));
			//there may be some conditions that the ShowSelf execute more than once with the same canShow, then the layer may be invalid.
			if(isShow)
				TouchEventBlocker.Instance.SetState(BlockerReason.NoviceGuide,false);
//			}
//			SetLayerToBlocker(true);
			LogHelper.Log("open msgWindow showSelf false");
		}
		isShow = canShow;
	}
	
	void Reset(){
		btnCenterParam = null;
		btnLeftParam = null;
		btnRightParam = null;
		
		btnLeft.gameObject.SetActive(false);
		btnRight.gameObject.SetActive(false);
		btnCenter.gameObject.SetActive(false);

		guidePicTex.gameObject.SetActive (false);
		
		SetButtonLabelText(btnLeft, TextCenter.GetText("OK"));
		SetButtonLabelText(btnRight, TextCenter.GetText("Cancel"));
		SetButtonLabelText(btnCenter, TextCenter.GetText("OK"));
		
		msgLabelCenter.text = string.Empty;
		msgLabelTop.text = string.Empty;
		msgLabelBottom.text = string.Empty;

		msgLabelTop.width = msgLabelCenter.width = msgLabelBottom.width =  originWidth;
	}
	
	void SetUIElement(){
		this.gameObject.SetActive(false);
		Reset();
	}
	
	void ResetUIElement(){
		titleLabel.text = string.Empty;
		SetLayerToBlocker(true);
	}
	
	public override void CallbackView(params object[] args){

		//LogHelper.Log ("novice msg window callback");
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (args[0].ToString()){
		case "ShowMsg": 
			ShowMsgWindow(args[1]);
			break;
		case "CloseMsg": 
			CloseMsgWindow(args[1]);
			break;
		default:
			break;
		}
	}
	
	void SetLayerToBlocker(bool toBlocker){
		LogHelper.Log("SetLayerToBlocker() before, {0}", Main.Instance.NguiCamera.eventReceiverMask.ToString());
		LogHelper.Log("SetLayerToBlocker(), {0}", toBlocker);
		if (toBlocker){
			mask.gameObject.SetActive(true);
//			btnLeft.gameObject.layer = TouchEventBlocker.blockerLayer;
//			btnRight.gameObject.layer = TouchEventBlocker.blockerLayer;
//			btnCenter.gameObject.layer = TouchEventBlocker.blockerLayer;
		}
		else {
			mask.gameObject.SetActive(false);
//			btnLeft.gameObject.layer = TouchEventBlocker.defaultLayer;
//			btnRight.gameObject.layer = TouchEventBlocker.defaultLayer;
//			btnCenter.gameObject.layer = TouchEventBlocker.defaultLayer;
		}
	}
	
	void ClickRightButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		if (btnRightParam != null){
			BtnParam bp = btnRightParam;
			ShowSelf(false);
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
			ShowSelf(false);
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
			ShowSelf(false);
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

		UIEventListener.Get (maskObj).onClick = ClickCenterButton;

		btnCenter.gameObject.SetActive(true);
		btnCenterParam = btnParam;
		SetButtonLabelText(btnCenter, btnParam.text);
	}
	
	void ResetBtnCallback(){
		UIEventListener.Get (maskObj).onClick = null;

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

	void UpdateGuidePic(){
		if (msgWindowParams.guidePic != null) {
//			Debug.Log("show novice guide msg window with the turexture:"+msgWindowParams.guidePic.ToString ());
			guidePicTex.gameObject.SetActive(true);

			guidePicTex.spriteName = msgWindowParams.guidePic.ToString (); //.mainTexture = tex;
//			guidePicTex.width = guidePicTex.mainTexture.width;
//			guidePicTex.height = guidePicTex.mainTexture.height;
			guidePicTex.MakePixelPerfect();

			msgLabelTop.width = msgLabelCenter.width = msgLabelBottom.width = 529 - guidePicTex.width;
			ModifyThePos(msgLabelTop.gameObject.transform,-137 + guidePicTex.width/4);
			ModifyThePos(msgLabelBottom.gameObject.transform,-137 + guidePicTex.width/4);
			ModifyThePos(msgLabelCenter.gameObject.transform,-137 + guidePicTex.width/4);
//			ResourceManager.Instance.LoadLocalAsset ("Texture/NoviceGuide/" + msgWindowParams.guidePic.ToString (), o =>{
//				Texture2D tex = o as Texture2D;
//				if(tex == null)
//					LogHelper.Log("guide texture: null");
//
//			});

		} else {
			guidePicTex.gameObject.SetActive(false);
			msgLabelTop.width = msgLabelCenter.width = msgLabelBottom.width =  originWidth;
			ModifyThePos(msgLabelTop.gameObject.transform,-274.0f);
			ModifyThePos(msgLabelBottom.gameObject.transform,-274.0f);
			ModifyThePos(msgLabelCenter.gameObject.transform,-274.0f);
		}
	}

	void ModifyThePos(Transform trans,float x){
		trans.localPosition = new Vector3 (x, trans.localPosition.y, trans.localPosition.z);
	}
	
	void ShowMsgWindow(object args){



		GuideWindowParams nextMsgWindowParams = args as GuideWindowParams;
		if (nextMsgWindowParams == null){
			return;
		}


//		LogHelper.Log ("show novice msg window"+args);
		msgWindowParams = nextMsgWindowParams;
		if (msgWindowParams.fullScreenClick) {
			clider.enabled = true;
		} else {
			clider.enabled = false;
		}

		ShowSelf(true);  
		LogHelper.Log("UpdateNotePanel() start");
		titleLabel.text = msgWindowParams.titleText;

		UpdateGuidePic ();

		UpdateTitleLabel();
		UpdateLabels();
		UpdateBtnParams();

		//LogHelper.Log("show novice guide msg window!" );
	}
	
	void CloseMsgWindow(object args){
		ShowSelf(false);
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
