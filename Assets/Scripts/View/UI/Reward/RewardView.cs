using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class RewardView : UIComponentUnity {

	public static List<int> bonusIDs = new List<int>();

	private DragPanel dragPanel;

	private Dictionary<int,List<BonusInfo>> aList = new Dictionary<int, List<BonusInfo>>();

	private int currentContentIndex = 0;

	private GameObject content;

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
	}
	
	public override void ShowUI() {
		base.ShowUI();

		bonusIDs.Clear ();
	}
	
	public override void HideUI() {
		base.HideUI();

//		Debug.Log ("bonusIDs: " + bonusIDs.Count);
		if(bonusIDs.Count > 0)
			AcceptBonus.SendRequest(null,bonusIDs);
		bonusIDs.Clear ();
	}
	
	public override void DestoryUI () {
		aList.Clear ();
		dragPanel.DestoryUI ();

		base.DestoryUI ();
	}

	private void InitUI(){
		FindUIElement ();
		InitData ();
		CreateDragView ();
		RefreshView ();

		MsgCenter.Instance.AddListener (CommandEnum.TakeAward, OnTakeAward);
	}

	private void InitData(){
		foreach (var item in DataCenter.Instance.LoginInfo.Bonus) {
			if(item.type <= 3){

				if(!aList.ContainsKey(item.type))
					aList[item.type] = new List<BonusInfo>();
				aList[item.type].Add(item);
			}else{
				if(!aList.ContainsKey(4))
					aList[4] = new List<BonusInfo>();
				aList[4].Add(item);
			}

		}
	}

	private void CreateDragView(){
		dragPanel = new DragPanel("RewardDragPanel", RewardItemView.Prefab);
		dragPanel.CreatUI();
		dragPanel.AddItem (0);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.RewardListDragPanelArgs, content.transform);
	}

	private void FindUIElement(){
		content = FindChild ("Content");
	}

	private void RefreshView(){
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
}
