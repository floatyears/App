using UnityEngine;
using System.Collections;

public class UnitUnity : UIBaseUnity
{
	private UIButton partyBtn;
	public UIButton PartyBtn
	{
		get{ return partyBtn; }
	}

	private UIButton levelUpBtn;
	public UIButton LevelUpBtn
	{
		get{ return levelUpBtn; }
	}

	private UIButton evolveBtn;
	public UIButton EvolveBtn
	{
		get{ return evolveBtn; }
	}

	private UIButton sellBtn;
	public UIButton SellBtn
	{
		get{ return sellBtn; }
	}

	private UIButton listBtn;
	public UIButton ListBtn
	{
		get{ return listBtn; }
	}

	private UIButton catalogBtn;
	public UIButton CatalogBtn
	{
		get{ return catalogBtn; }
	}

	public override void Init(string name)
	{
		base.Init(name);

		partyBtn = FindChild<UIButton>("Bottom/Party");
		levelUpBtn = FindChild<UIButton>("Bottom/LevelUp");
		evolveBtn = FindChild<UIButton>("Bottom/Evolve");
		sellBtn = FindChild<UIButton>("Bottom/Sell");
		listBtn = FindChild<UIButton>("Bottom/UnitList");
		catalogBtn = FindChild<UIButton>("Bottom/Catalog");
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

	public void JumpToParty(GameObject btn)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Party);
		//ControllerManager.Instance.ShowActor(1);
	}

	public void JumpToLevelUp(GameObject btn)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.LevelUp);
	}
	
	public void JumpToEvolve(GameObject btn)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Evolve);
	}
	
	public void JumpToList(GameObject btn)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.UnitList);
	}
	
	public void JumpToSell(GameObject btn)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.Sell);
	}
	public void JumpToCatalog(GameObject btn)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.UnitCatalog);
	}

	private void Handlecallback (GameObject caller)
	{
		ControllerManager.Instance.HideActor();
	}
}
