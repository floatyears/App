using UnityEngine;
using System.Collections.Generic;

public class QuestSelectDecoratorUnity : UIComponentUnity ,IUICallback{

	public static UIImageButton btnSelect;
	IUICallback iuiCallback;
	bool temp = false;
	private UILabel labDoorName;
	private UILabel labDoorType;
	private UILabel labFloorVaule;
	private UILabel labStaminaVaule;
	private UILabel labStoryContent;
	private UILabel labQuestInfo;
	private UILabel labRewardInfo;

	public override void Init (UIInsConfig config, IUIOrigin origin) {
		base.Init (config, origin);
		InitUI();

		temp = origin is IUICallback;
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI(){

		btnSelect = FindChild<UIImageButton>("btn_friend_select"); //FindChild( "btn_friend_select" ).GetComponent< UIImageButton >();
		btnSelect.isEnabled = false;

		labDoorName = FindChild( "title/Label_door_name").GetComponent< UILabel>();
		labDoorName.text = string.Empty;
		labDoorType = FindChild("title/Label_door_type_name" ).GetComponent< UILabel >();
		labDoorType.text = string.Empty;

		labFloorVaule = FindChild("window_left/Label_floor_V").GetComponent< UILabel >();
		labFloorVaule.text = string.Empty;

		labStaminaVaule = FindChild("window_left/Label_stamina_V").GetComponent< UILabel >();
		labStaminaVaule.text = string.Empty;

		labStoryContent = FindChild("window_right/content_story/Label_story").GetComponent< UILabel >();
		labStoryContent.text = string.Empty;

		labQuestInfo = FindChild("window_right/content_detail/Label_quest_info").GetComponent< UILabel >();
		labQuestInfo.text = string.Empty;

		labRewardInfo = FindChild("window_right/content_detail/Label_reward_info").GetComponent< UILabel >();
		labRewardInfo.text = string.Empty;

		UIEventListener.Get( btnSelect.gameObject ).onClick = OnClickCallback;

	}

	void OnClickCallback( GameObject caller ) {
		if (!temp) {
			return;
		}
		
		SceneEnum se = SceneEnum.FriendSelect;
		
		if (iuiCallback == null) {
			iuiCallback = origin as IUICallback;
		} 
		
		iuiCallback.Callback(se);
		//Debug.Log("QuestSelectDecoratorUnity  "+se.ToString());
	}
	
	public void Callback (object data)
	{
		bool b = (bool)data;
		btnSelect.isEnabled = b;
	}

}
