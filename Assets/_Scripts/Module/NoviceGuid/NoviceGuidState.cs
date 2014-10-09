public class NoviceGuidState {

	//no need to hold the stepEntity instance. the stepEntity argument of funcs can do the same thing.
	//public T target;
	private bool jumpToNextState;

	public NoviceGuidState()
	{
		NoviceGuidFSMManager.Instance ().AddStepStateInstance (this);
	}

	public virtual void Enter(NoviceGuideStepEntity stepEntity)
	{

	}

	public virtual void Execute(NoviceGuideStepEntity stepEntity)
	{
		if (jumpToNextState) {
				
		}
	}

	public virtual void Exit(NoviceGuideStepEntity stepEntity)
	{

	}

	//jump to next state
	public bool JumpToNextState
	{
		set{jumpToNextState = value;}
		get{return jumpToNextState;}
	}
}
