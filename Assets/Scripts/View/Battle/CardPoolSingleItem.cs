using UnityEngine;
using System.Collections.Generic;

public class CardPoolSingleItem : UIBaseUnity 
{
	
	protected UITexture backTexture;

	protected List<CardItem> itemCardList = new List<CardItem>();

	protected TweenPosition tweenPosition;

	public override void Init (string name)
	{
		base.Init (name);

		backTexture = FindChild<UITexture>("Back");
	}

	public virtual bool GenerateCard(object cardID)
	{
		if(itemCardList.Count == 1)
			return false;

		int id = (int)cardID;

		ItemData itemData = Config.Instance.CardData[id];

		GameObject go = NGUITools.AddChild(gameObject);

		go.name = itemData.itemName;

		go.AddComponent<TweenPosition>();

		CardItem ci = go.AddComponent<CardItem>();

		ci.Init(itemData.itemName);

		itemCardList.Add(ci);

		Texture2D tex2d = LoadAsset.Instance.LoadAssetFromResource(id) as Texture2D;

		itemCardList[0].SetTexture(tex2d,backTexture.width,backTexture.height);

		return true;
	}



}
