using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonNoteLogic : ConcreteComponent
{
	public CommonNoteLogic(string uiName):base(uiName){}

	public override void ShowUI()
	{
		base.ShowUI();
		AddListener();
	}

	public override void HideUI()
	{
		base.HideUI();
		RemoveListener();
	}

	public override void DestoryUI()
	{
		base.DestoryUI();
	}

	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.NoteInformation, ShowNoteInformation);
	}


	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.NoteInformation, ShowNoteInformation);
	}

	void ShowEmptyIDInputError(object msg)
	{
		Dictionary<string,string> noteDic = new Dictionary<string, string>();
		noteDic.Add("title", "ID Input Empty Error");
		noteDic.Add("content", "Could not Input empty ID!");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("NoteIDInputError", noteDic);
		ExcuteCallback(cbdArgs);
	}
	void ShowFriendUpdateNote(object msg)
	{
		Dictionary<string,string> noteDic = new Dictionary<string, string>();
		noteDic.Add("title", "Friend Update");
		noteDic.Add("content", "Are you sure to update friend list?");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("NoteUpdateFriend", noteDic);
		ExcuteCallback(cbdArgs);
	}

	void ShowRefuseApplyNote(object msg)
	{
		Dictionary<string,string> noteDic = new Dictionary<string, string>();
		noteDic.Add("title", "Refuse Apply");
		noteDic.Add("content", "Are you sure to refuse all friend apply?");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("NoteRefuseApply", noteDic);
		ExcuteCallback(cbdArgs);
	}

	public override void Callback(object data)
	{
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName)
		{
			case "ClickSure":
				CallBackDispatcherHelper.DispatchCallBack(SendMessage, cbdArgs);
				break;
			default:
				break;
		}
	}

	void SendMessage(object args)
	{
		if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.FriendList)
			MsgCenter.Instance.Invoke(CommandEnum.EnsureUpdateFriend, null);
		if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.Reception)
			MsgCenter.Instance.Invoke(CommandEnum.EnsureRefuseAll, null);

	}

	void ShowNoteInformation(object msg){
		Dictionary<string,string> noteMsg = msg as Dictionary<string,string>;
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ShowNote", noteMsg);
		ExcuteCallback(cbdArgs);
	}


}
