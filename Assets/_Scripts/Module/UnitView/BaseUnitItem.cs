using UnityEngine;
using System.Collections;
using bbproto;

public class BaseUnitItem : DragPanelItemBase {
	protected const string emptyBorder = "unit_empty_bg";

	protected bool canCrossed = true;
	protected bool isCrossed;
	protected UISprite avatar;
	protected UISprite avatarBorderSpr;
	protected UISprite avatarBg;
	protected UISprite maskSpr;
	protected UILabel crossFadeLabel;

	protected UserUnit userUnit;

	public UserUnit UserUnit{
		get{
			return userUnit;
		}
	}

	public override void SetData<T>(T data, params object[] args){
		if (maskSpr == null) {
			InitUI();	
		}
		userUnit = data as UserUnit;
		RefreshState();
	}

	public override void ItemCallback (params object[] args)
	{
//		throw new System.NotImplementedException ();
	}

	private bool isEnable;
	public bool IsEnable{
		get{
			return isEnable;
		}
		set{
			if(value != isEnable){
				isEnable = value;
				UpdatEnableState();
			}

		}
	}
	
	private SortRule currentSortRule;
	public SortRule CurrentSortRule{
		get{
			return currentSortRule;
		}
		set{
			if(currentSortRule !=  value){
				currentSortRule = value;
				UpdateCrossFadeText();
			}

		}
	}

	protected virtual void InitUI(){
		avatar = transform.FindChild("Avatar").GetComponent<UISprite>();
		crossFadeLabel = transform.FindChild("Label_Cross_Fade").GetComponent<UILabel>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		avatarBorderSpr = transform.FindChild("Sprite_Avatar_Border").GetComponent<UISprite>();
		avatarBg = transform.FindChild("Background").GetComponent<UISprite>();


		UIEventListenerCustom listener = UIEventListenerCustom.Get (gameObject);
		//		Debug.LogError ("GameObject : " + gameObject + "parent : " + transform.parent + " parent 2 : " + transform.parent.parent +" -- UpdatEnableState : maskSpr -- " + maskSpr + " -- listener : -- " + listener);

		listener.LongPress = PressItem;
		listener.onClick = ClickObjItem;

	}

	protected virtual void RefreshState() {
		if(userUnit == null){
			SetEmptyState();
		}else{
			SetCommonState();
		}

	}

	protected virtual void UpdatEnableState() {
		maskSpr.enabled = !isEnable;
	}

	private void UpdateCrossFadeState(){
		if( userUnit == null )
			return;

		if(userUnit.AddNumber==0) {
			isCrossed = !isCrossed;
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = new Color(223.0f/255f, 223.0f/255f, 223.0f/255f);
			return;
		}

		if(isCrossed){
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = new Color(223.0f/255f, 223.0f/255f, 223.0f/255f);
			isCrossed = false;
		}
		else{
			crossFadeLabel.text = crossFadeAfterText;
			crossFadeLabel.color = new Color(255.0f/255f, 89.0f/255f, 98.0f/255f);
			isCrossed = true;
		}
	}

	private void ClickObjItem(GameObject item){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click_invalid);
		if(isEnable)
			ClickItem (item);
	}

	protected virtual void ClickItem(GameObject item){

	}

	protected virtual void PressItem(GameObject item){
//		Debug.LogError ("userUnit == null : " + (userUnit == null));
		if (userUnit == null) {
			return;	
		}

		ModuleManager.Instance.ShowModule(ModuleEnum.UnitDetailModule,"unit",userUnit);
	}

	private string crossFadeBeforeText;
	private string crossFadeAfterText;
	
	private void UpdateCrossFadeText(){
		switch (currentSortRule){
			case SortRule.ID : 
			case SortRule.Attribute : 
			case SortRule.Race :
			case SortRule.GetTime :
			case SortRule.AddPoint :
			case SortRule.Fav : 
				crossFadeBeforeText = "Lv" + userUnit.level;
				if(userUnit.AddNumber == 0){ 
					canCrossed = false;
					crossFadeLabel.text = crossFadeBeforeText;
					crossFadeLabel.color = Color.yellow;
				}
				else {
					canCrossed = true;
					crossFadeAfterText = "+" + userUnit.AddNumber;
				}
				break;
			case SortRule.Attack : 
				crossFadeBeforeText = userUnit.Attack.ToString();
				crossFadeAfterText = "+" + userUnit.AddNumber;
				break;
			case SortRule.HP : 
				crossFadeBeforeText = userUnit.Hp.ToString();
				crossFadeAfterText = "+" + userUnit.AddNumber;
				break;
			default:
				break;
		}
	}


	protected virtual void SetEmptyState(){
		IsEnable = false;
		avatar.spriteName = string.Empty;
		avatarBorderSpr.spriteName = emptyBorder;
		crossFadeLabel.text = string.Empty;
	}

	protected virtual void SetCommonState(){
		IsEnable = userUnit.isEnable;
//		if (IsEnable) Debug.LogWarning(">>>>> userUnit.ID:"+userUnit.ID+" SetCommonState to!");
		avatarBorderSpr.spriteName = GetBorderSpriteName ();
		avatarBg.spriteName = GetAvatarBgSpriteName ();

		if (! IsInvoking("UpdateCrossFadeState"))
			InvokeRepeating("UpdateCrossFadeState", 0f, 1f);

//		Debug.LogError ("gameobject : " + gameObject + "set common state : " + avatar.spriteName + " userUnit.UnitID : " + userUnit.UnitID);
		ResourceManager.Instance.GetAvatarAtlas(userUnit.unitId, avatar);
	}

	string GetBorderSpriteName () {
		switch (userUnit.UnitType) {
		case 1:
			return "avatar_border_fire";
		case 2:
			return "avatar_border_water";
		case 3:
			return "avatar_border_wind";
		case 4:
			return "avatar_border_light";
		case 5:
			return "avatar_border_dark";
		case 6:
			return "avatar_border_none";
		default:
			return "avatar_border_none";
			break;
		}
	}

	string GetAvatarBgSpriteName() {
		switch (userUnit.UnitType) {
		case 1:
			return "avatar_bg_fire";
		case 2:
			return "avatar_bg_water";
		case 3:
			return "avatar_bg_wind";
		case 4:
			return "avatar_bg_light";
		case 5:
			return "avatar_bg_dark";
		case 6:
			return "avatar_bg_none";
		default:
			return "avatar_bg_none";
			break;
		}
	}



}
