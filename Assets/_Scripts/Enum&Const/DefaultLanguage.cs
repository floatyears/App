using UnityEngine;
using System.Collections;

public class DefaultLanguage {
	
	public const string Version = 
		#if LANGUAGE_CN
		"版本：";
	#elif LANGUAGE_EN
	"Version:";
	#else
	"";
	#endif
	
	public const  string CurrentDownload = 
		#if LANGUAGE_CN
		"已下载：";
	#elif LANGUAGE_EN
	"Downloaded: ";
	#else
	"";
	#endif
	
	public const string TotalDownload = 
		#if LANGUAGE_CN
		"总大小：";
	#elif LANGUAGE_EN
	"Total: ";
	#else
	"";
	#endif
	
	public const string DownloadError = 
		#if LANGUAGE_EN
		"Download Error";
	#else
	"下载错误";
	#endif
	
	public const string DownloadErrorDescription = 
		#if LANGUAGE_EN
		"There is an error occured when dowloading resources, please connect to the Internet and Retry!";
	#else
	"下载资源过程中出现错误，请连接到服务器并重试！";
	#endif
	
	public const string Retry = 
		#if LANGUAGE_EN
		"Retry";
	#else
	"重试";
	#endif
	
	public const string OK = 
		#if LANGUAGE_CN
		"确定";
	#else
	"OK";
	#endif
	
	public const string FirstDownloadTips = 
		#if LANGUAGE_EN
		"It will take a few seconds to download for the first time, \nplease wait a moment";
	#else
	"首次进入游戏需要一定时间进行加载，请稍等.";
	#endif
	
}
