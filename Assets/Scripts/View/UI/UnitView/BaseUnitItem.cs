using UnityEngine;
using System.Collections;
using bbproto;

public class BaseUnitItem : MonoBehaviour {
	protected const string emptyBorder = "unit_empty_bg";
//	protected const string normalBorder = "avatar_border_none";

	protected bool canCrossed = true;
	protected bool isCrossed;
	protected UISprite avatar;
	protected UISprite avatarBorderSpr;
	protected UISprite avatarBg;
	protected UISprite maskSpr;
	protected UILabel crossFadeLabel;

//	public static bool canShowUnitDetail = true;

	public static BaseUnitItem Inject(GameObject item){
		BaseUnitItem view = item.GetComponent<BaseUnitItem>();
		if (view == null) view = item.AddComponent<BaseUnitItem>();
		return view;
	}

	protected virtual void Awake() {
		if(maskSpr == null){ InitUI(); }
	}
	
	private void FindUIElement() {
		avatar = transform.FindChild("Avatar").GetComponent<UISprite>();
		crossFadeLabel = transform.FindChild("Label_Cross_Fade").GetComponent<UILabel>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		avatarBorderSpr = transform.FindChild("Sprite_Avatar_Border").GetComponent<UISprite>();
		avatarBg = transform.FindChild("Background").GetComponent<UISprite>();
	}

	protected TUserUnit userUnit;
	public TUserUnit UserUnit {
		get{
			return userUnit;
		}
		set{
			userUnit = value;
			RefreshState();
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

	protected virtual void RefreshState() {
		if(userUnit == null){
			SetEmptyState();
			return;
		}
		SetCommonState();
	}
	private void ExecuteCrossFade() {
		if (! IsInvoking("UpdateCrossFadeState"))
			InvokeRepeating("UpdateCrossFadeState", 0f, 1f);
	}

	void CheckCross() {
		if(userUnit.AddNumber == 0) { 
			canCrossed = false;
		}
		else {
			canCrossed = true;
		}
	}

	protected virtual void UpdatEnableState() {
//		Debug.LogError ("UpdatEnableState : " + IsEnable);

		maskSpr.enabled = !IsEnable;
		UIEventListenerCustom listener = UIEventListenerCustom.Get (gameObject);
		listener.LongPress = PressItem;

		if (IsEnable) {
			listener.onClick = ClickItem;
		} else {
			listener.onClick = PlayClickAudio;
		}
	}

	void PlayClickAudio(GameObject go) {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click_invalid);
	}

	private void UpdateCrossFadeState(){
		if( userUnit == null )
			return;

		if(userUnit.AddNumber==0) {
			isCrossed = !isCrossed;
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = new Color(223.0f/255, 223.0f/255, 223.0f/255);
			return;
		}

		if(isCrossed){
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = new Color(223.0f/255, 223.0f/255, 223.0f/255);
			isCrossed = false;
		}
		else{
			crossFadeLabel.text = crossFadeAfterText;
			crossFadeLabel.color = new Color(255.0f/255, 89.0f/255, 98.0f/255);
			isCrossed = true;
		}
	}

	protected virtual void ClickItem(GameObject item){}
	protected virtual void PressItem(GameObject item){
//		Debug.LogError ("hp : " + userUnit.Hp + " atk : " + userUnit.UnitInfo.GetCurveValue(userUnit.Level,userUnit.UnitInfo.Object.powerType));
		if (userUnit == null) {
			return;	
		}

//		Debug.LogError ("PressItem : " + userUnit.UnitInfo.ID + " avatar : " + avatar.spriteName);

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
		avatar.spriteName = string.Empty;
		avatarBorderSpr.spriteName = emptyBorder;
		crossFadeLabel.text = string.Empty;
	}

	protected virtual void SetCommonState(){
		IsEnable = userUnit.isEnable;
		avatarBorderSpr.spriteName = GetBorderSpriteName ();
		avatarBg.spriteName = GetAvatarBgSpriteName ();
		ExecuteCrossFade ();
//		Debug.LogError ("gameobject : " + gameObject + "set common state : " + avatar.spriteName + " userUnit.UnitID : " + userUnit.UnitID);
		DataCenter.Instance.GetAvatarAtlas(userUnit.UnitID, avatar);
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

	public static void SetAvatarSprite(UISprite sprite, UIAtlas asset, uint ID) {
//		UIAtlas atlas = asset as UIAtlas;
//		if (ID == 216) {
//			Debug.LogError("atlas : " + asset);	
//		}
//		Debug.LogError ("SetAvatarSprite : UIAtlas :  " + asset);
		if (asset == null) {
			return;	
		}
		sprite.atlas = asset;
		sprite.spriteName = ID.ToString();
	
	}

	private void ShowUnitType(){
		switch (userUnit.UnitInfo.Type){
			case EUnitType.UFIRE :
				avatarBg.spriteName = "avatar_bg_fire";
				avatarBorderSpr.spriteName = "avatar_border_fire";
				break;
			case EUnitType.UWATER :
				avatarBg.spriteName = "avatar_bg_water";
				avatarBorderSpr.spriteName = "avatar_border_water";

				break;
			case EUnitType.UWIND :
				avatarBg.spriteName = "avatar_bg_wind";
				avatarBorderSpr.spriteName = "avatar_border_wind";

				break;
			case EUnitType.ULIGHT :
				avatarBg.spriteName = "avatar_bg_light";
				avatarBorderSpr.spriteName = "avatar_border_light";

				break;
			case EUnitType.UDARK :
				avatarBg.spriteName = "avatar_bg_dark";
				avatarBorderSpr.spriteName = "avatar_border_dark";

				break;
			case EUnitType.UNONE :
				avatarBg.spriteName = "avatar_bg_none";
				avatarBorderSpr.spriteName = "avatar_border_none";

				break;
			default:
				break;
		}
	}

//	public static string GetUserUnitBorderName(EUnitType unitType) {
//		switch (userUnit.UnitInfo.Type){
//		case EUnitType.UFIRE :
//			avatarBg.spriteName = "avatar_bg_1";
//			avatarBorderSpr.spriteName = "avatar_border_1";
//			break;
//		case EUnitType.UWATER :
//			avatarBg.spriteName = "avatar_bg_2";
//			avatarBorderSpr.spriteName = "avatar_border_2";
//			
//			break;
//		case EUnitType.UWIND :
//			avatarBg.spriteName = "avatar_bg_3";
//			avatarBorderSpr.spriteName = "avatar_border_3";
//			
//			break;
//		case EUnitType.ULIGHT :
//			avatarBg.spriteName = "avatar_bg_4";
//			avatarBorderSpr.spriteName = "avatar_border_4";
//			
//			break;
//		case EUnitType.UDARK :
//			avatarBg.spriteName = "avatar_bg_5";
//			avatarBorderSpr.spriteName = "avatar_border_5";
//			
//			break;
//		case EUnitType.UNONE :
//			avatarBg.spriteName = "avatar_bg_6";
//			avatarBorderSpr.spriteName = "avatar_border_6";
//			
//			break;
//		default:
//			break;
//	}
}
