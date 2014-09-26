using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class PartyView : ViewBase, IDragChangeView{
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
	private List<UserUnit> myUnitDataList = new List<UserUnit>();
	private List<PartyUnitItem> partyUnitViewList = new List<PartyUnitItem>();

	private DragSliderBase dragChangeView;

	private UILabel totalHpLabel;
	private UILabel curCostLabel;
	private UILabel fireAtkLabel;
	private UILabel waterAtkLabel;
	private UILabel windAtkLabel;
	private UILabel lightAtkLabel;
	private UILabel darkAtkLabel;
	private UILabel noneAtkLabel;
	private UILabel leaderSkillNameLabel;
	private UILabel leaderSkillDscpLabel;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitPagePanel();
		InitDragPanel();
		InitPartyInfoPanel();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		UnitParty curParty = DataCenter.Instance.UnitData.PartyInfo.CurrentParty;

		int curPartyIndex = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId + 1;
		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;
		dragChangeView.RefreshData ();

		RefreshDragPanel();
		UpdateInfoPanelView(DataCenter.Instance.UnitData.PartyInfo.CurrentParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);
		RefreshItemCounter();
		ShowUIAnimation();
	
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
	}

	public override void HideUI(){
		base.HideUI();
//		if(UIManager.Instance.baseScene.CurrentScene != ModuleEnum.UnitDetail)
//			DataCenter.Instance.UnitData.PartyInfo.ExitParty();
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
		UIEventListenerCustom.Get(prePageBtn.gameObject).onClick = PrevPage;
		nextPageBtn = FindChild<UIButton>("Top/Button_Right");
		UIEventListenerCustom.Get(nextPageBtn.gameObject).onClick = NextPage;

		dragChangeView = FindChild<PartyDragView>("Top/DragPartyLevelUp");
		dragChangeView.SetDataInterface (this);

		ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Friend/RejectItem", o=>{
			rejectItem = o as GameObject;
		}) ;
	}

	void PrevPage(GameObject go){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		RefreshParty(true);  
	}
        
	void NextPage(GameObject go){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);

		RefreshParty(false);  
	}

	public int xInterv {
		get {
			return 450;
		}
	}

	public void RefreshView (List<PageUnitItem> view) {
//		foreach (var item in partyItems) {
//
//		}

		for (int i = 0; i < view.Count; i++) {
			partyItems[i] = view[i];
			partyItems[i].callback = PartyItemClick;
		}
	}

	public void RefreshParty (bool isRight){
		ClearPartyFocusState();
		ClearUnitListFocusState();

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

		RefreshUnitListByCurId();
	}

