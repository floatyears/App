using UnityEngine;
using System.Collections.Generic;

public class UnitsWindow : UIComponentUnity{
	private UIButton prePageBtn;
	private UIButton nextPageBtn;
	private UISprite pageIndexSpr;
	IUICallback iuiCallback;
	private GameObject topRoot;
	private GameObject bottomRoot;

	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum>();
	private Dictionary<int, PageUnitItem> partyItems = new Dictionary<int, PageUnitItem>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitChildScenes();
		iuiCallback = origin as IUICallback;
		InitPagePanel();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
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

		go = FindChild("Bottom/Catalog");
		buttonInfo.Add(go, SceneEnum.UnitCatalog);
		
		go = FindChild("Bottom/Evolve");
		buttonInfo.Add(go, SceneEnum.Evolve);
		
		go = FindChild("Bottom/LevelUp");
		buttonInfo.Add(go, SceneEnum.LevelUp);
		
		go = FindChild("Bottom/Party");
		buttonInfo.Add(go, SceneEnum.Party);
		
		go = FindChild("Bottom/Sell");
		buttonInfo.Add(go, SceneEnum.Sell);
		
		go = FindChild("Bottom/UnitList");
		buttonInfo.Add(go, SceneEnum.UnitList);
		
		foreach (var item in buttonInfo.Keys)
			UIEventListener.Get(item).onClick = OnClickCallback;
	}

	void OnClickCallback(GameObject caller){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		if (iuiCallback == null)
			return;
		SceneEnum se = buttonInfo [caller];
		iuiCallback.CallbackView(se);
	}
	
	void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(0, -476, 0);
		topRoot.transform.localPosition = 1000 * Vector3.up;
		bottomRoot.transform.localPosition = new Vector3(-1000, -165, 0);
		iTween.MoveTo(topRoot, iTween.Hash("y", 230, "time", 0.4f, "islocal", true));
		iTween.MoveTo(bottomRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));

		//start units step
		NoviceGuideStepEntityManager.Instance ().StartStep ();
	}

	private void InitPagePanel(){
		topRoot = transform.FindChild("Top").gameObject;
		bottomRoot = transform.FindChild("Bottom").gameObject;
		pageIndexSpr = transform.FindChild("Top/Sprite_Page_Index").GetComponent<UISprite>();
		prePageBtn = FindChild<UIButton>("Top/Button_Left");
		UIEventListener.Get(prePageBtn.gameObject).onClick = PrevPage;
		nextPageBtn = FindChild<UIButton>("Top/Button_Right");
		UIEventListener.Get(nextPageBtn.gameObject).onClick = NextPage;

		for (int i = 0; i < PartyView.PARTY_MEMBER_COUNT; i++){
			GameObject item = topRoot.transform.FindChild(i.ToString()).gameObject;
			PageUnitItem puv = item.GetComponent<PageUnitItem>();
			partyItems.Add(i, puv);
		}
	}

	void RefreshParty(TUnitParty party){
		List<TUserUnit> partyMemberList = party.GetUserUnit();
		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;

		//Debug.Log("Current party's member count is : " + partyMemberList.Count);
		for (int i = 0; i < partyMemberList.Count; i++){
			partyItems[ i ].Init(partyMemberList [ i ]);
		}
	}

	void PrevPage(GameObject go){
		TUnitParty preParty = DataCenter.Instance.PartyInfo.PrevParty;
		RefreshParty(preParty);  
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, preParty);         
	}
	
	void NextPage(GameObject go){
		TUnitParty nextParty = DataCenter.Instance.PartyInfo.NextParty;
		RefreshParty(nextParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, nextParty);
	}
}
