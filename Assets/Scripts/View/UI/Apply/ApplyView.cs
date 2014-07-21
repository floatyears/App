using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplyView : UIComponentUnity{
	private SortRule curSortRule;
	private TFriendInfo curPickedFriend;
	private DragPanel dragPanel;
	private List<TFriendInfo> friendOutDataList = new List<TFriendInfo>();
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
		ShowUIAnimation();
	}
	
	public override void HideUI(){
		base.HideUI();
		dragPanel.DestoryUI();
		RmvCmdListener();
	}
	
	private void InitUIElement(){
		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
	}

	private void CreateDragView(){
		friendOutDataList = DataCenter.Instance.FriendList.FriendOut;
		dragPanel = new DragPanel("ApplyDragPanel", FriendUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(friendOutDataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.FriendListDragPanelArgs, transform);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitItem fuv = FriendUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Init(friendOutDataList[ i ]);
			fuv.callback = ClickItem;
		}
	}

	private void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.GetText("ApplyCounterTitle");
		int current = DataCenter.Instance.FriendList.FriendOut.Count;
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
		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, curPickedFriend);
	}

	private void DeleteMyApply(object msg){
		CancelFriendRequest(curPickedFriend.UserId);
	}

	private void CancelFriendRequest(uint friendUid){
		DelFriend.SendRequest(OnDelFriend, friendUid);
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
		DataCenter.Instance.SetFriendList(inst);
		HideUI();
		ShowUI();
	}
	
	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(curSortRule, friendOutDataList);
		
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

