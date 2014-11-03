using UnityEngine;
using System.Collections;
using bbproto;

public class AchieveAndTaskItemView : DragPanelItemBase {

	private UISprite bigIcon;
	private UILabel name;
	private UILabel progressLabel;
	private GameObject Icon;

	private UILabel btnLabel;
	private UIButton goBtnBg;
	private Color takeAwardColor = new Color(229f/255,148f/255,45f/255);

	private TaskConf data;

	private GameObject goBtn;

	private UIAtlas atlas;
	private Transform item;

	private void Init(){ 
		bigIcon = transform.FindChild ("Item/Img").GetComponent<UISprite> ();
		name = transform.FindChild ("Name").GetComponent<UILabel>();
		progressLabel = transform.FindChild ("Label_Progress").GetComponent<UILabel> ();

		transform.FindChild ("Label_Award").GetComponent<UILabel> ().text = TextCenter.GetText("Text_Award");
		btnLabel = transform.FindChild ("Go/Label").GetComponent<UILabel> ();
		goBtnBg = transform.FindChild ("Go").GetComponent<UIButton> ();

		transform.FindChild ("Label_Award").GetComponent<UILabel> ().text = TextCenter.GetText ("Label_Award");

		goBtn = transform.FindChild ("Go").gameObject;
		UIEventListenerCustom.Get (goBtn).onClick = OnGo;

		atlas = transform.FindChild ("Item/Img").gameObject.GetComponent<UISprite>().atlas;
		item = transform.FindChild( "Item" );
	}

	public override void SetData<T> (T d, params object[] args)
	{
		if (name == null)
			Init ();
		data = d as TaskConf;
		name.text = data.taskDesc;


		if (data.TaskState == TaskStateEnum.TaskComp) {
			progressLabel.text = data.goalCnt + "/" + data.goalCnt;
			btnLabel.text = TextCenter.GetText ("Reward_Take");
			goBtn.GetComponent<UIButton>().isEnabled = true;

			goBtnBg.defaultColor = takeAwardColor;

//			goBtn.SetActive(true);
		}else if(data.TaskState == TaskStateEnum.TaskBonusComp){
			progressLabel.text = data.goalCnt + "/" + data.goalCnt;
			btnLabel.text = TextCenter.GetText ("Achieve_Complete");
			goBtn.GetComponent<UIButton>().isEnabled = false;

			goBtnBg.defaultColor = Color.white;

		}else if(data.TaskState == TaskStateEnum.NotComp){
			progressLabel.text = data.CurrGoalCount + "/" + data.goalCnt;
			btnLabel.text = TextCenter.GetText ("Btn_GoTo");
			goBtn.GetComponent<UIButton>().isEnabled = true;

			goBtnBg.defaultColor = Color.white;

//			goBtn.SetActive(true);
		}
		int count = data.giftItem.Count;
		for (int i = 2; i >0; i--) {
			GameObject obj = transform.FindChild( "Icon" + i).gameObject;
			if(i <= count){
				obj.SetActive(true);
				GiftItem gi = data.giftItem[i-1];
				if(gi.content == (int)EGiftContent.UNIT) {
					ResourceManager.Instance.GetAvatarAtlas((uint)gi.value, obj.GetComponent<UISprite>());
				}else{
					obj.GetComponent<UISprite>().atlas = atlas;
					obj.GetComponent<UISprite>().spriteName = "icon_" + gi.content;
				}

				transform.FindChild( "Icon" + i + "/Label").GetComponent<UILabel>().text = "x " + data.giftItem[i-1].count;
			}else{
				obj.SetActive(false);
			}

			if(i == 1 && count > 0){

				GiftItem gift = data.giftItem[0];
				bigIcon.spriteName = "icon_" + gift.content;
				if (gift.content == (int)EGiftContent.UNIT) {
//					UIEventListenerCustom.Get (item.gameObject).LongPress = ClickUnit;
//					item.GetComponent<BoxCollider>().enabled = true;// obj.GetComponent<BoxCollider>()
					
					ResourceManager.Instance.GetAvatarAtlas((uint)gift.value, bigIcon);
					int type = (int)DataCenter.Instance.UnitData.GetUnitInfo((uint)gift.value).type;
					item.FindChild("Bg").GetComponent<UISprite>().spriteName = GetAvatarBgSpriteName(type);
					item.FindChild("Border").GetComponent<UISprite>().spriteName = GetBorderSpriteName(type);
					
					
				} else {
//					item.GetComponent<BoxCollider>().enabled = false;// obj.GetComponent<BoxCollider>()
					
					bigIcon.atlas = atlas;
					bigIcon.spriteName = "icon_" + gift.content;
					item.FindChild("Bg").GetComponent<UISprite>().spriteName = GetAvatarBgSpriteName(2);
					item.FindChild("Border").GetComponent<UISprite>().spriteName = GetBorderSpriteName(2);
					

				}
			}
		}


	}
	string GetBorderSpriteName (int unitType) {
		switch (unitType) {
		case 1:
			return "avatar_border_fire";
		case 2:
			return "avatar_border_water";
		case 3:
			return "avatar_border_wind";
		case 4:
			return "avatar_border_light";
		case 5:
			return "avatar_border_dark";
		case 6:
			return "avatar_border_none";
		default:
			return "avatar_border_none";
			break;
		}
	}
	
