using UnityEngine;
using System.Collections;

public class CardItem : UIBaseUnity 
{
	private UITexture actorTexture;

	private TweenPosition tweenPosition;

	public UITexture ActorTexture;

	private Vector3 initPosition;

	private bool canDrag = false;

	public override void Init (string name)
	{
		base.Init (name);

		actorTexture = GetComponent<UITexture>();

		tweenPosition = GetComponent<TweenPosition>();

		tweenPosition.enabled = false;

		actorTexture.enabled = false;

		initPosition = actorTexture.transform.localPosition;
	}
	
	public override void ShowUI ()
	{
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

	public void SetTexture(Texture2D tex,int width,int height)
	{
		actorTexture.mainTexture = tex;
		actorTexture.width = width;
		actorTexture.height = height;
	}

	public void DragTexure(Vector2 position)
	{
		actorTexture.transform.localPosition += (Vector3)position;
	}

	public void Reset()
	{
		actorTexture.transform.localPosition = initPosition;
	}
}
