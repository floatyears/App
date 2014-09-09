// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public class ErrorCode {
    public const int SUCCESS = 0;

    public const int ERROR_BASE = -100;
    public const int  FAILED = -101;
    public const int INVALID_PARAMS = -102;
    public const int MARSHAL_ERROR = -103;
    public const int UNMARSHAL_ERROR = -104;
    public const int IOREAD_ERROR = -105;
    public const int IOWRITE_ERROR = -106;
    public const int CONNECT_DB_ERROR = -107;
    public const int READ_DB_ERROR = -108;
    public const int SET_DB_ERROR = -109;
    public const int DATA_NOT_EXISTS = -110;
        

    public const int EU_USER_BASE = -200;
    public const int EU_INVALID_USERID = -201;
    public const int EU_GET_USERINFO_FAIL = -202;
    public const int EU_USER_NOT_EXISTS = -203;
    public const int EU_GET_NEWUSERID_FAIL = -204;
    public const int EU_UPDATE_USERINFO_ERROR = -205;
	public const int EU_NO_ENOUGH_MONEY = -206;
	public const int EU_UNITMAX_REACHED = -207;

    public const int EF_FRIEND_BASE = -300;
    public const int EF_FRIEND_NOT_EXISTS = -301;
    public const int EF_GET_FRIENDINFO_FAIL = -302;
    public const int EF_ADD_FRIEND_FAIL = -303;
    public const int EF_DEL_FRIEND_FAIL = -304;
    public const int EF_IS_ALREADY_FRIEND = -305;
    public const int EF_IS_ALREADY_FRIENDOUT = -306;
    public const int EF_INVALID_FRIEND_STATE = -307;
	public const int EF_FRIENDNUM_OVERFLOW = -308;
	public const int EF_FRIEND_FRIENDNUM_OVERFLOW = -309;


    public const int EQ_QUEST_BASE = -400;
    public const int EQ_QUEST_ID_INVALID = -401;
    public const int EQ_GET_QUESTINFO_ERROR = -402;
    public const int EQ_STAMINA_NOT_ENOUGH = -403;
    public const int EQ_GET_QUEST_CONFIG_ERROR = -404;
    public const int EQ_GET_QUEST_LOG_ERROR = -405;
    public const int EQ_UPDATE_QUEST_RECORD_ERROR = -406;
    public const int EQ_INVALID_DROP_UNIT = -407;
    public const int EQ_QUEST_IS_PLAYING = -408;
	public const int EQ_USER_QUEST_NOT_PLAYING = -409;

    public const int E_UNIT_BASE = -500;
    public const int E_UNIT_ID_ERROR = -501;
    public const int E_LEVELUP_NO_ENOUGH_MONEY = -502;
    public const int E_GET_UNIT_INFO_ERROR = -503;
	public const int E_UNIT_CANNOT_EVOLVE_TODAY = E_UNIT_BASE - 4;
	public const int E_GACHA_TIME_EXPIRED		= E_UNIT_BASE - 5;
	public const int E_LOAD_GACHA_POOL_FAIL		= E_UNIT_BASE - 6;

    // usual
    public const int ILLEGAL_PARAM = -1000;

    // network
    public const int NETWORK = -2000;
    public const int TIMEOUT = -2001;
    public const int INVALID_SESSIONID = -2002;
    public const int CONNECT_ERROR = -2003;
    public const int SERVER_500 = -2004;

    // model
    public const int   MODEL = -3000;
    public const int ENCRYPT = -3001;
    public const int DECRYPT = - 3002; 
    public const int ILLEGAL_DATA = -3003;
    public const int INVALID_MODEL_NAME = -3004;

    // controller
    public const int CONTROLLER = -4000;

    // view
    public const int VIEW = -5000;
}


public class ErrorMsgCenter {
    private ErrorMsgCenter(){
        InitMsgDic();
    }
    private Dictionary<int, string> msgStringDic = new Dictionary<int, string>();

    public string GetErrorMsgText(int errorCode){
        string msg = "";
        msgStringDic.TryGetValue(errorCode, out msg);
		if (msg==null || msg.Equals(string.Empty)) {
			msg = string.Format("Error: {0}", errorCode);
		}

		UnityEngine.Debug.LogError(string.Format("GetErrorMsgText() code {1} msg {0}, msgStringCount {2}", msg, errorCode, msgStringDic.Count));
        return msg;
    }


