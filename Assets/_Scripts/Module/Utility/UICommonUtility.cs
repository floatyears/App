using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BlockerReason{
	MessageWindow,
	BriefInfoWindow,
	SortWindow,
	Connecting,
	NoviceGuide
}

public struct CameraLayerObj{
	public BlockerReason reason;
	public LayerMask originLayer;
	public bool isBlocked;
}

public class TouchEventBlocker{
	public const int blockerLayer = 15;
	public const string blockerLayerName = "Blocker";
    public const int defaultLayer = 0;
    public const string defaultLayerName = "Default";
	public const int guideLayer = 16;
	public const string guideLayerName = "NoviceGuide";

	private int blockEvent = 0;

	private TouchEventBlocker(){
		nguiCamera = Camera.main.GetComponent<UICamera>();
		blockEvent = GameLayer.LayerToInt (GameLayer.blocker) | GameLayer.LayerToInt (GameLayer.BottomInfo);
	}
	private int originLayer = 1;

//    private bool isBlocked;
	
	private UICamera nguiCamera ;

	private Stack<CameraLayerObj> cameraLayerChanges = new Stack<CameraLayerObj>();

	private List<CameraLayerObj> remainChanges = new List<CameraLayerObj> ();
//	private Dictionary<BlockerReason, bool> stateDic = new Dictionary<BlockerReason, bool>();

	private static TouchEventBlocker instance;

	public static TouchEventBlocker Instance{
		get{ 
			if(instance == null){
				instance = new TouchEventBlocker();
			}
			return instance;
		}
	}

//    public bool IsBlocked {
//        get {return isBlocked;}
//    }

	public void SetState(BlockerReason reason, bool isBlocked){
		RecordState(reason, isBlocked);
	}

	private void RecordState(BlockerReason reason, bool isBlocked){
//		if(stateDic.ContainsKey(reason)){
//			stateDic[reason] = isBlocked;
//		}
//		else{
//			stateDic.Add(reason, isBlocked);
//		}

//		Debug.Log ("blocker reason: " +reason +" block: " + isBlocked);

		CameraLayerObj camLayerObj = new CameraLayerObj ();
		camLayerObj.reason = reason;
//		if (reason == BlockerReason.Connecting) {
//			camLayerObj.originLayer = nguiCamera.eventReceiverMask;
//		} else {
			camLayerObj.originLayer = nguiCamera.eventReceiverMask;
//		}

		camLayerObj.isBlocked = isBlocked;

		if (isBlocked) {
			Debug.Log("en queue: " + camLayerObj.reason);
			cameraLayerChanges.Push (camLayerObj);
			SetBlocked(camLayerObj);
		}else if(cameraLayerChanges.Count > 0){
			if(cameraLayerChanges.Peek ().reason == reason){
				Debug.Log("dequeue: " + cameraLayerChanges.Peek().reason);
				CameraLayerObj obj = cameraLayerChanges.Pop();
				obj.isBlocked = false;
				SetBlocked(obj);
				CheckRemains();
			}else
			{
				remainChanges.Add(camLayerObj);
			}
		}

	}

	private void CheckRemains(){
		if (cameraLayerChanges.Count <= 0)
			return;
	    foreach (var item in remainChanges) {
			if(item.reason == cameraLayerChanges.Peek().reason){
				CameraLayerObj obj = cameraLayerChanges.Pop();
				obj.isBlocked = false;
				SetBlocked(obj);
				CheckRemains();
				break;
			}
		} 
	}

//	private bool GetFinalState(BlockerReason reason, bool isBlocked){
//		RecordState(reason, isBlocked);
//		return CalculateFinalState(isBlocked);
//	}

//	private bool CalculateFinalState(bool isBlocked){
//		bool result = isBlocked;
//		
//		if(isBlocked){
//			result = true;
//		}
//		else{
//			foreach (var item in stateDic) {
//				if(item.Value){
//					result = true;
//					break;
//				}
//			}
//		}
//
//		return result;
//	}
	
	private void SetBlocked(CameraLayerObj camObj){
		//Debug.LogError("TouchEventBlocker.SetBlocked(), isBlocked " + isBlocked);
//		Debug.Log ("ui camera: " + nguiCamera.eventReceiverMask.value + " origin: " + camObj.originLayer.value + " isblocked: " + camObj.isBlocked.ToString() + " reason: " + camObj.reason);
		if (camObj.isBlocked){

			if(camObj.reason != BlockerReason.NoviceGuide){
				nguiCamera.eventReceiverMask = 1 << LayerMask.NameToLayer(blockerLayerName);
			}else{
				nguiCamera.eventReceiverMask = 1 << LayerMask.NameToLayer(guideLayerName);

//				BattleBottom.notClick = true;
				MsgCenter.Instance.Invoke(CommandEnum.ShiledInput,true);
//				BaseUnitItem.canShowUnitDetail = false;
			}
		}
		else{
//			foreach (var item in remainChanges) {
//				if(item.reason == camObj.reason)
//					return;
//			}
			nguiCamera.eventReceiverMask = camObj.originLayer;

			if(camObj.reason == BlockerReason.NoviceGuide){
				MsgCenter.Instance.Invoke(CommandEnum.ShiledInput,false);
//				BaseUnitItem.canShowUnitDetail = true;
			}
//			Debug.LogError("TouchEventBlocker.SetBlocked(), when false, eventReceiverMask " + (int)nguiCamera.eventReceiverMask);

		}

	}

//	private void Test(bool result){
//		//Test
//		Debug.LogError("CalculateFinalState.Test(), result is : " + result);
//		foreach (var item in stateDic){
//			Debug.LogError(string.Format("Test, Key is {0}, Value is {1}", item.Key, item.Value));
//		}
//	}

}