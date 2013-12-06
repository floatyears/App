using UnityEngine;
using System.Collections;


//using Fbmly1;
//using com.fbmly;
using System.IO;
using ProtoBuf;
using msg;
using System;
using Utility;
using NetWork;

public class Test : MonoBehaviour
{

    public bool receivePersonInfoSucceed (msg.Person modifiedPerson ){

        Debug.Log("deserialized info: person's name is " + modifiedPerson.name);
        Debug.Log("deserialized info: person's id is " + modifiedPerson.id);
        Debug.Log("deserialized info: person's email is " + modifiedPerson.email);

        return true;
    }

    public bool receivePersonInfoFailed (string responseString){
        //POST请求成功
        Debug.Log("request failed :  ErrorCode: " + ErrorCode.TimeOut);
        return false;
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
            HttpClient client = new HttpClient();
            client.sendPost<msg.Person>(this, "http://192.168.0.200:8000/get_quest_map", person, receivePersonInfoFailed, receivePersonInfoSucceed);
		}
	}
}
