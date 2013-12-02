using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager
{

	private static UIManager instance;

	/// <summary>
	/// singleton 
	/// </summary>
	/// <value>The instance.</value>
	public static UIManager Instance
	{
		get
		{
			if(instance  == null)
				instance = new UIManager();

			return instance;
		}
	}


	#region UI object manager
	private Dictionary<string,IUIInterface> uiDic = new Dictionary<string, IUIInterface>();

	/// <summary>
	/// add ui to uimanager
	/// </summary>
	/// <param name="uiName">ui name.</param>
	/// <param name="ui">ui object.</param>
	public void AddUIObject(string uiName,IUIInterface ui)
	{
		if(!uiDic.ContainsKey(uiName))
			uiDic.Add(uiName,ui);
		else
			uiDic[uiName] = ui;
	}

	/// <summary>
	/// have this ui object?
	/// </summary>
	/// <returns><c>true</c> if this instance has user interface object the specified uiName; otherwise, <c>false</c>.</returns>
	/// <param name="uiName">ui name.</param>
	public bool HasUIObject(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
			return true;

		return false;
	}

	/// <summary>
	/// get this ui object
	/// </summary>
	/// <returns>ui object.</returns>
	/// <param name="uiName">ui name.</param>
	public IUIInterface GetUI(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
			return uiDic[uiName];
		else
			return null;
	}

	/// <summary>
	/// Remove UI
	/// </summary>
	/// <param name="uiName">User interface name.</param>
	public void RemoveUI(string uiName)
	{
		if(uiDic.ContainsKey(uiName))
			uiDic.Remove(uiName);
	}
	#endregion


}
