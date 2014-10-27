using UnityEngine;
using System.Collections;
using bbproto;

public class AchieveAndTaskItemView : DragPanelItemBase {

	private UISprite bigIcon;
	private UILabel name;
	private UILabel progressLabel;
	private GameObject Icon;

	private UILabel btnLabel;

	private TaskConf data;

	private GameObject goBtn;

	private void Init(){
		bigIcon = transform.FindChild ("Item/Img").GetComponent<UISprite> ();
		name = transform.FindChild ("Name").GetComponent<UILabel>();
		progressLabel = transform.FindChild ("Label_Progress").GetComponent<UILabel> ();

		transform.FindChild ("Label_Award").GetComponent<UILabel> ().text = TextCenter.GetText("Text_Award");
		btnLabel = transform.FindChild ("Go/Label").GetComponent<UILabel> ();

		transform.FindChild ("Label_Award").GetComponent<UILabel> ().text = TextCenter.GetText ("Label_Award");

		goBtn = transform.FindChild ("Go").gameObject;
		UIEventListenerCustom.Get (goBtn).onClick = OnGo;
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
			goBtn.SetActive(true);
		}else if(data.TaskState == TaskStateEnum.TaskBonusComp){
			progressLabel.text = data.goalCnt + "/" + data.goalCnt;
//			btnLabel.text = TextCenter.GetText ("Reward_Take");
			goBtn.SetActive(false);
		}else if(data.TaskState == TaskStateEnum.NotComp){
			progressLabel.text = data.CurrGoalCount + "/" + data.goalCnt;
			btnLabel.text = TextCenter.GetText ("Btn_GoTo");
			goBtn.SetActive(true);
		}
		int count = data.giftItem.Count;
		for (int i = 2; i >0; i--) {
			GameObject obj = transform.FindChild( "Icon" + i).gameObject;
			if(i <= count){
				obj.SetActive(true);
				obj.GetComponent<UISprite>().spriteName = "icon_" + data.giftItem[i-1].content;
				transform.FindChild( "Icon" + i + "/Label").GetComponent<UILabel>().text = "x " + data.giftItem[i-1].count;
			}else{
				obj.SetActive(false);
			}

			if(i == 1 && count > 0){
				bigIcon.spriteName = "icon_" + data.giftItem[0].content;
			}
		}
	}

	public override void ItemCallback (params object[] args)
	{

	}

	private void OnGo(GameObject obj){
//		BonusController.Instance.TakeTaskBonus(null,);
		if (data.TaskState == TaskStateEnum.TaskComp) {
//			ModuleManager.SendMessage(ModuleEnum.ta
			DataCenter.Instance.TaskAndAchieveData.TakeAwardTemp(data);
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
