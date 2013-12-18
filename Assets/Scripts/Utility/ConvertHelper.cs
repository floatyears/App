using System;
using System.IO;
using UnityEngine;


/// <summary>
/// Converter. 	 for convert from one to other
/// </summary>
public sealed class ConvertHelper {

    private ConvertHelper(){

    }

	/// <summary>
	/// Streams to bytes.
	/// </summary>
	/// <returns>The to bytes.</returns>
	/// <param name="source">Source.</param>
	public static byte[] StreamToBytes (Stream source){
		byte[] buffer = null;
		// output to bytes
		source.Position = 0;
		int length = (int)source.Length;
		buffer = new byte[length];
		source.Read(buffer, 0, length);
//        source.Seek(0, SeekOrigin.Begin);
		return buffer;
	}

	/// <summary>
	/// Strings to bytes.
	/// </summary>
	/// <returns>The to bytes.</returns>
	/// <param name="text">Text.</param>
	public static byte[] StringToBytes (string text){
//		char[] chars = text.ToCharArray();
//		byte[] buffer = new byte[chars.Length];
////		LogHelper.Log("start convert");
//		for(int i = 0; i < chars.Length; i++)
//		{
//			buffer[i] = Convert.ToByte(chars[i]);
//		}
        LogHelper.Log("source : " + text);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes( text );
        string des = BytesToString(buffer);
        LogHelper.Log("des : " + des);
        LogHelper.Log("source == des: " + (des == text).ToString() );
		return buffer;
	}

    /// <summary>
    /// Byteses to string.
    /// </summary>
    /// <returns>The to string.</returns>
    /// <param name="source">Source.</param>
    public static string BytesToString(byte[] source){
        return System.Text.Encoding.UTF8.GetString ( source );
    }

	/// <summary>
	/// Strings to stream.
	/// </summary>
	/// <returns>The to stream.</returns>
	/// <param name="text">Text.</param>
	public static MemoryStream StringToStream (string text){
		return new MemoryStream(ConvertHelper.StringToBytes(text));
	}

    /// <summary>
    /// Byteses to stream.
    /// </summary>
    /// <returns>The to stream.</returns>
    /// <param name="buffer">Buffer.</param>
    public static MemoryStream BytesToStream (byte[] buffer){
        return new MemoryStream(buffer);
    }
}
