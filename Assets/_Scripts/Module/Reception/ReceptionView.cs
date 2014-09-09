using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReceptionView : ViewBase {
	private SortRule curSortRule;
	private TFriendInfo curPickedFriend;
	private DragPanel dragPanel;
	private UIButton refuseAllBtn;
	private List<TFriendInfo> friendInDataList = new List<TFriendInfo>();

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
//		Debug.LogError("ReceptionView Init 1");
		base.Init(config, data);
//		Debug.LogError("ReceptionView Init 2");
		InitUIElement();
//		Debug.LogError("ReceptionView Init 3");
	}

	public override void ShowUI(){
//		Debug.LogError("ReceptionView 1");
		base.ShowUI();
//		Debug.LogError("ReceptionView 2");
		AddCmdListener();
//		Debug.LogError("ReceptionView 3");
//		Debug.LogError("ReceptionView.ShowUI()...");
		CreateDragView();
//		Debug.LogError("ReceptionView 4");
		SortUnitByCurRule();
//		Debug.LogError("ReceptionView 5");
		RefreshCounter();
//		Debug.LogError("ReceptionView 6");
		ShowUIAnimation();
//		Debug.LogError("ReceptionView 7");
	}

	public override void HideUI(){
		base.HideUI();
		if(dragPanel != null)
			dragPanel.DestoryUI();
		RmvCmdListener();
	}

	private void InitUIElement(){
		refuseAllBtn = FindChild<UIButton>("Button_Refuse");
		UILabel refuseBtnLabel = FindChild<UILabel>("Button_Refuse/Label_Text");
		refuseBtnLabel.text = TextCenter.GetText("Btn_Reception_RefuseAll");
		UIEventListener.Get(refuseAllBtn.gameObject).onClick = ClickRefuseBtn;

		curSortRule = SortUnitTool.GetSortRule(SortRuleByUI.ReceptionView);
	}

	private void CreateDragView(){
//		Debug.LogError("CreateDragView(), Reception...");
		friendInDataList = DataCenter.Instance.FriendList.FriendIn;
		dragPanel = new DragPanel("ApplyDragPanel", FriendUnitItem.ItemPrefab,transform);
//		dragPanel.CreatUI();
		dragPanel.AddItem(friendInDataList.Count);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitItem fuv = FriendUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Init(friendInDataList[ i ]);
			fuv.callback = ClickItem;
		}
	}

	private void ClickRefuseBtn(GameObject args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetMsgWindowParams());
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("RefuseAll"), TextCenter.GetText ("ConfirmRefuseAll"), TextCenter.GetText ("OK"), TextCenter.GetText ("CANCEL"), CallbackRefuseAll);
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
		string title = TextCenter.GetText("ReceptionCounterTitle");
		int current = DataCenter.Instance.FriendList.FriendIn.Count;
		int max = 0;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		countArgs.Add("posy", -766);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	void ClickItem(FriendUnitItem item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, curPickedFriend);
	}

	void DeleteFriendPicked(object msg){
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetDeleteMsgParams());
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("DeleteNoteTitle"), TextCenter.GetText ("DeleteNoteContent"), TextCenter.GetText ("OK"), TextCenter.GetText ("CANCEL"), CallBackDeleteFriend);
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
		if(CheckFriendCountLimit()){
			Debug.LogError(string.Format("Friend Count limited. Current Friend count is :" + DataCenter.Instance.FriendCount));
//			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetFriendExpansionMsgParams());
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FirendOverflow"),
			                                   TextCenter.GetText("FriendOverflowText",DataCenter.Instance.UserUnitList.GetAllMyUnit().Count,DataCenter.Instance.UserInfo.UnitMax),
			                                   TextCenter.GetText("DoFriendExpand"),TextCenter.GetText("CANCEL"),CallBackScratchScene);
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

	void CallBackScratchScene(object args){
		ModuleManager.Instance.ShowModule(ModuleEnum.ShopModule);
	}

	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(curSortRule, friendInDataList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.ReceptionView);

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

	private void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, -470, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
	}

}
