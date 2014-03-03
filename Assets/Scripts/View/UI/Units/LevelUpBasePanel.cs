using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpBasePanel : UIComponentUnity {

	DragPanel baseDragPanel;
	Dictionary<GameObject, TUserUnit> baseUnitInfoDic = new Dictionary<GameObject, TUserUnit>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	List<TUserUnit> userUnitInfoList = new List<TUserUnit>();
	List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		InitUI();
		base.Init(config, origin);
		MsgCenter.Instance.Invoke(CommandEnum.ReqAuthUser, null);
	}

	public override void ShowUI(){

		AddListener();
		base.ShowUI();
		this.gameObject.SetActive(true);

	}
	
	public override void HideUI(){
		//StopCoroutine( "CrossShow");
		RemoveListener();
		base.HideUI();
	}

	private void InitUI(){
		InitDragPanel();
	}

	//CommandEnum.PanelFocus
	void ShowMyself(object data){
//		Debug.LogError("Recive Comad PanelFocus");
		string msg = (string)data;
//		Debug.Log( "ShowMyself, canShow is " + msg );
		if( msg == "Tab_Friend"){
			this.gameObject.SetActive( false );
//			StopCoroutine("CrossShow");
			return;
		}
		this.gameObject.SetActive(true);

		if(IsInvoking("CrossShow")) {
			CancelInvoke("CrossShow");
		}
//		Debug.LogError("InvokeRepeating");
		InvokeRepeating("CrossShow",0.1f, 1f);
//		StartCoroutine( CrossShow(unitInfoStruct));
	}

	void AddListener(){
//		Debug.Log("LevelUpBasePanel.AddListener()");
		MsgCenter.Instance.AddListener(CommandEnum.PanelFocus, ShowMyself);
	}

	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.PanelFocus, ShowMyself);
	}
	
	void ShowItem( GameObject item){
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();

		uint uid = baseUnitInfoDic[item].UnitID;
		avatarTex.mainTexture = GlobalData.unitInfo[ uid ].GetAsset(UnitAssetType.Avatar);

		int addAttack = baseUnitInfoDic[ item ].AddAttack;
		//Debug.Log("LevelUpBasePanel.ShowAvatar(),  addAttack is " + addAttack);
                
                int addHp = baseUnitInfoDic[ item ].AddHP;
		//Debug.Log("LevelUpBasePanel.ShowAvatar(),  addHp is " + addHp);

		int level = baseUnitInfoDic[ item ].Level;
		//Debug.Log("LevelUpBasePanel.ShowAvatar(),  level is " + level );

                int addPoint = addAttack + addHp;

		//UILabel crossFadeLabel = item.transform.FindChild("Label_Info").GetComponent<UILabel>();

	}

	private void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickBaseItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}

	private void ClickBaseItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TUserUnit tempInfo = baseUnitInfoDic[ item ];
		MsgCenter.Instance.Invoke( CommandEnum.PickBaseUnitInfo, tempInfo );
		MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
		ShowMask( item, true );
	}

	void ShowMask( GameObject target, bool canMask) {
		GameObject maskSpr = target.transform.FindChild("Mask").gameObject;
		maskSpr.gameObject.SetActive( canMask );
	}

	void PressItem(GameObject item ){
		TUserUnit unitInfo = baseUnitInfoDic[ item ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);

        }

	void InitDragPanel(){
		if ( GlobalData.myUnitList != null)
			userUnitInfoList.AddRange(GlobalData.myUnitList.GetAll().Values);

		string name = "BaseDragPanel";
		//Debug.LogError("GlobalData.myUnitList.Count : " + GlobalData.myUnitList.Count );
		if(userUnitInfoList == null ){
			Debug.LogWarning("userUnitInfoList is null ");
			return;
		}
		//Debug.Log("GlobalData.myUnitList count is " + GlobalData.myUnitList.Count);

		int count = userUnitInfoList.Count;
		//Debug.Log( string.Format("Base Panel: The count to add is : " + count) );
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		InitDragPanelArgs();
		baseDragPanel = CreateDragPanel( name, count, itemGo) ;
		FillDragPanel( baseDragPanel );
		baseDragPanel.RootObject.SetScrollView(dragPanelArgs);


	}
	
	private DragPanel CreateDragPanel( string name, int count, GameObject item){
		//Debug.Log(string.Format("Create Drag Panel -> {0}, Item's count is {1}", name, count) );
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}

	//Fill Unit Item by with config data
	void FillDragPanel(DragPanel panel){
		if( panel == null ){
			Debug.LogError( "LevelUpBasePanel.FillDragPanel(), DragPanel is null, return!");
			return;
		}

		for( int i = 0; i < panel.ScrollItem.Count; i++){
			//Get each panel item GameObject
			GameObject scrollItem = panel.ScrollItem[ i ];

			//Get target data of each panel item
			TUserUnit uuItem = userUnitInfoList[ i ] ;
			Debug.LogError("userUnitInfoList id " + userUnitInfoList[ i ].UnitID);

			baseUnitInfoDic.Add( scrollItem, uuItem );

			StoreLabelInfo( scrollItem);
			ShowItem( scrollItem );
			AddEventListener( scrollItem );
		}
	}

	void StoreLabelInfo(GameObject item){

		TUserUnit tuu = baseUnitInfoDic[ item ];
		UnitInfoStruct infoStruct = new UnitInfoStruct();
		infoStruct.text1 = tuu.Level.ToString();
		infoStruct.text2 = (tuu.AddHP + tuu.AddAttack).ToString();
//		Debug.LogError("TUserUnit.Level : " + tuu.Level.ToString());
		infoStruct.targetLabel = item.transform.FindChild("Label_Info").GetComponent<UILabel>();
		unitInfoStruct.Add(infoStruct);
	}
	
	private void InitDragPanelArgs(){
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
	bool exchange = false;
	void CrossShow(){
		if(exchange){
			//				target.text = text2;
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "+{0}", unitInfoStruct[ i ].text2);
				//Debug.LogError("Text2 : " + item.text2);
			}
			exchange = false;
		} else {
			//                                target.text = text1;
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "Lv{0}", unitInfoStruct[ i ].text1);
				//Debug.LogError("Text2 : " + item.text2);
                        }
			exchange = true;
                }
        }//End
        
        //Cross Show Label
//        IEnumerator CrossShow(List<UnitInfoStruct> infoStruct){
//                
//                float timer = 0.0f;
//                float cycle = 1.0f;
//		bool exchange = false;
//		Debug.LogError("infoStruct.Count : " + infoStruct.Count);
//		while(true){
//			Debug.Log("CrossShow() : Start");
//			timer += Time.deltaTime;
//			if(exchange){
////				target.text = text2;
//				foreach (var item in infoStruct) {
//					item.targetLabel.text = string.Format( "+{0}", item.text2);
////					Debug.LogError("Text2 : " + item.text2);
//				}
//                        } else {
////                                target.text = text1;
//				foreach (var item in infoStruct) {
//					item.targetLabel.text = string.Format("Lv: {0}", item.text1);
////					Debug.LogError("Text2 : " + item.text1);
//                                }
//                        }
//                        yield return new WaitForSeconds(cycle);
//                        exchange = !exchange;
//                } 
//        }//End 
        
}


public class UnitInfoStruct{
	public string text1;
	public string text2;
	public UILabel targetLabel;
}


