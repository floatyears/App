using UnityEngine;
using System.Collections;
using bbproto;

public class LevelUpItem : MyUnitItem {

	public DataListener callback;
	public DataListener delCallback;

	private GameObject delBtn;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}
	
	protected override void InitUI(){
		avatar = transform.FindChild("Avatar").GetComponent<UISprite>();
		crossFadeLabel = transform.FindChild("Label_Cross_Fade").GetComponent<UILabel>();
		avatarBorderSpr = transform.FindChild("Sprite_Avatar_Border").GetComponent<UISprite>();
		avatarBg = transform.FindChild("Background").GetComponent<UISprite>();

		partyLabel = transform.FindChild("Label_Party").GetComponent<UILabel>();
		partyLabel.enabled = false;
		partyLabel.text = TextCenter.GetText("Text_Party");
		partyLabel.color = new Color (0.9333f, 0.192f, 0.192f);

		partyLabel.enabled = true;
		partyLabel.text = "";
		delBtn = transform.FindChild ("DelIcon").gameObject;
		UIEventListenerCustom.Get (delBtn).onClick = ClickDel;
		UIEventListenerCustom.Get (gameObject).LongPress = PressItem;
		UIEventListenerCustom.Get (gameObject).onClick = ClickItem;
	}

	protected override void UpdateFavoriteState(){
//		lockSpr.spriteName = isFavorite ? "Lock_close" : "Lock_open";
	}

	protected override void UpdateFocus(){
		lightSpr.enabled = IsFocus;
	}

	protected override void SetEmptyState(){
		avatar.spriteName = string.Empty;
		avatarBorderSpr.spriteName = emptyBorder;
		avatarBg.spriteName = "avatar_bg_none"; 
		crossFadeLabel.text = string.Empty;
	}
	
	protected override void SetCommonState(){
		avatarBorderSpr.spriteName = GetBorderSpriteName ();
		avatarBg.spriteName = GetAvatarBgSpriteName ();
		
		ResourceManager.Instance.GetAvatarAtlas(userUnit.unitId, avatar);
	}

	protected override void UpdatePartyState(){
		partyLabel.enabled = IsParty;
	}

	public override void SetData<T> (T data, params object[] args)
	{
		if (avatar == null) {
			InitUI();	
		}
		userUnit = data as UserUnit;
		if (userUnit == null) {
			delBtn.SetActive(false);	
		}else{
			delBtn.SetActive(true);
		}
		RefreshState ();

		if(args.Length > 0){
			delCallback = args[0] as DataListener;
		}
		
	}

	void ClickDel(GameObject obj){
		if(delCallback != null) {
			delCallback(this);
		}
	}

}
