using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class QuestRewardItemView : DragPanelItemBase {	
	private GameObject btn;
	private UILabel starNum;
	private bool inited = false;
	

	private StageInfo data;
	public StageInfo Data{
		get{return data;}
	}

	public void ClickTakeAward(){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );

		QuestController.Instance.AcceptStarBonus(OnRspAcceptStarBonus, Data.id, Data.CopyType);
//		MsgCenter.Instance.Invoke(CommandEnum.TakeAward,data);
	}

	void OnRspAcceptStarBonus(object data) {
		RspAcceptStarBonus rsp = data as RspAcceptStarBonus;
		if( rsp != null && rsp.header.code == ErrorCode.SUCCESS ) {
			DataCenter.Instance.UserData.AccountInfo.stone = rsp.currentStone;
			DataCenter.Instance.UserData.AccountInfo.stoneFree += rsp.addStone;

			CopyPassInfo passInfo = DataCenter.Instance.GetCopyPassInfo( Data.CopyType );
			passInfo.SetAcceptBonus(Data.id);
			MsgCenter.Instance.Invoke(CommandEnum.SyncChips);
			MsgCenter.Instance.Invoke(CommandEnum.RefreshPlayerCoin);

			//已领奖
			btn.GetComponent<UIButton>().isEnabled = false;
			transform.FindChild("CollectBtn/Label").GetComponent<UILabel>().text = TextCenter.GetText("Reward_HasTaken");
		}
		Debug.LogWarning("rspAcceptStarBonus result: "+rsp.header.code);

		//TODO: show floating message window for result
	}

	public override void ItemCallback (params object[] args)
	{
		//		throw new System.NotImplementedException ();
	}

	public override void SetData<T>(T data, params object[] args) {
		//
	}

	public void SetData<T>(T data){
		this.data = data as StageInfo;
//		

		CopyPassInfo passInfo = DataCenter.Instance.GetCopyPassInfo( Data.CopyType );
		int totalStars = 0, maxStars=0;
		foreach( QuestInfo quest in this.data.quests) {
			totalStars += passInfo.GetQuestStar(this.data.id, quest.id);
			maxStars += 3;
		}

		if(!inited){
			btn = transform.FindChild("CollectBtn").gameObject;
			starNum = transform.FindChild("StarNum").GetComponent<UILabel>();
			btn.GetComponent<UIDragScrollView>().scrollView = FindObjectOfType<UIScrollView>();

			transform.FindChild("Tips").GetComponent<UILabel>().text = TextCenter.GetText("QuestRewardItemTips", maxStars);
			transform.FindChild("Bonus").GetComponent<UILabel>().text = TextCenter.GetText("QuestRewardItemBonus");

			inited = true;
		}

		starNum.text = totalStars.ToString()+"/"+maxStars.ToString();

		bool bBonusIsAccept = passInfo.IsBonusAccept(Data.id);

		btn.GetComponent<UIButton>().isEnabled = false;
		if( totalStars >= maxStars) { 
			btn.GetComponent<UIButton>().isEnabled = !bBonusIsAccept;
		}

		if( bBonusIsAccept ) { //已领奖
			transform.FindChild("CollectBtn/Label").GetComponent<UILabel>().text = TextCenter.GetText("Reward_HasTaken");
		}else {
			transform.FindChild("CollectBtn/Label").GetComponent<UILabel>().text = TextCenter.GetText("Reward_Take");
		}

	}
}
