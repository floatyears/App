using UnityEngine;
using System.Collections;

/// <summary>
/// drag item must 
/// </summary>
public class DragBaseModel {

	private int id;

	/// <summary>
	/// id must have. it will use to load texture and call back data and so on.
	/// </summary>
	/// <value>The I.</value>
	public int ID
	{
		get{
			return id;
		}
		set{
			id = value;
		}
	}

	public GameObject item;

	public UICallback callbak;

}

public class DragLy : DragBaseModel
{

}