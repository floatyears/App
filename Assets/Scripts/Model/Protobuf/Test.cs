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
	public static byte[] ConvertStringToBytes (string str){
//		int bytesLength = str.Length - 2;
//		string[] strArray = str.Substring(1, str.Length - 2).Split(' ');
		char[] charArray = str.ToCharArray();
		byte[] ret = new byte[charArray.Length];
		Debug.Log("start convert");
		for(int i = 0; i < charArray.Length; i++)
		{
			Debug.Log(i + "now char is " + charArray[i]);
			ret[i] = Convert.ToByte(charArray[i]);
		}
		return ret;
	}

	IEnumerator POST(string url, Stream stream)
	{
		byte[] buffer = null;
		// output to bytes
		stream.Position = 0;
		int length = (int)stream.Length;
		buffer = new byte[length];
		stream.Read(buffer, 0, length);
		Debug.Log("send:" + buffer + ", length of bytes sended: " + length);
//		byte[] byteStream = System.Text.Encoding.Default.GetBytes(stream);
//		form.AddBinaryData("post", buffer);

//		form.AddField("post", System.Text.Encoding.Default.GetString(buffer));
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
			string rs = www.text;
			byte[] receivedBytes = System.Text.Encoding.Default.GetBytes(rs);
//			byte[] receivedBytes = Test.ConvertStringToHexBytes(rs);
			Debug.Log("received:" + receivedBytes + ", length of bytes sended: " + receivedBytes.Length);
			using ( MemoryStream m = new MemoryStream (receivedBytes) )

			{

				msg.Person modifiedPerson = Serializer.Deserialize<msg.Person>(m);

				Debug.Log("deserialized info: person's name is " + modifiedPerson.name);
				Debug.Log("deserialized info: person's id is " + modifiedPerson.id);
				Debug.Log("deserialized info: person's email is " + modifiedPerson.email);

			}
		}
	}
	
	void OnGUI( )
	{
		
		if ( GUILayout.Button ( "Test" , GUILayout.Width ( 200f ) , GUILayout.Height ( 200f ) ) )
		{

//			Fbmly person = new Fbmly ( );
			msg.Person person = new msg.Person();
			person.name = "Rose Mary";
			person.id = 20;
			Debug.Log("person's name is " + person.name);
			Debug.Log("person's id is " + person.id);
			

			using ( MemoryStream m = new MemoryStream ( ) )
			{
				Serializer.Serialize(m, person);


//				msg.Person deserializedPerson = Serializer.Deserialize<msg.Person>(m);
//				//Deserialize
//				Debug.Log("newPerson's name is " + deserializedPerson.name);
//				Debug.Log("newPerson's id is " + deserializedPerson.id);


				//Serialize
//				byte[] buffer = null;
				//				// output to bytes
//				m.Position = 0;
//				int length = (int)m.Length;
//				buffer = new byte[length];
//				m.Read(buffer, 0, length);
//				int read;
//				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
//				{
//					ms.Read(buffer, 0, read);
////				}
//				string outPut = System.Text.Encoding.Default.GetString ( buffer );
//				Debug.Log("output Person binary is " + outPut);


				// Test 
				StartCoroutine(POST("http://192.168.0.200:8000/get_quest_map", m));

			}
			
		}
		
		
	}
	
	
}
