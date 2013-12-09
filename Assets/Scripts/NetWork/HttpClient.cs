// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;

public delegate object PostCallbackFailed(string responseStr, ErrorMsg errorMsg, params object[] values); 
public delegate object PostCallbackSucceed<T>(T instance, ErrorMsg errorMsg, params object[] values); 

public delegate object GetCallbackFailed(string responseStr, ErrorMsg errorMsg, params object[] values);
public delegate object GetCallbackSucceed(string responseStr, ErrorMsg errorMsg, params object[] values); 


/// <summary>
/// Http client.
/// </summary>
public class HttpClient
{

    private static HttpClient instance;
    public static HttpClient Instance
    {
        get
        {
            if(instance  == null)
                instance = new HttpClient();
            
            return instance;
        }
    }

	public HttpClient ()
	{
	}

    /// <summary>
    /// Position the specified url, buffer, failedFunc, succeedFunc and errorMsg.
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="buffer">Buffer.</param>
    /// <param name="failedFunc">Failed func.</param>
    /// <param name="succeedFunc">Succeed func.</param>
    /// <param name="errorMsg">Error message.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    IEnumerator POST<T>(string url, byte[] buffer, PostCallbackFailed failedFunc, PostCallbackSucceed<T> succeedFunc, ErrorMsg errorMsg, params object[] values)
    {

        Debug.Log("send:" + buffer + ", length of bytes sended: " + buffer.Length);
        WWW www = new WWW(url, buffer);
        yield return www;

        // when return
        if (www.error != null)
        {
            // POST request faild
            Debug.Log("error is :"+ www.error);
            failedFunc(www.error, errorMsg, values);
            // TODO: record error code
        } else
        {
            // POST request succeed
            Debug.Log("request ok : text is " + www.text);

            // deserilize
            T instance = ProtobufSerializer.ParseFormString<T>(www.text);
            // parse to current instance
            if (instance != null){
                succeedFunc(instance, errorMsg, values);
            }
        }
    }

    /// <summary>
    /// GE the specified url, failedFunc, succeedFunc and errorMsg.
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="failedFunc">Failed func.</param>
    /// <param name="succeedFunc">Succeed func.</param>
    /// <param name="errorMsg">Error message.</param>
    IEnumerator GET(string url, GetCallbackFailed failedFunc, GetCallbackSucceed succeedFunc, ErrorMsg errorMsg, params object[] values)
    {
        WWW www = new WWW(url);
        yield return www;

        // deal
        if (www.error != null)
        {
            // POST request faild
            Debug.Log("error is :"+ www.error);
            failedFunc(www.error, errorMsg, values);
            // TODO: record error code
        } else
        {
            // POST request succeed
            Debug.Log("request ok : text is " + www.text);
            succeedFunc(www.text, errorMsg, values);
        }
    }

    /// <summary>
    /// Sends the post.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="url">URL.</param>
    /// <param name="instance">Instance.</param>
    /// <param name="failedFunc">Failed func.</param>
    /// <param name="succeedFunc">Succeed func.</param>
    /// <param name="errorMsg">Error message.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public void sendPost<T>(MonoBehaviour sender, string url, T instance, PostCallbackFailed failedFunc, PostCallbackSucceed<T> succeedFunc, ErrorMsg errorMsg, params object[] values){

        // validate
        if (url == null || url == ""){
            Debug.Log("request url is" + url + ", error code is " + ErrorCode.IllegalParam);
            errorMsg.Code = ErrorCode.IllegalParam;
            errorMsg.Msg = "request url is null";
            return;
        }


        // validate func arguments
        else if (failedFunc == null || succeedFunc == null){
            errorMsg.Code = ErrorCode.IllegalParam;
            if (failedFunc == null ){
                errorMsg.Msg = "response failed callback is null, ErrorCode";
                Debug.Log("response failed callback is null, ErrorCode" + ErrorCode.IllegalParam);
            }
            else {
                errorMsg.Msg = "response succeed callback is null, ErrorCode";
                Debug.Log("response succeed callback is null, ErrorCode" + ErrorCode.IllegalParam);
            }
            return;
        }

        else {
            byte[] sendBytes = ProtobufSerializer.SerializeToBytes<T>(instance);
            if (sendBytes == null){
                errorMsg.Code = ErrorCode.IllegalParam;
                errorMsg.Msg = "Serializer get invalid instance";
                return;
            }
            sender.StartCoroutine(POST<T>(url, sendBytes, failedFunc, succeedFunc, errorMsg, values));
        }
    }

}
