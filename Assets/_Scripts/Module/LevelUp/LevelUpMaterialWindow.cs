using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpMaterialWindow : ViewBase {
	DragPanel materialDragPanel;
	private List<UserUnit> userUnitInfoList = new List<UserUnit>();

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
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
//		DataCenter.Instance.UnitData.GetUnitInfo (curUnitId).GetAsset (UnitAssetType.Avatar, o=>{
//			avatarTex.mainTexture = o as Texture2D;
//		});//UnitInfo[ curUnitId ].GetAsset(UnitAssetType.Avatar);
		ResourceManager.Instance.GetAvatar(UnitAssetType.Avatar,curUnitId, o=>{
			avatarTex.mainTexture = o as Texture2D;
		});

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
		ModuleManager.Instance.ShowModule(ModuleEnum.UnitDetailModule ,"unit",materialUnitInfoDic[ item ]);
	}

	private void InitDragPanel(){
		string name = "MaterialDragPanel";
		int count = DataCenter.Instance.ConfigViewData.Count;
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		ResourceManager.Instance.LoadLocalAsset( itemSourcePath ,o =>{
			GameObject itemGo = o as GameObject;

			materialDragPanel = new DragPanel(name,itemGo,transform);
			//		panel.CreatUI();
			materialDragPanel.AddItem( count);
			FillDragPanel( materialDragPanel );
		});

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
