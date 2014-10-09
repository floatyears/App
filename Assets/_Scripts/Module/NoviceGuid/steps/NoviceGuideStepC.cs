using UnityEngine;
using System.Collections;

public class NoviceGuideStepC_1:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepC_2);

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide5_title"),TextCenter.GetText ("guide5_content"),TextCenter.GetText ("OK"),ClickOk);

	}

	private void ClickOk(object data){
		
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);

		TipsManager.Instance.ShowGuideMsgWindow(TextCenter.GetText ("guide6_title"),TextCenter.GetText ("guide6_content"),TextCenter.GetText ("OK"),forceClick);
	}

	private void forceClick(object data)
	{
		
		GameObject btn = GameObject.FindWithTag("rare_scratch");
		NoviceGuideUtil.ForceOneBtnClick (btn);
		
		UIEventListenerCustom.Get (btn).onClick += TapRareCard;
		NoviceGuideUtil.ShowArrow (new GameObject[]{btn}, new Vector3[]{new Vector3(0,0,1)});
	}
	
	private void TapRareCard(GameObject btn)
	{
		UIEventListenerCustom.Get (btn).onClick -= TapRareCard;
		NoviceGuideUtil.RemoveArrow (btn);

		GoToNextState();
	}

}

public class NoviceGuideStepC_2:NoviceGuidStep
{
	private static NoviceGuideStepC_2 instance;
	
	public static NoviceGuideStepC_2 Instance()
	{
		if (instance == null)
			instance = new NoviceGuideStepC_2 ();
		return instance;
	}
	
	private NoviceGuideStepC_2 ():base()	{}
	
	public override void Enter()
	{
		nextState = typeof(NoviceGuideStepC_3);

		MsgWindowView mwv = GameObject.Find ("CommonNoteWindow(Clone)").GetComponent<MsgWindowView> ();
		
		UIButton cwLBtn = mwv.BtnLeft;
		NoviceGuideUtil.ForceOneBtnClick (cwLBtn.gameObject);
		UIEventListenerCustom.Get (cwLBtn.gameObject).onClick += clickLeftBtn;
		
		NoviceGuideUtil.ShowArrow (new GameObject[]{cwLBtn.gameObject},new Vector3[]{new Vector3(0,0,3)});
		
//		mwv.BtnRight.isEnabled = false;
	}
	
	private void clickLeftBtn(GameObject btn)
	{
		MsgWindowView mwv = GameObject.Find ("CommonNoteWindow(Clone)").GetComponent<MsgWindowView> ();
		
		UIButton cwLBtn = mwv.BtnLeft;
		UIEventListenerCustom.Get (cwLBtn.gameObject).onClick -= clickLeftBtn;
		
		NoviceGuideUtil.RemoveArrow (btn);
		
		mwv.BtnRight.isEnabled = true;
	}

}

public class NoviceGuideStepC_3:NoviceGuidStep
{
	
	public override void Enter()
	{
		nextState = null;
		TipsManager.Instance.ShowGuideMsgWindow (TextCenter.GetText("guide8_title"),TextCenter.GetText("guide8_content"),TextCenter.GetText("NEXT"));

		NoviceGuideStepManager.CurrentNoviceGuideStage = NoviceGuideStage.FRIEND_SELECT;
	}
	
}