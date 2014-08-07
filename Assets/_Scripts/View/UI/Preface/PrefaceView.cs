using UnityEngine;
using System.Collections;

public class PrefaceView : UIComponentUnity {

	private UILabel text;

	private UILabel speak;

	private int i;

	private TweenAlpha ta;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();

	}
	
	public override void ShowUI(){
		base.ShowUI();

		ShowContent ();
		Umeng.GA.Event("PrefaceStart");
//		InvokeRepeating ("ShowContent",0,3);
		//NoviceGuideStepEntityManager.Instance ().StartStep ();
	}
	
	public override void HideUI(){
		base.HideUI();
	}

	void InitUI()
	{
		i = 1;
		text = FindChild ("Text").GetComponent<UILabel> ();
		speak = FindChild ("Speak").GetComponent<UILabel> ();
		speak.enabled = false;

		ta = text.GetComponent<TweenAlpha> ();
	}

	public void ShowContent()
	{
		if(i > 5){
			Umeng.GA.Event("PrefaceComplete");
			UIManager.Instance.ChangeScene(SceneEnum.SelectRole);

			return;
		}
		ta.ResetToBeginning ();
		ta.enabled = true;
//		text.GetComponent<TweenScale> ().ResetToBeginning ();
		text.text = TextCenter.GetText ("Preface_Content" + i);
//		Debug.Log("content: " + TextCenter.GetText ("Preface_Content" + i) + "index: " + i);
		i++;
	}

}
