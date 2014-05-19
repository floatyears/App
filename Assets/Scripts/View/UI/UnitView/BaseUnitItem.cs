﻿using UnityEngine;
using System.Collections;
using bbproto;

public class BaseUnitItem : MonoBehaviour {
	protected bool canCrossed = true;
	protected bool isCrossed;
	protected UITexture avatarTex;
//	protected UISprite typeSpr;
	protected UISprite unitTypeBg;
	protected UISprite maskSpr;
	protected UILabel crossFadeLabel;

	public static BaseUnitItem Inject(GameObject item){
		BaseUnitItem view = item.GetComponent<BaseUnitItem>();
		if (view == null) view = item.AddComponent<BaseUnitItem>();
		return view;
	}

	public void Awake() {
		if(maskSpr == null){ InitUI(); }
	}
	
	private void FindUIElement() {
		avatarTex = transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
		crossFadeLabel = transform.FindChild("Label_Cross_Fade").GetComponent<UILabel>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		//typeSpr = transform.FindChild("Sprite_Type").GetComponent<UISprite>();
		unitTypeBg = transform.FindChild("Background").GetComponent<UISprite>();
	}

	protected TUserUnit userUnit;
	public TUserUnit UserUnit{
		get{
			return userUnit;
		}
		set{
//			if(userUnit != null && userUnit.Equals(value)) {}
//			else{
				userUnit = value;
				RefreshState();
//			}
		}
	}

	private int index;
	public int Index{
		get{
			return index;
		}
		set{
			index = value;
		}
	}

	private bool isEnable;
	public bool IsEnable{
		get{
			return isEnable;
		}
		set{
			isEnable = value;
			UpdatEnableState();
		}
	}
	
	private SortRule currentSortRule;
	public SortRule CurrentSortRule{
		get{
			return currentSortRule;
		}
		set{
			currentSortRule = value;
			UpdateCrossFadeText();
		}
	}
	
	public void Init(TUserUnit userUnit){
		Awake();
		this.userUnit = userUnit;
		InitState();
		ExecuteCrossFade();
	}

	protected virtual void InitUI(){
		FindUIElement();
	}


	protected virtual void InitState(){
		if(userUnit == null){
			SetEmptyState();
			return;
		}
		SetCommonState();
	}

	protected virtual void RefreshState(){
		if(userUnit == null){
			SetEmptyState();
			return;
		}
		SetCommonState();
	}
	private void ExecuteCrossFade(){
		if (! IsInvoking("UpdateCrossFadeState"))
			InvokeRepeating("UpdateCrossFadeState", 0f, 1f);
	}

	void CheckCross() {
		if(userUnit.AddNumber == 0){ 
			canCrossed = false;
		}
		else{
			canCrossed = true;;
		}
	}

	protected virtual void UpdatEnableState(){
		maskSpr.enabled = !isEnable;
		//new code by leiliang
		UIEventListenerCustom listener = UIEventListenerCustom.Get (gameObject);
		listener.LongPress = PressItem;
		if (isEnable) {
			listener.onClick = ClickItem;
		} else {
			listener.onClick = null;
		}
		//new code end

		//old code by lynn
//		UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
//		if(isEnable)
//			UIEventListenerCustom.Get(this.gameObject).onClick = ClickItem;
//		else
//			UIEventListenerCustom.Get(this.gameObject).onClick = null;
		//old code end
	}

	private void UpdateCrossFadeState(){
		if( userUnit == null )
			return;

		if(userUnit.AddNumber==0) {
			isCrossed = !isCrossed;
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = Color.yellow;
			return;
		}

		if(isCrossed){
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = Color.yellow;
			isCrossed = false;
		}
		else{
			crossFadeLabel.text = crossFadeAfterText;
			crossFadeLabel.color = Color.red;
			isCrossed = true;
		}
	}

	protected virtual void ClickItem(GameObject item){}
	protected virtual void PressItem(GameObject item){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, userUnit);
	}
	
	private string crossFadeBeforeText;
	private string crossFadeAfterText;
	
	private void UpdateCrossFadeText(){
		switch (currentSortRule){
			case SortRule.ID : 
				CrossFadeLevelFirst();
				break;
			case SortRule.Attack : 
				CrossFadeAttackFirst();
				break;
			case SortRule.Attribute : 
				CrossFadeLevelFirst();
				break;
			case SortRule.HP : 
				CrossFadeHpFirst();
				break;
			case SortRule.Race :
				CrossFadeLevelFirst();
				break;
			case SortRule.GetTime : 
				CrossFadeLevelFirst();
				break;
			case SortRule.AddPoint :
				CrossFadeLevelFirst();
				break;
			case SortRule.Fav : 
				CrossFadeLevelFirst();
				break;
			default:
				break;
		}
	}
	
	private void CrossFadeLevelFirst(){
		crossFadeBeforeText = "Lv" + userUnit.Level;
		if(userUnit.AddNumber == 0){ 
			canCrossed = false;
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = Color.yellow;
		}
		else {
			canCrossed = true;
			crossFadeAfterText = "+" + userUnit.AddNumber;
		}
	}

	private void CrossFadeHpFirst(){
		crossFadeBeforeText = UserUnit.Hp.ToString();
		crossFadeAfterText = "+" + userUnit.AddNumber;
	}

	private void CrossFadeAttackFirst(){
		crossFadeBeforeText = UserUnit.Attack.ToString();
		crossFadeAfterText = "+" + userUnit.AddNumber;
	}

	protected virtual void SetEmptyState(){
		IsEnable = false;
		avatarTex.mainTexture = null;
		//typeSpr.color = Color.white ;
		crossFadeLabel.text = string.Empty;
	}

	protected virtual void SetCommonState(){
		IsEnable = true;
		//Debug.LogError("gameobject: " + gameObject + " userUnit : " + userUnit.ID);
		avatarTex.mainTexture = userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		//typeSpr.color = DGTools.TypeToColor(userUnit.UnitInfo.Type);
		CurrentSortRule = SortRule.ID;
	}

	private void ShowUnitType(){
		switch (userUnit.UnitInfo.Type){
			case EUnitType.UFIRE :
				unitTypeBg.spriteName = "avatar_bg_1";
				break;
			case EUnitType.UWATER :
				unitTypeBg.spriteName = "avatar_bg_2";
				break;
			case EUnitType.UWIND :
				unitTypeBg.spriteName = "avatar_bg_3";
				break;
			case EUnitType.ULIGHT :
				unitTypeBg.spriteName = "avatar_bg_4";
				break;
			case EUnitType.UDARK :
				unitTypeBg.spriteName = "avatar_bg_5";
				break;
			case EUnitType.UNONE :
				unitTypeBg.spriteName = "avatar_bg_6";
				break;
			default:
				break;
		}
	}

}
