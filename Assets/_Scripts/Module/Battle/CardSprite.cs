using UnityEngine;
using System.Collections;

public class CardSprite : ViewBase 
{
	public event UICallback<CardSprite> tweenCallback;
	
	private UISprite actorSprite;
	
	public UISprite ActorSprite {
		get{return actorSprite;}
	}
	
	private TweenPosition tweenPosition;
	private TweenScaleExtend tse;

	
	private Vector3 initPosition;
	public Vector3 InitPosition {
		set{initPosition = value;}
	}
	
	private int initDepth;
	
	private Transform parentObject;
	
	private float xOffset = 0f;
	
	private float defaultMoveTime = 0.1f;
	
	[HideInInspector]
	public int itemID = -1;	
	[HideInInspector]
	public int location = -1;

	public override void Init (UIConfigItem config)
	{
		base.Init (config);
		parentObject = transform.parent;
		actorSprite = GetComponent<UISprite>();
		if (actorSprite.enabled) {
			actorSprite.spriteName = "";
		}
//		Debug.LogError ("actorSprite.width  : " + actorSprite.width);
		tweenPosition = GetComponent<TweenPosition>();
		if (tweenPosition.enabled) {
			tweenPosition.enabled = false;	
		}
		tse = GetComponent<TweenScaleExtend> ();
		if (tse.enabled) {
			tse.enabled = false;	
		}
		tweenPosition.eventReceiver = gameObject;
		tweenPosition.callWhenFinished = "TweenPositionCallback";
		initPosition = actorSprite.transform.localPosition;
		initDepth = actorSprite.depth;
//		Debug.LogError ("actorSprite.width  : " + actorSprite.width);
	}
	
	public override void ShowUI () {
		if (!actorSprite.enabled)
			actorSprite.enabled = true;
		
		base.ShowUI ();
	}
	
	public override void HideUI () {
		actorSprite.spriteName = "";
//		if(actorSprite.enabled)
//			actorSprite.enabled = false;
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	Texture texure ;

	public void SetTexture(int itemID,bool canAttack) {
		this.itemID = itemID;
		if(!actorSprite.enabled)
			actorSprite.enabled = true;
		actorSprite.spriteName = itemID.ToString ();

		if (!canAttack) {
			actorSprite.color = CardItem.NoAttackColor;
		} else {
			actorSprite.color = Color.white;
		}
		xOffset = (float)actorSprite.width / 4;
	}
	
	public void Reset() {
		actorSprite.transform.localPosition = initPosition;
		actorSprite.spriteName = "";
	}
	
	public void SetTweenPosition(Vector3 start,Vector3 end) {
		tweenPosition.enabled = true;
		tweenPosition.from = start;
		tweenPosition.to = end;
		tweenPosition.duration = defaultMoveTime;
	}
	
	public void Move(Vector3 to,float time) {
		Move(transform.localPosition,to,time);
	}
	
	public void Move(Vector3 to) {
		Move(transform.localPosition,to,defaultMoveTime);
	}
	
	public void Move(Vector3 from,Vector3 to) {
		Move(from,to,defaultMoveTime);
	}
	
	public void Move(Vector3 from,Vector3 to, float time) {
		if(!tweenPosition.enabled )
			tweenPosition.enabled = true;
		tweenPosition.duration = time;
		tweenPosition.from = from;
		tweenPosition.to = to;
		tweenPosition.ResetToBeginning ();
		initPosition = to;
	}
	
	public void Scale(Vector3 to, float time) {
		Scale(transform.localScale,to,time);
	}
	
	public void Scale(Vector3 from, Vector3 to, float time) {
		iTween.ScaleTo (gameObject, iTween.Hash("x", to.x,"y",to.y,"time", 0.3f,"easetype","easeoutquad"));
	}
	
	void TweenPositionCallback() {
		if(tweenCallback != null) {
			tweenCallback(this);
		}
	}
	
	void SetPosition(int sortID) {
		gameObject.layer = GameLayer.IgnoreCard;
		actorSprite.depth = initDepth + sortID + 1;
		Vector3 pos = Battle.ChangeCameraPosition() - ViewManager.Instance.ParentPanel.transform.localPosition;
		Vector3 offset = new Vector3(sortID * (float)actorSprite.width / 2f , - sortID * (float)actorSprite.height / 2, 0f) - transform.parent.localPosition;
		transform.localPosition  = new Vector3(pos.x,pos.y,transform.localPosition.z) + offset ;
	}
	
	public void SetPos (Vector3 to) {
		transform.localPosition = to;
		initPosition = to;
	}
}
