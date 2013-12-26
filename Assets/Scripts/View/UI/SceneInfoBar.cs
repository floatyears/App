using UnityEngine;
using System.Collections;

public class SceneInfoBar : UIBaseUnity
{
	public UIImageButton backBtn;
	
	public static UILabel labVauleUIName;
	
	public event UICallback callback;

	

	void Start()
	{
		Init("SceneInoBar");
	}
	public override void Init (string name)
	{
		base.Init (name);

		labVauleUIName = FindChild<UILabel>("Lab_UI_Name");
		backBtn = FindChild<UIImageButton>("ImgBtn_Arrow");

		UIEventListener.Get(backBtn.gameObject).onClick = Back;
	}

	void Back(GameObject caller)
	{
		if(callback != null)
			callback(caller);
	}

}
