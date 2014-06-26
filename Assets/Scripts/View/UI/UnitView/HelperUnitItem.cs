using UnityEngine;
using System.Collections;

public class HelperUnitItem : FriendUnitItem {
	private UILabel friendTypeLabel;
	private UILabel friendPointLabel;
	private UILabel rankLabel;
	private UISprite baseBoardSpr;

	public delegate void UnitItemCallback(HelperUnitItem huv);
	public UnitItemCallback callback;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}

	}

	protected override void SetEmptyState(){
		base.SetEmptyState();
		friendTypeLabel.text = string.Empty;
		friendPointLabel.text = string.Empty;
	}
	protected override void SetCommonState(){
		base.SetCommonState();
		SetFriendType();
		SetFriendPoint();
		SetFriendRank();
	}
	
	private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get { 
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/HelperUnitPrefab";
				itemPrefab = ResourceManager.Instance.LoadLocalAsset(sourcePath,null) as GameObject;
			}
			return itemPrefab;
		}
	}

	public static HelperUnitItem Inject(GameObject item){
		HelperUnitItem view = item.GetComponent<HelperUnitItem>();
		if (view == null)
			view = item.AddComponent<HelperUnitItem>();
		return view;
	}

	private int friendPoint;
	public int FriendPoint{
		get{ return friendPoint; }
		set{ friendPoint = value;}
	}

	protected override void InitUI(){
		base.InitUI();
		friendTypeLabel = transform.FindChild("Label_Friend_Type").GetComponent<UILabel>();
		friendPointLabel = transform.FindChild("Label_Friend_Point").GetComponent<UILabel>();
		rankLabel = transform.FindChild("Label_Rank").GetComponent<UILabel>();
		baseBoardSpr = transform.FindChild("Sprite_Base_Board").GetComponent<UISprite>();
	}

	protected override void InitState(){
		base.InitState();
		SetFriendType();
		SetFriendPoint();
		SetFriendRank();
	}

	private void SetFriendType(){
		switch (friendInfo.FriendState) {
			case bbproto.EFriendState.FRIENDHELPER : 
				friendTypeLabel.text = TextCenter.GetText("Text_Support");
				friendTypeLabel.color = new Color(255.0f/255, 202.0f/255, 98.0f/255);
				friendPointLabel.color = new Color(255.0f/255, 202.0f/255, 98.0f/255);
				baseBoardSpr.spriteName = UIConfig.SPR_NAME_BASEBOARD_HELPER;
				break;
			case bbproto.EFriendState.ISFRIEND : 
				friendTypeLabel.text = TextCenter.GetText("Text_Friend");
				friendTypeLabel.color = new Color(223.0f/255, 223.0f/255, 223.0f/255);
				friendPointLabel.color = new Color(223.0f/255, 223.0f/255, 223.0f/255);
				baseBoardSpr.spriteName = UIConfig.SPR_NAME_BASEBOARD_FRIEND;
				break;
			default:
				friendTypeLabel.text = string.Empty;
				baseBoardSpr.spriteName = string.Empty;
				break;
		}
	}

	private void SetFriendPoint(){
		if(friendInfo.FriendPoint != 0){
			friendPointLabel.text = string.Format("{0}" + TextCenter.GetText("Text_Point"), friendInfo.FriendPoint.ToString());
		}
		else{
			friendPointLabel.text = string.Empty;
		}
	}

	private void SetFriendRank(){
		rankLabel.text = TextCenter.GetText("Text_Rank")+": " + friendInfo.Rank;
	}

}
