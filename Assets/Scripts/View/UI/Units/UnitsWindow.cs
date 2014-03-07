using UnityEngine;
using System.Collections.Generic;

public class UnitsWindow : UIComponentUnity{
	IUICallback iuiCallback;


	private Dictionary<GameObject,SceneEnum> buttonInfo = new Dictionary<GameObject, SceneEnum>();
	
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);

		InitChildScenes();
	
		iuiCallback = origin as IUICallback;
	}
	
	public override void ShowUI(){
//		Debug.LogError("UnitsWindow.ShowUI(), Start...");
		base.ShowUI();
		ShowTween();
//		Debug.LogError("UnitsWindow.ShowUI(), End...");
	}
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}

	private void InitChildScenes(){
		GameObject go;

		go = FindChild("Bottom/Catalog");
		buttonInfo.Add(go, SceneEnum.UnitCatalog);
		
		go = FindChild("Bottom/Evolve");
		buttonInfo.Add(go, SceneEnum.Evolve);
		
		go = FindChild("Bottom/LevelUp");
		buttonInfo.Add(go, SceneEnum.LevelUp);
		
		go = FindChild("Bottom/Party");
		buttonInfo.Add(go, SceneEnum.Party);
		
		go = FindChild("Bottom/Sell");
		buttonInfo.Add(go, SceneEnum.Sell);
		
		go = FindChild("Bottom/UnitList");
		buttonInfo.Add(go, SceneEnum.UnitList);
		
		foreach (var item in buttonInfo.Keys)
			UIEventListener.Get(item).onClick = OnClickCallback;
	}

	void OnClickCallback(GameObject caller){
		if (iuiCallback == null)
			return;
		SceneEnum se = buttonInfo [caller];
		iuiCallback.Callback(se);
	}
	
	void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)	return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)	continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

}
