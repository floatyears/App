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
    public void TestSub()
    {

        int subA = 1;
        int subB = 1;
        int result = subA + subB;
        LogHelper.Log(subA + " + " + subB + " = " + result);
        Assert.Approx(result, 2, 0);
    }
}
#endif