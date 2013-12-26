#if UNITY_EDITOR
using UnitTesting;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static test. Only for static functions
/// </summary>
public class StaticTest {

    [SetUp]
    public void HandleSetup()
    {
    }
    
    
    [TearDown]
    public void HandleTearDown()
    {

    }

    /// <summary>
    /// Tests the UUID. UUID should be unique
    /// </summary>
    [Test]
    public void TestUUID()
    {
        int TEST_COUNT = 100000;
        List <string> uuidList = new List<string>();
        for (int i = 0; i < TEST_COUNT; i++){
            uuidList.Add(GenerationHelper.NewUUID());
        }
        HashSet<string> uuidSet = new HashSet<string>(uuidList);

        int j = 0;
        foreach (string uuid in uuidSet){
            j++;
        }
        Assert.Approx(j, TEST_COUNT, 0);
    }
}

#endif
