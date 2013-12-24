using UnityEngine;
using System.Collections;

public class QuestSelectUnity : UIBaseUnity
{
	private UIImageButton selectBtn;

	private GameObject items;

	private GameObject questBossPrefab;

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
	private UILabel labStory;

	public override void Init (string name)
	{
		base.Init (name);

		//find labels
		labFriendSelect = FindChild<UILabel>("btn/Label_friend_select");
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
		labStory = FindChild<UILabel>("window_right/content_story/Label_story");

		selectBtn = transform.FindChild("Btn_Friend_Select").GetComponent<UIImageButton>();
		selectBtn.isEnabled = false;

		items = transform.FindChild("QuestItems").gameObject;

		questBossPrefab = Resources.Load("Prefabs/QuestBossItem") as GameObject;

		ShowQuest(Random.Range(1,5));

	}

	void ShowQuest(int num = 1)
	{
		if(questBossPrefab == null)
		{
			LogHelper.LogError("Quest List Item prefab is Null, Return...");
			return;
		}
		
		for( int i = 0; i < num; i++ )
		{
			GameObject ins = GameObject.Instantiate(questBossPrefab) as GameObject;

			ins.transform.parent = items.transform;
			float pos_x = -250F + 140*i;
			float pos_y = -116F;
			float pos_z = 0;
			ins.transform.localPosition = new Vector3(pos_x,pos_y,pos_z);
			ins.transform.localScale = Vector3.one;

			ins.name = i.ToString();
			ins.SetActive(true);

			UIEventListener.Get(ins).onClick = QuestBossItemClick;
			UIEventListener.Get(selectBtn.gameObject).onClick = SelectBtnClick;
		}
	}
	void QuestBossItemClick(GameObject btn)
	{
		LogHelper.Log("click");

		selectBtn.isEnabled = true;
	}

	void SelectBtnClick(GameObject btn)
	{
		LogHelper.Log("Btn Click, change Friend Select....");
		//Change Scene to Friend Select

		ControllerManager.Instance.ChangeScene(SceneEnum.FriendSelect);
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
