using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BlockerReason{
	MessageWindow,
	PopUpWindow,
	SortWindow,
	Connecting,
	NoviceGuide
}

public struct CameraLayerObj{
	public BlockerReason reason;
	public LayerMask originLayer;
	public bool isBlocked;
}

public class InputManager{
	public const int blockerLayer = 15;
	public const string blockerLayerName = "Blocker";
    public const int defaultLayer = 0;
    public const string defaultLayerName = "Default";
	public const int guideLayer = 16;
	public const string guideLayerName = "NoviceGuide";

	private int blockEvent = 0;

	private InputManager(){
		nguiCamera = Camera.main.GetComponent<UICamera>();
		blockEvent = GameLayer.LayerToInt (GameLayer.blocker) | GameLayer.LayerToInt (GameLayer.BottomInfo);
	}
	private int originLayer = 1;

//    private bool isBlocked;
	
	private UICamera nguiCamera ;

	private Stack<CameraLayerObj> cameraLayerChanges = new Stack<CameraLayerObj>();

	private List<CameraLayerObj> remainChanges = new List<CameraLayerObj> ();
//	private Dictionary<BlockerReason, bool> stateDic = new Dictionary<BlockerReason, bool>();

	private static InputManager instance;

	public static InputManager Instance{
		get{ 
			if(instance == null){
				instance = new InputManager();
			}
			return instance;
		}
	}


	public void SetBlockWithinLayer(BlockerReason reason, bool isBlocked){

		Debug.Log ("blocker reason: " +reason +" block: " + isBlocked);

		CameraLayerObj camLayerObj = new CameraLayerObj ();
		camLayerObj.reason = reason;
		camLayerObj.originLayer = nguiCamera.eventReceiverMask;

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
	
	private void SetBlocked(CameraLayerObj camObj){
		if (camObj.isBlocked){

			if(camObj.reason != BlockerReason.NoviceGuide){
				nguiCamera.eventReceiverMask = 1 << LayerMask.NameToLayer(blockerLayerName);
			}else{
				nguiCamera.eventReceiverMask = 1 << LayerMask.NameToLayer(guideLayerName);

				MsgCenter.Instance.Invoke(CommandEnum.ShiledInput,true);
			}
		}
		else{
			nguiCamera.eventReceiverMask = camObj.originLayer;

			if(camObj.reason == BlockerReason.NoviceGuide){
				MsgCenter.Instance.Invoke(CommandEnum.ShiledInput,false);
			}
		}
	}

	public void SetBlockWithinModule(ModuleEnum module, bool isFocus){
		UIEventListenerCustom.SetFocusModule (module, isFocus);
	}


}