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

public class TextCenter {

    public static TextCenter Instace {
        get {
            if (instance == null){
                instance = new TextCenter();
                instance.init();
            }
            return instance;
        }
    }

    public string GetCurrentText(string key){
        string result = "";
        textDict.TryGetValue(key, out result);
        return result;
    }

    public string GetCurrentText(string key, params object[] args){
        string result = "";
        textDict.TryGetValue(key, out result);
        result = string.Format(result, args);
        return result;
    }

    public void Test(){
        LogHelper.Log("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT TextHelper.Test() start");
        LogHelper.Log("test get string {0}, result {1}", "error", GetCurrentText("error"));
        LogHelper.Log("test get string {0}, result {1}", "error1", GetCurrentText("error1", "test error1"));
    }

    private static TextCenter instance;

    private Dictionary<string, string> textDict;

    private void init(){
        textDict = new Dictionary<string, string>();

        //
        textDict.Add("error", "error");
        textDict.Add("error1", "error {0}");
        textDict.Add("OK", "OK");
        textDict.Add("Cancel", "Cancel");

        textDict.Add("SearchError", "Search Error");
        textDict.Add("UserNotExist", "The Friend {0} you search not exist");

        textDict.Add("InputError", "Input Error");
        textDict.Add("InputEmpty", "Could not Input empty ID!");

//        textDict.Add("SearchError", "Input Error");
        textDict.Add("UserAlreadyFriend", "The ID {0} you searched is already your friend!");

        textDict.Add("RefuseAll", "The ID {0} you searched is already your friend!");
        textDict.Add("ConfirmRefuseAll", "Are you sure to refuse all friend apply?");

        textDict.Add("RefreshFriend", "Friend Update");
        textDict.Add("ConfirmRefreshFriend", "Are you sure to update friend list?");


    }
}