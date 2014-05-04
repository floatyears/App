using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoviceGuideUtil {

	private static List<GameObject> arrows = new List<GameObject>();

	public static void ShowArrow(GameObject[] parents,int[] direction){

		Vector3 dir;
		int i = 0,len = direction.Length;

		foreach (GameObject parent in parents) {




			GameObject obj = LoadAsset.Instance.LoadAssetFromResources("NoviceGuideArrow",ResourceEuum.Prefab) as GameObject;
//			GameObject arrow = GameObject.Instantiate(obj,new Vector3(pos.x,pos.y,0),dir) as GameObject;
			GameObject arrow = NGUITools.AddChild(parent, obj);
			TweenPosition tPos = arrow.GetComponent<TweenPosition>();

			switch(i < len ? direction[i] : 0)
			{
				//point to the top
			case 3:
				dir = new Vector3(0f,0f,180f);// = Quaternion.FromToRotation(new Vector3(1,0,0),Vector3.zero);
				tPos.to.y = -parent.GetComponent<BoxCollider>().size.y/2 -32;
				tPos.from.y = -parent.GetComponent<BoxCollider>().size.y/2-62.0f;
				break;
				//point to the right
			case 4:
				dir = new Vector3(0f,0f,90f);
				//					dir = Quaternion.FromToRotation(new Vector3(-1,0,0),Vector3.zero);
				tPos.to.x = -parent.GetComponent<BoxCollider>().size.x/2 - 32;
				tPos.from.x = -parent.GetComponent<BoxCollider>().size.x/2-62.0f;
				break;
				//point to the bottom
			case 1:
				//					dir = Quaternion.FromToRotation(new Vector3(0,1,0),Vector3.zero);
				dir = new Vector3(0f,0f,0f);
				tPos.to.y = parent.GetComponent<BoxCollider>().size.y/2 + 32;
				tPos.from.y = parent.GetComponent<BoxCollider>().size.y/2+62.0f;
				break;
			case 2:
				//point to the left
				//					dir = Quaternion.FromToRotation(new Vector3(0,-1,0),Vector3.zero);
				dir = new Vector3(0f,0f,270f);
				tPos.to.x = parent.GetComponent<BoxCollider>().size.x/2 + 32;
				tPos.from.x = parent.GetComponent<BoxCollider>().size.x/2+62.0f;
				break;
			default:
				dir = Vector3.zero;
				break;
			}

			arrow.transform.Rotate(dir);
			NGUITools.AdjustDepth(arrow,24);
			LogHelper.Log("novice guide arrow: "+obj+", pos x: "+ arrow.transform.position.x + " pos y: " + arrow.transform.position.y + "tPos: "+ tPos.from.x + ", " +tPos.from.y);
			arrows.Add(arrow);
			i++;
		}
	}

	public static void RemoveAllArrows(){
		while (arrows.Count > 0) {
			GameObject obj = arrows[0];
			arrows.Remove(obj);
			GameObject.Destroy(obj);
		}
	}

	public static void HideArrow()
	{

	}
}
