using UnityEngine;
using System.Collections;

public class ImgBtnMessage : MonoBehaviour 
{
	public GameObject InfoBar;
	public GameObject info;
	private tk2dTextMesh textInfo;

	void Start()
	{
		textInfo = info.GetComponent<tk2dTextMesh>();
	}

	private IEnumerator SetScene(string sceneInfo)
	{
		InfoBar.animation.Play();
		yield return new WaitForSeconds(0.7F);
		textInfo.text = sceneInfo;
	}

	public void TurnToQuest()
	{
		StartCoroutine(SetScene("Quest"));
	}

	public void TurnToFriends()
	{
		StartCoroutine(SetScene("Friends"));
	}

	public void TurnToScratch()
	{
		StartCoroutine(SetScene("Scratch"));
	}

	public void TurnToOthers()
	{
		StartCoroutine(SetScene("Others"));
	}

	public void TurnToUnits()
	{
		StartCoroutine(SetScene("Units"));
	}

	public void TurnToShop()
	{
		StartCoroutine(SetScene("Shop"));
	}

}
