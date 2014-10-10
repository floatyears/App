public class NoviceGuidStep {

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

	public System.Type NextState{
		get{
			return nextState;
		}
	}
}
