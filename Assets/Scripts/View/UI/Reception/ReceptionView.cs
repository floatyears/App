using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReceptionView : UIComponentUnity {
	private SortRule curSortRule;
	private TFriendInfo curPickedFriend;
	private DragPanel dragPanel;
	private UIButton sortBtn;
	private UILabel sortRuleLabel;
	private UIButton refuseAllBtn;
	private List<TFriendInfo> friendInDataList = new List<TFriendInfo>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		CreateDragView();
		SortUnitByCurRule();
		RefreshCounter();
	}

	public override void HideUI(){
		base.HideUI();
		dragPanel.DestoryUI();
		RmvCmdListener();
	}

	private void InitUIElement(){
		sortBtn = FindChild<UIButton>("Button_Sort");
		sortRuleLabel = transform.FindChild("Button_Sort/Label_Rule").GetComponent<UILabel>();
		refuseAllBtn = FindChild<UIButton>("Button_Refuse");
		UIEventListener.Get(refuseAllBtn.gameObject).onClick = ClickRefuseBtn;
		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;
		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
	}

	private void CreateDragView(){
		friendInDataList = DataCenter.Instance.FriendList.FriendIn;
		dragPanel = new DragPanel("ReceptionDragPanel", FriendUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(friendInDataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.FriendListDragPanelArgs, transform);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitItem fuv = FriendUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Init(friendInDataList[ i ]);
			fuv.callback = ClickItem;
		}
	}

	private void ClickRefuseBtn(GameObject args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetMsgWindowParams());
	}

	MsgWindowParams GetMsgWindowParams(){
		MsgWindowParams msgWindowParam = new MsgWindowParams();
		msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("RefuseAll");
		msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("ConfirmRefuseAll");
		msgWindowParam.btnParams = new BtnParam[ 2 ]{new BtnParam(), new BtnParam()};
		msgWindowParam.btnParams[ 0 ].callback = CallbackRefuseAll;
		return msgWindowParam;
	}

	void CallbackRefuseAll(object args){
		MsgCenter.Instance.Invoke(CommandEnum.EnsureRefuseAll);
	}

	void RefuseAllApplyFromOthers(object msg){
		List <uint> refuseList = new List<uint>();
		for (int i = 0; i < DataCenter.Instance.FriendList.FriendIn.Count; i++)
			refuseList.Add(DataCenter.Instance.FriendList.FriendIn [i].UserId);
		DelFriend.SendRequest(OnDelFriend, refuseList);
	}

	void OnDelFriend(object data){
		if (data == null)
			return;
		bbproto.RspDelFriend rsp = data as bbproto.RspDelFriend;
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.LogError("OnRspDelFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);

			return;
		}
		bbproto.FriendList inst = rsp.friends;
		DataCenter.Instance.SetFriendList(inst);
		HideUI();
		ShowUI();
	}

	void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.Instace.GetCurrentText("ReceptionCounterTitle");
		int current = DataCenter.Instance.FriendList.FriendIn.Count;
		int max = 0;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	void ClickItem(FriendUnitItem item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, curPickedFriend);
	}

	void DeleteFriendPicked(object msg){
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetDeleteMsgParams());
	}

	MsgWindowParams GetDeleteMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.Instace.GetCurrentText("DeleteNoteTitle");
		msgParams.contentText = TextCenter.Instace.GetCurrentText("DeleteNoteContent");
		msgParams.btnParams = new BtnParam[ 2 ]{ new BtnParam(), new BtnParam()};
		msgParams.btnParams[ 0 ].callback = CallBackDeleteFriend;
		return msgParams;
	}

	void CallBackDeleteFriend(object args){
		DelFriend.SendRequest(OnDelFriend, curPickedFriend.UserId);
	}

	void DeleteApplyFromOther(object msg){
		Debug.LogError("FriendListLogic.DeleteApplyFromOther(), receive the message, to delete apply from other player...");
		RefuseFriend(curPickedFriend.UserId);
	}

	void RefuseFriend(uint friendUid){
		DelFriend.SendRequest(OnDelFriend, friendUid);
	}

	void AcceptApplyFromOther(object msg){
//		Debug.LogError("FriendListLogic.AcceptApplyFromOther(), receive the message, to accept apply from other player...");
		if(CheckFriendCountLimit()){
			Debug.LogError(string.Format("Friend Count limited. Current Friend count is :" + DataCenter.Instance.FriendCount));
			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetFriendExpansionMsgParams());
			return;
		}
		
		AcceptFriendRequest(curPickedFriend.UserId);
	}

	bool CheckFriendCountLimit(){
		if(DataCenter.Instance.FriendList.Friend.Count >= DataCenter.Instance.UserInfo.FriendMax){
			Debug.LogError("Friend.Count > FriendMax!!! Refuse to add this friend...");
			return true;
		}
		else
			return false;
	}

	void AcceptFriendRequest(uint friendUid){
		AcceptFriend.SendRequest(OnAcceptFriend, friendUid);
	}

	void OnAcceptFriend(object data){
		if (data == null)
			return;
		
		bbproto.RspAcceptFriend rsp = data as bbproto.RspAcceptFriend;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.Log("RspAddFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);

			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
		DataCenter.Instance.SetFriendList(inst);
		
		HideUI();
		ShowUI();
	}

	MsgWindowParams GetFriendExpansionMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.Instace.GetCurrentText("FirendOverflow");
		msgParams.contentText = TextCenter.Instace.GetCurrentText("FriendOverflowText",
		                                                          DataCenter.Instance.MyUnitList.Count,
		                                                          DataCenter.Instance.UserInfo.UnitMax);
		msgParams.btnParams = new BtnParam[2]{ new BtnParam(), new BtnParam()};
		msgParams.btnParams[ 0 ].text = TextCenter.Instace.GetCurrentText("DoFriendExpand");
		msgParams.btnParams[ 0 ].callback = CallBackScratchScene;
		return msgParams;
	}

	void CallBackScratchScene(object args){
		UIManager.Instance.ChangeScene(SceneEnum.Shop);
	}

	void ClickSortBtn(GameObject btn){
		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
	}
	
	private void SortUnitByCurRule(){
		sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, friendInDataList);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitItem fuv = dragPanel.ScrollItem[ i ].GetComponent<FriendUnitItem>();
			fuv.UserUnit = friendInDataList[ i ].UserUnit;
			fuv.CurrentSortRule = curSortRule;
		}
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.EnsureRefuseAll, RefuseAllApplyFromOthers);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);   
		MsgCenter.Instance.AddListener(CommandEnum.EnsureRefuseSingleApply, DeleteApplyFromOther); 
		MsgCenter.Instance.AddListener(CommandEnum.EnsureAcceptApply, AcceptApplyFromOther);
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureRefuseAll, RefuseAllApplyFromOthers);  
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked); 
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureRefuseSingleApply, DeleteApplyFromOther); 
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureAcceptApply, AcceptApplyFromOther);
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

}
