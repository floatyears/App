#if UNITY_EDITOR
using UnitTesting;
using UnityEngine;
using LitJson;
using bbproto;

public class TestLitJson 
{
	[SetUp]
	public void HandleSetup()
	{
		//        dummyGameObject = new GameObject(objectName);
	}
	
	
	[TearDown]
	public void HandleTearDown()
	{
		//        GameObject.DestroyImmediate(dummyGameObject);
		
	}

//	[Test]
//	public void TestJson()
//	{
//		string a = (Resources.Load ("Config/UnitInfo", typeof(TextAssetu)) as TextAsset).text;
//		LogHelper.LogError(a);
//		JsonData jd = JsonMapper.ToObject (a);
//
//		LogHelper.LogError(jd["id"]);
//		LogHelper.LogError(jd["power"].Count);
//		LogHelper.LogError(jd["power"][1]["attack"]);
//
//
//
////		Assert.AreEqual ();
//		//Assert.AreEqual("ss", "1");
//	}

//	[Test]
//	public void TestModel()
//	{
//		UserUnit uu = new UserUnit ();
//		uu.id = 1;
//		uu.level = 2;
//		uu.limitbreakLv = 3;
//		uu.exp = 4;
//		uu.addAttack = 5;
//
//		Debug.LogError ("before id: " + uu.id);
//		Debug.LogError ("before level: " + uu.level);
//		Debug.LogError ("before limitbreakLv: " + uu.limitbreakLv);
//		Debug.LogError ("before exp: " + uu.exp);
//		Debug.LogError ("before addAttack: " + uu.addAttack);
//
//		UserUnitInfo uui = new UserUnitInfo (uu);
//
//		UserUnit uuh = uui.Load ();
//
//		Debug.LogError ("behind id: " + uuh.id);
//		Debug.LogError ("behind level: " + uuh.level);
//		Debug.LogError ("behind limitbreakLv: " + uuh.limitbreakLv);
//		Debug.LogError ("behind exp: " + uuh.exp);
//		Debug.LogError ("behind addAttack: " + uuh.addAttack);
//	}
}
#endif