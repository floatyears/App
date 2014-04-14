using UnityEngine;
using System.Collections;

public class HelperUnitView : UnitView {
	private UILabel nameLabel;
	private UILabel typeLabel;
	private UILabel pointLabel;

	public delegate void UnitItemCallback(HelperUnitView huv);
	public UnitItemCallback callback;
	
	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}
	
	private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get { 
			if(itemPrefab == null) {
				itemPrefab = Resources.Load("Prefabs/UI/UnitItem/HelperUnitPrefab") as GameObject ;
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

	private TFriendInfo friendInfo;
	public TFriendInfo FriendInfo{
		get{ return friendInfo; }
		set{ friendInfo = value; }
	}

	private string helperName;
	public string HelperName{
		get{ return helperName; }
		set{ helperName = value; }
	}

	private int friendPoint;
	public int FriendPoint{
		get{ return friendPoint; }
		set{ friendPoint = value;}
	}

	protected override void InitUI(){
		base.InitUI();
		nameLabel = transform.FindChild("Label_Name").GetComponent<UILabel>();
		typeLabel = transform.FindChild("Label_Friend_Type").GetComponent<UILabel>();
		pointLabel = transform.FindChild("Label_Friend_Point").GetComponent<UILabel>();
	}

	protected override void InitState(){
		base.InitState();
		if(string.IsNullOrEmpty(friendInfo.NickName)) nameLabel.text = "NoName";
		else nameLabel.text = friendInfo.NickName;

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
		if(friendInfo.FriendPoint != 0){
			pointLabel.text = string.Format("{0}pt", friendInfo.FriendPoint.ToString());
		}
		else{
			pointLabel.text = string.Empty;
		}
	}
	
	public void Init(TFriendInfo friendInfo){
		this.friendInfo = friendInfo;
		base.Init(friendInfo.UserUnit);
	}

}
