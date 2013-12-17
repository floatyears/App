using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : UIBaseUnity
{
	private List<UITexture> monster = new List<UITexture>();

	private Transform parentTrans;

	private UITexture template;

	private UITexture tempTex;

	private int monsterCount = 0;

	private int interv = 10;
	
	private float xScale = 0f;

	public override void Init (string name)
	{
		base.Init (name);

		template = FindChild<UITexture>("Texture");
		template.enabled = true;
		template.gameObject.SetActive(false);

		Vector3 pos = transform.localPosition;

		transform.localPosition = new Vector3(pos.x,pos.y + 400f,pos.z);
	}

	public void Refresh(int count)
	{
		Clear();

		monsterCount = count;

		parentTrans = NGUITools.AddChild(gameObject).transform;

		for (int i = 0; i < count; i++) 
		{
			GameObject go = NGUITools.AddChild(gameObject,template.gameObject);

			go.SetActive(true);

			tempTex = go.GetComponent<UITexture>();

			tempTex.mainTexture = LoadAsset.Instance.LoadAssetFromResources("FullMask",ResourceEuum.Image) as Texture2D;

			tempTex.transform.localScale = new Vector3(Main.TexScale,Main.TexScale,1f);

			xScale = tempTex.width * Main.TexScale;

			CaculatePosition(i,go);

			monster.Add(tempTex);
		}
	}

	void Clear()
	{
		for (int i = 0; i < monster.Count; i++)
		{
			Destroy(monster[i].gameObject);
		}

		monster.Clear();
	}

	void CaculatePosition(int index,GameObject target)
	{
		target.transform.parent = parentTrans;

		Vector3 pos = parentTrans.localPosition;

		target.transform.localPosition = new Vector3(pos.x + index * (xScale + interv),pos.y,pos.z);

		if(index == monsterCount - 1)
		{
			pos = transform.parent.localPosition;

			float xWidth = (xScale * monsterCount + interv * monsterCount) / 2;

			parentTrans.localPosition = new Vector3(pos.x - xWidth,pos.y,pos.z);


		}
	}
}
