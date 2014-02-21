using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpFriendWindow : UIComponentUnity {
	DragPanel friendDragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	List<TempAvailFriend> availFriendList;
	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);

		availFriendList = ConfigAvailFriend.availFriendList;

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
		//CreateDragPanel();
		InitDragPanel();
		this.gameObject.SetActive(false);
	}

	void InitDragPanel(){
		string name = "FriendDragPanel";
		int count = ConfigViewData.OwnedUnitInfoList.Count;
		Debug.Log( string.Format("The Friend count to add is : " + count) );
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
		//Debug.Log("Fill Drag Panel");
		if( panel == null )	return;
		
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject currentItem = panel.ScrollItem[ i ];
			UITexture tex = currentItem.GetComponentInChildren<UITexture>();
			friendUnitInfoDic.Add(currentItem, ConfigViewData.OwnedUnitInfoList[ i ]);
			ShowAvatar( currentItem );
			AddEventListener( currentItem );
		}
	}

	private void ShowAvatar( GameObject item){
		//Debug.Log(string.Format("Show Avatar named as {0}", item));
		//find des
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		//find src 
		//uint id = friendUnitInfoDic[ item ].id;
		//string sourceTexPath = "Avatar/role00" + id.ToString();
		//Debug.Log("ShowAvatar, the avatar texure path is : " + sourceTexPath);
		//Texture2D sourceTex = Resources.Load( sourceTexPath ) as Texture2D;
		//show
		//avatarTex.mainTexture = sourceTex;
	}

	Dictionary<GameObject,UserUnit> friendUnitInfoDic = new Dictionary<GameObject, UserUnit>();
	void ClickFriendItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		//UnitInfo tempInfo = friendUnitInfoDic[ item ];
		//MsgCenter.Instance.Invoke(CommandEnum.PickFriendUnitInfo, tempInfo);
		MsgCenter.Instance.Invoke(CommandEnum.TryEnableLevelUp, true);
	}

	void PressItem(GameObject item ){
//		UnitInfo unitInfo = friendUnitInfoDic[ item ];
//		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitInfo, unitInfo);
	}

	void FocusOnPanel(object data) {
		string message = (string)data;
		Debug.Log("Friend Window receive : " + message);

		if(message == "Tab_Friend"){
			this.gameObject.SetActive(true);
		}else{
			this.gameObject.SetActive(false);
		}
	}
	
	void CreateDragPanel(){
		GameObject friendItem = 
			Resources.Load("Prefabs/UI/Friend/AvailFriendItem") as GameObject;
		friendDragPanel = new DragPanel("DragPanel", friendItem);
		friendDragPanel.CreatUI();
		friendDragPanel.AddItem(availFriendList.Count);

		for (int i = 0; i < friendDragPanel.ScrollItem.Count; i++){
			UIEventListenerCustom.Get(friendDragPanel.ScrollItem[ i ]).onClick = ClickFriendItem;


			GameObject avatarGo = friendDragPanel.ScrollItem[i].transform.FindChild("Texture_Avatar").gameObject;
			UITexture avatarTexture = avatarGo.GetComponent<UITexture>();
			GameObject userNameGo = friendDragPanel.ScrollItem[ i ].transform.FindChild("Label_Name").gameObject;
			UILabel availFriendUserNameLabel = userNameGo.GetComponent<UILabel>();
			GameObject friendTypeGo = friendDragPanel.ScrollItem[ i ].transform.FindChild("Label_Friend_Type").gameObject;
			UILabel friendTypeLabel = friendTypeGo.GetComponent<UILabel>();
			GameObject friendPointGo = friendDragPanel.ScrollItem[ i ].transform.FindChild("Label_Friend_Point").gameObject;
			UILabel friendPointLabel = friendPointGo.GetComponent<UILabel>();

			//Get Friend Avatar Texture
			avatarTexture.mainTexture = Resources.Load(availFriendList[ i ].unitTextureSoucePath) as Texture2D;
			//Get Friend's UserName
			availFriendUserNameLabel.text = availFriendList[ i ].userName;

			//Get Friend Type
			switch (availFriendList[ i ].friendType) {
				case FriendType.IsFriend :
					friendTypeLabel.text = "Friend";
					friendTypeLabel.color = Color.yellow;
					friendPointLabel.text = string.Format("{0}pt", availFriendList[ i ].friendPoint);
					friendPointLabel.color = Color.yellow;

					break;
				case FriendType.Support :
					friendTypeLabel.text = "Support";
					friendTypeLabel.color = Color.green;
					friendPointLabel.text = string.Format("{0}pt", availFriendList[ i ].friendPoint);
					friendPointLabel.color = Color.green;
					break;
				default:
					friendTypeLabel.text = string.Empty;
					break;
			}

			//Get Friend 

		}

		InitDragPanelArgs();
		friendDragPanel.RootObject.SetScrollView(dragPanelArgs);
	}



	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans", 	transform);
		dragPanelArgs.Add("scrollerScale", 	Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	-255 * Vector3.up);
		dragPanelArgs.Add("position", 		Vector3.zero);
		dragPanelArgs.Add("clipRange", 		new Vector4(0, 0, 640, 200));
		dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Horizontal);
		dragPanelArgs.Add("maxPerLine", 	0);
		dragPanelArgs.Add("scrollBarPosition", 	new Vector3(-320, -130, 0));
		dragPanelArgs.Add("cellWidth", 		140);
		dragPanelArgs.Add("cellHeight",		140);
	}
}

