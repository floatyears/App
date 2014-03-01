using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpBasePanel : UIComponentUnity {

	DragPanel baseDragPanel;
	Dictionary<GameObject, TUserUnit> baseUnitInfoDic = new Dictionary<GameObject, TUserUnit>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	List<TUserUnit> userUnitInfoList = new List<TUserUnit>();

	void GetData(object data){
//		TUserUnit uu = new TUserUnit();

		//GlobalData.userInfo.
	}

	public override void Init(UIInsConfig config, IUIOrigin origin){
		InitUI();
		base.Init(config, origin);
		MsgCenter.Instance.Invoke(CommandEnum.ReqAuthUser, null);
	}

	public override void ShowUI(){
		base.ShowUI();

		AddListener();
		this.gameObject.SetActive(true);
	}
	
	public override void HideUI(){
		base.HideUI();

		RemoveListener();
	}

	private void InitUI(){
		InitDragPanel();
	}

	//CommandEnum.PanelFocus
	void ShowMyself(object data){
		string msg = (string)data;
		Debug.Log( "ShowMyself, canShow is " + msg );
		if( msg == "Tab_Friend"){
			this.gameObject.SetActive( false );
			return;
		}
		this.gameObject.SetActive(true);
	}

	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.PanelFocus, ShowMyself);
	}

	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.PanelFocus, ShowMyself);
	}
	
	void ShowItem( GameObject item){
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();

		uint uid = baseUnitInfoDic[item].ID;
		avatarTex.mainTexture = GlobalData.unitInfo[ uid ].GetAsset(UnitAssetType.Avatar);

		int addAttack = baseUnitInfoDic[ item ].AddAttack;
		//Debug.Log("LevelUpBasePanel.ShowAvatar(),  addAttack is " + addAttack);
                
                int addHp = baseUnitInfoDic[ item ].AddHP;
		//Debug.Log("LevelUpBasePanel.ShowAvatar(),  addHp is " + addHp);

		int level = baseUnitInfoDic[ item ].Level;
		//Debug.Log("LevelUpBasePanel.ShowAvatar(),  level is " + level );

                int addPoint = addAttack + addHp;

		UILabel crossFadeLabel = item.transform.FindChild("Label_Info").GetComponent<UILabel>();

		List<int> crossFadeList = new List<int>();
		crossFadeList.Add( level );
		crossFadeList.Add( addPoint );
		MsgCenter.Instance.Invoke(CommandEnum.CrossFade, crossFadeList );
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

		userUnitInfoList = GetMyUnitList();
		string name = "BaseDragPanel";
		//Debug.LogError("GlobalData.myUnitList.Count : " + GlobalData.myUnitList.Count );
		if(GlobalData.myUnitList == null ){
			Debug.LogWarning("GlobalData.myUnitList is null ");
			return;
		}
		//Debug.Log("GlobalData.myUnitList count is " + GlobalData.myUnitList.Count);

		int count = GlobalData.myUnitList.Count;
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

			baseUnitInfoDic.Add( scrollItem, uuItem );

			ShowItem( scrollItem );
			AddEventListener( scrollItem );
		}
	}


	List<TUserUnit> GetMyUnitList(){
		List<TUserUnit> uuList = new List<TUserUnit>();
		if( GlobalData.myUnitList == null ){
			Debug.LogError( "LevelUpBasePanel.GetMyUnitList(), GlobalData.myUnitList is NULL, return!");
			return null;  
		}
		uuList.AddRange( GlobalData.myUnitList.Values );
		Debug.Log( "LevelUpBasePanel.GetMyUnitList(), Get Unit Count : " + uuList.Count );
		return uuList;
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
