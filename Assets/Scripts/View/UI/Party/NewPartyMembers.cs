using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPartyMembers : UIComponentUnity{
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private GameObject rejectItem;
	private UILabel pageIndexLabel;
	private UILabel rightIndexLabel;
	private UILabel pageIndexSuffixLabel;
	private UILabel sortRuleLabel;
	private UIButton prePageButton;
	private UIButton nextPageButton;
	private UIButton sortButton;

	private MyUnitView pickedUnitFromPartyMembers;
	private MyUnitView focusUnitInPartyMembers;
	private MyUnitView pickedUnitFromUnitList;

	private Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private Dictionary<int, PageUnitView> partyView = new Dictionary<int, PageUnitView>();
	private List<TUserUnit> memberList = new List<TUserUnit>();
	private List<PartyUnitView> partyUnitViewList = new List<PartyUnitView>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitPagePanel();
		InitDragPanel();
	}

	public override void ShowUI(){
		base.ShowUI();

		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
		RefreshDragPanel();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);
	}

	public override void HideUI(){
		base.HideUI();
		DataCenter.Instance.PartyInfo.ExitParty();
		Debug.Log("PartyScene.HideUI(), Record Party State Change...");
		Debug.Log("PartyScene.HideUI(), current party id is : " + DataCenter.Instance.PartyInfo.CurrentPartyId);
	}

	public override void DestoryUI(){
		base.DestoryUI();
		partyView.Clear();
	}

	private void InitPagePanel(){
		pageIndexLabel = FindChild<UILabel>("Label_Left/Label_Before");
		pageIndexSuffixLabel = FindChild<UILabel>("Label_Left/Label_After");
		rightIndexLabel = FindChild<UILabel>("Label_Cur_Party");
		prePageButton = FindChild<UIButton>("Button_Left");
		UIEventListener.Get(prePageButton.gameObject).onClick = PrevPage;
		nextPageButton = FindChild<UIButton>("Button_Right");
		UIEventListener.Get(nextPageButton.gameObject).onClick = NextPage;
		rejectItem = Resources.Load("Prefabs/UI/Friend/RejectItem") as GameObject;

		sortButton = FindChild<UIButton>("PartyDragPanel/SortButton");
		UIEventListener.Get(sortButton.gameObject).onClick = ClickSortButton;
		sortRuleLabel = sortButton.transform.FindChild("Label").GetComponent<UILabel>();

		for (int i = 0; i < 4; i++){
			PageUnitView puv = FindChild<PageUnitView>(i.ToString());
			partyView.Add(i, puv);
			puv.callback = PartyItemClick;
		}
	}

	void PrevPage(GameObject go){
		TUnitParty preParty = DataCenter.Instance.PartyInfo.PrevParty;
		RefreshParty(preParty);  
		RefreshUnitListByCurId();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, preParty);
	}
        
	void NextPage(GameObject go){
		TUnitParty nextParty = DataCenter.Instance.PartyInfo.NextParty;
		RefreshParty(nextParty);
		RefreshUnitListByCurId();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, nextParty);
	}

	void RefreshParty(TUnitParty party){
		List<TUserUnit> partyData = party.GetUserUnit();
//		Debug.LogError("Partyed count is : " + partyData.Count);
		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexLabel.text = curPartyIndex.ToString();
		rightIndexLabel.text = curPartyIndex.ToString();
		pageIndexSuffixLabel.text = UnitsWindow.partyIndexDic[ curPartyIndex ].ToString();

		int cout = partyData.Count;
		if(cout > partyView.Count) cout = partyView.Count;

		for (int i = 0; i < cout; i++){
			partyView [ i ].Init(partyData [ i ]);
		}

		for (int i = cout; i < partyView.Count; i++) {
			partyView[i].Init(null);
		}
	}
	
	private void RefreshUnitListByCurId(){
		//Debug.Log("RefreshUnitListByCurId()...curIndex is : " + DataCenter.Instance.PartyInfo.CurrentPartyId);
		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitView puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
			puv.IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(puv.UserUnit.ID);
			//Debug.Log("puv.IsParty : " + puv.IsParty);
		}
	}
        
	void PartyItemClick(MyUnitView puv) {
		pickedUnitFromPartyMembers = puv;
		OnPartyItemClick();
	}

	/// <summary>
	/// Click the item which have been partyed
	/// </summary>
	void OnPartyItemClick() {
		if (focusUnitInPartyMembers == null) {
			if(pickedUnitFromUnitList != null){
				int afterpos = GetUnitPosInParty(pickedUnitFromPartyMembers);

				if(!DataCenter.Instance.PartyInfo.ChangeParty(afterpos, pickedUnitFromUnitList.UserUnit.ID)) {
					return;
				}

				OutNoParty(pickedUnitFromPartyMembers.UserUnit);
				pickedUnitFromPartyMembers.UserUnit = pickedUnitFromUnitList.UserUnit;
				pickedUnitFromUnitList.IsFocus = false;
				pickedUnitFromUnitList.IsParty = true;
				pickedUnitFromUnitList = null;
			}
			else{
				focusUnitInPartyMembers = pickedUnitFromPartyMembers;
				focusUnitInPartyMembers.IsFocus = true;
			}
		} 
		else {
			//Debug.LogError("partyFocusUnit != null");
			if (FocusIsLeader() && (pickedUnitFromPartyMembers.UserUnit == null)) {
				Debug.Log("Check Focus is Leader... clear focus and return...");
				ClearPartyFocusState();
				return;
			}

			if(CurrentPickedIsLeader() && focusUnitInPartyMembers.UserUnit == null){
				Debug.Log("Check Picked is Leader... clear focus and return...");
				ClearPartyFocusState();
				return;
			}
		
			int afterPos = 0;
			int beforePos = 0;
			uint beforeId = 0;
			uint afterId = 0;

			foreach (var item in partyView) {
				if(item.Value.UserUnit == null) continue;
				if(item.Value.UserUnit.Equals(pickedUnitFromPartyMembers.UserUnit)){
					afterPos = item.Key;
					afterId = item.Value.UserUnit.ID;
					//Debug.Log("Find afterClickItem...");
				}
				if(item.Value.UserUnit.Equals(focusUnitInPartyMembers.UserUnit)){
					beforePos = item.Key;
					beforeId = item.Value.UserUnit.ID;
					//Debug.Log("Find beforeClickItem...");
				}
			}

			DataCenter.Instance.PartyInfo.ChangeParty(afterPos, beforeId);
			DataCenter.Instance.PartyInfo.ChangeParty(beforePos, afterId);


			TUserUnit tuu = pickedUnitFromPartyMembers.UserUnit; 
			pickedUnitFromPartyMembers.UserUnit = focusUnitInPartyMembers.UserUnit;
			focusUnitInPartyMembers.UserUnit = tuu;

//			DataCenter.Instance.PartyInfo.ChangeParty(partyView);
			//MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.PartyInfo.CurrentParty);
			ClearPartyFocusState();
		}		
	}


	void OutNoParty(TUserUnit tuu) {
		for (int i = 0; i < partyUnitViewList.Count; i++) {
			PartyUnitView puv = partyUnitViewList[i];
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
	void OutPartyItemClick(MyUnitView puv){
		//store picked info
		if(pickedUnitFromUnitList != null){
			pickedUnitFromUnitList.IsFocus = false;
			if(pickedUnitFromUnitList.Equals(puv)){//click bottom unit item again, change focus state only
				pickedUnitFromUnitList = null;
			}
			else{
				pickedUnitFromUnitList = puv;
				pickedUnitFromUnitList.IsFocus = true;
			}
		}
		else{
			pickedUnitFromUnitList = puv;

			if(focusUnitInPartyMembers == null){
				Debug.Log("focus on-party-unit is null");
				if(! AddUnitToPartyByOrder(1, puv)){
					pickedUnitFromUnitList = puv;
					pickedUnitFromUnitList.IsFocus = true;
				}
			}
			else{
				Debug.Log("focus on-party-unit is  not null");
				if(focusUnitInPartyMembers.UserUnit != null){
					ReplaceFocusWithPickedUnit();
				}
				else{
					AddToFocusWithPickedUnit();
				}
			}


		}
		//store picked info


	}

	private void AddToFocusWithPickedUnit(){
		int focusPos = GetUnitPosInParty(focusUnitInPartyMembers);
		Debug.Log("AddToFocusWithPickedUnit(), focus pos is : " + focusPos);
		//		DataCenter.Instance.PartyInfo.ChangeParty(partyView);{}
		if(! DataCenter.Instance.PartyInfo.ChangeParty(focusPos, pickedUnitFromUnitList.UserUnit.ID))  {
			ClearUnitListFocus();
			ClearPartyFocusState();
			return;
		}

		//MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.PartyInfo.CurrentParty);
		pickedUnitFromUnitList.IsParty = true;
		partyView[ focusPos ].UserUnit = pickedUnitFromUnitList.UserUnit;
		ClearPartyFocusState();
		ClearUnitListFocus();
	}

	private void ReplaceFocusWithPickedUnit(){
		Debug.Log("Replace Focus With PickedUnit...");
		int focusPos = GetUnitPosInParty(focusUnitInPartyMembers);

		if(!DataCenter.Instance.PartyInfo.ChangeParty(focusPos, pickedUnitFromUnitList.UserUnit.ID)){
			ClearUnitListFocus();
			ClearPartyFocusState();
			return ;
		}

		OutNoParty(pickedUnitFromPartyMembers.UserUnit);
		pickedUnitFromPartyMembers.UserUnit = pickedUnitFromUnitList.UserUnit;
		pickedUnitFromUnitList.IsFocus = false;
		pickedUnitFromUnitList.IsParty = true;
		pickedUnitFromUnitList = null;
		ClearPartyFocusState();
	}

//	void ExchangeData(MyUnitView first, )

	private bool AddUnitToPartyByOrder(int pos, MyUnitView target){
		if(pos > 3){
			Debug.LogError("Party is full, can not add new member...return!!!");
//			pickedUnitFromUnitList.UserUnit = target;
//			pickedUnitFromUnitList.IsFocus = true;
			return false;
		}

		if(partyView[ pos ].UserUnit != null){
			pos++;
			return AddUnitToPartyByOrder(pos, target);
		}
		else{
			//Access to add

			if(!DataCenter.Instance.PartyInfo.ChangeParty(pos, target.UserUnit.ID)){
				ClearUnitListFocus();
				ClearPartyFocusState();
				return false;
			}

			partyView[ pos ].UserUnit = target.UserUnit;
//			DataCenter.Instance.PartyInfo.ChangeParty(partyView);
			//MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.PartyInfo.CurrentParty);

			target.IsParty = true;
			target.IsFocus = false;
			ClearUnitListFocus();
			ClearPartyFocusState();
			return true;
		}
	}

	int GetUnitPosInParty(MyUnitView targetView){
		int pos = -1;
		foreach (var item in partyView){
			MyUnitView muv = item.Value;
			if(muv.Equals(targetView)){
				pos = item.Key;
				break;
			}
		}
		return pos;
	}

	bool FocusIsLeader()	{
		bool isLeader = false;
		if(focusUnitInPartyMembers.Equals(partyView[ 0 ])){
			isLeader = true;
		}
		else{
			isLeader = false;
		}

		//Debug.Log("Check isFocusLeader : " + isLeader);
		return isLeader;
	}

	bool CurrentPickedIsLeader(){
		bool isLeader = false;
		if(pickedUnitFromPartyMembers.Equals(partyView[ 0 ])){
			isLeader = true;
		}
		return isLeader;
	}

	bool ClearPartyFocusState() {
		if(focusUnitInPartyMembers != null){
			focusUnitInPartyMembers.IsFocus = false;
			focusUnitInPartyMembers = null;
		}
		return true;
	}

	bool ClearUnitListFocus(){
		if(pickedUnitFromUnitList != null){
			pickedUnitFromUnitList.IsFocus = false;
			pickedUnitFromUnitList = null;
		}
		return true;
	}

	private List<TUserUnit> GetUnitList(){
		if(DataCenter.Instance.MyUnitList.GetAll() == null){
			Debug.LogError("!!!Data Read Error!!! DataCenter.Instance.MyUnitList.GetAll() is return null!");
			return null;
		}

		List<TUserUnit> partyMembers = new List<TUserUnit>();
		partyMembers.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		//Debug.Log("partyMember's count is : " + partyMembers.Count);
		return partyMembers;
	}

	private void InitDragPanel(){
		dragPanel = new DragPanel("PartyDragPanel", PartyUnitView.ItemPrefab);
		dragPanel.CreatUI();
		InitDragPanelArgs();
		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);
		InitRejectBtn();
		InitUnitListView();
	}

	private void InitRejectBtn(){
		dragPanel.AddItem(1, rejectItem);
		GameObject rejectItemIns = dragPanel.ScrollItem[ 0 ];
		UIEventListener.Get(rejectItemIns).onClick = RejectPartyMember;
	}

	private void InitUnitListView(){
		memberList = GetUnitList();
		dragPanel.AddItem(memberList.Count, MyUnitView.ItemPrefab);
		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitView puv = PartyUnitView.Inject(dragPanel.ScrollItem[ i ]);
			puv.Init(memberList[ i - 1 ]);
			puv.callback = OutPartyItemClick;
			partyUnitViewList.Add(puv);
		}

		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		sortRuleLabel.text = curSortRule.ToString();
	}

	private void RejectPartyMember(GameObject item){
		Debug.Log("Click Reject item...");
		if(focusUnitInPartyMembers == null){
			Debug.Log("RejectPartyMember(), partyFocusUnit == null, reject from the last one by one...");
			RejectByOrder(partyView.Count - 1);
		}
		else{
			Debug.Log("RejectPartyMember(), partyFocusUnit != null, reject the focus...");
			int focusPos = GetPartyMemberPosition(focusUnitInPartyMembers.UserUnit);
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
		foreach (var item in partyView){
			if(item.Value.UserUnit == null) continue;
			if(item.Value.UserUnit.Equals(tuu)){
				pos = item.Key;
				return pos;
			}
		}
		return pos;
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans",		transform);
		dragPanelArgs.Add("scrollerScale",		Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	-28 * Vector3.up);
		dragPanelArgs.Add("position", 				Vector3.zero);
		dragPanelArgs.Add("clipRange", 			new Vector4(0, -120, 640, 400));
		dragPanelArgs.Add("gridArrange", 		UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine",			3);
		dragPanelArgs.Add("scrollBarPosition",	new Vector3(-320, -315, 0));
		dragPanelArgs.Add("cellWidth", 			120);
		dragPanelArgs.Add("cellHeight",			110);
	}
	
	void RejectByOrder(int pos){
		Debug.Log("RejectByOrder : " + pos);
		if(pos == 0) return;//Leader can not be rejected
		if(partyView[ pos ].UserUnit == null){
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
			PartyUnitView partyUnitView = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
			if(partyUnitView.UserUnit.Equals(partyView[ pos ].UserUnit)){
				partyUnitView.IsParty = false;
				partyUnitView.IsEnable = true;
				partyView[ pos ].UserUnit = null;
				break;
			}
		}
		//When reject every time, record party state change
		Debug.LogError("Reject pos : " + pos);
//		DataCenter.Instance.PartyInfo.ChangeParty(partyView);
		DataCenter.Instance.PartyInfo.ChangeParty(pos, 0); 
		//Message to note to refresh party info panel view 
		//MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.PartyInfo.CurrentParty);
		ClearPartyFocusState();
	}
	
	void ClickSortButton(GameObject btn){
		curSortRule = SortUnitTool.GetNextRule(curSortRule);
		sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, memberList);
		Debug.Log("xxxxx ....memberList : " + memberList.Count + " dragPanel.ScrollItem.Count : " + dragPanel.ScrollItem.Count);
		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitView puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
			puv.UserUnit = memberList[ i - 1 ];//before
			puv.CurrentSortRule = curSortRule;//after
		}
	}

	void RefreshDragPanel(){
		memberList = GetUnitList();
		int memCount = memberList.Count;
		int dragCount = dragPanel.ScrollItem.Count - 1;
		if( memCount >  dragCount){
			int addItemCount = memberList.Count - dragCount;//the first one is reject item
			dragPanel.AddItem(addItemCount, MyUnitView.ItemPrefab);
			dragCount = dragPanel.ScrollItem.Count;
			//Debug.Log("RefreshDragPanel(), dragPanel count : " + dragPanel.ScrollItem.Count + "  member list count : " + memberList.Count);
			for (int i = 1; i < dragCount; i++) {
				//RefreshData
				PartyUnitView puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
				if(puv == null){
					puv.Init(memberList[ i - 1 ]);
				}
				else
					puv.UserUnit = memberList[ i - 1 ];//before

				puv.CurrentSortRule = curSortRule;//after	
			}
		}
		else{
//			Debug.Log("RefreshDragPanel memberList : " + memberList.Count + "  dragPanel.ScrollItem.Count " +dragCount
			//Refresh
			for (int i = 0; i < memCount; i++) {
				PartyUnitView puv = dragPanel.ScrollItem[ i + 1 ].GetComponent<PartyUnitView>();
				puv.UserUnit = memberList[ i ];//before
				puv.CurrentSortRule = curSortRule;//after
			}

			//Remove
			for (int i = memCount + 1; i < dragPanel.ScrollItem.Count; i++) {
				Debug.LogError("i : " + i + " dragPanel.ScrollItem[ i ] : " + dragPanel.ScrollItem[ i ]);
				dragPanel.RemoveItem(dragPanel.ScrollItem[ i ]);
			}
			dragPanel.Refresh();
		}
	}

}
