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

public enum ErrorCode {
    SUCCESS = 0,

    ERROR_BASE       = -100,
    FAILED           = -101,
    INVALID_PARAMS   = -102,
    MARSHAL_ERROR    = -103,
    UNMARSHAL_ERROR  = -104,
    IOREAD_ERROR     = -105,
    IOWRITE_ERROR    = -106,
    CONNECT_DB_ERROR = -107,
    READ_DB_ERROR    = -108,
    SET_DB_ERROR     = -109,
    DATA_NOT_EXISTS  = -110,


    EU_USER_BASE             = -200,
    EU_INVALID_USERID        = -201,
    EU_GET_USERINFO_FAIL     = -202,
    EU_USER_NOT_EXISTS       = -203,
    EU_GET_NEWUSERID_FAIL    = -204,
    EU_UPDATE_USERINFO_ERROR = -205,

    EF_FRIEND_BASE          = -300,
    EF_FRIEND_NOT_EXISTS    = -301,
    EF_GET_FRIENDINFO_FAIL  = -302,
    EF_ADD_FRIEND_FAIL      = -303,
    EF_DEL_FRIEND_FAIL      = -304,
    EF_IS_ALREADY_FRIEND    = -305,
    EF_IS_ALREADY_FRIENDOUT = -306,
    EF_INVALID_FRIEND_STATE = -307,

    EQ_QUEST_BASE                = -400,
    EQ_QUEST_ID_INVALID          = -401,
    EQ_GET_QUESTINFO_ERROR       = -402,
    EQ_STAMINA_NOT_ENOUGH        = -403,
    EQ_GET_QUEST_CONFIG_ERROR    = -404,
    EQ_GET_QUEST_LOG_ERROR       = -405,
    EQ_UPDATE_QUEST_RECORD_ERROR = -406,
    EQ_INVALID_DROP_UNIT         = -407,
    EQ_QUEST_IS_PLAYING          = -408,

    E_UNIT_BASE                 = -500,
    E_UNIT_ID_ERROR             = -501,
    E_LEVELUP_NO_ENOUGH_MONEY   = -502,
    E_GET_UNIT_INFO_ERROR       = -503,

    // usual
    ILLEGAL_PARAM = -1000,

    // network
    NETWORK = -2000,
    TIMEOUT = -2001,
    INVALID_SESSIONID = -2002,

    // model
    MODEL = -3000,
    ENCRYPT = -3001,
    DECRYPT = - 3002, 
    ILLEGAL_DATA = -3003,
    INVALID_MODEL_NAME = -3004,

    // controller
    CONTROLLER = -4000,

    // view
    VIEW = -5000,
};

public enum ErrorMsgType {
    SUCCEES = 0,
    // network error comes from 
    ERROR_BASE       = -100,
    FAILED           = -101,
    INVALID_PARAMS   = -102,
    MARSHAL_ERROR    = -103,
    UNMARSHAL_ERROR  = -104,
    IOREAD_ERROR     = -105,
    IOWRITE_ERROR    = -106,
    CONNECT_DB_ERROR = -107,
    READ_DB_ERROR    = -108,
    SET_DB_ERROR     = -109,
    DATA_NOT_EXISTS  = -110,
    
    
    EU_USER_BASE             = -200,
    EU_INVALID_USERID        = -201,
    EU_GET_USERINFO_FAIL     = -202,
    EU_USER_NOT_EXISTS       = -203,
    EU_GET_NEWUSERID_FAIL    = -204,
    EU_UPDATE_USERINFO_ERROR = -205,
    
    EF_FRIEND_BASE          = -300,
    EF_FRIEND_NOT_EXISTS    = -301,
    EF_GET_FRIENDINFO_FAIL  = -302,
    EF_ADD_FRIEND_FAIL      = -303,
    EF_DEL_FRIEND_FAIL      = -304,
    EF_IS_ALREADY_FRIEND    = -305,
    EF_IS_ALREADY_FRIENDOUT = -306,
    EF_INVALID_FRIEND_STATE = -307,
    
