using System;
using System.IO;
using UnityEngine;

namespace Utility {

	// for convert all need
	public class Converter {


		public static byte[] StreamToBytes (Stream source){
			byte[] buffer = null;
			// output to bytes
			source.Position = 0;
			int length = (int)source.Length;
			buffer = new byte[length];
			source.Read(buffer, 0, length);
			return buffer;
		}


		public static byte[] StringToBytes (string text){
			char[] chars = text.ToCharArray();
			byte[] buffer = new byte[chars.Length];
			Debug.Log("start convert");
			for(int i = 0; i < chars.Length; i++)
			{
				Debug.Log(i + "now char is " + chars[i]);
				buffer[i] = Convert.ToByte(chars[i]);
			}
			return buffer;
		}

		// 
		public static MemoryStream StringToStream (string text){
			return new MemoryStream(Converter.StringToBytes(text));
		}
	}
}
