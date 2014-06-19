using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectRoleView : UIComponentUnity {
	UIButton selectBtn;
	List<GameObject> tabList = new List<GameObject>();
	List<GameObject> contentList = new List<GameObject>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();

		//NoviceGuideStepEntityManager.Instance ().StartStep ();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (call.funcName){
			case "ShowInitialView" : 
				CallBackDispatcherHelper.DispatchCallBack(ShowInitialView, call);
				break;
			default:
				break;
		}
	}

	void InitUI(){
		int itemCount = 3;
		GameObject item;
		for (int i = 0; i < itemCount; i++){
			item = transform.FindChild("Tab_" + i.ToString()).gameObject;
			tabList.Add(item);
			item = transform.FindChild("Content_" + i.ToString()).gameObject;
			contentList.Add(item);
		}
		Debug.Log("UnitSelect.FindItem......Tab Item count is : " + tabList.Count);
		Debug.Log("UnitSelect.FindItem......Content Item count is : " + contentList.Count);

		selectBtn = transform.FindChild("Button_Select").GetComponent<UIButton>();
		UIEventListener.Get(selectBtn.gameObject).onClick = ClickButton;
	}

	void ShowInitialView(object args){
		List<TUnitInfo> unitInfoList = args as List<TUnitInfo>;

		int initialLevel = 1;
		UITexture texture;
		UILabel label;

		Debug.LogError("tabList.Count : " + tabList.Count);
		Debug.LogError("unitInfoList.Count : " + unitInfoList.Count);

		//Tab
		for (int i = 0; i < tabList.Count; i++){
			label = tabList[ i ].transform.FindChild("Label_Name").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].Name;

			UIEventListener.Get(tabList[ i ]).onClick = ClickTab;
		}

		//Content
		for (int i = 0; i < contentList.Count; i++){
			texture = contentList[ i ].transform.FindChild("Texture_Role").GetComponent<UITexture>();
			Texture2D source = unitInfoList[ i ].GetAsset(UnitAssetType.Profile);
			texture.mainTexture = source;
			texture.width = source.width;
			texture.height = source.height;

			label = contentList[ i ].transform.FindChild("Label_No").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].ID.ToString();

			label = contentList[ i ].transform.FindChild("Label_Name").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].Name;

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
			label.text = unitInfoList[ i ].UnitType;
		}
	}

	void ClickTab(GameObject tab){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		int pos = tabList.IndexOf(tab);
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ClickTab", pos);
		ExcuteCallback(call);
	}

	void ClickButton(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ClickButton", null);
		ExcuteCallback(call);
	}

}
