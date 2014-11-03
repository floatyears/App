using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class FriendListView : ViewBase{
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private FriendInfo curPickedFriend;
	private UIButton updateBtn;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitUIElement();
		LogHelper.Log("FriendListView.CreateDragView(), receive call from logic, to create ui...");
		dragPanel = new DragPanel("ApplyDragPanel","Prefabs/UI/UnitItem/FriendUnitPrefab",typeof(FriendUnitItem), transform);
	}

	public override void ShowUI(){
		base.ShowUI();

		dragPanel.SetData<FriendInfo> (DataCenter.Instance.FriendData.Friend, ClickItem  as DataListener);
		SortUnitByCurRule();
		RefreshCounter();
		AddCmdListener();
//		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
//		dragPanel.DestoryUI();
//		if(dragPanel != null)
//			dragPanel.DestoryUI();

		RmvCmdListener();
	}
		
	public override void DestoryUI ()
	{
		if(dragPanel != null)
			dragPanel.DestoryUI();
		base.DestoryUI ();
	}

	void EnableUpdateButton(object args){
		updateBtn.gameObject.SetActive(true);
		UIEventListenerCustom.Get(updateBtn.gameObject).onClick += ClickUpdateBtn;
	}

	void ClickRefuseButton(GameObject args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefuseApplyButtonClick", null);
//		ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.FriendListModule, "RefuseApplyButtonClick");
	}

	void InitUIElement(){
		updateBtn = FindChild<UIButton>("Button_Update");
		UIEventListenerCustom.Get(updateBtn.gameObject).onClick = ClickUpdateBtn;
		curSortRule = SortUnitTool.GetSortRule(SortRuleByUI.FriendListView);
	}

	void ClickItem(object data){
		FriendUnitItem item = data as FriendUnitItem;
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		curPickedFriend = item.FriendInfo;
//		MsgCenter.Instance.Invoke(CommandEnum.FriendBriefInfoShow, curPickedFriend);

		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
		ModuleManager.Instance.ShowModule (ModuleEnum.ApplyMessageModule, "data", curPickedFriend,"title",TextCenter.GetText ("DeleteNoteTitle"),"content",TextCenter.GetText ("DeleteNoteContent"));
	}


	void ClickUpdateBtn(GameObject button){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("RefreshFriend"), TextCenter.GetText ("ConfirmRefreshFriend"), TextCenter.GetText ("OK"),TextCenter.GetText ("CANCEL"), CallBackRefreshFriend);
	}

	void CallBackRefreshFriend(object args){
//		MsgCenter.Instance.Invoke(CommandEnum.EnsureUpdateFriend);
		FriendController.Instance.GetFriendList(OnGetFriendList,true,false);
	}

	void OnGetFriendList(object data){
		if (data == null)
			return;
		bbproto.RspGetFriend rsp = data as bbproto.RspGetFriend;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			LogHelper.Log("RspGetFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
		DataCenter.Instance.FriendData.RefreshFriendList(inst);
	}

	void DeleteFriendPicked(object msg){
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("DeleteNoteTitle"), TextCenter.GetText ("DeleteNoteContent"), TextCenter.GetText ("OK"), TextCenter.GetText ("CANCEL"), CallBackDeleteFriend);
	}

	void CallBackDeleteFriend(object args){
		FriendController.Instance.DelFriend(OnDelFriend, curPickedFriend.userId);
	}

	void OnDelFriend(object data){
		if (data == null)
			return;
		
		Debug.Log("TFriendList.OnDelFriend() begin");
		LogHelper.Log(data);
		bbproto.RspDelFriend rsp = data as bbproto.RspDelFriend;
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rsp.header.code+", error:"+rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
		LogHelper.LogError("OnRspDelFriend friends {0}", rsp.friends);
		
		DataCenter.Instance.FriendData.RefreshFriendList(inst);
		
//		ShowUI();

		SortUnitTool.SortByTargetRule(curSortRule, DataCenter.Instance.FriendData.Friend);
		dragPanel.SetData<FriendInfo> (DataCenter.Instance.FriendData.Friend);
	}

	void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.GetText("FriendCounterTitle");
		int current = DataCenter.Instance.FriendData.FriendCount;
		int max = DataCenter.Instance.UserData.UserInfo.friendMax;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		countArgs.Add("posy", -774);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private void ReceiveSortInfo(object msg){
		//curSortRule = SortUnitTool.GetNextRule(curSortRule);
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){

		SortUnitTool.SortByTargetRule(curSortRule, DataCenter.Instance.FriendData.Friend);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.FriendListView);

		dragPanel.SetData<FriendInfo> (DataCenter.Instance.FriendData.Friend);
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);     
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
}


