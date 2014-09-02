using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpMaterialWindow : ViewBase {
	DragPanel materialDragPanel;
	private List<TUserUnit> userUnitInfoList = new List<TUserUnit>();

	public override void Init(UIConfigItem config){
		base.Init(config);
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
		DataCenter.Instance.GetUnitInfo (curUnitId).GetAsset (UnitAssetType.Avatar, o=>{
			avatarTex.mainTexture = o as Texture2D;
		});//UnitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);

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
//		MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
	}

	void PressItem(GameObject item){  
		UserUnit unitInfo = materialUnitInfoDic[ item ];
		ModuleManger.Instance.ShowModule(ModuleEnum.UnitDetailModule );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);
	}

	private void InitDragPanel(){
		string name = "MaterialDragPanel";
		int count = DataCenter.Instance.ConfigViewData.Count;
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		ResourceManager.Instance.LoadLocalAsset( itemSourcePath ,o =>{
			GameObject itemGo = o as GameObject;
			materialDragPanel = CreateDragPanel( name, count, itemGo) ;
			FillDragPanel( materialDragPanel );
			materialDragPanel.DragPanelView.SetScrollView(ConfigDragPanel.LevelUpMaterialDragPanelArgs, transform);
		});

	}
	
	private DragPanel CreateDragPanel( string name, int count, GameObject item){
		DragPanel panel = new DragPanel(name,item);
//		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}

	private void FillDragPanel(DragPanel panel){
		if( panel == null )	return;
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject currentItem = panel.ScrollItem[ i ];
			materialUnitInfoDic.Add(currentItem, DataCenter.Instance.ConfigViewData[ i ]);
                        ShowAvatar( currentItem );
                        AddEventListener( currentItem );
                }
	}


}
