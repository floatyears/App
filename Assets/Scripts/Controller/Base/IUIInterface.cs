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
public interface IUIBaseComponent {
	UIInsConfig uiConfig{ get;}
	void ShowUI();
	void HideUI();
	void DestoryUI();
}

public interface IUIComponentUnity :  IUIBaseComponent{
	void Init(UIInsConfig config, IUIOrigin originInterface);
}

/// <summary>
/// logic ui interface
/// </summary>
public interface IUIComponent :  IUIBaseComponent{

	void CreatUI();
}

public interface IUIOrigin {

}

public interface IUISort {
	int Ascending(object first,object second);
	int Descending(object first, object second);
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


public interface IUIAnimation
{

}


public delegate void Callback();

public delegate void UICallback(GameObject caller);

public delegate void UICallback<T>(T arg1);

public delegate void DataListener(object data);

public delegate void HttpCallback(NetworkBase network);
