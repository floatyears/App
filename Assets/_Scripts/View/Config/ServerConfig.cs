using UnityEngine;
using System.Collections;

public sealed class ServerConfig {


#if LANGUAGE_EN
	public const string ServerHost = "http://us.yeedion.com:8080/";
	public const string ResourceHost = "http://us.yeedion.com";
#elif LANGUAGE_CN
	public const string ServerHost = "http://61.153.100.131:8080/";
	public const string ResourceHost = "http://61.153.100.131";
#else
		"";
#endif
	

	public const int AppVersion = 100; //1.0.0

	public const string touchToLogin = "点击屏幕进入游戏";

	public const string Channel = 
#if UNITY_ANDROID
	#if LANGUAGE_EN
	"GooglePlay";
	#else
	"AndroidCN";
	#endif
#elif UNITY_IOS
	"AppStore";
#else
	"Editor";
#endif

}
