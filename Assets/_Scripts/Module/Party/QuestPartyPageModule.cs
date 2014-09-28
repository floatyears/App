using UnityEngine;
using System.Collections;
using bbproto;

public class QuestPartyPageModule : PartyPageModule{
	private UnitDataModel evolveStart;

	public QuestPartyPageModule(UIConfigItem config) : base(  config){
//		CreateUI<questp
	}
	
	public override void InitUI(){
		base.InitUI();
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
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableItemLeft", null);
//		view.CallbackView(cbdArgs);
	}

	void AddHelperItem(object msg){
		FriendInfo tfi = msg as FriendInfo;
		Refresh (tfi);
	}

	void Refresh (FriendInfo tfi) {
		ResourceManager.Instance.GetAvatar(UnitAssetType.Avatar, tfi.UserUnit.unitId, o=>{
//			CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowItemLeft", o as Texture2D);
//			tfi.UserUnit.UnitInfo
			ModuleManager.SendMessage(ModuleEnum.FriendSelectModule,"refresh");
//			view.CallbackView(cbdArgs);
		});

	}
	
	void EvolveSelectQuest(object data) {
		evolveStart = data as UnitDataModel;
		bbproto.UnitParty up = new bbproto.UnitParty ();
		up.id = 10000;
		bbproto.PartyItem pi = new bbproto.PartyItem ();
		pi.unitPos = 0;
//		pi.unitUniqueId = evolveStart.EvolveStart.BaseUnitId;
//		up.items.Add (pi);
//		for (int i = 0; i < evolveStart.EvolveStart.PartUnitId.Count; i++) {
//			pi = new bbproto.PartyItem ();
//			pi.unitPos = i + 1;
//			pi.unitUniqueId = evolveStart.EvolveStart.PartUnitId[i];
//			up.items.Add (pi);
//		}
//		TUnitParty tup = new TUnitParty (up);
//		RefreshEvolvePartyInfo (tup);
//		uint friendID = evolveStart.EvolveStart.HelperUserId;
//		TFriendInfo tfi = DataCenter.Instance.SupportFriends.Find (a => a.UserId == friendID);
//		Refresh (tfi);
	}

	void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.AddHelperItem, AddHelperItem);
		MsgCenter.Instance.AddListener(CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
	}

	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.AddHelperItem, AddHelperItem);
		MsgCenter.Instance.AddListener(CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
	}
}
