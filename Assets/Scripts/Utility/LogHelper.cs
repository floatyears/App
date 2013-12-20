using System;
using UnityEngine;

public sealed class LogHelper {
    private LogHelper(){

    }

    public static void Log(object message){
        if (DebugHelper.DEBUG){
            Debug.Log(TimeHelper.FormattedTimeNow() + " " + message.ToString());
        }
    }

    public static void LogError(object message){
        if (DebugHelper.DEBUG){
            Debug.LogError(TimeHelper.FormattedTimeNow() + " " + message.ToString());
        }
    }

    public static void LogError(object message, object content){
        if (DebugHelper.DEBUG){
            Debug.LogError(TimeHelper.FormattedTimeNow() + " " + message.ToString());
        }
    }

    public static void LogWarning(object message){
        if (DebugHelper.DEBUG){
            Debug.LogWarning(TimeHelper.FormattedTimeNow() + " " + message.ToString());
        }
    }

    public static void LogWarning(object message, object content){
        if (DebugHelper.DEBUG){
            Debug.LogWarning(TimeHelper.FormattedTimeNow() + " " + message.ToString());
        }
    }

    public static void LogException(Exception exception){
        if (DebugHelper.DEBUG){
            LogHelper.Log(TimeHelper.FormattedTimeNow());
            LogHelper.LogException(exception);
        }
    }
}
