using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatalogView : UIComponentUnity {
	private int startPageIndex = 0;
	private int endPageIndex;
	private int totalPageCount;


//	private List<>

	private DragPanel dragPanel;
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	private int GetTotalPageNum(){
		//TODO GET FROM DATACENTER
		int num = 0;
		num = 3;
		return num;
	}

}
