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

	private GameObject contentDetail;
	private GameObject contentStory;

	private GameObject detail_low_light;
	private GameObject story_low_light;

	private DragPanel questSelectScroller;
	private GameObject questItem;

	public override void Init (UIInsConfig config, IUIOrigin origin) {

		base.Init (config, origin);
		temp = origin is IUICallback;
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		btnSelect.isEnabled = false;
		SetUIActive(true);
	}
	
	public override void HideUI () {
		base.HideUI ();

	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI(){

		btnSelect = FindChild<UIImageButton>("ScrollView/btn_quest_select"); 

		labDoorName = FindChild( "Window/title/Label_door_name").GetComponent< UILabel>();
		labDoorName.text = string.Empty;
		labDoorType = FindChild("Window/title/Label_door_type_name" ).GetComponent< UILabel >();
		labDoorType.text = string.Empty;

		labFloorVaule = FindChild("Window/window_left/Label_floor_V").GetComponent< UILabel >();
		labFloorVaule.text = string.Empty;

		labStaminaVaule = FindChild("Window/window_left/Label_stamina_V").GetComponent< UILabel >();
		labStaminaVaule.text = string.Empty;

		labStoryContent = FindChild("Window/window_right/content_story/Label_story").GetComponent< UILabel >();
		labStoryContent.text = string.Empty;

		labQuestInfo = FindChild("Window/window_right/content_detail/Label_quest_info").GetComponent< UILabel >();
		labQuestInfo.text = string.Empty;

		labRewardInfo = FindChild("Window/window_right/content_detail/Label_reward_info").GetComponent< UILabel >();
		labRewardInfo.text = string.Empty;

		UIEventListener.Get( btnSelect.gameObject ).onClick = ClickQuestSelectBtn;

		questItem = Resources.Load("Prefabs/UI/Quest/QuestScrollerItem") as GameObject;
		questSelectScroller = new DragPanel ("QuestSelectScroller", questItem);
		questSelectScroller.CreatUI();
		questSelectScroller.AddItem (3);
		questSelectScroller.RootObject.SetItemWidth(230);

		questSelectScroller.RootObject.gameObject.transform.parent = gameObject.transform.FindChild("ScrollView");
		questSelectScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		questSelectScroller.RootObject.gameObject.transform.localPosition = -95*Vector3.up;
		
		for(int i = 0; i < questSelectScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(questSelectScroller.ScrollItem[ i ].gameObject).onClick = PickQuestInfo;
		}

	}

	void ClickQuestSelectBtn( GameObject btn ) 
	{
		UIManager.Instance.ChangeScene( SceneEnum.FriendSelect );
	}
	
	public void Callback (object data)
	{
		bool b = (bool)data;
		btnSelect.isEnabled = b;
	}

	private void SetUIActive(bool b)
	{
		questSelectScroller.RootObject.gameObject.SetActive(b);
	}

	private void PickQuestInfo(GameObject go)
	{
		btnSelect.isEnabled = true;
	}
	

}