//	void RefreshParty(TUnitParty party){
//		List<TUserUnit> partyData = party.GetUserUnit();
//		//Debug.LogError("Partyed count is : " + partyData.Count);
//		int curPartyIndex = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId + 1;
//		pageIndexSpr.spriteName = UIConfig.SPR_NAME_PAGE_INDEX_PREFIX  + curPartyIndex;
//
//		int count = partyData.Count;
//		if(count > partyItems.Count) count = partyItems.Count;
//
//		for (int i = 0; i < count; i++){
//			partyItems [ i ].Init(partyData [ i ]);
//		}
//
//		for (int i = count; i < partyItems.Count; i++) {
//			partyItems[ i ].Init(null);
//		}
//	}
	
	private void RefreshUnitListByCurId(){
		//Debug.Log("RefreshUnitListByCurId()...curIndex is : " + DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId);
		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitItem puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitItem>();
			puv.IsParty = DataCenter.Instance.UnitData.PartyInfo.UnitIsInCurrentParty(puv.UserUnit.uniqueId);
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
//		Debug.LogError ("focusedOnParty : " + (focusedOnParty == null));
		if (focusedOnParty == null) {
			if(pickedFromUnitList != null){
				int afterpos = GetUnitPosInParty(pickedFromParty);

				if(!DataCenter.Instance.UnitData.PartyInfo.ChangeParty(afterpos, pickedFromUnitList.UserUnit.uniqueId)) {
					return;
				}

				OutNoParty(pickedFromParty.UserUnit);
				pickedFromParty.UserUnit = pickedFromUnitList.UserUnit;
				pickedFromUnitList.IsFocus = false;
				pickedFromUnitList.IsParty = true;
				pickedFromUnitList = null;
			} else {
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
					afterId = item.Value.UserUnit.uniqueId;
					//Debug.Log("Find afterClickItem...");
				}
				if(item.Value.UserUnit.Equals(focusedOnParty.UserUnit)){
					beforePos = item.Key;
					beforeId = item.Value.UserUnit.uniqueId;
					//Debug.Log("Find beforeClickItem...");
				}
			}

			DataCenter.Instance.UnitData.PartyInfo.ChangeParty(afterPos, beforeId);
			DataCenter.Instance.UnitData.PartyInfo.ChangeParty(beforePos, afterId);

			UserUnit tuu = pickedFromParty.UserUnit; 
			pickedFromParty.UserUnit = focusedOnParty.UserUnit;
//			Debug.LogError("focusedOnParty : " + focusedOnParty.UserUnit.ID + " tuu : " + tuu.ID);
			focusedOnParty.UserUnit = tuu;
			ClearPartyFocusState();
		}	

		RefreshUnitListByCurId ();
	}


	void OutNoParty(UserUnit tuu) {
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
				if(! AddUnitToPartyByOrder(1, puv)){
					pickedFromUnitList = puv;
					pickedFromUnitList.IsFocus = true;
				}
			}
			else{
				if(focusedOnParty.UserUnit != null){
					ReplaceFocusWithPickedUnit();
				}
				else{
					AddToFocusWithPickedUnit();
				}
			}
		}

		RefreshUnitListByCurId ();
	}

	private void AddToFocusWithPickedUnit(){
		int focusPos = GetUnitPosInParty(focusedOnParty);
		if(! DataCenter.Instance.UnitData.PartyInfo.ChangeParty(focusPos, pickedFromUnitList.UserUnit.uniqueId))  {
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
		int focusPos = GetUnitPosInParty(focusedOnParty);

		if(!DataCenter.Instance.UnitData.PartyInfo.ChangeParty(focusPos, pickedFromUnitList.UserUnit.uniqueId)){
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
//			Debug.LogError("Party is full, can not add new member...return!!!");
			return false;
		}

		if(partyItems[ pos ].UserUnit != null){
			pos++;
			return AddUnitToPartyByOrder(pos, target);
		}
		else{
			//Access to add
			if(!DataCenter.Instance.UnitData.PartyInfo.ChangeParty(pos, target.UserUnit.uniqueId)){
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

	private List<UserUnit> GetUnitList(){
		List<UserUnit> myUnit = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ();
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
		dragPanel = new DragPanel("PartyDragPanel", PartyUnitItem.ItemPrefab,bottomRoot.transform);
//		dragPanel.SetDragPanel();
		InitRejectBtn();
		InitUnitListView();
	}

	private void InitRejectBtn(){
		dragPanel.AddItem(1, rejectItem);

		GameObject rejectItemIns = dragPanel.ScrollItem[ 0 ];
		rejectItemIns.transform.FindChild ("Label_Text").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Reject");
		UIEventListenerCustom.Get(rejectItemIns).onClick = RejectPartyMember;
	}

	private void InitUnitListView(){
		myUnitDataList = GetUnitList();
		dragPanel.AddItem(myUnitDataList.Count, MyUnitItem.ItemPrefab);

		curSortRule = SortUnitTool.GetSortRule (SortRuleByUI.PartyView);//DEFAULT_SORT_RULE;
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

	private int GetPartyMemberPosition(UserUnit tuu){
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
//		Debug.Log("RejectByOrder : " + pos);
		if(pos == 0) return;//Leader can not be rejected
//		Debug.LogError ("RejectByOrder pos : " + pos);
		if(partyItems[ pos ].UserUnit == null){
			pos--;
			RejectByOrder(pos);
		}else {
			Reject(pos);
		}
	}

	void RejectFocus(int pos){
//		Debug.Log("RejectFocus : " + pos);
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
//		Debug.LogError("Reject pos : " + pos);
		DataCenter.Instance.UnitData.PartyInfo.ChangeParty(pos, 0); 
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

		for (int i = UNIT_ITEM_START_POS; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitItem puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitItem>();
			puv.UserUnit = myUnitDataList[ i - 1 ];
			puv.CurrentSortRule = curSortRule;
		}
	}

	void RefreshDragPanel(){
		myUnitDataList = GetUnitList();
		SortUnitByCurRule();
//		Debug.LogError("")
		int memCount = myUnitDataList.Count;
		int dragCount = dragPanel.ScrollItem.Count - 1;
		if( memCount >  dragCount){
			int addItemCount = myUnitDataList.Count - dragCount;//the first one is reject item
			dragPanel.AddItem(addItemCount, MyUnitItem.ItemPrefab);
			dragCount = dragPanel.ScrollItem.Count;
			for (int i = 1; i < dragCount; i++) {
				//RefreshData
				PartyUnitItem puv =  PartyUnitItem.Inject(dragPanel.ScrollItem[ i ]);
				puv.callback = OutPartyItemClick;

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
				PartyUnitItem puv =PartyUnitItem.Inject(dragPanel.ScrollItem[ i + 1 ]);
				puv.callback = OutPartyItemClick;
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
	}

	private void SortUnitByCurRule(){
		//sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, myUnitDataList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.PartyView);

	}

	private void ShowUIAnimation() {
		gameObject.transform.localPosition = new Vector3(0, -476, 0);

		topRoot.transform.localPosition = 1000 * Vector3.up;
		bottomRoot.transform.localPosition = new Vector3(-1000, -145, 0);

		iTween.MoveTo(topRoot, iTween.Hash("y", 150, "time", 0.4f,"islocal", true));
		iTween.MoveTo (bottomRoot, iTween.Hash ("x", 0, "time", 0.4f, "islocal", true, "oncomplete", "BottomRootMoveEnd", "oncompletetarget", gameObject));
	}

	void BottomRootMoveEnd() {
		dragPanel.GetDragViewObject().GetComponent<DragPanelView>().scrollBar.gameObject.SetActive (false);
		dragPanel.GetDragViewObject().GetComponent<DragPanelView>().scrollBar.gameObject.SetActive (true);
	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserData.UserInfo.unitMax);
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

	public GameObject GetUnitItem(uint id){
//		return dragPanel.ScrollItem [i];

		for (int i = UNIT_ITEM_START_POS; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitItem puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitItem>();
			if(puv.UserUnit.unitId == id)
				return dragPanel.ScrollItem[ i ];
		}
		return null;
	}

	private void InitPartyInfoPanel(){
		totalHpLabel = topRoot.transform.FindChild("Label_Total_HP").GetComponent<UILabel>();
		curCostLabel = topRoot.transform.FindChild("Label_Cost").GetComponent<UILabel>();

		fireAtkLabel = topRoot.transform.FindChild("Label_Atk_Fire").GetComponent<UILabel>();
		waterAtkLabel = topRoot.transform.FindChild("Label_Atk_Water").GetComponent<UILabel>();
		windAtkLabel = topRoot.transform.FindChild("Label_Atk_Wind").GetComponent<UILabel>();
		lightAtkLabel = topRoot.transform.FindChild("Label_Atk_Light").GetComponent<UILabel>();
		darkAtkLabel = topRoot.transform.FindChild("Label_Atk_Dark").GetComponent<UILabel>();
		noneAtkLabel = topRoot.transform.FindChild("Label_Atk_None").GetComponent<UILabel>();
		
		leaderSkillNameLabel = topRoot.transform.FindChild("Label_Leader_Skill_Name").GetComponent<UILabel>();
		leaderSkillDscpLabel = topRoot.transform.FindChild("Label_Leader_Skill_Dscp").GetComponent<UILabel>();

		topRoot.transform.FindChild ("Bgs/ATK").GetComponent<UILabel>().text = TextCenter.GetText("Text_HP");
		topRoot.transform.FindChild ("Bgs/COST").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_COST");;
	}

	private void UpdateInfoPanelView(object data){
		UnitParty unitParty = data as UnitParty;
		if(unitParty == null){
			Debug.LogError("PartyInfoView.UpdateView(), TUnitParty is NULL!");
			return;
		}
		
		SkillBase skillBase = unitParty.GetLeaderSkillInfo ();
		if (skillBase == null) {
			leaderSkillNameLabel.text = TextCenter.GetText("Name_No_LeaderSkill");
			leaderSkillDscpLabel.text = TextCenter.GetText("Description_No_LeaderSkill");
		}
		else{
			leaderSkillNameLabel.text = TextCenter.GetText("Text_Leader_Skill_Colon") + TextCenter.GetText("SkillName_" +  skillBase.id);
			leaderSkillDscpLabel.text =  TextCenter.GetText("Text_Leader_Skill_Colon") + TextCenter.GetText("SkillDesc_" +  skillBase.id);
		}
		
		totalHpLabel.text = unitParty.TotalHp.ToString();	
		string curCostStr = unitParty.TotalCost.ToString();
		if (DataCenter.Instance.UserData.UserInfo.costMax < unitParty.TotalCost) {
			curCostStr = "[FF0000]" + curCostStr + "[-]";
		}
		curCostLabel.text = curCostStr + "/" +DataCenter.Instance.UserData.UserInfo.costMax.ToString();
		
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
