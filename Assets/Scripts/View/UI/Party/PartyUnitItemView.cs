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

public class PartyUnitItemView{
	private bool isEnable;
	public bool IsEnabel{
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
        
	private TUserUnit dataItem;
	private PartyUnitItemView(){}

	public static PartyUnitItemView Create(TUserUnit dataItem){
		PartyUnitItemView partyUnitItemView = new PartyUnitItemView();
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
				default:
					break;
			}
                }
        }
        
        private void initWithTUserUnit(TUserUnit dataItem){
		InitDataItem(dataItem);
 		InitWithArgs();
                InitAvatar();
        }
       
	private void InitDataItem(TUserUnit dataItem){
		this.dataItem = dataItem;
	}

	private void InitWithArgs(){
		Dictionary <string, object> initArgs = new Dictionary<string, object>();
		initArgs.Add("collect", true);
		initArgs.Add("enable", true);
		initArgs.Add("party", false);
                
                RefreshStates(initArgs);
	}
	
        private void ClickViewItem(GameObject item){
		if(!isEnable)	return;
	}
	
	private void PressViewItem(GameObject item){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, dataItem);
        }
   
        private void RefreshPartyState(bool state){
                UILabel label = viewItem.transform.FindChild("Label_Party").GetComponent<UILabel>();
		label.enabled = state;
        }

	private void RefreshMarkState(bool state){
		this.isCollected = state;
		UISprite starMarkSpr = viewItem.transform.FindChild("StarMark").GetComponent<UISprite>();
		starMarkSpr.enabled = state;
        }
		
	private void RefreshEnableState(bool state){
		if(state)	
			AddEventListener();
		else
			RemoveEventListner();
		UISprite maskSpr = viewItem.transform.FindChild("Mask").GetComponent<UISprite>();
                maskSpr.enabled = state;
        }

	private void AddEventListener(){
		UIEventListenerCustom.Get(viewItem).onClick += ClickViewItem;
		UIEventListenerCustom.Get(viewItem).LongPress += PressViewItem;
        }

	private void RemoveEventListner(){
		UIEventListenerCustom.Get(viewItem).onClick -= ClickViewItem;
		UIEventListenerCustom.Get(viewItem).LongPress -= PressViewItem;
	}
        
}

