using UnityEngine;
using System.Collections;


using System.IO;
using ProtoBuf;
using System;
using bbproto;

public class TempTest : MonoBehaviour
{

//    public ErrorMsg receivePersonInfoSucceed (msg.Person modifiedPerson, ErrorMsg errorMsg, object[] values){
//
//        // output strings
//        LogHelper.Log("deserialized info: person's name is " + modifiedPerson.name);
//        LogHelper.Log("deserialized info: person's id is " + modifiedPerson.id);
//        LogHelper.Log("deserialized info: person's email is " + modifiedPerson.email);
//
//        errorMsg = new ErrorMsg(ErrorCode.SUCCESS, "");
//        return errorMsg;
//    }

//    public ErrorMsg receivePersonInfoFailed (string responseString, ErrorMsg errorMsg, object[] values){
//        // post failed
//        errorMsg = new ErrorMsg(ErrorCode.NetWork, "request failed :  Timed out ");
//        LogHelper.Log(errorMsg);
//        return errorMsg;
//    }
	
	void OnGUI( )
	{
		
		if ( GUILayout.Button ( "Test" , GUILayout.Width ( 400f ) , GUILayout.Height ( 400f )) )
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


//            // test sqlite
////            DbManager.Instance.
//            string connectString = "DATA source=" + Application.dataPath + @"/test2.db";
////            string secret = "next";
//            SqliteDbHelper dbHelper = new SqliteDbHelper(connectString);
//            dbHelper.CreateTable("UserInfo", new string[]{"name"}, new string[]{"varchar(20)"});
////            dbHelper.CloseSqlConnection();
////            dbHelper.OpenDB(connectString, "");
//            dbHelper.InsertInto("UserInfo", new string[]{"'Jackie'"});
//            dbHelper.InsertInto("UserInfo", new string[]{"'Rose'"});
//            dbHelper.CloseSqlConnection();

            #region test protobufSerializer
//            UserInfo userInfo = new UserInfo();
//            userInfo.userId = 127;
//            userInfo.userName = "Rose Mary";
//            userInfo.exp = 20;
//            userInfo.rank = 20;
//            userInfo.staminaMax = 128;
//            userInfo.staminaNow = 127;
//            userInfo.staminaRecover = 127000000;
//            userInfo.loginTime = 127;
//
//            byte[] serUserInfo = ProtobufSerializer.SerializeToBytes<UserInfo>(userInfo);
//            
//            long start = TimeHelper.MillionSecondsNow();
//
//
//            LogHelper.Log("now time is " + start);
//            for (int i = 0; i < 10000; i++){
//                UserInfo info = ProtobufSerializer.ParseFormBytes<UserInfo>(serUserInfo);
//            }
//            UserInfo info = ProtobufSerializer.ParseFormBytes<UserInfo>(serUserInfo);
//
//            LogHelper.Log("stanimaNow " + info.staminaNow);
//            LogHelper.Log("stanimaMax " + info.staminaMax);
//            LogHelper.Log("stanimaRecover " + info.staminaRecover);
//            LogHelper.Log("userInfo.userName " + info.userName);
//
//            long end = TimeHelper.MillionSecondsNow();
//            LogHelper.Log("now time is " + end + " , cost " + (end - start));
            #endregion

            #region test reflection getProperty
//            UserInfo userInfo = new UserInfo();
//            userInfo.userId = 100;
//            Type t = userInfo.GetType();
//            LogHelper.Log("userId is: " + t.GetProperty("userId").GetValue(userInfo, null));

            #endregion

            #region test model->msgCenter
            //ModelManagerTest.Test();
            #endregion
		}
	}
}
