using UnityEngine;
using System.Collections;

public class UnitUnity : UIBaseUnity
{
	private UIButton partyBtn;
	private UIButton levelUpBtn;
	private UIButton evolveBtn;
	private UIButton sellBtn;
	private UIButton listBtn;
	private UIButton catalogBtn;

	public override void Init(string name)
	{
		base.Init(name);

		partyBtn = transform.Find("Bottom/Party").GetComponent<UIButton>();
		levelUpBtn = transform.Find("Bottom/LevelUp").GetComponent<UIButton>();
		evolveBtn = transform.Find("Bottom/Evolve").GetComponent<UIButton>();
		sellBtn = transform.Find("Bottom/Sell").GetComponent<UIButton>();
		listBtn = transform.Find("Bottom/UnitList").GetComponent<UIButton>();
		catalogBtn = transform.Find("Bottom/Catalog").GetComponent<UIButton>();

		
		UIEventListener.Get(partyBtn.gameObject).onClick = TurnToParty;
		UIEventListener.Get(levelUpBtn.gameObject).onClick = TurnToLevelUp;
		UIEventListener.Get(evolveBtn.gameObject).onClick = TurnToEvolve;
		UIEventListener.Get(listBtn.gameObject).onClick = TurnToList;
		UIEventListener.Get(sellBtn.gameObject).onClick = TurnToSell;
		UIEventListener.Get(catalogBtn.gameObject).onClick = TurnToCatalog;
	}

	void TurnToParty(GameObject go)
	{
		ChangeScene(SceneEnum.Party);
		ControllerManager.Instance.ShowActor(1);
	}
	void Handlecallback (GameObject caller)
	{
		ControllerManager.Instance.HideActor();
	}

	void TurnToLevelUp(GameObject go)
	{
		ChangeScene(SceneEnum.LevelUp);
	}
	
	void TurnToEvolve(GameObject go)
	{
		ChangeScene(SceneEnum.Evolve);
	}
	
	void TurnToList(GameObject go)
	{
		ChangeScene(SceneEnum.UnitList);
	}
	
	void TurnToSell(GameObject go)
	{
		ChangeScene(SceneEnum.Sell);
	}
	void TurnToCatalog(GameObject go)
	{
		ChangeScene(SceneEnum.UnitCatalog);
	}

	public override void ShowUI ()
	{
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
