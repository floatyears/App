using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPartyMembers : UIComponentUnity
{

	public override void Init(UIInsConfig config, IUICallback origin)
	{
		Debug.LogError("Init 1");
		base.Init(config, origin);
		Debug.LogError("Init 2");
	}

	public override void ShowUI()
	{
		Debug.LogError("showui");
		base.ShowUI();
		InitParty();
		Debug.LogError("showui 2");
	}
	
//	List<PartyUnitView> partyMemberList = new List<PartyUnitView>();
//
//	PartyUnitView currentPick;
//	public PartyUnitView CurrentPick{
//		get{
//			return currentPick;
//		}
//		set{
//			currentPick = value;
//		}
//	}
//
//	PartyUnitView focus;
//	public PartyUnitView Focus{
//		get{
//			return focus;
//		}
//		set{
//			focus = value;
//		}
//	}
//
//	//接受当前点击
//	void ReceiveInput(int clickPos){
//		RecordInput(clickPos);
//		DealInput();
//	}
//
//	//记录当前点击
//	void RecordInput(int clickPos ){
//		//有可能点击位置所对应的partyMemeber为null
//		currentPick = partyMemberList[clickPos];
//	}
//
//	//处理当前点击
//	void DealInput(){
//		if(focus == null){
//			//当前focus == null 说明没有选中任何member,
//			//那么点击操作意味着应该只是记录focus状态
//			if(currentPick == null){}
//			else{}
//
//			focus = currentPick;
//			//ShowFocusLightSpr
//		}
//		else{//focus != null
//			//当前focus != null 说明已经有选中某一个partyMember了,
//			//那么再次点击其他的member就意味着应该直接进行交换操作了
//
//			if( FocusIsLeader() && (currentPick == null) ){
//				//如果focus是队长,当前选中的又是空的,那么就不执行互换,因为换了队长就为空了
//				return;
//			}
//
//
//			//Exchange
//			PartyUnitView temp = currentPick;
//			currentPick = focus;
//			focus = temp;
//
//			//clear focus
//			ClearFocusState();
//		}
//
//	}
//
//	void ClearFocusState(){
//		//交换完了之后,应该清除focus状态
//		focus = null;
//	}
//
//	bool FocusIsLeader(){
//		bool isLeader = false;
//		return isLeader;
//	}

	private UILabel pageLabel;
	private UIButton prePageButton;
	private UIButton nextPageButton;
	private Dictionary<int,PartyUnitView> partyView = new Dictionary<int, PartyUnitView>();
	private PartyUnitView currentPick;
	private PartyUnitView recordPick;

	private void InitParty()
	{
		pageLabel = FindChild<UILabel>("Label_Left/Label_Before");
		prePageButton = FindChild<UIButton>("Button_Left");
		UIEventListener.Get(prePageButton.gameObject).onClick = PrevPage;
		nextPageButton = FindChild<UIButton>("Button_Right");
		UIEventListener.Get(nextPageButton.gameObject).onClick = NextPage;

		for (int i = 0; i < 4; i++)
		{
			PartyUnitView puv = FindChild<PartyUnitView>(i.ToString());
			partyView.Add(i, puv);
			puv.callback = PartyCallback;
		}

		RefreshParty(DataCenter.Instance.PartyInfo.CurrentParty);
	}

	void PrevPage(GameObject go)
	{
		RefreshParty(DataCenter.Instance.PartyInfo.PrevParty);           
	}
        
	void NextPage(GameObject go)
	{
		RefreshParty(DataCenter.Instance.PartyInfo.NextParty);
	}

	void RefreshParty(TUnitParty party)
	{
		List<TUserUnit> partyData = party.GetUserUnit();
		pageLabel.text = DataCenter.Instance.PartyInfo.CurrentPartyId.ToString();
		for (int i = 0; i < partyData.Count; i++)
		{
			partyView [i].Init(partyData [i]);
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
		} else {
			if (FocusIsLeader() && (currentPick == null)) {
				return;
			}
			TUserUnit tuu = currentPick.UserUnit;            		
			currentPick.Init(recordPick.UserUnit);
			recordPick.Init(tuu);			                            
			ClearFocusState();
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
}
