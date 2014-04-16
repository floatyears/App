using UnityEngine;
using System.Collections;

public class CardItem : UIBaseUnity 
{
	public static Color32 NoAttackColor = new Color32 (174, 174, 174, 255);
	public event UICallback<CardItem> tweenCallback;

	[HideInInspector]
	public bool canAttack = true;

	private UISprite actorTexture;

	public UISprite ActorTexture
	{
		get{return actorTexture;}
	}

	private Vector3 initActorPosition;
	private Vector3 hideActorPosition = new Vector3 (10000f, 10000f, 10000f);
	
	private TweenPosition tweenPosition;

	public TweenPosition TweenP
	{
		get{return tweenPosition;}
	}

	private UIButtonScale anim;
	
	private TweenScaleExtend tse;

	public TweenScaleExtend TweenSE
	{
		get{return tse;}
	}

	private Vector3 initPosition;

	public Vector3 InitPosition
	{
		set{initPosition = value;}
	}
	
	private int initDepth;
	public int InitDepth {
		get { return initDepth;}
	}

	private bool canDrag = true;

	public bool CanDrag
	{
		set{
			canDrag = value;
//			Debug.LogError("CanDrag : " + this);
			if(canDrag) {
				gameObject.layer = GameLayer.ActorCard;
			}
			else{
				gameObject.layer = GameLayer.IgnoreCard;
			}
		}
		get{return canDrag;}
	}

	private Transform parentObject;
	private float xOffset = 0f;
	private float defaultMoveTime = 0.1f;

	[HideInInspector]
	public int itemID = -1;
	[HideInInspector]
	public int location = -1;
	[HideInInspector]
	public int color = -1;

	public override void Init (string name) {
		base.Init (name);
		parentObject = transform.parent;
		actorTexture = GetComponent<UISprite>();
		if (!actorTexture.enabled) {
			actorTexture.enabled = true;
		}

		actorTexture.spriteName = "";
		xOffset = (float)actorTexture.width / 4;
		initActorPosition = actorTexture.transform.localPosition;
		tweenPosition = GetComponent<TweenPosition>();
		tweenPosition.enabled = false;
		tse = GetComponent<TweenScaleExtend>();
		tse.enabled = false;
		tweenPosition.eventReceiver = gameObject;
		tweenPosition.callWhenFinished = "TweenPositionCallback";
		initPosition = actorTexture.transform.localPosition;
		anim = GetComponent<UIButtonScale>();
		initDepth = actorTexture.depth;
		CanDrag = true;
	}

	public override void ShowUI ()
	{
		if(!actorTexture.enabled)
			actorTexture.enabled = true;
		if (itemID != -1) {
			actorTexture.spriteName = itemID.ToString();
		}
		//actorTexture.transform.localPosition = initActorPosition;

		base.ShowUI ();
	}

	public override void HideUI ()
	{
		//actorTexture.mainTexture = null;
		actorTexture.spriteName = "";

//		actorTexture.transform.localPosition = hideActorPosition;
		base.HideUI ();
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}

	public void SetSprite(int index,bool canAttack) {
		this.canAttack = canAttack;
		itemID = index;
		actorTexture.spriteName = index.ToString ();
		if (!canAttack) {
			actorTexture.color = NoAttackColor;	
		}				
		else {
			actorTexture.color = Color.white;
		}
	}

	public void OnDrag(Vector3 position,int index)
	{
		if(!canDrag)
			return;
		float offset = index * xOffset;

		actorTexture.transform.localPosition = new Vector3(position.x + offset, position.y - offset, position.z);
	}

	public void OnPress(bool isPress,int sortID)
	{
		if(!canDrag)
			return;
		anim.OnPress(isPress);
		if(isPress)
		{	
			SetPosition(sortID);
		}
		else
		{
			Reset();
		}
	}

	public void Reset()
	{
		actorTexture.transform.localPosition = initPosition;
	}

	public void SetTweenPosition(Vector3 start,Vector3 end)
	{
		tweenPosition.enabled = true;
		tweenPosition.from = start;
		tweenPosition.to = end;
		tweenPosition.duration = defaultMoveTime;
	}

	public bool SetCanDrag(int id)
	{
		if(id == this.itemID)
			CanDrag = true;
		else
			CanDrag = false;

		return canDrag;
	}

	public void Move(Vector3 to,float time)
	{
		Move(transform.localPosition,to,time);
	}

	public void Move(Vector3 to)
	{
		Move(transform.localPosition,to,defaultMoveTime);
	}

	public void Move(Vector3 from,Vector3 to)
	{
		Move(from,to,defaultMoveTime);
	}

	public void Move(Vector3 from,Vector3 to, float time)
	{
		if(!tweenPosition.enabled )
			tweenPosition.enabled = true;

		tweenPosition.duration = time;
		tweenPosition.from = from;
		tweenPosition.to = to;
		tweenPosition.Reset ();
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

	void SetPosition(int sortID)
	{
		gameObject.layer = GameLayer.IgnoreCard;

		actorTexture.depth = initDepth + sortID + 1;

		Vector3 pos = Battle.ChangeCameraPosition() - vManager.ParentPanel.transform.localPosition;

		Vector3 offset = new Vector3(sortID * (float)actorTexture.width / 2f , - sortID * (float)actorTexture.height / 2, 0f) - transform.parent.localPosition;

		transform.localPosition  = new Vector3(pos.x,pos.y,transform.localPosition.z) + offset ;
	}
	
	public void SetPos (Vector3 to)
	{
		transform.localPosition = to;

		initPosition = to;
	}
}
