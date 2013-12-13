using UnityEngine;
using System.Collections;

public class ScrollViewController : UIBaseUnity
{

	public Transform leftTranform;
	public Transform rightTranform;

	private GameObject itemElement;

	DragUI activityScrollView;

	void Start()
	{
		Init();
		activityScrollView.ShowData(2);
	}

	void Init()
	{
		activityScrollView = new DragUI( leftTranform, rightTranform, itemElement);
	}
}
