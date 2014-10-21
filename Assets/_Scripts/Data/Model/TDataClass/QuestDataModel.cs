using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class QuestDataModel : ProtobufDataBase {

	private List<StageInfo> eventStageList;
	public List<StageInfo> EventStageList { 
		get { return eventStageList; }
		set { eventStageList = value; } 
	}

	private QuestClearInfo questClearInfo;
	public QuestClearInfo QuestClearInfo {
		get { return questClearInfo; }
		set { questClearInfo = value; }
	}

	private List<CityInfo> cityListInfo = new List<CityInfo>();
	
	public List<CityInfo> GetCityListInfo(){
		if(cityListInfo.Count == 0){
			//			Debug.Log("DataCenter.GetCityListInfo(), CityListInfo is NULL");
			cityListInfo = DGTools.LoadCityList();
		}
		//		Debug.Log("DataCenter.GetCityListInfo(), CityListInfo count is : " + CityListInfo.Count);
		return cityListInfo;
	}

	public StageInfo GetStageInfo (uint stageID) {
		uint cityId = stageID/10;
		CityInfo cityInfo = GetCityInfo(cityId);
		for(int i=0; i < cityInfo.stages.Count; i++) {
			if (stageID==cityInfo.stages[i].id)
				return cityInfo.stages[i];
		}
		
		return null;
	}
	
	public CityInfo GetCityInfo (uint cityID) {
		if (cityInfo.ContainsKey(cityID)) {
			CityInfo tui = cityInfo[cityID];
			return tui;
		}
		else {
			CityInfo tui = DGTools.LoadCityInfo(cityID);
			if(tui == null) {
				Debug.LogError("city id : " + cityID + " is invalid");
				return null;
			}
			//CityInfo.Add(tui.ID,tui);
			cityInfo.Add(cityID,tui);
			return tui;
		}
	}

	public QuestInfo GetQuestInfo(uint questID){
		uint stageId = questID / 10;
		foreach (var item in GetStageInfo (stageId).QuestInfo) {
			if(item.id == questID){
				return item;
			}
		}
		return null;
	}
	
	private Dictionary<uint, CityInfo> cityInfo = new Dictionary<uint, CityInfo>();
}
