using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BlockerReason{
	MessageWindow,
	BriefInfoWindow,
	SortWindow,
	Connecting
}

public class TouchEventBlocker{
	public const int blockerLayer = 15;
	public const string blockerLayerName = "Blocker";
    public const int defaultLayer = 0;
    public const string defaultLayerName = "Default";
	public const int guideLayer = 16;
	public const string guideLayerName = "NoviceGuide";

	private TouchEventBlocker(){
		nguiCamera = Camera.main.GetComponent<UICamera>();
	}
	private int originLayer = 1;

    private bool isBlocked;
	
	private UICamera nguiCamera ;

	private Dictionary<BlockerReason, bool> stateDic = new Dictionary<BlockerReason, bool>();

	private static TouchEventBlocker instance;

	public static TouchEventBlocker Instance{
		get{ 
			if(instance == null){
				instance = new TouchEventBlocker();
			}
			return instance;
		}
	}

    public bool IsBlocked {
        get {return isBlocked;}
    }

	public void SetState(BlockerReason reason, bool isBlocked){
        this.isBlocked = GetFinalState(reason, isBlocked);
		SetBlocked(this.isBlocked);
	}

	private void RecordState(BlockerReason reason, bool isBlocked){
		if(stateDic.ContainsKey(reason)){
			stateDic[reason] = isBlocked;
		}
		else{
			stateDic.Add(reason, isBlocked);
		}
	}

	private bool GetFinalState(BlockerReason reason, bool isBlocked){
		RecordState(reason, isBlocked);
		return CalculateFinalState(isBlocked);
	}

	private bool CalculateFinalState(bool isBlocked){
		bool result = isBlocked;
		
		if(isBlocked){
			result = true;
		}
		else{
			foreach (var item in stateDic) {
				if(item.Value){
					result = true;
					break;
				}
			}
		}

//		Test(result);
		return result;
	}
	
	private void SetBlocked(bool isBlocked){
		//Debug.LogError("TouchEventBlocker.SetBlocked(), isBlocked " + isBlocked);
		if (isBlocked){	
			if(nguiCamera.eventReceiverMask != LayerMask.NameToLayer(blockerLayerName) << blockerLayer){
				originLayer = nguiCamera.eventReceiverMask;
			}
			nguiCamera.eventReceiverMask = LayerMask.NameToLayer(blockerLayerName) << blockerLayer;

//			Debug.LogError("TouchEventBlocker.SetBlocked(), when true, eventReceiverMask " + (int)nguiCamera.eventReceiverMask);

		}
		else{
			nguiCamera.eventReceiverMask = originLayer;
//			Debug.LogError("TouchEventBlocker.SetBlocked(), when false, eventReceiverMask " + (int)nguiCamera.eventReceiverMask);

		}
	}

	private void Test(bool result){
		//Test
		Debug.LogError("CalculateFinalState.Test(), result is : " + result);
		foreach (var item in stateDic){
			Debug.LogError(string.Format("Test, Key is {0}, Value is {1}", item.Key, item.Value));
		}
	}

}