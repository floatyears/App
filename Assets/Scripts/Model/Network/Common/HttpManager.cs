using UnityEngine;
using System.Collections.Generic;
using System;

public class HttpManager : INetSendPost {
    private static HttpManager instance;
    public static HttpManager Instance {
        get {
            if (instance == null) {
                instance = new HttpManager();
            }
            return instance;
        }
    }

    //public static string baseUrl = "http://192.168.0.108:6666/";
    public static string baseUrl = "http://107.170.243.127:6666/";

    private List<IWWWPost> wwwRequst = new List<IWWWPost>();

    private HttpManager() {
        GameInput.OnUpdate += HttpUpdate;
    }

    private string sessionId = "";
    /// <summary>
    /// network session id , init on game start, all network request must have this property
    /// </summary>
    public string SessionId {
        get { return sessionId; }
        set { sessionId = value; }
    }

    public ErrorMsg ValidateSessionId(object protobufModel) {
        ErrorMsg errMsg = new ErrorMsg();
        Type t = protobufModel.GetType();
        try {
            string protoSessionId = (string)t.GetProperty("sessionId").GetValue(protobufModel, null);
            if (protoSessionId != sessionId) {
                errMsg.Code = (int)ErrorCode.INVALID_SESSIONID;
            }
        }
        catch (Exception ex) {
            errMsg.Code = (int)ErrorCode.ILLEGAL_PARAM;
            errMsg.Msg = "request or response not has field sessionId";
        }
        return errMsg;
    }
	
    public void SendHttpPost(IWWWPost post) {
        if (post.WwwInfo != null) {
            wwwRequst.Add(post);	
        }
    }
	
    public void SendAssetPost(IWWWPost post) {
        post.WwwInfo = WWW.LoadFromCacheOrDownload(post.Url, post.Version);
        wwwRequst.Add(post);
    }

    void HttpUpdate() {
        int count = wwwRequst.Count;
        if (count == 0) {
            return;	
        }
        for (int i = wwwRequst.Count - 1; i >= 0; i--) {
            WWW www = wwwRequst[i].WwwInfo;
            if (www.isDone && string.IsNullOrEmpty(www.error)) {
                RequestDone(wwwRequst[i]);
            }
            else if (!string.IsNullOrEmpty(www.error)) {
                wwwRequst.RemoveAt(i);
            }
        }
    }

    void RequestDone(IWWWPost wwwPost) {
        wwwRequst.Remove(wwwPost);
        wwwPost.ExcuteCallback();
    }
}
