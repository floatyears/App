using UnityEngine;
using System.Collections.Generic;

public class FriendSupportView : UIComponentUnity/*,IUICallback */{
    private GameObject msgBox;
    private UIImageButton btnStart;
    private UIButton btnSure;
    private UIButton btnCancel;
    private UIButton btnSeeInfo;
    private UILabel labelCurrentPartyIndex;
    private UILabel labelPartyTotalCount;
    private GameObject leftArrowBtn;
    private GameObject rightArrowBtn;
//    private DragPanel friendsScroller;
    private GameObject friendItem;
    private int currentPartyIndex;
    private int partyTotalCount;
    private int initPartyPage = 1;
    private Dictionary<int, UITexture> partySprite = new Dictionary<int,UITexture>();
    private Dictionary<int, UnitBaseInfo> unitBaseInfo = new Dictionary<int, UnitBaseInfo>();
    private UITexture friendSprite;
    private UnitBaseInfo friendBaseInfo;


	
	GameObject dragPanelCell;
	DragPanel dragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	bool exchange = false;
	List<UILabel> crossShowLabelList = new List<UILabel>();
	List<UnitItemViewInfo> supportViewList = new List<UnitItemViewInfo>();

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
	}

	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "CreateDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragView, cbdArgs);
				break;
			case "DestoryDragView": 
				CallBackDispatcherHelper.DispatchCallBack(DestoryDragView, cbdArgs);
                        break;
                default:
				break;
		}
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans", 			transform);
		dragPanelArgs.Add("scrollerScale", 			Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos", 		-105 * Vector3.up);
		dragPanelArgs.Add("position", 					Vector3.zero);
		dragPanelArgs.Add("clipRange", 				new Vector4(0, 0, 640, 220));
		dragPanelArgs.Add("gridArrange", 			UIGrid.Arrangement.Horizontal);
		dragPanelArgs.Add("maxPerLine", 			0);
		dragPanelArgs.Add("scrollBarPosition",		new Vector3(-320, -120, 0));
        dragPanelArgs.Add("cellWidth", 				140);
        dragPanelArgs.Add("cellHeight", 				140);
    }
        
	DragPanel CreateDragPanel(string name, int count){
		DragPanel panel = new DragPanel(name, dragPanelCell);
		panel.CreatUI();
		panel.AddItem(count, dragPanelCell);
		return panel;
	}

	void CreateDragView(object args){
		LogHelper.Log("FriendSelectDecoratorUnity.CreateDragView(), receive call from logic, to create drag list...");
		List<UnitItemViewInfo> viewInfoList = args as List<UnitItemViewInfo>;
		supportViewList = viewInfoList;
		dragPanel = CreateDragPanel("SupportFriendList", viewInfoList.Count);
		FindCrossShowLabelList();

		UpdateAvatarTexture(viewInfoList);
		UpdateEventListener();
		ShowFriendName(viewInfoList);
		//UpdateSupportInfo(viewInfoList);
		UpdateCrossShow();

		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);
	}

	void UpdateCrossShow(){
		if (IsInvoking("CrossShow")){
			CancelInvoke("CrossShow");
		}
		InvokeRepeating("CrossShow", 0f, 1f);
	}

	void UpdateSupportInfo(List<TFriendInfo> friendInfoList){
		Debug.Log("UpdateSupportType(), Start...");
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem [i];
			UILabel typeLabel = scrollItem.transform.FindChild("Label_Friend_Type").GetComponent<UILabel>();
			UILabel pointLabel = scrollItem.transform.FindChild("Label_Friend_Point").GetComponent<UILabel>();

			Debug.Log(string.Format("UpdateSupportType(), Friend[{0}] FriendState is {1} : ", i, friendInfoList[ i ].FriendState.ToString()));
			switch (friendInfoList[ i ].FriendState) {
				case bbproto.EFriendState.FRIENDHELPER : 
					typeLabel.text = "Support";
					typeLabel.color = Color.green;
					pointLabel.color = Color.green;
					break;
				case bbproto.EFriendState.ISFRIEND : 
					typeLabel.text = "Friend";
					typeLabel.color = Color.yellow;
					pointLabel.color = Color.yellow;
					break;
				default:
					typeLabel.text = string.Empty;
					break;
			}
			if(friendInfoList[ i ].FriendPoint != 0){
				pointLabel.text = string.Format("{0}pt", friendInfoList[ i ].FriendPoint.ToString());
			}
			else{
				pointLabel.text = string.Empty;
			}
		}
	}

	void FindCrossShowLabelList(){
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem [i];
			UILabel label = scrollItem.transform.FindChild("Label_Info").GetComponent<UILabel>();
			crossShowLabelList.Add(label);
		}
	}

	void CrossShow(){
		if (exchange){
			for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
				GameObject scrollItem = dragPanel.ScrollItem [i];
				crossShowLabelList [i].text = "Lv" + supportViewList [i].CrossShowTextBefore;
				crossShowLabelList [i].color = Color.yellow;
			}
			exchange = false;
		}
		else{
			for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
				GameObject scrollItem = dragPanel.ScrollItem [i];
				crossShowLabelList [ i ].text = "+" + supportViewList [i].CrossShowTextAfter;
				crossShowLabelList [ i ].color = Color.red;
			}
			exchange = true;
		}
	}


	void ShowFriendName(List<UnitItemViewInfo> friendInfoList){
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem [i];
			UILabel nameLabel = scrollItem.transform.FindChild("Label_Name").GetComponent<UILabel>();
			nameLabel.text = friendInfoList[ i ].DataItem.UnitInfo.Name;
		}
	}

	void ClickItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickItem", dragPanel.ScrollItem.IndexOf(item));
		ExcuteCallback(cbdArgs);
	}
	
	void PressItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("PressItem", dragPanel.ScrollItem.IndexOf(item));
                ExcuteCallback(cbdArgs);
    }
	
	void UpdateEventListener(){
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem [i];
			UIEventListenerCustom.Get(scrollItem).onClick = ClickItem;
            UIEventListenerCustom.Get(scrollItem).LongPress = PressItem;
		}
	}

	void UpdateAvatarTexture(List<UnitItemViewInfo> friendInfoList){
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UITexture uiTexture = scrollItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
            uiTexture.mainTexture = friendInfoList[ i ].Avatar;
		}
	}

    public override void ShowUI() {
        base.ShowUI();

//        ShowTween();
		gameObject.transform.localPosition = new Vector3(-1000, -567, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "easetype", iTween.EaseType.linear));        

