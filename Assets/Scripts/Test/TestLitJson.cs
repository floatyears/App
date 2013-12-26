#if UNITY_EDITOR
using UnitTesting;
using UnityEngine;
using LitJson;


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

	[Test]
	public void TestJson()
	{
		string a = (Resources.Load ("Config/UnitInfo", typeof(TextAsset)) as TextAsset).text;
		LogHelper.LogError(a);
		JsonData jd = JsonMapper.ToObject (a);

		LogHelper.LogError(jd["id"]);
		LogHelper.LogError(jd["power"].Count);
		LogHelper.LogError(jd["power"][0]["attack"]);



//		Assert.AreEqual ();
		//Assert.AreEqual("ss", "1");
	}
}
#endif