using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelUpBasePanel : UIComponentUnity {
	DragPanel baseDragPanel;

	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private List<UserUnitInfo> userUnitInfoList = new List<UserUnitInfo>();
	Dictionary<GameObject, UserUnitInfo> baseUnitInfoDic = new Dictionary<GameObject, UserUnitInfo>();

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
		GetUnitInfoList();
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

	private int IndexOfItem(DragPanel panel, GameObject item){
		if( panel == null || item == null ){
			Debug.LogError("IndexOf Item Error!");
			return 0;
		}
		return panel.ScrollItem.IndexOf( item);
	}

	void GetUnitInfoList() {
		UnitPartyInfo upi = ModelManager.Instance.GetData(ModelEnum.UnitPartyInfo, new ErrorMsg()) as UnitPartyInfo;
		userUnitInfoList = upi.GetUserUnit();
	}

	private void ShowAvatar( GameObject item){
		//Debug.Log(string.Format("Show Avatar named as {0}", item));
		//find des
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		 //find src 
		int scrollItemIndex = IndexOfItem( baseDragPanel, item);
		string sourceTexPath = GetAvatarInfo( scrollItemIndex );
		//Debug.Log("ShowAvatar, the avatar texure path is : " + sourceTexPath);
		Texture2D sourceTex = Resources.Load( sourceTexPath ) as Texture2D;
		//show
		avatarTex.mainTexture = sourceTex;
	}

	private void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}

	private void ClickItem(GameObject item){
		//Debug.LogError("Click Item " + item.name);
		MsgCenter.Instance.Invoke( CommandEnum.PickBaseUnitInfo, item );
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	void PressItem(GameObject itemGo){
		UserUnitInfo userUnitInfo = baseUnitInfoDic [ itemGo ];
		MsgCenter.Instance.Invoke(CommandEnum.EnterUnitInfo, userUnitInfo);
        }

	private void InitDragPanel(){
		
		string name = "MaterialDragPanel";
		int count = userUnitInfoList.Count - 2;
		//Debug.Log( string.Format("The count to add is : " + count) );
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		InitDragPanelArgs();
		baseDragPanel = CreateDragPanel( name, count, itemGo) ;
		FillDragPanel( baseDragPanel );
		baseDragPanel.RootObject.SetScrollView(dragPanelArgs);
	}
	
	private DragPanel CreateDragPanel( string name, int count, GameObject item){
		//Debug.Log("Create Drag Panel");
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}

	private void FillDragPanel(DragPanel panel){
		//Debug.Log("Fill Drag Panel");
		if( panel == null )	return;

		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject currentItem = panel.ScrollItem[ i ];
			UITexture tex = currentItem.GetComponentInChildren<UITexture>();
			UnitBaseInfo ubi = GlobalData.tempUnitBaseInfo [userUnitInfoList [i].unitBaseInfo];
			tex.mainTexture = Resources.Load(ubi.GetHeadPath) as Texture2D;
			baseUnitInfoDic.Add(currentItem, userUnitInfoList [i]);
			//ShowAvatar( currentItem );
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
