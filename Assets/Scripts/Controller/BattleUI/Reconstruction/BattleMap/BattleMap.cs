using UnityEngine;
using System.Collections.Generic;

public class BattleMap : UIBaseUnity 
{
	private MapItem template;

	private MapItem[,] map;

	private MapItem temp;

	private BattleQuest bQuest;

	private List<MapItem> prevAround = new List<MapItem>();

	private List<MapItem> useMapItem = new List<MapItem>();

	private MapItem prevMapItem;

	public BattleQuest BQuest
	{
		set{ bQuest = value; }
	}

	public override void Init (string name)
	{
		base.Init (name);

		map = new MapItem[bQuest.MapWidth, bQuest.MapHeight];

		template = FindChild<MapItem>("SingleMap");

		template.Init("SingleMap");

		template.gameObject.SetActive(false);
	}

	public override void CreatUI ()
	{
		base.CreatUI ();

		int x = map.GetLength(0);

		int y = map.GetLength(1);

		for (int i = 0; i < x; i++)
		{
			for (int j = 0; j < y; j++)
			{
				tempObject = NGUITools.AddChild(gameObject, template.gameObject);
				tempObject.SetActive(true);
				float xp = template.InitPosition.x + i * template.Width;
				float yp = template.InitPosition.y + j * template.Height;
				tempObject.transform.localPosition = new Vector3(xp,yp,template.InitPosition.z);
				temp = tempObject.GetComponent<MapItem>();
				temp.Coor = new Coordinate(i, j);
				temp.Init(i+"|"+j);
				UIEventListener.Get(tempObject).onClick = OnClickMapItem;
				map[i,j] = temp;
			}
		}

	}
	  
	void OnClickMapItem(GameObject go)
	{
		temp = go.GetComponent<MapItem>();

		bQuest.TargetItem(temp.Coor);
	}

	public Vector3 GetPosition(int x, int y)
	{
		if(x > map.Length || y > map.GetLength(0))
			return Vector3.zero;

		return map[x, y].transform.localPosition;
	}

	public bool ReachMapItem(Coordinate coor)
	{
		prevMapItem = map[coor.x,coor.y];

		if(!useMapItem.Contains(prevMapItem))
		{
			useMapItem.Add(prevMapItem);
			prevMapItem.IsOld = true;

			return false;
		}

		ChangeStyle(coor);

		return true;
	}

	void ChangeStyle(Coordinate coor)
	{
		if(prevAround.Count > 0)
		{
			for (int i = 0; i < prevAround.Count; i++)
			{
				prevAround[i].Around(false);
			}

			prevAround.Clear();
		}

		if(coor.x > 0)
			DisposeAround(map[coor.x - 1,coor.y]);

		if(coor.x < map.GetLength(0) - 1)
			DisposeAround(map[coor.x + 1,coor.y]);

		if(coor.y > 0)
			DisposeAround(map[coor.x,coor.y - 1]);

		if(coor.y < map.GetLength(1) - 1)
			DisposeAround(map[coor.x,coor.y + 1]);
	}

	void DisposeAround(MapItem item)
	{
		if(!item.IsOld)
		{
			item.Around(true);
			prevAround.Add(item);
		}
	}
}
