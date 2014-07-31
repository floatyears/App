using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoviceGuideUtil {

	private static LayerMask camLastLayer;

	private static int oneBtnClickLayer;

	private static Dictionary<string ,GameObject> arrows = new Dictionary<string,GameObject>();

	private static GameObject tipText;

	private static UIEventListenerCustom.VoidDelegate clickDelegate;

	private static UIEventListenerCustom.LongPressDelegate longPressDelegate;

	private static UIEventListenerCustom.LongPressDelegate pressDelegate;

	private static GameObject[] multiBtns;

	// posAndDir:the x,y stand for the position, the z stands for direction
	public static void ShowArrow(GameObject[] parents,Vector3[] posAndDir, bool showTap = true){

		Vector3 dir;
		int i = 0,len = posAndDir.Length;
		LoadAsset.Instance.LoadAssetFromResources ("NoviceGuideArrow", ResourceEuum.Prefab, o => {
			GameObject obj = o as GameObject;
			foreach (GameObject parent in parents) {
				//			GameObject arrow = GameObject.Instantiate(obj,new Vector3(pos.x,pos.y,0),dir) as GameObject;
				GameObject arrow = NGUITools.AddChild (parent, obj);
				GameObject tap = arrow.transform.FindChild("Sprite/Sprite").gameObject;
				tap.layer = arrow.transform.FindChild("Sprite").gameObject.layer = arrow.layer;
				if(showTap){
					tap.GetComponent<UISprite>().enabled = true;
				}else{
					tap.GetComponent<UISprite>().enabled = false;
				}
				TweenPosition tPos = arrow.transform.FindChild("Sprite").GetComponent<TweenPosition> ();

				Vector3 size = Vector3.zero;
				try {
					size = parent.GetComponent<BoxCollider> ().size;
				} catch (MissingComponentException e) {
					LogHelper.LogWarning (e.ToString ());
					size = parent.transform.localPosition;
				}

				switch (i < len ? (int)posAndDir [i].z : 0) {
				//point to the top
				case 3:
					dir = new Vector3 (0.0f, 0.0f, 180.0f);// = Quaternion.FromToRotation(new Vector3(1,0,0),Vector3.zero);
					tPos.to.y = -size.y / 2 - 12 + posAndDir [i].y;
					tPos.from.y = -size.y / 2 - 32.0f + posAndDir [i].y;
					tPos.to.x = posAndDir [i].x;
					tPos.from.x = posAndDir [i].x;
					break;
				//point to the right
				case 4:
					dir = new Vector3 (0f, 0f, 90f);
//					dir = Quaternion.FromToRotation(new Vector3(-1,0,0),Vector3.zero);
					tPos.to.x = -size.x / 2 - 12 + posAndDir [i].x;
					tPos.from.x = -size.x / 2 - 32.0f + posAndDir [i].x;
					tPos.to.y = posAndDir [i].y;
					tPos.from.y = posAndDir [i].y;
					break;
				//point to the bottom
				case 1:
//					dir = Quaternion.FromToRotation(new Vector3(0,1,0),Vector3.zero);
					dir = new Vector3 (0f, 0f, 0f);
					tPos.to.y = size.y / 2 + 12 + posAndDir [i].y;
					tPos.from.y = size.y / 2 + 32.0f + posAndDir [i].y;
					tPos.to.x = posAndDir [i].x;
					tPos.from.x = posAndDir [i].x;
					break;
				case 2:
	//point to the left
//					dir = Quaternion.FromToRotation(new Vector3(0,-1,0),Vector3.zero);
					dir = new Vector3 (0f, 0f, 270f);
					tPos.to.x = size.x / 2 + 12 + posAndDir [i].x;
					tPos.from.x = size.x / 2 + 32.0f + posAndDir [i].x;
					tPos.to.y = posAndDir [i].y;
					tPos.from.y = posAndDir [i].y;
					break;
				default:
					dir = Vector3.zero;
					break;
				}

				arrow.transform.FindChild("Sprite").transform.Rotate (dir);
				tap.transform.Rotate (-dir);
				NGUITools.AdjustDepth (arrow, 20);
				//			if(obj.transform.parent != null)
				//			{
				//LogHelper.Log("-------///-......parent is not null: " + obj.transform.parent);
				//			}
				LogHelper.Log ("=====add arrow dic key: " + parent.GetInstanceID () + parent.name);

				arrows.Add (parent.GetInstanceID () + parent.name, arrow);
				i++;
			}
		});
	}

	public static void RemoveAllArrows(){
		LogHelper.Log ("arrow count: " + arrows.Count);

		foreach (string key in arrows.Keys) {
			GameObject.Destroy(arrows[key]);
			LogHelper.Log ("===/////===remove arrow: "+key);
		}
		arrows.Clear ();
	}

	public static void RemoveArrow(GameObject obj)
	{
		LogHelper.Log ("arrow count: " + arrows.Count);
		string key = obj.GetInstanceID () + obj.name;
		if (arrows.ContainsKey (key)) {
			GameObject obj1 = arrows[key];
			GameObject.Destroy(obj1);
			arrows.Remove(key);	

			LogHelper.Log ("===/////===remove arrow: "+ key);
		}
	}

	public static void showTipText(string text,Vector2 pos = default(Vector2)){
		Debug.Log ("--------------///////tip text: " + text);
		if (tipText == null) {
			LoadAsset.Instance.LoadAssetFromResources ("TipText", ResourceEuum.Prefab, o => {
				GameObject tip = o as GameObject;
				tipText = GameObject.Instantiate (tip) as GameObject;
				Transform trans = tipText.transform;
				trans.parent = ViewManager.Instance.CenterPanel.transform;
				trans.localPosition = Vector3.zero;
				//trans.position =Vector3.zero;
				trans.gameObject.layer = ViewManager.Instance.CenterPanel.layer;
				trans.localScale = Vector3.one;
				
				NGUITools.AdjustDepth (tipText, 100);
				
				tipText.SetActive (true);
				
				tipText.transform.localPosition = new Vector3 (pos.x, 0, 0);
				LogHelper.Log ("tip text position: " + tipText.transform.position);
				
				tipText.GetComponent<TipText> ().SetText (text);
			});

		} else {
			tipText.SetActive (true);
			
			tipText.transform.localPosition = new Vector3 (pos.x, 0, 0);
			LogHelper.Log ("tip text position: " + tipText.transform.position);
			
			tipText.GetComponent<TipText>().SetText(text);
		}



	}

	public static void HideTipText(){
		tipText.SetActive (false);
	}

	public static void ForceOneBtnClick(GameObject obj,bool isExecuteBefore = true)
	{
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		camLastLayer = mainCam.eventReceiverMask;

		//TODO:Change the execute order....this may be different in different platform
		if (isExecuteBefore) {
			clickDelegate = UIEventListenerCustom.Get (obj).onClick;
			UIEventListenerCustom.Get (obj).onClick = null;
			UIEventListenerCustom.Get (obj).onClick += BtnClick;
			UIEventListenerCustom.Get (obj).onClick += clickDelegate;
			clickDelegate = null;
		
		} else {
			UIEventListenerCustom.Get (obj).onClick += BtnClick;	
		}
 		
		longPressDelegate = UIEventListenerCustom.Get (obj).LongPress;
		UIEventListenerCustom.Get (obj).LongPress = null;

		oneBtnClickLayer = obj.layer;
		LayerMask mask =  1 << LayerMask.NameToLayer ("NoviceGuide");
//		mainCam.eventReceiverMask = mask;
		obj.layer = LayerMask.NameToLayer ("NoviceGuide");

		TouchEventBlocker.Instance.SetState (BlockerReason.NoviceGuide, true);

		LogHelper.Log ("main cam layer(force click): " + mainCam.eventReceiverMask.value);
	}



	private static void BtnClick(GameObject btn)
	{
		UIEventListener.Get (btn).onClick -= BtnClick;
		UIEventListenerCustom.Get (btn).LongPress = longPressDelegate;
		longPressDelegate = null;
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
//		mainCam.eventReceiverMask = camLastLayer;

		TouchEventBlocker.Instance.SetState (BlockerReason.NoviceGuide, false);

		btn.layer = oneBtnClickLayer;
		LogHelper.Log ("btn layer: " + oneBtnClickLayer + ", mainCam layer: " + mainCam.eventReceiverMask.value);

	}

	public static void ForceBtnsClick(GameObject[] objs,UIEventListenerCustom.VoidDelegate clickCalback){
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		camLastLayer = mainCam.eventReceiverMask;
//		LayerMask mask =  1 << LayerMask.NameToLayer ("NoviceGuide");
//		mainCam.eventReceiverMask = mask;
		TouchEventBlocker.Instance.SetState (BlockerReason.NoviceGuide, true);

		multiBtns = objs;
		
		
		clickDelegate = clickCalback;
		foreach (GameObject item in objs) {
			clickDelegate = UIEventListenerCustom.Get (item).onClick;
			UIEventListenerCustom.Get (item).onClick = null;
			UIEventListenerCustom.Get (item).onClick += MultiBtnClick;
			UIEventListenerCustom.Get (item).onClick += clickCalback;
			UIEventListenerCustom.Get (item).onClick += clickDelegate;
			clickDelegate = clickCalback;
			oneBtnClickLayer = item.layer;
			item.layer = LayerMask.NameToLayer ("NoviceGuide");
		}
		
		LogHelper.Log ("main cam layer(force click): " + mainCam.eventReceiverMask.value);
	}

	private	static void MultiBtnClick(GameObject btn){

		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
//		mainCam.eventReceiverMask = camLastLayer;

		TouchEventBlocker.Instance.SetState (BlockerReason.NoviceGuide, false);

		foreach (GameObject item in multiBtns) {
			UIEventListenerCustom.Get (item).onClick -= MultiBtnClick;
			UIEventListenerCustom.Get (item).onClick -= clickDelegate;
			item.layer = oneBtnClickLayer;
			LogHelper.Log("======multi btns layer: "+item.layer);
		}
		multiBtns = null;
		clickDelegate = null;

		LogHelper.Log ("btn layer: " + oneBtnClickLayer + ", mainCam layer: " + mainCam.eventReceiverMask.value);
	}

	public static void ForceOneBtnPress(GameObject obj)
	{
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
		camLastLayer = mainCam.eventReceiverMask;
		
		//TODO:Change the execute order....this may be different in different platform
		pressDelegate = UIEventListenerCustom.Get (obj).LongPress;
		UIEventListenerCustom.Get (obj).LongPress = null;
		UIEventListenerCustom.Get (obj).LongPress += BtnPress;
		UIEventListenerCustom.Get (obj).LongPress += pressDelegate;
		pressDelegate = null;
		
		oneBtnClickLayer = obj.layer;
		LayerMask mask =  1 << LayerMask.NameToLayer ("NoviceGuide");
//		mainCam.eventReceiverMask = mask;
		TouchEventBlocker.Instance.SetState (BlockerReason.NoviceGuide, true);

		obj.layer = LayerMask.NameToLayer ("NoviceGuide");
		LogHelper.Log ("main cam layer(force click): " + mainCam.eventReceiverMask.value);
	}
	
	private static void BtnPress(GameObject btn)
	{
		UIEventListenerCustom.Get (btn).LongPress -= BtnPress;
		UICamera mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<UICamera>();
//		mainCam.eventReceiverMask = camLastLayer;

		TouchEventBlocker.Instance.SetState (BlockerReason.NoviceGuide, false);

		btn.layer = oneBtnClickLayer;
		LogHelper.Log ("btn layer: " + oneBtnClickLayer + ", mainCam layer: " + mainCam.eventReceiverMask.value);
		
	}

	public static void showTipTextAnimation(){
		tipText.GetComponent<TipText> ().ShowAnimation ();
	}
	
}
