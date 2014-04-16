using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListView : UIComponentUnity{
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private TFriendInfo curPickedFriend;
	private UIButton sortBtn;
	private UILabel sortRuleLabel;
	private UIButton updateBtn;
	private List<TFriendInfo> friendDataList = new List<TFriendInfo>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		CreateDragView();
		SortUnitByCurRule();
		RefreshCounter();
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);     
		MsgCenter.Instance.AddListener(CommandEnum.EnsureUpdateFriend, UpdateFriendList);
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
		dragPanel.DestoryUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteFriend, DeleteFriendPicked);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureUpdateFriend, UpdateFriendList);
	}
		
	void EnableUpdateButton(object args){
		updateBtn.gameObject.SetActive(true);
		UIEventListener.Get(updateBtn.gameObject).onClick += ClickUpdateBtn;
	}

	void ClickRefuseButton(GameObject args){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefuseApplyButtonClick", null);
		ExcuteCallback(cbdArgs);
	}

	void InitUIElement(){
		sortBtn = FindChild<UIButton>("Button_Sort");
		sortRuleLabel = transform.FindChild("Button_Sort/Label_Rule").GetComponent<UILabel>();
		updateBtn = FindChild<UIButton>("Button_Update");
		UIEventListener.Get(updateBtn.gameObject).onClick = ClickUpdateBtn;
		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;
		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
	}

	void CreateDragView(){
		LogHelper.Log("FriendListView.CreateDragView(), receive call from logic, to create ui...");
		friendDataList = DataCenter.Instance.FriendList.Friend;
		dragPanel = new DragPanel("FriendDragPanel", FriendUnitView.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(friendDataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.FriendListDragPanelArgs, transform);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitView fuv = FriendUnitView.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Init(friendDataList[ i ]);
			fuv.callback = ClickItem;
		}
	}

	void ShowUIAnimation(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list){		
			if (tweenPos == null) continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	void ClickItem(FriendUnitView item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
		MsgCenter.Instance.Invoke(CommandEnum.FriendBriefInfoShow, curPickedFriend);
	}


	void ClickUpdateBtn(GameObject button){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetMsgWindowParams());
	}

	MsgWindowParams GetMsgWindowParams(){
		MsgWindowParams msgWindowParam = new MsgWindowParams();
		msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("RefreshFriend");
		msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("ConfirmRefreshFriend");
		msgWindowParam.btnParams = new BtnParam[ 2 ]{new BtnParam(), new BtnParam()};
		msgWindowParam.btnParams[ 0 ].callback = CallBackRefreshFriend;
		return msgWindowParam;
	}

	void CallBackRefreshFriend(object args){
		MsgCenter.Instance.Invoke(CommandEnum.EnsureUpdateFriend);
	}

	void UpdateFriendList(object args){
		GetFriendList.SendRequest(OnGetFriendList);
	}

	void OnGetFriendList(object data){
		if (data == null)
			return;
		bbproto.RspGetFriend rsp = data as bbproto.RspGetFriend;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.Log("RspGetFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
		DataCenter.Instance.SetFriendList(inst);
		HideUI();
		ShowUI();
	}

	void DeleteFriendPicked(object msg){
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetDeleteMsgParams());
	}

	MsgWindowParams GetDeleteMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.Instace.GetCurrentText("DeleteNoteTitle");
		msgParams.contentText = TextCenter.Instace.GetCurrentText("DeleteNoteContent");
		msgParams.btnParams = new BtnParam[2]{ new BtnParam(), new BtnParam()};
		msgParams.btnParams[ 0 ].callback = CallBackDeleteFriend;
		return msgParams;
	}

	void CallBackDeleteFriend(object args){
		DelFriend.SendRequest(OnDelFriend, curPickedFriend.UserId);
	}

	void OnDelFriend(object data){
		if (data == null)
			return;
		
		Debug.Log("TFriendList.OnDelFriend() begin");
		LogHelper.Log(data);
		bbproto.RspDelFriend rsp = data as bbproto.RspDelFriend;
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.LogError("OnRspDelFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}
		
		bbproto.FriendList inst = rsp.friends;
		LogHelper.LogError("OnRspDelFriend friends {0}", rsp.friends);
		
		DataCenter.Instance.SetFriendList(inst);
		
		HideUI();
		ShowUI();
	}

	void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.Instace.GetCurrentText("FriendCounterTitle");
		int current = DataCenter.Instance.FriendCount;
		int max = DataCenter.Instance.UserInfo.FriendMax;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	void ClickSortBtn(GameObject btn){
		curSortRule = SortUnitTool.GetNextRule(curSortRule);
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){
		sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, friendDataList);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitView fuv = dragPanel.ScrollItem[ i ].GetComponent<FriendUnitView>();
			fuv.UserUnit = friendDataList[ i ].UserUnit;
			fuv.CurrentSortRule = curSortRule;
		}
	}
}


