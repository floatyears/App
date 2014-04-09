using UnityEngine;
using System.Collections.Generic;

public class UnitsWindow : UIComponentUnity{
	private UILabel pageIndexLabel;
	private UIButton prePageButton;
	private UIButton nextPageButton;
	private UILabel pageIndexSuffixLabel;
	IUICallback iuiCallback;
	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum>();
	private Dictionary<int, PageUnitView> partyView = new Dictionary<int, PageUnitView>();
	public static Dictionary< int, string > partyIndexDic = new Dictionary< int, string >();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitChildScenes();
		iuiCallback = origin as IUICallback;
		InitIndexTextDic();
		InitPagePanel();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		ShowTween();

		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);

	}
	
	public override void HideUI(){
		base.HideUI();
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
	
	void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)	return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)	continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	private void InitPagePanel(){
		pageIndexLabel = FindChild<UILabel>("Label_Left/Label_Before");
		pageIndexSuffixLabel = FindChild<UILabel>("Label_Left/Label_After");
		prePageButton = FindChild<UIButton>("Button_Left");
		UIEventListener.Get(prePageButton.gameObject).onClick = PrevPage;
		nextPageButton = FindChild<UIButton>("Button_Right");
		UIEventListener.Get(nextPageButton.gameObject).onClick = NextPage;

		for (int i = 0; i < 4; i++){
			PageUnitView puv = FindChild<PageUnitView>(i.ToString());
			partyView.Add(i, puv);
		}

	}

	void RefreshParty(TUnitParty party){
		List<TUserUnit> partyData = party.GetUserUnit();

		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexLabel.text = curPartyIndex.ToString();
		pageIndexSuffixLabel.text = partyIndexDic[ curPartyIndex ].ToString();

		for (int i = 0; i < partyData.Count; i++){
			partyView [ i ].Init(partyData [ i ]);
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

	public static void InitIndexTextDic() {
		partyIndexDic.Add( 1, "st");
		partyIndexDic.Add( 2, "nd");
		partyIndexDic.Add( 3, "rd");
		partyIndexDic.Add( 4, "th");
		partyIndexDic.Add( 5, "th");
	}


}
