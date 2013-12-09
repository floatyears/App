using UnityEngine;
using System.Collections.Generic;

public class Fight : UIBase
{
	private UICamera nguiMainCamera;

	private Camera mainCamera;

	private CardItem[] actorCard;

	private CardPoolSingleItem cardPSingleItem;

	private RaycastHit raycastHit;

	private GameObject parentObject;

	private GameObject singlePoolParent;

	private CardPoolSingleItem[] cardSingleArray;

	public Fight(string uiName) : base(uiName)
	{
		nguiMainCamera = ViewManager.Instance.MainUICamera;

		mainCamera = nguiMainCamera.GetComponent<Camera>();

		parentObject = NGUITools.AddChild(ViewManager.Instance.ParentPanel);

		parentObject.name = "Fight";

		singlePoolParent = NGUITools.AddChild(parentObject);

		GameInput.OnPressEvent += HandleOnPressEvent;

		GameInput.OnReleaseEvent += HandleOnReleaseEvent;

		GameInput.OnStationaryEvent += HandleOnStationaryEvent;

		GameInput.OnDragEvent += HandleOnDragEvent;
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


		cardSingleArray = new CardPoolSingleItem[Config.cardPoolSingle];

		for (int i = 0; i < cardSingleArray.Length; i++)
		{



		}

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

	void HandleOnDragEvent (Vector2 obj)
	{
		
	}
	
	void HandleOnStationaryEvent ()
	{
		
	}
	
	void HandleOnReleaseEvent ()
	{
		
	}
	
	void HandleOnPressEvent ()
	{
		
	}

	void Check(int layer)
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray,out raycastHit,layer))
		{

		}
	}
}
