using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ApplyView : ViewBase{
	private SortRule curSortRule;
	private FriendInfo curPickedFriend;
	private DragPanel dragPanel;
	private List<FriendInfo> friendOutDataList = new List<FriendInfo>();
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitUIElement();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		CreateDragView();
		SortUnitByCurRule();
		RefreshCounter();
		ShowUIAnimation();
	}
	
	public override void HideUI(){
		base.HideUI();
//		dragPanel.DestoryUI();
		if(dragPanel != null)
			dragPanel.DestoryUI();

		RmvCmdListener();
	}
	
	private void InitUIElement(){

		curSortRule = SortUnitTool.GetSortRule (SortRuleByUI.ApplyView);//DEFAULT_SORT_RULE;
	}

	private void CreateDragView(){
		friendOutDataList = DataCenter.Instance.FriendData.FriendOut;
		if( dragPanel != null ) {
			dragPanel.DestoryUI();
		}
		dragPanel = new DragPanel("ApplyDragPanel", FriendUnitItem.ItemPrefab,transform);
//		dragPanel.CreatUI();
		dragPanel.AddItem(friendOutDataList.Count);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitItem fuv = FriendUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Init(friendOutDataList[ i ]);
			fuv.callback = ClickItem;
		}
	}

	private void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.GetText("ApplyCounterTitle");
		int current = DataCenter.Instance.FriendData.FriendOut.Count;
		int max = 0;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		countArgs.Add("posy", -772);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private void ClickItem(FriendUnitItem item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
//		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, curPickedFriend);
		ModuleManager.Instance.ShowModule (ModuleEnum.ApplyMessageModule, "data", curPickedFriend);
	}

	private void DeleteMyApply(object msg){
		CancelFriendRequest(curPickedFriend.userId);
	}

	private void CancelFriendRequest(uint friendUid){
		FriendController.Instance.DelFriend(OnDelFriend,friendUid);
	}

	private void OnDelFriend(object data){
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
		HideUI();
		ShowUI();
	}
	
	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(curSortRule, friendOutDataList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.ApplyView);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitItem fuv = dragPanel.ScrollItem[ i ].GetComponent<FriendUnitItem>();
			fuv.UserUnit = friendOutDataList[ i ].UserUnit;
			fuv.CurrentSortRule = curSortRule;
		}
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteApply, DeleteMyApply);
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteApply, DeleteMyApply);
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

	private void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, -478, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
	}
}