    private static ErrorMsgCenter instance;
    public static ErrorMsgCenter Instance{
        get {
            if (instance == null){
                instance = new ErrorMsgCenter();
            }
            return instance;
        }
    }

    private void InitMsgDic() {
        msgStringDic.Add((int)ErrorCode.SUCCESS, TextCenter.GetText("success"));
        msgStringDic.Add((int)ErrorCode.ERROR_BASE, TextCenter.GetText("ERROR_BASE"));

		msgStringDic.Add((int)ErrorCode.NETWORK, TextCenter.GetText("NETWORK_ERROR"));
		msgStringDic.Add((int)ErrorCode.TIMEOUT, TextCenter.GetText("TIMEOUT_ERROR"));
		msgStringDic.Add((int)ErrorCode.CONNECT_ERROR, TextCenter.GetText("CONNECT_ERROR"));
		msgStringDic.Add((int)ErrorCode.SERVER_500, TextCenter.GetText("SERVER_500"));
        
        msgStringDic.Add((int)ErrorCode.FAILED, TextCenter.GetText("FAILED"));
        msgStringDic.Add((int)ErrorCode.INVALID_PARAMS, TextCenter.GetText("INVALID_PARAMS"));
        msgStringDic.Add((int)ErrorCode.MARSHAL_ERROR, TextCenter.GetText("MARSHAL_ERROR"));
        msgStringDic.Add((int)ErrorCode.UNMARSHAL_ERROR, TextCenter.GetText("UNMARSHAL_ERROR"));
        msgStringDic.Add((int)ErrorCode.IOREAD_ERROR, TextCenter.GetText("IOREAD_ERROR"));
        msgStringDic.Add((int)ErrorCode.IOWRITE_ERROR, TextCenter.GetText("IOWRITE_ERROR"));
        msgStringDic.Add((int)ErrorCode.CONNECT_DB_ERROR, TextCenter.GetText("CONNECT_DB_ERROR"));
        msgStringDic.Add((int)ErrorCode.READ_DB_ERROR, TextCenter.GetText("READ_DB_ERROR"));
        msgStringDic.Add((int)ErrorCode.SET_DB_ERROR, TextCenter.GetText("SET_DB_ERROR"));
        msgStringDic.Add((int)ErrorCode.DATA_NOT_EXISTS, TextCenter.GetText("DATA_NOT_EXISTS"));
        
        msgStringDic.Add((int)ErrorCode.EU_USER_BASE, TextCenter.GetText("EU_USER_BASE"));
        msgStringDic.Add((int)ErrorCode.EU_INVALID_USERID, TextCenter.GetText("EU_INVALID_USERID"));
        msgStringDic.Add((int)ErrorCode.EU_GET_USERINFO_FAIL, TextCenter.GetText("EU_GET_USERINFO_FAIL"));
        msgStringDic.Add((int)ErrorCode.EU_USER_NOT_EXISTS, TextCenter.GetText("EU_USER_NOT_EXISTS"));
        msgStringDic.Add((int)ErrorCode.EU_GET_NEWUSERID_FAIL, TextCenter.GetText("EU_GET_NEWUSERID_FAIL"));
        msgStringDic.Add((int)ErrorCode.EU_UPDATE_USERINFO_ERROR, TextCenter.GetText("EU_UPDATE_USERINFO_ERROR"));
        msgStringDic.Add((int)ErrorCode.EF_FRIEND_BASE, TextCenter.GetText("EF_FRIEND_BASE"));
        msgStringDic.Add((int)ErrorCode.EF_FRIEND_NOT_EXISTS, TextCenter.GetText("EF_FRIEND_NOT_EXISTS"));
		msgStringDic.Add((int)ErrorCode.EF_GET_FRIENDINFO_FAIL, TextCenter.GetText("EF_GET_FRIENDINFO_FAIL"));
		msgStringDic.Add((int)ErrorCode.EF_DEL_FRIEND_FAIL, TextCenter.GetText("EF_DEL_FRIEND_FAIL"));
        msgStringDic.Add((int)ErrorCode.EF_IS_ALREADY_FRIEND, TextCenter.GetText("EF_IS_ALREADY_FRIEND"));
        msgStringDic.Add((int)ErrorCode.EF_INVALID_FRIEND_STATE, TextCenter.GetText("EF_INVALID_FRIEND_STATE"));
        
        msgStringDic.Add((int)ErrorCode.EQ_QUEST_BASE, TextCenter.GetText("EQ_QUEST_BASE"));
        msgStringDic.Add((int)ErrorCode.EQ_QUEST_ID_INVALID, TextCenter.GetText("EQ_QUEST_ID_INVALID"));
        msgStringDic.Add((int)ErrorCode.EQ_GET_QUESTINFO_ERROR, TextCenter.GetText("EQ_GET_QUESTINFO_ERROR"));
        msgStringDic.Add((int)ErrorCode.EQ_STAMINA_NOT_ENOUGH, TextCenter.GetText("EQ_STAMINA_NOT_ENOUGH"));
        msgStringDic.Add((int)ErrorCode.EQ_GET_QUEST_CONFIG_ERROR, TextCenter.GetText("EQ_GET_QUEST_CONFIG_ERROR"));
        msgStringDic.Add((int)ErrorCode.EQ_GET_QUEST_LOG_ERROR, TextCenter.GetText("EQ_GET_QUEST_LOG_ERROR"));
        msgStringDic.Add((int)ErrorCode.EQ_UPDATE_QUEST_RECORD_ERROR, TextCenter.GetText("EQ_UPDATE_QUEST_RECORD_ERROR"));
        msgStringDic.Add((int)ErrorCode.EQ_INVALID_DROP_UNIT, TextCenter.GetText("EQ_INVALID_DROP_UNIT"));
        msgStringDic.Add((int)ErrorCode.EQ_QUEST_IS_PLAYING, TextCenter.GetText("EQ_QUEST_IS_PLAYING"));
        msgStringDic.Add((int)ErrorCode.E_UNIT_BASE, TextCenter.GetText("E_UNIT_BASE"));
        msgStringDic.Add((int)ErrorCode.E_UNIT_ID_ERROR, TextCenter.GetText("E_UNIT_ID_ERROR"));
        msgStringDic.Add((int)ErrorCode.E_LEVELUP_NO_ENOUGH_MONEY, TextCenter.GetText("E_LEVELUP_NO_ENOUGH_MONEY"));
        msgStringDic.Add((int)ErrorCode.E_GET_UNIT_INFO_ERROR, TextCenter.GetText("E_GET_UNIT_INFO_ERROR"));


    }

