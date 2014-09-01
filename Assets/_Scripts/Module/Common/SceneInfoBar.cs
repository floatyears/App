using UnityEngine;
using System.Collections;

public class SceneInfoBar : ViewBase
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
	
	void Start(){
//		Init("SceneInoBar");
	}

	public override void Init (UIConfigItem config)
	{
		base.Init (config);
		uiTitleLab = FindChild<UILabel>("Lab_UI_Name");
		backBtn = FindChild<UIImageButton>("ImgBtn_Arrow");
	}
}
