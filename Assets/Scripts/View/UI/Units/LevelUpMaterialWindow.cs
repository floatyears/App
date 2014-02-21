using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpMaterialWindow : UIComponentUnity {
	DragPanel materialDragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private List<UserUnitInfo> userUnitInfoList = new List<UserUnitInfo>();
//	Dictionary<GameObject, UserUnitInfo> materialUnitInfoDic = new Dictionary<GameObject, UserUnitInfo>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}
	
	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
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
		Debug.Log(string.Format("Show Avatar named as {0}", item));
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		uint id = materialUnitInfoDic[ item ].id;
		string sourceTexPath = "Avatar/role00" + id.ToString();
		Debug.Log("ShowAvatar, the avatar texure path is : " + sourceTexPath);
		Texture2D sourceTex = Resources.Load( sourceTexPath ) as Texture2D;
		avatarTex.mainTexture = sourceTex;
	}

	private void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickMaterialItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}

	Dictionary<GameObject, UnitInfo> materialUnitInfoDic = new Dictionary<GameObject, UnitInfo>();
	private void ClickMaterialItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		UnitInfo tempInfo = materialUnitInfoDic[ item ];
		MsgCenter.Instance.Invoke( CommandEnum.PickMaterialUnitInfo, tempInfo);
		MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
	}

	void PressItem(GameObject item){  
		UnitInfo unitInfo = materialUnitInfoDic[ item ];
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitInfo, unitInfo);
        }

	private void InitDragPanel(){

		string name = "MaterialDragPanel";
		int count = ConfigViewData.OwnedUnitInfoList.Count;
		Debug.Log( string.Format("Material Window: The count to add is : " + count) );
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		materialDragPanel = CreateDragPanel( name, count, itemGo) ;
		FillDragPanel( materialDragPanel );
		InitDragPanelArgs();
		materialDragPanel.RootObject.SetScrollView(dragPanelArgs);
	}
	
	private DragPanel CreateDragPanel( string name, int count, GameObject item){
		Debug.Log("Create Drag Panel");
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}

	private void FillDragPanel(DragPanel panel){
		Debug.Log("Fill Drag Panel");
		if( panel == null )	return;

		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject currentItem = panel.ScrollItem[ i ];
			UITexture tex = currentItem.GetComponentInChildren<UITexture>();
			//materialUnitInfoDic.Add(currentItem, ConfigViewData.OwnedUnitInfoList[ i ]);
			ShowAvatar( currentItem );
			AddEventListener( currentItem );
		}
	}

	private void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans",	transform);
		dragPanelArgs.Add("scrollerScale",	Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	-60 * Vector3.up);
		dragPanelArgs.Add("position", 		Vector3.zero);
		dragPanelArgs.Add("clipRange", 		new Vector4(0, -120, 640, 400));
		dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine",		3);
		dragPanelArgs.Add("scrollBarPosition",	new Vector3(-320, -330, 0));
		dragPanelArgs.Add("cellWidth", 		120);
		dragPanelArgs.Add("cellHeight",		120);
	}
}
