using UnityEngine;
using System.Collections;

public class CardItem : UIBaseUnity 
{
	//private UITexture backTexture;

	private UITexture actorTexture;

	public override void Init (string name)
	{
		base.Init (name);

		actorTexture = FindChild<UITexture>("ActorTexture");

		actorTexture.enabled = false;
	}


	public override void ShowUI ()
	{
		actorTexture.enabled = true;

		base.ShowUI ();
	}

	public override void HideUI ()
	{


		base.HideUI ();
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}
}
