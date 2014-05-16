using UnityEngine;
using System.Collections;

public class HelperUnitItem : FriendUnitItem {
	private UILabel typeLabel;
	private UILabel pointLabel;
	private UILabel rankLabel;
	public delegate void UnitItemCallback(HelperUnitItem huv);
	public UnitItemCallback callback;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	protected override void SetEmptyState(){
		base.SetEmptyState();
		typeLabel.text = string.Empty;
		pointLabel.text = string.Empty;
	}
	protected override void SetCommonState(){
		base.SetCommonState();
		InitFriendType();
		InitFriendPoint();
		InitRank();
	}
	
	private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get { 
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/HelperUnitPrefab";
				itemPrefab = Resources.Load(sourcePath) as GameObject ;
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
		typeLabel = transform.FindChild("Label_Friend_Type").GetComponent<UILabel>();
		pointLabel = transform.FindChild("Label_Friend_Point").GetComponent<UILabel>();
		rankLabel = transform.FindChild("Label_Rank").GetComponent<UILabel>();
	}

	protected override void InitState(){
		base.InitState();
		InitFriendType();
		InitFriendPoint();
		InitRank();
	}

	private void InitFriendType(){
		switch (friendInfo.FriendState) {
			case bbproto.EFriendState.FRIENDHELPER : 
				typeLabel.text = "SUPPORT";
				typeLabel.color = Color.green;
				pointLabel.color = Color.green;
				break;
			case bbproto.EFriendState.ISFRIEND : 
				typeLabel.text = "FRIEND";
				typeLabel.color = Color.yellow;
				pointLabel.color = Color.yellow;
				break;
			default:
				typeLabel.text = string.Empty;
				break;
		}
	}

	private void InitFriendPoint(){
		if(friendInfo.FriendPoint != 0){
			pointLabel.text = string.Format("{0}PT", friendInfo.FriendPoint.ToString());
		}
		else{
			pointLabel.text = string.Empty;
		}
	}

	private void InitRank(){
		rankLabel.text = "RANK : " + friendInfo.Rank;
	}


}
