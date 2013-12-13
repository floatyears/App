//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's local scale.
/// </summary>

[RequireComponent(typeof(UITexture))]
public class TweenScaleExtend : UITweener
{
	public Vector3 from = Vector3.one;
	public Vector3 to = Vector3.one;

	private UITexture target;
	private Vector3 vec= Vector3.zero;

	void Start()
	{
		duration = 0.2f;

		target = GetComponent<UITexture>();
	}

	protected override void OnUpdate (float factor, bool isFinished)
	{
		if(duration <= 0)
			enabled = false;

		vec = from * (1f - factor) + to * factor;

		target.width = (int)vec.x;
		target.height = (int)vec.y;


	}
	

}
