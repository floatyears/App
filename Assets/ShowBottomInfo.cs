using UnityEngine;
using System.Collections;

public class ShowBottomInfo : MonoBehaviour {
	public static float scaleTime = 0.3f;
	public static float showTime = 0.5f;
	private UILabel typeLabel;
	private UILabel nameLabel;
	private UILabel cateGoryLabel;
	private UISprite itemSprite;

	void Awake() {
//		string parent = "BottomInfo/";
		typeLabel = transform.Find( "TypeLabel").GetComponent<UILabel>();
		nameLabel = transform.Find ("NameLabel").GetComponent<UILabel> ();
		cateGoryLabel = transform.Find( "CatagoryLabel").GetComponent<UILabel>();
		itemSprite = transform.Find ( "Trap").GetComponent<UISprite> ();
	}

	void OnEnalbe() {
		MsgCenter.Instance.AddListener (CommandEnum.ShowTrap, ShowTrap);
	}

	void OnDisabel() {
		MsgCenter.Instance.AddListener (CommandEnum.ShowTrap, ShowTrap);
	}

	void OnDestory() {
		MsgCenter.Instance.AddListener (CommandEnum.ShowTrap, ShowTrap);
	}

	void ShowTrap(object data) {
		TrapBase tb = data as TrapBase;
		if (tb == null) {
			return;	
		}

		typeLabel.text = "Trap";
		itemSprite.spriteName = tb.GetTrapSpriteName ();
		nameLabel.text = tb.GetItemName ();
		cateGoryLabel.text = tb.GetTypeName () + " : Lv." + tb.GetLevel;
		iTween.ScaleTo(gameObject,iTween.Hash("y", 1f, "time", scaleTime,"oncompletetarget",gameObject,"oncomplete","ShowEnd"));
	}

	void ShowEnd() {
		GameTimer.GetInstance ().AddCountDown (showTime, InfoEnd);
	}

	void InfoEnd() {
		transform.localScale = new Vector3 (1f, 0f, 1f);
	}
}
