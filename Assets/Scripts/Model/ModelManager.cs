using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

public enum ModelEnum
{
    User = 1000,
}

public class ModelManager
{
    private static ModelManager instance;
    
    /// <summary>
    /// singleton 
    /// </summary>
    /// <value>The instance.</value>
    public static ModelManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ModelManager();
            }
            return instance;
        }
    }

    private Dictionary<ModelEnum, BaseModel> modelDic = new Dictionary<ModelEnum, BaseModel>();

    // TODO
    /// <summary>
    /// Init this instance.
    /// </summary>
    public void Init (){
        // init all instance be used for game.
    }

    public void Add (ModelEnum modelType, BaseModel model){
        modelDic.Add(modelType, model);
    }

    /// <summary>
    /// Gets the data.
    /// usage: ModelManager.Instance.GetaData<User>
    /// </summary>
    /// <returns>The data.</returns>
    /// <param name="key">Key.</param>
    /// <param name="errorMsg">Error message.</param>
    public BaseModel GetData (ModelEnum modelType, ErrorMsg errorMsg) {

        BaseModel model = null;

        if (!modelDic.TryGetValue(modelType, out model)){
            errorMsg.Code = ErrorCode.InvalidModelName;
            errorMsg.Msg = String.Format("required key {0}, but it not exist in ModelManager", modelType);
        }
        return model;
    }
}

