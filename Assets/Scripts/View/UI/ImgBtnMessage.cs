using UnityEngine;
using System.Collections;

public class ImgBtnMessage : MonoBehaviour 
{
	public GameObject InfoBar;
	public GameObject infoLabel;
	private UILabel textInfo;

	void Start()
	{
		textInfo = infoLabel.GetComponent<UILabel>();
	}

	public IEnumerator SetScene(string sceneInfo)
	{
		InfoBar.animation.Play();
		yield return new WaitForSeconds(0.7F);
		textInfo.text = sceneInfo;
	}

	public void TurnToQuest()
	{
		StartCoroutine(SetScene("Quest"));
		ControllerManager.Instance.ChangeScene(SceneEnum.Quest);
	}

	public void TurnToFriends()
	{
		StartCoroutine(SetScene("Friends"));
		ControllerManager.Instance.ChangeScene(SceneEnum.Friends);
	}

	public void TurnToScratch()
	{
		StartCoroutine(SetScene("Scratch"));
		ControllerManager.Instance.ChangeScene(SceneEnum.Scratch);
	}

	public void TurnToOthers()
	{
		StartCoroutine(SetScene("Others"));
		ControllerManager.Instance.ChangeScene(SceneEnum.Others);
	}

	public void TurnToUnits()
	{
		StartCoroutine(SetScene("Units"));
		ControllerManager.Instance.ChangeScene(SceneEnum.Units);
	}

	public void TurnToShop()
	{
		StartCoroutine(SetScene("Shop"));
		ControllerManager.Instance.ChangeScene(SceneEnum.Shop);
	}

}
