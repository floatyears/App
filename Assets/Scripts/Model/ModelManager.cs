using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Data operation. Class inherit this interface can get data they need.
/// </summary>
public interface IDataOperation {

    /// <summary>
    /// Load this instance.
    /// </summary>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    T Load<T>();

    /// <summary>
    /// Save instance, record the specified errorMsg.
    /// </summary>
    /// </summary>
    /// <param name="instance">Instance.</param>
    /// <param name="errorMsg">Error message.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    void Save<T>(T instance, ErrorMsg errorMsg);

    ErrorMsg Validate<T>(T instance);
}

public class BaseModel : IDataOperation {
    private object data;
    
    public T Load<T>(){
        return (T)data;
    }
    
    public void Save<T>(T newData, ErrorMsg errorMsg){
        // validate
        errorMsg = Validate(newData);
        if (errorMsg.Code == ErrorCode.Succeed){

        }
    }

    public ErrorMsg Validate<T>(T instance){
        ErrorMsg eMsg = new ErrorMsg();
        eMsg.Code = ErrorCode.Succeed;
        return eMsg;
    }

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
            if(instance  == null)
            {
                instance = new ModelManager();
                instance.Init();
            }
            return instance;
        }
    }

    private Dictionary<string, IDataOperation> modelDic = new Dictionary<string, IDataOperation>();

    /// <summary>
    /// Init this instance.
    /// </summary>
    public void Init (){
//        modelDic.Add("user", Us)
    }
}