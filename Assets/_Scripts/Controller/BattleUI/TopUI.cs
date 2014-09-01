using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopUI : ViewBase {
	[HideInInspector]
	public UILabel coinLabel;
	private UILabel dropLabel;
	private UILabel floorLabel;
	private UIButton menuButton;

	private UIButton retryButton;

	[HideInInspector]
	public BattleQuest battleQuest;

	private BattleMenu battleMenu;

	public override void Init (UIConfigItem config)
	{
		base.Init (config);
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
		if(battleMenu != null)
			battleMenu.DestoryUI ();
	}
	
	public int Coin {
		set { if(coinLabel != null) coinLabel.text = value.ToString(); }
	}

	public int Drop {
		set { if(dropLabel != null) dropLabel.text = value.ToString(); }
	}

	public void SetFloor (int currentFloor, int maxFloor) {
//		Debug.LogError ("top ui set floor : " + currentFloor);
		if(floorLabel != null)
			floorLabel.text = currentFloor + "/" + maxFloor + "F";
	}

	public void RefreshTopUI(TQuestDungeonData questData, List<TClearQuestParam> questGet) {
		int coin = 0;
		int drop = 0;
		foreach (var item in questGet) {
			coin += item.getMoney;
			drop += item.getUnit.Count;
		}
		Coin = coin;
		Drop = drop;
		SetFloor (questData.currentFloor + 1, questData.Floors.Count);
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

		if (battleMenu == null) {
			CreatBattleMenu ();
			return;
		}

		if (!battleMenu.gameObject.activeSelf) {
			battleMenu.ShowUI ();
		} else {
			battleMenu.HideUI ();
		}
	}

	void CreatBattleMenu () {
		 ResourceManager.Instance.LoadLocalAsset ("Prefabs/BattleMenu", o => {
			GameObject go = o as GameObject;
			go = NGUITools.AddChild (ViewManager.Instance.CenterPanel, go);
			go.transform.localPosition = new Vector3 (0f, 0f, 0f);
			go.layer = GameLayer.BottomInfo;
			battleMenu = go.GetComponent<BattleMenu> ();
			battleMenu.battleQuest = battleQuest;
//			battleMenu.Init ("BattleMenu");
			battleMenu.ShowUI();
		});

	}
}
