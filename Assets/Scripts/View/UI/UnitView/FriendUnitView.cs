using UnityEngine;
using System.Collections;

public class FriendUnitView : UnitView {
	private UILabel nameLabel;
	public delegate void UnitItemCallback(FriendUnitView huv);
	public UnitItemCallback callback;

	public static FriendUnitView Inject(GameObject item){
		FriendUnitView view = item.AddComponent<FriendUnitView>();
		if (view == null)
			view = item.AddComponent<FriendUnitView>();
		return view;
	}

	protected TFriendInfo friendInfo;
	public TFriendInfo FriendInfo{
		get{ return friendInfo; }
		set{ friendInfo = value; }
	}
		
	public void Init(TFriendInfo friendInfo){
		this.friendInfo = friendInfo;
		base.Init(friendInfo.UserUnit);
	}

	protected override void InitUI(){
		base.InitUI();
		nameLabel = transform.FindChild("Label_Name").GetComponent<UILabel>();
	}
	
	protected override void InitState(){
		base.InitState();
		if(string.IsNullOrEmpty(friendInfo.NickName)){
			nameLabel.text = "NoName";
		}
		else{
			nameLabel.text = friendInfo.NickName;
		}
	}

	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get { 
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/FriendUnitPrefab";
				itemPrefab = Resources.Load(sourcePath) as GameObject ;
			}
			return itemPrefab;
		}
	}

	protected override void SetEmptyState(){
		base.SetEmptyState();
		nameLabel.text = string.Empty;
	}

	protected override void SetCommonState(){
		base.SetCommonState();
		nameLabel.text = friendInfo.NickName;
	}
}
