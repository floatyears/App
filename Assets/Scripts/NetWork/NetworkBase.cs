using UnityEngine;
using System.Collections;

public class NetworkBase  {

	public static string baseUrl;

	public WWW wwwRequest;

	public HttpCallback httpCallback;

	public void ExcuteCallback()
	{
		if (httpCallback != null) {
			httpCallback(this);
		}
	}
}
