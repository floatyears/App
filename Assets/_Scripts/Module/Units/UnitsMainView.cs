using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class UnitsMainView : ViewBase, IDragChangeView{
	private UIButton prePageBtn;
	private UIButton nextPageBtn;
	private UISprite pageIndexSpr;
//	IUICallback iuiCallback;
	private GameObject topRoot;
	private GameObject bottomRoot;

	private Dictionary<GameObject,ModuleEnum> buttonInfo = new Dictionary<GameObject, ModuleEnum>();

	private DragSliderBase dragChangeView;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);

		GameObject go;
		UILabel btnLabel;
		
		go = FindChild("Bottom/Catalog");
		btnLabel = FindChild<UILabel>("Bottom/Catalog/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Catalog");
		buttonInfo.Add(go, ModuleEnum.CatalogModule);
		
		go = FindChild("Bottom/Evolve");
		btnLabel = FindChild<UILabel>("Bottom/Evolve/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Evolve");
		buttonInfo.Add(go, ModuleEnum.EvolveModule);
		
		go = FindChild("Bottom/LevelUp");
		btnLabel = FindChild<UILabel>("Bottom/LevelUp/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_LevelUp");
		buttonInfo.Add(go, ModuleEnum.LevelUpModule);
		
		go = FindChild("Bottom/Party");
		btnLabel = FindChild<UILabel>("Bottom/Party/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Party");
		buttonInfo.Add(go, ModuleEnum.PartyModule);
		
		go = FindChild("Bottom/Sell");
		btnLabel = FindChild<UILabel>("Bottom/Sell/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Sell");
		buttonInfo.Add(go, ModuleEnum.SellUnitModule);
		
		go = FindChild("Bottom/UnitList");
		btnLabel = FindChild<UILabel>("Bottom/UnitList/Label");
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_UnitList");
		buttonInfo.Add(go, ModuleEnum.MyUnitsListModule);
		
		dragChangeView = FindChild<DragSliderBase> ("Top/DragPartyUnits");
		dragChangeView.SetDataInterface (this);
		//		Debug.LogError ("InitChildScenes dragChangeView.Init ");
		//		dragChangeView.Init ();
		
		foreach (var item in buttonInfo.Keys)
			UIEventListenerCustom.Get(item).onClick = OnClickCallback;

		topRoot = transform.FindChild("Top").gameObject;
		bottomRoot = transform.FindChild("Bottom").gameObject;
		pageIndexSpr = transform.FindChild("Top/Sprite_Page_Index").GetComponent<UISprite>();
		prePageBtn = FindChild<UIButton>("Top/Button_Left");
		UIEventListenerCustom.Get(prePageBtn.gameObject).onClick = PrevPage;
		nextPageBtn = FindChild<UIButton>("Top/Button_Right");
		UIEventListenerCustom.Get(nextPageBtn.gameObject).onClick = NextPage;
	}
	
	public override void ShowUI(){
		base.ShowUI();
//		RefreshParty();

		int curPartyIndex = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId + 1;
		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;
		dragChangeView.RefreshData ();


		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.UnitData.PartyInfo.CurrentParty);
	}
	
	public override void HideUI(){
		base.HideUI();
		DataCenter.Instance.UnitData.PartyInfo.ExitParty();
	}

	public override void DestoryUI(){
		base.DestoryUI();
	}

	void OnClickCallback(GameObject caller){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		if (iuiCallback == null)
//			return;

		ModuleEnum se = buttonInfo [caller];
		if (se == ModuleEnum.CatalogModule) {
			Umeng.GA.Event("Catalog");	
		}
//		iuiCallback.CallbackView(se);

		ModuleManager.Instance.ShowModule (buttonInfo [caller]);
	}

	protected override void ToggleAnimation (bool isShow)
	{
		base.ToggleAnimation (isShow);

		if (isShow) {
			//			Debug.Log("Show Module!: [[[---" + config.moduleName + "---]]]pos: " + config.localPosition.x + " " + config.localPosition.y);
			gameObject.SetActive(true);
//			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);
			gameObject.transform.localPosition = new Vector3(0, -510, 0);
			topRoot.transform.localPosition = 1000 * Vector3.up;
			bottomRoot.transform.localPosition = new Vector3(-1000, -50, 0);
			iTween.MoveTo(topRoot, iTween.Hash("y", 220, "time", 0.4f, "islocal", true));
			iTween.MoveTo(bottomRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
			NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.UNITS);
			//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
			//			Debug.Log("Hide Module!: [[[---" + config.moduleName + "---]]]");
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
			//			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true,"oncomplete","AnimationComplete","oncompletetarget",gameObject));
		}
	}

	void PrevPage(GameObject go){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		RefreshParty(true);  
	}
	
	void NextPage(GameObject go){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		RefreshParty(false);
	}

	public void RefreshParty (bool isRight){
		UnitParty tup = null;
		if (isRight) {
			tup = DataCenter.Instance.UnitData.PartyInfo.PrevParty;
		} else {
			tup = DataCenter.Instance.UnitData.PartyInfo.NextParty;
		}
		int curPartyIndex = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId + 1;
		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;
		dragChangeView.RefreshData ();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, tup);   
	}


	public int xInterv {
		get {
			return 450;
		}
	}
	
	public void RefreshView (List<PageUnitItem> view) { 	}

}
