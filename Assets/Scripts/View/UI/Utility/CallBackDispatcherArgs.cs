using System.Collections.Generic;

public class CallBackDispatcherArgs
{
	public string funcName;

	private object args;
	public object Args
	{
		get
		{
			return args;
		}
	}

	private Dictionary<string, object> argsDic;
	public Dictionary<string, object> ArgsDic
	{
		get
		{
			return argsDic;
		}
	}

	public CallBackDispatcherArgs(string funcName, object args)
	{
		this.funcName = funcName;
		this.args = args;
		this.InitArgsDic(args);
	}

	private void InitArgsDic(object args)
	{
		if (args is Dictionary<string, object>)
		{
			this.argsDic = args as Dictionary<string, object>;
		}
	}
}


public class CallBackDispatcherHelper
{
	public delegate void dispatchFunc(object args);
	public static void DispatchCallBack(dispatchFunc func, CallBackDispatcherArgs dispatcherArgs){
		Dictionary<string, object> argsDic = dispatcherArgs.ArgsDic;
		//LogHelper.LogError("CallBackDispatcherHelper(), funcName {0}", dispatcherArgs.funcName);
		if (argsDic == null){
			//LogHelper.Log(string.Format("CallBackDispatcherHelper.DispatchCallBack(), argsDic == null, args is {0}", dispatcherArgs.Args));
			func(dispatcherArgs.Args);
		} 
		else{
			//LogHelper.Log(string.Format("CallBackDispatcherHelper.DispatchCallBack(), argsDic == null, argsDic is {0}", argsDic));
			func(argsDic);
		}
	}
}