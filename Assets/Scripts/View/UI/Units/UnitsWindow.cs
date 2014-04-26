using UnityEngine;
using System.Collections.Generic;

public class UnitsWindow : UIComponentUnity{
	private UILabel pageIndexLabel;
	private UIButton prePageBtn;
	private UIButton nextPageBtn;
	private UILabel pageIndexSuffixLabel;
	private UILabel rightIndexLabel;
	IUICallback iuiCallback;
	private GameObject topRoot;
	private GameObject bottomRoot;

	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum>();
	private Dictionary<int, PageUnitItem> partyItems = new Dictionary<int, PageUnitItem>();
	public static Dictionary< int, string > partyIndexDic = new Dictionary< int, string >();

	public override void Init(UIInsConfig config, IUICallback origin){
//		Debug.LogError ("UnitsController Init begin");
		base.Init(config, origin);
		InitChildScenes();
		iuiCallback = origin as IUICallback;
		InitIndexTextDic();
		InitPagePanel();
//		Debug.LogError ("UnitsController Init end");
	}
	
	public override void ShowUI(){
//		Debug.LogError ("UnitsController ShowUI");
		base.ShowUI();
		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);
		ShowUIAnimation();
	}
	
	public override void HideUI(){
//		Debug.LogError ("UnitsController base");
		base.HideUI();
		DataCenter.Instance.PartyInfo.ExitParty();
	}

	public override void DestoryUI(){
//		Debug.LogError ("UnitsController base");
		partyIndexDic.Clear ();
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
		topRoot.transform.localPosition = 1000 * Vector3.up;
		bottomRoot.transform.localPosition = 1000 * Vector3.left;
		iTween.MoveTo(topRoot, iTween.Hash("y", 0, "time", 0.4f));
		iTween.MoveTo(bottomRoot, iTween.Hash("x", 0, "time", 0.4f));
	}

	private void InitPagePanel(){
//		Debug.LogError ("InitPagePanel begin");
		topRoot = transform.FindChild("Top").gameObject;
		bottomRoot = transform.FindChild("Bottom").gameObject;
		pageIndexLabel = FindChild<UILabel>("Top/Label_Left/Label_Before");
		pageIndexSuffixLabel = FindChild<UILabel>("Top/Label_Left/Label_After");
		rightIndexLabel = FindChild<UILabel>("Top/Label_Cur_Party");
		prePageBtn = FindChild<UIButton>("Top/Button_Left");
		UIEventListener.Get(prePageBtn.gameObject).onClick = PrevPage;
		nextPageBtn = FindChild<UIButton>("Top/Button_Right");
		UIEventListener.Get(nextPageBtn.gameObject).onClick = NextPage;

		for (int i = 0; i < 4; i++){
			GameObject item = topRoot.transform.FindChild(i.ToString()).gameObject;
			PageUnitItem puv = item.GetComponent<PageUnitItem>();
			partyItems.Add(i, puv);
		}
//		Debug.LogError ("InitPagePanel end");
	}

	void RefreshParty(TUnitParty party){
		List<TUserUnit> partyMemberList = party.GetUserUnit();
		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexLabel.text = curPartyIndex.ToString();
		rightIndexLabel.text = curPartyIndex.ToString();
		pageIndexSuffixLabel.text = partyIndexDic[ curPartyIndex ].ToString();

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

	public static void InitIndexTextDic() {
//		Debug.LogError ("InitIndexTextDic begin");
		partyIndexDic.Add( 1, "st");
		partyIndexDic.Add( 2, "nd");
		partyIndexDic.Add( 3, "rd");
		partyIndexDic.Add( 4, "th");
		partyIndexDic.Add( 5, "th");
//		Debug.LogError ("InitIndexTextDic begin");
	}
}