    public void OpenNetWorkErrorMsgWindow(int errorCode,DataListener callback = null){

//        MsgWindowParams msgWindowParams = new MsgWindowParams();

        ErrorMsg errMsg = new ErrorMsg(errorCode);
////        errMsg.Msg = string.Format(errMsg.Msg, args);
//        msgWindowParams.titleText = ;
//        msgWindowParams.contentText = ;
//
//        msgWindowParams.btnParam = new BtnParam();
//		msgWindowParams.btnParam.callback = callback;
//        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, msgWindowParams);

		TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("Error"),errMsg.Msg,TextCenter.GetText("OK"),callback);
    }
}

/// <summary>
///public const int Error message.
/// </summary>
public class ErrorMsg {

    private int code = (int)ErrorCode.SUCCESS;
    private string msg = "";

    public ErrorMsg() {
    }

    public ErrorMsg(int errorCode) {
        code = errorCode;
        msg = ErrorMsgCenter.Instance.GetErrorMsgText(errorCode);
    }

    public ErrorMsg(int errorCode, string message) {
        code = errorCode;
        msg = message;
    }
    /// <summary>
    /// The code.
    /// </summary>
    public int Code {
        get { return code; }
        set { code = value; }
    }
    
    /// <summary>
    /// The message.
    /// </summary>
    public string Msg {
        get { return msg; }
        set { msg = value; }
    }

    public static void OpenErrorMsgWindow(int code) {
        ErrorMsg errMsg = new ErrorMsg(code);
    }

 
}