using UnityEngine;
using System.Collections;
using bbproto;

public class FriendUnitItem : BaseUnitItem {
	protected UILabel nameLabel;
	public delegate void UnitItemCallback(FriendUnitItem huv);
	public UnitItemCallback callback;

	public static FriendUnitItem Inject(GameObject item){
		FriendUnitItem view = item.AddComponent<FriendUnitItem>();
		if (view == null)
			view = item.AddComponent<FriendUnitItem>();
		return view;
	}

	protected FriendInfo friendInfo;
	public FriendInfo FriendInfo{
		get{ return friendInfo; }
		set{ friendInfo = value; }
	}
		
	public void Init(FriendInfo friendInfo){
		this.friendInfo = friendInfo;
		base.Init(friendInfo.UserUnit);
	}

	protected override void InitUI(){
		base.InitUI();
		nameLabel = transform.FindChild("Label_Name").GetComponent<UILabel>();
	}
	
	protected override void InitState(){
		base.InitState();
		SetName();
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
				itemPrefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject ;
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
		SetName();
	}

	private void SetName(){
		if(string.IsNullOrEmpty(friendInfo.nickName)){
			nameLabel.text = "NONAME";
		}
		else{
			nameLabel.text = friendInfo.nickName;
		}
	}
}
