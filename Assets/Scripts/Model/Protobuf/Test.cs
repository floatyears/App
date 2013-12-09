using UnityEngine;
using System.Collections;


//using Fbmly1;
//using com.fbmly;
using System.IO;
using ProtoBuf;
using msg;
using System;

public class Test : MonoBehaviour
{

    public ErrorMsg receivePersonInfoSucceed (msg.Person modifiedPerson, ErrorMsg errorMsg, object[] values){

        Debug.Log("deserialized info: person's name is " + modifiedPerson.name);
        Debug.Log("deserialized info: person's id is " + modifiedPerson.id);
        Debug.Log("deserialized info: person's email is " + modifiedPerson.email);

        errorMsg = new ErrorMsg(ErrorCode.Succeed, "");
        return errorMsg;
    }

    public ErrorMsg receivePersonInfoFailed (string responseString, ErrorMsg errorMsg, object[] values){
        // post failed
        errorMsg = new ErrorMsg(ErrorCode.NetWork, "request failed :  Timed out ");
        Debug.Log(errorMsg);
        return errorMsg;
    }
	
	void OnGUI( )
	{
		
		if ( GUILayout.Button ( "Test" , GUILayout.Width ( 200f ) , GUILayout.Height ( 200f ) ) )
		{

			msg.Person person = new msg.Person();
			person.name = "Rose Mary";
			person.id = 20;
			Debug.Log("person's name is " + person.name);
			Debug.Log("person's id is " + person.id);
			
//			StartCoroutine(POST("http://192.168.0.200:8000/get_quest_map", ProtobufSerializer.SerializeToBytes<msg.Person>(person)));
            ErrorMsg errorMsg = new ErrorMsg();
            HttpClient.Instance.sendPost<msg.Person>(this, "http://192.168.0.200:8000/get_quest_map", person, receivePersonInfoFailed, receivePersonInfoSucceed, errorMsg);
		}
	}
}
