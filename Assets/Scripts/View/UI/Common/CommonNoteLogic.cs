using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonNoteLogic : ConcreteComponent {
	
	public CommonNoteLogic(string uiName):base(uiName) {}
	
	public override void ShowUI(){
		base.ShowUI();
		AddListener();
	}

	public override void HideUI(){
		base.HideUI();
		RemoveListener();
	}

	public override void DestoryUI(){
		base.DestoryUI();
	}

	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.NoteFriendUpdate , ShowFriendUpdateNote);
	}
	
	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.NoteFriendUpdate , ShowFriendUpdateNote);
	}

	void ShowFriendUpdateNote(object msg){
		Dictionary<string,string> noteDic = new Dictionary<string, string>();
		noteDic.Add("title","Friend Update");
		noteDic.Add("content", "Are you sure to update friend list?");
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("NoteUpdateFriend", noteDic);
		ExcuteCallback(cbdArgs);
	}

}
