using UnityEngine;
using System.Collections;

public class ShowBottomInfo : MonoBehaviour {
	public static float scaleTime = 0.5f;
	public static float showTime = 0.5f;
	private UILabel typeLabel;
	private UILabel nameLabel;
	private UILabel cateGoryLabel;
	private UISprite itemSprite;

	private UILabel nameTitleLabel;
	private UILabel categoryTitleLabel;

	private const string categoryTitle = "Category: ";
	private const string coinTitle = "Number: ";

	void Awake() {
		nameTitleLabel = transform.Find("NameTitleLabel").GetComponent<UILabel>();
		categoryTitleLabel = transform.Find("CategoryTitleLabel").GetComponent<UILabel>();
		typeLabel = transform.Find( "TypeLabel").GetComponent<UILabel>();
		nameLabel = transform.Find ("NameLabel").GetComponent<UILabel> ();
		cateGoryLabel = transform.Find( "CatagoryLabel").GetComponent<UILabel>();
		itemSprite = transform.Find ( "Trap").GetComponent<UISprite> ();

		gameObject.layer = GameLayer.BottomInfo;
	}

	void OnEnable() {
		MsgCenter.Instance.AddListener (CommandEnum.ShowTrap, ShowTrap);
		MsgCenter.Instance.AddListener (CommandEnum.ShowCoin, ShowCoin);
	}

	void OnDisable() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowTrap, ShowTrap);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowCoin, ShowCoin);
	}

	void OnDestory() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowTrap, ShowTrap);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowCoin, ShowCoin);
	}


	void ShowTrap(object data) {
		TrapBase tb = data as TrapBase;
		if (tb == null) {
			return;	
		}

		if (!nameTitleLabel.enabled) {
			nameTitleLabel.enabled = true;		
		}

		categoryTitleLabel.text = categoryTitle;
		typeLabel.text = "Trap";
		itemSprite.spriteName = BattleMap.trapSpriteName; //tb.GetTrapSpriteName ();
		nameLabel.text = tb.GetItemName ();
		cateGoryLabel.text = tb.GetTypeName () + " : Lv." + tb.GetLevel;
		TweenAnim ();
	}

	void ShowCoin(object coin) {
		int number = (int)coin;
		nameLabel.text = "";
		typeLabel.text = "Coin";
		itemSprite.spriteName = BattleMap.chestSpriteName; // S  is coin sprite name in atlas.
		cateGoryLabel.text = number.ToString ();
		categoryTitleLabel.text = coinTitle;
		if (nameTitleLabel.enabled) {
			nameTitleLabel.enabled = false;
		}

		TweenAnim ();
	}

	void TweenAnim() {
		iTween.ScaleTo(gameObject,iTween.Hash("y", 1f, "time", scaleTime,"oncompletetarget",gameObject,"oncomplete","ShowEnd"));
	}

	void SetTitleLabel(bool b) {
		
	}

	void ShowEnd() {
		GameTimer.GetInstance ().AddCountDown (showTime, InfoEnd);
	}

	void InfoEnd() {
		nameLabel.text = "";
		cateGoryLabel.text = "";
		transform.localScale = new Vector3 (1f, 0f, 1f);
	}
}
