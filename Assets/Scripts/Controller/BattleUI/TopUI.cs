using UnityEngine;
using System.Collections;

public class TopUI : UIBaseUnity {
	private UILabel coinLabel;
	private UILabel dropLabel;
	private UILabel floorLabel;

	private UIAnchor leftAnchor;
	private UIAnchor rightAnchor;

	public override void Init (string name) {
		base.Init (name);

		coinLabel = FindChild<UILabel> ("Topleft/CoinLabel");
		dropLabel = FindChild<UILabel> ("Topleft/DropLabel");
		floorLabel = FindChild<UILabel> ("TopRight/FloorLabel");

		UISprite  sprite = FindChild<UISprite>("TopRight/Sprite");
		UIEventListener.Get (sprite.gameObject).onClick = ShowMenu;

		leftAnchor = transform.Find ("Topleft").gameObject.AddComponent<UIAnchor> ();
		leftAnchor.side = UIAnchor.Side.TopLeft;
		leftAnchor.runOnlyOnce = false;
		leftAnchor.enabled = true;

		rightAnchor = transform.Find ("TopRight").gameObject.AddComponent<UIAnchor> ();
		rightAnchor.side = UIAnchor.Side.TopRight;
		rightAnchor.runOnlyOnce = false;
		rightAnchor.enabled = true;

		StartCoroutine (Set ());
	}

	IEnumerator Set () {
		yield return 0;
		leftAnchor.runOnlyOnce = true;
		rightAnchor.runOnlyOnce = true;
	}

	public override void HideUI () {
		if(gameObject.activeSelf)
			gameObject.SetActive (false);
	}

	public override void ShowUI () {
		if(!gameObject.activeSelf)
			gameObject.SetActive (true);
	}

	public override void DestoryUI () {
		Destroy (gameObject);
	}
	
	public int Coin {
		set { if(coinLabel != null) coinLabel.text = value.ToString(); }
	}

	public int Drop {
		set { if(dropLabel != null) dropLabel.text = value.ToString(); }
	}

	public void SetFloor (int currentFloor, int maxFloor) {
		if(floorLabel != null)
			floorLabel.text = currentFloor + "/" + maxFloor + "F";
	}

	public void RefreshTopUI(TQuestDungeonData questData, ClearQuestParam questGet) {
		Coin = questGet.getMoney;
		Drop = questGet.getUnit.Count;
		SetFloor (questData.currentFloor + 1, questData.Floors.Count);
	}

	void ShowMenu (GameObject go) {
		//TODO show fight menu.
	}
}
