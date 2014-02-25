using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpFriendWindow : UIComponentUnity {

	DragPanel friendDragPanel;
	Dictionary<GameObject,UserUnit> friendUnitInfoDic = new Dictionary<GameObject, UserUnit>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);

		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		this.gameObject.SetActive( false );
		MsgCenter.Instance.AddListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}

	public override void HideUI() {
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}
	


	void InitUI(){
		InitDragPanel();
		this.gameObject.SetActive(false);
	}

	void InitDragPanel(){
		string name = "FriendDragPanel";
		int count = ConfigViewData.OwnedUnitInfoList.Count;
//		Debug.Log( string.Format("The Friend count to add is : " + count) );
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		InitDragPanelArgs();
		friendDragPanel = CreateDrag( name, count, itemGo) ;
		FillDragPanel( friendDragPanel );
		friendDragPanel.RootObject.SetScrollView(dragPanelArgs);
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
			GameObject currentItem = panel.ScrollItem[ i ];
			friendUnitInfoDic.Add(currentItem, ConfigViewData.OwnedUnitInfoList[ i ]);
                        ShowAvatar( currentItem );
                        AddEventListener( currentItem );
                }
	}

	private void ShowAvatar( GameObject item){
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		
		uint curUnitId = friendUnitInfoDic[item].unitId;
		//Debug.LogError("Base Show Avatar : curUnitId is : " + curUnitId);
		avatarTex.mainTexture = GlobalData.tempUnitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);

		int addAttack = friendUnitInfoDic[ item ].addAttack;
		int addHp = friendUnitInfoDic[ item ].addHp;
		item.gameObject.SendMessageUpwards( "ReceiveAddMsg", addAttack + addHp, SendMessageOptions.RequireReceiver);
		
		int level = friendUnitInfoDic[ item ].level;
		item.gameObject.SendMessageUpwards("ReceiveLevel",level,SendMessageOptions.RequireReceiver);
	}


	void ClickFriendItem(GameObject item){

		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		UserUnit tempInfo = friendUnitInfoDic[ item ];
		//Debug.LogError( tempInfo.name );
		MsgCenter.Instance.Invoke( CommandEnum.PickFriendUnitInfo, tempInfo );
                MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
	}

	void PressItem(GameObject item ){
		UserUnit unitInfo = friendUnitInfoDic[ item ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );//before
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);//after
	}

	void FocusOnPanel(object data) {
		string message = (string)data;
//		Debug.Log("Friend Window receive : " + message);

		if(message == "Tab_Friend"){
			this.gameObject.SetActive(true);
		}else{
			this.gameObject.SetActive(false);
		}
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans", 	transform);
		dragPanelArgs.Add("scrollerScale", 	Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	-238 * Vector3.up);
		dragPanelArgs.Add("position", 		Vector3.zero);
		dragPanelArgs.Add("clipRange", 		new Vector4(0, 0, 640, 200));
		dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Horizontal);
		dragPanelArgs.Add("maxPerLine", 	0);
		dragPanelArgs.Add("scrollBarPosition", 	new Vector3(-320, -96, 0));
		dragPanelArgs.Add("cellWidth", 		130);
		dragPanelArgs.Add("cellHeight",		130);
	}
}








