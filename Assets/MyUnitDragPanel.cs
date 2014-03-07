using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MyUnitDragPanel : UIComponentUnity {

       	bool canChangePartyItem = false;

	GameObject rejectItem;
	protected DragPanel dragPanel;
	protected bool exchange = false;
        protected List<TUserUnit> userUnitInfoList = new List<TUserUnit>();
	protected Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	protected Dictionary<GameObject, TUserUnit> myUnitInfoDic = new Dictionary<GameObject, TUserUnit>();
	protected List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
//		MsgCenter.Instance.Invoke(CommandEnum.ReqAuthUser, null);
		InitDragPanel();
	}

	public override void ShowUI(){
		base.ShowUI();

		canChangePartyItem = false;

		if(IsInvoking("CrossShow")) {
			CancelInvoke("CrossShow");
		}
                InvokeRepeating("CrossShow",0.1f, 1f);
		ActivateAllMask(true);
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();

	}

	protected void InitDragPanel(){
		if ( GlobalData.myUnitList != null)
			userUnitInfoList.AddRange(GlobalData.myUnitList.GetAll().Values);
		else {
			Debug.Log("lobalData.myUnitList is null, return");
			return;
		}

		if(userUnitInfoList == null ){
			Debug.LogWarning("userUnitInfoList is null ");
			return;
		}

		int unitCount = userUnitInfoList.Count;
//		Debug.Log("My Unit Count : " + unitCount);
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject unitItem =  Resources.Load( itemSourcePath ) as GameObject;
		rejectItem = Resources.Load("Prefabs/UI/Friend/RejectItem") as GameObject ;
		
		InitDragPanelArgs();

		dragPanel =new DragPanel("MyUnitDragPanel", unitItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(1,rejectItem);
		dragPanel.AddItem(unitCount,unitItem);
		FillDragPanel( dragPanel );
		dragPanel.RootObject.SetScrollView(dragPanelArgs);
	}

	protected DragPanel CreateDragPanel( string name, int count, GameObject item){
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count, item);
		return panel;
	}

	void FillDragPanel(DragPanel panel){
		if( panel == null ){
			Debug.LogError( "MyUnitDragPanel.FillDragPanel(), DragPanel is null, return!");
			return;
		}

		for( int i = 1; i < panel.ScrollItem.Count; i++){
			GameObject scrollItem = panel.ScrollItem[ i ];

			TUserUnit uuItem = userUnitInfoList[ i - 1 ] ;
			myUnitInfoDic.Add( scrollItem, uuItem );
			
			StoreLabelInfo( scrollItem);
			ShowItem( scrollItem );
			AddEventListener( scrollItem );
		}
	}

	void ShowItem( GameObject item){
		ShowMask(item,true);
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		
		uint uid = myUnitInfoDic[item].UnitID;
		avatarTex.mainTexture = GlobalData.unitInfo[ uid ].GetAsset(UnitAssetType.Avatar);
		
		int addAttack = myUnitInfoDic[ item ].AddAttack;

		int addHp = myUnitInfoDic[ item ].AddHP;

		int level = myUnitInfoDic[ item ].Level;

		int addPoint = addAttack + addHp;
	}

	void ClickDragItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		if( !canChangePartyItem ){ 
			Debug.LogError("MyUnitDragPanel.ClickDragItem(), canChangePartyItem is false!!! , do nothing!! ");
			return;
		}

		TUserUnit tuu = myUnitInfoDic[ item ];
		BriefUnitInfo bui = new BriefUnitInfo("unitList", tuu);

		MsgCenter.Instance.Invoke(CommandEnum.ShowSelectUnitInfo, bui);
		MsgCenter.Instance.Invoke(CommandEnum.OnPartySelectUnit, tuu);

		//MsgCenter.Instance.Invoke(CommandEnum.ShowMyUnitListBriefInfo, tuu );
	}

	protected void PressItem(GameObject item ){
		TUserUnit unitInfo = myUnitInfoDic[ item ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);
		
	}

	void ShowMask( GameObject target, bool canMask) {
		GameObject maskSpr = target.transform.FindChild("Mask").gameObject;
		maskSpr.gameObject.SetActive( canMask );
	}

	void AddEventListener( GameObject item){
		UIEventListener.Get( item ).onClick = ClickDragItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}

	void StoreLabelInfo(GameObject item){
		
		TUserUnit tuu = myUnitInfoDic[ item ];
		UnitInfoStruct infoStruct = new UnitInfoStruct();
		infoStruct.text1 = tuu.Level.ToString();
		infoStruct.text2 = (tuu.AddHP + tuu.AddAttack).ToString();
		infoStruct.targetLabel = item.transform.FindChild("Label_Info").GetComponent<UILabel>();
		unitInfoStruct.Add(infoStruct);
	}

	void InitDragPanelArgs(){
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

	void CrossShow(){
		if(exchange){
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "+{0}", unitInfoStruct[ i ].text2);
				unitInfoStruct[ i ].targetLabel.color = Color.yellow;
                                
                        }
			exchange = false;
		} else {
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "Lv{0}", unitInfoStruct[ i ].text1);
				unitInfoStruct[ i ].targetLabel.color = Color.red;
                                
                        }
                        exchange = true;
                }
        }//End

	void ShowTween()
	{
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list)
		{		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	public override void Callback(object data){
		base.Callback(data);

		CallBackDeliver cbd = data as CallBackDeliver;
		switch (cbd.callBackName){
			case "activate" : 
				ActivateAllMask(false);
				break; 
			default:
				break;
		}	
	}

	void ActivateAllMask(bool b){
		rejectItem.transform.FindChild("Mask").gameObject.SetActive(b);
		foreach (var item in myUnitInfoDic){
			ShowMask(item.Key,b);
		}
		canChangePartyItem = true;
	}

}

public class CallBackDeliver{
	public CallBackDeliver(string name,object content){
		this.callBackName = name;
		this.callBackContent = content;
	}

	public string callBackName;
	public object callBackContent;
}




