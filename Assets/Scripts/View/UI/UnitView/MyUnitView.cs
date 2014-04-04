using UnityEngine;
using System.Collections;

public class MyUnitView : UnitView {
	private UILabel partyLabel;
	private UISprite collectedSpr;
	private bool isParty;
	public bool IsParty{
		get{
			return isParty;
		}
		set{
			isParty = value;
			UpdatePartyState();
		}
	}

	private bool isCollected;
	public bool IsCollected{
		get{
			return isCollected;
		}
		set{
			isCollected = value;
			UpdateCollectState();
		}
	}

    private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get {
			if(itemPrefab == null) {
				itemPrefab = Resources.Load("Prefabs/UI/UnitItem/MyUnitPrefab") as GameObject ;
			}
			return itemPrefab;
		}
	}

	public static MyUnitView Inject(GameObject item){
		MyUnitView view = item.AddComponent<MyUnitView>();
		if (view == null) view = item.AddComponent<MyUnitView>();
                return view;
	}

	protected override void InitUI(){
		base.InitUI();
		collectedSpr = transform.FindChild("Sprite_Collect").GetComponent<UISprite>();
		partyLabel = transform.FindChild("Label_Party").GetComponent<UILabel>();
		partyLabel.text = "Party";
		partyLabel.color = Color.red;
	}

	protected override void InitState(){
		base.InitState();
		IsCollected = false;
		IsParty = false;
	}

	protected override void ClickItem(GameObject item){}

	private void UpdatePartyState(){
		partyLabel.enabled = isParty;
    }

	private void UpdateCollectState(){
		collectedSpr.enabled = isCollected;
	}

}
