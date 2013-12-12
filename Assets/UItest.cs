using UnityEngine;
using System.Collections;

public class UItest : MonoBehaviour 
{
	public Camera m_camera;
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, 100))
			{
				Debug.Log(hit.transform.name);
			}
		}
	}
}
