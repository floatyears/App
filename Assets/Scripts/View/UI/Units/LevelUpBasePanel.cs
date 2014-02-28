using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpBasePanel : UIComponentUnity {

	DragPanel baseDragPanel;
	Dictionary<GameObject, UserUnit> baseUnitInfoDic = new Dictionary<GameObject, UserUnit>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	List<TUserUnit> userUnitInfoList = new List<TUserUnit>();

	void GetData(object data){
		//GlobalData.userInfo.
	}

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);

		MsgCenter.Instance.Invoke(CommandEnum.ReqAuthUser, null);

		MsgCenter.Instance.AddListener(CommandEnum.RspAuthUser, GetData);

		InitUI();


	}

	public override void ShowUI(){
		base.ShowUI();
		AddListener();
		this.gameObject.SetActive(true);//start show in tabs
	}
	
	public override void HideUI(){
		base.HideUI();
		RemoveListener();

	}

	private void InitUI(){
		InitDragPanel();
	}

	private void FocusOnPanel(object data){
		string message = (string)data;
		if(message == "Tab_Base" ){
			this.gameObject.SetActive(true);
		}else{
			this.gameObject.SetActive(false);
		}
	}

	void AddListener(){

		MsgCenter.Instance.AddListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}

	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}
	
	void GetBaseUnitInfo(GameObject item, UserUnit unitInfo){
		baseUnitInfoDic.Add(item,unitInfo);
	}

	private void ShowAvatar( GameObject item){
//		Debug.LogError("Base Show Avatar: ");
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();

		uint curUnitId = baseUnitInfoDic[item].unitId;
//		Debug.LogError("Base Show Avatar : curUnitId is : " + curUnitId);
		avatarTex.mainTexture = GlobalData.unitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);

		int addAttack = baseUnitInfoDic[ item ].addAttack;
		int addHp = baseUnitInfoDic[ item ].addHp;
//		item.gameObject.SendMessageUpwards( "RcvArg", addAttack + addHp, SendMessageOptions.RequireReceiver);
		MsgCenter.Instance.Invoke(CommandEnum.CrossFade, addAttack + addHp );
		int level = baseUnitInfoDic[ item ].level;
		MsgCenter.Instance.Invoke(CommandEnum.CrossFade, level);
//		item.gameObject.SendMessageUpwards("ReceiveLevel",level,SendMessageOptions.RequireReceiver);
	}

	private void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickBaseItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}
	

	private void ClickBaseItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		UserUnit tempInfo = baseUnitInfoDic[ item ];
		MsgCenter.Instance.Invoke( CommandEnum.PickBaseUnitInfo, tempInfo );
		MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
		ShowMask( item, true );
	}

	void ShowMask( GameObject target, bool canMask) {
		GameObject maskSpr = target.transform.FindChild("Mask").gameObject;
		maskSpr.gameObject.SetActive( canMask );
	}

	void PressItem(GameObject item ){
		UserUnit unitInfo = baseUnitInfoDic[ item ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);

        }

	void InitDragPanel(){
		string name = "BaseDragPanel";
		int count = ConfigViewData.OwnedUnitInfoList.Count;
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

	private void FillDragPanel(DragPanel panel){
		if( panel == null )	return;
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject currentItem = panel.ScrollItem[ i ];
			baseUnitInfoDic.Add(currentItem, ConfigViewData.OwnedUnitInfoList[ i ]);
			ShowAvatar( currentItem );
			AddEventListener( currentItem );
		}
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
}
