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
				itemPrefab = Resources.Load(sourcePath) as GameObject;
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
		baseBoardSpr = transform.FindChild("Base_Board").GetComponent<UISprite>();
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
				friendTypeLabel.text = "SUPPORT";
				friendTypeLabel.color = new Color(255.0f/255, 202.0f/255, 98.0f/255);
				friendPointLabel.color = new Color(255.0f/255, 202.0f/255, 98.0f/255);
				baseBoardSpr.spriteName = UIConfig.SPR_NAME_BASEBOARD_HELPER;
				break;
			case bbproto.EFriendState.ISFRIEND : 
				friendTypeLabel.text = "FRIEND";
				friendTypeLabel.color = new Color(223.0f/255, 223.0f/255, 223.0f/255);
				friendPointLabel.color = new Color(223.0f/255, 223.0f/255, 223.0f/255);
				baseBoardSpr.spriteName = UIConfig.SPR_NAME_BASEBOARD_Friend;
				break;
			default:
				friendTypeLabel.text = string.Empty;
				baseBoardSpr.spriteName = string.Empty;
				break;
		}
	}

	private void SetFriendPoint(){
		if(friendInfo.FriendPoint != 0){
			friendPointLabel.text = string.Format("{0}PT", friendInfo.FriendPoint.ToString());
		}
		else{
			friendPointLabel.text = string.Empty;
		}
	}

	private void SetFriendRank(){
		rankLabel.text = "RANK : " + friendInfo.Rank;
	}

}
