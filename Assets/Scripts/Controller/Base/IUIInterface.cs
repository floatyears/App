using UnityEngine;
using System.Collections.Generic;

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

	SceneEnum GetScene{get;set;}
	
}
#endregion

//------------------------------------------------------------------------------------------------------------------------
// new code
//------------------------------------------------------------------------------------------------------------------------

/// <summary>
/// view interface
/// </summary>
public interface IUIComponentUnity {
	UIInsConfig UIConfig{ get;}
	void ShowUI();
	void HideUI();
	void DestoryUI();
}

/// <summary>
/// logic ui interface
/// </summary>
public interface IUIComponent :  IUIComponentUnity{

	void CreatUI();
}

public interface IUIOrigin {

}

/// <summary>
/// ui callback interface
/// </summary>
public interface IUICallback : IUIOrigin {
	void Callback(object data);
}

public interface IUISetBool : IUIOrigin {
	void SetEnable(bool b);
}

public delegate void Callback();

public delegate void UICallback(GameObject caller);

public delegate void UICallback<T>(T arg1);

public delegate void DataListener(object data);

public delegate void HttpCallback(NetworkBase network);
