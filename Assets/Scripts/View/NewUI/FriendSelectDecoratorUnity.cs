using UnityEngine;
using System.Collections.Generic;

public class FriendSelectDecoratorUnity : UIComponentUnity,IUICallback {

	private GameObject msgBox;
	private UIImageButton btnStart;
	private UIButton btnSure;
	private UIButton btnCancel;
	private UIButton btnSeeInfo;
	private UILabel labelCurrentPartyIndex;
	private UILabel labelPartyTotalCount;

	private GameObject leftArrowBtn;
	private GameObject rightArrowBtn;

	private DragPanel friendsScroller;
	private GameObject friendItem;

	private int currentPartyIndex;
	private int partyTotalCount;
	private int initPartyPage = 1;

	private List< UISprite > partySpriteList = new List< UISprite >();
	private Dictionary<int, UISprite> partySprit = new Dictionary<int,UISprite> ();
	private Dictionary<int, UnitBaseInfo> unitBaseInfo = new Dictionary<int, UnitBaseInfo> ();
	private UISprite friendSprite;

	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);
		InitUI();

	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTweenPostion( 0.2f );
		btnStart.isEnabled = false;
		friendsScroller.RootObject.gameObject.SetActive(true);
	}
	
	public override void HideUI () {
		base.HideUI ();
		ShowTweenPostion();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		InitPartyLabel();
		InitPartyArrow();
		InitPartyUnits();
		InitMsgBox();
		InitFriendList();
	}

	
	public void Callback (object data) {
		if (data == null) {
			ShowPartyInfo (null);
		} 
		else {
			Dictionary<int,UnitBaseInfo> upi = data as Dictionary<int,UnitBaseInfo>;
			if(upi == null) {
				return;
			}

			ShowPartyInfo(upi);
		}
	}

	void SendUnitPage (int pageIndex) {
		IUICallback call = origin as IUICallback;
		if (call == null) {
			return;		
		} 

		call.Callback (pageIndex);
	}

	private void InitPartyUnits() {
		UISprite temp;
		for( int i = 1; i < 5; i++) {
			temp = FindChild< UISprite >("Window/window_party/Unit" + i.ToString() );
			UIEventListenerCustom.Get(temp.gameObject).LongPress = LongPressCallback;
			partySpriteList.Add( temp );
			partySprit.Add(i, temp);

		}
		friendSprite = FindChild< UISprite >("Window/window_party/Friend");
		//ShowPartyUnit();
		friendSprite.spriteName = string.Empty;

		SendUnitPage (1);
	}

	void LongPressCallback(GameObject target) {
		int posID = -1;
		foreach (var item in partySprit) {
			if(target == item.Value.gameObject) {
				posID = item.Key;
			}
		}
		MsgCenter.Instance.Invoke (CommandEnum.EnterUnitInfo, unitBaseInfo [posID].spriteName);
	}

	private void InitPartyArrow()
	{
		leftArrowBtn = FindChild("Window/window_party/left_arrow");
		rightArrowBtn = FindChild("Window/window_party/right_arrow");
		
		UIEventListener.Get( leftArrowBtn ).onClick = BackParty;
		UIEventListener.Get( rightArrowBtn ).onClick = ForwardParty;
	}
	
	private void InitPartyLabel()
	{
		labelCurrentPartyIndex = FindChild< UILabel >("Window/window_party/Label_party_current");
		labelPartyTotalCount = FindChild< UILabel >("Window/window_party/Label_party_total");
		currentPartyIndex = 1;
		partyTotalCount = UIConfig.partyTotalCount;	
		labelCurrentPartyIndex.text = currentPartyIndex.ToString();
		labelPartyTotalCount.text = partyTotalCount.ToString();
	}
	private void InitPartyInfoPanel() {}

	private void InitMsgBox()
	{
		msgBox = FindChild("Window/msg_box");
		btnSure = FindChild< UIButton >( "Window/msg_box/btn_choose" );
		btnCancel = FindChild< UIButton >( "Window/msg_box/btn_exit" );
		btnSeeInfo = FindChild< UIButton >( "Window/msg_box/btn_see_info" );
		btnStart = FindChild< UIImageButton >( "ScrollView/btn_quest_start" );	
		UIEventListener.Get(btnStart.gameObject).onClick = ClickStartBtn;
		UIEventListener.Get(btnCancel.gameObject).onClick = ClickCancelBtn;
		UIEventListener.Get(btnSure.gameObject).onClick = ClickChooseBtn;
		UIEventListener.Get(btnSeeInfo.gameObject).onClick = ClickSeeInfoBtn;
		msgBox.SetActive( false );
	}

	private void InitFriendList()
	{
		friendItem = Resources.Load("Prefabs/UI/Friend/FriendScrollerItem") as GameObject;
		friendsScroller = new DragPanel ("FriendSelectScroller", friendItem);
		friendsScroller.CreatUI();
		friendsScroller.AddItem (13);
		friendsScroller.RootObject.SetItemWidth(140);
		
		friendsScroller.RootObject.gameObject.transform.parent = gameObject.transform.FindChild("ScrollView");
		friendsScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		friendsScroller.RootObject.gameObject.transform.localPosition = -115*Vector3.up;
		
		for(int i = 0; i < friendsScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(friendsScroller.ScrollItem[ i ].gameObject).onClick = PickFriend;
		}
	}
	private void ShowPartyFriend()
	{
//		friendSprite.spriteName = string.Empty;
	}
	private void ShowPartyUnit()
	{
		for( int i = 0; i < partySpriteList.Count; i++ )
		{
			if(UIConfig.PlayerParty[ currentPartyIndex - 1, i ] == string.Empty)
			{
				//Debug.LogError("Party " + currentPartyIndex + " 's " + i + "th Unit is NOT exist!");
				partySpriteList[ i ].spriteName = string.Empty;
				continue;
			}
			partySpriteList[ i ].spriteName = UIConfig.PlayerParty[ currentPartyIndex - 1, i ];
			//Debug.Log("Sprite[" + i +"]'s name: " + partySpriteList[ i ].spriteName);
		}
	}

	void ShowPartyInfo(Dictionary<int,UnitBaseInfo> name) {
		unitBaseInfo = name;
		if (name == null) {
			foreach (var item in partySprit.Values) {
				item.spriteName = string.Empty;
			}
		} 
		else {
			foreach(var item in partySprit){
			
				if(name.ContainsKey(item.Key)){
					partySprit[item.Key].spriteName = name[item.Key].spriteName;
				}
				else{
					partySprit[item.Key].spriteName = string.Empty;
				}
			}
		}
	}

	void BackParty( GameObject go )
	{
		currentPartyIndex = Mathf.Abs( (currentPartyIndex - 1) % partyTotalCount );
		if( currentPartyIndex == 0 )
			currentPartyIndex = partyTotalCount ;
		labelCurrentPartyIndex.text = currentPartyIndex.ToString();

		SendUnitPage (currentPartyIndex);
	}

	void ForwardParty( GameObject go )
	{
		currentPartyIndex++;
		if (currentPartyIndex > partyTotalCount) {
			currentPartyIndex = initPartyPage;
		} 

		SendUnitPage (currentPartyIndex);
	}
	void ClickCancelBtn(GameObject btn) {

		msgBox.SetActive( false );

	}

	void ClickChooseBtn(GameObject btn) {

		msgBox.SetActive( false );
		btnStart.isEnabled = true;
	}

	void ClickSeeInfoBtn(GameObject btn) {

		msgBox.SetActive( false );
	}

	void ClickStartBtn(GameObject btn) {

		UIManager.Instance.EnterBattle();
	
	}
	
	void PickFriend(GameObject btn)
	{
		msgBox.SetActive( true );
	}

	private void ShowTweenPostion( float mDelay = 0f, UITweener.Method mMethod = UITweener.Method.Linear ) 
	{
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		
		if( list == null )
			return;
		
		foreach( var tweenPos in list)
		{		
			if( tweenPos == null )
				continue;

			Vector3 temp;
			temp = tweenPos.to;
			tweenPos.to = tweenPos.from;
			tweenPos.from = temp;
			
			tweenPos.delay = mDelay;
			tweenPos.method = mMethod;
			
			tweenPos.Reset();
			tweenPos.PlayForward();
			
		}
	}

}
