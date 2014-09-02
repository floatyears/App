using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class RewardView : ViewBase {

	public static List<int> bonusIDs = new List<int>();

	private DragPanel dragPanel;

	private Dictionary<int,List<BonusInfo>> aList = new Dictionary<int, List<BonusInfo>>();

	private int currentContentIndex = 0;

	private GameObject content;

	private GameObject OKBtn;

	private Dictionary<int,GameObject> Nums;

	UILabel tabInfo;


	public override void Init(UIConfigItem config) {
		base.Init(config);
		InitUI();
	}
	
	public override void ShowUI() {
		base.ShowUI();

		bonusIDs.Clear ();

		InitData ();

		RefreshView ();

//		ShowUIAnimation ();

//		GetBonusList.SendRequest (OnRequest);
	}

//	void OnRequest(object data){
//			//		bbproto.BonusInfo bsInfo = 
//		Debug.Log ("purchase success, change to reward. rsp data:"+data);
//		bbproto.RspBonusList rsp = data as bbproto.RspBonusList;
//		if (rsp != null && rsp.bonus != null ) {
//			DataCenter.Instance.LoginInfo.Bonus = rsp.bonus;
//
//
////			MsgCenter.Instance.Invoke(CommandEnum.GotoRewardMonthCardTab);
////			ModuleManger.Instance.ShowModule (ModuleEnum.Reward);
//		}
//	}

	public override void HideUI() {
		base.HideUI();


//		Debug.Log ("bonusIDs: " + bonusIDs.Count);
		if(bonusIDs.Count > 0)
			AcceptBonus.SendRequest(OnAcceptBonus,bonusIDs);

		int count = dragPanel.ScrollItem.Count;
		for (int i = 0; i < count; i++) {
			GameObject go = dragPanel.ScrollItem[i];
			GameObject.Destroy(go);
		}
		dragPanel.ScrollItem.Clear();
		
//		iTween.Stop (gameObject);

		aList.Clear ();

//		base.DestoryUI ();
	}

	private void OnAcceptBonus(object data){
		RspAcceptBonus rsp = data as RspAcceptBonus;

		if(rsp.header.code == ErrorCode.SUCCESS)
		{
			foreach (var num in bonusIDs) {
				for (int i = DataCenter.Instance.LoginInfo.Bonus.Count - 1; i >= 0; i--) {
					if(DataCenter.Instance.LoginInfo.Bonus[i].id == num){
						DataCenter.Instance.LoginInfo.Bonus.RemoveAt(i);
						continue;
					}
				}
			}
			DataCenter.Instance.AccountInfo.Stone = rsp.stone;
			DataCenter.Instance.AccountInfo.Money = rsp.money;
			DataCenter.Instance.AccountInfo.FriendPoint = rsp.friendPoint;
			DataCenter.Instance.UserUnitList.AddMyUnitList(rsp.newUnitList);

			MsgCenter.Instance.Invoke(CommandEnum.SyncChips);
			
//			MsgCenter.Instance.Invoke(CommandEnum.SyncStamina);
			MsgCenter.Instance.Invoke(CommandEnum.RefreshPlayerCoin);

			MsgCenter.Instance.Invoke (CommandEnum.RefreshRewardList);
		}
		bonusIDs.Clear ();

	} 

	public override void DestoryUI () {

		dragPanel.DestoryUI ();

		base.DestoryUI ();

		UIEventListenerCustom.Get (OKBtn).onClick -= OnClickOK;
		MsgCenter.Instance.RemoveListener (CommandEnum.TakeAward, OnTakeAward);
		MsgCenter.Instance.RemoveListener (CommandEnum.GotoRewardMonthCardTab, OnGotoTab);
	}

	private void InitUI(){
		FindUIElement ();

		InitData ();

		CreateDragView ();


		UIEventListenerCustom.Get (OKBtn).onClick += OnClickOK;

		MsgCenter.Instance.AddListener (CommandEnum.TakeAward, OnTakeAward);
		MsgCenter.Instance.AddListener (CommandEnum.GotoRewardMonthCardTab, OnGotoTab);
	}

	private void InitData(){
		aList.Clear ();
		foreach (var item in DataCenter.Instance.LoginInfo.Bonus) {
			if(item.type <= 3){

				if(!aList.ContainsKey(item.type))
					aList[item.type] = new List<BonusInfo>();
				aList[item.type].Add(item);
			}else if(item.type == 4){
				if(!aList.ContainsKey(5))
					aList[5] = new List<BonusInfo>();
				aList[5].Add(item);
			}else if(item.type >4){
				if(!aList.ContainsKey(4))
					aList[4] = new List<BonusInfo>();
				aList[4].Add(item);
			}
		}
	}

	void OnClickOK(GameObject obj){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );

		ModuleManger.Instance.ShowModule (UIManager.Instance.current.CurrentDecoratorScene);
//		HideUI ();
	}


	private void CreateDragView(){
		dragPanel = new DragPanel("RewardDragPanel", RewardItemView.Prefab);
//		dragPanel.CreatUI();
		dragPanel.AddItem (0);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.RewardListDragPanelArgs, content.transform);
	}

	private void FindUIElement(){
		content = FindChild ("Content");
		OKBtn = FindChild ("OkBtn");

		FindChild<UILabel> ("OkBtn/Label").text = TextCenter.GetText("OK");
		FindChild<UILabel> ("1/Label").text = TextCenter.GetText ("Reward_Tab1");
		FindChild<UILabel> ("2/Label").text = TextCenter.GetText ("Reward_Tab2");
		FindChild<UILabel> ("3/Label").text = TextCenter.GetText ("Reward_Tab3");
		FindChild<UILabel> ("4/Label").text = TextCenter.GetText ("Reward_Tab4");
		FindChild<UILabel> ("5/Label").text = TextCenter.GetText ("Reward_Tab5");

		Nums = new Dictionary<int, GameObject> ();

		Nums.Add (1, FindChild ("1/Num"));
		Nums.Add (2, FindChild ("2/Num"));
	 	Nums.Add (3, FindChild ("3/Num"));
		Nums.Add (4, FindChild ("4/Num"));
  		Nums.Add (5, FindChild ("5/Num"));

		FindChild<UILabel> ("Title").text = TextCenter.GetText ("Reward_Title");

		tabInfo = FindChild<UILabel> ("Info");
	}

	private void RefreshView(){

		for (int i = 1; i < 6; i++) {
			int count = 0;
			if(aList.ContainsKey(i)){
				foreach (var item in aList[i]) {
					if(item.enabled == 1){
						count++;
					}
				}
			}
			
			if(count > 0){
				Nums[i].SetActive(true);
//				UIToggle.
				Nums[i].transform.Find("Label").gameObject.GetComponent<UILabel>().text = count+"";
			}else{
				Nums[i].SetActive(false);
			}
		}

		if (!aList.ContainsKey (currentContentIndex)) {
			//Debug.Log("item:" + dragPanel.ScrollItem);
			int count = dragPanel.ScrollItem.Count;
			for (int i = 0; i < count; i++) {
				GameObject go = dragPanel.ScrollItem[i];
				GameObject.Destroy(go);
			}
			dragPanel.ScrollItem.Clear();
			return;
		}

		//dragPanel.AddItem (aList [currentContentIndex].Count, null, true);
		//Debug.Log ("current count: " + aList [currentContentIndex].Count);

		for (int i = 0; i < aList[currentContentIndex].Count; i++){
			//Debug.Log ("scroll item count:" + dragPanel.ScrollItem.Count+ " i: "+ i);
			if(dragPanel.ScrollItem.Count <= i){
				GameObject go = dragPanel.DragPanelView.AddObject(dragPanel.SetResourceObject);
				//Debug.Log("go: " + go);
				if(go != null){
					dragPanel.ScrollItem.Add(go);
				}
			}
			//Debug.Log("index: " + i);
			RewardItemView rewardItemView = RewardItemView.Inject(dragPanel.ScrollItem[ i ]);
			
			rewardItemView.Data = aList[currentContentIndex][i];
		}

		//Debug.Log ("count: " + dragPanel.ScrollItem.Count + " ,"+ aList [currentContentIndex].Count);
		if (dragPanel.ScrollItem.Count > aList [currentContentIndex].Count) {
			for (int i = dragPanel.ScrollItem.Count-1; i >= aList [currentContentIndex].Count; i--) {
				GameObject go = dragPanel.ScrollItem[i];
				GameObject.Destroy(go);
				dragPanel.ScrollItem.RemoveAt(i);
			}
		}



		dragPanel.Refresh ();
		dragPanel.DragPanelView.scrollBar.value = 0;
	}


	private void OnTakeAward(object data){

		aList [currentContentIndex].Remove (data as BonusInfo);
		RefreshView ();
	}


	void Update(){
		int i;
		int.TryParse(UIToggle.GetActiveToggle (3).name,out i);
		if (currentContentIndex != i) {
			currentContentIndex = i;

			RefreshView();
		}
	}

	public void ShowTabInfo(object data){
		UIToggle toggle = UIToggle.GetActiveToggle (3);
		//Debug.Log ("tab info: " + data);
//		Debug.Log ("toggle: " + toggle);
		if (toggle != null) {
			tabInfo.text = TextCenter.GetText ("Reward_Tab_Info" + toggle.ToString().Substring(0,1));
		}
	}

	
	private void OnGotoTab(object data){
		for(int i = 1; i < 6; i++){
			if(i == (int)data){
				transform.FindChild(i+"").GetComponent<UIToggle>().startsActive = true;
				transform.FindChild(i+"").SendMessage("OnClick");
			}else{
				transform.FindChild(i+"").GetComponent<UIToggle>().startsActive = false;
			}

		}
	}
}
