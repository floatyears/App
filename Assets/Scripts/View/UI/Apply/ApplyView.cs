using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplyView : UIComponentUnity{
	TFriendInfo curPickedFriend;
	UIButton sortButton;
	DragPanel dragPanel;
	List<TFriendInfo> friendInDataList = new List<TFriendInfo>();
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		CreateDragView();
		RefreshCounter();
		MsgCenter.Instance.AddListener(CommandEnum.EnsureDeleteApply, DeleteMyApply);
	}
	
	public override void HideUI(){
		base.HideUI();
		dragPanel.DestoryUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureDeleteApply, DeleteMyApply);
	}
	
	private void InitUIElement(){
		sortButton = FindChild<UIButton>("Button_Sort");
//		UIEventListener.Get(sortButton.gameObject).onClick = ClickRefuseBtn;
	}

	private void CreateDragView(){
		friendInDataList = DataCenter.Instance.FriendList.FriendOut;
		dragPanel = new DragPanel("ReceptionDragPanel", FriendUnitView.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(friendInDataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.FriendListDragPanelArgs, transform);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			FriendUnitView fuv = FriendUnitView.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Init(friendInDataList[ i ]);
			fuv.callback = ClickItem;
		}
	}

	void RefreshCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		string title = TextCenter.Instace.GetCurrentText("ReceptionCounterTitle");
		int current = DataCenter.Instance.FriendList.FriendOut.Count;
		int max = 0;
		countArgs.Add("title", title);
		countArgs.Add("current", current);
		countArgs.Add("max", max);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	void ClickItem(FriendUnitView item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		curPickedFriend = item.FriendInfo;
		MsgCenter.Instance.Invoke(CommandEnum.ViewApplyInfo, curPickedFriend);
	}

	void DeleteMyApply(object msg){
		CancelFriendRequest(curPickedFriend.UserId);
	}

	void CancelFriendRequest(uint friendUid){
		DelFriend.SendRequest(OnDelFriend, friendUid);
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

}

