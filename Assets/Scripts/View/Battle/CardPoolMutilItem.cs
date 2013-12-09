using UnityEngine;
using System.Collections.Generic;

public class CardPoolMutilItem : CardPoolSingleItem 
{
	public event UICallback Callback;

	public override void Init (string name)
	{
		base.Init (name);
	}

	public override bool GenerateCard(object data)
	{
		if(itemCardList.Count == 5)
			return false;

		GameObject item = (GameObject)data;

		item.transform.parent = transform.parent;

		TweenPosition tp = item.GetComponent<TweenPosition>();

		return true;
	}
}
