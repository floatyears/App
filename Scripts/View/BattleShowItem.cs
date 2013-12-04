using UnityEngine;
using System.Collections;

public class BattleShowItem : MonoBehaviour 
{
	private int id;

	public int ID
	{
		get{return id;}
	}

	public void InitData(object data)
	{
		id = (int)data;
	}
}
