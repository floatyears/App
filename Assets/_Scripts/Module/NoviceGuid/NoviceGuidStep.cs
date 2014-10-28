public class NoviceGuidStep {

	protected System.Type nextState;

	private bool isExited = false;

	public NoviceGuidStep()
	{
		NoviceGuideStepManager.Instance.AddStep (this);
	}

	public virtual void Enter()
	{

	}
//
//	public virtual void Exit()
//	{
//
//	}

	//jump to next state
	protected void GoToNextState(bool isExecute = false)
	{
		NoviceGuideStepManager.Instance.ChangeState (nextState,isExecute);
	}

	public System.Type NextState{
		get{
			return nextState;
		}
	}
}
