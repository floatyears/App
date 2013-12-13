using UnityEngine;
using System.Collections;


//using Fbmly1;
//using com.fbmly;
using System.IO;
using ProtoBuf;
using msg;
using System;

public class TempTest : MonoBehaviour
{

    public ErrorMsg receivePersonInfoSucceed (msg.Person modifiedPerson, ErrorMsg errorMsg, object[] values){

        // output strings
        LogHelper.Log("deserialized info: person's name is " + modifiedPerson.name);
        LogHelper.Log("deserialized info: person's id is " + modifiedPerson.id);
        LogHelper.Log("deserialized info: person's email is " + modifiedPerson.email);

        errorMsg = new ErrorMsg(ErrorCode.Succeed, "");
        return errorMsg;
    }

    public ErrorMsg receivePersonInfoFailed (string responseString, ErrorMsg errorMsg, object[] values){
        // post failed
        errorMsg = new ErrorMsg(ErrorCode.NetWork, "request failed :  Timed out ");
        LogHelper.Log(errorMsg);
        return errorMsg;
    }
	
	void OnGUI( )
	{
		
		if ( GUILayout.Button ( "Test" , GUILayout.Width ( 200f ) , GUILayout.Height ( 200f ) ) )
		{

//            // test protobuf
//			msg.Person person = new msg.Person();
//			person.name = "Rose Mary";
//			person.id = 20;
//			LogHelper.Log("person's name is " + person.name);
//			LogHelper.Log("person's id is " + person.id);
//			
////			StartCoroutine(POST("http://192.168.0.200:8000/get_quest_map", ProtobufSerializer.SerializeToBytes<msg.Person>(person)));
//            ErrorMsg errorMsg = new ErrorMsg();
//            HttpClient.Instance.sendPost<msg.Person>(this, "http://192.168.0.200:8000/get_quest_map", person, receivePersonInfoFailed, receivePersonInfoSucceed, errorMsg);


            // test sqlite
//            DbManager.Instance.
            string connectString = "DATA source=" + Application.dataPath + @"/test2.db";
//            string secret = "next";
            SqliteDbHelper dbHelper = new SqliteDbHelper(connectString);
            dbHelper.CreateTable("UserInfo", new string[]{"name"}, new string[]{"varchar(20)"});
//            dbHelper.CloseSqlConnection();
//            dbHelper.OpenDB(connectString, "");
            dbHelper.InsertInto("UserInfo", new string[]{"'Jackie'"});
            dbHelper.InsertInto("UserInfo", new string[]{"'Rose'"});
            dbHelper.CloseSqlConnection();

		}
	}
}
