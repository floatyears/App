using UnityEngine;
using System.Collections;

public class QuestSelectUnity : UIBaseUnity
{
	private UILabel labFriendSelect;
	private UILabel labDoorName;
	private UILabel labDoorTypeName;
	private UILabel labBoss;
	private UILabel labFloorText;
	private UILabel labFloorVaule;
	private UILabel labStaminaText;
	private UILabel labStaminaVaule;
	private UILabel labEnemy;
	private UILabel labQuestInfo;
	private UILabel labQuestName;
	private UILabel labReward;
	private UILabel labRewardInfo;
	private UILabel labStoryIntroduction;

	public override void Init (string name)
	{
		base.Init (name);

		//find labels
		labFriendSelect = FindChild<UILabel>("btn_friend_select/Label_friend_select");
		labDoorName = FindChild<UILabel>("title/Label_door_name");
		labDoorTypeName = FindChild<UILabel>("title/Label_door_type_name");
		labBoss = FindChild<UILabel>("window_left/Label_boss");
		labFloorText = FindChild<UILabel>("window_left/Label_floor_T");
		labFloorVaule = FindChild<UILabel>("window_left/Label_floor_V");
		labStaminaText = FindChild<UILabel>("window_left/Label_stamina_T");
		labStaminaVaule = FindChild<UILabel>("window_left/Label_stamina_V");
		labEnemy = FindChild<UILabel>("window_right/content_detail/Label_enemy");
		labQuestInfo = FindChild<UILabel>("window_right/content_detail/Label_quest_info");
		labQuestName = FindChild<UILabel>("window_right/content_detail/Label_quest_name");
		labReward = FindChild<UILabel>("window_right/content_detail/Label_reward");
		labRewardInfo = FindChild<UILabel>("window_right/content_detail/Label_reward_info");
		labStoryIntroduction = FindChild<UILabel>("window_right/content_story/Label_story");

		CleanPanelInfo();

	}
	public void CleanPanelInfo()
	{
		labQuestInfo.text = string.Empty;
		labRewardInfo.text = string.Empty;
		labStoryIntroduction.text = string.Empty;
		labStaminaVaule.text = string.Empty;
		labFloorVaule.text = string.Empty;
	}
	public void UpdatePanelInfo()
	{
		labQuestInfo.text = "Quest Information";
		labRewardInfo.text = "Reward information";
		labStoryIntroduction.text = "This is Story Information.This is Story Information.This is Story Information." +
			"This is Story Information.This is Story Information.This is Story Information.This is Story Information.";
		labStaminaVaule.text = "5";
		labFloorVaule.text = "2";
	}
	public override void ShowUI ()
	{
		base.ShowUI ();
	}

	public override void HideUI ()
	{
		base.HideUI ();

	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}
}
