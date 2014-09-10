using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleTopView : ViewBase {
	[HideInInspector]
	public UILabel coinLabel;
	private UILabel dropLabel;
	private UILabel floorLabel;
	private UIButton menuButton;

	private UIButton retryButton;

	[HideInInspector]
	public BattleMapModule battleQuest;

//	private BattleMenu battleMenu;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);
//	}
//		base.Init (name);

		coinLabel = FindChild<UILabel> ("Top/CoinLabel");
		dropLabel = FindChild<UILabel> ("Top/DropLabel");
		floorLabel = FindChild<UILabel> ("Top/FloorLabel");
		retryButton = FindChild<UIButton>("Top/RetryButton");
		UIEventListener.Get (retryButton.gameObject).onClick = Retry;

		menuButton = FindChild<UIButton>("Top/MenuButton");
		UIEventListener.Get (menuButton.gameObject).onClick = ShowMenu;
	}

	public override void HideUI () {
		if(gameObject.activeSelf)
			gameObject.SetActive (false);
	}

	public override void ShowUI () {
		if (!gameObject.activeSelf) {
			gameObject.SetActive (true);	
			Coin= 0;
			Drop = 0;
		}
			
	}

	public override void DestoryUI () {
		base.DestoryUI ();
//		if(battleMenu != null)
//			battleMenu.DestoryUI ();
	}
	
	public int Coin {
		set { if(coinLabel != null) coinLabel.text = value.ToString(); }
	}

	public int Drop {
		set { if(dropLabel != null) dropLabel.text = value.ToString(); }
	}

	public void SetFloor () {
		int currentFloor = ConfigBattleUseData.Instance.questDungeonData.currentFloor + 1;
		int maxFloor = ConfigBattleUseData.Instance.questDungeonData.Floors.Count;
//		Debug.LogError ("top ui set floor : " + currentFloor);
		if(floorLabel != null)
			floorLabel.text = currentFloor + "/" + maxFloor + "F";
	}

	public void RefreshTopUI() {
		int coin = 0;
		int drop = 0;
		foreach (var item in ConfigBattleUseData.Instance.storeBattleData.questData) {
			coin += item.getMoney;
			drop += item.getUnit.Count;
		}
		Coin = coin;
		Drop = drop;
		SetFloor ();
	}

	public void Reset() {
		coinLabel.text = "";
		dropLabel.text = "";
		SheildInput (true);
	}

	public void SheildInput(bool b) {
		retryButton.isEnabled = b;
		menuButton.isEnabled = b;
	}

	void Retry(GameObject go) {
#if !UNITY_EDITOR
		if (NoviceGuideStepEntityManager.isInNoviceGuide()) {
			return;	
		}
#endif

		battleQuest.Retry ();
	}

	void ShowMenu (GameObject go) {
#if !UNITY_EDITOR
		if (NoviceGuideStepEntityManager.isInNoviceGuide()) {
			return;	
		}
#endif

//		if (battleMenu == null) {
//			CreatBattleMenu ();
//			return;
//		}

//		if (!battleMenu.gameObject.activeSelf) {
//			battleMenu.ShowUI ();
//		} else {
//			battleMenu.HideUI ();
//		}
	}

	void CreatBattleMenu () {
		 ResourceManager.Instance.LoadLocalAsset ("Prefabs/BattleMenu", o => {
			GameObject go = o as GameObject;
			go = NGUITools.AddChild (ViewManager.Instance.CenterPanel, go);
			go.transform.localPosition = new Vector3 (0f, 0f, 0f);
			go.layer = GameLayer.BottomInfo;
//			battleMenu = go.GetComponent<BattleMenu> ();
//			battleMenu.battleQuest = battleQuest;
////			battleMenu.Init ("BattleMenu");
//			battleMenu.ShowUI();
		});

	}

}
