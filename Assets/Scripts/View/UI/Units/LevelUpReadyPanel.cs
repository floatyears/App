using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;
public class LevelUpReadyPanel: UIComponentUnity {
	UITexture baseTex;
	int currentMaterialPos = 1;
	List<GameObject> TabList = new List<GameObject>();
	Dictionary<int,GameObject> materialPoolDic = new Dictionary<int,GameObject>();
	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.PickBaseUnitInfo, PickBaseInfo );
		MsgCenter.Instance.AddListener(CommandEnum.PickFriendUnitInfo, PickFriendInfo);
		MsgCenter.Instance.AddListener(CommandEnum.PickMaterialUnitInfo, PickMaterialInfo);
        }

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.PickBaseUnitInfo, PickBaseInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.PickFriendUnitInfo, PickFriendInfo);
		MsgCenter.Instance.RemoveListener(CommandEnum.PickMaterialUnitInfo, PickMaterialInfo);
        }

	void PickBaseInfo( object data){
		GameObject go = data as GameObject;
		UpdateBaseInfo( go );
	}

	void PickMaterialInfo( object data ){
		GameObject go = data as GameObject;
		UpdateMaterialInfo( go );
	}

	void PickFriendInfo( object data ){
		GameObject go = data as GameObject;
		UpdateFriendInfo( go );
	}

	void UpdateBaseInfo( GameObject go){
		GameObject baseTab = TabList[0];
		UITexture tex = baseTab.GetComponentInChildren<UITexture>();
		tex.mainTexture = go.GetComponentInChildren<UITexture>().mainTexture;
	}
	
	void UpdateMaterialInfo( GameObject go){
		if( currentMaterialPos > 4 ) return;
		//Debug.LogError("current  material pos : " + currentMaterialPos);
		GameObject materialTab = materialPoolDic[ currentMaterialPos ];
		UITexture tex = materialTab.GetComponentInChildren<UITexture>();
		tex.mainTexture = go.GetComponentInChildren<UITexture>().mainTexture;
		currentMaterialPos ++;
        }
        
        void UpdateFriendInfo(GameObject go){
		GameObject friendTab = TabList[1];
		UITexture tex = friendTab.GetComponentInChildren<UITexture>();
		tex.mainTexture = go.GetComponentInChildren<UITexture>().mainTexture;
        }

	void InitUI(){
		GameObject tab;
		tab = FindChild("Tab_Base");
		baseTex = tab.GetComponentInChildren< UITexture >();
		TabList.Add(tab);
	
		tab = FindChild("Tab_Friend");
		TabList.Add(tab);

	
		tab = FindChild("Tab_Material");
		TabList.Add(tab);
		for (int i = 1; i < 5; i++){
			GameObject temp = tab.transform.FindChild("Material" + i.ToString()).gameObject;
			materialPoolDic.Add( i, temp);
                }
	
		foreach (var item in TabList){
			UIEventListener.Get(item).onClick = ClickTab;
		}
		OnTab(TabList[0]);
	}

	void ClickTab(GameObject tab){
		OnTab(tab);
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	void OnTab(GameObject focus){
		//Debug.LogError("First OnTab");
		MsgCenter.Instance.Invoke(CommandEnum.LevelUpPanelFocus, focus.name );
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
