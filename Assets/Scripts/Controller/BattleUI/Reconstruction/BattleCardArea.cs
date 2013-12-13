using UnityEngine;
using System.Collections;

public class BattleCardArea : UIBaseUnity
{
	private UITexture backTexture;

	private BattleCardAreaItem[] battleCardAreaItem;

	public override void Init (string name)
	{
		base.Init (name);

		backTexture = FindChild<UITexture>("Back");

		backTexture.gameObject.SetActive(false);
	}

	public void CreatArea(Vector3[] position)
	{
		if(position == null)
			return;

		battleCardAreaItem = new BattleCardAreaItem[position.Length];

		for (int i = 0; i < position.Length; i++)
		{
			tempObject = NGUITools.AddChild(gameObject,backTexture.gameObject);

			tempObject.SetActive(true);

			tempObject.layer = GameLayer.BattleCard;

			tempObject.transform.localPosition = position[i];

			BattleCardAreaItem bca = tempObject.AddComponent<BattleCardAreaItem>();

			bca.Init(tempObject.name);

			battleCardAreaItem[i] = bca;
		}
	}

}
