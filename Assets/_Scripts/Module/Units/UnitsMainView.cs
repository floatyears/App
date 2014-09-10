using UnityEngine;
using System.Collections.Generic;

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
		InitChildScenes();
//		iuiCallback = origin as IUICallback;
		InitPagePanel();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
//		RefreshParty();

		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;
		dragChangeView.RefreshData ();


		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);
		ShowUIAnimation();
	}
	
	public override void HideUI(){
		base.HideUI();
		DataCenter.Instance.PartyInfo.ExitParty();
	}

	public override void DestoryUI(){
		base.DestoryUI();
	}

	void InitChildScenes(){
//		Debug.LogError ("InitChildScenes");

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

		dragChangeView = FindChild<DragSliderBase> ("Top/DragParty");
		dragChangeView.SetDataInterface (this);
//		Debug.LogError ("InitChildScenes dragChangeView.Init ");
//		dragChangeView.Init ();

		foreach (var item in buttonInfo.Keys)
			UIEventListener.Get(item).onClick = OnClickCallback;
	}

	void OnClickCallback(GameObject caller){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		if (iuiCallback == null)
//			return;

//		ViewManager.Instance.ShowTipsLabel ("Click", caller);
		ModuleEnum se = buttonInfo [caller];
		if (se == ModuleEnum.CatalogModule) {
			Umeng.GA.Event("Catalog");	
		}
//		iuiCallback.CallbackView(se);

		ModuleManager.Instance.ShowModule (buttonInfo [caller]);
	}
	
	void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(0, -510, 0);
		topRoot.transform.localPosition = 1000 * Vector3.up;
		bottomRoot.transform.localPosition = new Vector3(-1000, -50, 0);
		iTween.MoveTo(topRoot, iTween.Hash("y", 220, "time", 0.4f, "islocal", true));
		iTween.MoveTo(bottomRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));

		//start units step
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
	}

	private void InitPagePanel(){
		topRoot = transform.FindChild("Top").gameObject;
		bottomRoot = transform.FindChild("Bottom").gameObject;
		pageIndexSpr = transform.FindChild("Top/Sprite_Page_Index").GetComponent<UISprite>();
		prePageBtn = FindChild<UIButton>("Top/Button_Left");
		UIEventListener.Get(prePageBtn.gameObject).onClick = PrevPage;
		nextPageBtn = FindChild<UIButton>("Top/Button_Right");
		UIEventListener.Get(nextPageBtn.gameObject).onClick = NextPage;

//		for (int i = 0; i < PartyView.PARTY_MEMBER_COUNT; i++){
//			GameObject item = topRoot.transform.FindChild(i.ToString()).gameObject;
//			PageUnitItem puv = item.GetComponent<PageUnitItem>();
//			partyItems.Add(i, puv);
//		}
	}

//	void RefreshParty(TUnitParty party){
//		List<TUserUnit> partyMemberList = party.GetUserUnit();
//		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
//		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;
//
//		for (int i = 0; i < partyMemberList.Count; i++){
//			partyItems[i].UserUnit = partyMemberList[i];
//		}
//	}
	
	void PrevPage(GameObject go){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		RefreshParty(true);  
	}
	
	void NextPage(GameObject go){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		RefreshParty(false);
	}

	public void RefreshParty (bool isRight){
		TUnitParty tup = null;
		if (isRight) {
			tup = DataCenter.Instance.PartyInfo.PrevParty;
		} else {
			tup = DataCenter.Instance.PartyInfo.NextParty;
		}
		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
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
