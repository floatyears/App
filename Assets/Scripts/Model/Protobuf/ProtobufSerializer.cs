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
using System.IO;
using UnityEngine;
using ProtoBuf;


/// <summary>
/// Protobuf serializer. For convert
/// </summary>
public class ProtobufSerializer
{

    /// <summary>
    /// Serializes to bytes.
    /// </summary>
    /// <returns>The to bytes.</returns>
    /// <param name="instance">Instance.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
	public static byte[] SerializeToBytes<T> (T instance){
		using (MemoryStream ms = new MemoryStream ())
		{
            // validate
			if (instance == null){
                LogHelper.Log("try to serialize null instance, errorcode: " + ErrorCode.IllegalParam);
				return null;
			}
			Serializer.Serialize<T>(ms, instance);
			return ConvertHelper.StreamToBytes(ms);
		}
	}

    /// <summary>

    /// Parses the form bytes.
    /// </summary>
    /// <returns>The form bytes.</returns>
    /// <param name="source">Source.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T ParseFormBytes<T> (byte[] buffer){
        MemoryStream ms = ConvertHelper.BytesToStream(buffer);
        T retInstance = Serializer.Deserialize<T>(ms);
        
        // validate
        if (retInstance == null){
            LogHelper.Log("Deserialize instance failed, errorcode: " + ErrorCode.IllegalParam);
        }
        return retInstance;
    }
}


