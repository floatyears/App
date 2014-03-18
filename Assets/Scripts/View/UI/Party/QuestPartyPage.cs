using UnityEngine;
using System.Collections;

public class QuestPartyPage : PartyPageLogic{
	public QuestPartyPage(string uiName) : base(uiName){}
	
	public override void CreatUI(){
		base.CreatUI();
		EnableFriendDisplay();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCommandListener();
	}

	public override void HideUI(){
		base.HideUI();
		RemoveCommandListener();
	}
	
	private void EnableFriendDisplay(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableItemLeft", null);
		ExcuteCallback(cbdArgs);
	}

	void AddHelperItem(object msg){
		TFriendInfo tfi = msg as TFriendInfo;
		Texture2D tex = tfi.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowItemLeft", tex);
		ExcuteCallback(cbdArgs);
	}

	void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.AddHelperItem, AddHelperItem);
	}

	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.AddHelperItem, AddHelperItem);
	}
}
