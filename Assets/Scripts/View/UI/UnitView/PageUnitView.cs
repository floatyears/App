using UnityEngine;
using System.Collections;

public class PageUnitView : MyUnitView {
	public static PageUnitView Inject(GameObject item){
		PageUnitView view = item.AddComponent<PageUnitView>();
		if (view == null) view = item.AddComponent<PageUnitView>();
		return view;
	}
	public delegate void UnitItemCallback(PageUnitView puv);
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
