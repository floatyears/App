using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpMaterialWindow : UIComponentUnity {
	DragPanel materialDragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private List<TUserUnit> userUnitInfoList = new List<TUserUnit>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.PanelFocus, FocusOnPanel);
	}
	
	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.PanelFocus, FocusOnPanel);
	}
	
	private void InitUI(){
		//GetUnitInfoList();
		InitDragPanel();
		this.gameObject.SetActive(false);
	}
	
	private void FocusOnPanel(object data){
		string message = (string)data;
		//Debug.Log("Material Window receive : " + message);
		if(message == "Tab_Material" ){
			this.gameObject.SetActive(true);
		}else{
			this.gameObject.SetActive(false);
		}
	}
	

	private void ShowAvatar( GameObject item){
//		Debug.LogError("Material Show Avatar: ");
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		
		uint curUnitId = materialUnitInfoDic[item].unitId;
//		Debug.LogError("Material Show Avatar : curUnitId is : " + curUnitId);
		avatarTex.mainTexture = GlobalData.unitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);

		int addAttack = materialUnitInfoDic[ item ].addAttack;
		int addHp = materialUnitInfoDic[ item ].addHp;
		//item.gameObject.SendMessageUpwards( "ReceiveAddMsg", addAttack + addHp, SendMessageOptions.RequireReceiver);
		
		int level = materialUnitInfoDic[ item ].level;
		//item.gameObject.SendMessageUpwards("ReceiveLevel",level,SendMessageOptions.RequireReceiver);
	}

	private void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickMaterialItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;

	}

	Dictionary<GameObject, UserUnit> materialUnitInfoDic = new Dictionary<GameObject, UserUnit>();
	private void ClickMaterialItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		UserUnit tempInfo = materialUnitInfoDic[ item ];
		MsgCenter.Instance.Invoke( CommandEnum.PickMaterialUnitInfo, tempInfo);
		MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
	}

	void PressItem(GameObject item){  
		UserUnit unitInfo = materialUnitInfoDic[ item ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);
	}

	private void InitDragPanel(){
		string name = "MaterialDragPanel";
		int count = ConfigViewData.OwnedUnitInfoList.Count;
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		materialDragPanel = CreateDragPanel( name, count, itemGo) ;
		FillDragPanel( materialDragPanel );
		InitDragPanelArgs();
		materialDragPanel.RootObject.SetScrollView(dragPanelArgs);
	}
	
	private DragPanel CreateDragPanel( string name, int count, GameObject item){
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}

	private void FillDragPanel(DragPanel panel){
		if( panel == null )	return;
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject currentItem = panel.ScrollItem[ i ];
			materialUnitInfoDic.Add(currentItem, ConfigViewData.OwnedUnitInfoList[ i ]);
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
