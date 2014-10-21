using UnityEngine;
using System.Collections;
using bbproto;

public class FriendUnitItem : BaseUnitItem {
	protected UILabel nameLabel;
//	public delegate void UnitItemCallback(FriendUnitItem huv);
	protected DataListener callback;

	public static FriendUnitItem Inject(GameObject item){
		FriendUnitItem view = item.AddComponent<FriendUnitItem>();
		if (view == null)
			view = item.AddComponent<FriendUnitItem>();
		return view;
	}

	protected FriendInfo friendInfo;
	public FriendInfo FriendInfo{
		get{ 
			return friendInfo; 
		}
		set{ 
			friendInfo = value; 
		}
	}
		
	public override void SetData<T>(T friendInfo, params object[] args){
		this.friendInfo = friendInfo as FriendInfo;
		base.SetData(this.friendInfo == null ? null : this.friendInfo.UserUnit);
		if(args.Length > 0)
			callback += (DataListener)args [0];
		tag = "Untagged";
		if (name == "0") {
			tag = "friend_one";
		}
	}

	protected override void InitUI(){
		base.InitUI();
		nameLabel = transform.FindChild("Label_Name").GetComponent<UILabel>();
		UIEventListenerCustom.Get (gameObject).onClick = ClickItem;
		SetName();
	}

	protected override void ClickItem(GameObject item){
		if(callback != null) {
			callback(this);
		}
	}

	protected override void SetEmptyState(){
		base.SetEmptyState();
		nameLabel.text = string.Empty;
	}

	protected override void SetCommonState(){
		base.SetCommonState();
		SetName();
	}

	private void SetName(){
		if(friendInfo == null || string.IsNullOrEmpty(friendInfo.nickName)){
			nameLabel.text = "NONAME";
		}
		else{
			nameLabel.text = friendInfo.nickName;
		}
	}
}
