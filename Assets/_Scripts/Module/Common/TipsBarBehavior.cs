using UnityEngine;
using System.Collections;

public class TipsBarBehavior : ViewBase {

	private UILabel labelTips;

	private int times;

	public override void Init ( UIInsConfig config ) {
		base.Init (config);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
        AddListener();

		ShowTips ();
		times = 2;
	}
	
	public override void HideUI () {
		base.HideUI ();
        RemoveListener();
	}


	public override void DestoryUI () {
		base.DestoryUI ();
		RemoveListener();
	}

    private void AddListener(){
        MsgCenter.Instance.AddListener(CommandEnum.EnableMenuBtns, EnableDisplay);
    }
    
    private void RemoveListener(){
        MsgCenter.Instance.RemoveListener(CommandEnum.EnableMenuBtns, EnableDisplay);
    }

	private void InitUI() 
	{
		labelTips = FindChild< UILabel >("Scroll/Label_Tips");
	}

    private void EnableDisplay(object args){
        this.gameObject.SetActive((bool)args);
    }
	
	public void ShowTips(){
		times--;
		if (times <= 0) {
			if (DataCenter.Instance.LoginInfo != null && DataCenter.Instance.LoginInfo.Data != null) {
				if (DataCenter.Instance.LoginInfo.Data.Rank < 5) {
					labelTips.text = TextCenter.GetText ("Tips_A_" + MathHelper.RandomToInt (1, 13));
				} else if (DataCenter.Instance.LoginInfo.Data.Rank < 10) {
					labelTips.text = TextCenter.GetText ("Tips_B_" + MathHelper.RandomToInt (1, 10));
				} else if (DataCenter.Instance.LoginInfo.Data.Rank < 20) {
					labelTips.text = TextCenter.GetText ("Tips_C_" + MathHelper.RandomToInt (1, 18));
				} else if (DataCenter.Instance.LoginInfo.Data.Rank < 30) {
					labelTips.text = TextCenter.GetText ("Tips_D_" + MathHelper.RandomToInt (1, 18));
				} else {
					labelTips.text = TextCenter.GetText ("Tips_E_" + MathHelper.RandomToInt (1, 24));
				}	
			} else {
				labelTips.text = TextCenter.GetText ("Tips_A_" + MathHelper.RandomToInt (1, 13));
			}
			times = 2;
		}

		labelTips.GetComponent<TweenPosition> ().enabled = true;
		labelTips.GetComponent<TweenPosition> ().ResetToBeginning ();
	}
}
