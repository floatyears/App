using UnityEngine;
using System.Collections;

public class NetworkBase  {
	public WWW wwwRequest;
	public static string baseUrl;
	public HttpCallback httpCallback;

	public void ExcuteCallback() {
		if (httpCallback != null) {
			httpCallback(this);
		}
	}
}

public class HttpNetBase : IWWWPost {
	private WWW www;
	public WWW WwwInfo {
		get { return www; }
		set {www = value;}
	}
	private string currentUrl = "";
	public string Url {
		get { return HttpManager.baseUrl + currentUrl; }
		set { currentUrl = value; }
	}
	private int version;
	public int Version {
		get { return version; }
		set { version = value; }
	}

	private NetCallback callback;

	public void Send (ProtoManager nettemp,WWWForm wf)	{
		callback = nettemp.Receive;
		www = new WWW (Url, wf);
		HttpManager.Instance.SendHttpPost (this);
	}

	public void Send (ProtoManager nettemp, string urlPath, byte[] data) {
		callback = nettemp.Receive;
		Url = urlPath;
//		Debug.LogError ("send : " + Url);
		www = new WWW (Url, data);
		HttpManager.Instance.SendHttpPost (this);
	}
	
	public void ExcuteCallback () {
		if (callback != null) {
			callback(this);
		}
	}
}