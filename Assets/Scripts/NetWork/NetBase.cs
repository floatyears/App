using UnityEngine;
using System;
using System.Collections;

public class NetBase {
	/// <summary>
	/// Universal Url
	/// </summary>
	public static string baseUrl = "";

	private static string sessionId = "";
	
	public static string SessionId {
		get { return sessionId; }
		set { sessionId = value; }
	}

	private WWW www;

	private HttpCallback callback;
	
	public NetBase(HttpCallback cb){
		callback = cb;
	}

	public ErrorMsg ValidateSessionId(object protobufModel){
		ErrorMsg errMsg = new ErrorMsg();
		Type t = protobufModel.GetType();
		try {
			string protoSessionId = (string)t.GetProperty("sessionId").GetValue(protobufModel, null);
			if (protoSessionId != sessionId){
				errMsg.Code = ErrorCode.InvalidSessionId;
			}
		} catch (Exception ex) {
			errMsg.Code = ErrorCode.IllegalParam;
			errMsg.Msg = "request or response not has field sessionId";
		}
		return errMsg;
	}

	public IEnumerator SendHttpRequest(string url, byte[] data = null)
	{
		string reallyUrl = baseUrl + url;	

		if (data == null) {
			www = new WWW (reallyUrl);
		}
		else {
			www = new WWW (reallyUrl, data);
		}		

		yield return www;

		if (!string.IsNullOrEmpty (www.error)) {
			LogHelper.Log (www.error);
			www = null;
		}
		else if(www.isDone) {
			LogHelper.Log ("url : " + url + " request is sucess.");
		}

		if (callback != null) {
			callback(www);
		}
	}


}
