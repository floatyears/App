using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoUILogic : ConcreteComponent {

	public PartyInfoUILogic(string uiName):base(uiName) {}

	public override void ShowUI(){
		base.ShowUI();

		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();

		RmvCmdListener();
	}

	void AddCmdListener(){
		Debug.Log("PartyInfoUILogic.AddCmdListener(), Start...");
		MsgCenter.Instance.AddListener(CommandEnum.UpdatePartyInfoPanel, Recive);
		Debug.Log("PartyInfoUILogic.AddCmdListener(), End...");
	}

	void RmvCmdListener(){
		Debug.Log("PartyInfoUILogic.RmvCmdListener(), Start...");
		MsgCenter.Instance.RemoveListener(CommandEnum.UpdatePartyInfoPanel, Recive);
		Debug.Log("PartyInfoUILogic.RmvCmdListener(), End...");
	}


	void Recive(object data){
		Debug.Log("PartyInfoUILogic.Recive(), Start...");
		TUnitParty tup = data as TUnitParty;
		if(tup == null){
			Debug.LogError("PartyInfoUILogic.Recive(), TUnitParty is NULL!");
			return;
		}

		UpdateData(GetData(tup));
		Debug.Log("PartyInfoUILogic.Recive(), End...");
	}

	Dictionary<string,string> GetData(TUnitParty tup){
		Debug.Log("PartyInfoUILogic.GetData(), Start...");

		//Get Skill Name
		string leaderSkillName = tup.GetLeaderSkillInfo().name;
		//Get skill dscp
		string leaderSkillDscp = tup.GetLeaderSkillInfo().description;
		//Get total hp
		string hp = string.Empty;
		//Get cur cost
		string curCost = string.Empty;
		//Get maxCost
		string maxCost =string.Empty;
		//Get fireAtk vaule
		string fireAtk = string.Empty;
		//Get waterAtk vaule
		string waterAtk = string.Empty;
		//Get windAtk vaule
		string windAtk = string.Empty;
		//Get wuAtk vaule
		string wuAtk = string.Empty;
		//Get lightAtk vaule
		string lightAtk = string.Empty;
		//Get drakAtk vaule
		string darkAtk = string.Empty;

		Dictionary<string,string> text = new Dictionary<string, string>();
		text.Add("hp",hp);
		text.Add("fire", fireAtk);
		text.Add("water", waterAtk);
		text.Add("wind", windAtk);
		text.Add("wu", wuAtk);
		text.Add("light", lightAtk);
		text.Add("dark", darkAtk);
		text.Add("curCost", curCost);
		text.Add("maxCost", maxCost);
		text.Add("skillName", leaderSkillName);
		text.Add("skillDscp", leaderSkillDscp);

		return text;
		Debug.Log("PartyInfoUILogic.GetData(), End...");
	}

	void UpdateData(Dictionary<string,string> data){
		Debug.LogError("PartyInfoUILogic.UpdateData(), Start...");
		if(data == null)	return;
		ExcuteCallback(data);
		Debug.LogError("PartyInfoUILogic.UpdateData(), End...");
	}



}
