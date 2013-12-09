using UnityEngine;
using System.Collections;

public class CardPoolItem : UIBaseUnity 
{
	private CardPoolEnum cardPoolType;

	public CardPoolEnum CardPoolType
	{
		set{cardPoolType = value;}
		get{return cardPoolType;}
	}


	private UITexture uiTexture;

	private UITexture itemTexture;

	public override void Init (string name)
	{
		base.Init (name);

		uiTexture = FindChild<UITexture>("Back");

		itemTexture = FindChild<UITexture>("Texture");
	}

	void GenerateCard(int cardID)
	{
		Vector3 initPosition = transform.localPosition;

		float width = uiTexture.mainTexture.width;

		float height = uiTexture.mainTexture.height;

		Texture2D tex2d = LoadAsset.Instance.LoadAssetFromResource(cardID) as Texture2D;

		if(!itemTexture.enabled)
			itemTexture.enabled = true;

		itemTexture.mainTexture = tex2d;

	}

}
