using UnityEngine;
using System.Collections;

public class MyUnitView : UnitView {
	protected UISprite lightSpr;
	protected UILabel partyLabel;
	protected UISprite collectedSpr;

	protected bool isParty;
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

	private bool isFocus;
	public bool IsFocus{
		get{
			return isFocus;
		}
		set{
			if(isFocus == value) return;
			isFocus = value;
			UpdateFocus();
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
		lightSpr = transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		collectedSpr = transform.FindChild("Sprite_Collect").GetComponent<UISprite>();
		partyLabel = transform.FindChild("Label_Party").GetComponent<UILabel>();
		partyLabel.enabled = false;
		partyLabel.text = "Party";
		partyLabel.color = Color.red;
	}

	protected override void InitState(){
		base.InitState();
		IsCollected = false;
		IsParty = false;
	}


	protected override void ClickItem(GameObject item){}

	protected virtual void UpdatePartyState(){}

	private void UpdateCollectState(){
		collectedSpr.enabled = isCollected;
	}

	protected virtual void UpdateFocus(){}

}
