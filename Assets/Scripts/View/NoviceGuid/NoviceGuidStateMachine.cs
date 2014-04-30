using UnityEngine;
using System.Collections;

public class NoviceGuidStateMachine{

	private NoviceGuideStepEntity stepEntity;

	private NoviceGuidState currentState;

	private NoviceGuidState prevState;

	private NoviceGuidState globalState;

	private bool isRunning;

	public NoviceGuidStateMachine(NoviceGuideStepEntity stepOwner)
	{
		stepEntity = stepOwner;
		currentState = null;
		prevState = null;
		globalState = null;
	}

	public NoviceGuidState GlobalState
	{
		set{
			globalState = value;
			if(globalState != null)
				globalState.Enter(stepEntity);
		}
		get{
			return globalState;
		}

	}

	public NoviceGuidState CurrentState
	{
		set{
			currentState = value;
			currentState.Enter (stepEntity);
		}

		get{
			return currentState;
		}
	}

	public bool IsRunning
	{
		get{return isRunning;}
	}

	public void ChangeState(NoviceGuidState nextState)
	{
		prevState = currentState;
		prevState.Exit (stepEntity);
		if (nextState == null) {
			LogHelper.LogWarning("Novice Guide's step is null.The State machine will stop.");
			currentState = null;
			isRunning = false;
			return;
		}
		currentState = nextState;
		currentState.Enter (stepEntity);
	}

	public void SMUpate()
	{
		if (currentState != null)
			currentState.Execute (stepEntity);
		if (globalState != null)
			globalState.Execute (stepEntity);
	}
}
