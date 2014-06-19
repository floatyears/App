using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Role : UIBaseUnity {
	private Coordinate currentCoor;
	public Coordinate CurrentCoor {
		get{ return currentCoor; }
	}

	private Coordinate prevCoor;
	public Coordinate PrevCoor {
		get { return prevCoor; }
	}
	[HideInInspector]
	public bool isMove = false;
	public bool waitMove = false;
	private Vector3 targetPoint;
	private const int xOffset = -5;
	private const int YOffset = 20;
	private const int ZOffset = -40;
	private Vector3 scale = new Vector3(30f, 25f, 30f);
	private Vector3 angle = new Vector3(330f, 0f, 0f);
	private List<Coordinate> firstWay = new List<Coordinate>();
	private Vector3 distance = Vector3.zero;
	public Vector3 TargetPoint {
		set {
			targetPoint.x = value.x + xOffset;
			targetPoint.y = value.y + YOffset;
			targetPoint.z = transform.localPosition.z;
		}
	}

	private BattleQuest bQuest;
	public BattleQuest BQuest {
		set{ bQuest = value; }
	}

	private Jump jump;
	private Vector3 initPosition = new Vector3 (-1115f, 340f, -20f);
	public override void Init (string name) {
		base.Init (name);
		jump = GetComponent<Jump> ();
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	void RoleStart() {
		prevCoor = currentCoor = ConfigBattleUseData.Instance.roleInitCoordinate;
		TargetPoint = bQuest.GetPosition(currentCoor);
		jump.Init (GetInitPosition());
		jump.GameStart (targetPoint);	
		SyncRoleCoordinate(currentCoor);

		Stop();
	}

	Vector3 GetInitPosition() {
		return new Vector3 (targetPoint.x, targetPoint.y, targetPoint.z - 100f);
	}

	Vector3 GetRolePosition(Vector3 pos) {
		Vector3 reallyPosition = new Vector3 (pos.x + 7f, pos.y + 30f, pos.z - 50f);
		return reallyPosition;
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		RoleStart ();
		MsgCenter.Instance.AddListener (CommandEnum.TrapMove, TrapMove);
		MsgCenter.Instance.AddListener (CommandEnum.NoSPMove, NoSPMove);
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
		MsgCenter.Instance.RemoveListener (CommandEnum.TrapMove, TrapMove);
		MsgCenter.Instance.RemoveListener (CommandEnum.NoSPMove, NoSPMove);
	}

	void NoSPMove(object data) {
		bQuest.battle.ShieldInput(false);
		Coordinate cd;
		if (data == null) {
			cd = prevCoor;
		} else {
			cd = (Coordinate)data;	
		}
	
		SetTarget (cd);
		bQuest.RoleCoordinate(cd);
		StartCoroutine (MoveByTrap ());
	}

	void TrapMove(object data) {
		bQuest.battle.ShieldInput (false);
		if (data == null) {
			SetTarget (currentCoor);
			StartCoroutine(MoveByTrap());
			return;
		}
		Coordinate cd = (Coordinate)data;
		SetTarget (cd);
		MsgCenter.Instance.Invoke(CommandEnum.TrapTargetPoint, cd);
		bQuest.RoleCoordinate(cd);
//		if (cd.x == bQuest.RoleInitPosition.x && cd.y == bQuest.RoleInitPosition.y) {
			GoTarget ();
//		} else {
//			GoTarget();	
//		}
	}

	void GoTarget() {
		bQuest.battleMap.ChangeStyle (currentCoor);
		transform.localPosition = targetPoint;
		bQuest.battle.ShieldInput (true);
	}

	IEnumerator MoveByTrap() {
		while (true) {
			jump.JumpAnim ();
			Stop ();	
			transform.localPosition = Vector3.Lerp(transform.localPosition,targetPoint,Time.deltaTime * 20);
			distance = transform.localPosition - targetPoint;
			yield return Time.deltaTime;
			if (distance.magnitude < 1f) {
				bQuest.battle.ShieldInput(true);
				bQuest.battleMap.ChangeStyle (currentCoor);
				yield break;

			}
		}
	}

	void Move() {
		if(firstWay.Count == 0) {
			return;
		}

		isMove = true;
		SetTarget(firstWay[0]);

		MoveRole ();
	}

	void QuestCoorEnd(Coordinate coor) {
		bQuest.currentCoor = coor;
	}

	void SetTarget(Coordinate tc) {
		QuestCoorEnd (tc);
		prevCoor = currentCoor;
		currentCoor = tc;
		TargetPoint = bQuest.GetPosition(tc);
		if (isMove) {
			jump.JumpAnim ();
		}
	}
	
//	void Update() {
//		if(!isMove)
//			return;
//
//		distance = transform.localPosition - targetPoint;
//
//		if(distance.magnitude > 1f)
//			transform.localPosition = Vector3.Lerp(transform.localPosition,targetPoint,Time.deltaTime * 10);
//		else
//			MoveEnd();
//	}

	Vector3[] secondPath = new Vector3[3];
	//-15 -659 -100
//	float y =  100f;
//	float time = 0.5f;
	Vector3 middlePoint = Vector3.zero;
	void MoveRole() {
		Vector3 localposition = transform.localPosition;
		Vector3 leftMiddlePoint = Vector3.zero;
		Vector3 rightMiddlePoint = Vector3.zero;
		Vector3 rightFristMiddlePoint = Vector3.zero;

		if (localposition.x == targetPoint.x) {
			middlePoint = new Vector3 (localposition.x, localposition.y + BattleMap.itemWidth * 1.5f, localposition.z);
			leftMiddlePoint = new Vector3(middlePoint.x, middlePoint.y * 0.9f, middlePoint.z);
			rightMiddlePoint = targetPoint;
		} else {
			float x = targetPoint.x - localposition.x;
			middlePoint = new Vector3 (localposition.x + x * 0.2f , localposition.y + BattleMap.itemWidth * 1.5f, localposition.z);
			leftMiddlePoint = new Vector3 (localposition.x + x * 0.1f , localposition.y + BattleMap.itemWidth * 0.85f, localposition.z);
//			rightFristMiddlePoint = new Vector3(middlePoint.x + x * 0.1f, middlePoint.y + 10f, middlePoint.z);
			rightMiddlePoint = new Vector3(localposition.x + x * 0.55f,localposition.y + BattleMap.itemWidth * 1.1f , localposition.z);
		}

		Vector3[] path = new Vector3[3];
		path [0] = transform.localPosition;
		path [1] = transform.localPosition;
		path [2] = middlePoint;

		secondPath [0] = middlePoint;
//		secondPath [1] = rightFristMiddlePoint;
		secondPath [1] = rightMiddlePoint;
		secondPath [2] = targetPoint;
//		Debug.LogError ("middlePoint : " + middlePoint + " rightMiddlePoint : " + rightMiddlePoint + " targetPoint : " + targetPoint); 

		iTween.MoveTo (gameObject, iTween.Hash ("path", path, "movetopath", false, "islocal", true, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", "MoveRoleSecond", "oncompletetarget", gameObject));
//		iTween.MoveTo (gameObject, iTween.Hash ("position", middlePoint, "islocal", true, "time", 0.25f, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", "MoveRoleSecond", "oncompletetarget", gameObject));
	}

	void MoveRoleSecond() {
//		iTween.MoveTo (gameObject, iTween.Hash ("position", targetPoint, "islocal", true, "time", 0.35f, "easetype", iTween.EaseType.easeInCubic, "oncomplete", "MoveEnd", "oncompletetarget", gameObject));
		iTween.MoveTo (gameObject, iTween.Hash ("path", secondPath, "movetopath", false, "islocal", true, "time", 0.3f, "easetype", iTween.EaseType.easeInCubic, "oncomplete", "MoveEnd", "oncompletetarget", gameObject));
	}
	
	Coordinate tempCoor; 

	void MoveEnd() {
		if(!isMove) {
			firstWay.Clear();
		}
		else {
			tempCoor = firstWay[0];
			firstWay.RemoveAt(0);
			bQuest.battleMap.ChangeStyle (tempCoor);
			SyncRoleCoordinate(tempCoor);
			if(firstWay.Count > 0) {
				GameTimer.GetInstance().AddCountDown(0.1f, Move);
			}
			else
				isMove = false;
		}
	}

	public void Stop() {
		isMove = false;
		firstWay.Clear ();
	}
	
	public void StartMove(Coordinate coor) {
		if (isMove)
			return;
		GenerateWayPoint(coor);
		Move();
	}

	public void SyncRoleCoordinate(Coordinate coor) {
		MsgCenter.Instance.Invoke (CommandEnum.MoveToMapItem, coor);
		if (!bQuest.ChainLinkBattle) {
			MsgCenter.Instance.Invoke(CommandEnum.ReduceActiveSkillRound);	
		}

		bQuest.RoleCoordinate(coor);
	}

	void GenerateWayPoint(Coordinate endCoord) {
		if(currentCoor.x == endCoord.x) {
			firstWay.AddRange(CaculateY(endCoord));
			return;
		}
		if(currentCoor.y == endCoord.y) {
			firstWay.AddRange(CaculateX(endCoord));
			return;
		}
		firstWay.AddRange(CaculateX(endCoord));
		firstWay.AddRange(CaculateY(endCoord));
	}
	
	List<Coordinate> CaculateX(Coordinate endCoord) {
		List<Coordinate> xWay = new List<Coordinate>();
		if(currentCoor.x < endCoord.x) {
			int i = currentCoor.x + 1;
			while(i <= endCoord.x) {
				xWay.Add(new Coordinate(i,currentCoor.y));
				i++;
			}
		}
		else if(currentCoor.x > endCoord.x) {
			int i = currentCoor.x - 1;
			while(i >= endCoord.x) {
				xWay.Add(new Coordinate(i,currentCoor.y));
				i--;
			}
		}
		return xWay;
	}
	
	List<Coordinate> CaculateY(Coordinate endCoord)
	{
		List<Coordinate> yWay = new List<Coordinate>();
		if(currentCoor.y < endCoord.y) {
			int i = currentCoor.y +1 ;
			while(i <= endCoord.y) {
				yWay.Add(new Coordinate(endCoord.x,i));
				i++;
			}
		}
		else {
			int i = currentCoor.y - 1;
			while(i >= endCoord.y)	{
				yWay.Add(new Coordinate(endCoord.x,i));
				i--;
			}
		}
		
		return yWay;
	}

}
