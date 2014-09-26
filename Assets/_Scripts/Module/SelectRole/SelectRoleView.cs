using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class SelectRoleView : ViewBase {
	UIButton selectBtn;
	List<GameObject> tabList = new List<GameObject>();
	List<GameObject> contentList = new List<GameObject>();

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();

		//NoviceGuideStepEntityManager.Instance ().StartStep ();
		Umeng.GA.StartLevel ("SelectRole");
		GameDataAnalysis.Event(GameDataAnalysisEventType.SelectRole, new Dictionary<string,string>(){{"DeviceInfo",SystemInfo.deviceUniqueIdentifier},{"Stage","Start"}});
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void CallbackView(params object[] args){
//		base.CallbackView(data);
//		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (args[0].ToString()){
			case "ShowInitialView" : 
				ShowInitialView(args[1]);
				break;
			default:
				break;
		}
	}

	void InitUI(){
		FindChild<UILabel>("Button_Select/Label").text = TextCenter.GetText ("SelectRoleBtn");
		FindChild<UILabel>("Label_Note").text = TextCenter.GetText ("SelectRoleContent");

		FindChild<UILabel> ("Text/Label_No").text = TextCenter.GetText ("SelectRole_No");
		FindChild<UILabel> ("Text/Label_ATK").text = TextCenter.GetText ("SelectRole_ATK");
		FindChild<UILabel> ("Text/Label_Fav").text = TextCenter.GetText ("SelectRole_Fav");
		FindChild<UILabel> ("Text/Label_HP").text = TextCenter.GetText ("SelectRole_HP");
		FindChild<UILabel> ("Text/Label_LV").text = TextCenter.GetText ("SelectRole_LV");
		FindChild<UILabel> ("Text/Label_Name").text = TextCenter.GetText ("SelectRole_Name");
		FindChild<UILabel> ("Text/Label_Race").text = TextCenter.GetText ("SelectRole_Race");
		FindChild<UILabel> ("Text/Label_Type").text = TextCenter.GetText ("SelectRole_Type");

		int itemCount = 3;
		GameObject item;
		for (int i = 0; i < itemCount; i++){
			item = transform.FindChild("Tab_" + i.ToString()).gameObject;
			tabList.Add(item);
			item = transform.FindChild("Content_" + i.ToString()).gameObject;
			contentList.Add(item);
		}
//		Debug.Log("UnitSelect.FindItem......Tab Item count is : " + tabList.Count);
//		Debug.Log("UnitSelect.FindItem......Content Item count is : " + contentList.Count);

		selectBtn = transform.FindChild("Button_Select").GetComponent<UIButton>();
		UIEventListenerCustom.Get(selectBtn.gameObject).onClick = ClickButton;
	}

	void ShowInitialView(object args){
		List<UnitInfo> unitInfoList = args as List<UnitInfo>;

		int initialLevel = 1;
		UITexture texture;
		UILabel label;

		//Tab
		for (int i = 0; i < tabList.Count; i++){
			label = tabList[ i ].transform.FindChild("Label_Name").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].name;

			UIEventListenerCustom.Get(tabList[ i ]).onClick = ClickTab;
		}



		ResourceManager.Instance.GetAvatar(UnitAssetType.Profile, unitInfoList[ 0 ].id, o => {
//			
			UITexture texture1 = contentList[ 0 ].transform.FindChild("Texture_Role").GetComponent<UITexture>();

			Texture2D source = o as Texture2D;
			texture1.mainTexture = source;
			texture1.width = source.width;
			texture1.height = source.height;
		});


		ResourceManager.Instance.GetAvatar(UnitAssetType.Profile,unitInfoList[ 1 ].id, o => {

			UITexture texture1 = contentList[ 1 ].transform.FindChild("Texture_Role").GetComponent<UITexture>();

			Texture2D source = o as Texture2D;
			texture1.mainTexture = source;
			texture1.width = source.width;
			texture1.height = source.height;
		});


		ResourceManager.Instance.GetAvatar(UnitAssetType.Profile,unitInfoList[ 2 ].id ,o => {
			UITexture texture1 = contentList[ 2 ].transform.FindChild("Texture_Role").GetComponent<UITexture>();
			Texture2D source = o as Texture2D;
			texture1.mainTexture = source;
			texture1.width = source.width;
			texture1.height = source.height;
		});

		//Content
		for (int i = 0; i < contentList.Count; i++){



			label = contentList[ i ].transform.FindChild("Label_No").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].id.ToString();

			label = contentList[ i ].transform.FindChild("Label_Name").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].name;

			label = contentList[ i ].transform.FindChild("Label_LV").GetComponent<UILabel>();
			label.text = initialLevel.ToString();

			label = contentList[ i ].transform.FindChild("Label_ATK").GetComponent<UILabel>();

			int atkValue = unitInfoList[ i ].Attack;
			label.text = atkValue.ToString();

			label = contentList[ i ].transform.FindChild("Label_HP").GetComponent<UILabel>();
			int hpValue = unitInfoList[ i ].Hp;
			label.text = hpValue.ToString();

			label = contentList[ i ].transform.FindChild("Label_Race").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].UnitRace;

			label = contentList[ i ].transform.FindChild("Label_Type").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].UnitTypeText;
		}
	}

	void ClickTab(GameObject tab){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		int pos = tabList.IndexOf(tab);
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ClickTab", pos);
//		ExcuteCallback(call);
		ModuleManager.SendMessage (ModuleEnum.SelectRoleModule, "ClickTab", pos);
	}

	void ClickButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ClickButton", null);
//		ExcuteCallback(call);
		ModuleManager.SendMessage (ModuleEnum.SelectRoleModule, "ClickButton");

		Umeng.GA.FinishLevel ("SelectRole");
		GameDataAnalysis.Event(GameDataAnalysisEventType.SelectRole, new Dictionary<string,string>(){{"DeviceInfo",SystemInfo.deviceUniqueIdentifier},{"Stage","End"}});
	}

}
