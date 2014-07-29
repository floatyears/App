using UnityEngine;
using System.Collections;

public sealed class ServerConfig {

	public const string ServerHost = "";

	public const string ResourceHost = "";

	public const int AppVersion = 100; //1.0.0

	public const string touchToLogin = "点击进入";

	public const string Channel = 
#if UNITY_ANDROID
	"GooglePlay";
#elif UNITY_IOS
	"AppStore";
#else
	"Editor";
#endif

}
