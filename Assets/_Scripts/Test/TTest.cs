//using UnityEngine;
//using System.Collections.Generic;
//
//public class Proxy<T> {
//	public string Name;
//
//	public T Model;
//
//	public Proxy(string name, T model) {
//		this.Name = name;
//		this.Model = model;
//	}
//}
//
//public class LoginData {
//	public string userName;
//
//	public int password;
//}
//
//public class LoginProxy : Proxy<LoginData> {
//	public LoginProxy (string proxyName, LoginData ld) : base (proxyName, ld) {
//		DataMana<LoginData>.dic.Add (proxyName, this);
//	}
//}
//
//public class DescribeData {
//	public string describe;
//
//	public int des;
//}
//
//
//public class DescribeProxy : Proxy<DescribeData> {
//	public DescribeProxy (string proxyName, DescribeData dd) : base (proxyName, dd) {
//		DataMana<DescribeData>.dic.Add (proxyName, this);
//	}
//}
//
//public class DataMana<T> {
//	public Dictionary< string, Proxy<T>> dic = new Dictionary< string, Proxy<T>> ();
//
//
//
// }