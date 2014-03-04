using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpFriendWindow : UIComponentUnity {

	DragPanel friendDragPanel;
	Dictionary<GameObject,TFriendInfo> friendUnitInfoDic = new Dictionary<GameObject, TFriendInfo>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	List<TFriendInfo> friendInfoList = new List<TFriendInfo>();

	List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();


	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);

		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		this.gameObject.SetActive( false );
		MsgCenter.Instance.AddListener(CommandEnum.PanelFocus, FocusOnPanel);
	}

	public override void HideUI() {
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.PanelFocus, FocusOnPanel);
	}

	void InitUI(){
		InitDragPanel();
		this.gameObject.SetActive(false);
	}

	void InitDragPanel(){
		if ( GlobalData.FriendBaseInfo != null)
			friendInfoList.AddRange(GlobalData.friends);

		string name = "FriendDragPanel";
		int count = GlobalData.friends.Count;
//		Debug.Log( string.Format("The Friend count to add is : " + count) );
		string itemSourcePath = "Prefabs/UI/Friend/AvailFriendItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		InitDragPanelArgs();
		friendDragPanel = CreateDrag( name, count, itemGo) ;
		FillDragPanel( friendDragPanel );
		friendDragPanel.RootObject.SetScrollView(dragPanelArgs);

		//StartCoroutine( CrossShow(unitInfoStruct));
	}

	private DragPanel CreateDrag( string name, int count, GameObject item){
		//Debug.Log("Create Drag Panel");
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}


	private void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickFriendItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}

	private void FillDragPanel(DragPanel panel){
		if( panel == null )	return;
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject scrollItem = panel.ScrollItem[ i ];
			//Get target data of each panel item
			TFriendInfo tfiItem = friendInfoList[ i ] ;	
			friendUnitInfoDic.Add( scrollItem, tfiItem );
			StoreLabelInfo( scrollItem);
//			Debug.Log("LevelUpFriendWindow.FillDragPanel(), unitInfoStruct cout is : " + unitInfoStruct.Count);
			ShowItem( scrollItem );
			AddEventListener( scrollItem );
                }
	}

	private void ShowItem( GameObject item){
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
//		if(friendUnitInfoDic.ContainsKey(item))
		TFriendInfo tfriendInfo;
		if(!friendUnitInfoDic.TryGetValue(item,out tfriendInfo)) {
			Debug.Log("ShowItem(),Not Exist vaule");
			return;
		}

		if(friendUnitInfoDic[item].UserUnit == null){
			Debug.LogError("LevelUpFriendWindow, friendUnitInfoDic[item].UserUnit is Null, Return");
			return;
		}
		//
		uint curUnitId = friendUnitInfoDic[item].UserUnit.UnitID;
//		Debug.LogError("Base Show Avatar : curUnitId is : " + curUnitId);
		avatarTex.mainTexture = GlobalData.unitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);

		int addAttack = tfriendInfo.UserUnit.AddAttack;
		int addHp = tfriendInfo.UserUnit.AddHP;

		int level = tfriendInfo.UserUnit.Level;

		UILabel nickNameLabel = item.transform.FindChild("Label_Name").GetComponent<UILabel>();
		if(tfriendInfo.NickName == string.Empty)
			nickNameLabel.text = "NoName";
		else
			nickNameLabel.text = tfriendInfo.NickName;

		UILabel friendTypeLabel = item.transform.FindChild("Label_Friend_Type").GetComponent<UILabel>();
		 int friendType = (int)tfriendInfo.FriendState;
//		Debug.Log("friendType : " + friendType);
		switch (friendType){
			case 1:
				friendTypeLabel.text = "Friend";
				friendTypeLabel.color = Color.yellow;
				break;
			case 4:
				friendTypeLabel.text = "Support";
				friendTypeLabel.color = Color.green;
				break;
			default:
				break;
		}

		if(friendUnitInfoDic[ item ].FriendPoint == null){
			Debug.Log("friendUnitInfoDic[ item ].FriendPoint is Null  ");
			return;
		}
