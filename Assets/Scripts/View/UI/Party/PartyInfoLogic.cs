using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyInfoLogic : ConcreteComponent {

	public PartyInfoLogic(string uiName):base(uiName) {}

	public override void CreatUI(){
//		Debug.LogError("PartyInfoLogic creat ui start  ");
		base.CreatUI();
//		Debug.LogError("PartyInfoLogic creat ui end ");
	}
	public override void ShowUI(){
//		Debug.LogError("PartyInfoLogic show ui  start ");
		base.ShowUI();
//		Debug.LogError("PartyInfoLogic show ui  end ");
		AddCmdListener();
//		Debug.LogError("AddCmdListener end ");
	}

	public override void HideUI(){
		base.HideUI();

		RmvCmdListener();
	}

	void AddCmdListener(){
//		Debug.Log("PartyInfoUILogic.AddCmdListener(), Start...");
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyPanelInfo, Recive);
//		Debug.Log("PartyInfoUILogic.AddCmdListener(), End...");
	}

	void RmvCmdListener(){
//		Debug.Log("PartyInfoUILogic.RmvCmdListener(), Start...");
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPartyPanelInfo, Recive);
//		Debug.Log("PartyInfoUILogic.RmvCmdListener(), End...");
	}


	void Recive(object data){
//		Debug.Log("PartyInfoUILogic.Recive(), Start...");

		TUnitParty tup = data as TUnitParty;
		if(tup == null){
			Debug.LogError("PartyInfoUILogic.Recive(), TUnitParty is NULL!");
			return;
		}
		Dictionary<string,string> dic = new Dictionary<string, string> ();
		UpdateData (GetData (tup));
//		Debug.Log("PartyInfoUILogic.Recive(), End...");
	}

	Dictionary<string,string> GetData(TUnitParty tup){
		SkillBase sb = tup.GetLeaderSkillInfo ();
		string leaderSkillName = "";
		string leaderSkillDscp = "";
		if (sb != null) {
			leaderSkillName = sb.name;
			leaderSkillDscp = sb.description;
		}

		string hp = tup.TotalHp.ToString();
		//Get cur cost
		string curCost = tup.TotalCost.ToString();
		//Get maxCost
		string maxCost = DataCenter.Instance.UserCost.ToString();

		int value = 0;
		tup.TypeAttack.TryGetValue (EUnitType.UFIRE, out value);
		//Get fireAtk vaule
		string fireAtk = value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.UWATER, out value);
		//Get waterAtk vaule
		string waterAtk =  value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.UWIND, out value);
		//Get waterAtk vaule
		string windAtk =  value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.UNONE, out value);
		//Get waterAtk vaule
		string wuAtk =  value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.ULIGHT, out value);
		//Get waterAtk vaule
		string lightAtk =  value.ToString();

		tup.TypeAttack.TryGetValue (EUnitType.UDARK, out value);
		//Get waterAtk vaule
		string darkAtk =  value.ToString();
		
		//Get windAtk vaule
//		string windAtk = tup.TypeAttack[ EUnitType.UWIND ].ToString();
//		//Get wuAtk vaule
//		string wuAtk = tup.TypeAttack[ EUnitType.UNONE ].ToString();
//		//Get lightAtk vaule
//		string lightAtk = tup.TypeAttack[ EUnitType.ULIGHT ].ToString();
//		//Get drakAtk vaule
//		string darkAtk = tup.TypeAttack[ EUnitType.UDARK ].ToString();

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
//		Debug.Log("PartyInfoUILogic.GetData(), End...");
	}

	void UpdateData(Dictionary<string,string> data){
//		Debug.Log("PartyInfoUILogic.UpdateData(), Start...");
		if(data == null)	return;
		ExcuteCallback(data);
//		Debug.Log("PartyInfoUILogic.UpdateData(), End...");
	}



}
