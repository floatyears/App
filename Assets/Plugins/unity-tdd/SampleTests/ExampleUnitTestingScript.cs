#if UNITY_EDITOR
using UnitTesting;
using UnityEngine;


public class ExampleUnitTestingScript
{
	GameObject dummyGameObject;
	const string objectName = "Dummy";


	[SetUp]
	public void HandleSetup()
	{
		dummyGameObject = new GameObject(objectName);
	}


	[TearDown]
	public void HandleTearDown()
	{
		GameObject.DestroyImmediate(dummyGameObject);
	}


	[Test]
	public void DummyGameObjectNameShouldBeDummy()
	{
		Assert.AreEqual(objectName, dummyGameObject.name);
	}


	[Test]
	public void SlightlyOverPointFiveOfZeroToOneLerpShouldBeApproxPointFive()
	{
		float result = Mathf.Lerp(0.0f, 1.0f, 0.505f);

		// If you change the following 0.01f to 0, you're test should fail
		Assert.Approx(0.5f, result, 0.01f);
	}
}
#endif