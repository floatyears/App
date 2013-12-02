using UnityEngine;
using System.Collections;

public interface IUIInterface 
{
	/// <summary>
	/// ui name
	/// </summary>
	/// <value>The name.</value>
	string name{set;get;}

	/// <summary>
	/// parent object
	/// </summary>
	GameObject UIRoot{set;}


	UIManager uiManager{set;}
	/// <summary>
	/// creat ui object
	/// </summary>
	/// <param name="UIRoot">User interface root.</param>
	void CreatUI(GameObject UIRoot);

	/// <summary>
	/// show ui object on screen
	/// </summary>
	void ShowUI();

	/// <summary>
	/// hide ui object
	/// </summary>
	void HideUI();

	/// <summary>
	/// destory ui object
	/// </summary>
	void DestoryUI();
	
}
