using UnityEngine;
using System.Collections;

public class PageUnitView : MyUnitView {
	private UISprite lightSpr;

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
	
	private bool isFocus;

	public bool IsFocus{
		get{
			return isFocus;
		}
		set{
			if(isFocus == value) return;
			isFocus = value;
			lightSpr.enabled = isFocus;
		}
	}

	protected override void InitUI(){
		base.InitUI();
		lightSpr = transform.FindChild("Sprite_Light").GetComponent<UISprite>();
	}
	
	protected override void InitState(){
		base.InitState();
		IsFocus = false;
		if(userUnit != null){
			IsParty = DataCenter.Instance.PartyInfo.UnitIsInCurrentParty(userUnit.ID);
			IsEnable = !IsParty;
		}
	}


}
