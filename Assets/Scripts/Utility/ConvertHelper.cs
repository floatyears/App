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
    /// Byteses to string.
    /// </summary>
    /// <returns>The to string.</returns>
    /// <param name="source">Source.</param>
    public static string BytesToString(byte[] source){
        return System.Text.Encoding.UTF8.GetString(source);
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
