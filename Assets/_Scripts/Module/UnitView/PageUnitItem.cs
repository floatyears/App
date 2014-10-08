using UnityEngine;
using System.Collections;

public class PageUnitItem : MyUnitItem {
	public static PageUnitItem Inject(GameObject item){
		PageUnitItem view = item.GetComponent<PageUnitItem>();
		if (view == null) view = item.AddComponent<PageUnitItem>();
		return view;
	}

	public DataListener callback;

	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	protected override void InitUI(){
		base.InitUI();
		lightSpr = transform.FindChild("Sprite_Light").GetComponent<UISprite>();
//		maskSpr.enabled = false;
		IsFocus = false;
		IsParty = true;
	}

	protected override void UpdatePartyState(){
	}

	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}

	protected override void UpdatEnableState(){
		if(IsEnable){
			UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
		}
		else{
			UIEventListenerCustom.Get(this.gameObject).LongPress = null;
		}
	}

	
}
