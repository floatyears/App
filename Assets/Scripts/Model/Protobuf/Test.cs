using UnityEngine;
using System.Collections;


//using Fbmly1;
//using com.fbmly;
using System.IO;
using ProtoBuf;
using msg;

public class Test : MonoBehaviour
{
	
	void OnGUI( )
	{
		
		if ( GUILayout.Button ( "Test" , GUILayout.Width ( 200f ) , GUILayout.Height ( 200f ) ) )
		{

//			Fbmly person = new Fbmly ( );
			msg.Person person = new msg.Person();
			person.name = "Rose";
			person.id = 20;
			Debug.Log("person's name is " + person.name);
			
//			
//			ProtobufSerializer serializer = new ProtobufSerializer ( );
			
			//Serialize
			byte[] buffer = null;
			
			using ( MemoryStream m = new MemoryStream ( ) )
			{
				Serializer.Serialize(m, person);
				m.Position = 0;
				int length = (int)m.Length;
				buffer = new byte[length];
				m.Read(buffer, 0 ,length);
				
				msg.Person deserializedPerson = Serializer.Deserialize<msg.Person>(m);
				//Deserialize
				Debug.Log("newPerson's name is " + deserializedPerson.name);
				Debug.Log("newPerson's id is " + deserializedPerson.id);
			}
			
		}
		
		
	}
	
	
}
