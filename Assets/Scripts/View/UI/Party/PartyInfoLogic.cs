using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoLogic : ConcreteComponent {

	public PartyInfoLogic(string uiName):base(uiName) {}

	public override void CreatUI(){
		base.CreatUI();
	}
	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		Refresh(DataCenter.Instance.PartyInfo.CurrentParty);
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyPanelInfo, Refresh);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPartyPanelInfo, Refresh);
	}
	
	void Refresh(object data){
		Debug.Log("Receive the message of refresh party info panel.......");
		TUnitParty tup = data as TUnitParty;
		if(tup == null){
			Debug.LogError("PartyInfoUILogic.Recive(), TUnitParty is NULL!");
			return;
		}
		UpdateData(GetData (tup));
	}

	Dictionary<string,string> GetData(TUnitParty tup){
		SkillBase sb = tup.GetLeaderSkillInfo ();
		string leaderSkillName = string.Empty;
		string leaderSkillDscp = string.Empty;
		if (sb != null) {
			leaderSkillName = sb.name;
			leaderSkillDscp = sb.description;
		}

		string hp = tup.TotalHp.ToString();

		string curCost = tup.TotalCost.ToString();

		string maxCost = DataCenter.Instance.UserInfo.CostMax.ToString();

		int value = 0;
		tup.TypeAttack.TryGetValue (EUnitType.UFIRE, out value);

		string fireAtk = value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.UWATER, out value);

		string waterAtk =  value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.UWIND, out value);

		string windAtk =  value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.UNONE, out value);
	
		string wuAtk =  value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.ULIGHT, out value);

		string lightAtk =  value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.UDARK, out value);

		string darkAtk =  value.ToString();

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
	}

	void UpdateData(Dictionary<string,string> data){
		if(data == null)	return;
		ExcuteCallback(data);
	}
	
}
