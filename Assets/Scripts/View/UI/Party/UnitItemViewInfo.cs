using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitItemInfo {
	public GameObject scrollItem;
	public UILabel stateLabel;
	public UIEventListenerCustom listener;
	public TUserUnit userUnitItem;
	public bool isCollect 	= false;
	public bool isPartyItem = false;
        public bool isSelect = false;
	public bool isEnable = false;
}

public class UnitItemViewInfo{
	private bool isEnable;
	public bool IsEnable{
		get{ return isEnable;}
	}

	private bool isParty;
	public bool IsParty{
		get{ return isParty; }
	}

	private bool isCollected;
	public bool IsCollected{
		get{ return isCollected;}
	}
        
	private string crossShowTextBefore;
	public string CrossShowTextBefore{
		get{
			return crossShowTextBefore;
		}
		set{
			crossShowTextBefore = value;
		}
	}

	private string crossShowTextAfter;
	public string CrossShowTextAfter{
		get{
			return crossShowTextAfter;
		}
		set{
			crossShowTextAfter = value;
		}
	}

	private Texture2D avatar;
	public Texture2D Avatar{
		get{
			return avatar;
		}
	}

	private TUserUnit dataItem;
	public TUserUnit DataItem{
		get{
			return dataItem;
		}
	}
	private UnitItemViewInfo(){}

	public static UnitItemViewInfo Create(TUserUnit dataItem){
		UnitItemViewInfo partyUnitItemView = new UnitItemViewInfo();
		partyUnitItemView.initWithTUserUnit(dataItem);
		return partyUnitItemView;
	}

	public void RefreshStates(Dictionary <string, object> statesDic){
		foreach (var key in statesDic.Keys){
			switch (key) {
				case "collect":
					RefreshMarkState((bool)statesDic[key]);
					break;
				case "enable":
					RefreshEnableState((bool)statesDic[key]);
					break;
				case "party":
					RefreshPartyState((bool)statesDic[key]);
                                	break;
				case "cross" :
					RefreshCrossTextState(statesDic[key] as List<string>);
					break;
				default:
					break;
			}
                }
        }
	private void RefreshCrossTextState(List<string> textList){
		this.crossShowTextBefore = textList[ 0 ];
		this.crossShowTextAfter = textList[ 1 ];
	}

	private void initWithTUserUnit(TUserUnit dataItem){
		InitDataItem(dataItem);
 		InitWithArgs();
        	GetAvatar();
	}
       
	private void InitDataItem(TUserUnit dataItem){
		this.dataItem = dataItem;
	}

	private void InitCrossShowText(){
		crossShowTextBefore = dataItem.Level.ToString();
		crossShowTextAfter = (dataItem.AddHP + dataItem.AddAttack).ToString();
	}

	private void InitWithArgs(){
		Dictionary <string, object> initArgs = new Dictionary<string, object>();
		initArgs.Add("collect", true);
		initArgs.Add("enable", false);
		initArgs.Add("party", GlobalData.partyInfo.UnitIsInCurrentParty(dataItem.ID));

		List<string> textList = new List<string>();
		textList.Add(dataItem.Level.ToString());
		textList.Add((dataItem.AddHP + dataItem.AddAttack).ToString());
		initArgs.Add("cross", textList);
        	RefreshStates(initArgs);
	}
	
	private void GetAvatar(){
		avatar = dataItem.UnitInfo.GetAsset(UnitAssetType.Avatar);
	}
	
        private void RefreshPartyState(bool state){
		this.isParty = state;
        }

	private void RefreshMarkState(bool state){
		this.isCollected = state;
        }
		
	private void RefreshEnableState(bool state){
		this.isEnable = state;
        }

}

