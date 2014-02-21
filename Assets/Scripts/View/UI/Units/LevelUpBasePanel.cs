using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpBasePanel : UIComponentUnity {

	DragPanel baseDragPanel;
	Dictionary<GameObject, UserUnit> baseUnitInfoDic = new Dictionary<GameObject, UserUnit>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private List<UserUnitInfo> userUnitInfoList = new List<UserUnitInfo>();
	
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
		InitDragPanel();
	}
	
	private void FocusOnPanel(object data){
		string message = (string)data;
		Debug.Log("Base Window receive : " + message);

		if(message == "Tab_Base" ){
			this.gameObject.SetActive(true);
		}else{
			this.gameObject.SetActive(false);
		}
	}

	private string GetAvatarInfo( int index){
		if( ConfigViewData.OwnedUnitInfoList.Count < (index + 1) ){
			Debug.LogError("The OwnedUnitInfo List Not has so many item");
			return string.Empty;
		}
		string avatarSourcePath;
		int unitId = (int)ConfigViewData.OwnedUnitInfoList[ index ].id;
		avatarSourcePath = "Avatar/role01" + unitId.ToString();
		//Debug.LogError(avatarSourcePath);
		return avatarSourcePath;
	}
	

	void GetBaseUnitInfo(GameObject item, UserUnit unitInfo){
		baseUnitInfoDic.Add(item,unitInfo);
	}

	private void ShowAvatar( GameObject item){
		//Debug.Log(string.Format("Show Avatar named as {0}", item));
		//find des
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		 //find src 
		uint id = baseUnitInfoDic[item].id;
		string sourceTexPath = "Avatar/role00" + id.ToString();
		//Debug.Log("ShowAvatar, the avatar texure path is : " + sourceTexPath);
		Texture2D sourceTex = Resources.Load( sourceTexPath ) as Texture2D;
		//show
		avatarTex.mainTexture = sourceTex;
	}

	private void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickBaseItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}
	

	private void ClickBaseItem(GameObject item){
		//Debug.Log()
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		UserUnit tempInfo = baseUnitInfoDic[ item ];
		//Debug.LogError( tempInfo.name );
		MsgCenter.Instance.Invoke( CommandEnum.PickBaseUnitInfo, tempInfo );
		MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
	}

	void PressItem(GameObject item ){
		UserUnit unitInfo = baseUnitInfoDic[ item ];
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitInfo, unitInfo);

        }

	private void InitDragPanel(){
		string name = "BaseDragPanel";
		int count = ConfigViewData.OwnedUnitInfoList.Count;
		Debug.Log( string.Format("Base Panel: The count to add is : " + count) );
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		InitDragPanelArgs();
		baseDragPanel = CreateDragPanel( name, count, itemGo) ;
		FillDragPanel( baseDragPanel );
		baseDragPanel.RootObject.SetScrollView(dragPanelArgs);
	}
	
	private DragPanel CreateDragPanel( string name, int count, GameObject item){
		Debug.Log(string.Format("Create Drag Panel -> {0}, Item's count is {1}", name, count) );
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}

	private void FillDragPanel(DragPanel panel){
		Debug.Log(string.Format("Fill Drag Panel -> {0}" + panel.RootObject.name) );
		if( panel == null )	return;
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject currentItem = panel.ScrollItem[ i ];
			baseUnitInfoDic.Add(currentItem, ConfigViewData.OwnedUnitInfoList[ i ]);
			ShowAvatar( currentItem );
			AddEventListener( currentItem );
		}
	}
		
	void FillItem( GameObject item){

	}

	Texture2D GetAvatarSouce(string sourcePath){
		Texture2D tex2d = Resources.Load( sourcePath ) as Texture2D;;
		if( tex2d == null ){
			return null;
			Debug.LogError( string.Format("GetAvatarSouce Error, sourcePath is : {0}", sourcePath) );
		}
		return tex2d;
	}

	string GetSourcePath( int id){
		//string sourcePath = "Avatar/role"
	}

	void FillAvatar(GameObject target, Texture2D source){
		GameObject avatar = target.transform.FindChild("Texture_Avatar").gameObject;
		UITexture avatarTex = avatar.GetComponent<UITexture>();
		avatarTex.mainTexture = source;
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
