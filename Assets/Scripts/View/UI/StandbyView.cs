using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandbyView : UIComponentUnity {
	private UIButton prePageBtn;
	private UIButton nextPageBtn;
	private UIImageButton startFightBtn;
	private UILabel totalHPLabel;
	private UILabel totalAtkLabel;

	private Dictionary<int, PageUnitItem> partyView = new Dictionary<int, PageUnitItem>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
	}

	private void InitUI(){
		prePageBtn = FindChild<UIButton>("Button_Left");
		nextPageBtn = FindChild<UIButton>("Button_Right");
		startFightBtn = transform.FindChild("ImgBtn_Fight").GetComponent<UIImageButton>();
		totalHPLabel = transform.FindChild("Label_Total_Hp").GetComponent<UILabel>();
		totalAtkLabel = transform.FindChild("Label_Total_Atk").GetComponent<UILabel>();

		UIEventListener.Get(startFightBtn.gameObject).onClick = ClickFightBtn;
		UIEventListener.Get(prePageBtn.gameObject).onClick = PrevPage;
		UIEventListener.Get(nextPageBtn.gameObject).onClick = NextPage;

		for (int i = 0; i < 4; i++){
			PageUnitItem puv = FindChild<PageUnitItem>(i.ToString());
			partyView.Add(i, puv);
		}
	}

	private void ClickFightBtn(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	private void PrevPage(GameObject go){
		Debug.Log("PrevPage");
		TUnitParty preParty = DataCenter.Instance.PartyInfo.PrevParty;
		RefreshParty(preParty);  
		//MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, preParty);         
	}
	
	private void NextPage(GameObject go){
		Debug.Log("NextPage");
		TUnitParty nextParty = DataCenter.Instance.PartyInfo.NextParty;
		RefreshParty(nextParty);
		//MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, nextParty);
	}

	private void RefreshParty(TUnitParty party){
		List<TUserUnit> partyMemberList = party.GetUserUnit();
		//int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		for (int i = 0; i < partyMemberList.Count; i++){
			partyView[ i ].Init(partyMemberList [ i ]);
		}
	}

	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));       
	}
}
