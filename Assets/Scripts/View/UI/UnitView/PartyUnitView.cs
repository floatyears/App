﻿using UnityEngine;
using System.Collections;

public class PartyUnitView : MyUnitView {
	private UISprite lightSpr;
	public static PartyUnitView Inject(GameObject item){
		PartyUnitView view = item.AddComponent<PartyUnitView>();
		if (view == null) view = item.AddComponent<PartyUnitView>();
		return view;
	}
	public delegate void UnitItemCallback(PartyUnitView puv);
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
