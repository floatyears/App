using UnityEngine;
using System.Collections;
using bbproto;

public class HelperUnitItem : FriendUnitItem {
	private UILabel raceLabel;
	private UILabel arrtLabel;
	private UISprite baseBoardSpr;
	private UILabel descLabel;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	protected override void SetEmptyState(){
		base.SetEmptyState();
		raceLabel.text = string.Empty;
		arrtLabel.text = string.Empty;
	}

	protected override void SetCommonState(){
		base.SetCommonState();
		SetFriendType();

		if(friendInfo.friendPoint != 0){
			arrtLabel.text = string.Format("{0}" + TextCenter.GetText("Text_Point"), friendInfo.friendPoint.ToString());
		}
		else{
			arrtLabel.text = string.Empty;
		}

	}

	public override void SetData<T>(T friendInfo, params object[] args){

		base.SetData<T>(friendInfo, args);

		if (args.Length > 1) {
			switch(args[1].ToString()){
			case "level_up":
				descLabel.text = "";
				break;
			case "evolve":
				break;
			case "quest":
				break;
			}
		}
	}

	protected override void InitUI(){
		nameLabel = transform.FindChild("Label_Name").GetComponent<UILabel>();
		avatar = transform.FindChild("Avatar").GetComponent<UISprite>();
		descLabel = transform.FindChild("Label_Desc").GetComponent<UILabel>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		avatarBorderSpr = transform.FindChild("Sprite_Avatar_Border").GetComponent<UISprite>();
		avatarBg = transform.FindChild("Background").GetComponent<UISprite>();
		crossFadeLabel = transform.FindChild("Label_Lv").GetComponent<UILabel>();

		raceLabel = transform.FindChild("Label_Race").GetComponent<UILabel>();
		arrtLabel = transform.FindChild("Label_Attr").GetComponent<UILabel>();
		baseBoardSpr = transform.FindChild("Sprite_Base_Board").GetComponent<UISprite>();

		transform.FindChild ("SelectBtn/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Select");

		UIEventListenerCustom.Get(transform.FindChild("SelectBtn").gameObject).onClick = ClickItem;
	}

	private void SetFriendType(){

		raceLabel.color = new Color(36.0f/255, 26.0f/255, 30.0f/255);
		arrtLabel.color = new Color(36.0f/255, 26.0f/255, 30.0f/255);
		switch (friendInfo.friendState) {
			case bbproto.EFriendState.FRIENDHELPER : 
				raceLabel.text = TextCenter.GetText("Text_Support");
//				rankLabel.color = Color.white;
				nameLabel.color = Color.white;
				baseBoardSpr.spriteName = UIConfig.SPR_NAME_BASEBOARD_HELPER;
				break;
			case bbproto.EFriendState.ISFRIEND : 
				raceLabel.text = TextCenter.GetText("Text_Friend");
//				rankLabel.color = new Color(229.0f/255, 184.0f/255, 78.0f/255);
				nameLabel.color = new Color(229.0f/255, 184.0f/255, 78.0f/255);
				baseBoardSpr.spriteName = UIConfig.SPR_NAME_BASEBOARD_FRIEND;
				break;
			default:
				raceLabel.text = string.Empty;
				baseBoardSpr.spriteName = string.Empty;
				break;
		}
	}


}
