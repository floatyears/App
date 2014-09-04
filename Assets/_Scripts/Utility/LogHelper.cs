using System;
using UnityEngine;

public sealed class LogHelper {
    private LogHelper(){

    }

	public static void Log(string format, params object[] args ){
		if (!DebugHelper.DEBUG){
			return;
		}
		Debug.Log("["+Utility.TimeHelper.FormattedTimeNow()+"] " +  string.Format(format,args));
	}
    
	public static void Log(object message ){
		if (!DebugHelper.DEBUG){
			return;
		}
		Debug.LogError("["+Utility.TimeHelper.FormattedTimeNow()+"] " + message);
	}

	public static void LogError(string format, params object[] args){
        if (!DebugHelper.DEBUG){
            return;
        }
		Debug.LogError("["+Utility.TimeHelper.FormattedTimeNow()+"]  " + string.Format(format,args));
    }

	public static void LogError(object message){
//		if (!DebugHelper.DEBUG){
//			return;
//		}
		Debug.LogError("["+Utility.TimeHelper.FormattedTimeNow()+"]  " +message);
	}

	public static void LogWarning(string format, params object[] args){
		if (!DebugHelper.DEBUG){
			return;
		}
		Debug.LogWarning("["+Utility.TimeHelper.FormattedTimeNow()+"]  " + string.Format(format,args));
	}

    public static void LogException(Exception exception){
        if (!DebugHelper.DEBUG){
            return;
        }
        LogHelper.Log(Utility.TimeHelper.FormattedTimeNow());
        LogHelper.LogException(exception);
    }
}
