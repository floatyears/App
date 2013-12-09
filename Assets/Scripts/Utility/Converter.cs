using System;
using System.IO;
using UnityEngine;


/// <summary>
/// Converter. 	 for convert from one to other
/// </summary>
public class Converter {

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
		return buffer;
	}

	/// <summary>
	/// Strings to bytes.
	/// </summary>
	/// <returns>The to bytes.</returns>
	/// <param name="text">Text.</param>
	public static byte[] StringToBytes (string text){
		char[] chars = text.ToCharArray();
		byte[] buffer = new byte[chars.Length];
		Debug.Log("start convert");
		for(int i = 0; i < chars.Length; i++)
		{
			buffer[i] = Convert.ToByte(chars[i]);
		}
		return buffer;
	}

	/// <summary>
	/// Strings to stream.
	/// </summary>
	/// <returns>The to stream.</returns>
	/// <param name="text">Text.</param>
	public static MemoryStream StringToStream (string text){
		return new MemoryStream(Converter.StringToBytes(text));
	}
}
