using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

public class ModelManager {
    private static ModelManager instance;
    
    /// <summary>
    /// singleton 
    /// </summary>
    /// <value>The instance.</value>
    public static ModelManager Instance {
        get {
            if (instance == null) {
                instance = new ModelManager();
            }
            return instance;
        }
    }

    

    //---------------------------------------------------------------------------------------------------------------//

    //---------------------------------------------------------------------------------------------------------------//

    
	
    /// <summary>
    /// Init this instance.
    /// </summary>
//    public void Init() {
////		Debug.LogWarning("model manager init");
////        InitConfigData();
//        
//		InitData();
//        //new all server protocol handler
////        InitNetworkHandler();
//    }

//    void InitNetworkHandler() {
////		AuthUser authUser = new AuthUser ();
////		RenameNick rename = new RenameNick ();
////		StartQuest startquest = new StartQuest ();
////		ClearQuest clearquest = new ClearQuest ();
////		ChangeParty changeparty = new ChangeParty();
////		LevelUp
//    }

    //init config data
    
}

