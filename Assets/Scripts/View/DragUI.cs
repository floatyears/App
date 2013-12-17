using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragUI : UIBase
{
	public static Object DragUIPrefabs;

	public List<GameObject> DragList
	{
		get{return dragList;}
	}

	private static GameObject sourceObject;

	private string dragUIObjectPath = "Prefabs/DragUI";
	
	private static int YPosition = 1000;

	private GameObject itemObject;

	private UIViewport dragUIViewport;

	public UIDraggableCamera dragCamera;

	private GameObject parentContainer;
	
	public Camera camera;
	
	private Vector3 initPosition = Vector3.zero;

	private List<GameObject> dragList = new List<GameObject>();
	
	public DragUI(Transform left,Transform right,GameObject itemObject = null) : base("DragUI")
	{
		if(left == null || right == null)
			return;

		this.itemObject = itemObject;

		if(sourceObject == null)
		{
			sourceObject = Resources.Load(dragUIObjectPath) as GameObject;
		}

		insUIObject = GameObject.Instantiate(sourceObject) as GameObject;

		Transform viewCameraTrans = insUIObject.transform.Find("ViewCamera");

		dragUIViewport = viewCameraTrans.GetComponent<UIViewport>();

		dragUIViewport.topLeft = left;

		dragUIViewport.bottomRight = right;

		dragCamera = viewCameraTrans.GetComponent<UIDraggableCamera>();

		camera = viewCameraTrans.GetComponent<Camera>();

		parentContainer = insUIObject.transform.Find("Anchor/Offset").gameObject;

		Camera cam = ViewManager.Instance.MainUICamera.GetComponent<Camera>();

		dragUIViewport.sourceCamera = cam;

		insUIObject.transform.Find("Anchor").GetComponent<UIAnchor>().uiCamera = cam;

		initPosition = insUIObject.transform.Find("Anchor/Offset/InitPosition").localPosition;

		SetPosition(camera.transform);

		SetPosition(parentContainer.transform);

		YPosition += 1000;
	}

	void SetPosition(Transform trans)
	{
		trans.localPosition = new Vector3(trans.localPosition.x,trans.localPosition.y + YPosition , trans.localPosition.z);
	}

	
	public void ShowData(int number = 1)
	{
		if(itemObject == null)
		{
			LogHelper.LogError("scroll view source object is null !  return ...");
			return;
		}

		for (int i = 0; i < number; i++) 
		{
			GameObject ins = NGUITools.AddChild(parentContainer,itemObject);

			ins.name = i.ToString();

			ins.SetActive(true);

			ins.AddComponent<UIDragCamera>().draggableCamera = dragCamera;

			dragList.Add(ins);
		}
	}
}
