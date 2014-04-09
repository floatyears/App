using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPartyMembers : UIComponentUnity{
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private GameObject rejectItem;
	private UILabel pageIndexLabel;
	private UILabel pageIndexSuffixLabel;
	private UIButton prePageButton;
	private UIButton nextPageButton;
	private UIButton sortButton;

	private MyUnitView partyPickedUnit;
	private MyUnitView partyFocusUnit;
	private MyUnitView onPartyPickedUnit;

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
	}

	public override void HideUI(){
		base.HideUI();
		DataCenter.Instance.PartyInfo.ExitParty();
	}

	public override void DestoryUI(){
		base.DestoryUI();
		partyView.Clear();
	}

	private void InitPagePanel(){
		pageIndexLabel = FindChild<UILabel>("Label_Left/Label_Before");
		pageIndexSuffixLabel = FindChild<UILabel>("Label_Left/Label_After");

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

		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);
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

	void RefreshParty(TUnitParty party){
		List<TUserUnit> partyData = party.GetUserUnit();

		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		pageIndexLabel.text = curPartyIndex.ToString();
		pageIndexSuffixLabel.text = UnitsWindow.partyIndexDic[ curPartyIndex ].ToString();

		for (int i = 0; i < partyData.Count; i++){
			partyView [ i ].Init(partyData [ i ]);
		}
	}
        
	void PartyItemClick(MyUnitView puv) {
		partyPickedUnit = puv;
		DealWithPartyItemClick();
	}

	void OnPartyItemClick(MyUnitView puv){
		if(onPartyPickedUnit != null){//have clicked bottom unit item before
			onPartyPickedUnit.IsFocus = false;
			if(onPartyPickedUnit.Equals(puv)){//click bottom unit item again, change focus state only
				onPartyPickedUnit = null;
			}
			else{
				onPartyPickedUnit = puv;
				onPartyPickedUnit.IsFocus = true;
			}
		}
		else{
			if(partyFocusUnit != null){
				partyFocusUnit.IsFocus = false;
				for (int i = 1; i < dragPanel.ScrollItem.Count; i++) {
					PartyUnitView partyUnitView = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
					if(partyUnitView.UserUnit.Equals(partyFocusUnit.UserUnit)){
						partyUnitView.IsParty = false;
						partyUnitView.IsEnable = true;
						break;
					}
				}
				partyFocusUnit.UserUnit = puv.UserUnit;
				puv.IsParty = true;
				puv.IsEnable = false;
				partyFocusUnit = null;
			}
			else{
				onPartyPickedUnit = puv;
				onPartyPickedUnit.IsFocus = true;
			}
		}
	}

	void DealWithPartyItemClick() {
		if (partyFocusUnit == null) {
			partyFocusUnit = partyPickedUnit;
			partyFocusUnit.IsFocus = true;
		} 
		else {
			if (FocusIsLeader() && (partyPickedUnit == null)) {
				return;
			}
		
			int afterPos = 0;
			int beforePos = 0;
			uint beforeId = 0;
			uint afterId = 0;

			foreach (var item in partyView) {
				if(item.Value.Equals(partyPickedUnit.UserUnit)){
					afterPos = item.Key;
					afterId = item.Value.UserUnit.ID;
				}
				if(item.Value.Equals(partyFocusUnit.UserUnit)){
					beforePos = item.Key;
					beforeId = item.Value.UserUnit.ID;
				}
			}

			TUserUnit tuu = partyPickedUnit.UserUnit; 
			partyPickedUnit.Init(partyFocusUnit.UserUnit);
			partyFocusUnit.Init(tuu);	

//			DataCenter.Instance.PartyInfo.ChangeParty(afterPos, afterId);
//			DataCenter.Instance.PartyInfo.ChangeParty(beforePos, beforeId);

			ClearFocusState();
		}		
	}

	bool FocusIsLeader()	{
		foreach (var item in partyView) {
			if (partyFocusUnit.Equals(item.Value)){
				if (item.Key == 0) 
					return true;
				else 
					return false;
			}
		}
		return false;
	}

	bool ClearFocusState() {
		partyFocusUnit.IsFocus = false;
		partyFocusUnit = null;
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

		InitReject();
		InitUnitListView();
	}

	private void InitReject(){
		dragPanel.AddItem(1, rejectItem);
		GameObject rejectItemInstance = dragPanel.ScrollItem[ 0 ];
		UIEventListener.Get(rejectItemInstance).onClick = RejectPartyMember;
	}

	private void InitUnitListView(){
		memberList = GetUnitList();
		dragPanel.AddItem(memberList.Count, MyUnitView.ItemPrefab);

		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitView puv = PartyUnitView.Inject(dragPanel.ScrollItem[ i ]);
			puv.Init(memberList[ i - 1 ]);
			puv.callback = OnPartyItemClick;
		}
	}

	private void RejectPartyMember(GameObject item){
		Debug.Log("Click Reject item...");
		CheckReject(partyView.Count - 1);
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
	
	void CheckReject(int pos){
		if(pos == 1) return;//Leader can not reject
		if(partyView[ pos ].UserUnit == null){
			pos--;
			CheckReject(pos);
		}

		for (int i = 1; i < dragPanel.ScrollItem.Count; i++) {
			PartyUnitView partyUnitView = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
			if(partyUnitView.UserUnit.Equals(partyView[ pos ].UserUnit)){
				partyUnitView.IsParty = false;
				partyUnitView.IsEnable = true;
				partyView[ pos ].UserUnit = null;
				break;
			}
		}

		DataCenter.Instance.PartyInfo.ChangeParty(pos, 0);
	}
	
	void ClickSortButton(GameObject btn){
		curSortRule = UnitItemSort.GetNextRule(curSortRule);
		UILabel label = sortButton.transform.FindChild("Label").GetComponent<UILabel>();
		label.text = curSortRule.ToString();

		Debug.Log("Before :: memberList[ 4 ].Level ->" + memberList[ 4 ].Level);

		switch (curSortRule){
			case SortRule.ByAddPoint : 
				DGTools.InsertSort(memberList, new TUserUnitSortAddPoint());
				break;
			case SortRule.ByAttack : 
				DGTools.InsertSort(memberList, new TUserUnitSortAtk());
				break;
			case SortRule.ByAttribute : 
				DGTools.InsertSort(memberList, new TUserUnitSortAttribute());
				break;
			case SortRule.ByGetTime : 
				DGTools.InsertSort(memberList, new TUserUnitSortGetTime());
				break;
			case SortRule.ByHP : 
				DGTools.InsertSort(memberList, new TUserUnitSortHP());
				break;
			case SortRule.ByID : 
				DGTools.InsertSort(memberList, new TUserUnitSortID());
				break;
			case SortRule.ByIsCollected : 
				DGTools.InsertSort(memberList, new TUserUnitSortFavourite());
				break;
			case SortRule.ByRace : 
				DGTools.InsertSort(memberList, new TUserUnitSortRace());
				break;
			default:
				break;
		}
		Debug.Log("After :: memberList[ 4 ].Level ->" + memberList[ 4 ].Level);

		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitView puv = dragPanel.ScrollItem[ i ].GetComponent<PartyUnitView>();
			puv.UserUnit = memberList[ i - 1 ];
			puv.CurrentSortRule = curSortRule;
		}

	}

}
