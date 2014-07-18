using UnityEngine;
using System.Collections;

public class PrefaceView : UIComponentUnity {

	private UILabel text;

	private UILabel speak;

	private int i;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();

		InvokeRepeating ("ShowContent",0,2);
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
	}

	void ShowContent()
	{
		if(i > 6){
			UIManager.Instance.ChangeScene(SceneEnum.SelectRole);
			return;
		}
		text.text = TextCenter.GetText ("Preface_Content" + i);
		Debug.Log("content: " + TextCenter.GetText ("Preface_Content" + i) + "index: " + i);
		i++;
	}

}
