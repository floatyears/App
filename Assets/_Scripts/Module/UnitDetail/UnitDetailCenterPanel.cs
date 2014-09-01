//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using bbproto;
//
//public class UnitDetailCenterPanel : UIComponentUnity,IUICallback  {
//
//
//	public override void Init ( UIInsConfig config ) {
//		base.Init (config);
//		InitUI();
//		InitEffect ();
//	}
//	
//	public override void ShowUI () {
//		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
//		MsgCenter.Instance.AddListener (CommandEnum.LevelUp, LevelUpFunc);
//		base.ShowUI ();
//		//UIManager.Instance.HideBaseScene();
//
//		//TODO:
//		//StartCoroutine ("nextState");
//		//NoviceGuideStepEntityManager.Instance ().StartStep ();
//	}
//	
//	public override void HideUI () {
//		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
//		MsgCenter.Instance.RemoveListener (CommandEnum.LevelUp, LevelUpFunc);
//		base.HideUI ();
//		ClearEffectCache();
//	}
//	
//	public override void DestoryUI () {
//		base.DestoryUI ();
//	}
//
//	//----------Init functions of UI Elements----------
//	void InitUI() {
//
//	}
//
//
//
//
//
//	void ShowInfo(TUserUnit userUnit) {
//		ShowBodyTexture( userUnit ); 
//		ShowUnitScale();
//	}
//
////	private TUserUnit curUserUnit;
//	public void CallBackUnitData(object data)	{
//		TUserUnit userUnit = data as TUserUnit;
//		if (userUnit != null) {
//			ShowInfo (userUnit);
//		}
//	}
//
//
//	
//	TUserUnit oldBlendUnit = null;
//	TUserUnit newBlendUnit = null;
//	Vector3 targetPosition;
//	RspLevelUp levelUpData;
//
//	Queue<GameObject> material = new Queue<GameObject> ();
//	int count = 0;
//
//
//	public void SetEffectCamera() {
//		Camera camera = Main.Instance.effectCamera;
//		camera.transform.eulerAngles = new Vector3 (15f, 0f, 0f);
//		camera.orthographicSize = 1.3f;
//	}
//
//	public void RecoverEffectCamera() {
//		Camera camera = Main.Instance.effectCamera;
//		camera.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
//		camera.orthographicSize = 1f;
//	}
//
//	List<GameObject> effectCache = new List<GameObject>();
//	GameObject levelUpEffect;
//	GameObject swallowEffect;
//	GameObject linhunqiuEffect;
//
//	void InitEffect(){
//		string path = "Effect/effect/LevelUpEffect";
//		ResourceManager.Instance.LoadLocalAsset( path , o =>{
//			levelUpEffect = o as GameObject;
//		});
//
//		path = "Effect/effect/level_up01";
//		ResourceManager.Instance.LoadLocalAsset( path , o =>{
//			swallowEffect = o as GameObject;
//		});
//
//		path = "Effect/effect/linhunqiu1";
//		ResourceManager.Instance.LoadLocalAsset( path , o =>{
//			linhunqiuEffect = o as GameObject;
//		});
//	}
//
//	GameObject materilUse = null;
//	GameObject linhunqiuIns = null;
//	GameObject swallowEffectIns = null;
//
//	IEnumerator SwallowUserUnit () {
//		yield return new WaitForSeconds(1f);
//
//		while (material.Count > 0) {
//			materilUse = material.Dequeue();
//			iTween.ScaleTo(materilUse, iTween.Hash("y", 0f, "time", 0.2f));
//			yield return new WaitForSeconds(0.2f);
//			Destroy(materilUse);
//			GameObject linhunqiuIns = NGUITools.AddChild(parent, linhunqiuEffect);
//			linhunqiuIns.transform.localPosition = materilUse.transform.localPosition;
//			linhunqiuIns.transform.localScale = Vector3.zero;
//			iTween.ScaleTo(linhunqiuIns, iTween.Hash("y", 1f, "time", 0.2f));
//			yield return new WaitForSeconds(0.2f);
//			iTween.MoveTo(linhunqiuIns, iTween.Hash("position", targetPosition, "time", 0.3f, "islocal", true));
//			yield return new WaitForSeconds(0.3f);
//			Destroy(linhunqiuIns);
//			swallowEffectIns = NGUITools.AddChild(gameObject, swallowEffect);
//			yield return new WaitForSeconds(0.4f);
//			Destroy(swallowEffectIns);
//		}
//
//		StartCoroutine (CreatEffect ());
//	}
//
//	IEnumerator CreatEffect() {
//		yield return new WaitForSeconds(0.5f);
//		GameObject go = Instantiate (levelUpEffect) as GameObject;
//		effectCache.Add (go);
//		if (effectCache.Count == count) {
//			yield return new WaitForSeconds(2f);
//			ClearEffectCache ();
//			topPanel.ShowPanel();
//			RecoverEffectCamera();
//			MsgCenter.Instance.Invoke(CommandEnum.ShowLevelupInfo);
//		} else {
//			yield return new WaitForSeconds(1.5f);
//			StartCoroutine (CreatEffect ());
//		}
//	}
//
//	void ClearEffectCache(){
//		StopAllCoroutines ();
//
//		DGTools.SafeDestory (materilUse);
//		DGTools.SafeDestory (linhunqiuIns);
//		DGTools.SafeDestory (swallowEffectIns);
//
//		for (int i = effectCache.Count - 1; i >= 0 ; i--) {
//			GameObject go = effectCache[i];
//			Destroy( go );
//			effectCache.Remove(go);
//		}
//	}
//
//}
