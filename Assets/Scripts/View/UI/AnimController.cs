using UnityEngine;
using System.Collections;

public class AnimController : MonoBehaviour 
{
	public static void UpdateSceneInfo(string uiName)
	{
		TopUI.infoBar.animation.Play();
		TopUI.labVauleUIName.text = uiName;
	}

}
