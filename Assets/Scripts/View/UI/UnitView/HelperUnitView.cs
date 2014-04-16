using UnityEngine;
using System.Collections;

public class HelperUnitView : FriendUnitView {
	private UILabel typeLabel;
	private UILabel pointLabel;
	public delegate void UnitItemCallback(HelperUnitView huv);
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

	public static HelperUnitView Inject(GameObject item){
		HelperUnitView view = item.AddComponent<HelperUnitView>();
		if (view == null)
			view = item.AddComponent<HelperUnitView>();
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
	}

	protected override void InitState(){
		base.InitState();
		InitFriendType();
		InitFriendPoint();
	}

	private void InitFriendType(){
		switch (friendInfo.FriendState) {
			case bbproto.EFriendState.FRIENDHELPER : 
				typeLabel.text = "Support";
				typeLabel.color = Color.green;
				pointLabel.color = Color.green;
				break;
			case bbproto.EFriendState.ISFRIEND : 
				typeLabel.text = "Friend";
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
			pointLabel.text = string.Format("{0}pt", friendInfo.FriendPoint.ToString());
		}
		else{
			pointLabel.text = string.Empty;
		}
	}
}
