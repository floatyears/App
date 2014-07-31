using UnityEngine;
using System.Collections;

public sealed class ServerConfig {

	public const string ServerHost = 
#if LANGUAGE_EN
		"http://us.yeedion.com:8080/";
#elif LANGUAGE_CN
		"http://61.153.100.131:8080";
		//"http://cn.yeedion.com:8080/";
#else
		"";
#endif



	public const string ResourceHost = "http://us.yeedion.com";

	public const int AppVersion = 100; //1.0.0

	public const string touchToLogin = "点击屏幕进入游戏";

	public const string Channel = 
#if UNITY_ANDROID
	"GooglePlay";
#elif UNITY_IOS
	"AppStore";
#else
	"Editor";
#endif

}
