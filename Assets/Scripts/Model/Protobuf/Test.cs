using UnityEngine;
using System.Collections;


//using Fbmly1;
//using com.fbmly;
using System.IO;
using ProtoBuf;
using msg;
using System;
using Utility;

public class Test : MonoBehaviour
{

	IEnumerator POST(string url, byte[] buffer)
	{
		Debug.Log("send:" + buffer + ", length of bytes sended: " + buffer.Length);
		WWW www = new WWW(url, buffer);
		yield return www;
		if (www.error != null)
			{
			//POST请求失败
			Debug.Log("error is :"+ www.error);
						
		} else
			{
			//POST请求成功
			Debug.Log("request ok : received data " + www.text);
			msg.Person modifiedPerson = ProtobufSerializer.ParseFormString<msg.Person>(www.text);

			Debug.Log("deserialized info: person's name is " + modifiedPerson.name);
			Debug.Log("deserialized info: person's id is " + modifiedPerson.id);
			Debug.Log("deserialized info: person's email is " + modifiedPerson.email);

		}
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
			
			StartCoroutine(POST("http://192.168.0.200:8000/get_quest_map", ProtobufSerializer.SerializeToBytes<msg.Person>(person)));

		}
		
		
	}
	
	
}
