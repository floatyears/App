using UnityEngine;
using System.Collections.Generic;

public class BattleCardArea : UIBaseUnity
{
	private UITexture backTexture;

	private BattleCardAreaItem[] battleCardAreaItem;

	private Battle battle;

	public Battle BQuest
	{
		set{ battle = value; }
	}

	private Dictionary<int,List<CardItem>> battleAttack = new Dictionary<int, List<CardItem>>();

	public override void Init (string name)
	{
		base.Init (name);

		backTexture = FindChild<UITexture>("Back");

		backTexture.gameObject.SetActive(false);

		Vector3 pos = transform.localPosition;

		transform.localPosition = new Vector3(pos.x,pos.y + 200f,pos.z);
	}

	public override void ShowUI ()
	{
		base.ShowUI ();

	}

	public override void HideUI ()
	{
		base.HideUI ();

		for (int i = 0; i < battleCardAreaItem.Length; i++) 
		{
			battleCardAreaItem[i].ClearCard();
		}
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


	public bool tempCountTime = false;
	float time = 5f;

	void Update()
	{
		if(tempCountTime)
		{
			if(time > 0)
				time -= Time.deltaTime;
			else
			{
				time = 5f;
				tempCountTime = false;
				StartBattle();
			}
		}
	}
	
	void StartBattle()
	{
		battleAttack.Clear();

		for (int i = 0; i < battleCardAreaItem.Length; i++)
		{
			battleAttack.Add(i,battleCardAreaItem[i].CardItemList);
		}

		battle.StartBattle(battleAttack);
	}

	public void OnGUI()
	{
		if(tempCountTime)
			GUILayout.Box(time.ToString(),GUILayout.Width(100f),GUILayout.Height(100f));


	}

}
