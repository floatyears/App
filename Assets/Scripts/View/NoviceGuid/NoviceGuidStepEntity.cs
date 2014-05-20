using UnityEngine;
using System.Collections;

public class NoviceGuideStepEntity {

	protected NoviceGuidStateMachine stateMachine;

	protected NoviceGuideStepEntityID _id;

	private static ArrayList idList = new ArrayList();

	private NoviceGuidState startState;

	public NoviceGuideStepEntity(NoviceGuideStepEntityID stepID,NoviceGuidState sState, NoviceGuidState globalState)
	{
		ID = stepID;
		stateMachine = new NoviceGuidStateMachine (this);
		stateMachine.CurrentState = sState;
		stateMachine.GlobalState = globalState;
		this.startState = sState;
		NoviceGuideStepEntityManager.Instance ().AddStepEntity (this);
	}

	public NoviceGuideStepEntity(NoviceGuideStepEntityID stepID,NoviceGuidState sState)
	{
		ID = stepID;
		stateMachine = new NoviceGuidStateMachine (this);
		stateMachine.CurrentState = sState;
		stateMachine.GlobalState = null;
		this.startState = sState;
		NoviceGuideStepEntityManager.Instance ().AddStepEntity (this);
	}

	public NoviceGuideStepEntityID ID
	{
		get{ return _id;}
		set{ 
			if(idList.Contains(value)){
				LogHelper.LogError("this Novice Guide object has wrong id:"+value);
				return;
			}
			idList.Add(value);
			_id = value;
		}
	}

	public NoviceGuidState StartState {
		get{ return startState;}
		private set{startState = value;}
	}
	
	public void Update () {
		if (stateMachine != null && IsRunning) {
			stateMachine.SMUpate ();
		}
	}

	public bool IsRunning
	{
		get{return stateMachine.IsRunning;}
		set{stateMachine.IsRunning = value;}
	}
	
	public NoviceGuidStateMachine GetStateMachine()
	{
		return stateMachine;
	}
}
