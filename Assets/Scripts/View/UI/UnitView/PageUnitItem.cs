using UnityEngine;
using System.Collections;

public class PageUnitItem : MyUnitItem {
	public static PageUnitItem Inject(GameObject item){
		PageUnitItem view = item.AddComponent<PageUnitItem>();
		if (view == null) view = item.AddComponent<PageUnitItem>();
		return view;
	}
	public delegate void UnitItemCallback(PageUnitItem puv);
	public UnitItemCallback callback;

	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	protected override void InitUI(){
		base.InitUI();
		lightSpr = transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		maskSpr.enabled = false;
	}
	
	protected override void InitState(){
		base.InitState();
		IsFocus = false;
		IsParty = true;
	}

	protected override void UpdatePartyState(){	}

	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}

	protected override void UpdatEnableState(){
		UIEventListenerCustom.Get(this.gameObject).onClick = ClickItem;
		if(IsEnable){
			UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
		}
		else{
			UIEventListenerCustom.Get(this.gameObject).LongPress = null;
		}
	}
}
