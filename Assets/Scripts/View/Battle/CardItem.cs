using UnityEngine;
using System.Collections.Generic;

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

	private UISprite linkLineSprite;

	private Queue<Transform> linkLineSpriteCache = new Queue<Transform> ();
	private List<UISprite> linkLineSpriteList = new List<UISprite> ();

	private List<Transform> target = new List<Transform> ();

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

		linkLineSprite = FindChild<UISprite>("Sprite");

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

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void SetSprite(int index,bool canAttack) {

		CalculateAngel = false;
		target.Clear ();
		foreach (var item in linkLineSpriteList) {
			item.gameObject.SetActive(false);
			linkLineSpriteCache.Enqueue(item.transform);
		}
		linkLineSpriteList.Clear ();

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

	public void Move(Vector3 from,Vector3 to, float time) {
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

	public void SetTargetLine(List<Transform> target) {
		this.target = target;
//		CalculateAngel = target.Count > 0 ? true : false;
		if (target.Count == 0) {
			return;	
		}
		foreach (var item in target) {
			Transform trans ;
			if(linkLineSpriteCache.Count == 0) {
				trans = NGUITools.AddChild(gameObject, linkLineSprite.gameObject).transform;
//			.enabled = true;
			}else{
				trans = linkLineSpriteCache.Dequeue();
			}
			UISprite sprite = 	trans.GetComponent<UISprite>();
			sprite.enabled = true;
			linkLineSpriteList.Add(sprite);
		}
	}
	bool CalculateAngel = false;
	Quaternion qa = new Quaternion();
	void LateUpdate () {
		if (CalculateAngel) {
			for (int i = 0; i < target.Count; i++) {
				Vector3 targetPosition = target[i].localPosition;

				Transform trans = linkLineSpriteList[i].transform;
				Vector3 localposition = trans.parent.localPosition;
				Vector3 forward = targetPosition - localposition;
				float yAngel = CalculateAngle(trans.right, forward);
				Vector3 angle = new Vector3(trans.eulerAngles.x, 0f, yAngel);
				trans.eulerAngles = angle;
				int distance = (int)Vector3.Distance(localposition, targetPosition);
				linkLineSpriteList[i].width = distance;
			}
		}
	}

	float CalculateAngle(Vector3 x, Vector3 y) {

		float angle = Vector3.Angle (x, y);
		if (x.y > y.y) {
			angle += 180f;
		}
					
		return angle;
	}
}
