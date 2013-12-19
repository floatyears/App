using bbproto;
using System.Collections;

public class User : BaseModel
{
    public override ErrorMsg Validate (byte[] byteData)
    {
        ErrorMsg errMsg = new ErrorMsg();
        //do validate
        UserInfo userInfo = Load();
        if (userInfo == null){
            errMsg.Code = ErrorCode.IllegalData;
        }
        else {
            // TODO: other validation
        }
        return errMsg;
    }

    public override void Init ()
    {
        base.Init ();
    }

    /// <summary>
    /// Load this instance. Each Model should complete this.
    /// </summary>
    public UserInfo Load(){
        return LoadProtobuf<UserInfo>();
    }
}