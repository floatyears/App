using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSelectView : UIComponentUnity {
	UITexture roleTex;
	UIButton selectBtn;
	UILabel noLabel;
	UILabel nameLabel;
	UILabel typeLabel;
	UILabel raceLabel;
	UILabel lvLabel;
	UILabel hpLabel;
	UILabel atkLabel;
	UILabel favLabel;
	List<GameObject> tab = new List<GameObject>();
	List<GameObject> content = new List<GameObject>();

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
			tab.Add(item);
			item = transform.FindChild("Content_" + i.ToString()).gameObject;
			content.Add(item);
		}
		Debug.Log("UnitSelect.FindItem......Tab Item count is : " + tab.Count);
		Debug.Log("UnitSelect.FindItem......Content Item count is : " + content.Count);
	}

	void ShowInitialView(object args){
		Debug.Log("Receive the dispather, to Update Select View...");

		List<TUnitInfo> unitInfoList = args as List<TUnitInfo>;

		string basePath;
		UITexture texture;
		UILabel label;

		//Tab
		for (int i = 0; i < tab.Count; i++){
			basePath = string.Format("Tab_{0}/", i);
			texture = tab[ i ].transform.FindChild(basePath + "Texture_Avatar").GetComponent<UITexture>();
			texture.mainTexture = unitInfoList[ i ].GetAsset(UnitAssetType.Avatar);

			label = tab[ i ].transform.FindChild(basePath + "Label_No").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].ID.ToString();

			label = tab[ i ].transform.FindChild(basePath + "Label_Name").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].Name;
		}

		//Content
		for (int i = 0; i < content.Count; i++){
			basePath = string.Format("Content_{0}/", i);

			label = content[ i ].transform.FindChild(basePath + "Label_No").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].ID.ToString();

			label = content[ i ].transform.FindChild(basePath + "Label_Name").GetComponent<UILabel>();
			label.text = unitInfoList[ i ].Name;
		}
	}



}
