using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;
public class LevelUpReadyPanel: UIComponentUnity {
	UILabel hpLabel;
	UILabel atkLabel;
	UILabel expNeedLabel;
	UILabel expCurGotLabel;
	UILabel cionNeedLabel;

	UIImageButton levelUpButton;
	List<GameObject> TabList = new List<GameObject>();
	Dictionary<int,GameObject> materialPoolDic = new Dictionary<int,GameObject>();

	UITexture baseCollectorTex;
	UITexture friendCollectorTex;
	List<UITexture> materialCollectorTex = new List<UITexture>();
	
	UserUnit baseUnitInfo;
	UserUnit friendUnitInfo;
	List<UserUnit> materialUnitInfo = new List<UserUnit>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		InitUI();
		base.Init(config, origin);
	}

	public override void ShowUI(){

		base.ShowUI();
		OnTab(TabList[0]);
		AddListener();
		levelUpButton.isEnabled = false;
		ClearTexture();
		ClearLabel();
		ClearData();
        }

	public override void HideUI(){
		base.HideUI();
		RemoveListener();
        }

	void InitUI(){
		InitTab();
		InitButton();
		FindInfoPanelLabel();
		FindCollectorTexture();
	}



	void UpdateBaseInfoView( UserUnit unitInfo){
		GameObject baseTab = TabList[0];
		uint curUnitId = unitInfo.unitId;
		TUnitInfo tu = GlobalData.unitInfo[ curUnitId ];

		UITexture tex = baseTab.GetComponentInChildren<UITexture>();
		tex.mainTexture = tu.GetAsset(UnitAssetType.Avatar);

		int hp = GlobalData.Instance.GetUnitValue(tu.HPType,unitInfo.level);
		hpLabel.text = hp.ToString();

		int atk =  GlobalData.Instance.GetUnitValue(tu.Object.AttackType, unitInfo.level);
		atkLabel.text = atk.ToString();

		expNeedLabel.text = "16918";

		expCurGotLabel.text = "0";

		cionNeedLabel.text = "0";
	}

	void ClearLabel(){
		hpLabel.text = UIConfig.emptyLabelTextFormat;
		atkLabel.text = UIConfig.emptyLabelTextFormat;
		expNeedLabel.text = UIConfig.emptyLabelTextFormat;
		expCurGotLabel.text = UIConfig.emptyLabelTextFormat;
		cionNeedLabel.text = UIConfig.emptyLabelTextFormat;
	}

	void UpdateMaterialInfoView( UserUnit unitInfo){
		GameObject materialTab = materialPoolDic[ materialUnitInfo.Count ];
		UITexture tex = materialTab.GetComponentInChildren<UITexture>();
		uint curUnitId = unitInfo.unitId;
		tex.mainTexture = GlobalData.unitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);
        }
        
	void UpdateFriendInfo(UserUnit unitInfo){
		GameObject friendTab = TabList[1];
		UITexture tex = friendTab.GetComponentInChildren<UITexture>();
		uint curUnitId = unitInfo.unitId;
		tex.mainTexture = GlobalData.unitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);
        }

	void FindInfoPanelLabel(){
		hpLabel = FindChild< UILabel >("InfoPanel/Label_Vaule/0");
		atkLabel = FindChild< UILabel >("InfoPanel/Label_Vaule/1");
		expNeedLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/2");
		expCurGotLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/3");
		cionNeedLabel = FindChild< UILabel >( "InfoPanel/Label_Vaule/4");
	}
	
	void FindCollectorTexture(){
		baseCollectorTex = FindChild<UITexture>("Tab_Base/role");
		friendCollectorTex = FindChild<UITexture>("Tab_Friend/role");

		for( int i = 1; i <= 4; i++ ){
			string path = string.Format( "Tab_Material/Material{0}/Avatar", i );
			//Debug.Log("Ready Panel,FindAvatarTexture, Path is " + path);
			UITexture tex = FindChild< UITexture >( path );
			materialCollectorTex.Add( tex );
		}
	}

	void ClearTexture(){
		baseCollectorTex.mainTexture = null;
		friendCollectorTex.mainTexture = null;
		foreach (var item in materialCollectorTex)
			item.mainTexture = null;
	}

	void ClearData(){
		baseUnitInfo = null;
		friendUnitInfo = null;
		materialUnitInfo.Clear();
	}
	
	void InitTab(){
		GameObject tab;
		tab = FindChild("Tab_Base");
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

	}
	
	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.PickBaseUnitInfo, PickBaseUnitInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.PickFriendUnitInfo, PickFriendUnitInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.PickMaterialUnitInfo, PickMaterialUnitInfo );
		MsgCenter.Instance.RemoveListener(CommandEnum.TryEnableLevelUp, EnableLevelUp);
	}
	
	void EnableLevelUp(object info){
		Dictionary<string, object> levelUpInfo = PackLevelUpInfo();
		if( levelUpInfo == null){	
			levelUpButton.isEnabled = false;
		}
		else{
			levelUpButton.isEnabled = true;
		}
	}
	
	void InitButton(){
		levelUpButton = FindChild<UIImageButton>("Button_LevelUp");
		UIEventListener.Get( levelUpButton.gameObject ).onClick = ClickLevelUpButton;
		levelUpButton.isEnabled = false;
	}

	void ClickLevelUpButton(GameObject go){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);//before
		MsgCenter.Instance.Invoke(CommandEnum.LevelUp, PackUserUnitInfo());//after
	}
	
	List<UserUnit> PackUserUnitInfo(){
		List<UserUnit> pickedUserUnitInfo = new List<UserUnit>();
		pickedUserUnitInfo.Add(baseUnitInfo);
		pickedUserUnitInfo.Add(friendUnitInfo);
		foreach (var item in materialUnitInfo){
			pickedUserUnitInfo.Add(item);
		}
		return pickedUserUnitInfo;
	}

	void PickBaseUnitInfo(object info){
//		Debug.Log("Ready Pool Receive info !");
		if(baseUnitInfo != null)	return;
		baseUnitInfo = info as UserUnit;
		UpdateBaseInfoView( baseUnitInfo );
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
		//condition : exist base && material && friend
		if(baseUnitInfo == null)		
			return null;
		if(friendUnitInfo == null)	
			return null;
		if( materialUnitInfo.Count < 1)
			return null;
		Dictionary<string, object> levelUpInfo = new Dictionary<string, object>();
		levelUpInfo.Add("BaseInfo", baseUnitInfo);
		levelUpInfo.Add("FriendInfo", friendUnitInfo);
		levelUpInfo.Add("MaterialInfo",materialUnitInfo);

		return levelUpInfo;
	}

}