//        btnStart.isEnabled = false;
//        friendsScroller.DragPanelView.gameObject.SetActive(true);
    }
	
    public override void HideUI() {
        base.HideUI();
    }
	
    public override void DestoryUI() {
        base.DestoryUI();
    }

    private void InitUI() {
        friendBaseInfo = DataCenter.Instance.FriendBaseInfo;
//        InitPartyLabel();
//        InitPartyArrow();
//        InitPartyUnits();
//        InitMsgBox();
//        InitFriendList();

		dragPanelCell = Resources.Load("Prefabs/UI/Friend/AvailFriendItem") as GameObject;
		InitDragPanelArgs();
    }

    void ClickCancelBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(false);
    }

    void ClickChooseBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(false);
        friendSprite.enabled = true;
        friendSprite.mainTexture = Resources.Load(friendBaseInfo.GetHeadPath) as Texture2D;
//        btnStart.isEnabled = true;
    }

    void ClickSeeInfoBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(false);
    }

    void ClickStartBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//        btnStart.isEnabled = false;

        RequestStartQuest();
    }

    INetBase startQuestNetBase; 
    void RequestStartQuest() {
        if (startQuestNetBase == null) {
            startQuestNetBase = new StartQuest();
        }

        StartQuestParam p = new StartQuestParam();
        p.currPartyId = 0;
        p.questId = 1101;
        p.stageId = 11;
		TFriendInfo tfi = DataCenter.Instance.SupportFriends [0];
		p.helperUserId = tfi.UserId;
		p.helperUniqueId = tfi.UserUnit.ID;
		startQuestNetBase.OnRequest(p, RspStartQuest);
    }

    void RspStartQuest(object data) {
        TQuestDungeonData tqdd = null;
        bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
        if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {

            DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
            DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;

            LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);

            tqdd = new TQuestDungeonData(rspStartQuest.dungeonData);
				
            ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
        }

        if (data == null || tqdd == null) {
            Debug.LogError("Request quest info fail : data " + data + "  TQuestDungeonData : " + tqdd);
            //TODO: show failed window for user to retry
            return;
        }

        UIManager.Instance.EnterBattle();
    } 

	
    void PickFriend(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(true);
    }
		
	void DestoryDragView(object args){
		crossShowLabelList.Clear();
		supportViewList.Clear();

		foreach (var item in dragPanel.ScrollItem){
			GameObject.Destroy(item);
		}
		dragPanel.ScrollItem.Clear();
		GameObject.Destroy(dragPanel.DragPanelView.gameObject);
	}

}
