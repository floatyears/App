using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GuidePicPath{
	None,
	GoldBox,
	StarMove,
	ColorStarMove,
	FullBlock,
	FindKey,
	ChangeBlockOrder,
	HealBlock,
	HealSkill,
	Boost,
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

	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);

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
		
		
		UIEventListenerCustom.Get(btnRight.gameObject).onClick = ClickRightButton;
		UIEventListenerCustom.Get(btnLeft.gameObject).onClick = ClickLeftButton;
		UIEventListenerCustom.Get(btnCenter.gameObject).onClick = ClickCenterButton;
	}
	
	public override void ShowUI(){

		if (viewData != null && viewData.ContainsKey ("data")) {
			base.ShowUI();
			
			msgWindowParams = viewData["data"] as GuideWindowParams;;
			if (msgWindowParams.fullScreenClick) {
				clider.enabled = true;
			} else {
				clider.enabled = false;
			}
			
			titleLabel.text = msgWindowParams.titleText;
			
			UpdateGuidePic ();
			
			UpdateTitleLabel();
			UpdateLabels();
			UpdateBtnParams();	
		}else{
			ModuleManager.Instance.HideModule(ModuleEnum.NoviceMsgWindowModule);
		}

	}
	
	public override void HideUI(){
		titleLabel.text = string.Empty;

		Reset();
	}

	public override void DestoryUI () {
		UIEventListenerCustom.Get(btnRight.gameObject).onClick = null;
		UIEventListenerCustom.Get(btnLeft.gameObject).onClick = null;
		UIEventListenerCustom.Get(btnCenter.gameObject).onClick = null;
		msgWindowParams = null;
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
		SetButtonLabelText(btnCenter, TextCenter.GetText("NEXT"));
		
		msgLabelCenter.text = string.Empty;
		msgLabelTop.text = string.Empty;
		msgLabelBottom.text = string.Empty;

		msgLabelTop.width = msgLabelCenter.width = msgLabelBottom.width =  originWidth;
	}
	
	void ClickRightButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		if (btnRightParam != null){
			BtnParam bp = btnRightParam;
			
			DataListener callback = bp.callback;
			
			if (callback != null){
				callback(bp.args);
			}

			ModuleManager.Instance.HideModule(ModuleEnum.NoviceMsgWindowModule);
		}
		
	}
	
	void ClickLeftButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		if (btnLeftParam != null){
			BtnParam bp = btnLeftParam;
			DataListener callback = bp.callback;
			
			if (callback != null){
				callback(bp.args);
			}

			ModuleManager.Instance.HideModule(ModuleEnum.NoviceMsgWindowModule);
		}
		
	}
	
	void ClickCenterButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		if (btnCenterParam != null){
			BtnParam bp = btnCenterParam;
			DataListener callback = bp.callback;
			if (callback != null){
				callback(bp.args);
			}

			ModuleManager.Instance.HideModule(ModuleEnum.NoviceMsgWindowModule);
		}
		
	}
	
	void SetButtonLabelText(UIButton button, string text){
		if (text == null || text == ""){
			return;
		}
		UILabel label = button.transform.FindChild("Label").GetComponent<UILabel>();
		label.text = text;
	}
	
	void UpdateBtnCenterCallback(BtnParam btnParam){

		UIEventListenerCustom.Get (maskObj).onClick = ClickCenterButton;

		btnCenter.gameObject.SetActive(true);
		btnCenterParam = btnParam;
		SetButtonLabelText(btnCenter, btnParam.text);
	}
	
	void ResetBtnCallback(){
		UIEventListenerCustom.Get (maskObj).onClick = null;

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
			msgLabelCenter.text = text;
		}
		else if (texts != null) {
			if (texts.Length != 2){
				return;
			}
			msgLabelTop.text = texts[0];
			msgLabelBottom.text = texts[1];
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

}
