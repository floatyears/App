using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;
public class LevelUpReadyPanel: UIComponentUnity {
	UITexture baseTex;
	UIImageButton levelUpButton;
	int currentMaterialPos = 1;
	List<GameObject> TabList = new List<GameObject>();
	Dictionary<int,GameObject> materialPoolDic = new Dictionary<int,GameObject>();


	UserUnit baseUnitInfo;
	UserUnit friendUnitInfo;
	List<UserUnit> materialUnitInfo = new List<UserUnit>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		InitUI();
	}
	
	Dictionary< GameObject, bool> readySignDic = new Dictionary<GameObject, bool>() ;

	public override void ShowUI(){
		base.ShowUI();
		//InitReadySign();
		AddListener();
        }

	public override void HideUI(){
		base.HideUI();
		RemoveListener();
        }
	
	void InitReadySign(){
		foreach (var item in TabList){
			readySignDic.Add(item,false);
		}
	}

	void UpdateReadySign(GameObject tab,bool isSign){
			readySignDic[ tab ] = isSign;
	}
	
	void UpdateBaseInfoView( UserUnit unitInfo){
		GameObject baseTab = TabList[0];
		UITexture tex = baseTab.GetComponentInChildren<UITexture>();
		uint curUnitId = unitInfo.unitId;
		tex.mainTexture = GlobalData.tempUnitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);
	}
	
	void UpdateMaterialInfoView( UserUnit unitInfo){
		GameObject materialTab = materialPoolDic[ materialUnitInfo.Count ];
		UITexture tex = materialTab.GetComponentInChildren<UITexture>();
		uint curUnitId = unitInfo.unitId;
		tex.mainTexture = GlobalData.tempUnitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);
        }
        
	void UpdateFriendInfo(UserUnit unitInfo){
//		Debug.LogError("Friend Item Show");
		GameObject friendTab = TabList[1];
		UITexture tex = friendTab.GetComponentInChildren<UITexture>();
		uint curUnitId = unitInfo.unitId;
		tex.mainTexture = GlobalData.tempUnitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);
        }

	void InitUI(){
		InitTab();
		InitButton();
	}
	
	void InitTab(){
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
	
	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.PickBaseUnitInfo, PickBaseUnitInfo );
		MsgCenter.Instance.AddListener(CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo );
		MsgCenter.Instance.AddListener(CommandEnum.PickMaterialUnitInfo, PickMaterialUnitInfo );
		MsgCenter.Instance.AddListener(CommandEnum.TryEnableLevelUp, EnableLevelUp);
		//MsgCenter.Instance.AddListener(CommandEnum.SendLevelUpInfo, SendLevelUpInfo);
	}
	
	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.PickBaseUnitInfo, PickBaseUnitInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.PickMaterialUnitInfo, PickMaterialUnitInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.TryEnableLevelUp, EnableLevelUp);
		//MsgCenter.Instance.RemoveListener(CommandEnum.SendLevelUpInfo, SendLevelUpInfo);
	}


	void EnableLevelUp(object info){
		Dictionary<string, object> levelUpInfo = PackLevelUpInfo();
		if( levelUpInfo == null)	
			levelUpButton.isEnabled = false;
		else
			levelUpButton.isEnabled = true;
	}
	
	void InitButton(){
		levelUpButton = FindChild<UIImageButton>("Button_LevelUp");
		UIEventListener.Get( levelUpButton.gameObject ).onClick = ClickLevelUpButton;
		levelUpButton.isEnabled = false;
	}

	void ClickLevelUpButton(GameObject go){
		Debug.LogError("Ready Pool : Click LevelUp Button");
		UIManager.Instance.ChangeScene( SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke( CommandEnum.LevelUp, PackUserUnitInfo() );
//		Debug.LogError("Click LeveUp Button PackUserCount : " + PackUserUnitInfo().Count);

	}


	
	/// <summary>
	/// Packs the user unit info. 
	/// </summary>
	List<UserUnit> PackUserUnitInfo(){
		List<UserUnit> pickedUserUnitInfo = new List<UserUnit>();
		pickedUserUnitInfo.Add(baseUnitInfo);
		pickedUserUnitInfo.Add(friendUnitInfo);
//		Debug.LogError("Pack : Material Count " + materialUnitInfo.Count); 
		foreach (var item in materialUnitInfo){
			pickedUserUnitInfo.Add(item);
		}
//		Debug.LogError("Pck: Package Count " + pickedUserUnitInfo.Count );
		return pickedUserUnitInfo;
	}

	void PickBaseUnitInfo(object info){
		Debug.Log("Ready Pool Receive info !");
		if(baseUnitInfo != null)	return;
		baseUnitInfo = info as UserUnit;
		UpdateBaseInfoView( baseUnitInfo );

	}

	void CheckLevelUpInfo( object info){
		Debug.LogError("Check LevelUp Info");
		//MsgCenter.Instance.Invoke(CommandEnum.EnableLevelUp, );
	}

	void PickFriendUnitInfo(object info){
		if(friendUnitInfo != null)	return;
		friendUnitInfo = info as UserUnit;

		UpdateFriendInfo(friendUnitInfo);
	}
	void PickMaterialUnitInfo(object info){
		if( materialUnitInfo.Count == 4)	return;
		UserUnit tempInfo = info as UserUnit;
		materialUnitInfo.Add(tempInfo);
		UpdateMaterialInfoView( tempInfo );

	}

	Dictionary<string, object> PackLevelUpInfo(){
		if(baseUnitInfo == null)	return null;
		if(friendUnitInfo == null)	return null;
		foreach (var item in materialUnitInfo)	
			if(item == null)		return null;

		Dictionary<string, object> levelUpInfo = new Dictionary<string, object>();
		levelUpInfo.Add("BaseInfo", baseUnitInfo);
		levelUpInfo.Add("FriendInfo", friendUnitInfo);
		levelUpInfo.Add("MaterialInfo",materialUnitInfo);

		return levelUpInfo;
	}

	void SendLevelUpInfo(object info){
		bool b = (bool)info;
		if(!b)	return;
		Dictionary<string, object> levelUpInfo = PackLevelUpInfo();
		if(levelUpInfo == null)		return;
		UIManager.Instance.ChangeScene( SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke( CommandEnum.LevelUp, levelUpInfo);
	}

}