//		Debug.Log("friendUnitInfoDic[ item ].FriendPoint is : " + friendUnitInfoDic[ item ].FriendPoint);
		UILabel friendPointLabel = item.transform.FindChild("Label_Friend_Point").GetComponent<UILabel>();
		friendPointLabel.text = string.Format("{0}pt",tfriendInfo.FriendPoint);
	}


	void ClickFriendItem(GameObject item){

		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TUserUnit tempInfo = friendUnitInfoDic[ item ].UserUnit;
		//Debug.LogError( tempInfo.name );
		MsgCenter.Instance.Invoke( CommandEnum.PickFriendUnitInfo, tempInfo );
                MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
	}

	void PressItem(GameObject item ){
		TUserUnit unitInfo = friendUnitInfoDic[ item ].UserUnit;
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );//before
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);//after
	}

	void FocusOnPanel(object data) {
		string msg = (string)data;
//		Debug.Log("Friend Window receive : " + msg);
		if(msg == "Tab_Friend"){
			this.gameObject.SetActive(true);

			if(IsInvoking("CrossShow")) {
				CancelInvoke("CrossShow");
			}
//			Debug.LogError("InvokeRepeating");

                        InvokeRepeating("CrossShow",0.1f, 1f);
		}else{
			this.gameObject.SetActive(false);
		}
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans", 	transform);
		dragPanelArgs.Add("scrollerScale", 	Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	-240 * Vector3.up);
		dragPanelArgs.Add("position", 		Vector3.zero);
		dragPanelArgs.Add("clipRange", 		new Vector4(0, 0, 640, 200));
		dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Horizontal);
		dragPanelArgs.Add("maxPerLine", 	0);
		dragPanelArgs.Add("scrollBarPosition", 	new Vector3(-320, -120, 0));
		dragPanelArgs.Add("cellWidth", 		130);
		dragPanelArgs.Add("cellHeight",		130);
	}

//	//Cross Show Label
//	IEnumerator CrossShow(List<UnitInfoStruct> infoStruct){
//		//		Debug.Log("CrossShow() : Start");
//		float timer = 0.0f;
//		float cycle = 1.0f;
//		bool exchange = false;
//		Debug.LogError("infoStruct.Count : " + infoStruct.Count);
//		while(true){
//			timer += Time.deltaTime;
//			if(exchange){
//				foreach (var item in infoStruct) {
//					item.targetLabel.text = string.Format( "+{0}", item.text2);
//					Debug.LogError("Text2 : " + item.text2);
//				}
//			} else {
//				foreach (var item in infoStruct) {
//					item.targetLabel.text = string.Format("Lv: {0}", item.text1);
//					Debug.LogError("Text1 : " + item.text1);
//				}
//			}
//			yield return new WaitForSeconds(cycle);
//			exchange = !exchange;
//		} 
//	}//End 


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
                                unitInfoStruct[ i ].targetLabel.text = string.Format( "+{0}", unitInfoStruct[ i ].text1);
                                //Debug.LogError("Text2 : " + item.text2);
                        }
                        exchange = true;
                }
        }//End
        
        void StoreLabelInfo(GameObject item){
                TUserUnit tuu = friendUnitInfoDic[ item ].UserUnit;
		UnitInfoStruct infoStruct = new UnitInfoStruct();
		infoStruct.text1 = tuu.Level.ToString();
		infoStruct.text2 = (tuu.AddHP + tuu.AddAttack).ToString();
//		Debug.LogError("TUserUnit.Level : " + tuu.Level.ToString());
//		Debug.LogError("TUserUnit.Add : " + (tuu.AddHP + tuu.AddAttack).ToString());
		infoStruct.targetLabel = item.transform.FindChild("Label_Info").GetComponent<UILabel>();
		unitInfoStruct.Add(infoStruct);
	}
}








