using UnityEngine;
using System.Collections;

public class SceneInfoBar : UIBaseUnity
{
	private UIImageButton backBtn;
	public UIImageButton BackBtn 
	{
		get{
			return backBtn;
		}
	}

	private UILabel uiTitleLab;
	public UILabel UITitleLab
	{
		get{
			return uiTitleLab;
		}
	}
	
	void Start()
	{
		Init("SceneInoBar");
	}

	public override void Init (string name)
	{
		base.Init (name);
		uiTitleLab = FindChild<UILabel>("Lab_UI_Name");
		backBtn = FindChild<UIImageButton>("ImgBtn_Arrow");
	}
}
