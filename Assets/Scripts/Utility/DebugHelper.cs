using System.Collections;

/// <summary>
/// Platform.
/// </summary>
public enum Platform {
    EDITOR = 1000,
    MOBILE,
}

public sealed class DebugHelper {

    /// <summary>
    /// The DEBUG. When development overed, set debug = false
    /// </summary>
    public static bool DEBUG = true;

    /// <summary>
    /// The platform. When play in mobile, set platform = mobile.
    /// </summary>
    public static Platform platform = Platform.EDITOR;
//    public static Platform platform = Platform.MOBILE;

    private DebugHelper(){
    }

    public static bool PlayInEditor(){
        return platform == Platform.EDITOR;
    }
}