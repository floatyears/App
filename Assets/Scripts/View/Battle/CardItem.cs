using UnityEngine;
using System.Collections;

public class CardItem : UIBaseUnity 
{
	public event UICallback<CardItem> tweenCallback;

	private UITexture actorTexture;

	public UITexture ActorTexture
	{
		get{return actorTexture;}
	}
	
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

	private bool canDrag = true;

	public bool CanDrag
	{
		set{canDrag = value;}
		get{return canDrag;}
	}

	private Transform parentObject;

	[HideInInspector]
	public int itemID = -1;

	[HideInInspector]
	public int location = -1;

	public override void Init (string name)
	{
		base.Init (name);

		parentObject = transform.parent;

		actorTexture = GetComponent<UITexture>();

		tweenPosition = GetComponent<TweenPosition>();

		tse = GetComponent<TweenScaleExtend>();

		tse.enabled = false;

		tweenPosition.eventReceiver = gameObject;

		tweenPosition.callWhenFinished = "TweenPositionCallback";

		tweenPosition.enabled = false;

		initPosition = actorTexture.transform.localPosition;

		anim = GetComponent<UIButtonScale>();

		initDepth = actorTexture.depth;

		canDrag = true;
	}
	
	public override void ShowUI ()
	{
		if(!actorTexture.enabled)
			actorTexture.enabled = true;

		base.ShowUI ();
	}

	public override void HideUI ()
	{
		actorTexture.mainTexture = null;

		actorTexture.enabled = false;

		base.HideUI ();
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}

	public void SetTexture(Texture2D tex,int itemID)
	{
		ShowUI();

		this.itemID = itemID;

		actorTexture.mainTexture = tex;

		actorTexture.width = actorTexture.height = 100;

		NGUITools.UpdateWidgetCollider(gameObject);

		if(itemID == 1)
			actorTexture.color = Color.yellow;
		else
			actorTexture.color = Color.white;
	}

	public void SetTexture(Texture2D tex,int width,int height,int location,int itemID)
	{
		this.itemID = itemID;
		this.location = location;

		actorTexture.mainTexture = tex;
		actorTexture.width = width;
		actorTexture.height = height;

		ShowUI();

		if(itemID == 1)
		{
			actorTexture.color = Color.black;
		}
		else
			actorTexture.color = Color.white;
	}

	public void OnDrag(Vector3 position)
	{
		if(!canDrag)
			return;

		actorTexture.transform.localPosition += position ;
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
		gameObject.layer = GameLayer.ActorCard;
		actorTexture.depth = initDepth;
		transform.parent = parentObject;
		actorTexture.transform.localPosition = initPosition;
	}

	public void SetTweenPosition(Vector3 start,Vector3 end)
	{
		tweenPosition.enabled = true;
		tweenPosition.from = start;
		tweenPosition.to = end;
		tweenPosition.duration = 0.2f;
	}

	public bool SetCanDrag(int id)
	{
		if(id == this.itemID)
			canDrag = true;
		else
			canDrag = false;

		return canDrag;
	}

	public void Move(Vector3 to,float time)
	{
		Move(transform.localPosition,to,time);
	}

	public void Move(Vector3 to)
	{
		Move(transform.localPosition,to,0.2f);
	}

	public void Move(Vector3 from,Vector3 to)
	{
		Move(from,to,0.2f);
	}

	public void Move(Vector3 from,Vector3 to, float time)
	{
		tweenPosition.enabled = true;

		tweenPosition.duration = time;

		tweenPosition.from = from;

		tweenPosition.to = to;

		initPosition = to;
	}

	public void Scale(Vector3 to, float time)
	{
		Scale(transform.localScale,to,time);
	}

	public void Scale(Vector3 from, Vector3 to, float time)
	{
		tse.enabled = true;

		tse.duration = time;

		tse.from = from;

		tse.to = to;
	}
	
	void TweenPositionCallback(TweenPosition go)
	{
		if(tweenCallback != null)
		{
			tweenCallback(this);
		}
	}

	void SetPosition(int sortID)
	{
		gameObject.layer = GameLayer.IgnoreCard;

		actorTexture.depth = initDepth + sortID + 1;
		
		Vector3 pos = Battle.ChangeCameraPosition();

		Vector3 offset = new Vector3(sortID * (float)actorTexture.width / 2f , - sortID * (float)actorTexture.height / 2, 0f) - transform.parent.localPosition;

		transform.localPosition =new Vector3(pos.x,pos.y,transform.localPosition.z) + offset;
	}
}
