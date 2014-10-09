public class NoviceGuidStep {

	//no need to hold the stepEntity instance. the stepEntity argument of funcs can do the same thing.
	//public T target;

	protected System.Type nextState;

	public NoviceGuidStep()
	{
		NoviceGuideStepManager.Instance.AddStep (this);
	}

	public virtual void Enter()
	{

	}

	public virtual void Exit()
	{

	}

	//jump to next state
	protected void GoToNextState()
	{
		NoviceGuideStepManager.Instance.ChangeState (nextState);
	}
}
