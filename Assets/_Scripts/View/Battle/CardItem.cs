using UnityEngine;
using System.Collections.Generic;

public class CardItem : UIBaseUnity {
	public static Color32 NoAttackColor = new Color32 (174, 174, 174, 255);
	public event UICallback<CardItem> tweenCallback;
	[HideInInspector]
	public bool canAttack = true;
	private UISprite actorTexture;
	public UISprite ActorTexture {
		get{return actorTexture;}
	}

	private UISprite linkLineSprite;	
	private List<UISprite> linkLineSpriteList = new List<UISprite> ();
	private List<Transform> target = new List<Transform> ();
	private Vector3 initActorPosition;
	private Vector3 hideActorPosition = new Vector3 (10000f, 10000f, 10000f);
	private TweenPosition tweenPosition;

	public TweenPosition TweenP {
		get{return tweenPosition;}
	}
	

	private TweenScaleExtend tse;

	public TweenScaleExtend TweenSE {
		get{return tse;}
	}

	private Vector3 initPosition;

	public Vector3 InitPosition {
		set{initPosition = value;}
	}
	
	private int initDepth;
	public int InitDepth {
		get { return initDepth;}
	}

	private bool canDrag = true;

	public bool CanDrag {
		set {
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
		linkLineSprite.enabled = false;

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
		Debug.LogError ("initPosition : " + initPosition + "gameobject : " + gameObject);
		initDepth = actorTexture.depth;
		CanDrag = true;

		for (int i = 0; i < 5; i++) {	// 5 == 
			Transform trans = NGUITools.AddChild(gameObject, linkLineSprite.gameObject).transform;
			UISprite sprite = trans.GetComponent<UISprite>();
			sprite.spriteName = "";
			sprite.enabled = false;
			linkLineSpriteList.Add(sprite);
		}
	}

	public override void ShowUI () {
		if(!actorTexture.enabled)
			actorTexture.enabled = true;
		if (itemID != -1) {
			actorTexture.spriteName = itemID.ToString();
		}
		base.ShowUI ();
	}

	public override void HideUI () {
		actorTexture.spriteName = "";
		base.HideUI ();
	}

//	void AttackEnemyEnd(object data) {
//		Clear ();
//	}

	public void SetPosition(Vector3 localposition) {
//		Debug.LogError ("gameobject : " + gameObject + "card item set position : " + localposition);
		transform.localPosition = localposition;
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public void SetSprite(int index,bool canAttack) {
		itemID = index;
		Clear ();

		this.canAttack = canAttack;
		actorTexture.spriteName = index.ToString ();
		linkLineSprite.spriteName = "line_0" + index;

		if (!canAttack) {
			actorTexture.color = NoAttackColor;	
		} else {
			actorTexture.color = Color.white;
		}
	}

	public void OnDrag(Vector3 position,int index) {
		if(!canDrag)
			return;
		float offset = index * xOffset;
		SetPosition (new Vector3 (position.x + offset, position.y - offset, 0f));
//		actorTexture.transform.localPosition = ;
	}

	public void OnPress(bool isPress,int sortID) {
		if(!canDrag)
			return;
		if(isPress) {	
			SetPosition(sortID);
		}
		else {
			Reset();
		}
	}
//	bool b = false;
	public void Reset() {
//		Debug.LogError ("gameobject : " + gameObject + "initposition : " + initPosition);
//		transform.localPosition = initPosition;
		SetPosition (initPosition);
//		Debug.LogError ("transform.localPosition : " + transform.localPosition);
//		b = true;
	}

	public void SetTweenPosition(Vector3 start,Vector3 end)	{
		tweenPosition.enabled = true;
		tweenPosition.from = start;
		tweenPosition.to = end;
		tweenPosition.duration = defaultMoveTime;
	}

	public bool SetCanDrag(int id) {
		if(id == this.itemID)
			CanDrag = true;
		else
			CanDrag = false;

		return canDrag;
	}

	public void Move(Vector3 to,float time) {
		Move(transform.localPosition, to, time);
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

		actorTexture.depth = initDepth + sortID + 1;

		Vector3 pos = Battle.ChangeCameraPosition() - vManager.ParentPanel.transform.localPosition;

		Vector3 offset = new Vector3(sortID * (float)actorTexture.width / 2f , - sortID * (float)actorTexture.height / 2, 0f) - transform.parent.localPosition;

		SetPosition (new Vector3 (pos.x, pos.y, transform.localPosition.z) + offset);
//		transform.localPosition  = ;
	}
	
	public void SetPos (Vector3 to) {
//		transform.localPosition = to;
		SetPosition (to);
		initPosition = to;
	}

//	public void StartBattle() {
//
//			Clear();	
//
////		foreach (var item in linkLineSpriteList) {
////			if(item.spriteName == "" && b) {
////				return;
////			}
////
////			if(!b) {
////				item.enabled = b;
////			}
////
////			item.enabled = b;
////		}
//	}
	
	public void SetTargetLine(List<Transform> target) {
		this.target = target;
		CalculateAngel = target.Count > 0 ? true : false;
		if (target.Count == 0) {
			Clear();
			return;	
		}
		for (int i = 0; i < target.Count; i++) {
			linkLineSpriteList[i].enabled = true;
			linkLineSpriteList[i].spriteName = linkLineSprite.spriteName;
		}

		Rotate ();
	}

	public void Clear () {
		CalculateAngel = false;
		target.Clear ();
		foreach (var item in linkLineSpriteList) {
			item.spriteName = "";
			item.enabled = false;
		}
	}

	bool CalculateAngel = false;
	Quaternion qa = new Quaternion();
	private Vector3 prevPosition;

	void Update () {
		Vector3 position = transform.localPosition;
		if (CalculateAngel && Vector3.Distance (prevPosition, position) > 0.01f) {
			Rotate();
		}
	}

//	void LateUpdate () {
//		
//		if (b && transform.localPosition != initPosition) {
//			transform.localPosition = initPosition;
//			b = false;
//		}
//	}

	void Rotate() {
		for (int i = 0; i < target.Count; i++) {
			Vector3 targetPosition = target[i].localPosition;
			Transform trans = linkLineSpriteList[i].transform;
			Vector3 localposition = transform.localPosition;
			Vector3 forward = targetPosition - localposition;
			float angle = CalculateAngle(localposition, targetPosition, forward);

			trans.eulerAngles = new Vector3(0f,0f,angle);
			int distance = (int)forward.magnitude;
			linkLineSpriteList[i].height = distance;
		}
		prevPosition = transform.localPosition;
	}

	float CalculateAngle(Vector3 x, Vector3 y, Vector3 direction) {
		Vector3 direcX = transform.up;

		float angle = Vector3.Angle (direcX, direction);
//		Debug.LogError ("target position : " + y + " locaposition : " + x + " angle : " + angle);
		if (x.x < y.x) {
			angle = 360 - angle;
		}
//		Debug.LogError("360 - angle : " + angle + " gameobject : " + gameObject);
		return angle;
	}
}
