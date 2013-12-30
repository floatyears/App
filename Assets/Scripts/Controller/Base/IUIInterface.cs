using UnityEngine;
#region old
public interface IUIInterface 
{
	/// <summary>
	/// ui name
	/// </summary>
	/// <value>The name.</value>

	string UIName{get;}
	
	/// <summary>
	/// creat ui object
	/// </summary>
	/// <param name="UIRoot">User interface root.</param>
	void CreatUI();

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

	UIState GetState{get;}
	
}
#endregion

#region new

public interface IUIComponent
{
	string UIName{ get;}

	void CreatUI();

	void ShowUI();

	void HideUI();

	void DestoryUI();
}

#endregion
public delegate void Callback();

public delegate void UICallback(GameObject caller);

public delegate void UICallback<T>(T arg1);

public delegate void DataListener(object data);

public delegate void HttpCallback(NetworkBase network);