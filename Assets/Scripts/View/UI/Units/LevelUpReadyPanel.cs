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

	GameObject baseTab;
	GameObject friendTab;
	GameObject materialTab;

	UIImageButton levelUpButton;
	List<GameObject> Tabs = new List<GameObject>();
	Dictionary<int,GameObject> materialPoolDic = new Dictionary<int,GameObject>();

	UITexture baseCollectorTex;
	UITexture friendCollectorTex;
	List<UITexture> materialCollectorTex = new List<UITexture>();
	
	TUserUnit baseUnitInfo;
	TUserUnit friendUnitInfo;
	List<TUserUnit> materialUnitInfo = new List<TUserUnit>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		InitUI();
		base.Init(config, origin);
	}

	public override void ShowUI(){

		base.ShowUI();
		FoucsOnTab( Tabs[0] );
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


	void UpdateBaseInfoView( TUserUnit unitInfo){
		GameObject baseTab = Tabs[0];
			
		TUnitInfo tu = GlobalData.unitInfo[ unitInfo.ID ];

		UITexture tex = baseTab.GetComponentInChildren<UITexture>();
		tex.mainTexture = tu.GetAsset(UnitAssetType.Avatar);

		int hp = GlobalData.Instance.GetUnitValue(tu.HPType,unitInfo.Level);
		hpLabel.text = hp.ToString();

		int atk =  GlobalData.Instance.GetUnitValue(tu.AttackType, unitInfo.Level);
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

	void UpdateMaterialInfoView( TUserUnit unitInfo){
		GameObject materialTab = materialPoolDic[ materialUnitInfo.Count ];
		UITexture tex = materialTab.GetComponentInChildren<UITexture>();
		tex.mainTexture = GlobalData.unitInfo[ unitInfo.ID ].GetAsset(UnitAssetType.Avatar);
        }
        
	void UpdateFriendInfo(TUserUnit unitInfo){
		GameObject friendTab = Tabs[1];
		UITexture tex = friendTab.GetComponentInChildren<UITexture>();
		tex.mainTexture = GlobalData.unitInfo[ unitInfo.ID ].GetAsset(UnitAssetType.Avatar);
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
		Tabs.Add(tab);
		
		tab = FindChild("Tab_Friend");
		Tabs.Add(tab);

		tab = FindChild("Tab_Material");
		Tabs.Add(tab);

		for (int i = 1; i < 5; i++){
			GameObject item = tab.transform.FindChild("Material" + i.ToString()).gameObject;
                        materialPoolDic.Add( i, item);
                }
                
                foreach (var item in Tabs)
                        UIEventListener.Get(item).onClick = ClickTab;
        }


        void ClickTab(GameObject tab){
		FoucsOnTab(tab);
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	void FoucsOnTab(GameObject focus){
		//disable all tab
		foreach (var tab in Tabs){
			tab.transform.FindChild("Light_Frame").gameObject.SetActive(false);
			tab.transform.FindChild("Label_Title").GetComponent< UILabel >().color = Color.white;
		}

		//activate focus tab
		focus.transform.FindChild("Light_Frame").gameObject.SetActive(true);
		focus.transform.FindChild("Label_Title").GetComponent< UILabel >().color = Color.yellow;
		//activate focus tab content
		MsgCenter.Instance.Invoke(CommandEnum.PanelFocus, focus.name );
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
	
	List<TUserUnit> PackUserUnitInfo(){
		List<TUserUnit> pickedUserUnitInfo = new List<TUserUnit>();
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
		baseUnitInfo = info as TUserUnit;
		UpdateBaseInfoView( baseUnitInfo );
	}


	void PickFriendUnitInfo(object info){
		if(friendUnitInfo != null)	return;
		friendUnitInfo = info as TUserUnit;

		UpdateFriendInfo(friendUnitInfo);
	}
	void PickMaterialUnitInfo(object info){
		if( materialUnitInfo.Count == 4)	return;
		TUserUnit tempInfo = info as TUserUnit;
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


