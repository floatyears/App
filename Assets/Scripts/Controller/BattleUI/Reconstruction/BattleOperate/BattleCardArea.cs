using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

	private static List<GameObject> battleCardIns = new List<GameObject>();

	private GameObject cardItem;

	public override void Init (string name)
	{
		base.Init (name);

		backTexture = FindChild<UITexture>("Back");

		backTexture.gameObject.SetActive(false);

		if (cardItem == null) 
		{
			GameObject go = LoadAsset.Instance.LoadAssetFromResources (Config.battleCardName, ResourceEuum.Prefab) as GameObject;
			cardItem = go.transform.Find("Texture").gameObject;
		}

		StartCoroutine (GenerateCard ());
	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		gameObject.SetActive (true);
	}

	public override void HideUI ()
	{
		base.HideUI ();

		for (int i = 0; i < battleCardAreaItem.Length; i++) 
		{
			battleCardAreaItem[i].HideUI();
		}

		gameObject.SetActive (false);
	}

	public void CreatArea(Vector3[] position,int height)
	{
		if(position == null)
			return;

		battleCardAreaItem = new BattleCardAreaItem[position.Length];

		for (int i = 0; i < position.Length; i++)
		{
			tempObject = NGUITools.AddChild(gameObject,backTexture.gameObject);

			tempObject.SetActive(true);

			tempObject.layer = GameLayer.BattleCard;

			tempObject.transform.localPosition = new Vector3(position[i].x,position[i].y + height,position[i].z) ;

			BattleCardAreaItem bca = tempObject.AddComponent<BattleCardAreaItem>();

			bca.Init(tempObject.name);

			bca.AreaItemID = i;

			battleCardAreaItem[i] = bca;
		}
	}


	public bool tempCountTime = false;
	float time = 5f;

//	void Update()
//	{
//		if(tempCountTime)
//		{
//			if(time > 0)
//				time -= Time.deltaTime;
//			else
//			{
//				time = 5f;
//				tempCountTime = false;
//				StartBattle();
//			}
//		}
//	}
	
	void StartBattle()
	{
		battleAttack.Clear();
		count = 0;
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

	static int count = 0;

	public static GameObject GetCard()
	{
		GameObject go = battleCardIns [count];
		count ++;
		return go;
	}

	IEnumerator GenerateCard()
	{
		bool b = battleCardIns.Count < 25;
		if (b) 
		{
			GameObject go = NGUITools.AddChild(gameObject,cardItem);//Instantiate (cardItem) as GameObject;
			go.layer = gameObject.layer;
			Destroy(go.GetComponent<BoxCollider>());
			go.AddComponent<CardItem>();
			battleCardIns.Add(go);
		}
		yield return 1;
		if (b) 
		{
			StartCoroutine (GenerateCard ());
		}
	}


}