	string GetAvatarBgSpriteName(int unitType) {
		switch (unitType) {
		case 1:
			return "avatar_bg_fire";
		case 2:
			return "avatar_bg_water";
		case 3:
			return "avatar_bg_wind";
		case 4:
			return "avatar_bg_light";
		case 5:
			return "avatar_bg_dark";
		case 6:
			return "avatar_bg_none";
		default:
			return "avatar_bg_none";
			break;
		}
	}
	
	
	private void ClickUnit(GameObject obj){
		
		Debug.Log ("Click Item To Detail");
		uint i = 0; 
		uint.TryParse(obj.transform.FindChild("Img").GetComponent<UISprite>().spriteName,out i);
		DGTools.ChangeToUnitDetail (i);
		
		
	}

	public override void ItemCallback (params object[] args)
	{

	}

	private void OnGo(GameObject obj){
//		BonusController.Instance.TakeTaskBonus(null,);
		if (data.TaskState == TaskStateEnum.TaskComp) {
//			ModuleManager.SendMessage(ModuleEnum.ta
			DataCenter.Instance.TaskAndAchieveData.TakeAwardTemp(data);
			if ( data.taskType == ETaskType.ACHIEVEMENT ) {
				goBtn.GetComponent<UIButton>().isEnabled = false;
				btnLabel.text = TextCenter.GetText ("Achieve_Complete");
			}
		}else if(data.TaskState == TaskStateEnum.NotComp){
			if(data.taskType == ETaskType.ACHIEVEMENT){
				ModuleManager.Instance.HideModule(ModuleEnum.AchieveModule);
			}else{
				ModuleManager.Instance.HideModule(ModuleEnum.TaskModule);
			}

			switch (data.goToSence) {
			case "Mountain":
				ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"story",(uint)1);
				break;
			case "Wilderness":
				ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"story",(uint)2);
				break;
			case "Island":
				ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"story",(uint)3);
				break;
			case "Forest":
				ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"story",(uint)4);
				break;
			case "Castlevania":
				ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"story",(uint)5);
				break;
			case "Friend":
				ModuleManager.Instance.ShowModule(ModuleEnum.FriendMainModule);
				break;
			case "Summon":
				ModuleManager.Instance.ShowModule(ModuleEnum.ScratchModule);
				break;
			case "PowerUp":
				ModuleManager.Instance.ShowModule(ModuleEnum.UnitsListModule);
				break;
			case "Evolve":
				ModuleManager.Instance.ShowModule(ModuleEnum.UnitsListModule);
				break;
			case "NewestCity":
				ModuleManager.Instance.ShowModule(ModuleEnum.HomeModule);
				break;
			case "Event":
				ModuleManager.Instance.ShowModule(ModuleEnum.HomeModule);
				break;
			default:
			break;
			}
		}
	}

	public void DestroyUI(){

	}
}
