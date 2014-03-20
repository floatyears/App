using UnityEngine;
using System.Collections.Generic;

public class FriendHelperView : UIComponentUnity{
    private GameObject msgBox;

    UIImageButton bottomButton;

    private UIButton btnSure;
    private UIButton btnCancel;
    private UIButton btnSeeInfo;
    private UILabel labelCurrentPartyIndex;
    private UILabel labelPartyTotalCount;
    private GameObject leftArrowBtn;
    private GameObject rightArrowBtn;
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
	
	public override void ShowUI() {
		base.ShowUI();
		
		gameObject.transform.localPosition = new Vector3(-1000, -567, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "easetype", iTween.EaseType.linear));        
		
		SetBottomButtonActive(false);
	}
	
	public override void HideUI() {
		base.HideUI();
		SetBottomButtonActive(false);
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
			case "EnableBottomButton":
				CallBackDispatcherHelper.DispatchCallBack(EnableBottomButton, cbdArgs);
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
		Debug.Log("FriendSelectDecoratorUnity.CreateDragView(), receive call from logic, to create drag list...");
		List<UnitItemViewInfo> viewInfoList = args as List<UnitItemViewInfo>;
		supportViewList = viewInfoList;
		dragPanel = CreateDragPanel("SupportFriendList", viewInfoList.Count);
		FindCrossShowLabelList();

		UpdateAvatarTexture(viewInfoList);
		UpdateEventListener();
		ShowFriendName(viewInfoList);
		UpdateSupportInfo(viewInfoList);
		UpdateCrossShow();

		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);
	}

	void UpdateCrossShow(){
		if (IsInvoking("CrossShow")){
			CancelInvoke("CrossShow");
		}
		InvokeRepeating("CrossShow", 0f, 1f);
	}

	void UpdateSupportInfo(List<UnitItemViewInfo> friendInfoList){
		Debug.Log("UpdateSupportType(), Start...");
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem [i];
			UILabel typeLabel = scrollItem.transform.FindChild("Label_Friend_Type").GetComponent<UILabel>();
			UILabel pointLabel = scrollItem.transform.FindChild("Label_Friend_Point").GetComponent<UILabel>();

			switch (friendInfoList[ i ].HelperItem.FriendState) {
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
			if(friendInfoList[ i ].HelperItem.FriendPoint != 0){
				pointLabel.text = string.Format("{0}pt", friendInfoList[ i ].HelperItem.FriendPoint.ToString());
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


    private void InitUI() {
        friendBaseInfo = DataCenter.Instance.FriendBaseInfo;
		bottomButton = FindChild<UIImageButton>("Button_QuestStart");
		//bottomButton.isEnabled = false;

		dragPanelCell = Resources.Load("Prefabs/UI/Friend/AvailFriendItem") as GameObject;
		InitDragPanelArgs();
    }

	void EnableBottomButton(object args){
		bottomButton.isEnabled = true;
		UIEventListener.Get(bottomButton.gameObject).onClick = ClickBottomButton;

	}

	void ClickBottomButton(GameObject btn){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickBottomButton", null);
		bottomButton.isEnabled = false;
		ExcuteCallback(cbdArgs);
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
    }

    void ClickSeeInfoBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        msgBox.SetActive(false);
    }

    void ClickStartBtn(GameObject btn) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//        RequestStartQuest();
    }

//    INetBase startQuestNetBase; 
//    void RequestStartQuest() {
//        if (startQuestNetBase == null) {
//            startQuestNetBase = new StartQuest();
//        }
//
//    }

   

	
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

	void SetBottomButtonActive(bool active){
		bottomButton.isEnabled = active;
	}

}
