﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MyUnitDragPanel : UIComponentUnity {
       
	protected DragPanel dragPanel;
	protected bool exchange = false;
        protected List<TUserUnit> userUnitInfoList = new List<TUserUnit>();
	protected Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	protected Dictionary<GameObject, TUserUnit> baseUnitInfoDic = new Dictionary<GameObject, TUserUnit>();
	protected List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitDragPanel();
		MsgCenter.Instance.Invoke(CommandEnum.ReqAuthUser, null);
	}

	public override void ShowUI(){
		base.ShowUI();

		if(IsInvoking("CrossShow")) {
			CancelInvoke("CrossShow");
		}
		//		Debug.LogError("InvokeRepeating");
                InvokeRepeating("CrossShow",0.1f, 1f);
	}

	public override void HideUI(){
		base.HideUI();
	}

	protected void InitDragPanel(){
		if ( GlobalData.myUnitList != null)
			userUnitInfoList.AddRange(GlobalData.myUnitList.GetAll().Values);
		
		string name = "MyUnitDragPanel";
		if(userUnitInfoList == null ){
			Debug.LogWarning("userUnitInfoList is null ");
			return;
		}

		int count = userUnitInfoList.Count;
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		InitDragPanelArgs();
		dragPanel = CreateDragPanel( name, count, itemGo) ;
		FillDragPanel( dragPanel );
		dragPanel.RootObject.SetScrollView(dragPanelArgs);
		
	}

	protected DragPanel CreateDragPanel( string name, int count, GameObject item){
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}

	void FillDragPanel(DragPanel panel){
		if( panel == null ){
			Debug.LogError( "LevelUpBasePanel.FillDragPanel(), DragPanel is null, return!");
			return;
		}
		
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject scrollItem = panel.ScrollItem[ i ];

			TUserUnit uuItem = userUnitInfoList[ i ] ;

			baseUnitInfoDic.Add( scrollItem, uuItem );
			
			StoreLabelInfo( scrollItem);
			ShowItem( scrollItem );
			AddEventListener( scrollItem );
		}
	}

	protected void ShowItem( GameObject item){
		ShowMask(item,true);
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		
		uint uid = baseUnitInfoDic[item].UnitID;
		avatarTex.mainTexture = GlobalData.unitInfo[ uid ].GetAsset(UnitAssetType.Avatar);
		
		int addAttack = baseUnitInfoDic[ item ].AddAttack;

		int addHp = baseUnitInfoDic[ item ].AddHP;

		int level = baseUnitInfoDic[ item ].Level;

		int addPoint = addAttack + addHp;
	}

	protected void ClickBaseItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TUserUnit tempInfo = baseUnitInfoDic[ item ];
		MsgCenter.Instance.Invoke( CommandEnum.PickBaseUnitInfo, tempInfo );
		MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
		ShowMask( item, true );
	}

	protected void PressItem(GameObject item ){
		TUserUnit unitInfo = baseUnitInfoDic[ item ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);
		
	}

	void ShowMask( GameObject target, bool canMask) {
		GameObject maskSpr = target.transform.FindChild("Mask").gameObject;
		maskSpr.gameObject.SetActive( canMask );
	}

	void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickBaseItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}

	void StoreLabelInfo(GameObject item){
		
		TUserUnit tuu = baseUnitInfoDic[ item ];
		UnitInfoStruct infoStruct = new UnitInfoStruct();
		infoStruct.text1 = tuu.Level.ToString();
		infoStruct.text2 = (tuu.AddHP + tuu.AddAttack).ToString();
		infoStruct.targetLabel = item.transform.FindChild("Label_Info").GetComponent<UILabel>();
		unitInfoStruct.Add(infoStruct);
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans",	transform);
		dragPanelArgs.Add("scrollerScale",	Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	-28 * Vector3.up);
		dragPanelArgs.Add("position", 		Vector3.zero);
		dragPanelArgs.Add("clipRange", 		new Vector4(0, -120, 640, 400));
		dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine",		3);
		dragPanelArgs.Add("scrollBarPosition",	new Vector3(-320, -315, 0));
		dragPanelArgs.Add("cellWidth", 		110);
		dragPanelArgs.Add("cellHeight",		110);
	}

	protected void CrossShow(){
		if(exchange){
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "+{0}", unitInfoStruct[ i ].text2);
				unitInfoStruct[ i ].targetLabel.color = Color.yellow;
                                
                        }
			exchange = false;
		} else {
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "Lv{0}", unitInfoStruct[ i ].text1);
				unitInfoStruct[ i ].targetLabel.color = Color.red;
                                
                        }
                        exchange = true;
                }
        }//End
}
