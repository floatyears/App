using UnityEngine;
using System.Collections;

public class Fight : UIBase
{
	private UICamera nguiMainCamera;



	public Fight(string uiName) : base(uiName)
	{
		nguiMainCamera = ViewManager.Instance.MainUICamera;
	}

	/// <summary>
	/// close ngui input , open custom input
	/// </summary>
	/// <param name="isShield">If set to <c>true</c> is shield.</param>

	void SwitchInput(bool isShield)
	{
		nguiMainCamera.useMouse = isShield;
		nguiMainCamera.useKeyboard = isShield;
		nguiMainCamera.useTouch = isShield;

		main.GInput.IsCheckInput = !isShield;
	}

	public override void CreatUI ()
	{

	}

	public override void DestoryUI ()
	{

	}

	public override void HideUI ()
	{
		SwitchInput(true);
	}

	public override void ShowUI ()
	{
		SwitchInput(false);
	}
}