public class ConfigAvailFriend{
	public static List<TempAvailFriend> availFriendList = new List<TempAvailFriend>();
	public ConfigAvailFriend(){
		Config();
	}

	void Config(){
//		Debug.Log("Config the data of available friend list");
		TempAvailFriend friendItem;

		friendItem = new TempAvailFriend();
		friendItem.userName = "Orca";
		friendItem.friendType = FriendType.IsFriend;
		friendItem.friendPoint = 10;
		friendItem.hp = 1788;
		friendItem.atk = 988;
		friendItem.hpAdd = 5;
		friendItem.atkAdd = 6;
		friendItem.currentLevel = 50;
		friendItem.maxLevel = 50;
		friendItem.unitTextureSoucePath = "Avatar/role028";
		availFriendList.Add(friendItem);

		friendItem = new TempAvailFriend();
		friendItem.userName = "Kory";
		friendItem.friendType = FriendType.IsFriend;
		friendItem.friendPoint = 10;
		friendItem.hp = 2143;
		friendItem.atk = 1024;
		friendItem.hpAdd = 7;
		friendItem.atkAdd = 8;
		friendItem.currentLevel = 99;
		friendItem.maxLevel = 72;
		friendItem.unitTextureSoucePath = "Avatar/role016";
		availFriendList.Add(friendItem);

		friendItem = new TempAvailFriend();
		friendItem.userName = "Lynn";
		friendItem.friendType = FriendType.Support;
		friendItem.friendPoint = 5;
		friendItem.hp = 693;
		friendItem.atk = 271;
		friendItem.hpAdd = 3;
		friendItem.atkAdd = 2;
		friendItem.currentLevel = 33;
		friendItem.maxLevel = 60;
		friendItem.unitTextureSoucePath = "Avatar/role015";
		availFriendList.Add(friendItem);

		friendItem = new TempAvailFriend();
		friendItem.userName = "Kirere";
		friendItem.friendType = FriendType.IsFriend;
		friendItem.friendPoint = 10;
		friendItem.hp = 567;
		friendItem.atk = 178;
		friendItem.hpAdd = 4;
		friendItem.atkAdd = 1;
		friendItem.currentLevel = 50;
		friendItem.maxLevel = 50;
		friendItem.unitTextureSoucePath = "Avatar/role012";
		availFriendList.Add(friendItem);

		friendItem = new TempAvailFriend();
		friendItem.userName = "Tony";
		friendItem.friendType = FriendType.Support;
		friendItem.friendPoint = 5;
		friendItem.hp = 1555;
		friendItem.atk = 918;
		friendItem.hpAdd = 4;
		friendItem.atkAdd = 3;
		friendItem.currentLevel = 99;
		friendItem.maxLevel = 72;
		friendItem.unitTextureSoucePath = "Avatar/role007";
		availFriendList.Add(friendItem);

		friendItem = new TempAvailFriend();
		friendItem.userName = "Neon";
		friendItem.friendType = FriendType.Support;
		friendItem.friendPoint = 5;
		friendItem.hp = 809;
		friendItem.atk = 478;
		friendItem.hpAdd = 2;
		friendItem.atkAdd = 1;
		friendItem.currentLevel = 35;
		friendItem.maxLevel = 26;
		friendItem.unitTextureSoucePath = "Avatar/role018";
		availFriendList.Add(friendItem);

	}

}

public class TempAvailFriend{
	public string userName = string.Empty;
	public FriendType friendType = FriendType.IsFriend;
	public int friendPoint = 0;
	public int hpAdd = 0;
	public int atkAdd = 0;
	public int atk = 0;
	public int hp = 0;
	public int currentLevel = 0;
	public int maxLevel = 0;
	public string unitTextureSoucePath = string.Empty;
}

public enum FriendType{
	IsFriend,
	Support
}








