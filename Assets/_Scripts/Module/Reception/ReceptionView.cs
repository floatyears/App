using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ReceptionView : ViewBase {
	private SortRule curSortRule;
	private FriendInfo curPickedFriend;
	private DragPanel dragPanel;
	private UIButton refuseAllBtn;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		refuseAllBtn = FindChild<UIButton>("Button_Refuse");
		UILabel refuseBtnLabel = FindChild<UILabel>("Button_Refuse/Label_Text");
		refuseBtnLabel.text = TextCenter.GetText("Btn_Reception_RefuseAll");
		UIEventListenerCustom.Get(refuseAllBtn.gameObject).onClick = ClickRefuseBtn;
		
		curSortRule = SortUnitTool.GetSortRule(SortRuleByUI.ReceptionView);

		dragPanel = new DragPanel("ApplyDragPanel", "Prefabs/UI/UnitItem/FriendUnitPrefab" ,typeof(FriendUnitItem), transform);
		
		dragPanel.SetData<FriendInfo> (DataCenter.Instance.FriendData.FriendIn, ClickItem as DataListener);
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		SortUnitByCurRule();
		RefreshCounter();
//		ShowUIAnimation();
	}

	public override void HideUI(){
		RmvCmdListener();
		base.HideUI();

	}

	public override void DestoryUI ()
	{
		dragPanel.DestoryUI();
		base.DestoryUI ();
	}

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			//			Debug.Log("Show Module!: [[[---" + config.moduleName + "---]]]pos: " + config.localPosition.x + " " + config.localPosition.y);
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);

//			transform.localPosition = new Vector3(-1000, -470, 0);
//			iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
			//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
			//			Debug.Log("Hide Module!: [[[---" + config.moduleName + "---]]]");
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
			//			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true,"oncomplete","AnimationComplete","oncompletetarget",gameObject));
		}

	}

	private void ClickRefuseBtn(GameObject args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("RefuseAll"), TextCenter.GetText ("ConfirmRefuseAll"), TextCenter.GetText ("OK"), TextCenter.GetText ("CANCEL"), CallbackRefuseAll);
	}

	void CallbackRefuseAll(object args){
		MsgCenter.Instance.Invoke(CommandEnum.EnsureRefuseAll);
		RefuseAllApplyFromOthers ();
	}

	void RefuseAllApplyFromOthers(){
		List <uint> refuseList = new List<uint>();
		for (int i = 0; i < DataCenter.Instance.FriendData.FriendIn.Count; i++)
			refuseList.Add(DataCenter.Instance.FriendData.FriendIn [i].userId);
		FriendController.Instance.DelFriend(OnDelFriend, refuseList);
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
		DataCenter.Instance.FriendData.RefreshFriendList(inst);
	}

	void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.GetText("ReceptionCounterTitle");
		int current = DataCenter.Instance.FriendData.FriendIn.Count;
		int max = 0;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		countArgs.Add("posy", -766);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	void ClickItem(object data){
		FriendUnitItem item = data as FriendUnitItem;
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
		ModuleManager.Instance.ShowModule (ModuleEnum.ApplyMessageModule, "data", curPickedFriend,"title",TextCenter.GetText ("AcceptApply"),"content",TextCenter.GetText ("ConfirmAccept"));
	}

	void DeleteFriendPicked(object msg){
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("DeleteNoteTitle"), TextCenter.GetText ("DeleteNoteContent"), TextCenter.GetText ("OK"), TextCenter.GetText ("CANCEL"), CallBackDeleteFriend);
	}

	void CallBackDeleteFriend(object args){
		FriendController.Instance.DelFriend(OnDelFriend, curPickedFriend.userId);
	}

	void DeleteApplyFromOther(object msg){
//		Debug.LogError("FriendListLogic.DeleteApplyFromOther(), receive the message, to delete apply from other player...");
		RefuseFriend(curPickedFriend.userId);
	}

	void RefuseFriend(uint friendUid){
		FriendController.Instance.DelFriend(OnDelFriend, friendUid);
	}

	void AcceptApplyFromOther(object msg){
		if(CheckFriendCountLimit()){
			Debug.LogError(string.Format("Friend Count limited. Current Friend count is :" + DataCenter.Instance.FriendData.FriendCount));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FirendOverflow"),
			                                   TextCenter.GetText("FriendOverflowText",DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count,DataCenter.Instance.UserData.UserInfo.unitMax),
			                                   TextCenter.GetText("DoFriendExpand"),TextCenter.GetText("CANCEL"),CallBackScratchScene);
			return;
		}
		
//		AcceptFriendRequest();
		FriendController.Instance.AcceptFriend(OnAcceptFriend, curPickedFriend.userId);
	}

	bool CheckFriendCountLimit(){
		if(DataCenter.Instance.FriendData.Friend.Count >= DataCenter.Instance.UserData.UserInfo.friendMax){
			Debug.LogError("Friend.Count > FriendMax!!! Refuse to add this friend...");
			return true;
		}
		else
			return false;
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
		DataCenter.Instance.FriendData.RefreshFriendList(inst);
		
		SortUnitByCurRule ();
	}

	void CallBackScratchScene(object args){
		ModuleManager.Instance.ShowModule(ModuleEnum.ShopModule);
	}

	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(curSortRule, DataCenter.Instance.FriendData.FriendIn);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.ReceptionView);

		dragPanel.SetData<FriendInfo> (DataCenter.Instance.FriendData.FriendIn);
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);   
		MsgCenter.Instance.AddListener(CommandEnum.EnsureRefuseSingleApply, DeleteApplyFromOther); 
		MsgCenter.Instance.AddListener(CommandEnum.EnsureAcceptApply, AcceptApplyFromOther);
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked); 
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureRefuseSingleApply, DeleteApplyFromOther); 
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureAcceptApply, AcceptApplyFromOther);
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}


}
