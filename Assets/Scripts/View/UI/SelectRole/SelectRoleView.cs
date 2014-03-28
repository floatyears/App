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
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void Callback(object data){
		base.Callback(data);
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
	}

	void ShowInitialView(object args){
		Debug.Log("Receive the dispather, to Update Select View...");

		List<TUnitInfo> unitInfoList = args as List<TUnitInfo>;

		string basePath;
		UITexture texture;
		UILabel label;

		//Tab
		for (int i = 0; i < tabList.Count; i++){
			basePath = string.Format("Tab_{0}/", i);

			texture = tabList[ i ].transform.FindChild(basePath + "Texture_Avatar").GetComponent<UITexture>();
			Texture2D source = unitInfoList[ i ].GetAsset(UnitAssetType.Avatar);
			texture.mainTexture = source;
			texture.width = source.width;
			texture.height = source.height;

			label = tabList[ i ].transform.FindChild(basePath + "Label_No").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].ID.ToString();

			label = tabList[ i ].transform.FindChild(basePath + "Label_Name").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].Name;

			UIEventListener.Get(tabList[ i ]).onClick = ClickTab;
		}

		//Content
		for (int i = 0; i < contentList.Count; i++){
			basePath = string.Format("Content_{0}/", i);

			label = contentList[ i ].transform.FindChild(basePath + "Label_No").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].ID.ToString();

			label = contentList[ i ].transform.FindChild(basePath + "Label_Name").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].Name;
		}
	}

	void ClickTab(GameObject tab){
		int pos = tabList.IndexOf(tab);
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ClickTab", pos);
		ExcuteCallback(call);
	}



}