    EQ_QUEST_BASE                = -400,
    EQ_QUEST_ID_INVALID          = -401,
    EQ_GET_QUESTINFO_ERROR       = -402,
    EQ_STAMINA_NOT_ENOUGH        = -403,
    EQ_GET_QUEST_CONFIG_ERROR    = -404,
    EQ_GET_QUEST_LOG_ERROR       = -405,
    EQ_UPDATE_QUEST_RECORD_ERROR = -406,
    EQ_INVALID_DROP_UNIT         = -407,
    EQ_QUEST_IS_PLAYING          = -408,
    
    E_UNIT_BASE                 = -500,
    E_UNIT_ID_ERROR             = -501,
    E_LEVELUP_NO_ENOUGH_MONEY   = -502,
    E_GET_UNIT_INFO_ERROR       = -503,

    PARAM_VERIFY_FAILED = -1000,
    RSP_AUTHUSER_NULL = -1001,
}

/// <summary>
/// Error message.
/// </summary>
public class ErrorMsg {

    private static Dictionary<int, string> msgStringDic;
    private int code = (int)ErrorCode.SUCCESS;
    private string msg = "";

    public ErrorMsg() {
        initMsgDic();
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

    public static string GetErrorMsgInfo(ErrorCode code) {
        string msg = "";
        ErrorMsg.msgStringDic.TryGetValue((int)code, out msg);
        return msg;
    }

    public static string GetErrorMsgInfo(int code) {
        string msg = "";
        ErrorMsg.msgStringDic.TryGetValue(code, out msg);
        return msg;
    }

    public void SetErrorMsg(ErrorCode code) {
        Code = (int)code;
        Msg = GetErrorMsgInfo(code);
    }

    public void SetErrorMsg(int code) {
        Code = code;
        Msg = GetErrorMsgInfo(code);
    }

    private void initMsgDic() {
        if (ErrorMsg.msgStringDic != null) {
            return;
        }

        ErrorMsg.msgStringDic = new Dictionary<int, string>();
        ErrorMsg.msgStringDic.Add((int)ErrorCode.SUCCESS, "success");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.ERROR_BASE, "ERROR_BASE");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.FAILED, "FAILED");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.INVALID_PARAMS, "INVALID_PARAMS");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.MARSHAL_ERROR, "MARSHAL_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.UNMARSHAL_ERROR, "UNMARSHAL_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.IOREAD_ERROR, "IOREAD_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.IOWRITE_ERROR, "IOWRITE_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.CONNECT_DB_ERROR, "CONNECT_DB_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.READ_DB_ERROR, "READ_DB_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.SET_DB_ERROR, "SET_DB_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.DATA_NOT_EXISTS, "DATA_NOT_EXISTS");

        ErrorMsg.msgStringDic.Add((int)ErrorCode.EU_USER_BASE, "success");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EU_INVALID_USERID, "success");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EU_GET_USERINFO_FAIL, "success");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EU_USER_NOT_EXISTS, "EU_USER_NOT_EXISTS");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EU_GET_NEWUSERID_FAIL, "EU_GET_NEWUSERID_FAIL");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EU_UPDATE_USERINFO_ERROR, "EU_UPDATE_USERINFO_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EF_FRIEND_BASE, "EF_FRIEND_NOT_EXISTS");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EF_GET_FRIENDINFO_FAIL, "EF_ADD_FRIEND_FAIL");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EF_DEL_FRIEND_FAIL, "EF_DEL_FRIEND_FAIL");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EF_IS_ALREADY_FRIEND, "EF_IS_ALREADY_FRIEND");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EF_INVALID_FRIEND_STATE, "EF_INVALID_FRIEND_STATE");

        ErrorMsg.msgStringDic.Add((int)ErrorCode.EQ_QUEST_BASE, "EQ_QUEST_BASE");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EQ_QUEST_ID_INVALID, "EQ_QUEST_ID_INVALID");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EQ_GET_QUESTINFO_ERROR, "EQ_GET_QUESTINFO_ERROR");
        ErrorMsg.msgStringDic.Add((int)ErrorCode.EQ_STAMINA_NOT_ENOUGH, "EQ_STAMINA_NOT_ENOUGH");
//        ErrorMsg.msgStringDic.Add((int)ErrorCode.SUCCEES, "success");

    }
}