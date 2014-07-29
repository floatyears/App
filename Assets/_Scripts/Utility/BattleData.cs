using UnityEngine;
using System.Collections;

public class BattleData
{
	public static byte cardCount = 5;

}


public class BattleItem
{
	private int id;

	public int Id 
	{
		get 
		{
			return id;
		}
		set 
		{
			id = value;
		}
	}

	private GameObject self;

	public GameObject Self 
	{
		get 
		{
			return self;
		}
		set 
		{
			self = value;
		}
	}

	private UITexture selfTexture;

	public UITexture SelfTexture 
	{
		get 
		{
			return selfTexture;
		}
		set 
		{
			selfTexture = value;
		}
	}

	private TweenPosition selfTween;

	public TweenPosition SelfTween 
	{
		get 
		{
			return selfTween;
		}
		set 
		{
			selfTween = value;
		}
	}
}