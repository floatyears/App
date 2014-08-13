using UnityEngine;
using System.Collections.Generic;

public class UnitsWindow : UIComponentUnity, IDragChangeView{
	private UIButton prePageBtn;
	private UIButton nextPageBtn;
	private UISprite pageIndexSpr;
	IUICallback iuiCallback;
	private GameObject topRoot;
	private GameObject bottomRoot;

	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum>();

	private DragChangeView dragChangeView;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitChildScenes();
		iuiCallback = origin as IUICallback;
		InitPagePanel();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
//		RefreshParty();

		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;
		dragChangeView.RefreshParty ();


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
		GameObject go;
		UILabel btnLabel;

		go = FindChild("Bottom/Catalog");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Catalog");
		buttonInfo.Add(go, SceneEnum.UnitCatalog);
		
		go = FindChild("Bottom/Evolve");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Evolve");
		buttonInfo.Add(go, SceneEnum.Evolve);
		
		go = FindChild("Bottom/LevelUp");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_LevelUp");
		buttonInfo.Add(go, SceneEnum.LevelUp);

		go = FindChild("Bottom/Party");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Party");
		buttonInfo.Add(go, SceneEnum.Party);
		
		go = FindChild("Bottom/Sell");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_Sell");
		buttonInfo.Add(go, SceneEnum.Sell);
		
		go = FindChild("Bottom/UnitList");
		btnLabel = go.GetComponentInChildren<UILabel>();
		btnLabel.text = TextCenter.GetText("Btn_JumpScene_UnitList");
		buttonInfo.Add(go, SceneEnum.UnitList);

		dragChangeView = FindChild<DragChangeView> ("Top/DragParty");
		dragChangeView.SetDataInterface (this);

		foreach (var item in buttonInfo.Keys)
			UIEventListener.Get(item).onClick = OnClickCallback;
	}

	void OnClickCallback(GameObject caller){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		if (iuiCallback == null)
			return;

//		ViewManager.Instance.ShowTipsLabel ("Click", caller);
		SceneEnum se = buttonInfo [caller];
		if (se == SceneEnum.UnitCatalog) {
			Umeng.GA.Event("Catalog");	
		}
		iuiCallback.CallbackView(se);
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
		dragChangeView.RefreshParty ();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, tup);   
	}


	public int xInterv {
		get {
			return 450;
		}
	}


}
