using UnityEngine;
using System.Collections;

public sealed class ServerConfig {

	public const int AppVersion = 102; //1.0.0

	//PID_PREFIX : 每个App需设置不同名称
	public const string ProductIDPrefix = "monsterquest"; //ml : Monster Legend

	
#if INNER_TEST || UNITY_EDITOR
//测试服务器
	public const string ServerHost = "http://61.153.100.131:8080/";
	public static string ResourceHost = "http://61.153.100.131/resource/" + AppVersion;
#elif LANGUAGE_EN
//美国服务器
	public const string ServerHost = "http://us.yeedion.com:8080/";
	public static string ResourceHost = "http://us.yeedion.com/resource/" + AppVersion;
#elif LANGUAGE_CN
//中国服务器(ucloud)
	public const string ServerHost = "http://61.153.100.131:8080/";
	public static string ResourceHost = "http://61.153.100.131/resource/" + AppVersion;
#else
		"";
#endif
	



	public const string touchToLogin = "点击屏幕进入游戏";


#if UNITY_ANDROID
	#if INNER_TEST
	public const string Channel = "AndroidTest";
	public const string UmengAppKey = "53e7f407fd98c594da01b21f"; //AndroidTest
	#elif LANGUAGE_EN
	public const string Channel = "GooglePlay";
	public const string UmengAppKey = "53f59e2ffd98c5e80b045f1c"; //GooglePlay Umeng AppKey
	#else
	public const string Channel = "AndroidCN";
	public const string UmengAppKey = "53e7f407fd98c594da01b21f"; //AndroidTest
	#endif
#elif UNITY_IOS
	public const string Channel = "AppStore";
	public const string UmengAppKey = "539a56ce56240b8c1f074094"; //iOS
#else
	public const string Channel = "UnityEditor";
	public const string UmengAppKey = "53e7f407fd98c594da01b21f"; //AndroidTest
#endif

}
