#if UNITY_EDITOR
using UnitTesting;
using UnityEngine;

public class TestSample
{
//    GameObject dummyGameObject;
//    const string objectName = "Dummy";
    
    
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
    public void testSub()
    {

        int result = 1 + 1;
        LogHelper.Log(result);
        Assert.Approx(result, 3, 0);
    }
}
#endif