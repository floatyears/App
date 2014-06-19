using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class PartyView : UIComponentUnity{
	public const int PARTY_MEMBER_COUNT = 4;
	public const int UNIT_ITEM_START_POS = 1;
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private GameObject rejectItem;
	private UILabel sortRuleLabel;
	private UIButton prePageBtn;
	private UIButton nextPageBtn;
	private GameObject topRoot;
	private GameObject bottomRoot;

	private MyUnitItem pickedFromParty;
	private MyUnitItem focusedOnParty;
	private MyUnitItem pickedFromUnitList;
	private UISprite pageIndexSpr;

	private Dictionary<int, PageUnitItem> partyItems = new Dictionary<int, PageUnitItem>();
	private List<TUserUnit> myUnitDataList = new List<TUserUnit>();
	private List<PartyUnitItem> partyUnitViewList = new List<PartyUnitItem>();


	private UILabel totalHpLabel;
	private UILabel curCostLabel;
	private UILabel maxCostLabel;
	private UILabel fireAtkLabel;
	private UILabel waterAtkLabel;
	private UILabel windAtkLabel;
	private UILabel lightAtkLabel;
	private UILabel darkAtkLabel;
	private UILabel noneAtkLabel;
	private UILabel leaderSkillNameLabel;
	private UILabel leaderSkillDscpLabel;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitPagePanel();
		InitDragPanel();
		InitPartyInfoPanel();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
		RefreshDragPanel();
		UpdateInfoPanelView(DataCenter.Instance.PartyInfo.CurrentParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);
		RefreshItemCounter();
		ShowUIAnimation();
	
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
	}

	public override void HideUI(){
		base.HideUI();
		if(UIManager.Instance.baseScene.CurrentScene != SceneEnum.UnitDetail)
			DataCenter.Instance.PartyInfo.ExitParty();
		RmvCmdListener();
	}

	public override void DestoryUI(){
		base.DestoryUI();
		partyItems.Clear();
	}

	private void InitPagePanel(){
		topRoot = transform.FindChild("Top").gameObject;
		bottomRoot = transform.FindChild("Bottom").gameObject;
		pageIndexSpr = transform.FindChild("Top/Sprite_Page_Index").GetComponent<UISprite>();
		prePageBtn = FindChild<UIButton>("Top/Button_Left");
		UIEventListener.Get(prePageBtn.gameObject).onClick = PrevPage;
		nextPageBtn = FindChild<UIButton>("Top/Button_Right");
		UIEventListener.Get(nextPageBtn.gameObject).onClick = NextPage;
		ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Friend/RejectItem", o=>{
			rejectItem = o as GameObject;
			for (int i = 0; i < 4; i++){
				GameObject item = topRoot.transform.FindChild(i.ToString()).gameObject;
				PageUnitItem puv = item.GetComponent<PageUnitItem>();
				partyItems.Add(i, puv);
				puv.callback = PartyItemClick;
			}
		}) ;


	}

	void PrevPage(GameObject go){
		ClearPartyFocusState();
		ClearUnitListFocusState();
		TUnitParty preParty = DataCenter.Instance.PartyInfo.PrevParty;
		RefreshParty(preParty);
		RefreshUnitListByCurId();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, preParty);
	}
        
	void NextPage(GameObject go){
		ClearPartyFocusState();
		ClearUnitListFocusState();
		TUnitParty nextParty = DataCenter.Instance.PartyInfo.NextParty;
		RefreshParty(nextParty);
		RefreshUnitListByCurId();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, nextParty);
	}

	void RefreshParty(TUnitParty party){
		List<TUserUnit> partyData = party.GetUserUnit();
		//Debug.LogError("Partyed count is : " + partyData.Count);
		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;

		int count = partyData.Count;
		if(count > partyItems.Count) count = partyItems.Count;

		for (int i = 0; i < count; i++){
			partyItems [ i ].Init(partyData [ i ]);
		}

		for (int i = count; i < partyItems.Count; i++) {
			partyItems[ i ].Init(null);
		}
	}
	
	private void RefreshUnitListByCurId(){
		//Debug.Log("RefreshUnitListByCurId()...curIndex is : " + DataCenter.Instance.PartyInfo.CurrentPartyId);
		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitItem puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitItem>();
			puv.IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(puv.UserUnit.ID);
			//Debug.Log("puv.IsParty : " + puv.IsParty);
		}
	}
        
	void PartyItemClick(MyUnitItem puv) {
		pickedFromParty = puv;
		OnPartyItemClick();
	}

	/// <summary>
	/// Click the item which have been partyed
	/// </summary>
	void OnPartyItemClick() {
		if (focusedOnParty == null) {
			if(pickedFromUnitList != null){
				int afterpos = GetUnitPosInParty(pickedFromParty);

				if(!DataCenter.Instance.PartyInfo.ChangeParty(afterpos, pickedFromUnitList.UserUnit.ID)) {
					return;
				}

				OutNoParty(pickedFromParty.UserUnit);
				pickedFromParty.UserUnit = pickedFromUnitList.UserUnit;
				pickedFromUnitList.IsFocus = false;
				pickedFromUnitList.IsParty = true;
				pickedFromUnitList = null;
			}
			else{
				focusedOnParty = pickedFromParty;
				focusedOnParty.IsFocus = true;
			}
		} 
		else {
			//Debug.LogError("partyFocusUnit != null");
			if (FocusIsLeader() && (pickedFromParty.UserUnit == null)) {
				Debug.Log("Check Focus is Leader... clear focus and return...");
				ClearPartyFocusState();
				return;
			}

			if(CurrentPickedIsLeader() && focusedOnParty.UserUnit == null){
				Debug.Log("Check Picked is Leader... clear focus and return...");
				ClearPartyFocusState();
				return;
			}
		
			int afterPos = 0;
			int beforePos = 0;
			uint beforeId = 0;
			uint afterId = 0;

			foreach (var item in partyItems) {
				if(item.Value.UserUnit == null) continue;
				if(item.Value.UserUnit.Equals(pickedFromParty.UserUnit)){
					afterPos = item.Key;
					afterId = item.Value.UserUnit.ID;
					//Debug.Log("Find afterClickItem...");
				}
				if(item.Value.UserUnit.Equals(focusedOnParty.UserUnit)){
					beforePos = item.Key;
					beforeId = item.Value.UserUnit.ID;
					//Debug.Log("Find beforeClickItem...");
				}
			}

			DataCenter.Instance.PartyInfo.ChangeParty(afterPos, beforeId);
			DataCenter.Instance.PartyInfo.ChangeParty(beforePos, afterId);

			TUserUnit tuu = pickedFromParty.UserUnit; 
			pickedFromParty.UserUnit = focusedOnParty.UserUnit;
			focusedOnParty.UserUnit = tuu;
			ClearPartyFocusState();
		}		
	}


	void OutNoParty(TUserUnit tuu) {
		for (int i = 0; i < partyUnitViewList.Count; i++) {
			PartyUnitItem puv = partyUnitViewList[i];
			if(puv.UserUnit.Equals(tuu)) {
				puv.IsParty = false;
				return;
			}
		}
	}

	/// <summary>
	/// Click the item in unit list
	/// </summary>
	/// <param name="puv">Puv.</param>
	void OutPartyItemClick(MyUnitItem puv){
		//store picked info
		if(pickedFromUnitList != null){
			pickedFromUnitList.IsFocus = false;
			if(pickedFromUnitList.Equals(puv)){//click bottom unit item again, change focus state only
				pickedFromUnitList = null;
			}
			else{
				pickedFromUnitList = puv;
				pickedFromUnitList.IsFocus = true;
			}
		}
		else{
			pickedFromUnitList = puv;

			if(focusedOnParty == null){
//				Debug.Log("focus on-party-unit is null");
				if(! AddUnitToPartyByOrder(1, puv)){
					pickedFromUnitList = puv;
					pickedFromUnitList.IsFocus = true;
				}
			}
			else{
//				Debug.Log("focus on-party-unit is  not null");
				if(focusedOnParty.UserUnit != null){
					ReplaceFocusWithPickedUnit();
				}
				else{
					AddToFocusWithPickedUnit();
				}
			}
		}
	}

	private void AddToFocusWithPickedUnit(){
		int focusPos = GetUnitPosInParty(focusedOnParty);
//		Debug.Log("AddToFocusWithPickedUnit(), focus pos is : " + focusPos);
		if(! DataCenter.Instance.PartyInfo.ChangeParty(focusPos, pickedFromUnitList.UserUnit.ID))  {
			ClearUnitListFocusState();
			ClearPartyFocusState();
			return;
		}

		pickedFromUnitList.IsParty = true;
		partyItems[ focusPos ].UserUnit = pickedFromUnitList.UserUnit;
		ClearPartyFocusState();
		ClearUnitListFocusState();
	}

	private void ReplaceFocusWithPickedUnit(){
//		Debug.Log("Replace Focus With PickedUnit...");
		int focusPos = GetUnitPosInParty(focusedOnParty);

		if(!DataCenter.Instance.PartyInfo.ChangeParty(focusPos, pickedFromUnitList.UserUnit.ID)){
			ClearUnitListFocusState();
			ClearPartyFocusState();
			return ;
		}

		OutNoParty(pickedFromParty.UserUnit);
		pickedFromParty.UserUnit = pickedFromUnitList.UserUnit;
		pickedFromUnitList.IsFocus = false;
		pickedFromUnitList.IsParty = true;
		pickedFromUnitList = null;
		ClearPartyFocusState();
	}

	private bool AddUnitToPartyByOrder(int pos, MyUnitItem target){
		if(pos > 3){
			Debug.LogError("Party is full, can not add new member...return!!!");
			return false;
		}

		if(partyItems[ pos ].UserUnit != null){
			pos++;
			return AddUnitToPartyByOrder(pos, target);
		}
		else{
			//Access to add
			if(!DataCenter.Instance.PartyInfo.ChangeParty(pos, target.UserUnit.ID)){
				ClearUnitListFocusState();
				ClearPartyFocusState();
				return false;
			}

			partyItems[ pos ].UserUnit = target.UserUnit;
			target.IsParty = true;
			target.IsFocus = false;
			ClearUnitListFocusState();
			ClearPartyFocusState();
			return true;
		}
	}

	int GetUnitPosInParty(MyUnitItem targetView){
		int pos = -1;
		foreach (var item in partyItems){
			MyUnitItem muv = item.Value;
			if(muv.Equals(targetView)){
				pos = item.Key;
				break;
			}
		}
		return pos;
	}

	bool FocusIsLeader()	{
		bool isLeader = false;
		if(focusedOnParty.Equals(partyItems[ 0 ])){
			isLeader = true;
		}
		else{
			isLeader = false;
		}
		return isLeader;
	}

	bool CurrentPickedIsLeader(){
		bool isLeader = false;
		if(pickedFromParty.Equals(partyItems[ 0 ])){
			isLeader = true;
		}
		return isLeader;
	}

	bool ClearPartyFocusState() {
		if(focusedOnParty != null){
			focusedOnParty.IsFocus = false;
			focusedOnParty = null;
		}
		return true;
	}

	bool ClearUnitListFocusState(){
		if(pickedFromUnitList != null){
			pickedFromUnitList.IsFocus = false;
			pickedFromUnitList = null;
		}
		return true;
	}

	private List<TUserUnit> GetUnitList(){
		List<TUserUnit> myUnit = DataCenter.Instance.UserUnitList.GetAllMyUnit ();
		if(myUnit == null){
			Debug.LogError("!!!Data Read Error!!! DataCenter.Instance.MyUnitList.GetAll() is return null!");
			return null;
		}

//		List<TUserUnit> partyMembers = new List<TUserUnit>();
//		partyMembers.AddRange(myUnit);
		//Debug.Log("partyMember's count is : " + partyMembers.Count);
		return myUnit;
	}

	private void InitDragPanel(){
		dragPanel = new DragPanel("PartyDragPanel", PartyUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.PartyListDragPanelArgs, bottomRoot.transform);
		InitRejectBtn();
		InitUnitListView();
	}

	private void InitRejectBtn(){
		dragPanel.AddItem(1, rejectItem);
		GameObject rejectItemIns = dragPanel.ScrollItem[ 0 ];
		UIEventListener.Get(rejectItemIns).onClick = RejectPartyMember;
	}

	private void InitUnitListView(){
		myUnitDataList = GetUnitList();
		dragPanel.AddItem(myUnitDataList.Count, MyUnitItem.ItemPrefab);

		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitItem puv = PartyUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			puv.Init(myUnitDataList[ i - 1 ]);
			puv.callback = OutPartyItemClick;
			partyUnitViewList.Add(puv);
		}

		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		//sortRuleLabel.text = curSortRule.ToString();
	}

	private void RejectPartyMember(GameObject item){
		Debug.Log("Click Reject item...");
		if(focusedOnParty == null){
			Debug.Log("RejectPartyMember(), partyFocusUnit == null, reject from the last one by one...");
			RejectByOrder(partyItems.Count - 1);
		}
		else{
			Debug.Log("RejectPartyMember(), partyFocusUnit != null, reject the focus...");
			int focusPos = GetPartyMemberPosition(focusedOnParty.UserUnit);
			Debug.LogError("RejectPartyMember focusPos : " + focusPos);
			if(focusPos == -1 || focusPos == 0)
				ClearPartyFocusState();
			else{
				RejectFocus(focusPos);
			}
		}
	}

	private int GetPartyMemberPosition(TUserUnit tuu){
		int pos = -1;
		if(tuu == null) return pos;
		foreach (var item in partyItems){
			if(item.Value.UserUnit == null) continue;
			if(item.Value.UserUnit.Equals(tuu)){
				pos = item.Key;
				return pos;
			}
		}
		return pos;
	}
	
	void RejectByOrder(int pos){
		Debug.Log("RejectByOrder : " + pos);
		if(pos == 0) return;//Leader can not be rejected
		if(partyItems[ pos ].UserUnit == null){
			pos--;
			RejectByOrder(pos);
		}else {
			Reject(pos);
		}
	}

	void RejectFocus(int pos){
		Debug.Log("RejectFocus : " + pos);
		if(pos == 0) return;//Leader can not be rejected
		Reject(pos);
	}

	void Reject(int pos){
		for (int i = 1; i < dragPanel.ScrollItem.Count; i++) {
			PartyUnitItem partyUnitView = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitItem>();
			if(partyUnitView.UserUnit.Equals(partyItems[ pos ].UserUnit)){
				partyUnitView.IsParty = false;
				partyUnitView.IsEnable = true;
				partyItems[ pos ].UserUnit = null;
				break;
			}
		}
		//When reject every time, record party state change
		Debug.LogError("Reject pos : " + pos);
		DataCenter.Instance.PartyInfo.ChangeParty(pos, 0); 
		ClearPartyFocusState();
	}
	
	void ClickSortBtn(GameObject btn){
		//curSortRule = SortUnitTool.GetNextRule(curSortRule);
		//SortUnitByCurRule();
		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	void RefreshDragPanel(){
		myUnitDataList = GetUnitList();
		int memCount = myUnitDataList.Count;
		int dragCount = dragPanel.ScrollItem.Count - 1;
		if( memCount >  dragCount){
			int addItemCount = myUnitDataList.Count - dragCount;//the first one is reject item
			dragPanel.AddItem(addItemCount, MyUnitItem.ItemPrefab);
			dragCount = dragPanel.ScrollItem.Count;
			for (int i = 1; i < dragCount; i++) {
				//RefreshData
				PartyUnitItem puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitItem>();
				if(puv == null){
					puv.Init(myUnitDataList[ i - 1 ]);
				}
				else
					puv.UserUnit = myUnitDataList[ i - 1 ];//before
				puv.CurrentSortRule = curSortRule;//after	
			}
		}
		else{
			for (int i = 0; i < memCount; i++) {
				PartyUnitItem puv = dragPanel.ScrollItem[ i + 1 ].GetComponent<PartyUnitItem>();
				puv.UserUnit = myUnitDataList[ i ];//before
				puv.CurrentSortRule = curSortRule;//after
			}
			//Remove
			for (int i = memCount + 1; i < dragPanel.ScrollItem.Count; i++) {
				//Debug.LogError("i : " + i + " dragPanel.ScrollItem[ i ] : " + dragPanel.ScrollItem[ i ]);
				dragPanel.RemoveItem(dragPanel.ScrollItem[ i ]);
			}
			dragPanel.Refresh();
		}
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){
		//sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, myUnitDataList);
		for (int i = UNIT_ITEM_START_POS; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitItem puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitItem>();
			puv.UserUnit = myUnitDataList[ i - 1 ];
			puv.CurrentSortRule = curSortRule;
		}
	}

	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(0, -476, 0);

		topRoot.transform.localPosition = 1000 * Vector3.up;
		bottomRoot.transform.localPosition = new Vector3(-1000, -145, 0);

		iTween.MoveTo(topRoot, iTween.Hash("y", 150, "time", 0.4f,"islocal", true));
		iTween.MoveTo(bottomRoot, iTween.Hash("x", 0, "time", 0.4f,"islocal", true));
	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyPanelInfo, UpdateInfoPanelView);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPartyPanelInfo, UpdateInfoPanelView);
	}

	public GameObject GetUnitItem(int i){
		return dragPanel.ScrollItem [i];
	}

	private void InitPartyInfoPanel(){
		totalHpLabel = topRoot.transform.FindChild("Label_Total_HP").GetComponent<UILabel>();
		curCostLabel = topRoot.transform.FindChild("Label_Cost_Cur").GetComponent<UILabel>();
		maxCostLabel = topRoot.transform.FindChild("Label_Cost_Max").GetComponent<UILabel>();
		
		fireAtkLabel = topRoot.transform.FindChild("Label_Atk_Fire").GetComponent<UILabel>();
		waterAtkLabel = topRoot.transform.FindChild("Label_Atk_Water").GetComponent<UILabel>();
		windAtkLabel = topRoot.transform.FindChild("Label_Atk_Wind").GetComponent<UILabel>();
		lightAtkLabel = topRoot.transform.FindChild("Label_Atk_Light").GetComponent<UILabel>();
		darkAtkLabel = topRoot.transform.FindChild("Label_Atk_Dark").GetComponent<UILabel>();
		noneAtkLabel = topRoot.transform.FindChild("Label_Atk_None").GetComponent<UILabel>();
		
		leaderSkillNameLabel = topRoot.transform.FindChild("Label_Leader_Skill_Name").GetComponent<UILabel>();
		leaderSkillDscpLabel = topRoot.transform.FindChild("Label_Leader_Skill_Dscp").GetComponent<UILabel>();
	}

	private void UpdateInfoPanelView(object data){
		TUnitParty unitParty = data as TUnitParty;
		if(unitParty == null){
			Debug.LogError("PartyInfoView.UpdateView(), TUnitParty is NULL!");
			return;
		}
		
		SkillBase skillBase = unitParty.GetLeaderSkillInfo ();
		if (skillBase == null) {
			leaderSkillNameLabel.text = string.Empty;
			leaderSkillDscpLabel.text = string.Empty;
		}
		else{
			leaderSkillNameLabel.text = skillBase.name;
			leaderSkillDscpLabel.text = skillBase.description;
		}
		
		totalHpLabel.text = unitParty.TotalHp.ToString();	
		curCostLabel.text = unitParty.TotalCost.ToString();
		maxCostLabel.text = DataCenter.Instance.UserInfo.CostMax.ToString();
		
		int value = 0;
		unitParty.TypeAttack.TryGetValue (EUnitType.UFIRE, out value);
		fireAtkLabel.text = value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.UWATER, out value);	
		waterAtkLabel.text =  value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.UWIND, out value);
		windAtkLabel.text =  value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.UNONE, out value);
		noneAtkLabel.text =  value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.ULIGHT, out value);
		lightAtkLabel.text =  value.ToString();
		
		unitParty.TypeAttack.TryGetValue (EUnitType.UDARK, out value);
		darkAtkLabel.text =  value.ToString();
	}

}
