using UnityEngine;
using System.Collections.Generic;

public class Role : UIBaseUnity
{
	private Coordinate currentCoor;

	public Coordinate CurrentCoor
	{
		get{ return currentCoor; }
	}

	/// <summary>
	/// role state
	/// </summary>
	private bool isMove = false;

	private Vector3 targetPoint;

	private const int YOffset = 25;
	private const int ZOffset = -40;

	private Vector3 scale = new Vector3(30f,25f,30f);
	private Vector3 angle = new Vector3(330f,0f,0f);

	private List<Coordinate> firstWay = new List<Coordinate>();

	private Vector3 distance = Vector3.zero;

	public Vector3 TargetPoint
	{
		set
		{
			targetPoint.x = value.x;
			targetPoint.y = value.y + YOffset;
			targetPoint.z = transform.localPosition.z;
		}
	}

	private BattleQuest bQuest;

	public BattleQuest BQuest
	{
		set{ bQuest = value; }
	}

	public override void Init (string name)
	{
		base.Init (name);

		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.Euler(angle);
	}

	public override void CreatUI ()
	{
	
		base.CreatUI ();


	}

	void RoleStart()
	{
		currentCoor = bQuest.RoleInitPosition;
		
		Vector3 pos = bQuest.GetPosition(currentCoor);
		
		transform.localPosition = new Vector3(pos.x,pos.y + YOffset ,pos.z + ZOffset);
		
		SyncRoleCoordinate(currentCoor);
		
		Stop();
	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		gameObject.SetActive (true);
		RoleStart ();
	}

	public override void HideUI ()
	{
		base.HideUI ();
		gameObject.SetActive (false);
	}

	void Move()
	{
		if(firstWay.Count == 0)
		{
			return;
		}

		isMove = true;

		SetTarget(firstWay[0]);
	}

	void SetTarget(Coordinate tc)
	{
		currentCoor.x = tc.x;
		currentCoor.y = tc.y;
		TargetPoint = bQuest.GetPosition(tc);

	}
	
	void Update()
	{
		if(!isMove)
			return;

		distance = transform.localPosition - targetPoint;

		if(distance.magnitude > 0.1f)
			transform.localPosition = Vector3.Lerp(transform.localPosition,targetPoint,Time.deltaTime * 10);
		else
			MoveEnd();
	}

	Coordinate tempCoor; 

	void MoveEnd()
	{
		if(!isMove)
		{
			firstWay.Clear();
		}
		else
		{
			tempCoor = firstWay[0];

			firstWay.RemoveAt(0);

			SyncRoleCoordinate(tempCoor);

			if(firstWay.Count > 0)
				SetTarget(firstWay[0]);
			else
				isMove = false;
		}
	}

	public void Stop()
	{
		isMove = false;

		firstWay.Clear ();
	}


	public void StartMove(Coordinate coor)
	{
		if(isMove)
			return;

		GenerateWayPoint(coor);

		Move();
	}

	void SyncRoleCoordinate(Coordinate coor)
	{
		bQuest.RoleCoordinate(coor);
	}

	void GenerateWayPoint(Coordinate endCoord)
	{
		if(currentCoor.x == endCoord.x)
		{
			firstWay.AddRange(CaculateY(endCoord));
			return;
		}

		if(currentCoor.y == endCoord.y)
		{
			
			firstWay.AddRange(CaculateX(endCoord));
			return;
		}
		
		firstWay.AddRange(CaculateX(endCoord));
		
		firstWay.AddRange(CaculateY(endCoord));

		Move();
	}
	
	List<Coordinate> CaculateX(Coordinate endCoord)
	{
		List<Coordinate> xWay = new List<Coordinate>();
		
		if(currentCoor.x < endCoord.x)
		{
			int i = currentCoor.x + 1;

			while(i <= endCoord.x)
			{
				xWay.Add(new Coordinate(i,currentCoor.y));
				i++;
			}
		}
		else if(currentCoor.x > endCoord.x)
		{
			int i = currentCoor.x - 1;

			while(i >= endCoord.x)
			{
				xWay.Add(new Coordinate(i,currentCoor.y));
				i--;
			}
		}
		
		return xWay;
	}
	
	List<Coordinate> CaculateY(Coordinate endCoord)
	{
		List<Coordinate> yWay = new List<Coordinate>();
		
		if(currentCoor.y < endCoord.y)
		{
			int i = currentCoor.y +1 ;
			
			while(i <= endCoord.y)
			{
				yWay.Add(new Coordinate(endCoord.x,i));
				i++;
			}
		}
		else
		{
			int i = currentCoor.y - 1;
			while(i >= endCoord.y)
			{
				yWay.Add(new Coordinate(endCoord.x,i));
				i--;
			}
		}
		
		return yWay;
	}

}
