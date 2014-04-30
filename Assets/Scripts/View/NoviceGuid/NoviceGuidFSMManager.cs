using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoviceGuidFSMManager {

	private static Dictionary<System.Type,NoviceGuidState> stepInsDic = new Dictionary<System.Type, NoviceGuidState>();

	private static NoviceGuidFSMManager instance;

	private NoviceGuidFSMManager(){

	}

	public static NoviceGuidFSMManager Instance()
	{
		if (instance == null)
			instance = new NoviceGuidFSMManager ();
		return instance;
	}

	public bool AddStepStateInstance(NoviceGuidState stepState)
	{

		if (stepInsDic.ContainsKey (stepState.GetType())) {
			LogHelper.LogError("there is already an " + stepState.GetType().ToString() + " in the dictionary,");
			return false;
		} else {
			stepInsDic.Add(stepState.GetType(),stepState);
			return true;
		}

		return false;
	}

//	public bool ClearAllState()
//	{
//		foreach (NoviceGuidState state in stepInsDic.Values) {
//			//state.
//		}
//	}
}
