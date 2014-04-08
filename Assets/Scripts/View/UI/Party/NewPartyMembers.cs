using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPartyMembers : UIComponentUnity{
	private DragPanel dragPanel;
	private GameObject rejectItem;
	private UILabel pageLabel;
	private UIButton prePageButton;
	private UIButton nextPageButton;
	private PartyUnitView currentPick;
	private PartyUnitView recordPick;
	private Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private Dictionary<int, PartyUnitView> partyView = new Dictionary<int, PartyUnitView>();
	
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
	}

	public override void ShowUI(){
		base.ShowUI();
		InitPagePanel();
		InitDragPanel();
	}

	private void InitPagePanel(){
		pageLabel = FindChild<UILabel>("Label_Left/Label_Before");
		prePageButton = FindChild<UIButton>("Button_Left");
		UIEventListener.Get(prePageButton.gameObject).onClick = PrevPage;
		nextPageButton = FindChild<UIButton>("Button_Right");
		UIEventListener.Get(nextPageButton.gameObject).onClick = NextPage;
		rejectItem = Resources.Load("Prefabs/UI/Friend/RejectItem") as GameObject;

		for (int i = 0; i < 4; i++){
			PartyUnitView puv = FindChild<PartyUnitView>(i.ToString());
			partyView.Add(i, puv);
			puv.callback = PartyCallback;
		}
		RefreshParty(DataCenter.Instance.PartyInfo.CurrentParty);
	}

	void PrevPage(GameObject go){
		RefreshParty(DataCenter.Instance.PartyInfo.PrevParty);           
	}
        
	void NextPage(GameObject go){
		RefreshParty(DataCenter.Instance.PartyInfo.NextParty);
	}

	void RefreshParty(TUnitParty party){
		List<TUserUnit> partyData = party.GetUserUnit();
		pageLabel.text = DataCenter.Instance.PartyInfo.CurrentPartyId.ToString();
		for (int i = 0; i < partyData.Count; i++){
			partyView [ i ].Init(partyData [ i ]);
			partyView[ i ].IsEnable = true;
			partyView[ i ].IsParty = false;
		}
	}
        
	void PartyCallback(PartyUnitView puv) {
		currentPick = puv;
		DealInput();
	}

	void DealInput() {
		if (recordPick == null) {
			recordPick = currentPick;
			recordPick.IsFocus = true;
		} 
		else {
			if (FocusIsLeader() && (currentPick == null)) {
				return;
			}
			TUserUnit tuu = currentPick.UserUnit;            		
			currentPick.Init(recordPick.UserUnit);
			recordPick.Init(tuu);			                            
			ClearFocusState();
//			DataCenter.Instance.PartyInfo.ChangeParty();
//			currentPick.IsParty = true;
//			currentPick.IsEnable = false;
		}		
	}

	bool FocusIsLeader()	{
		foreach (var item in partyView) {
			if (recordPick.Equals(item.Value)){
				if (item.Key == 0) 
					return true;
				else 
					return false;
			}
		}
		return false;
	}

	bool ClearFocusState() {
		recordPick.IsFocus = false;
		recordPick = null;
		return true;
	}

	private List<TUserUnit> GetUnitList(){
		if(DataCenter.Instance.MyUnitList.GetAll() == null){
			Debug.LogError("!!!Data Read Error!!! DataCenter.Instance.MyUnitList.GetAll() is return null!");
			return null;
		}

		List<TUserUnit> partyMembers = new List<TUserUnit>();
		partyMembers.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		Debug.Log("partyMember's count is : " + partyMembers.Count);
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
		List<TUserUnit> memberList = GetUnitList();
		dragPanel.AddItem(memberList.Count, MyUnitView.ItemPrefab);
		//Debug.LogError("memberList.Count : " + memberList.Count);
		//Debug.LogError("dragPanel.ScrollItem.Count : " + dragPanel.ScrollItem.Count);

		for (int i = 1; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitView puv = PartyUnitView.Inject(dragPanel.ScrollItem[ i ]);
			puv.Init(memberList[ i - 1 ]);
			puv.callback = PartyCallback;
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
		if(pos == 1) return;//Lead can not reject
		if(partyView[ pos ].UserUnit == null){
			pos--;
			CheckReject(pos);
		}
		partyView[ pos ].UserUnit = null;
		DataCenter.Instance.PartyInfo.ChangeParty(pos, 0);
	}

}
