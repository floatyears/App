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
	private UIButton prePageButton;
	private UIButton nextPageButton;
	private UIButton sortButton;

	private MyUnitView pickedUnitFromPartyMembers;
	private MyUnitView focusUnitInPartyMembers;
	private MyUnitView pickedUnitFromUnitList;

	private Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private Dictionary<int, PageUnitView> partyView = new Dictionary<int, PageUnitView>();
	List<TUserUnit> memberList = new List<TUserUnit>();
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitPagePanel();
		InitDragPanel();
	}

	public override void ShowUI(){
		base.ShowUI();

		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
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
		//Debug.LogError("Partyed count is : " + partyData.Count);
		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexLabel.text = curPartyIndex.ToString();
		rightIndexLabel.text = curPartyIndex.ToString();
		pageIndexSuffixLabel.text = UnitsWindow.partyIndexDic[ curPartyIndex ].ToString();

		for (int i = 0; i < partyData.Count; i++){
			partyView [ i ].Init(partyData [ i ]);
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
				Debug.Log("onPartyPickedUnit != null");
			}
			else{
				focusUnitInPartyMembers = pickedUnitFromPartyMembers;
				focusUnitInPartyMembers.IsFocus = true;
			}
		} 
		else {
			Debug.LogError("partyFocusUnit != null");

			if (FocusIsLeader() && (pickedUnitFromPartyMembers.UserUnit == null)) {
				Debug.Log("Check Focus is Leader... clear focus and return...");
				ClearFocusState();
				return;
			}

			if(CurrentPickedIsLeader() && focusUnitInPartyMembers.UserUnit == null){
				Debug.Log("Check Picked is Leader... clear focus and return...");
				ClearFocusState();
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

			TUserUnit tuu = pickedUnitFromPartyMembers.UserUnit; 
			pickedUnitFromPartyMembers.UserUnit = focusUnitInPartyMembers.UserUnit;
			focusUnitInPartyMembers.UserUnit = tuu;

			DataCenter.Instance.PartyInfo.ChangeParty(afterPos, beforeId);
			DataCenter.Instance.PartyInfo.ChangeParty(beforePos, afterId);
			ClearFocusState();
		}		
	}


	/// <summary>
	/// Click the item in unit list
	/// </summary>
	/// <param name="puv">Puv.</param>
	void OutPartyItemClick(MyUnitView puv){
		AddUnitToPartyByOrder(1, puv.UserUnit);
//		if(pickedUnitFromUnitList != null){//have clicked bottom unit item before
//			pickedUnitFromUnitList.IsFocus = false;
//			if(pickedUnitFromUnitList.Equals(puv)){//click bottom unit item again, change focus state only
//				pickedUnitFromUnitList = null;
//			}
//			else{
//				pickedUnitFromUnitList = puv;
//				pickedUnitFromUnitList.IsFocus = true;
//			}
//		}
//		else{
//			if(focusUnitInPartyMembers != null){
//				focusUnitInPartyMembers.IsFocus = false;
//				
//				//Traverse unit list, change the state of focus unit which has on-party to out-party
//				for (int i = 1; i < dragPanel.ScrollItem.Count; i++) {
//					PartyUnitView partyUnitView = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
//					if(partyUnitView.UserUnit.Equals(focusUnitInPartyMembers.UserUnit)){
//						partyUnitView.IsParty = false;
//						partyUnitView.IsEnable = true;
//						break;
//					}
//				}
//				
//				int onPartyPos = 0 ;
//				uint onPartyId = 0;
//				uint toReplaceId = puv.UserUnit.ID;
//				
//				foreach (var item in partyView) {
//					if(item.Value.UserUnit == null) continue;
//					if(item.Value.UserUnit.Equals(focusUnitInPartyMembers.UserUnit)){
//						onPartyPos = item.Key;
//						onPartyId = item.Value.UserUnit.ID;
//						Debug.Log("Find afterClickItem...");
//					}
//				}
//				Debug.Log("onPartyId is : " + onPartyId);
//				Debug.Log("toReplaceId is : " + toReplaceId);
//				Debug.Log("onPartyPos is : " + onPartyPos);
//				DataCenter.Instance.PartyInfo.ChangeParty(onPartyPos, 0);
//				DataCenter.Instance.PartyInfo.ChangeParty(onPartyPos, toReplaceId);
//				
//				focusUnitInPartyMembers.UserUnit = puv.UserUnit;
//				puv.IsParty = true;
//				puv.IsEnable = false;
//				focusUnitInPartyMembers = null;
//			}
//			else{
//				pickedUnitFromUnitList = puv;
//				pickedUnitFromUnitList.IsFocus = true;
//			}
//		}
	}
	
	private void AddUnitToPartyByOrder(int pos, TUserUnit target){
		if(pos > 3){
			Debug.LogError("Party is full, can not add new member...return!!!");
			pickedUnitFromUnitList.UserUnit = target;
			pickedUnitFromUnitList.IsFocus = true;
			return;
		}

		if(partyView[ pos ].UserUnit == null){
			pos++;
			AddUnitToPartyByOrder(pos, target);
		}
		else{
			DataCenter.Instance.PartyInfo.ChangeParty(pos, target.ID);
			partyView[ pos ].UserUnit = target;
		}
	}


	bool FocusIsLeader()	{
		bool isLeader = false;
		if(focusUnitInPartyMembers.Equals(partyView[ 0 ])){
			isLeader = true;
		}
		else{
			isLeader = false;
		}

		Debug.Log("Check isFocusLeader : " + isLeader);
		return isLeader;
	}

	bool CurrentPickedIsLeader(){
		bool isLeader = false;
		if(pickedUnitFromPartyMembers.Equals(partyView[ 0 ])){
			isLeader = true;
		}
		return isLeader;
	}

	bool ClearFocusState() {
		focusUnitInPartyMembers.IsFocus = false;
		focusUnitInPartyMembers = null;
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
		}
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
				ClearFocusState();
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
		if(pos == 0) return;//Leader can not be rejected
		if(partyView[ pos ].UserUnit == null){
			pos--;
			RejectByOrder(pos);
		}
		Reject(pos);
	}

	void RejectFocus(int pos){
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
		DataCenter.Instance.PartyInfo.ChangeParty(pos, 0);
		//Message to note to refresh party info panel view 
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.PartyInfo.CurrentParty);
	}
	
	void ClickSortButton(GameObject btn){
		curSortRule = SortUnitTool.GetNextRule(curSortRule);
		UILabel label = sortButton.transform.FindChild("Label").GetComponent<UILabel>();
		label.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, memberList);
		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitView puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
			puv.UserUnit = memberList[ i - 1 ];
			puv.CurrentSortRule = curSortRule;
		}
	}

}
