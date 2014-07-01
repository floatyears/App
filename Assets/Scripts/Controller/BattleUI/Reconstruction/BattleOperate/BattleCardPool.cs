using UnityEngine;
using System.Collections;

public class BattleCardPool : UIBaseUnity
{
	[HideInInspector]
	public UISprite templateBackTexture ;

	private Vector3[] cardPosition;

	public Vector3[] CardPosition {
		get{return cardPosition;}
	}

	private UISprite[] backTextureIns;
	private int cardInterv = 0;
	private Vector3 initPosition = Vector3.zero;
	private float xStart = 0f;

	public float XRange {
		set { xStart = transform.localPosition.x - value / 2f; }
	}

	public override void Init (string name) {
		base.Init (name);
		InitData();
		for (int i = 0; i < cardPosition.Length; i++) {
			tempObject = NGUITools.AddChild(gameObject, templateBackTexture.gameObject);
			cardPosition[i] = new Vector3(initPosition.x + i * cardInterv,initPosition.y,initPosition.z);
//			Debug.LogError("initposition : " + initPosition + " cardPosition[i] : " + cardPosition[i]);
			tempObject.transform.localPosition = cardPosition[i];
			backTextureIns[i] = tempObject.GetComponent<UISprite>();
		}
		templateBackTexture.gameObject.SetActive(false);
		tempObject = null;
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive(true);
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive(false);
	}

	void InitData() {
		int count = Config.cardPoolSingle;
		cardPosition = new Vector3[count];
		backTextureIns = new UISprite[count];		
		templateBackTexture = FindChild<UISprite>("Back");
		cardInterv = templateBackTexture.width + Config.cardInterv;
		initPosition = Config.cardPoolInitPosition;
	}

	public void IgnoreCollider(bool isIgnore)
	{
		if(isIgnore)
			gameObject.layer = GameLayer.IgnoreCard;
		else
			gameObject.layer = GameLayer.ActorCard;
	}

	public int CaculateSortIndex(Vector3 point)
	{
		float x = point.x - xStart;

		for (int i = 0; i < cardPosition.Length; i++) 
		{
			if(x > cardInterv * i && x <= cardInterv * (i + 1))
				return i;
		}

		return -1;
	}
}
