using UnityEngine;
using System.Collections.Generic;

public class FriendHelperView : UIComponentUnity{
	protected DragPanel dragPanel;
	//protected UIButton sortBtn;
	protected UILabel sortRuleLabel;
	protected SortRule curSortRule;

	protected List<TFriendInfo> helperDataList = new List<TFriendInfo>();
	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
	}
	
	public override void ShowUI() {
		base.ShowUI();
		AddCmdListener();
		CreateDragView();
		ShowUIAnimation();
	}

	public override void HideUI() {
		base.HideUI();
		dragPanel.DestoryUI();
		RmvCmdListener();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	private void InitUI(){
//		sortBtn = transform.FindChild("Button_Sort").GetComponent<UIButton>();
//		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;
		//sortRuleLabel = transform.FindChild("Label_Sort_Rule").GetComponent<UILabel>();

		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		//sortRuleLabel.text = curSortRule.ToString();
	}

	private void CreateDragView(){
		helperDataList = DataCenter.Instance.SupportFriends;
		dragPanel = new DragPanel("FriendHelperDragPanel", HelperUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(helperDataList.Count);
		CustomDragPanel();
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.HelperListDragPanelArgs, transform);

		SortUnitByCurRule();
	}

	void RefreshView() {
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			HelperUnitItem huv = HelperUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			huv.Init(helperDataList[ i ]);
			huv.callback = ClickHelperItem;
		}
	}
	
	private QuestItemView pickedQuestInfo;
	private void RecordPickedInfoForFight(object msg){
		//Debug.Log("FriendHelper.RecordPickedInfoForFight(), received info...");
		pickedQuestInfo = msg as QuestItemView;
	}

	protected virtual void ClickHelperItem(HelperUnitItem item){
		Debug.Log("ClickHelperItem..." + item);
		
		if(pickedQuestInfo == null){
			Debug.LogError("FriendHelerpView.ClickHelperItem(), pickedQuestInfo is NULL, return!!!");
			return;
		}

		if(CheckStaminaEnough()){
			Debug.LogError("TurnToFriendSelect()......Stamina is not enough, MsgWindow show...");
			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaLackMsgParams());
			return;
		}

		Dictionary<string, object> pickedInfo = new Dictionary<string, object>();
		pickedInfo.Add("QuestInfo", pickedQuestInfo);
		pickedInfo.Add("HelperInfo", item.FriendInfo);

		UIManager.Instance.ChangeScene(SceneEnum.StandBy);//before
		MsgCenter.Instance.Invoke(CommandEnum.OnPickHelper, pickedInfo);//after
	}

	/// <summary>
	/// Checks the stamina enough.
	/// MsgWindow show, note stamina is not enough.
	/// </summary>
	/// <returns><c>true</c>, if stamina enough was checked, <c>false</c> otherwise.</returns>
	/// <param name="staminaNeed">Stamina need.</param>
	/// <param name="staminaNow">Stamina now.</param>
	private bool CheckStaminaEnough(){
		int staminaNeed = pickedQuestInfo.Data.Stamina;
		int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;
		if(staminaNeed > staminaNow) return true;
		else return false;
	}
	
	private MsgWindowParams GetStaminaLackMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.Instace.GetCurrentText("StaminaLackNoteTitle");
		msgParams.contentText = TextCenter.Instace.GetCurrentText("StaminaLackNoteContent");
		msgParams.btnParam = new BtnParam();
		return msgParams;
	}
	
//	private void ClickSortBtn(GameObject btn){
//		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
//
//		//can not clicked before compelete showing sort panel
//		sortBtn.isEnabled = false;
//	}
	
	private void ReceiveSortInfo(object msg){
		//Debug.LogError("FriendHelper.ReceiveSortInfo()...");
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}
	
	private void SortUnitByCurRule(){
		//sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, helperDataList);
		RefreshView();
	}

	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));       
	}
	
	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.OnPickQuest, RecordPickedInfoForFight);
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
//		MsgCenter.Instance.AddListener(CommandEnum.ActivateSortBtn, ActivateSortBtn);
	}

	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickQuest, RecordPickedInfoForFight);
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

//	private void ActivateSortBtn(object msg){
//		Debug.Log("FriendHelperView.ActivateSortBtn(), receive msg to activate sort btn...");
//		sortBtn.isEnabled = true;
//	}

	/// <summary>
	/// Customs the drag panel.
	/// Custom this drag panel as vertical drag.
	/// </summary>
	private void CustomDragPanel(){
		GameObject scrollView = dragPanel.DragPanelView.transform.FindChild("Scroll View").gameObject;
		GameObject scrollBar = dragPanel.DragPanelView.transform.FindChild("Scroll Bar").gameObject;
		GameObject itemRoot = scrollView.transform.FindChild("UIGrid").gameObject;
		//scrollBar.transform.Rotate( new Vector3(0, 0, 270) );
		//scrollBar.gameObject.SetActive(false);
		UIScrollView uiScrollView = scrollView.GetComponent<UIScrollView>();
		UIScrollBar uiScrollBar = scrollBar.GetComponent<UIScrollBar>();

		uiScrollView.verticalScrollBar = uiScrollBar;
		uiScrollView.horizontalScrollBar = null;
	}

	    
}
