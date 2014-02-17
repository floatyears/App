using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelUpReadyPanel: UIComponentUnity {

	List<GameObject> TabList = new List<GameObject>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);

		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	void InitUI(){
		GameObject tab;
		tab = FindChild("Tab_Base");
		TabList.Add(tab);
	
		tab = FindChild("Tab_Friend");
		TabList.Add(tab);
	
		tab = FindChild("Tab_Material");
		TabList.Add(tab);
	
		foreach (var item in TabList){
			UIEventListener.Get(item).onClick = ClickTab;
		}
		OnTab(TabList[0]);
	}

	void ClickTab(GameObject tab){
		MsgCenter.Instance.Invoke(CommandEnum.LevelUpPanelFocus, tab.name );
		OnTab(tab);
	}

	void OnTab(GameObject focus){
		foreach (var tab in TabList) {
			if (tab == focus) {
				tab.transform.FindChild("Light_Frame").gameObject.SetActive(true);
				tab.transform.FindChild("Label_Title").GetComponent< UILabel >().color = Color.yellow;
			} else{
				tab.transform.FindChild("Light_Frame").gameObject.SetActive(false);
				tab.transform.FindChild("Label_Title").GetComponent< UILabel >().color = Color.white;
			}
		}
	}
        

}
