using UnityEngine;
using System.Collections.Generic;

public class EvolveDecoratorUnity : UIComponentUnity {
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	//==========================================interface end ==========================

	private const string preAtkLabel = "PrevAtkLabel";
	private const string preHPLabel = "PrevHPLabel";
	private const string evolveAtkLabel = "NextAtkLabel";
	private const string evolveHPLabel = "NextHPLabel";
	private const string needLabel = "NeedLabel";
	private string rootPath = "Window";

	private Dictionary<string,UILabel> showInfoLabel = new Dictionary<string, UILabel>();

	/// <summary>
	/// 1: base. 2, 3, 4: material. 5: friend
	/// </summary>
	private Dictionary<int,EvolveItem> evolveItem = new Dictionary<int, EvolveItem>();
	
	void InitUI () {
		InitItem ();
		InitLabel ();
	}

	void InitItem () {
		string path = rootPath + "/title/";
		for (int i = 1; i < 6; i++) {
			GameObject go = FindChild(path + i);
			EvolveItem ei = new EvolveItem();
			ei.itemObject = go;
			ei.showTexture = go.transform.Find("Texture").GetComponent<UITexture>();
			evolveItem.Add(i,ei);
			if(i == 1 || i == 5) {
				continue;
			}
			ei.haveLabel = go.transform.Find("HaveLabel").GetComponent<UILabel>();
		}
	}

	void InitLabel () {
		string path = rootPath + "/info_panel/";

		UILabel temp = transform.Find(path + preAtkLabel).GetComponent<UILabel>();
		showInfoLabel.Add (preAtkLabel, temp);

		temp = transform.Find(path + preHPLabel).GetComponent<UILabel>();
		showInfoLabel.Add (preHPLabel, temp);

		temp = transform.Find(path + evolveAtkLabel).GetComponent<UILabel>();
		showInfoLabel.Add (evolveAtkLabel, temp);

		temp = transform.Find(path + evolveHPLabel).GetComponent<UILabel>();
		showInfoLabel.Add (evolveHPLabel, temp);

		temp = transform.Find(path + needLabel).GetComponent<UILabel>();
		showInfoLabel.Add (needLabel, temp);
	}
}

public class EvolveItem {
	public GameObject itemObject;
	public UITexture showTexture;
	public UILabel haveLabel;
}