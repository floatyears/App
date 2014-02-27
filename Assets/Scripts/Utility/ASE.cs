using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public class ASE {
	public static byte[] AESKey = UTF8Encoding.UTF8.GetBytes("1234567890123456789012");
	public static byte[] AESEncrypt(string plainText , string strKey )  
	{  
		//分组加密算法  
		SymmetricAlgorithm des = Rijndael .Create () ;                
		byte[] inputByteArray =Encoding .UTF8  .GetBytes (plainText ) ;//得到需要加密的字节数组      
		//设置密钥及密钥向量  
		des.Key =Encoding.UTF8.GetBytes (strKey );  
		des.IV = AESKey ;              
		MemoryStream ms = new MemoryStream();  
		CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);  
		cs.Write(inputByteArray, 0, inputByteArray.Length);  
		cs.FlushFinalBlock();  
		byte[] cipherBytes = ms .ToArray () ;//得到加密后的字节数组  
		cs.Close();  
		ms.Close();  
		return cipherBytes ;           
	}  

}
